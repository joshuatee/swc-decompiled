using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Holonet;
using StaRTS.Main.Models;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class HolonetScreen : ClosableScreen, IEventObserver
	{
		private AbstractHolonetTab commandCenterTab;

		private AbstractHolonetTab videosTab;

		private AbstractHolonetTab devNotesTab;

		private AbstractHolonetTab transmissionsTab;

		private TransmissionsHolonetPopupView incommingTransmissions;

		private HolonetAnimationController anims;

		private int commandCenterCount;

		private int videosCount;

		private int devNotesCount;

		private int transmissionsCount;

		private UXElement incomingTransmissionsGroup;

		private List<AbstractHolonetTab> tabs;

		private AbstractHolonetTab currentTab;

		private EventManager eventManager;

		public UXTable NavTable
		{
			get;
			private set;
		}

		public AbstractHolonetTab PreviousTab
		{
			get;
			private set;
		}

		protected override bool WantTransitions
		{
			get
			{
				return base.IsClosing;
			}
		}

		public HolonetScreen(int commandCenterCount, int videosCount, int devNotesCount, int transmissionsCount) : base("gui_holonet")
		{
			this.allowClose = false;
			this.commandCenterCount = commandCenterCount;
			this.videosCount = videosCount;
			this.devNotesCount = devNotesCount;
			this.transmissionsCount = transmissionsCount;
		}

		protected override void OnScreenLoaded()
		{
			this.anims = new HolonetAnimationController(this);
			this.eventManager = Service.Get<EventManager>();
			this.InitButtons();
			this.tabs = new List<AbstractHolonetTab>();
			this.incomingTransmissionsGroup = base.GetElement<UXElement>("IncomingTransmissionsGroup");
			this.incomingTransmissionsGroup.Visible = false;
			this.NavTable = base.GetElement<UXTable>("NavTable");
			this.NavTable.SetTemplateItem("NavItem");
			this.commandCenterTab = new CommandCenterHolonetTab(this, HolonetControllerType.CommandCenter);
			this.tabs.Add(this.commandCenterTab);
			if (GameConstants.IsMakerVideoEnabled())
			{
				this.videosTab = new VideosHolonetTab(this, HolonetControllerType.Videos, this.anims);
				this.tabs.Add(this.videosTab);
			}
			else
			{
				this.videosTab = new DeadHolonetTab(this, HolonetControllerType.Videos, "MakerTab");
			}
			this.devNotesTab = new DevNotesHolonetTab(this, HolonetControllerType.DevNotes);
			this.tabs.Add(this.devNotesTab);
			this.transmissionsTab = new TransmissionsHolonetTab(this, HolonetControllerType.Transmissions);
			this.tabs.Add(this.transmissionsTab);
			this.commandCenterTab.SetBadgeCount(this.commandCenterCount);
			if (GameConstants.IsMakerVideoEnabled())
			{
				this.videosTab.SetBadgeCount(this.videosCount);
			}
			this.devNotesTab.SetBadgeCount(this.devNotesCount);
			this.transmissionsTab.SetBadgeCount(this.transmissionsCount);
			Service.Get<EventManager>().RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
			HolonetController holonetController = Service.Get<HolonetController>();
			int inCommingTransmissionCount = holonetController.TransmissionsController.GetInCommingTransmissionCount();
			bool flag = inCommingTransmissionCount > 0;
			if (!holonetController.CommandCenterController.HasNewPromoEntry() && (flag || holonetController.HasNewBattles()))
			{
				holonetController.SetLastViewed(this.transmissionsTab);
				this.incommingTransmissions = new TransmissionsHolonetPopupView(this);
				this.anims.OpenToIncomingTransmission();
			}
			else
			{
				this.ShowDefaultTabs();
				this.anims.OpenToCommandCenter();
			}
			this.eventManager.SendEvent(EventId.HolonetOpened, null);
			this.allowClose = true;
		}

		public void SetBackButtonToDefault()
		{
			base.InitDefaultBackDelegate();
		}

		public void SetBackButtonForVideoPostView(UXButton backButton, UXButtonClickedDelegate backDelegate)
		{
			base.CurrentBackDelegate = backDelegate;
			base.CurrentBackButton = backButton;
		}

		public void EnableAllTabButtons()
		{
			if (this.tabs != null)
			{
				int count = this.tabs.Count;
				for (int i = 0; i < count; i++)
				{
					this.tabs[i].EnableTabButton();
				}
			}
		}

		public void DisableAllTabButtons()
		{
			if (this.tabs != null)
			{
				int count = this.tabs.Count;
				for (int i = 0; i < count; i++)
				{
					this.tabs[i].DisableTabButton();
				}
			}
		}

		public void ShowDefaultTabs()
		{
			this.OpenTab(HolonetControllerType.CommandCenter);
		}

		public void ShowVideoPreView()
		{
			if (this.videosTab != null)
			{
				this.OpenTab(HolonetControllerType.Videos);
				((VideosHolonetTab)this.videosTab).ShowPreViewPage(true);
			}
		}

		public void ShowMoreVideos()
		{
			if (this.videosTab != null)
			{
				this.OpenTab(HolonetControllerType.Videos);
				((VideosHolonetTab)this.videosTab).ShowSearchPage(null);
			}
		}

		public void HideVideoPreView()
		{
			if (this.videosTab != null)
			{
				this.OpenTab(HolonetControllerType.Videos);
				((VideosHolonetTab)this.videosTab).HidePreViewPage();
			}
		}

		public void ShowVideoPostView(string guid)
		{
			if (this.videosTab != null)
			{
				this.OpenTab(HolonetControllerType.Videos);
				((VideosHolonetTab)this.videosTab).ShowPostViewPage(guid, true);
			}
		}

		public override void OnDestroyElement()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.GameStateChanged);
			int i = 0;
			int count = this.tabs.Count;
			while (i < count)
			{
				this.tabs[i].OnDestroyTab();
				i++;
			}
			this.tabs.Clear();
			this.tabs = null;
			if (this.incommingTransmissions != null)
			{
				this.incommingTransmissions.CleanUp();
			}
			if (this.NavTable != null)
			{
				this.NavTable.Clear();
				this.NavTable = null;
			}
			this.anims.Cleanup();
			this.anims = null;
			base.OnDestroyElement();
			this.Visible = false;
		}

		public void HideAllTabs()
		{
			int i = 0;
			int count = this.tabs.Count;
			while (i < count)
			{
				this.tabs[i].OnTabClose();
				i++;
			}
		}

		public void OpenTab(HolonetControllerType tabType)
		{
			if (this.tabs == null)
			{
				return;
			}
			if (this.currentTab != null && this.currentTab.HolonetControllerType != tabType)
			{
				this.PreviousTab = this.currentTab;
			}
			int i = 0;
			int count = this.tabs.Count;
			while (i < count)
			{
				if (this.currentTab == null || (this.tabs[i].HolonetControllerType == tabType && this.tabs[i].HolonetControllerType != this.currentTab.HolonetControllerType))
				{
					this.currentTab = this.tabs[i];
					this.currentTab.OnTabOpen();
					if (this.PreviousTab != null && this.PreviousTab != this.currentTab)
					{
						this.eventManager.SendEvent(EventId.HolonetTabClosed, this.PreviousTab.GetBITabName());
					}
					this.eventManager.SendEvent(EventId.HolonetTabOpened, this.currentTab.GetBITabName());
				}
				i++;
			}
			if (this.PreviousTab != null && this.PreviousTab != this.currentTab)
			{
				this.PreviousTab.OnTabClose();
			}
			if (this.currentTab == this.commandCenterTab)
			{
				this.anims.ShowCommandCenter();
				return;
			}
			if (this.currentTab == this.devNotesTab)
			{
				this.anims.ShowDevNotes();
				return;
			}
			if (this.currentTab == this.videosTab)
			{
				this.anims.ShowVideos();
				return;
			}
			if (this.currentTab == this.transmissionsTab)
			{
				this.anims.ShowTransmissionLog();
			}
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.GameStateChanged)
			{
				IState currentState = Service.Get<GameStateMachine>().CurrentState;
				if (!(currentState is HomeState) && !(currentState is VideoPlayBackState))
				{
					this.Close(null);
				}
			}
			return base.OnEvent(id, cookie);
		}

		public override void Close(object modalResult)
		{
			base.Close(modalResult);
			string cookie = string.Empty;
			HolonetController holonetController = Service.Get<HolonetController>();
			if (holonetController.TransmissionsController.IsTransmissionPopupOpened() && this.incommingTransmissions != null)
			{
				cookie = this.incommingTransmissions.GetBITabName();
			}
			else if (this.currentTab != null)
			{
				cookie = this.currentTab.GetBITabName();
			}
			string cookie2 = (this.currentTab != null) ? this.currentTab.GetBITabName() : string.Empty;
			this.eventManager.SendEvent(EventId.HolonetTabClosed, cookie2);
			this.eventManager.SendEvent(EventId.HolonetClosed, cookie);
		}

		protected internal HolonetScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).DisableAllTabButtons();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).EnableAllTabButtons();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).NavTable);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).PreviousTab);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).WantTransitions);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).HideAllTabs();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).HideVideoPreView();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).OpenTab((HolonetControllerType)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).NavTable = (UXTable)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).PreviousTab = (AbstractHolonetTab)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).SetBackButtonForVideoPostView((UXButton)GCHandledObjects.GCHandleToObject(*args), (UXButtonClickedDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).SetBackButtonToDefault();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).ShowDefaultTabs();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).ShowMoreVideos();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).ShowVideoPostView(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((HolonetScreen)GCHandledObjects.GCHandleToObject(instance)).ShowVideoPreView();
			return -1L;
		}
	}
}
