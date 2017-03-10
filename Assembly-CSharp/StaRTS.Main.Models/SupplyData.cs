using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class SupplyData : ISerializable
	{
		public string SupplyId
		{
			get;
			private set;
		}

		public string SupplyPoolId
		{
			get;
			private set;
		}

		public SupplyData()
		{
		}

		public string ToJson()
		{
			return "{}";
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				if (dictionary.ContainsKey("supplyId"))
				{
					this.SupplyId = Convert.ToString(dictionary["supplyId"]);
				}
				if (dictionary.ContainsKey("supplyPoolId"))
				{
					this.SupplyPoolId = Convert.ToString(dictionary["supplyPoolId"]);
				}
			}
			return this;
		}

		protected internal SupplyData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyData)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyData)GCHandledObjects.GCHandleToObject(instance)).SupplyId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyData)GCHandledObjects.GCHandleToObject(instance)).SupplyPoolId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SupplyData)GCHandledObjects.GCHandleToObject(instance)).SupplyId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SupplyData)GCHandledObjects.GCHandleToObject(instance)).SupplyPoolId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SupplyData)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
