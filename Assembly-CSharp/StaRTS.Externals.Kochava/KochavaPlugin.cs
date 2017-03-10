using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.Kochava
{
	public class KochavaPlugin : MonoBehaviour, IUnitySerializable
	{
		public delegate void KochavaInitDelegate();

		public delegate void KochavaFireEvent(string eventName, string eventData);

		public static KochavaPlugin.KochavaInitDelegate kochavaInitCallback;

		public static KochavaPlugin.KochavaFireEvent fireEventCallback;

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		private void Start()
		{
			if (KochavaPlugin.kochavaInitCallback != null)
			{
				KochavaPlugin.kochavaInitCallback();
				UnityEngine.Object.Destroy(this);
			}
		}

		public static void FireEvent(string eventName, string eventData)
		{
			if (KochavaPlugin.fireEventCallback != null)
			{
				KochavaPlugin.fireEventCallback(eventName, eventData);
			}
		}

		public KochavaPlugin()
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

		protected internal KochavaPlugin(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((KochavaPlugin)GCHandledObjects.GCHandleToObject(instance)).Awake();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			KochavaPlugin.FireEvent(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((KochavaPlugin)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((KochavaPlugin)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((KochavaPlugin)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((KochavaPlugin)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((KochavaPlugin)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((KochavaPlugin)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
