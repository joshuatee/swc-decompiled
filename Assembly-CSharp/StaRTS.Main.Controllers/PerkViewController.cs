using Net.RichardLord.Ash.Core;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.UX.Screens.Squads;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class PerkViewController : IEventObserver
	{
		private const string LANG_PERK_TITLE_PREFIX = "perk_title_";

		private const string LANG_PERK_DESC_PREFIX = "perk_desc_";

		private const string LANG_PERK_UPGRADE_LVL_REQ = "PERK_UPGRADE_LVL_REQ";

		private const string GENERATOR_TYPE = "generator";

		private const string CONTRACT_COST_TYPE = "contractCost";

		private const string CONTRACT_TIME_TYPE = "contractTime";

		private const string RELOCATION_TYPE = "relocation";

		private const string TRP_REQUEST_AMT_TYPE = "troopRequestAmount";

		private const string TRP_REQUEST_TIME_TYPE = "troopRequestTime";

		private const string PERK_CANCEL_POPUP_TITLE = "PERK_CANCEL_POPUP_TITLE";

		private const string PERK_CANCEL_POPUP_DESC = "PERK_CANCEL_POPUP_DESC";

		private uint perkBuildingHighlightTimerID;

		public PerkViewController()
		{
			Service.Set<PerkViewController>(this);
			this.perkBuildingHighlightTimerID = 0u;
			this.RegisterEvents();
		}

		private void RegisterEvents()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.BuildingConstructed, EventPriority.AfterDefault);
			eventManager.RegisterObserver(this, EventId.BuildingLevelUpgraded, EventPriority.AfterDefault);
			eventManager.RegisterObserver(this, EventId.WorldLoadComplete);
			eventManager.RegisterObserver(this, EventId.ActivePerksUpdated);
			eventManager.RegisterObserver(this, EventId.GameStateChanged);
		}

		private void RefreshPerkBuildingHighlightTimer()
		{
			if (this.perkBuildingHighlightTimerID != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.perkBuildingHighlightTimerID);
			}
			PerkManager perkManager = Service.Get<PerkManager>();
			List<Entity> buildingsForActivePerks = perkManager.GetBuildingsForActivePerks();
			uint num = 4294967295u;
			Entity entity = null;
			int i = 0;
			int count = buildingsForActivePerks.Count;
			while (i < count)
			{
				Entity entity2 = buildingsForActivePerks[i];
				uint maxActivationTimeRemaining = perkManager.GetMaxActivationTimeRemaining(entity2);
				if (maxActivationTimeRemaining > 0u && maxActivationTimeRemaining < num)
				{
					num = maxActivationTimeRemaining;
					entity = entity2;
				}
				i++;
			}
			if (entity != null)
			{
				this.perkBuildingHighlightTimerID = Service.Get<ViewTimerManager>().CreateViewTimer(num, false, new TimerDelegate(this.BuildingActivePerkEndTimer), entity);
			}
		}

		private void BuildingActivePerkEndTimer(uint timerId, object cookie)
		{
			if (cookie != null)
			{
				Entity building = cookie as Entity;
				Service.Get<BuildingController>().UpdateBuildingHighlightForPerks(building);
				this.RefreshPerkBuildingHighlightTimer();
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.BuildingConstructed)
			{
				if (id == EventId.BuildingLevelUpgraded || id == EventId.BuildingConstructed)
				{
					ContractEventData contractEventData = cookie as ContractEventData;
					Entity entity = (cookie as ContractEventData).Entity;
					BuildingComponent buildingComp = ((SmartEntity)entity).BuildingComp;
					if (entity != null && buildingComp != null && contractEventData.BuildingVO != null && Service.Get<PerkManager>().IsPerkAppliedToBuilding(contractEventData.BuildingVO))
					{
						Service.Get<BuildingController>().UpdateBuildingHighlightForPerks(entity);
					}
				}
			}
			else if (id != EventId.WorldLoadComplete)
			{
				if (id != EventId.GameStateChanged)
				{
					if (id == EventId.ActivePerksUpdated)
					{
						this.RefreshPerkBuildingHighlightTimer();
					}
				}
				else
				{
					IState currentState = Service.Get<GameStateMachine>().CurrentState;
					if (currentState is HomeState)
					{
						this.HighlightActivePerkBuildings();
						this.RefreshPerkBuildingHighlightTimer();
					}
				}
			}
			else
			{
				this.HighlightActivePerkBuildings();
				this.RefreshPerkBuildingHighlightTimer();
			}
			return EatResponse.NotEaten;
		}

		private void HighlightActivePerkBuildings()
		{
			List<Entity> buildingsForActivePerks = Service.Get<PerkManager>().GetBuildingsForActivePerks();
			int i = 0;
			int count = buildingsForActivePerks.Count;
			while (i < count)
			{
				Service.Get<BuildingController>().UpdateBuildingHighlightForPerks(buildingsForActivePerks[i]);
				i++;
			}
		}

		public string GetPerkNameForGroup(string perkGroup)
		{
			Lang lang = Service.Get<Lang>();
			return lang.Get("perk_title_" + perkGroup, new object[0]);
		}

		public string GetPerkDescForGroup(string perkGroup)
		{
			Lang lang = Service.Get<Lang>();
			return lang.Get("perk_desc_" + perkGroup, new object[0]);
		}

		public void SetPerkImage(UXTexture perkImage, PerkVO perkVO)
		{
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			string text = string.Empty;
			if (faction == FactionType.Empire)
			{
				text = perkVO.TextureIdEmpire;
			}
			else
			{
				text = perkVO.TextureIdRebel;
			}
			if (!string.IsNullOrEmpty(text))
			{
				IDataController dataController = Service.Get<IDataController>();
				TextureVO optional = dataController.GetOptional<TextureVO>(text);
				if (optional != null && perkImage.Tag != optional)
				{
					perkImage.LoadTexture(optional.AssetName);
					perkImage.Tag = optional;
				}
			}
		}

		public void SetupStatGridForPerk(PerkVO targetPerkVO, UXGrid statGrid, string templateElement, string descriptionElement, string valueElement, bool useUpgrade)
		{
			Lang lang = Service.Get<Lang>();
			IDataController dataController = Service.Get<IDataController>();
			string[] array = null;
			string[] perkEffects = targetPerkVO.PerkEffects;
			int num = perkEffects.Length;
			statGrid.SetTemplateItem(templateElement);
			string perkGroup = targetPerkVO.PerkGroup;
			int perkTier = targetPerkVO.PerkTier;
			if (useUpgrade && perkTier > 1)
			{
				PerkVO perkByGroupAndTier = GameUtils.GetPerkByGroupAndTier(perkGroup, perkTier - 1);
				array = perkByGroupAndTier.PerkEffects;
				if (perkEffects.Length != num)
				{
					Service.Get<StaRTSLogger>().Error("PerkEffects list not consistent between " + perkByGroupAndTier.Uid + " and " + targetPerkVO.Uid);
				}
			}
			statGrid.Clear();
			for (int i = 0; i < num; i++)
			{
				PerkEffectVO perkEffectVO = dataController.Get<PerkEffectVO>(perkEffects[i]);
				PerkEffectVO prevVO = null;
				if (array != null)
				{
					prevVO = dataController.Get<PerkEffectVO>(array[i]);
				}
				string itemUid = perkEffectVO.Uid + i;
				UXElement item = statGrid.CloneTemplateItem(itemUid);
				UXLabel subElement = statGrid.GetSubElement<UXLabel>(itemUid, descriptionElement);
				UXLabel subElement2 = statGrid.GetSubElement<UXLabel>(itemUid, valueElement);
				subElement.Text = lang.Get(perkEffectVO.StatStringId, new object[0]);
				subElement2.Text = this.GetFormattedValueBasedOnEffectType(perkEffectVO, prevVO);
				statGrid.AddItem(item, i);
			}
			statGrid.RepositionItems();
		}

		public string GetFormattedValueBasedOnEffectType(PerkEffectVO currentVO, PerkEffectVO prevVO)
		{
			Lang lang = Service.Get<Lang>();
			string displayValueForPerk = this.GetDisplayValueForPerk(currentVO);
			string id = currentVO.StatValueFormatStringId;
			if (prevVO != null)
			{
				string displayValueForPerk2 = this.GetDisplayValueForPerk(prevVO);
				if (displayValueForPerk2 != displayValueForPerk)
				{
					id = currentVO.StatUpgradeValueFormatStringId;
					return lang.Get(id, new object[]
					{
						displayValueForPerk2,
						displayValueForPerk
					});
				}
			}
			return lang.Get(id, new object[]
			{
				displayValueForPerk
			});
		}

		private string GetDisplayValueForPerk(PerkEffectVO vo)
		{
			string type = vo.Type;
			string result = "";
			if ("troopRequestTime" == type)
			{
				result = LangUtils.FormatTime((long)vo.TroopRequestTimeDiscount);
			}
			else if ("generator" == type)
			{
				result = Mathf.FloorToInt(vo.GenerationRate * 100f).ToString();
			}
			else if ("contractCost" == type)
			{
				result = Mathf.FloorToInt(vo.ContractDiscount * 100f).ToString();
			}
			else if ("contractTime" == type)
			{
				result = Mathf.FloorToInt(vo.ContractTimeReduction * 100f).ToString();
			}
			else if ("relocation" == type)
			{
				result = vo.RelocationDiscount.ToString();
			}
			else if ("troopRequestAmount" == type)
			{
				result = vo.TroopRequestAmount.ToString();
			}
			return result;
		}

		public void ShowActivePerksScreen(BuildingTypeVO vo)
		{
			ActivePerksInfoScreen screen = new ActivePerksInfoScreen(vo);
			Service.Get<ScreenController>().AddScreen(screen);
		}

		public void OnPerksButtonClicked(UXButton button)
		{
			this.ShowActivePerksScreen((BuildingTypeVO)button.Tag);
		}

		public void ShowCancelPerkAlert(string perkId, string perkGroup)
		{
			Lang lang = Service.Get<Lang>();
			string title = lang.Get("PERK_CANCEL_POPUP_TITLE", new object[0]);
			string perkNameForGroup = this.GetPerkNameForGroup(perkGroup);
			string message = lang.Get("PERK_CANCEL_POPUP_DESC", new object[]
			{
				perkNameForGroup
			});
			bool alwaysOnTop = true;
			YesNoScreen.ShowModal(title, message, false, false, false, alwaysOnTop, new OnScreenModalResult(this.OnCancelPerkModalResult), perkId, false);
		}

		public uint GetLastViewedPerkTime()
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			return sharedPlayerPrefs.GetPref<uint>("perks_last_view");
		}

		public void UpdateLastViewedPerkTime()
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			sharedPlayerPrefs.SetPref("perks_last_view", Service.Get<ServerAPI>().ServerTime.ToString());
		}

		public void AddToPerkBadgeList(string perkId)
		{
			IDataController dataController = Service.Get<IDataController>();
			PerkVO perkVO = dataController.Get<PerkVO>(perkId);
			if (perkVO != null)
			{
				List<string> list = this.GetListOfBadgedPerkGroups();
				if (list == null)
				{
					list = new List<string>();
				}
				int count = list.Count;
				int num = GameConstants.SQUADPERK_MAX_PERK_CARD_BADGES - count;
				if (num <= 0)
				{
					this.TrimPerkBadgeList(ref list, Math.Abs(num) + 1);
				}
				string perkGroup = perkVO.PerkGroup;
				list.Add(perkGroup);
				this.SetPerkBadgeList(list);
				return;
			}
			Service.Get<StaRTSLogger>().Error("PerkViewController.AddToPerkBadgeList Failed to find Perk Data for: " + perkId);
		}

		public void RemovePerkGroupFromBadgeList(string perkGroup)
		{
			List<string> listOfBadgedPerkGroups = this.GetListOfBadgedPerkGroups();
			if (listOfBadgedPerkGroups != null && listOfBadgedPerkGroups.Count > 0)
			{
				listOfBadgedPerkGroups.Remove(perkGroup);
				this.SetPerkBadgeList(listOfBadgedPerkGroups);
			}
		}

		public int GetBadgedPerkCount()
		{
			List<string> listOfBadgedPerkGroups = this.GetListOfBadgedPerkGroups();
			int result = 0;
			if (listOfBadgedPerkGroups != null)
			{
				result = listOfBadgedPerkGroups.Count;
			}
			return result;
		}

		public bool IsPerkGroupBadged(string perkGroup)
		{
			bool result = false;
			List<string> listOfBadgedPerkGroups = this.GetListOfBadgedPerkGroups();
			if (listOfBadgedPerkGroups != null && listOfBadgedPerkGroups.Count > 0)
			{
				result = listOfBadgedPerkGroups.Contains(perkGroup);
			}
			return result;
		}

		private void TrimPerkBadgeList(ref List<string> perkBadges, int amtToRemove)
		{
			int num = 0;
			while (num < amtToRemove && perkBadges.Count != 0)
			{
				perkBadges.RemoveAt(0);
				num++;
			}
		}

		private List<string> GetListOfBadgedPerkGroups()
		{
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			if (currentSquad == null)
			{
				return null;
			}
			Dictionary<string, string> available = currentSquad.Perks.Available;
			int level = currentSquad.Level;
			PerkManager perkManager = Service.Get<PerkManager>();
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			string pref = sharedPlayerPrefs.GetPref<string>("perk_badges");
			if (!string.IsNullOrEmpty(pref))
			{
				List<string> list = new List<string>();
				string[] array = pref.Split(new char[]
				{
					' '
				});
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					string text = array[i];
					if (available.ContainsKey(text))
					{
						PerkVO perkData = Service.Get<IDataController>().Get<PerkVO>(available[text]);
						if (!perkManager.IsPerkLevelLocked(perkData, level) && !perkManager.IsPerkReputationLocked(perkData, level, available) && !perkManager.IsPerkGroupActive(text) && !perkManager.IsPerkGroupInCooldown(text))
						{
							list.Add(text);
						}
					}
				}
				return list;
			}
			return null;
		}

		private void SetPerkBadgeList(List<string> badgedGroups)
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			string text = "";
			if (badgedGroups != null)
			{
				int count = badgedGroups.Count;
				for (int i = 0; i < count; i++)
				{
					text += badgedGroups[i];
					if (i < count - 1)
					{
						text += " ";
					}
				}
			}
			sharedPlayerPrefs.SetPrefUnlimitedLength("perk_badges", text);
		}

		public void ShowSquadLevelUpIfPending()
		{
			ScreenController screenController = Service.Get<ScreenController>();
			PerkManager perkManager = Service.Get<PerkManager>();
			SquadController squadController = Service.Get<SquadController>();
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			int level = currentSquad.Level;
			int lastViewedSquadLevelUp = squadController.GetLastViewedSquadLevelUp();
			if (!perkManager.HasPlayerSeenPerkTutorial())
			{
				return;
			}
			if (level > 1 && level > lastViewedSquadLevelUp)
			{
				squadController.SetLastViewedSquadLevelUp(level);
				int num = 1;
				if (lastViewedSquadLevelUp > 0)
				{
					num = level - lastViewedSquadLevelUp;
				}
				int sQUADPERK_MAX_SQUAD_LEVEL_CELEBRATIONS_SHOWN = GameConstants.SQUADPERK_MAX_SQUAD_LEVEL_CELEBRATIONS_SHOWN;
				if (sQUADPERK_MAX_SQUAD_LEVEL_CELEBRATIONS_SHOWN < num)
				{
					num = sQUADPERK_MAX_SQUAD_LEVEL_CELEBRATIONS_SHOWN;
				}
				int num2 = level - num + 1;
				QueueScreenBehavior subType = QueueScreenBehavior.Default;
				SquadLevelUpCelebrationScreen highestLevelScreen = screenController.GetHighestLevelScreen<SquadLevelUpCelebrationScreen>();
				if (highestLevelScreen != null)
				{
					num2 = level;
					subType = QueueScreenBehavior.QueueAndDeferTillClosed;
				}
				for (int i = num2; i <= level; i++)
				{
					List<PerkVO> perksUnlockedAtSquadLevel = perkManager.GetPerksUnlockedAtSquadLevel(i);
					SquadLevelUpCelebrationScreen screen = new SquadLevelUpCelebrationScreen(i, perksUnlockedAtSquadLevel);
					screenController.AddScreen(screen, subType);
					subType = QueueScreenBehavior.QueueAndDeferTillClosed;
				}
			}
		}

		private void OnCancelPerkModalResult(object result, object cookie)
		{
			if (result == null)
			{
				return;
			}
			PerkManager perkManager = Service.Get<PerkManager>();
			string text = cookie as string;
			bool flag = perkManager.CancelPlayerPerk(text);
			if (flag)
			{
				PerkVO perkVO = Service.Get<IDataController>().Get<PerkVO>(text);
				List<Entity> buildingsForPerk = perkManager.GetBuildingsForPerk(perkVO);
				int i = 0;
				int count = buildingsForPerk.Count;
				while (i < count)
				{
					Service.Get<BuildingController>().UpdateBuildingHighlightForPerks(buildingsForPerk[i]);
					i++;
				}
			}
		}

		protected internal PerkViewController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).AddToPerkBadgeList(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).GetBadgedPerkCount());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).GetDisplayValueForPerk((PerkEffectVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).GetFormattedValueBasedOnEffectType((PerkEffectVO)GCHandledObjects.GCHandleToObject(*args), (PerkEffectVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).GetListOfBadgedPerkGroups());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).GetPerkDescForGroup(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).GetPerkNameForGroup(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).HighlightActivePerkBuildings();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).IsPerkGroupBadged(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).OnCancelPerkModalResult(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).OnPerksButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).RefreshPerkBuildingHighlightTimer();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).RegisterEvents();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).RemovePerkGroupFromBadgeList(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).SetPerkBadgeList((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).SetPerkImage((UXTexture)GCHandledObjects.GCHandleToObject(*args), (PerkVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).SetupStatGridForPerk((PerkVO)GCHandledObjects.GCHandleToObject(*args), (UXGrid)GCHandledObjects.GCHandleToObject(args[1]), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), Marshal.PtrToStringUni(*(IntPtr*)(args + 4)), *(sbyte*)(args + 5) != 0);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).ShowActivePerksScreen((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).ShowCancelPerkAlert(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).ShowSquadLevelUpIfPending();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((PerkViewController)GCHandledObjects.GCHandleToObject(instance)).UpdateLastViewedPerkTime();
			return -1L;
		}
	}
}
