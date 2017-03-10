using StaRTS.Main.Controllers;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Story;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class HolocommScreen : ScreenBase, IUserInputObserver, IEventObserver
	{
		public delegate void HoloCallback();

		private const string INFO_PANEL = "InfoItems";

		private const string INFO_LABEL = "LabelInfoItem";

		private const string INFO_TEXTURE_HOLDER = "SpriteInfoItem";

		private const string PLANET_TEXTURE_HOLDER = "SpriteInfoItemPlanets";

		private const string INFO_TITLE_LABEL = "LabelInfoItemTitle";

		public const string NEXT_BUTTON = "BtnNext";

		public const string NEXT_BUTTON_LABEL = "LabelBtnNext";

		public const string STORE_BUTTON = "ButtonStore";

		private const string REGULAR_TEXT_BOX_GROUP = "NpcDialogLarge";

		private const string REGULAR_TITLE_LABEL = "LabelNpcMessageBottomLeftTitleLarge";

		private const string REGULAR_BODY_LABEL = "LabelNpcMessageBottomLeftBodyLarge";

		private const string STORE_TEXT_BOX_GROUP = "NpcDialog";

		private const string STORE_TITLE_LABEL = "LabelNpcMessageBottomLeftTitle";

		private const string STORE_BODY_LABEL = "LabelNpcMessageBottomLeftBody";

		private const string HOLO_POSITION_PANEL = "HoloHolder";

		private const string TEXTURE_INFO_ALLOY = "InfoAlloy";

		private const string TEXTURE_INFO_ATST = "InfoATST";

		private const string TEXTURE_INFO_CREDIT = "InfoCredit";

		private const string TEXTURE_INFO_CROSSGRADE = "InfoCrossgrade";

		private const string TEXTURE_INFO_DROID = "InfoDroid";

		private const string TEXTURE_INFO_HAN = "InfoHan";

		private const string TEXTURE_INFO_STRIX = "InfoStrix";

		private const string TEXTURE_INFO_TURRET = "InfoTurret";

		private const string TEXTURE_INFO_GALAXY_VIEW = "InfoGalaxyView";

		private const string TEXTURE_INFO_PLANETARY_EMPIRE = "InfoPlanetaryCommandEmpire";

		private const string TEXTURE_INFO_PLANETARY_REBEL = "InfoPlanetaryCommandRebel";

		private UXLabel infoLabel;

		private UXElement infoPanel;

		private UXTexture infoTexture;

		private UXTexture infoPlanetTexture;

		private UXLabel infoTitleLabel;

		private UXButton nextButton;

		private UXButton storeButton;

		private UXElement holoPositioner;

		private UXElement regularTextBoxGroup;

		private UXLabel regularTitleLabel;

		private UXLabel regularBodyLabel;

		private UXElement storeTextBoxGroup;

		private UXLabel storeTitleLabel;

		private UXLabel storeBodyLabel;

		private float storeAnchorAbsolute;

		private float regularAnchorAbsolute;

		private HoloCharacter currentCharacter;

		public HolocommScreen() : base("gui_npc_dialog")
		{
			base.IsAlwaysOnTop = true;
			Service.Get<UserInputManager>().RegisterObserver(this, UserInputLayer.Screen);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ShowHologramComplete);
		}

		public bool HasCharacterShowing()
		{
			return this.currentCharacter != null;
		}

		public void ResizeCharacter()
		{
			if (this.currentCharacter != null)
			{
				this.currentCharacter.ResizeCurrentCharacter();
				float num = (float)Screen.height / 1080f;
				this.storeTitleLabel.GetUIWidget.transform.parent.GetComponent<UIWidget>().leftAnchor.absolute = 50 + (int)(this.storeAnchorAbsolute * num);
				this.regularTitleLabel.GetUIWidget.transform.parent.GetComponent<UIWidget>().leftAnchor.absolute = 50 + (int)(this.storeAnchorAbsolute * num);
			}
		}

		public void ResizeCurrentCharacter(int width, int height)
		{
			if (this.currentCharacter != null)
			{
				string characterId = this.currentCharacter.CharacterId;
				this.ShowHoloCharacter(characterId);
				Vector3 vector = this.holoPositioner.LocalPosition;
				vector = base.UXCamera.Camera.ScreenToViewportPoint(vector);
				this.currentCharacter.ResizeCurrentCharacter(vector, width, height);
				float num = (float)Screen.height / 1080f;
				this.storeTitleLabel.GetUIWidget.transform.parent.GetComponent<UIWidget>().leftAnchor.absolute = 50 + (int)(this.storeAnchorAbsolute * num);
				this.regularTitleLabel.GetUIWidget.transform.parent.GetComponent<UIWidget>().leftAnchor.absolute = 50 + (int)(this.storeAnchorAbsolute * num);
			}
		}

		protected override void OnScreenLoaded()
		{
			this.InitElements();
		}

		private void InitElements()
		{
			base.GetElement<UXLabel>("LabelBtnNext").Text = this.lang.Get("s_WhatsNextButton", new object[0]);
			this.holoPositioner = base.GetElement<UXElement>("HoloHolder");
			this.nextButton = base.GetElement<UXButton>("BtnNext");
			this.storeButton = base.GetElement<UXButton>("ButtonStore");
			this.infoPanel = base.GetElement<UXElement>("InfoItems");
			this.infoLabel = base.GetElement<UXLabel>("LabelInfoItem");
			this.infoTexture = base.GetElement<UXTexture>("SpriteInfoItem");
			this.infoPlanetTexture = base.GetElement<UXTexture>("SpriteInfoItemPlanets");
			this.infoTitleLabel = base.GetElement<UXLabel>("LabelInfoItemTitle");
			this.regularTextBoxGroup = base.GetElement<UXElement>("NpcDialogLarge");
			this.regularTitleLabel = base.GetElement<UXLabel>("LabelNpcMessageBottomLeftTitleLarge");
			this.regularBodyLabel = base.GetElement<UXLabel>("LabelNpcMessageBottomLeftBodyLarge");
			this.storeTextBoxGroup = base.GetElement<UXElement>("NpcDialog");
			this.storeTitleLabel = base.GetElement<UXLabel>("LabelNpcMessageBottomLeftTitle");
			this.storeBodyLabel = base.GetElement<UXLabel>("LabelNpcMessageBottomLeftBody");
			this.storeAnchorAbsolute = (float)this.storeTitleLabel.GetUIWidget.transform.parent.GetComponent<UIWidget>().leftAnchor.absolute;
			this.regularAnchorAbsolute = (float)this.regularTitleLabel.GetUIWidget.transform.parent.GetComponent<UIWidget>().leftAnchor.absolute;
			float num = (float)Screen.height / 1080f;
			this.storeTitleLabel.GetUIWidget.transform.parent.GetComponent<UIWidget>().leftAnchor.absolute = 50 + (int)(this.storeAnchorAbsolute * num);
			this.regularTitleLabel.GetUIWidget.transform.parent.GetComponent<UIWidget>().leftAnchor.absolute = 50 + (int)(this.storeAnchorAbsolute * num);
			this.HideAllElements();
		}

		public override void SetupRootCollider()
		{
		}

		public void HideAllElements()
		{
			this.regularTextBoxGroup.Visible = false;
			this.storeTextBoxGroup.Visible = false;
			this.nextButton.Visible = false;
			this.infoPanel.Visible = false;
			this.infoLabel.Visible = false;
			this.infoTitleLabel.Visible = false;
			this.storeButton.Visible = false;
		}

		public void ShowInfoPanel(string graphicId, string text, string title, bool planetPanel)
		{
			this.infoPanel.Visible = true;
			if (planetPanel)
			{
				this.infoPlanetTexture.LoadTexture(graphicId);
				this.infoPlanetTexture.Visible = true;
				this.infoTexture.Visible = false;
			}
			else
			{
				this.infoTexture.LoadTexture(graphicId);
				this.infoTexture.Visible = true;
				this.infoPlanetTexture.Visible = false;
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.infoLabel.Visible = true;
				this.infoLabel.Text = this.lang.Get(text, new object[0]);
			}
			else
			{
				this.infoLabel.Visible = false;
			}
			if (!string.IsNullOrEmpty(title))
			{
				this.infoTitleLabel.Visible = true;
				this.infoTitleLabel.Text = this.lang.Get(title, new object[0]);
				return;
			}
			this.infoTitleLabel.Visible = false;
		}

		public void HideInfoPanel()
		{
			this.infoPanel.Visible = false;
			this.infoLabel.Visible = false;
			this.infoTitleLabel.Visible = false;
			this.DestroyIfEmpty();
		}

		public bool CharacterAlreadyShowing(string characterId)
		{
			return this.currentCharacter != null && this.currentCharacter.CharacterId == characterId;
		}

		public void ShowHoloCharacter(string characterId)
		{
			if (!base.IsLoaded())
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Cannot display {0} because screen is not loaded yet!", new object[]
				{
					characterId
				});
				return;
			}
			Service.Get<UXController>().HUD.Visible = false;
			if (this.currentCharacter != null)
			{
				this.currentCharacter.ChangeCharacter(characterId);
				return;
			}
			Vector3 vector = this.holoPositioner.LocalPosition;
			vector = base.UXCamera.Camera.ScreenToViewportPoint(vector);
			this.currentCharacter = new HoloCharacter(characterId, vector);
		}

		public void PlayHoloAnimation(string animName)
		{
			if (this.currentCharacter != null)
			{
				this.currentCharacter.Animate(animName);
				return;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("There is no character currently on screen.", new object[0]);
		}

		public void CloseAndDestroyHoloCharacter()
		{
			if (this.currentCharacter != null)
			{
				this.currentCharacter.CloseAndDestroy(new HolocommScreen.HoloCallback(this.OnCharacterAnimatedAway));
			}
		}

		private void OnCharacterAnimatedAway()
		{
			this.currentCharacter = null;
			this.DestroyIfEmpty();
		}

		private void DestroyCharacterImmediately()
		{
			if (this.currentCharacter != null)
			{
				this.currentCharacter.Destroy();
				this.currentCharacter = null;
			}
		}

		public void AddDialogue(string text, string title)
		{
			UXLabel uXLabel;
			UXElement uXElement;
			UXLabel uXLabel2;
			this.FindLabelAndPanelForSide(out uXLabel, out uXElement, out uXLabel2);
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = this.lang.Get(text, new object[0]);
				uXLabel.Text = (string.IsNullOrEmpty(text2) ? text : text2);
				uXElement.Visible = true;
			}
			if (!string.IsNullOrEmpty(title))
			{
				string text3 = this.lang.Get(title, new object[0]);
				uXLabel2.Text = text3;
				uXElement.Visible = true;
			}
		}

		public void RemoveDialogue()
		{
			UXLabel uXLabel;
			UXElement uXElement;
			UXLabel uXLabel2;
			this.FindLabelAndPanelForSide(out uXLabel, out uXElement, out uXLabel2);
			uXLabel.Text = "";
			uXLabel2.Text = "";
			uXElement.Visible = false;
			this.DestroyIfEmpty();
		}

		private void FindLabelAndPanelForSide(out UXLabel label, out UXElement panel, out UXLabel title)
		{
			if (this.storeButton.Visible)
			{
				label = this.storeBodyLabel;
				panel = this.storeTextBoxGroup;
				title = this.storeTitleLabel;
				return;
			}
			label = this.regularBodyLabel;
			panel = this.regularTextBoxGroup;
			title = this.regularTitleLabel;
		}

		public void ShowButton(string buttonType)
		{
			UXButton uXButton = this.nextButton;
			if (buttonType == "BtnNext")
			{
				this.storeButton.Visible = false;
				uXButton = this.nextButton;
			}
			else if (buttonType == "ButtonStore")
			{
				this.nextButton.Visible = false;
				uXButton = this.storeButton;
			}
			uXButton.Visible = true;
		}

		public void OnNextButtonClicked(UXButton button)
		{
			Service.Get<EventManager>().SendEvent(EventId.StoryNextButtonClicked, null);
		}

		public Camera GetHoloCamera()
		{
			if (this.currentCharacter != null)
			{
				return this.currentCharacter.Camera;
			}
			return null;
		}

		private void DestroyIfEmpty()
		{
			if (this.currentCharacter == null && !this.storeTextBoxGroup.Visible && !this.regularTextBoxGroup.Visible && !this.infoPanel.Visible)
			{
				base.DestroyScreen();
			}
		}

		public override void OnDestroyElement()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ShowHologramComplete);
			Service.Get<UserInputManager>().UnregisterObserver(this, UserInputLayer.Screen);
			Service.Get<EventManager>().SendEvent(EventId.HoloCommScreenDestroyed, null);
			this.DestroyCharacterImmediately();
			base.OnDestroyElement();
		}

		public EatResponse OnPress(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (!base.IsLoaded() || Service.Get<ScreenController>().GetHighestLevelScreen<ScreenBase>() != this)
			{
				return EatResponse.NotEaten;
			}
			if (this.nextButton.Visible)
			{
				this.OnNextButtonClicked(this.nextButton);
				return EatResponse.Eaten;
			}
			if (this.storeButton.Visible)
			{
				this.OnNextButtonClicked(this.nextButton);
				return EatResponse.Eaten;
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnDrag(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			return EatResponse.NotEaten;
		}

		public EatResponse OnRelease(int id)
		{
			return EatResponse.NotEaten;
		}

		public EatResponse OnScroll(float delta, Vector2 screenPosition)
		{
			return EatResponse.NotEaten;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ShowHologramComplete)
			{
				Service.Get<Engine>().ForceGarbageCollection(null);
			}
			return EatResponse.NotEaten;
		}

		protected internal HolocommScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).AddDialogue(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).CharacterAlreadyShowing(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).CloseAndDestroyHoloCharacter();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).DestroyCharacterImmediately();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).DestroyIfEmpty();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).GetHoloCamera());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).HasCharacterShowing());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).HideAllElements();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).HideInfoPanel();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).InitElements();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).OnCharacterAnimatedAway();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).OnNextButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).OnRelease(*(int*)args));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args, *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).PlayHoloAnimation(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).RemoveDialogue();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).ResizeCharacter();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).ResizeCurrentCharacter(*(int*)args, *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).SetupRootCollider();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).ShowButton(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).ShowHoloCharacter(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((HolocommScreen)GCHandledObjects.GCHandleToObject(instance)).ShowInfoPanel(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(sbyte*)(args + 3) != 0);
			return -1L;
		}
	}
}
