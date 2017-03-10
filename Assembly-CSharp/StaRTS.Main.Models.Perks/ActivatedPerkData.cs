using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Perks
{
	public class ActivatedPerkData : ISerializable
	{
		public string PerkId
		{
			get;
			set;
		}

		public uint StartTime
		{
			get;
			set;
		}

		public uint EndTime
		{
			get;
			set;
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("perkId"))
			{
				this.PerkId = (dictionary["perkId"] as string);
			}
			if (dictionary.ContainsKey("startTime"))
			{
				this.StartTime = Convert.ToUInt32(dictionary["startTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("endTime"))
			{
				this.EndTime = Convert.ToUInt32(dictionary["endTime"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		public string ToJson()
		{
			Service.Get<StaRTSLogger>().Warn("Attempting to inappropriately serialize ActivatedPerkData");
			return string.Empty;
		}

		public ActivatedPerkData()
		{
		}

		protected internal ActivatedPerkData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ActivatedPerkData)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ActivatedPerkData)GCHandledObjects.GCHandleToObject(instance)).PerkId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ActivatedPerkData)GCHandledObjects.GCHandleToObject(instance)).PerkId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ActivatedPerkData)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
