using Net.RichardLord.Ash.Core;
using StaRTS.Main.Configs;
using StaRTS.Main.Controllers.Entities.Systems;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;

namespace StaRTS.Main.Controllers
{
	public class EntityController : Game, ISimTimeObserver, IViewFrameTimeObserver
	{
		private float viewSpeed;

		public EntityViewManager View
		{
			get;
			protected set;
		}

		public EntityController() : base(new ComponentMatchingFamilyFactory())
		{
			Service.Set<EntityController>(this);
			new DroidController();
			new ChampionController();
			this.View = new EntityViewManager();
			this.viewSpeed = 1f;
			this.AddSimSystem(new TrackingSystem(), 1050, 21845);
			this.AddSimSystem(new MovementSystem(), 1060, 43690);
			this.AddViewSystem(new EntityRenderSystem(), 2030, SystemSchedulingPatterns.ENTITY_RENDER);
			this.AddViewSystem(new TrackingRenderSystem(), 2050, SystemSchedulingPatterns.TRACKING_RENDER);
			this.AddViewSystem(new HealthRenderSystem(), 2060, SystemSchedulingPatterns.HEALTH_RENDER);
			this.AddViewSystem(new TransportSystem(), 2090, 65535);
			this.AddViewSystem(new DroidSystem(), 2020, 65535);
			Service.Get<SimTimeEngine>().RegisterSimTimeObserver(this);
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		public void OnSimTime(uint dt)
		{
			this.UpdateSimSystems(dt);
		}

		public void OnViewFrameTime(float dt)
		{
			this.UpdateViewSystems(dt * this.viewSpeed);
		}

		public void SetSpeed(float speed)
		{
			this.viewSpeed = speed;
		}
	}
}
