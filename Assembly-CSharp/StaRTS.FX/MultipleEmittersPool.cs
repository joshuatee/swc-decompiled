using StaRTS.Utils.Pooling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.FX
{
	public class MultipleEmittersPool : EmitterPool
	{
		public static int ID;

		private GameObject emitterRootProto;

		private ObjectPool<GameObject> emitterRootPool;

		public static void StaticReset()
		{
			MultipleEmittersPool.ID = 0;
		}

		public MultipleEmittersPool(GameObject emitterRootNode, EmitterReturnedToPool emitterReturnedToPool) : base(emitterReturnedToPool)
		{
			this.emitterRootProto = emitterRootNode;
			this.emitterRootPool = new ObjectPool<GameObject>(new ObjectPool<GameObject>.CreatePoolObjectDelegate(this.CreateEmitterRootPoolObject), new ObjectPool<GameObject>.DestroyPoolObjectDelegate(this.DestroyEmitterRootPoolObject), new ObjectPool<GameObject>.ActivatePoolObjectDelegate(this.ActivateEmitterRootPoolObject), new ObjectPool<GameObject>.DeactivatePoolObjectDelegate(this.DeactivateEmitterRootPoolObject));
		}

		private GameObject CreateEmitterRootPoolObject(IObjectPool<GameObject> objectPool)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.emitterRootProto);
			gameObject.name = gameObject.name + "_" + MultipleEmittersPool.ID++;
			MultipleEmittersPool.StopChildEmitters(gameObject);
			this.ActivateEmitterRootPoolObject(gameObject);
			return gameObject;
		}

		private void DestroyEmitterRootPoolObject(GameObject emitterRootNode)
		{
			UnityEngine.Object.Destroy(emitterRootNode);
		}

		private void DeactivateEmitterRootPoolObject(GameObject emitterRootNode)
		{
			emitterRootNode.SetActive(false);
		}

		private void ActivateEmitterRootPoolObject(GameObject emitterRootNode)
		{
		}

		public GameObject GetEmitterRoot()
		{
			return this.emitterRootPool.GetFromPool();
		}

		public static ParticleSystem[] GetAllEmitters(GameObject rootNode)
		{
			return rootNode.GetComponentsInChildren<ParticleSystem>(true);
		}

		public static void StopChildEmitters(GameObject rootNode)
		{
			ParticleSystem[] allEmitters = MultipleEmittersPool.GetAllEmitters(rootNode);
			ParticleSystem[] array = allEmitters;
			for (int i = 0; i < array.Length; i++)
			{
				ParticleSystem particleSystem = array[i];
				if (!particleSystem.isStopped)
				{
					particleSystem.Stop(false);
				}
			}
		}

		public void StopEmissionAndReturnToPool(GameObject emitterRootNode, float delayPreEmitterStop, float delayPostEmitterStop)
		{
			if (emitterRootNode != null)
			{
				base.StopEmitterAndReturnToPool(emitterRootNode, delayPreEmitterStop, new EmitterStopDelegate(this.StopEmitter), delayPostEmitterStop, new EmitterStopDelegate(this.PostEmitterStop));
			}
		}

		private void PostEmitterStop(object cookie)
		{
			GameObject gameObject = (GameObject)cookie;
			if (gameObject != null)
			{
				this.emitterRootPool.ReturnToPool(gameObject);
			}
		}

		private void StopEmitter(object cookie)
		{
			GameObject gameObject = (GameObject)cookie;
			if (gameObject != null)
			{
				MultipleEmittersPool.StopChildEmitters(gameObject);
			}
		}

		public override void Destroy()
		{
			base.Destroy();
			this.emitterRootProto = null;
			this.emitterRootPool.Destroy();
			this.emitterRootPool = null;
		}

		protected internal MultipleEmittersPool(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((MultipleEmittersPool)GCHandledObjects.GCHandleToObject(instance)).ActivateEmitterRootPoolObject((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MultipleEmittersPool)GCHandledObjects.GCHandleToObject(instance)).CreateEmitterRootPoolObject((IObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((MultipleEmittersPool)GCHandledObjects.GCHandleToObject(instance)).DeactivateEmitterRootPoolObject((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((MultipleEmittersPool)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((MultipleEmittersPool)GCHandledObjects.GCHandleToObject(instance)).DestroyEmitterRootPoolObject((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MultipleEmittersPool.GetAllEmitters((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MultipleEmittersPool)GCHandledObjects.GCHandleToObject(instance)).GetEmitterRoot());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((MultipleEmittersPool)GCHandledObjects.GCHandleToObject(instance)).PostEmitterStop(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			MultipleEmittersPool.StaticReset();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			MultipleEmittersPool.StopChildEmitters((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((MultipleEmittersPool)GCHandledObjects.GCHandleToObject(instance)).StopEmissionAndReturnToPool((GameObject)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(float*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((MultipleEmittersPool)GCHandledObjects.GCHandleToObject(instance)).StopEmitter(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
