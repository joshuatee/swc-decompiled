using System;
using WinRTBridge;

namespace StaRTS.Utils.Animation.Anims
{
	public class Anim
	{
		private Action<float> optimizedUpdate;

		private Action<Anim, float> onUpdateCallback;

		public float Age
		{
			get;
			set;
		}

		public float Delay
		{
			get;
			set;
		}

		public uint DelayTimer
		{
			get;
			set;
		}

		public float Duration
		{
			get;
			set;
		}

		public object Tag
		{
			get;
			set;
		}

		public bool Playing
		{
			get;
			protected set;
		}

		public Action<Anim> OnCompleteCallback
		{
			get;
			set;
		}

		public Action<Anim> OnBeginCallback
		{
			get;
			set;
		}

		public Action<Anim, float> OnUpdateCallback
		{
			get
			{
				return this.onUpdateCallback;
			}
			set
			{
				this.onUpdateCallback = value;
				if (value == null)
				{
					this.optimizedUpdate = new Action<float>(this.UpdateWithoutCallback);
					return;
				}
				this.optimizedUpdate = new Action<float>(this.UpdateWithCallback);
			}
		}

		public virtual Easing.EasingDelegate EaseFunction
		{
			get;
			set;
		}

		public Anim()
		{
			this.EaseFunction = new Easing.EasingDelegate(Easing.Linear);
			this.optimizedUpdate = new Action<float>(this.UpdateWithoutCallback);
		}

		public virtual void OnBegin()
		{
		}

		public virtual void OnUpdate(float dt)
		{
		}

		public virtual void OnComplete()
		{
		}

		public void Begin()
		{
			this.Playing = true;
			this.OnBegin();
			if (this.OnBeginCallback != null)
			{
				this.OnBeginCallback.Invoke(this);
			}
		}

		public void Update(float dt)
		{
			this.optimizedUpdate.Invoke(dt);
		}

		private void UpdateWithCallback(float dt)
		{
			this.OnUpdate(dt);
			this.OnUpdateCallback.Invoke(this, dt);
		}

		private void UpdateWithoutCallback(float dt)
		{
			this.OnUpdate(dt);
		}

		public void Complete()
		{
			this.Age = 0f;
			this.Playing = false;
			this.OnComplete();
			if (this.OnCompleteCallback != null)
			{
				this.OnCompleteCallback.Invoke(this);
			}
		}

		public void Tick(float dt)
		{
			this.Age += dt;
		}

		public void Cleanup()
		{
			this.Tag = null;
			this.OnCompleteCallback = null;
			this.OnBeginCallback = null;
			this.optimizedUpdate = null;
			this.onUpdateCallback = null;
			this.OnUpdateCallback = null;
		}

		protected internal Anim(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).Begin();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).Complete();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Anim)GCHandledObjects.GCHandleToObject(instance)).Age);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Anim)GCHandledObjects.GCHandleToObject(instance)).Delay);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Anim)GCHandledObjects.GCHandleToObject(instance)).Duration);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Anim)GCHandledObjects.GCHandleToObject(instance)).EaseFunction);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Anim)GCHandledObjects.GCHandleToObject(instance)).OnBeginCallback);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Anim)GCHandledObjects.GCHandleToObject(instance)).OnCompleteCallback);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Anim)GCHandledObjects.GCHandleToObject(instance)).OnUpdateCallback);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Anim)GCHandledObjects.GCHandleToObject(instance)).Playing);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Anim)GCHandledObjects.GCHandleToObject(instance)).Tag);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).OnBegin();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).OnComplete();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).OnUpdate(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).Age = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).Delay = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).Duration = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).EaseFunction = (Easing.EasingDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).OnBeginCallback = (Action<Anim>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).OnCompleteCallback = (Action<Anim>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).OnUpdateCallback = (Action<Anim, float>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).Playing = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).Tag = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).Tick(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).Update(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).UpdateWithCallback(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((Anim)GCHandledObjects.GCHandleToObject(instance)).UpdateWithoutCallback(*(float*)args);
			return -1L;
		}
	}
}
