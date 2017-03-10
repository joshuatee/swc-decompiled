using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Response
{
	public class Response : AbstractResponse
	{
		public uint ProtocolVersion
		{
			get;
			protected set;
		}

		public string ServerTime
		{
			get;
			protected set;
		}

		public uint ServerTimestamp
		{
			get;
			protected set;
		}

		public List<Data> DataList
		{
			get;
			protected set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.ProtocolVersion = Convert.ToUInt32(dictionary["protocolVersion"], CultureInfo.InvariantCulture);
			this.ServerTime = Convert.ToString(dictionary["serverTime"], CultureInfo.InvariantCulture);
			this.ServerTimestamp = Convert.ToUInt32(dictionary["serverTimestamp"], CultureInfo.InvariantCulture);
			List<object> list = dictionary["data"] as List<object>;
			this.DataList = new List<Data>();
			for (int i = 0; i < list.Count; i++)
			{
				this.DataList.Add(new Data().FromObject(list[i]) as Data);
			}
			return this;
		}

		public Response()
		{
		}

		protected internal Response(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Response)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Response)GCHandledObjects.GCHandleToObject(instance)).DataList);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Response)GCHandledObjects.GCHandleToObject(instance)).ServerTime);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Response)GCHandledObjects.GCHandleToObject(instance)).DataList = (List<Data>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Response)GCHandledObjects.GCHandleToObject(instance)).ServerTime = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
