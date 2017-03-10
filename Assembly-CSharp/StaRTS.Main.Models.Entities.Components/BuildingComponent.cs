using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class BuildingComponent : AssetComponent
	{
		public const string SPARK_FX_ID = "effect203";

		public BuildingTypeVO BuildingType
		{
			get;
			set;
		}

		public Building BuildingTO
		{
			get;
			set;
		}

		public BuildingComponent(BuildingTypeVO buildingType, Building buildingTO) : base(buildingType.AssetName)
		{
			this.BuildingType = buildingType;
			this.BuildingTO = buildingTO;
		}

		protected internal BuildingComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingComponent)GCHandledObjects.GCHandleToObject(instance)).BuildingTO);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingComponent)GCHandledObjects.GCHandleToObject(instance)).BuildingType);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BuildingComponent)GCHandledObjects.GCHandleToObject(instance)).BuildingTO = (Building)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BuildingComponent)GCHandledObjects.GCHandleToObject(instance)).BuildingType = (BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
