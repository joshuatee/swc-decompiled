using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.RUF;
using StaRTS.Main.Story;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Controls;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class HQCelebScreen : ScreenBase, IViewFrameTimeObserver
	{
		private const int CENTER_THRESHOLD = 7;

		private const float OUTLINE_WIDTH = 0.0008f;

		private const string RATE_APP_STORY_UID = "rate_app_de";

		private const string ITEM_TURRETS = "TURRETS";

		private const string LABEL_TURRETS = "LabelTurrets";

		private const float FAR_Y = 1000f;

		private const string BUTTON_CONTINUE = "ButtonPrimaryAction";

		private const string GRID_UNLOCKED = "UnlockItemsGrid";

		private const string GROUP_UNLOCKED = "UnlockItems";

		private const string LABEL_UNLOCKCOUNT = "LabelUnlockCount";

		private const string LABEL_BUILDINGLEVEL = "LabelBuildingLevel";

		private const string LABEL_BUILDINGNAME = "LabelBuildingName";

		private const string LABEL_ITEMS = "LabelItems";

		private const string SPRITE_UNLOCKITEM = "SpriteItemImage";

		private const string TEMPLATE_UNLOCKED = "UnlockItemsTemplate";

		private GameObject celebrationRig;

		private GameObject buildingHq;

		private GameObject buildingHqShadow;

		private int upgradedLevel;

		private AssetHandle buildingHandle;

		private AssetHandle rigHandle;

		private bool fadingOutBuilding;

		private List<Material> outLineMatList;

		protected BuildingLookupController buildingLookUpController;

		protected BuildingTypeVO headQuarter;

		protected UXButton buttonContinue;

		private UXGrid itemGrid;

		public HQCelebScreen()
		{
			this.outLineMatList = new List<Material>();
			base..ctor("gui_building_complete");
			Service.Get<GalaxyViewController>().GoToHome();
			this.buildingLookUpController = Service.Get<BuildingLookupController>();
			Entity currentHQ = this.buildingLookUpController.GetCurrentHQ();
			this.headQuarter = currentHQ.Get<BuildingComponent>().BuildingType;
			this.upgradedLevel = this.headQuarter.Lvl;
		}

		protected override void OnScreenLoaded()
		{
			Service.Get<UXController>().HUD.Visible = false;
			this.InitButtons();
			this.SetUIText();
			this.SetupItemGrid();
			this.LoadFx();
			this.fadingOutBuilding = false;
		}

		public override void OnDestroyElement()
		{
			Service.Get<UXController>().HUD.Visible = true;
			if (this.itemGrid != null)
			{
				this.itemGrid.Clear();
				this.itemGrid = null;
			}
			base.OnDestroyElement();
			if (this.buildingHq != null)
			{
				UnityEngine.Object.Destroy(this.buildingHq);
				this.buildingHq = null;
				this.buildingHqShadow = null;
			}
			if (this.celebrationRig != null)
			{
				UnityEngine.Object.Destroy(this.celebrationRig);
				this.celebrationRig = null;
			}
			int i = 0;
			int count = this.outLineMatList.Count;
			while (i < count)
			{
				Material material = this.outLineMatList[i];
				if (material != null)
				{
					UnityUtils.DestroyMaterial(material);
				}
				i++;
			}
			this.outLineMatList.Clear();
			if (this.buildingHandle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.buildingHandle);
				this.buildingHandle = AssetHandle.Invalid;
			}
			if (this.rigHandle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.rigHandle);
				this.rigHandle = AssetHandle.Invalid;
			}
			if (this.fadingOutBuilding)
			{
				Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			}
		}

		private void InitButtons()
		{
			this.buttonContinue = base.GetElement<UXButton>("ButtonPrimaryAction");
			this.buttonContinue.OnClicked = new UXButtonClickedDelegate(this.OnButtonContinueClicked);
			base.CurrentBackDelegate = new UXButtonClickedDelegate(this.OnButtonContinueClicked);
			base.CurrentBackButton = this.buttonContinue;
			base.AllowFUEBackButton = true;
			Service.Get<UserInputInhibitor>().AddToAllow(this.buttonContinue);
		}

		private void SetUIText()
		{
			base.GetElement<UXLabel>("LabelBuildingLevel").Text = this.lang.Get("LEVEL", new object[]
			{
				this.upgradedLevel
			});
			base.GetElement<UXLabel>("LabelBuildingName").Text = this.lang.Get("HQ_CELEB_UPGRADED", new object[0]);
			base.GetElement<UXLabel>("LabelItems").Text = this.lang.Get("HQ_CELEB_UNLOCKED", new object[0]);
		}

		private void LoadFx()
		{
			Service.Get<AssetManager>().Load(ref this.rigHandle, "gui_CelebrationHQRig", new AssetSuccessDelegate(this.ShowFx), null, null);
		}

		private void ShowFx(object asset, object cookie)
		{
			this.celebrationRig = (GameObject)asset;
			this.celebrationRig = UnityEngine.Object.Instantiate<GameObject>(this.celebrationRig);
			this.celebrationRig.transform.position = new Vector3(0f, 1000f, 0f);
			Service.Get<AssetManager>().Load(ref this.buildingHandle, this.headQuarter.AssetName, new AssetSuccessDelegate(this.ArrangeRig), null, null);
		}

		private void ArrangeRig(object asset, object cookie)
		{
			this.buildingHq = (GameObject)asset;
			GameObject gameObject = UnityUtils.FindGameObject(this.celebrationRig, "BuildingHolder");
			GameObject gameObject2 = UnityUtils.FindGameObject(this.celebrationRig, "CelebrationCamera");
			GameObject gameObject3 = UnityUtils.FindGameObject(this.celebrationRig, "BuildingDisplayRig");
			string shaderName = "Outline_Unlit";
			Shader shader = Service.Get<AssetManager>().Shaders.GetShader(shaderName);
			if (this.buildingHq != null && gameObject != null)
			{
				this.buildingHq.transform.parent = gameObject.transform;
				this.buildingHq.transform.localPosition = Vector3.zero;
				this.buildingHq.transform.localRotation = Quaternion.identity;
				this.buildingHq.transform.localScale = Vector3.one;
				AssetMeshDataMonoBehaviour component = this.buildingHq.GetComponent<AssetMeshDataMonoBehaviour>();
				if (component == null)
				{
					return;
				}
				int i = 0;
				int count = component.SelectableGameObjects.Count;
				while (i < count)
				{
					Renderer component2 = component.SelectableGameObjects[i].GetComponent<Renderer>();
					if (component2 != null)
					{
						Material material = UnityUtils.EnsureMaterialCopy(component2);
						this.outLineMatList.Add(material);
						if (material != null)
						{
							material.shader = shader;
							material.SetColor("_OutlineColor", FXUtils.SELECTION_OUTLINE_COLOR);
							material.SetFloat("_Outline", 0.0008f);
						}
					}
					i++;
				}
				this.buildingHqShadow = component.ShadowGameObject;
			}
			if (gameObject2 != null)
			{
				gameObject2.GetComponent<Camera>().depth = 1f;
			}
			if (gameObject3 != null)
			{
				Animator component3 = gameObject3.GetComponent<Animator>();
				if (component3)
				{
					component3.enabled = true;
					component3.SetTrigger("Show");
				}
			}
			Service.Get<EventManager>().SendEvent(EventId.HQCelebrationPlayed, null);
		}

		private void SetupItemGrid()
		{
			this.itemGrid = base.GetElement<UXGrid>("UnlockItemsGrid");
			this.itemGrid.SetTemplateItem("UnlockItemsTemplate");
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			Dictionary<BuildingTypeVO, int> buildingsUnlockedBy = buildingLookupController.GetBuildingsUnlockedBy(this.headQuarter);
			int num = 0;
			foreach (KeyValuePair<BuildingTypeVO, int> current in buildingsUnlockedBy)
			{
				BuildingTypeVO key = current.get_Key();
				int value = current.get_Value();
				if (key.Type == BuildingType.Turret && key.BuildingRequirement != this.headQuarter.Uid)
				{
					if (num == 0)
					{
						num = value;
					}
				}
				else
				{
					string uid = key.Uid;
					UXElement item = this.itemGrid.CloneTemplateItem(uid);
					UXSprite subElement = this.itemGrid.GetSubElement<UXSprite>(uid, "SpriteItemImage");
					ProjectorConfig projectorConfig = ProjectorUtils.GenerateBuildingConfig(key, subElement);
					projectorConfig.AnimPreference = AnimationPreference.AnimationPreferred;
					ProjectorUtils.GenerateProjector(projectorConfig);
					UXLabel subElement2 = this.itemGrid.GetSubElement<UXLabel>(uid, "LabelUnlockCount");
					if (key.Type == BuildingType.Turret)
					{
						subElement2.Visible = false;
					}
					else
					{
						subElement2.Text = this.lang.Get("TROOP_MULTIPLIER", new object[]
						{
							value
						});
					}
					this.itemGrid.AddItem(item, key.Order);
				}
			}
			UXElement item2 = this.itemGrid.CloneTemplateItem("TURRETS");
			UXSprite subElement3 = this.itemGrid.GetSubElement<UXSprite>("TURRETS", "SpriteItemImage");
			subElement3.Visible = false;
			UXLabel subElement4 = this.itemGrid.GetSubElement<UXLabel>("TURRETS", "LabelUnlockCount");
			subElement4.Visible = false;
			UXLabel subElement5 = this.itemGrid.GetSubElement<UXLabel>("TURRETS", "LabelTurrets");
			subElement5.Visible = true;
			subElement5.Text = this.lang.Get("HQ_UPGRADE_TURRETS_UNLOCKED", new object[]
			{
				num
			});
			this.itemGrid.AddItem(item2, 99999999);
			this.itemGrid.RepositionItems();
			this.itemGrid.Scroll((this.itemGrid.Count > 7) ? 0f : 0.5f);
		}

		private void OnButtonContinueClicked(UXButton button)
		{
			this.Close(true);
		}

		public override void Close(object modalResult)
		{
			base.Close(modalResult);
			if (this.WantTransitions)
			{
				this.FadeOutBuilding();
			}
			if (!Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress && !Service.Get<RUFManager>().OmitRateAppLevels.Contains(this.upgradedLevel))
			{
				new ActionChain("rate_app_de");
			}
			Service.Get<EventManager>().SendEvent(EventId.HQCelebrationScreenClosed, this.upgradedLevel);
		}

		private void FadeOutBuilding()
		{
			if (this.fadingOutBuilding || this.outLineMatList.Count == 0)
			{
				return;
			}
			if (this.buildingHqShadow != null)
			{
				this.buildingHqShadow.SetActive(false);
			}
			Shader shader = Service.Get<AssetManager>().Shaders.GetShader("UnlitTexture_Fade");
			int i = 0;
			int count = this.outLineMatList.Count;
			while (i < count)
			{
				Material material = this.outLineMatList[i];
				if (material != null)
				{
					material.shader = shader;
				}
				i++;
			}
			this.fadingOutBuilding = true;
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		public void OnViewFrameTime(float dt)
		{
			float alpha = base.GetAlpha();
			if (alpha > 0f)
			{
				int i = 0;
				int count = this.outLineMatList.Count;
				while (i < count)
				{
					Material material = this.outLineMatList[i];
					if (material != null)
					{
						material.color = new Color(1f, 1f, 1f, alpha);
					}
					i++;
				}
				return;
			}
			this.fadingOutBuilding = false;
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		protected internal HQCelebScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).ArrangeRig(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).FadeOutBuilding();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).LoadFx();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).OnButtonContinueClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).SetUIText();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).SetupItemGrid();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((HQCelebScreen)GCHandledObjects.GCHandleToObject(instance)).ShowFx(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
