using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.FX
{
	public class EmitterPool
	{
		private EmitterReturnedToPool emitterReturnedToPool;

		private List<uint> beforeStopTimerIds;

		private List<uint> afterStopTimerIds;

		public EmitterPool(EmitterReturnedToPool emitterReturnedToPool)
		{
			this.emitterReturnedToPool = emitterReturnedToPool;
		}

		public static EmitterPool CreateEmitterPool(ParticleSystem emitter, EmitterReturnedToPool emitterReturnedToPool)
		{
			EmitterPool result = null;
			if (emitter != null)
			{
				GameObject gameObject = emitter.gameObject;
				while (gameObject.transform.parent != null)
				{
					gameObject = gameObject.transform.parent.gameObject;
				}
				ParticleSystem[] allEmitters = MultipleEmittersPool.GetAllEmitters(gameObject);
				if (allEmitters.Length > 1)
				{
					result = new MultipleEmittersPool(gameObject, emitterReturnedToPool);
				}
				else
				{
					result = new SingleEmitterPool(emitter, emitterReturnedToPool);
				}
			}
			return result;
		}

		protected void StopEmitterAndReturnToPool(object cookie, float delayPreEmitterStop, EmitterStopDelegate emitterStop, float delayPostEmitterStop, EmitterStopDelegate postEmitterStop)
		{
			EmitterPoolCookie cookie2 = new EmitterPoolCookie(cookie, emitterStop, delayPostEmitterStop, postEmitterStop);
			if (MathUtils.IsGreaterThanZero(delayPreEmitterStop))
			{
				uint timerId = Service.Get<ViewTimerManager>().CreateViewTimer(delayPreEmitterStop, false, new TimerDelegate(this.BeforeStopEmitter), cookie2);
				this.AddBeforeStopTimerId(timerId);
				return;
			}
			this.BeforeStopEmitter(0u, cookie2);
		}

		private void BeforeStopEmitter(uint timerId, object cookie)
		{
			if (this.beforeStopTimerIds != null)
			{
				this.beforeStopTimerIds.Remove(timerId);
			}
			EmitterPoolCookie emitterPoolCookie = (EmitterPoolCookie)cookie;
			emitterPoolCookie.EmitterStop(emitterPoolCookie.Cookie);
			if (MathUtils.IsGreaterThanZero(emitterPoolCookie.DelayPostEmitterStop))
			{
				timerId = Service.Get<ViewTimerManager>().CreateViewTimer(emitterPoolCookie.DelayPostEmitterStop, false, new TimerDelegate(this.AfterStopEmitter), emitterPoolCookie);
				this.AddAfterStopTimerId(timerId);
				return;
			}
			this.AfterStopEmitter(0u, emitterPoolCookie);
		}

		private void AfterStopEmitter(uint timerId, object cookie)
		{
			if (this.afterStopTimerIds != null)
			{
				this.afterStopTimerIds.Remove(timerId);
			}
			EmitterPoolCookie emitterPoolCookie = (EmitterPoolCookie)cookie;
			emitterPoolCookie.PostEmitterStop(emitterPoolCookie.Cookie);
			if (this.emitterReturnedToPool != null)
			{
				this.emitterReturnedToPool(emitterPoolCookie.Cookie);
			}
		}

		private void AddBeforeStopTimerId(uint timerId)
		{
			if (this.beforeStopTimerIds == null)
			{
				this.beforeStopTimerIds = new List<uint>();
			}
			this.beforeStopTimerIds.Add(timerId);
		}

		private void AddAfterStopTimerId(uint timerId)
		{
			if (this.afterStopTimerIds == null)
			{
				this.afterStopTimerIds = new List<uint>();
			}
			this.afterStopTimerIds.Add(timerId);
		}

		private void TriggerAndKillTimers(List<uint> timerIds)
		{
			if (!Service.IsSet<ViewTimerManager>() || timerIds == null)
			{
				return;
			}
			List<uint> list = new List<uint>();
			int i = 0;
			int count = timerIds.Count;
			while (i < count)
			{
				uint num = timerIds[i];
				if (num != 0u)
				{
					list.Add(num);
				}
				i++;
			}
			ViewTimerManager viewTimerManager = Service.Get<ViewTimerManager>();
			int j = 0;
			int count2 = list.Count;
			while (j < count2)
			{
				uint num2 = list[j];
				if (num2 != 0u)
				{
					viewTimerManager.TriggerKillViewTimer(num2);
				}
				j++;
			}
			list.Clear();
		}

		public virtual void Destroy()
		{
			this.TriggerAndKillTimers(this.beforeStopTimerIds);
			this.TriggerAndKillTimers(this.afterStopTimerIds);
			if (this.beforeStopTimerIds != null)
			{
				this.beforeStopTimerIds.Clear();
				this.beforeStopTimerIds = null;
			}
			if (this.afterStopTimerIds != null)
			{
				this.afterStopTimerIds.Clear();
				this.afterStopTimerIds = null;
			}
		}

		protected internal EmitterPool(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EmitterPool.CreateEmitterPool((ParticleSystem)GCHandledObjects.GCHandleToObject(*args), (EmitterReturnedToPool)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EmitterPool)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EmitterPool)GCHandledObjects.GCHandleToObject(instance)).StopEmitterAndReturnToPool(GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), (EmitterStopDelegate)GCHandledObjects.GCHandleToObject(args[2]), *(float*)(args + 3), (EmitterStopDelegate)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}
	}
}
