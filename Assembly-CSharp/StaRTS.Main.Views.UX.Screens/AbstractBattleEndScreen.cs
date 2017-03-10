using StaRTS.Assets;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public abstract class AbstractBattleEndScreen : ScreenBase
	{
		private const string BUTTON_REPLAY_BATTLE = "ButtonReplayBattle";

		private const string PRIMARY_ACTION_BUTTON_LABEL = "LabelPrimaryAction";

		public const string PRIMARY_ACTION_BUTTON = "ButtonPrimaryAction";

		private const string STAR_ANIM_DEFAULT = "StarAnim{0}";

		private const string STAR_ANIM_EMPIRE = "StarAnim{0}_Empire";

		private const string STAR_ANIM_REBEL = "StarAnim{0}_Rebel";

		protected const float STAR_ANIM_DELAY = 1f;

		protected const float STAR_ANIM_DURATION = 0.35f;

		protected const string ANIM_TRIGGER = "Show";

		protected const int MAX_STARS = 3;

		protected const int STAR_DAMAGE = 50;

		protected const string LANG_CONTINUE = "CONTINUE";

		protected const string LANG_EXPENDED = "EXPENDED";

		protected const string LANG_PERCENTAGE = "PERCENTAGE";

		protected UXButton primaryActionButton;

		protected UXButton replayBattleButton;

		protected UXGrid troopGrid;

		protected List<uint> viewTimers;

		protected List<Animator> viewAnimators;

		protected bool isNonAttackerReplayView;

		protected bool isReplay;

		private FactionType faction;

		private CameraShake cameraShake;

		private bool setup;

		private bool destroyed;

		private TroopTooltipHelper troopTooltipHelper;

		private AssetHandle starsHandle;

		private GameObject starsObject;

		protected int numAssetsPendingLoad;

		protected override bool AllowGarbageCollection
		{
			get
			{
				return false;
			}
		}

		protected override bool WantTransitions
		{
			get
			{
				return !base.IsClosing;
			}
		}

		protected abstract string StarsPlaceHolderName
		{
			get;
		}

		protected abstract string ReplayPrefix
		{
			get;
		}

		protected abstract string TroopCardName
		{
			get;
		}

		protected abstract string TroopCardDefaultName
		{
			get;
		}

		protected abstract string TroopCardQualityName
		{
			get;
		}

		protected abstract string TroopCardIconName
		{
			get;
		}

		protected abstract string TroopCardAmountName
		{
			get;
		}

		protected abstract string TroopCardLevelName
		{
			get;
		}

		protected abstract string TroopHeroDecalName
		{
			get;
		}

		public AbstractBattleEndScreen(string assetName, bool isReplay) : base(assetName)
		{
			this.isReplay = isReplay;
			this.isNonAttackerReplayView = (isReplay && Service.Get<CurrentPlayer>().PlayerId != Service.Get<BattleController>().GetCurrentBattle().Attacker.PlayerId);
			this.setup = false;
			this.destroyed = false;
			this.viewTimers = new List<uint>();
			this.viewAnimators = new List<Animator>();
			this.faction = Service.Get<CurrentPlayer>().Faction;
			this.cameraShake = new CameraShake(null);
			this.troopTooltipHelper = new TroopTooltipHelper();
			this.numAssetsPendingLoad++;
			this.starsHandle = AssetHandle.Invalid;
			Service.Get<AssetManager>().Load(ref this.starsHandle, "gui_battle_stars", new AssetSuccessDelegate(this.OnStarsLoadSuccess), new AssetFailureDelegate(this.OnAssetLoadFail), null);
		}

		public override void OnDestroyElement()
		{
			int i = 0;
			int count = this.viewAnimators.Count;
			while (i < count)
			{
				Animator animator = this.viewAnimators[i];
				animator.enabled = false;
				i++;
			}
			this.viewAnimators.Clear();
			int j = 0;
			int count2 = this.viewTimers.Count;
			while (j < count2)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.viewTimers[j]);
				j++;
			}
			this.viewTimers.Clear();
			this.troopTooltipHelper.Destroy();
			this.troopTooltipHelper = null;
			if (this.starsHandle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.starsHandle);
				this.starsHandle = AssetHandle.Invalid;
			}
			if (this.starsObject != null)
			{
				UnityEngine.Object.Destroy(this.starsObject);
				this.starsObject = null;
			}
			if (this.troopGrid != null)
			{
				this.troopGrid.Clear();
				this.troopGrid = null;
			}
			this.numAssetsPendingLoad = 0;
			this.primaryActionButton.Enabled = true;
			this.setup = false;
			this.destroyed = true;
			this.cameraShake = null;
			base.OnDestroyElement();
		}

		protected override void OnScreenLoaded()
		{
			this.TryInit();
		}

		public override void SetupRootCollider()
		{
		}

		public override void RefreshView()
		{
			this.TryInit();
		}

		protected abstract void InitElements();

		protected abstract void SetupView();

		private void TryInit()
		{
			if (!base.IsLoaded() || this.destroyed || this.numAssetsPendingLoad > 0)
			{
				if (this.root != null && this.root.activeSelf)
				{
					this.root.SetActive(false);
				}
				return;
			}
			if (!this.setup)
			{
				this.setup = true;
				this.Init();
			}
		}

		private void Init()
		{
			this.root.SetActive(true);
			this.InitElements();
			this.InitButtons();
			this.InitLabels();
			this.SetupView();
			base.SetupRootCollider();
		}

		private void OnStarsLoadSuccess(object asset, object cookie)
		{
			this.OnAssetLoadSuccess(asset, this.StarsPlaceHolderName, null, ref this.starsObject);
		}

		protected void OnAssetLoadSuccess(object asset, string placeholderName, string overrideGameObjectName, ref GameObject gameObject)
		{
			gameObject = Service.Get<AssetManager>().CloneGameObject(asset as GameObject);
			if (overrideGameObjectName != null)
			{
				gameObject.name = overrideGameObjectName;
			}
			Transform transform = gameObject.transform;
			transform.parent = base.GetElement<UXElement>(placeholderName).Root.transform;
			transform.localPosition = Vector3.zero;
			transform.localScale = Vector3.one;
			base.CreateElements(gameObject);
			this.numAssetsPendingLoad--;
			this.TryInit();
		}

		protected void OnAssetLoadFail(object cookie)
		{
			this.numAssetsPendingLoad--;
			this.TryInit();
		}

		protected void InitStars()
		{
			bool flag = this.faction == FactionType.Empire;
			bool flag2 = this.faction == FactionType.Rebel;
			bool visible = !flag && !flag2;
			string text = this.isNonAttackerReplayView ? (this.ReplayPrefix + "StarAnim{0}") : "StarAnim{0}";
			string text2 = this.isNonAttackerReplayView ? (this.ReplayPrefix + "StarAnim{0}_Empire") : "StarAnim{0}_Empire";
			string text3 = this.isNonAttackerReplayView ? (this.ReplayPrefix + "StarAnim{0}_Rebel") : "StarAnim{0}_Rebel";
			for (int i = 1; i <= 3; i++)
			{
				base.GetElement<UXElement>(string.Format(text, new object[]
				{
					i
				})).Visible = visible;
				base.GetElement<UXElement>(string.Format(text2, new object[]
				{
					i
				})).Visible = flag;
				base.GetElement<UXElement>(string.Format(text3, new object[]
				{
					i
				})).Visible = flag2;
			}
		}

		protected virtual void InitButtons()
		{
			this.primaryActionButton = base.GetElement<UXButton>("ButtonPrimaryAction");
			this.primaryActionButton.OnClicked = new UXButtonClickedDelegate(this.OnPrimaryActionButtonClicked);
			base.CurrentBackButton = this.primaryActionButton;
			base.CurrentBackDelegate = new UXButtonClickedDelegate(this.OnPrimaryActionButtonClicked);
			base.AllowFUEBackButton = true;
			this.replayBattleButton = base.GetElement<UXButton>("ButtonReplayBattle");
			this.replayBattleButton.OnClicked = new UXButtonClickedDelegate(this.OnReplayBattleButtonClicked);
		}

		protected virtual void InitLabels()
		{
			base.GetElement<UXLabel>("LabelPrimaryAction").Text = this.lang.Get("CONTINUE", new object[0]);
		}

		protected void InitTroopGrid(string troopGridName, string troopTemplateName, BattleEntry battleEntry)
		{
			this.troopGrid = base.GetElement<UXGrid>(troopGridName);
			this.troopGrid.SetTemplateItem(troopTemplateName);
			if (this.isNonAttackerReplayView)
			{
				battleEntry.SetupExpendedTroops();
			}
			BattleDeploymentData attackerDeployedData = battleEntry.AttackerDeployedData;
			Dictionary<string, int> dictionary = null;
			if (attackerDeployedData.SquadData != null)
			{
				dictionary = new Dictionary<string, int>(attackerDeployedData.SquadData);
			}
			this.PopulateExpendedTroops(attackerDeployedData.TroopData, dictionary, false, battleEntry);
			this.PopulateExpendedTroops(dictionary, null, false, battleEntry);
			this.PopulateExpendedTroops(attackerDeployedData.SpecialAttackData, null, true, battleEntry);
			this.PopulateExpendedTroops(attackerDeployedData.HeroData, null, false, battleEntry);
			this.PopulateExpendedTroops(attackerDeployedData.ChampionData, null, false, battleEntry);
			this.troopGrid.RepositionItems();
			this.troopGrid.Scroll(0.5f);
		}

		private void PopulateExpendedTroops(Dictionary<string, int> troops, Dictionary<string, int> squadTroops, bool isSpecialAttack, BattleEntry battle)
		{
			if (troops == null)
			{
				return;
			}
			IDataController dataController = Service.Get<IDataController>();
			foreach (KeyValuePair<string, int> current in troops)
			{
				if (current.get_Value() >= 1)
				{
					int num = 0;
					if (squadTroops != null && squadTroops.ContainsKey(current.get_Key()))
					{
						num = squadTroops[current.get_Key()];
						squadTroops.Remove(current.get_Key());
					}
					IDeployableVO arg_82_0;
					if (!isSpecialAttack)
					{
						IDeployableVO deployableVO = dataController.Get<TroopTypeVO>(current.get_Key());
						arg_82_0 = deployableVO;
					}
					else
					{
						IDeployableVO deployableVO = dataController.Get<SpecialAttackTypeVO>(current.get_Key());
						arg_82_0 = deployableVO;
					}
					IDeployableVO deployableVO2 = arg_82_0;
					UXElement item = this.CreateDeployableUXElement(this.troopGrid, current.get_Key(), deployableVO2.AssetName, current.get_Value() + num, deployableVO2, battle);
					this.troopGrid.AddItem(item, deployableVO2.Order);
				}
			}
		}

		private UXElement CreateDeployableUXElement(UXGrid grid, string uid, string assetName, int amount, IDeployableVO troop, BattleEntry battle)
		{
			UXElement result = grid.CloneTemplateItem(uid);
			UXSprite subElement = grid.GetSubElement<UXSprite>(uid, this.TroopCardIconName);
			ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(troop, subElement);
			Service.Get<EventManager>().SendEvent(EventId.ButtonCreated, new GeometryTag(troop, projectorConfig, battle));
			projectorConfig.AnimPreference = AnimationPreference.NoAnimation;
			ProjectorUtils.GenerateProjector(projectorConfig);
			UXLabel subElement2 = grid.GetSubElement<UXLabel>(uid, this.TroopCardAmountName);
			subElement2.Text = LangUtils.GetMultiplierText(amount);
			UXLabel subElement3 = grid.GetSubElement<UXLabel>(uid, this.TroopCardLevelName);
			subElement3.Text = LangUtils.GetLevelText(troop.Lvl);
			FactionDecal.UpdateDeployableDecal(uid, grid, troop, this.TroopHeroDecalName);
			int upgradeQualityForDeployable = Service.Get<DeployableShardUnlockController>().GetUpgradeQualityForDeployable(troop);
			UXUtils.SetCardQuality(this, this.troopGrid, uid, upgradeQualityForDeployable, this.TroopCardQualityName, this.TroopCardDefaultName);
			UXButton subElement4 = grid.GetSubElement<UXButton>(uid, this.TroopCardName);
			this.troopTooltipHelper.RegisterButtonTooltip(subElement4, troop, battle);
			return result;
		}

		protected void AnimateStars(int stars)
		{
			for (int i = 1; i <= stars; i++)
			{
				FactionType factionType = this.faction;
				string text;
				if (factionType != FactionType.Empire)
				{
					if (factionType != FactionType.Rebel)
					{
						text = (this.isNonAttackerReplayView ? (this.ReplayPrefix + "StarAnim{0}") : "StarAnim{0}");
					}
					else
					{
						text = (this.isNonAttackerReplayView ? (this.ReplayPrefix + "StarAnim{0}_Rebel") : "StarAnim{0}_Rebel");
					}
				}
				else
				{
					text = (this.isNonAttackerReplayView ? (this.ReplayPrefix + "StarAnim{0}_Empire") : "StarAnim{0}_Empire");
				}
				UXElement element = base.GetElement<UXElement>(string.Format(text, new object[]
				{
					i
				}));
				Animator component = element.Root.GetComponent<Animator>();
				if (component == null)
				{
					Service.Get<StaRTSLogger>().WarnFormat("Unable to play end star anim #{0}", new object[]
					{
						i
					});
				}
				else
				{
					this.viewAnimators.Add(component);
					KeyValuePair<int, Animator> keyValuePair = new KeyValuePair<int, Animator>(i, component);
					uint item = Service.Get<ViewTimerManager>().CreateViewTimer((float)(i - 1) * 1f, false, new TimerDelegate(this.OnAnimateStarTimer), keyValuePair);
					this.viewTimers.Add(item);
				}
			}
			uint item2 = Service.Get<ViewTimerManager>().CreateViewTimer((float)(stars - 1) * 1f + 0.35f, false, new TimerDelegate(this.OnAllStarsComplete), null);
			this.viewTimers.Add(item2);
		}

		private void OnAnimateStarTimer(uint id, object cookie)
		{
			KeyValuePair<int, Animator> keyValuePair = (KeyValuePair<int, Animator>)cookie;
			Animator value = keyValuePair.get_Value();
			value.enabled = true;
			value.SetTrigger("Show");
			uint item = Service.Get<ViewTimerManager>().CreateViewTimer(0.35f, false, new TimerDelegate(this.OnStarAnimationComplete), null);
			this.viewTimers.Add(item);
			int key = keyValuePair.get_Key();
			Service.Get<EventManager>().SendEvent(EventId.BattleEndVictoryStarDisplayed, key);
		}

		private void OnStarAnimationComplete(uint id, object cookie)
		{
			if (this.cameraShake != null)
			{
				this.cameraShake.Shake(0.5f, 0.75f);
			}
		}

		protected virtual void OnAllStarsComplete(uint id, object cookie)
		{
		}

		private void OnPrimaryActionButtonClicked(UXButton button)
		{
			button.Enabled = false;
			this.replayBattleButton.Enabled = false;
			BattleType type = Service.Get<BattleController>().GetCurrentBattle().Type;
			if (type == BattleType.PveDefend)
			{
				HomeState.GoToHomeStateAndReloadMap();
				this.Close(null);
			}
			else
			{
				HomeState.GoToHomeState(null, false);
			}
			Service.Get<BattlePlaybackController>().DiscardLastReplay();
		}

		private void OnReplayBattleButtonClicked(UXButton button)
		{
			button.Enabled = false;
			this.primaryActionButton.Enabled = false;
			Service.Get<EventManager>().SendEvent(EventId.BattleReplayRequested, null);
		}

		protected internal AbstractBattleEndScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).AnimateStars(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).CreateDeployableUXElement((UXGrid)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(int*)(args + 3), (IDeployableVO)GCHandledObjects.GCHandleToObject(args[4]), (BattleEntry)GCHandledObjects.GCHandleToObject(args[5])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).AllowGarbageCollection);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).ReplayPrefix);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).StarsPlaceHolderName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).TroopCardAmountName);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).TroopCardDefaultName);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).TroopCardIconName);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).TroopCardLevelName);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).TroopCardName);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).TroopCardQualityName);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).TroopHeroDecalName);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).WantTransitions);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).InitElements();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).InitStars();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).InitTroopGrid(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (BattleEntry)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).OnAssetLoadFail(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).OnPrimaryActionButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).OnReplayBattleButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).OnStarsLoadSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).PopulateExpendedTroops((Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, (BattleEntry)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).SetupRootCollider();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).SetupView();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((AbstractBattleEndScreen)GCHandledObjects.GCHandleToObject(instance)).TryInit();
			return -1L;
		}
	}
}
