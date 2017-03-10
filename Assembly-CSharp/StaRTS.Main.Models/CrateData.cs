using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class CrateData : ISerializable
	{
		public string UId
		{
			get;
			private set;
		}

		public string CrateId
		{
			get;
			private set;
		}

		public string Context
		{
			get;
			private set;
		}

		public string PlanetId
		{
			get;
			private set;
		}

		public int HQLevel
		{
			get;
			private set;
		}

		public uint ReceivedTimeStamp
		{
			get;
			private set;
		}

		public uint ExpiresTimeStamp
		{
			get;
			private set;
		}

		public bool DoesExpire
		{
			get;
			private set;
		}

		public bool Claimed
		{
			get;
			set;
		}

		public List<SupplyData> ResolvedSupplies
		{
			get;
			private set;
		}

		public string WarId
		{
			get;
			private set;
		}

		public string GuildId
		{
			get;
			private set;
		}

		public CrateData()
		{
			this.Claimed = false;
			this.DoesExpire = false;
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
				if (dictionary.ContainsKey("uid"))
				{
					this.UId = Convert.ToString(dictionary["uid"]);
				}
				if (dictionary.ContainsKey("crateId"))
				{
					this.CrateId = Convert.ToString(dictionary["crateId"]);
				}
				if (dictionary.ContainsKey("context"))
				{
					this.Context = Convert.ToString(dictionary["context"]);
				}
				if (dictionary.ContainsKey("planet"))
				{
					this.PlanetId = Convert.ToString(dictionary["planet"]);
				}
				if (dictionary.ContainsKey("hqLevel"))
				{
					this.HQLevel = Convert.ToInt32(dictionary["hqLevel"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("received"))
				{
					this.ReceivedTimeStamp = Convert.ToUInt32(dictionary["received"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("expires"))
				{
					this.ExpiresTimeStamp = Convert.ToUInt32(dictionary["expires"], CultureInfo.InvariantCulture);
					if (this.ExpiresTimeStamp == 0u)
					{
						this.DoesExpire = false;
					}
					else
					{
						this.DoesExpire = true;
					}
				}
				if (dictionary.ContainsKey("warId"))
				{
					this.WarId = Convert.ToString(dictionary["warId"]);
				}
				if (dictionary.ContainsKey("guildId"))
				{
					this.GuildId = Convert.ToString(dictionary["guildId"]);
				}
				this.ResolvedSupplies = new List<SupplyData>();
				if (dictionary.ContainsKey("resolvedSupplies"))
				{
					List<object> list = dictionary["resolvedSupplies"] as List<object>;
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						SupplyData supplyData = new SupplyData();
						supplyData.FromObject(list[i]);
						this.ResolvedSupplies.Add(supplyData);
						i++;
					}
				}
			}
			return this;
		}

		protected internal CrateData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).Claimed);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).CrateId);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).DoesExpire);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).GuildId);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).HQLevel);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).ResolvedSupplies);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).UId);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).WarId);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((CrateData)GCHandledObjects.GCHandleToObject(instance)).Claimed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((CrateData)GCHandledObjects.GCHandleToObject(instance)).Context = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((CrateData)GCHandledObjects.GCHandleToObject(instance)).CrateId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((CrateData)GCHandledObjects.GCHandleToObject(instance)).DoesExpire = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((CrateData)GCHandledObjects.GCHandleToObject(instance)).GuildId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((CrateData)GCHandledObjects.GCHandleToObject(instance)).HQLevel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((CrateData)GCHandledObjects.GCHandleToObject(instance)).PlanetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((CrateData)GCHandledObjects.GCHandleToObject(instance)).ResolvedSupplies = (List<SupplyData>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((CrateData)GCHandledObjects.GCHandleToObject(instance)).UId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((CrateData)GCHandledObjects.GCHandleToObject(instance)).WarId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateData)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
