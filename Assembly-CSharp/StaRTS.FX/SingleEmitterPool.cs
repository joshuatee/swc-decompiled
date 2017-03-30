using StaRTS.Utils.Pooling;
using System;
using UnityEngine;

namespace StaRTS.FX
{
	public class SingleEmitterPool : EmitterPool
	{
		private ParticleSystem emitterProto;

		private ObjectPool<ParticleSystem> emitterPool;

		public SingleEmitterPool(ParticleSystem emitter, EmitterReturnedToPool emitterReturnedToPool) : base(emitterReturnedToPool)
		{
			this.emitterProto = emitter;
			this.emitterPool = new ObjectPool<ParticleSystem>(new ObjectPool<ParticleSystem>.CreatePoolObjectDelegate(this.CreateEmitterPoolObject), new ObjectPool<ParticleSystem>.DestroyPoolObjectDelegate(this.DestroyEmitterPoolObject), new ObjectPool<ParticleSystem>.ActivatePoolObjectDelegate(this.ActivateEmitterPoolObject), new ObjectPool<ParticleSystem>.DeactivatePoolObjectDelegate(this.DeactivateEmitterPoolObject));
		}

		private ParticleSystem CreateEmitterPoolObject(IObjectPool<ParticleSystem> objectPool)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.emitterProto.gameObject);
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			this.ActivateEmitterPoolObject(component);
			return component;
		}

		private void DestroyEmitterPoolObject(ParticleSystem emitter)
		{
			UnityEngine.Object.Destroy(emitter.gameObject);
		}

		private void DeactivateEmitterPoolObject(ParticleSystem emitter)
		{
			emitter.gameObject.SetActive(false);
		}

		private void ActivateEmitterPoolObject(ParticleSystem emitter)
		{
			emitter.gameObject.SetActive(true);
		}

		public ParticleSystem GetEmitter()
		{
			return this.emitterPool.GetFromPool();
		}

		public void StopEmissionAndReturnToPool(ParticleSystem emitter, float delayPreEmitterStop, float delayPostEmitterStop)
		{
			if (emitter != null)
			{
				base.StopEmitterAndReturnToPool(emitter, delayPreEmitterStop, new EmitterStopDelegate(this.StopEmitter), delayPostEmitterStop, new EmitterStopDelegate(this.PostEmitterStop));
			}
		}

		private void PostEmitterStop(object cookie)
		{
			ParticleSystem particleSystem = (ParticleSystem)cookie;
			if (particleSystem != null)
			{
				this.emitterPool.ReturnToPool(particleSystem);
			}
		}

		private void StopEmitter(object cookie)
		{
			ParticleSystem particleSystem = (ParticleSystem)cookie;
			if (particleSystem != null && !particleSystem.isStopped)
			{
				particleSystem.Stop(false);
			}
		}

		public override void Destroy()
		{
			base.Destroy();
			this.emitterProto = null;
			this.emitterPool.Destroy();
			this.emitterPool = null;
		}
	}
}
