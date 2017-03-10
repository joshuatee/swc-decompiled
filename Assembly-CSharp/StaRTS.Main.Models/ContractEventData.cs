using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class ContractEventData
	{
		public Contract Contract
		{
			get;
			private set;
		}

		public bool Silent
		{
			get;
			private set;
		}

		public bool SendBILog
		{
			get;
			private set;
		}

		public Entity Entity
		{
			get;
			private set;
		}

		public BuildingTypeVO BuildingVO
		{
			get;
			private set;
		}

		public string BuildingKey
		{
			get;
			set;
		}

		public ContractEventData(Contract contract, Entity entity, bool silent, bool sendBILog)
		{
			this.Contract = contract;
			this.Entity = entity;
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			this.BuildingVO = buildingComponent.BuildingType;
			this.BuildingKey = buildingComponent.BuildingTO.Key;
			this.Silent = silent;
			this.SendBILog = sendBILog;
		}

		public ContractEventData(Contract contract, Entity entity, bool silent)
		{
			this.Contract = contract;
			this.Entity = entity;
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			this.BuildingVO = buildingComponent.BuildingType;
			this.BuildingKey = buildingComponent.BuildingTO.Key;
			this.Silent = silent;
			this.SendBILog = true;
		}

		protected internal ContractEventData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).BuildingKey);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).BuildingVO);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).Contract);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).SendBILog);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).Silent);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).BuildingKey = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).BuildingVO = (BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).Contract = (Contract)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).SendBILog = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ContractEventData)GCHandledObjects.GCHandleToObject(instance)).Silent = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
