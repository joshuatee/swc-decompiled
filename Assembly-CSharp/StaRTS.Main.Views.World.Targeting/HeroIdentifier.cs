using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.World.Targeting
{
	public class HeroIdentifier : IEventObserver
	{
		private List<HeroDecal> decals;

		public HeroIdentifier()
		{
			this.decals = new List<HeroDecal>();
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.HeroDeployed, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.HeroKilled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.AddDecalToTroop, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.ChampionKilled, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
			eventManager.RegisterObserver(this, EventId.TroopViewReady, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.GameStateChanged)
			{
				if (id != EventId.TroopViewReady)
				{
					if (id == EventId.GameStateChanged)
					{
						Type type = (Type)cookie;
						if (type == typeof(BattleEndState) || type == typeof(BattleEndPlaybackState))
						{
							this.CleanupAllDecals();
						}
					}
				}
				else
				{
					EntityViewParams entityViewParams = cookie as EntityViewParams;
					this.OnHeroOrChampionViewReady(entityViewParams.Entity);
				}
			}
			else
			{
				if (id != EventId.HeroDeployed)
				{
					switch (id)
					{
					case EventId.HeroKilled:
					case EventId.ChampionKilled:
						this.OnHeroOrChampionKilled(cookie as Entity);
						return EatResponse.NotEaten;
					case EventId.ChampionDeployed:
						return EatResponse.NotEaten;
					case EventId.AddDecalToTroop:
						break;
					default:
						return EatResponse.NotEaten;
					}
				}
				this.OnHeroDeployed(cookie as Entity);
			}
			return EatResponse.NotEaten;
		}

		private HeroDecal FindDecal(Entity entity)
		{
			int i = 0;
			int count = this.decals.Count;
			while (i < count)
			{
				HeroDecal heroDecal = this.decals[i];
				if (heroDecal.Entity == entity)
				{
					return heroDecal;
				}
				i++;
			}
			return null;
		}

		private void OnHeroDeployed(Entity entity)
		{
			this.decals.Add(new HeroDecal(entity));
		}

		private void OnHeroOrChampionViewReady(Entity entity)
		{
			HeroDecal heroDecal = this.FindDecal(entity);
			if (heroDecal != null)
			{
				heroDecal.TrySetupView();
			}
		}

		private void OnHeroOrChampionKilled(Entity entity)
		{
			HeroDecal heroDecal = this.FindDecal(entity);
			if (heroDecal != null)
			{
				heroDecal.FadeToGray();
			}
		}

		private void CleanupAllDecals()
		{
			int i = 0;
			int count = this.decals.Count;
			while (i < count)
			{
				this.decals[i].Cleanup();
				i++;
			}
			this.decals.Clear();
		}

		protected internal HeroIdentifier(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HeroIdentifier)GCHandledObjects.GCHandleToObject(instance)).CleanupAllDecals();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroIdentifier)GCHandledObjects.GCHandleToObject(instance)).FindDecal((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroIdentifier)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HeroIdentifier)GCHandledObjects.GCHandleToObject(instance)).OnHeroDeployed((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((HeroIdentifier)GCHandledObjects.GCHandleToObject(instance)).OnHeroOrChampionKilled((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((HeroIdentifier)GCHandledObjects.GCHandleToObject(instance)).OnHeroOrChampionViewReady((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
