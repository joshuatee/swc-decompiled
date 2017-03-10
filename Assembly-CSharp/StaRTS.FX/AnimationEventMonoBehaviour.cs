using StaRTS.Main.Models.Entities;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.FX
{
	public class AnimationEventMonoBehaviour : MonoBehaviour, IUnitySerializable
	{
		private EntityRef entityRefMB;

		public void Start()
		{
			this.entityRefMB = base.gameObject.GetComponent<EntityRef>();
		}

		public void AnimationEvent(string message)
		{
			if (!Service.IsSet<AnimationEventManager>())
			{
				return;
			}
			Service.Get<AnimationEventManager>().ProcessAnimationEvent(this.entityRefMB, message);
		}

		public AnimationEventMonoBehaviour()
		{
		}

		public override void Unity_Serialize(int depth)
		{
		}

		public override void Unity_Deserialize(int depth)
		{
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public override void Unity_NamedSerialize(int depth)
		{
		}

		public override void Unity_NamedDeserialize(int depth)
		{
		}

		protected internal AnimationEventMonoBehaviour(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AnimationEventMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).AnimationEvent(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AnimationEventMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AnimationEventMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AnimationEventMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AnimationEventMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AnimationEventMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AnimationEventMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
