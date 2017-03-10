using StaRTS.Audio;
using StaRTS.Externals.EnvironmentManager;
using StaRTS.Externals.GameServices;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Configs;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Notifications;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
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
	public class SettingsScreen : ClosableScreen, IEventObserver
	{
		private const string TITLE_LABEL = "LabelSettingsTitle";

		public const string MUSIC_BUTTON = "BtnMusic";

		private const string FORUMS_BUTTON = "BtnForums";

		public const string SFX_BUTTON = "BtnSoundEffects";

		public const string LANGUAGE_BUTTON = "BtnLanguage";

		public const string FACEBOOK_BUTTON = "BtnFacebook";

		public const string GOOGLE_BUTTON = "BtnGoogle";

		public const string GOOGLE_BUTTON_LABEL = "LabelBtnGoogle";

		public const string LOGIN_GROUP = "Login";

		private const string FACEBOOK_INCENTIVE_GROUP = "FBIncentive";

		public const string ABOUT_BUTTON = "BtnAbout";

		public const string HELP_BUTTON = "BtnHelp";

		public const string PRIVACY_BUTTON = "BtnPrivacyPolicy";

		public const string TOS_BUTTON = "BtnTOS";

		private const string MAIN_GROUP = "DefaultContainer";

		private const string ABOUT_GROUP = "AboutContainer";

		private const string ABOUT_TITLE_LABEL = "LabelAboutTitle";

		private const string ABOUT_BACK_BUTTON = "BtnBackAbout";

		private const string ABOUT_PLAYERID_LABEL = "LabelPlayerID";

		private const string ABOUT_VERSION_LABEL = "LabelVersion";

		private const string ABOUT_COPYRIGHT_LABEL = "LabelCopyright";

		private const string ABOUT_TRADEMARK_LABEL = "LabelTrademark";

		private const string LANG_GROUP = "LanguageContainer";

		private const string LANG_BACK_BUTTON = "BtnBack";

		private const string LANG_TITLE_LABEL = "LabelLanguageTitle";

		private const string LANG_GRID = "LanguageBtnGrid";

		private const string LANG_ITEM_TEMPLATE = "BtnLanguageSelect";

		private const string LANG_ITEM_LABEL = "LabelLanguageSelect";

		private const string LABEL_PREFIX = "Label";

		private const string LABEL_FB_INCENTIVE = "LabelFBIncentive";

		private const string LABEL_CRYSTALS = "LabelCrystals";

		private const string LABEL_CRYSTAL_COUNT = "LabelCrystalCount";

		private const string SHOW_ACHIEVEMENTS_BUTTON = "BtnAchievements";

		private const string FACTION_FLIP_BUTTON = "BtnFactionSwap";

		private const string FACTION_FLIP_LABEL = "LabelBtnFactionSwap";

		private const string FACTION_FLIP_SPRITE = "SpriteBkgBtnFactionSwap";

		private const string FACTION_EMPIRE_ICON = "btnEmpire";

		private const string FACTION_REBEL_ICON = "btnRebel";

		private const string OS_ANDROID = "Google";

		private const string OS_IOS = "iOS";

		private const string OS_NAME = "iOS";

		private const float LANGUAGE_CHANGE_DELAY = 0.5f;

		private UXElement mainGroup;

		private UXElement langGroup;

		private UXElement aboutGroup;

		private UXGrid langGrid;

		private UXButton aboutBackButton;

		private UXButton langBackButton;

		private bool achievementsClicked;

		public SettingsScreen() : base("gui_settings")
		{
		}

		public override void OnDestroyElement()
		{
			if (this.langGrid != null)
			{
				this.langGrid.Clear();
				this.langGrid = null;
			}
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.GameServicesSignedIn);
			eventManager.UnregisterObserver(this, EventId.GameServicesSignedOut);
			eventManager.UnregisterObserver(this, EventId.FacebookLoggedOut);
			base.OnDestroyElement();
		}

		protected override void OnScreenLoaded()
		{
			this.InitGroups();
			this.InitLabels();
			this.InitButtons();
			this.InitAbout();
			this.InitLanguages();
			this.CheckIfRestrictedUser();
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.GameServicesSignedIn, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.GameServicesSignedOut, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.FacebookLoggedOut, EventPriority.Default);
		}

		private void InitGroups()
		{
			base.GetElement<UXElement>("iOSLogin").Visible = false;
			base.GetElement<UXElement>("GoogleLogin").Visible = false;
			base.GetElement<UXElement>("iOSLogin").Visible = true;
		}

		private void InitLabels()
		{
			this.mainGroup = base.GetElement<UXElement>("DefaultContainer");
			this.mainGroup.Visible = true;
			UXLabel element = base.GetElement<UXLabel>("LabelSettingsTitle");
			element.Text = this.lang.Get("SETTINGS_TITLE", new object[0]);
			element = base.GetElement<UXLabel>("LabelBtnMusic");
			element.Text = this.lang.Get("SETTINGS_MUSIC", new object[0]);
			element = base.GetElement<UXLabel>("LabelBtnSoundEffects");
			element.Text = this.lang.Get("SETTINGS_SFX", new object[0]);
			element = base.GetElement<UXLabel>("LabelBtnLanguage");
			element.Text = this.lang.GetDisplayLanguage(this.lang.Locale);
			this.SetFacebookButtonLabel(Service.Get<ISocialDataController>().IsLoggedIn);
			element = base.GetElement<UXLabel>("LabelBtnAbout");
			element.Text = this.lang.Get("SETTINGS_ABOUT", new object[0]);
			element = base.GetElement<UXLabel>("LabelBtnHelp");
			element.Text = this.lang.Get("SETTINGS_HELP", new object[0]);
			element = base.GetElement<UXLabel>("LabelBtnPrivacyPolicy");
			element.Text = this.lang.Get("SETTINGS_PRIVACY", new object[0]);
			element = base.GetElement<UXLabel>("LabelBtnTOS");
			element.Text = this.lang.Get("SETTINGS_TOS", new object[0]);
			if (GameConstants.FORUMS_ENABLED)
			{
				element = base.GetElement<UXLabel>("LabelBtnForums");
				element.Text = this.lang.Get("SETTINGS_FORUMS", new object[0]);
			}
			element = base.GetElement<UXLabel>("LabelFBIncentiveiOS");
			element.Text = this.lang.Get("CONNECT_FB_SETTINGS_DESC", new object[0]);
			element = base.GetElement<UXLabel>("LabelBtnFactionSwap");
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			if (faction == FactionType.Empire)
			{
				element.Text = this.lang.Get("FACTION_FLIP_PLAY_REBEL", new object[0]);
				return;
			}
			if (faction != FactionType.Rebel)
			{
				return;
			}
			element.Text = this.lang.Get("FACTION_FLIP_PLAY_EMPIRE", new object[0]);
		}

		protected override void InitButtons()
		{
			base.InitButtons();
			UXCheckbox element = base.GetElement<UXCheckbox>("BtnMusic");
			element.OnSelected = new UXCheckboxSelectedDelegate(this.OnMusicCheckboxSelected);
			element.Selected = (PlayerSettings.GetMusicVolume() > 0f);
			element = base.GetElement<UXCheckbox>("BtnSoundEffects");
			element.OnSelected = new UXCheckboxSelectedDelegate(this.OnSfxCheckboxSelected);
			element.Selected = (PlayerSettings.GetSfxVolume() > 0f);
			UXButton element2 = base.GetElement<UXButton>("BtnLanguage");
			element2.OnClicked = new UXButtonClickedDelegate(this.OnLanguageButtonClicked);
			element2 = base.GetElement<UXButton>("BtnFacebookiOS");
			element2.OnClicked = new UXButtonClickedDelegate(this.OnFacebookButtonClicked);
			element2 = base.GetElement<UXButton>("BtnAbout");
			element2.OnClicked = new UXButtonClickedDelegate(this.OnAboutButtonClicked);
			element2 = base.GetElement<UXButton>("BtnHelp");
			element2.OnClicked = new UXButtonClickedDelegate(this.OnHelpButtonClicked);
			element2 = base.GetElement<UXButton>("BtnPrivacyPolicy");
			element2.OnClicked = new UXButtonClickedDelegate(this.OnPrivacyButtonClicked);
			element2 = base.GetElement<UXButton>("BtnTOS");
			element2.OnClicked = new UXButtonClickedDelegate(this.OnTermsButtonClicked);
			element2 = base.GetElement<UXButton>("BtnForums");
			if (GameConstants.FORUMS_ENABLED)
			{
				element2.OnClicked = new UXButtonClickedDelegate(this.OnForumsButtonClicked);
			}
			else
			{
				element2.Visible = false;
			}
			this.langBackButton = base.GetElement<UXButton>("BtnBack");
			this.langBackButton.OnClicked = new UXButtonClickedDelegate(this.OnBackButtonClicked);
			this.aboutBackButton = base.GetElement<UXButton>("BtnBackAbout");
			this.aboutBackButton.OnClicked = new UXButtonClickedDelegate(this.OnBackButtonClicked);
			element2 = base.GetElement<UXButton>("BtnFactionSwap");
			if (GameUtils.HasUserFactionFlipped(Service.Get<CurrentPlayer>()) && Service.Get<CurrentPlayer>().NumIdentities > 1)
			{
				element2.OnClicked = new UXButtonClickedDelegate(this.OnFactionFlipButtonClicked);
				UXSprite element3 = base.GetElement<UXSprite>("SpriteBkgBtnFactionSwap");
				FactionType faction = Service.Get<CurrentPlayer>().Faction;
				if (faction != FactionType.Empire)
				{
					if (faction == FactionType.Rebel)
					{
						element3.SpriteName = "btnEmpire";
					}
				}
				else
				{
					element3.SpriteName = "btnRebel";
				}
			}
			else
			{
				element2.Visible = false;
			}
			this.BackButtons.Add(this.langBackButton);
			this.BackButtons.Add(this.aboutBackButton);
		}

		private void CheckIfRestrictedUser()
		{
			if (Service.Get<EnvironmentController>().IsRestrictedProfile())
			{
				UXButton element = base.GetElement<UXButton>("BtnFacebookiOS");
				element.Visible = false;
				element = base.GetElement<UXButton>("BtnHelp");
				element.Visible = false;
				element = base.GetElement<UXButton>("BtnPrivacyPolicy");
				element.Visible = false;
				element = base.GetElement<UXButton>("BtnTOS");
				element.Visible = false;
				element = base.GetElement<UXButton>("BtnForums");
				element.Visible = false;
				element = base.GetElement<UXButton>("BtnAchievements");
				element.Visible = false;
				element = base.GetElement<UXButton>("BtnGoogle");
				element.Visible = false;
				UXLabel element2 = base.GetElement<UXLabel>("LabelFBIncentiveiOS");
				element2.Visible = false;
				base.GetElement<UXElement>("FBIncentiveiOS").Visible = false;
			}
		}

		private void InitAbout()
		{
			this.aboutGroup = base.GetElement<UXElement>("AboutContainer");
			this.aboutGroup.Visible = false;
			UXLabel element = base.GetElement<UXLabel>("LabelAboutTitle");
			element.Text = this.lang.Get("SETTINGS_ABOUT", new object[0]);
			UXLabel element2 = base.GetElement<UXLabel>("LabelPlayerID");
			element2.Text = this.lang.Get("SETTINGS_ABOUT_PLAYERID", new object[]
			{
				Service.Get<CurrentPlayer>().PlayerId
			});
			element2 = base.GetElement<UXLabel>("LabelVersion");
			element2.Text = this.lang.Get("SETTINGS_ABOUT_VERSION", new object[]
			{
				"4.7.0",
				"2"
			});
			element2 = base.GetElement<UXLabel>("LabelCopyright");
			element2.Text = this.lang.Get("SETTINGS_ABOUT_COPYRIGHT", new object[0]);
			element2 = base.GetElement<UXLabel>("LabelTrademark");
			element2.Text = this.lang.Get("SETTINGS_ABOUT_TRADEMARK", new object[]
			{
				Application.unityVersion,
				DateTime.get_Now().get_Year()
			});
		}

		private void InitLanguages()
		{
			this.langGroup = base.GetElement<UXElement>("LanguageContainer");
			this.langGroup.Visible = true;
			UXLabel element = base.GetElement<UXLabel>("LabelLanguageTitle");
			element.Text = this.lang.Get("SETTINGS_CHOOSELANGUAGE", new object[0]);
			this.langGrid = base.GetElement<UXGrid>("LanguageBtnGrid");
			this.langGrid.SetTemplateItem("BtnLanguageSelect");
			List<string> availableLocales = this.lang.GetAvailableLocales();
			string locale = this.lang.Locale;
			int i = 0;
			int count = availableLocales.Count;
			while (i < count)
			{
				string text = availableLocales[i];
				if (!(text == locale))
				{
					string itemUid = text;
					UXElement uXElement = this.langGrid.CloneTemplateItem(itemUid);
					UXButton uXButton = uXElement as UXButton;
					if (uXButton != null)
					{
						uXButton.OnClicked = new UXButtonClickedDelegate(this.OnLanguageItemButtonClicked);
						uXButton.Tag = text;
					}
					UXLabel subElement = this.langGrid.GetSubElement<UXLabel>(itemUid, "LabelLanguageSelect");
					if (text.ToLower().StartsWith("ko"))
					{
						string text2 = SystemInfo.operatingSystem.ToLower();
						if (text2.Contains("phone") && text2.Contains("(10."))
						{
							subElement.Font = Service.Get<Lang>().CustomKoreanFont;
						}
					}
					subElement.Text = this.lang.GetDisplayLanguage(text);
					this.langGrid.AddItem(uXElement, i);
				}
				i++;
			}
			this.langGrid.RepositionItems();
			this.langGroup.Visible = false;
		}

		private void OnMusicCheckboxSelected(UXCheckbox checkbox, bool selected)
		{
			float num = selected ? 1f : 0f;
			PlayerSettings.SetMusicVolume(num);
			Service.Get<AudioManager>().SetVolume(AudioCategory.Music, num);
			Service.Get<AudioManager>().SetVolume(AudioCategory.Ambience, num);
			Service.Get<EventManager>().SendEvent(EventId.SettingsMusicCheckboxSelected, selected);
		}

		private void OnSfxCheckboxSelected(UXCheckbox checkbox, bool selected)
		{
			float num = selected ? 1f : 0f;
			PlayerSettings.SetSfxVolume(num);
			Service.Get<AudioManager>().SetVolume(AudioCategory.Effect, num);
			Service.Get<AudioManager>().SetVolume(AudioCategory.Dialogue, num);
			Service.Get<EventManager>().SendEvent(EventId.SettingsSfxCheckboxSelected, selected);
		}

		private void OnNotificationsCheckboxSelected(UXCheckbox checkbox, bool selected)
		{
			int notificationsLevel = selected ? 100 : 0;
			PlayerSettings.SetNotificationsLevel(notificationsLevel);
			Service.Get<NotificationController>().Enabled = selected;
		}

		private void OnLanguageButtonClicked(UXButton button)
		{
			this.mainGroup.Visible = false;
			this.langGroup.Visible = true;
			base.CurrentBackButton = this.langBackButton;
			base.CurrentBackDelegate = new UXButtonClickedDelegate(this.OnBackButtonClicked);
		}

		private void OnAchievementButtonClicked(UXButton button)
		{
			if (GameServicesManager.IsUserAuthenticated())
			{
				GameServicesManager.ShowAchievements();
				return;
			}
			this.achievementsClicked = true;
			GameServicesManager.SignIn();
		}

		private void OnGoogleButtonClicked(UXButton button)
		{
			if (GameServicesManager.IsUserAuthenticated())
			{
				GameServicesManager.SignOut();
				Service.Get<IAccountSyncController>().UnregisterGameServicesAccount();
				return;
			}
			GameServicesManager.SignIn();
		}

		private void OnFacebookButtonClicked(UXButton button)
		{
			if (Service.Get<ISocialDataController>().IsLoggedIn)
			{
				Service.Get<ISocialDataController>().Logout();
				this.SetFacebookButtonLabel(false);
				Service.Get<EventManager>().SendEvent(EventId.SettingsFacebookLoggedIn, false);
				return;
			}
			Service.Get<ISocialDataController>().Login(new OnAllDataFetchedDelegate(this.OnFacebookLoggedIn));
			Service.Get<EventManager>().SendEvent(EventId.SettingsFacebookLoggedIn, true);
		}

		private void OnFacebookLoggedIn()
		{
			this.SetFacebookButtonLabel(true);
		}

		private void SetFacebookButtonLabel(bool connected)
		{
			string text = this.lang.Get(connected ? "SETTINGS_CONNECTED" : "SETTINGS_NOTCONNECTED", new object[0]);
			UXLabel element = base.GetElement<UXLabel>("LabelBtnFacebookiOS");
			element.Text = text;
			bool flag = !connected && !Service.Get<CurrentPlayer>().IsConnectedAccount;
			if (flag)
			{
				element = base.GetElement<UXLabel>("LabelCrystalCountiOS");
				element.Text = this.lang.Get("GET_AMOUNT", new object[]
				{
					GameConstants.FB_CONNECT_REWARD
				});
				element = base.GetElement<UXLabel>("LabelCrystalsiOS");
				element.Text = this.lang.Get("CRYSTALS", new object[0]);
			}
			base.GetElement<UXElement>("FBIncentiveiOS").Visible = flag;
		}

		private void SetGoogleButtonLabel(bool connected)
		{
			UXLabel element = base.GetElement<UXLabel>("LabelBtnGoogle");
			element.Text = this.lang.Get(connected ? "SETTINGS_CONNECTED" : "SETTINGS_NOTCONNECTED", new object[0]);
		}

		private void OnAboutButtonClicked(UXButton button)
		{
			this.mainGroup.Visible = false;
			this.aboutGroup.Visible = true;
			base.CurrentBackButton = this.aboutBackButton;
			base.CurrentBackDelegate = new UXButtonClickedDelegate(this.OnBackButtonClicked);
			Service.Get<EventManager>().SendEvent(EventId.SettingsAboutButtonClicked, null);
		}

		private void OnHelpButtonClicked(UXButton button)
		{
			GameUtils.OpenURL(this.lang.Get("SETTINGS_LINK_HELP", new object[0]));
			Service.Get<EventManager>().SendEvent(EventId.SettingsHelpButtonClicked, null);
		}

		private void OnPrivacyButtonClicked(UXButton button)
		{
			GameUtils.OpenURL(this.lang.Get("SETTINGS_LINK_PRIVACY", new object[0]));
		}

		private void OnTermsButtonClicked(UXButton button)
		{
			GameUtils.OpenURL(this.lang.Get("SETTINGS_LINK_TOS", new object[0]));
		}

		private void OnForumsButtonClicked(UXButton button)
		{
			Service.Get<EventManager>().SendEvent(EventId.SettingsFanForumsButtonClicked, null);
			GameUtils.OpenURL(this.lang.Get("SETTINGS_LINK_FORUMS", new object[0]));
		}

		private void OnFactionFlipButtonClicked(UXButton button)
		{
			Service.Get<ScreenController>().AddScreen(new FactionFlipScreen());
			Service.Get<EventManager>().SendEvent(EventId.UIFactionFlipOpened, "setting");
			this.Close(null);
		}

		private void OnBackButtonClicked(UXButton button)
		{
			this.langGroup.Visible = false;
			this.aboutGroup.Visible = false;
			this.mainGroup.Visible = true;
			base.InitDefaultBackDelegate();
			Service.Get<EventManager>().SendEvent(EventId.BackButtonClicked, null);
		}

		private void OnLanguageItemButtonClicked(UXButton button)
		{
			string text = (string)button.Tag;
			this.Close(null);
			bool useCustomKoreanFont = false;
			if (text.ToLower().StartsWith("ko"))
			{
				useCustomKoreanFont = true;
			}
			YesNoScreen.ShowModal(this.lang.Get("SETTINGS_CHOOSELANGUAGE", new object[0]), this.lang.Get("SETTINGS_CONFIRMLANGUAGE", new object[]
			{
				this.lang.GetDisplayLanguage(text)
			}), false, new OnScreenModalResult(this.OnLanguageChangeConfirmed), text, useCustomKoreanFont);
		}

		private void OnLanguageChangeConfirmed(object result, object cookie)
		{
			if (result == null)
			{
				return;
			}
			string cookie2 = (string)cookie;
			Service.Get<ViewTimerManager>().CreateViewTimer(0.5f, false, new TimerDelegate(this.OnLanguageChangeTimer), cookie2);
		}

		private void OnLanguageChangeTimer(uint id, object cookie)
		{
			string newValue = (string)cookie;
			Service.Get<ServerPlayerPrefs>().SetPref(ServerPref.Locale, newValue);
			Service.Get<ServerAPI>().Sync(new SetPrefsCommand(true));
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.GameServicesSignedIn)
			{
				if (id != EventId.GameServicesSignedOut)
				{
					if (id == EventId.FacebookLoggedOut)
					{
						this.SetFacebookButtonLabel(false);
					}
				}
				else if ((AccountProvider)cookie == AccountProvider.GOOGLEPLAY)
				{
					this.SetGoogleButtonLabel(false);
				}
			}
			else if ((AccountProvider)cookie == AccountProvider.GOOGLEPLAY)
			{
				this.SetGoogleButtonLabel(true);
				if (this.achievementsClicked)
				{
					this.achievementsClicked = false;
					GameServicesManager.ShowAchievements();
				}
			}
			return base.OnEvent(id, cookie);
		}

		protected internal SettingsScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).CheckIfRestrictedUser();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).InitAbout();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).InitGroups();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).InitLanguages();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnAboutButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnAchievementButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnBackButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnFacebookButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnFacebookLoggedIn();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnFactionFlipButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnForumsButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnGoogleButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnHelpButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnLanguageButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnLanguageChangeConfirmed(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnLanguageItemButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnMusicCheckboxSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnNotificationsCheckboxSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnPrivacyButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnSfxCheckboxSelected((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).OnTermsButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).SetFacebookButtonLabel(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((SettingsScreen)GCHandledObjects.GCHandleToObject(instance)).SetGoogleButtonLabel(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
