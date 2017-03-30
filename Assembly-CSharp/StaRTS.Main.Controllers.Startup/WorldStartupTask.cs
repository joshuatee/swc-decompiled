using StaRTS.FX;
using StaRTS.Main.Controllers.CombineMesh;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Controllers.World.Transitions;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Startup
{
	public class WorldStartupTask : StartupTask
	{
		private WorldController worldController;

		public WorldStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Service.Get<EventManager>().SendEvent(EventId.InitializeWorldStart, null);
			new BuildingLookupController();
			new WorldController();
			new WorldInitializer();
			new WorldPreloader();
			new BuildingController();
			new PlayerValuesController();
			new HomeMapDataLoader();
			new NpcMapDataLoader();
			new PvpMapDataLoader();
			new WarBaseMapDataLoader();
			new ReplayMapDataLoader();
			new LightingEffectsController();
			new PlanetEffectController();
			new CombineMeshManager();
			new ShaderTimeController();
			Service.Get<CurrentPlayer>().Map.InitializePlanet();
			Service.Get<CurrentPlayer>().BattleHistory.SetupBattles();
			IMapDataLoader mapDataLoader;
			if (Service.Get<CurrentPlayer>().HasNotCompletedFirstFueStep() && GameConstants.START_FUE_IN_BATTLE_MODE)
			{
				mapDataLoader = Service.Get<NpcMapDataLoader>();
				BattleTypeVO battle = Service.Get<IDataController>().Get<BattleTypeVO>(GameConstants.FUE_BATTLE);
				((NpcMapDataLoader)mapDataLoader).Initialize(battle);
				Service.Get<UXController>().HUD.Visible = false;
			}
			else
			{
				mapDataLoader = Service.Get<HomeMapDataLoader>();
			}
			Service.Get<EventManager>().SendEvent(EventId.InitializeWorldEnd, null);
			Service.Get<WorldTransitioner>().StartTransition(new WorldToWorldTransition(null, mapDataLoader, new TransitionCompleteDelegate(this.OnTransitionComplete), true, true));
		}

		private void OnTransitionComplete()
		{
			base.Complete();
		}
	}
}
