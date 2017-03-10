using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.Maker
{
	public class VideoFilterOption
	{
		public string UILabel
		{
			get;
			private set;
		}

		public string Value
		{
			get;
			private set;
		}

		public int Id
		{
			get;
			private set;
		}

		public VideoFilterOption(string label, string value, int id)
		{
			this.UILabel = label;
			this.Value = value;
			this.Id = id;
		}

		public VideoFilterOption(VideoFilterOption other) : this(other.UILabel, other.Value, other.Id)
		{
		}

		protected internal VideoFilterOption(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoFilterOption)GCHandledObjects.GCHandleToObject(instance)).Id);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoFilterOption)GCHandledObjects.GCHandleToObject(instance)).UILabel);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoFilterOption)GCHandledObjects.GCHandleToObject(instance)).Value);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VideoFilterOption)GCHandledObjects.GCHandleToObject(instance)).Id = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((VideoFilterOption)GCHandledObjects.GCHandleToObject(instance)).UILabel = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((VideoFilterOption)GCHandledObjects.GCHandleToObject(instance)).Value = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
