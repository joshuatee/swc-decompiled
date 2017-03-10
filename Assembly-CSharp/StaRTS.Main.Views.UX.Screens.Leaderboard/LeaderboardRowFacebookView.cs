using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Leaderboard
{
	public class LeaderboardRowFacebookView : AbstractLeaderboardRowView
	{
		private const string SPRITE = "SpriteFacebookIcon";

		private const string LABEL = "LabelConnectFacebook";

		private const string BUTTON = "BtnConnect";

		private const string BUTTON_LABEL = "LabelBtnConnect";

		private const string FACEBOOK_PREFIX = "facebook_";

		private const string CONNECT_FB_DESC = "CONNECT_FB_DESC";

		private const string CONNECT_FB_INCENTIVE_DESC = "CONNECT_FB_INCENTIVE_DESC";

		private const string SETTINGS_NOTCONNECTED = "SETTINGS_NOTCONNECTED";

		private const string INVITE_FB_DESC = "INVITE_FB_DESC";

		private const string INVITE_FRIENDS = "INVITE_FRIENDS";

		private UXSprite sprite;

		private UXLabel label;

		private UXButton button;

		private UXLabel buttonLabel;

		public LeaderboardRowFacebookView(AbstractLeaderboardScreen screen, UXGrid grid, UXElement templateItem, SocialTabs tab) : base(screen, grid, templateItem, tab, FactionToggle.All, 0, false)
		{
			this.InitView();
		}

		protected override void CreateItem()
		{
			this.id = string.Format("{0}{1}", new object[]
			{
				"facebook_",
				this.tab.ToString()
			});
			this.item = this.grid.CloneItem(this.id, this.templateItem);
		}

		private void InitView()
		{
			Lang lang = Service.Get<Lang>();
			this.sprite = this.grid.GetSubElement<UXSprite>(this.id, "SpriteFacebookIcon");
			this.label = this.grid.GetSubElement<UXLabel>(this.id, "LabelConnectFacebook");
			this.button = this.grid.GetSubElement<UXButton>(this.id, "BtnConnect");
			this.buttonLabel = this.grid.GetSubElement<UXLabel>(this.id, "LabelBtnConnect");
			if (!Service.Get<ISocialDataController>().IsLoggedIn)
			{
				string text;
				if (Service.Get<CurrentPlayer>().IsConnectedAccount)
				{
					text = lang.Get("CONNECT_FB_DESC", new object[0]);
				}
				else
				{
					text = lang.Get("CONNECT_FB_INCENTIVE_DESC", new object[]
					{
						GameConstants.FB_CONNECT_REWARD
					});
				}
				this.label.Text = text;
				this.buttonLabel.Text = lang.Get("SETTINGS_NOTCONNECTED", new object[0]);
				this.button.OnClicked = new UXButtonClickedDelegate(this.ConnectToFacebook);
				return;
			}
			if (GameConstants.FACEBOOK_INVITES_ENABLED)
			{
				this.label.Text = lang.Get("INVITE_FB_DESC", new object[0]);
				this.buttonLabel.Text = lang.Get("INVITE_FRIENDS", new object[0]);
				this.button.OnClicked = new UXButtonClickedDelegate(this.FacebookInviteFriends);
				return;
			}
			this.label.Visible = false;
			this.buttonLabel.Visible = false;
			this.button.Visible = false;
			this.sprite.Visible = false;
		}

		public void Hide()
		{
			this.sprite.Visible = false;
			this.label.Visible = false;
			this.button.Visible = false;
		}

		private void ConnectToFacebook(UXButton btn)
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadFB, null);
			Service.Get<ISocialDataController>().Login(new OnAllDataFetchedDelegate(this.screen.OnFacebookLoggedIn));
		}

		private void FacebookInviteFriends(UXButton btn)
		{
			Service.Get<EventManager>().SendEvent(EventId.SquadFB, null);
			Service.Get<ISocialDataController>().InviteFriends(null);
		}

		public override void Destroy()
		{
		}

		protected internal LeaderboardRowFacebookView(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((LeaderboardRowFacebookView)GCHandledObjects.GCHandleToObject(instance)).ConnectToFacebook((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((LeaderboardRowFacebookView)GCHandledObjects.GCHandleToObject(instance)).CreateItem();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((LeaderboardRowFacebookView)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LeaderboardRowFacebookView)GCHandledObjects.GCHandleToObject(instance)).FacebookInviteFriends((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LeaderboardRowFacebookView)GCHandledObjects.GCHandleToObject(instance)).Hide();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((LeaderboardRowFacebookView)GCHandledObjects.GCHandleToObject(instance)).InitView();
			return -1L;
		}
	}
}
