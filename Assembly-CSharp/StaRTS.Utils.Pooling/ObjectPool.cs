using StaRTS.Main.Views.World.Targeting;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils.Pooling
{
	public class ObjectPool<T> : IObjectPool<T>
	{
		public delegate T CreatePoolObjectDelegate(IObjectPool<T> objectPool);

		public delegate void DestroyPoolObjectDelegate(T instance);

		public delegate void ActivatePoolObjectDelegate(T instance);

		public delegate void DeactivatePoolObjectDelegate(T instance);

		private Queue<T> pool;

		private ObjectPool<T>.CreatePoolObjectDelegate createPoolObjectDelegate;

		private ObjectPool<T>.DestroyPoolObjectDelegate destroyPoolObjectDelegate;

		private ObjectPool<T>.DeactivatePoolObjectDelegate deactivatePoolObjectDelegate;

		private ObjectPool<T>.ActivatePoolObjectDelegate activatePoolObjectDelegate;

		public int Count
		{
			get
			{
				return this.pool.Count;
			}
		}

		public int Capacity
		{
			get;
			protected set;
		}

		public ObjectPool(ObjectPool<T>.CreatePoolObjectDelegate createPoolObjectDelegate, ObjectPool<T>.DestroyPoolObjectDelegate destroyPoolObjectDelegate, ObjectPool<T>.ActivatePoolObjectDelegate activatePoolObjectDelegate, ObjectPool<T>.DeactivatePoolObjectDelegate deactivatePoolObjectDelegate)
		{
			this.pool = new Queue<T>();
			this.Capacity = 0;
			this.createPoolObjectDelegate = createPoolObjectDelegate;
			this.destroyPoolObjectDelegate = destroyPoolObjectDelegate;
			this.activatePoolObjectDelegate = activatePoolObjectDelegate;
			this.deactivatePoolObjectDelegate = deactivatePoolObjectDelegate;
		}

		public void EnsurePoolCapacity(int n)
		{
			for (int i = this.Capacity; i < n; i++)
			{
				this.ReturnToPool(this.CreateNew());
			}
		}

		protected virtual T CreateNew()
		{
			int capacity = this.Capacity;
			this.Capacity = capacity + 1;
			return this.createPoolObjectDelegate(this);
		}

		public virtual void ReturnToPool(T obj)
		{
			if (this.deactivatePoolObjectDelegate != null)
			{
				this.deactivatePoolObjectDelegate(obj);
			}
			this.pool.Enqueue(obj);
		}

		public virtual T GetFromPool(bool allowBeyondCapacity)
		{
			if (this.pool.Count != 0)
			{
				T t = this.pool.Dequeue();
				if (this.activatePoolObjectDelegate != null)
				{
					this.activatePoolObjectDelegate(t);
				}
				return t;
			}
			if (allowBeyondCapacity)
			{
				return this.CreateNew();
			}
			throw new Exception("No more pool objects available");
		}

		public virtual T GetFromPool()
		{
			return this.GetFromPool(true);
		}

		public void ClearOutPool()
		{
			if (this.destroyPoolObjectDelegate != null)
			{
				while (this.pool.Count > 0)
				{
					this.destroyPoolObjectDelegate(this.pool.Dequeue());
				}
			}
		}

		public void Destroy()
		{
			this.ClearOutPool();
			this.pool = null;
			this.createPoolObjectDelegate = null;
			this.destroyPoolObjectDelegate = null;
			this.deactivatePoolObjectDelegate = null;
			this.activatePoolObjectDelegate = null;
		}

		protected internal ObjectPool(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(instance)).ClearOutPool();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(instance)).EnsurePoolCapacity(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(instance)).Capacity);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(instance)).Count);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ObjectPool<TargetReticle>)GCHandledObjects.GCHandleToObject(instance)).Capacity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(instance)).ClearOutPool();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(instance)).EnsurePoolCapacity(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(instance)).Capacity);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(instance)).Count);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ObjectPool<GameObject>)GCHandledObjects.GCHandleToObject(instance)).Capacity = *(int*)args;
			return -1L;
		}
	}
}
