using StaRTS.Assets;
using StaRTS.Externals.GameServices;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Controllers.Startup;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Player;
using StaRTS.Main.RUF;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.GameStates
{
	public class ApplicationLoadState : IGameState, IState
	{
		private LoadingScreen loadingScreen;

		private StartupTaskController startup;

		public void OnEnter()
		{
			new WWWManager();
			new EventManager();
			LangUtils.CreateLangService();
			new CameraManager();
			Service.Get<CameraManager>().MainCamera.Camera.enabled = false;
			new AssetManager();
			this.loadingScreen = new LoadingScreen();
			this.loadingScreen.Visible = true;
			this.InitStartupTasks();
		}

		public void OnExit(IState nextState)
		{
			Service.Get<CameraManager>().MainCamera.Camera.enabled = true;
			this.loadingScreen.Fade();
			this.loadingScreen = null;
			this.KillStartup();
		}

		public void KillStartup()
		{
			if (this.startup != null)
			{
				this.startup.KillStartup();
				this.startup = null;
			}
		}

		private void InitStartupTasks()
		{
			this.startup = new StartupTaskController(new StartupTaskProgress(this.OnStartupTaskProgress), new StartupTaskComplete(this.OnStartupTaskComplete));
			this.startup.AddTask(new StaticInitStartupTask(0f));
			this.startup.AddTask(new SchedulingStartupTask(10f));
			this.startup.AddTask(new BIStartupTask(12f));
			this.startup.AddTask(new ExternalsStartupTask(13f));
			this.startup.AddTask(new ServerStartupTask(15f));
			this.startup.AddTask(new EndpointStartupTask(16f));
			this.startup.AddTask(new PlayerIdentityStartupTask(17f));
			this.startup.AddTask(new PlayerLoginStartupTask(18f));
			this.startup.AddTask(new PlayerContentStartupTask(22f));
			this.startup.AddTask(new PlayerSquadStartupTask(23f));
			this.startup.AddTask(new AssetStartupTask(25f));
			this.startup.AddTask(new TestsStartupTask(26f));
			this.startup.AddTask(new PreloadStartupTask(29f));
			this.startup.AddTask(new GameDataStartupTask(30f));
			this.startup.AddTask(new AudioStartupTask(35f));
			this.startup.AddTask(new DamageStartupTask(40f));
			this.startup.AddTask(new LangStartupTask(45f));
			this.startup.AddTask(new UserInputStartupTask(47f));
			this.startup.AddTask(new ShowLoadingScreenPopupsStartupTask(50f));
			this.startup.AddTask(new BoardStartupTask(55f));
			this.startup.AddTask(new GeneralStartupTask(60f));
			this.startup.AddTask(new WorldStartupTask(65f));
			this.startup.AddTask(new DonePreloadingStartupTask(70f));
			this.startup.AddTask(new PlacementStartupTask(75f));
			this.startup.AddTask(new HomeStartupTask(80f));
			this.startup.AddTask(new EditBaseStartupTask(85f));
		}

		public void OnStartupTaskProgress(float percentage, string description)
		{
			this.loadingScreen.Progress(percentage, description);
		}

		private void OnStartupTaskComplete()
		{
			Service.Get<SquadController>().ServerManager.EnablePolling();
			GameServicesManager.Startup();
			if (!Service.Get<CurrentPlayer>().HasNotCompletedFirstFueStep())
			{
				GameServicesManager.OnReady();
			}
			if (!Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				Service.Get<ISocialDataController>().CheckFacebookLoginOnStartup();
			}
			Service.Get<WorldInitializer>().View.StartMapManipulation();
			Service.Get<UserInputManager>().Enable(true);
			Service.Get<UXController>().HUD.ReadyToToggleVisiblity = true;
			Service.Get<RUFManager>().PrepareReturningUserFlow();
			Service.Get<EventManager>().SendEvent(EventId.StartupTasksCompleted, null);
		}

		public bool CanUpdateHomeContracts()
		{
			return false;
		}

		public ApplicationLoadState()
		{
		}

		protected internal ApplicationLoadState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ApplicationLoadState)GCHandledObjects.GCHandleToObject(instance)).CanUpdateHomeContracts());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ApplicationLoadState)GCHandledObjects.GCHandleToObject(instance)).InitStartupTasks();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ApplicationLoadState)GCHandledObjects.GCHandleToObject(instance)).KillStartup();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ApplicationLoadState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ApplicationLoadState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ApplicationLoadState)GCHandledObjects.GCHandleToObject(instance)).OnStartupTaskComplete();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ApplicationLoadState)GCHandledObjects.GCHandleToObject(instance)).OnStartupTaskProgress(*(float*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}
	}
}
