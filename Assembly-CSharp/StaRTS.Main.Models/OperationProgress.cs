using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class OperationProgress : ISerializable
	{
		public string Uid;

		public OperationProgress()
		{
		}

		public string ToJson()
		{
			return string.Empty;
		}

		public ISerializable FromObject(object obj)
		{
			return this;
		}

		protected internal OperationProgress(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OperationProgress)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OperationProgress)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
