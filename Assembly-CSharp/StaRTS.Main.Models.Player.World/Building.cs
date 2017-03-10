using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.World
{
	public class Building : ISerializable
	{
		public const string BUILDING_ID_PREFIX = "bld_";

		public string Key;

		public int X;

		public int Z;

		public string Uid;

		public uint LastCollectTime;

		public int CurrentStorage;

		public int AccruedCurrency;

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Key = (dictionary["key"] as string);
			this.X = Convert.ToInt32(dictionary["x"], CultureInfo.InvariantCulture);
			this.Z = Convert.ToInt32(dictionary["z"], CultureInfo.InvariantCulture);
			this.Uid = (dictionary["uid"] as string);
			if (dictionary.ContainsKey("lastCollectTime"))
			{
				this.LastCollectTime = Convert.ToUInt32(dictionary["lastCollectTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("currentStorage"))
			{
				this.CurrentStorage = Convert.ToInt32(dictionary["currentStorage"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		public string ToJson()
		{
			Serializer serializer = Serializer.Start().AddString("key", this.Key).Add<int>("x", this.X).Add<int>("z", this.Z).AddString("uid", this.Uid).Add<int>("currentStorage", this.CurrentStorage);
			return serializer.End().ToString();
		}

		public static Building FromBuildingTypeVO(BuildingTypeVO buildingType)
		{
			Building building = new Building();
			building.X = 0;
			building.Z = 0;
			building.Uid = buildingType.Uid;
			building.Key = "bld_" + Service.Get<CurrentPlayer>().Map.GetNextBuildingNumberAndIncrement();
			if (buildingType.Type == BuildingType.Trap)
			{
				building.CurrentStorage = 1;
			}
			return building;
		}

		public void SyncWithTransform(TransformComponent transform)
		{
			this.X = Units.BoardToGridX(transform.X);
			this.Z = Units.BoardToGridZ(transform.Z);
		}

		public void AddString(StringBuilder sb, string uidOverride, uint timeOverride)
		{
			this.AddString(sb, uidOverride, timeOverride, this.X, this.Z);
		}

		public void AddString(StringBuilder sb, string uidOverride, uint timeOverride, int manualX, int manualZ)
		{
			string text = (uidOverride != null) ? uidOverride : this.Uid;
			uint num = (timeOverride != 0u) ? timeOverride : this.LastCollectTime;
			sb.Append(this.Key).Append("|").Append(manualX).Append("|").Append(manualZ).Append("|").Append(text).Append("|").Append(num).Append("|").Append(this.CurrentStorage).Append("|").Append("\n");
		}

		public Building Clone()
		{
			return (Building)base.MemberwiseClone();
		}

		public Building()
		{
		}

		protected internal Building(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Building)GCHandledObjects.GCHandleToObject(instance)).Clone());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Building.FromBuildingTypeVO((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Building)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Building)GCHandledObjects.GCHandleToObject(instance)).SyncWithTransform((TransformComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Building)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
