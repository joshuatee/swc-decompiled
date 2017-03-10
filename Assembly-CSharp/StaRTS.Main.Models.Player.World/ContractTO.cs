using StaRTS.Utils;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.World
{
	public class ContractTO : ISerializable
	{
		public string Uid
		{
			get;
			set;
		}

		public uint EndTime
		{
			get;
			set;
		}

		public ContractType ContractType
		{
			get;
			set;
		}

		public string BuildingKey
		{
			get;
			set;
		}

		public string Tag
		{
			get;
			set;
		}

		public List<string> PerkIds
		{
			get;
			set;
		}

		public ContractTO()
		{
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			this.Uid = (string)dictionary["uid"];
			this.EndTime = Convert.ToUInt32(dictionary["endTime"], CultureInfo.InvariantCulture);
			string name = StringUtils.ToLowerCaseUnderscoreSeperated((string)dictionary["contractType"]);
			this.ContractType = StringUtils.ParseEnum<ContractType>(name);
			this.BuildingKey = (string)dictionary["buildingId"];
			this.Tag = (dictionary.ContainsKey("tag") ? ((string)dictionary["tag"]) : "");
			this.PerkIds = new List<string>();
			if (dictionary.ContainsKey("perkIds"))
			{
				List<object> list = dictionary["perkIds"] as List<object>;
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					this.PerkIds.Add(list[i] as string);
					i++;
				}
			}
			return this;
		}

		public string ToJson()
		{
			Serializer serializer = Serializer.Start();
			return serializer.End().ToString();
		}

		public void AddString(StringBuilder sb)
		{
			sb.Append(this.Uid).Append("|").Append(this.BuildingKey).Append("|").Append(this.EndTime).Append("|").Append(this.ContractType.ToString()).Append("|").Append("\n");
		}

		protected internal ContractTO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ContractTO)GCHandledObjects.GCHandleToObject(instance)).AddString((StringBuilder)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractTO)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractTO)GCHandledObjects.GCHandleToObject(instance)).BuildingKey);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractTO)GCHandledObjects.GCHandleToObject(instance)).ContractType);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractTO)GCHandledObjects.GCHandleToObject(instance)).PerkIds);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractTO)GCHandledObjects.GCHandleToObject(instance)).Tag);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractTO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ContractTO)GCHandledObjects.GCHandleToObject(instance)).BuildingKey = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ContractTO)GCHandledObjects.GCHandleToObject(instance)).ContractType = (ContractType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ContractTO)GCHandledObjects.GCHandleToObject(instance)).PerkIds = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ContractTO)GCHandledObjects.GCHandleToObject(instance)).Tag = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ContractTO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractTO)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
