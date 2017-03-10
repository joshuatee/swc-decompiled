using StaRTS.GameBoard.Pathfinding;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Controllers.SquadWar;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class BoardStartupTask : StartupTask
	{
		public BoardStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Service.Get<EventManager>().SendEvent(EventId.InitializeBoardStart, null);
			new BoardController();
			new EntityRenderController();
			new SkinController();
			new EntityController();
			new EntityFactory();
			new SpatialIndexController();
			new EntityIdleController();
			new BuildingAnimationController();
			new PlanetRelocationController();
			new GalaxyViewController();
			new GalaxyPlanetController();
			new WarBoardViewController();
			new WarBoardBuildingController();
			new PathingManager();
			Service.Get<EventManager>().SendEvent(EventId.InitializeBoardEnd, null);
			base.Complete();
		}

		protected internal BoardStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BoardStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
