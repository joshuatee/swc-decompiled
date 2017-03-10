using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Response
{
	public class Data : AbstractResponse
	{
		public uint RequestId
		{
			get;
			protected set;
		}

		public Dictionary<string, object> Messages
		{
			get;
			protected set;
		}

		public object Result
		{
			get;
			protected set;
		}

		public uint Status
		{
			get;
			protected set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.RequestId = Convert.ToUInt32(dictionary["requestId"], CultureInfo.InvariantCulture);
			this.Messages = (dictionary["messages"] as Dictionary<string, object>);
			this.Result = dictionary["result"];
			this.Status = Convert.ToUInt32(dictionary["status"], CultureInfo.InvariantCulture);
			return this;
		}

		public Data()
		{
		}

		protected internal Data(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Data)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Data)GCHandledObjects.GCHandleToObject(instance)).Messages);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Data)GCHandledObjects.GCHandleToObject(instance)).Result);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Data)GCHandledObjects.GCHandleToObject(instance)).Messages = (Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Data)GCHandledObjects.GCHandleToObject(instance)).Result = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
