using StaRTS.Assets;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Audio
{
	public class AudioData
	{
		public AssetHandle Handle;

		public AudioClip Clip
		{
			get;
			set;
		}

		public AudioData()
		{
		}

		protected internal AudioData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioData)GCHandledObjects.GCHandleToObject(instance)).Clip);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AudioData)GCHandledObjects.GCHandleToObject(instance)).Clip = (AudioClip)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
