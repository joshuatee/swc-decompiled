using StaRTS.Assets;
using StaRTS.Externals.EnvironmentManager;
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
			Service.Get<EnvironmentController>().SetupAutoRotation();
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
			this.startup.AddTask(new TapjoyStartupTask(27f));
			this.startup.AddTask(new PreloadStartupTask(29f));
			this.startup.AddTask(new GameDataStartupTask(30f));
			this.startup.AddTask(new AudioStartupTask(35f));
			this.startup.AddTask(new DamageStartupTask(40f));
			this.startup.AddTask(new LangStartupTask(50f));
			this.startup.AddTask(new UserInputStartupTask(53f));
			this.startup.AddTask(new ShowLoadingScreenPopupsStartupTask(60f));
			this.startup.AddTask(new BoardStartupTask(65f));
			this.startup.AddTask(new GeneralStartupTask(70f));
			this.startup.AddTask(new WorldStartupTask(75f));
			this.startup.AddTask(new DonePreloadingStartupTask(95f));
			this.startup.AddTask(new PlacementStartupTask(96f));
			this.startup.AddTask(new HomeStartupTask(97f));
			this.startup.AddTask(new EditBaseStartupTask(98f));
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
	}
}
