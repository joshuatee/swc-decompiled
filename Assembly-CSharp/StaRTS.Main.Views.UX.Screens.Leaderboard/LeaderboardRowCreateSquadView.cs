using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Leaderboard
{
	public class LeaderboardRowCreateSquadView : AbstractLeaderboardRowView
	{
		private const string CREATE_BUTTON = "BtnConnect";

		private const string CREATE_BUTTON_LABEL = "LabelBtnConnect";

		private const string CREATE_DESC_LABEL = "LabelConnectFacebook";

		private const string CREATE_ROW_ICON = "SpriteFacebookIcon";

		private const string PREFIX = "create_squad_";

		private const string SQUAD_CREATE = "SQUAD_CREATE";

		private const string SQUAD_CREATE_DESC = "SQUAD_CREATE_DESC";

		private const string IN_WAR_CANT_LEAVE_SQUAD = "IN_WAR_CANT_LEAVE_SQUAD";

		public LeaderboardRowCreateSquadView(AbstractLeaderboardScreen screen, UXGrid grid, UXElement templateItem, int position) : base(screen, grid, templateItem, SocialTabs.Empty, FactionToggle.All, position, false)
		{
			this.InitView();
		}

		protected override void CreateItem()
		{
			this.id = string.Format("{0}{1}", new object[]
			{
				"create_squad_",
				this.position
			});
			this.item = this.grid.CloneItem(this.id, this.templateItem);
		}

		protected void InitView()
		{
			UXButton subElement = this.grid.GetSubElement<UXButton>(this.id, "BtnConnect");
			subElement.OnClicked = new UXButtonClickedDelegate(this.OnCreateSquadClicked);
			UXLabel subElement2 = this.grid.GetSubElement<UXLabel>(this.id, "LabelBtnConnect");
			subElement2.Text = Service.Get<Lang>().Get("SQUAD_CREATE", new object[0]);
			UXLabel subElement3 = this.grid.GetSubElement<UXLabel>(this.id, "LabelConnectFacebook");
			subElement3.Text = Service.Get<Lang>().Get("SQUAD_CREATE_DESC", new object[0]);
			UXSprite subElement4 = this.grid.GetSubElement<UXSprite>(this.id, "SpriteFacebookIcon");
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			if (faction == FactionType.Empire)
			{
				subElement4.SpriteName = "FactionEmpire";
				return;
			}
			if (faction != FactionType.Rebel)
			{
				return;
			}
			subElement4.SpriteName = "FactionRebel";
		}

		private void OnCreateSquadClicked(UXButton button)
		{
			if (!SquadUtils.CanLeaveSquad())
			{
				string message = Service.Get<Lang>().Get("IN_WAR_CANT_LEAVE_SQUAD", new object[0]);
				AlertScreen.ShowModal(false, null, message, null, null, true);
			}
			else
			{
				Service.Get<ScreenController>().AddScreen(new SquadCreateScreen(true));
			}
			this.screen.Close(null);
		}

		public override void Destroy()
		{
		}

		protected internal LeaderboardRowCreateSquadView(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((LeaderboardRowCreateSquadView)GCHandledObjects.GCHandleToObject(instance)).CreateItem();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((LeaderboardRowCreateSquadView)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((LeaderboardRowCreateSquadView)GCHandledObjects.GCHandleToObject(instance)).InitView();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LeaderboardRowCreateSquadView)GCHandledObjects.GCHandleToObject(instance)).OnCreateSquadClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
