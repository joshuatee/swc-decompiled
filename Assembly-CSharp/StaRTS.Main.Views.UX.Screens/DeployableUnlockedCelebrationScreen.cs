using StaRTS.Assets;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.ValueObjects;
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
	public class DeployableUnlockedCelebrationScreen : ScreenBase, IViewFrameTimeObserver
	{
		private const int CENTER_THRESHOLD = 7;

		private const float OUTLINE_WIDTH = 0.0008f;

		private const float FAR_Y = 1000f;

		private const string BUTTON_CONTINUE = "ButtonPrimaryAction";

		private const string LABEL_DEPLOYABLE_NAME = "LabelEquipmentName";

		private const string LABEL_DEPLOYABLE_UNLOCK_TITLE = "LabelEquipmentText";

		private const string LABEL_INSTRUCTIONS = "LabelInstructions";

		private const string SPRITE_INSTRUCTIONS = "SpriteInstructions";

		private const string UNLOCK_SHRD_INFANTRY_TITLE = "UNLOCK_SHRD_INFANTRY_TITLE";

		private const string UNLOCK_SHRD_VEHICLE_TITLE = "UNLOCK_SHRD_VEHICLE_TITLE";

		private const string UNLOCK_SHRD_STARSHIP_TITLE = "UNLOCK_SHRD_STARSHIP_TITLE";

		private const string UNLOCK_SHRD_HERO_TITLE = "UNLOCK_SHRD_HERO_TITLE";

		private const string UNLOCK_SHRD_MERCENARY_TITLE = "UNLOCK_SHRD_MERCENARY_TITLE";

		private const string UNLOCK_SHRD_INFANTRY_INSTRUCTIONS = "UNLOCK_SHRD_INFANTRY_INSTRUCTIONS";

		private const string UNLOCK_SHRD_VEHICLE_INSTRUCTIONS = "UNLOCK_SHRD_VEHICLE_INSTRUCTIONS";

		private const string UNLOCK_SHRD_STARSHIP_INSTRUCTIONS = "UNLOCK_SHRD_STARSHIP_INSTRUCTIONS";

		private const string UNLOCK_SHRD_HERO_INSTRUCTIONS = "UNLOCK_SHRD_HERO_INSTRUCTIONS";

		private const string UNLOCK_SHRD_MERCENARY_INSTRUCTIONS = "UNLOCK_SHRD_MERCENARY_INSTRUCTIONS";

		private const string UPGRADE_SHRD_INFANTRY_TITLE = "UPGRADE_SHRD_INFANTRY_TITLE";

		private const string UPGRADE_SHRD_VEHICLE_TITLE = "UPGRADE_SHRD_VEHICLE_TITLE";

		private const string UPGRADE_SHRD_STARSHIP_TITLE = "UPGRADE_SHRD_STARSHIP_TITLE";

		private const string UPGRADE_SHRD_HERO_TITLE = "UPGRADE_SHRD_HERO_TITLE";

		private const string UPGRADE_SHRD_MERCENARY_TITLE = "UPGRADE_SHRD_MERCENARY_TITLE";

		protected IDeployableVO subjectVO;

		private GameObject rig;

		private GameObject subject;

		private AssetHandle rigHandle;

		private AssetHandle subjectHandle;

		private List<Material> outLineMatList;

		private bool fadingOutSubject;

		protected UXButton buttonContinue;

		private UXGrid itemGrid;

		private bool isSpecialAttack;

		private bool isUnlock;

		protected override bool WantTransitions
		{
			get
			{
				return false;
			}
		}

		public DeployableUnlockedCelebrationScreen(IDeployableVO vo, bool isSpecialAttack, bool isUnlock)
		{
			this.outLineMatList = new List<Material>();
			base..ctor("gui_equipment_unlocked");
			this.subjectVO = vo;
			this.isSpecialAttack = isSpecialAttack;
			this.isUnlock = isUnlock;
		}

		protected override void OnScreenLoaded()
		{
			BuildingLookupController buildingLookupController = Service.Get<BuildingLookupController>();
			Service.Get<UXController>().HUD.Visible = false;
			this.InitButtons();
			this.SetUIText();
			this.LoadFx();
			UXSprite element = base.GetElement<UXSprite>("SpriteInstructions");
			BuildingTypeVO buildingTypeVO = null;
			if (this.isSpecialAttack)
			{
				if (buildingLookupController.HasStarshipCommand())
				{
					FleetCommandNode head = buildingLookupController.FleetCommandNodeList.Head;
					buildingTypeVO = head.BuildingComp.BuildingType;
				}
			}
			else
			{
				buildingTypeVO = buildingLookupController.GetHighestAvailableBuildingVOForTroop((TroopTypeVO)this.subjectVO);
			}
			if (buildingTypeVO != null)
			{
				ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(buildingTypeVO, element, false);
				projectorConfig.AnimPreference = AnimationPreference.AnimationPreferred;
				ProjectorUtils.GenerateProjector(projectorConfig);
			}
			else
			{
				element.Visible = false;
			}
			this.fadingOutSubject = false;
		}

		public override void OnDestroyElement()
		{
			if (this.itemGrid != null)
			{
				this.itemGrid.Clear();
				this.itemGrid = null;
			}
			base.OnDestroyElement();
			if (this.subject != null)
			{
				UnityEngine.Object.Destroy(this.subject);
				this.subject = null;
			}
			if (this.rig != null)
			{
				UnityEngine.Object.Destroy(this.rig);
				this.rig = null;
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
			if (this.subjectHandle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.subjectHandle);
				this.subjectHandle = AssetHandle.Invalid;
			}
			if (this.rigHandle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.rigHandle);
				this.rigHandle = AssetHandle.Invalid;
			}
			if (this.fadingOutSubject)
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
			string text;
			if (this.isSpecialAttack)
			{
				text = LangUtils.GetStarshipDisplayName((SpecialAttackTypeVO)this.subjectVO);
			}
			else
			{
				text = LangUtils.GetTroopDisplayName((TroopTypeVO)this.subjectVO);
			}
			base.GetElement<UXLabel>("LabelEquipmentName").Text = text;
			string text2;
			if (this.isUnlock)
			{
				text2 = this.GetUnlockDisplayTitleBasedOnSubject();
			}
			else
			{
				text2 = this.GetUpgradeDisplayTitleBasedOnSubject();
			}
			base.GetElement<UXLabel>("LabelEquipmentText").Text = text2;
			base.GetElement<UXLabel>("LabelInstructions").Text = this.GetDisplayInstructionsBasedOnSubject();
		}

		private string GetUnlockDisplayTitleBasedOnSubject()
		{
			string result = "";
			if (this.isSpecialAttack)
			{
				result = this.lang.Get("UNLOCK_SHRD_STARSHIP_TITLE", new object[0]);
			}
			else
			{
				TroopTypeVO troopTypeVO = (TroopTypeVO)this.subjectVO;
				switch (troopTypeVO.Type)
				{
				case TroopType.Infantry:
					result = this.lang.Get("UNLOCK_SHRD_INFANTRY_TITLE", new object[0]);
					break;
				case TroopType.Vehicle:
					result = this.lang.Get("UNLOCK_SHRD_VEHICLE_TITLE", new object[0]);
					break;
				case TroopType.Mercenary:
					result = this.lang.Get("UNLOCK_SHRD_MERCENARY_TITLE", new object[0]);
					break;
				case TroopType.Hero:
					result = this.lang.Get("UNLOCK_SHRD_HERO_TITLE", new object[0]);
					break;
				}
			}
			return result;
		}

		private string GetUpgradeDisplayTitleBasedOnSubject()
		{
			string result = "";
			if (this.isSpecialAttack)
			{
				result = this.lang.Get("UPGRADE_SHRD_STARSHIP_TITLE", new object[0]);
			}
			else
			{
				TroopTypeVO troopTypeVO = (TroopTypeVO)this.subjectVO;
				switch (troopTypeVO.Type)
				{
				case TroopType.Infantry:
					result = this.lang.Get("UPGRADE_SHRD_INFANTRY_TITLE", new object[0]);
					break;
				case TroopType.Vehicle:
					result = this.lang.Get("UPGRADE_SHRD_VEHICLE_TITLE", new object[0]);
					break;
				case TroopType.Mercenary:
					result = this.lang.Get("UPGRADE_SHRD_MERCENARY_TITLE", new object[0]);
					break;
				case TroopType.Hero:
					result = this.lang.Get("UPGRADE_SHRD_HERO_TITLE", new object[0]);
					break;
				}
			}
			return result;
		}

		private string GetDisplayInstructionsBasedOnSubject()
		{
			string result = "";
			if (this.isSpecialAttack)
			{
				result = this.lang.Get("UNLOCK_SHRD_STARSHIP_INSTRUCTIONS", new object[0]);
			}
			else
			{
				TroopTypeVO troopTypeVO = (TroopTypeVO)this.subjectVO;
				switch (troopTypeVO.Type)
				{
				case TroopType.Infantry:
					result = this.lang.Get("UNLOCK_SHRD_INFANTRY_INSTRUCTIONS", new object[0]);
					break;
				case TroopType.Vehicle:
					result = this.lang.Get("UNLOCK_SHRD_VEHICLE_INSTRUCTIONS", new object[0]);
					break;
				case TroopType.Mercenary:
					result = this.lang.Get("UNLOCK_SHRD_MERCENARY_INSTRUCTIONS", new object[0]);
					break;
				case TroopType.Hero:
					result = this.lang.Get("UNLOCK_SHRD_HERO_INSTRUCTIONS", new object[0]);
					break;
				}
			}
			return result;
		}

		private void LoadFx()
		{
			Service.Get<AssetManager>().Load(ref this.rigHandle, "gui_celebrationunlockrig", new AssetSuccessDelegate(this.ShowFx), null, null);
		}

		private void ShowFx(object asset, object cookie)
		{
			this.rig = (GameObject)asset;
			this.rig = UnityEngine.Object.Instantiate<GameObject>(this.rig);
			this.rig.transform.position = new Vector3(0f, 1000f, 0f);
			IGeometryVO geometryVO = this.subjectVO;
			Service.Get<AssetManager>().Load(ref this.subjectHandle, geometryVO.IconAssetName, new AssetSuccessDelegate(this.ArrangeRig), null, null);
		}

		private void ArrangeRig(object asset, object cookie)
		{
			this.subject = (GameObject)asset;
			GameObject gameObject = UnityUtils.FindGameObject(this.rig, "content_scaler");
			GameObject gameObject2 = UnityUtils.FindGameObject(this.rig, "CelebrationCamera");
			GameObject gameObject3 = UnityUtils.FindGameObject(this.rig, "BuildingDisplayRig");
			string shaderName = "Outline_Unlit";
			Shader shader = Service.Get<AssetManager>().Shaders.GetShader(shaderName);
			if (this.subject != null && gameObject != null)
			{
				this.subject.transform.parent = gameObject.transform;
				this.subject.transform.localPosition = this.subjectVO.IconUnlockPosition;
				this.subject.transform.localEulerAngles = this.subjectVO.IconUnlockRotation;
				this.subject.transform.localScale = this.subjectVO.IconUnlockScale;
				AssetMeshDataMonoBehaviour component = this.subject.GetComponent<AssetMeshDataMonoBehaviour>();
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
				if (component.ShadowGameObject != null)
				{
					component.ShadowGameObject.SetActive(false);
				}
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
			Service.Get<EventManager>().SendEvent(EventId.DeployableUnlockCelebrationPlayed, null);
		}

		private void OnButtonContinueClicked(UXButton button)
		{
			this.Close(true);
		}

		public override void Close(object modalResult)
		{
			base.Close(modalResult);
			BuildingController buildingController = Service.Get<BuildingController>();
			Service.Get<UXController>().HUD.ShowContextButtons(buildingController.SelectedBuilding);
			if (this.WantTransitions)
			{
				this.FadeOutSubject();
			}
		}

		private void FadeOutSubject()
		{
			if (this.fadingOutSubject || this.outLineMatList.Count == 0)
			{
				return;
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
			this.fadingOutSubject = true;
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
			this.fadingOutSubject = false;
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		protected internal DeployableUnlockedCelebrationScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).ArrangeRig(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).FadeOutSubject();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).WantTransitions);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).GetDisplayInstructionsBasedOnSubject());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).GetUnlockDisplayTitleBasedOnSubject());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).GetUpgradeDisplayTitleBasedOnSubject());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).LoadFx();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).OnButtonContinueClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).SetUIText();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((DeployableUnlockedCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).ShowFx(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
