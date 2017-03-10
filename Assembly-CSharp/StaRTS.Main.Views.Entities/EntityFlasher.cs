using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class EntityFlasher : IViewFrameTimeObserver, IEventObserver
	{
		private List<FlashingEntity> flashingEntities;

		public EntityFlasher()
		{
			this.flashingEntities = new List<FlashingEntity>();
		}

		public void AddFlashing(SmartEntity entity, float flashDuration, float flashDelay)
		{
			GameObjectViewComponent gameObjectViewComp = entity.GameObjectViewComp;
			if (gameObjectViewComp == null)
			{
				return;
			}
			if (gameObjectViewComp.IsFlashing)
			{
				return;
			}
			gameObjectViewComp.IsFlashing = true;
			if (this.flashingEntities.Count == 0)
			{
				this.RegisterObservers();
			}
			FlashingEntity item = new FlashingEntity(entity, gameObjectViewComp.MainGameObject, flashDuration, flashDelay);
			this.flashingEntities.Add(item);
		}

		public void RemoveFlashing(SmartEntity entity)
		{
			for (int i = this.flashingEntities.Count - 1; i >= 0; i--)
			{
				FlashingEntity flashingEntity = this.flashingEntities[i];
				if (flashingEntity.Entity == entity)
				{
					this.StopFlashing(i);
				}
			}
		}

		private void StopFlashing(int i)
		{
			FlashingEntity flashingEntity = this.flashingEntities[i];
			flashingEntity.Complete();
			this.flashingEntities.RemoveAt(i);
			if (this.flashingEntities.Count == 0)
			{
				this.UnregisterObservers();
			}
			GameObjectViewComponent gameObjectViewComp = flashingEntity.Entity.GameObjectViewComp;
			if (gameObjectViewComp != null)
			{
				gameObjectViewComp.IsFlashing = false;
			}
		}

		public void RemoveFlashingFromAllEntities()
		{
			if (this.flashingEntities.Count == 0)
			{
				return;
			}
			int count = this.flashingEntities.Count;
			for (int i = 0; i < count; i++)
			{
				FlashingEntity flashingEntity = this.flashingEntities[i];
				flashingEntity.Complete();
			}
			this.flashingEntities.Clear();
			this.UnregisterObservers();
		}

		private void RegisterObservers()
		{
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ViewObjectsPurged, EventPriority.Default);
		}

		private void UnregisterObservers()
		{
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ViewObjectsPurged);
		}

		public void OnViewFrameTime(float dt)
		{
			for (int i = this.flashingEntities.Count - 1; i >= 0; i--)
			{
				FlashingEntity flashingEntity = this.flashingEntities[i];
				if (flashingEntity.Flash(dt))
				{
					this.StopFlashing(i);
				}
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ViewObjectsPurged)
			{
				this.RemoveFlashing((SmartEntity)cookie);
			}
			return EatResponse.NotEaten;
		}

		protected internal EntityFlasher(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EntityFlasher)GCHandledObjects.GCHandleToObject(instance)).AddFlashing((SmartEntity)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(float*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityFlasher)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EntityFlasher)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EntityFlasher)GCHandledObjects.GCHandleToObject(instance)).RegisterObservers();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EntityFlasher)GCHandledObjects.GCHandleToObject(instance)).RemoveFlashing((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EntityFlasher)GCHandledObjects.GCHandleToObject(instance)).RemoveFlashingFromAllEntities();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EntityFlasher)GCHandledObjects.GCHandleToObject(instance)).StopFlashing(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((EntityFlasher)GCHandledObjects.GCHandleToObject(instance)).UnregisterObservers();
			return -1L;
		}
	}
}
