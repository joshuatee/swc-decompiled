using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Deployable
{
	public class DeployableContractRequest : PlayerIdChecksumRequest
	{
		public string ConstructorBuildingId
		{
			get;
			private set;
		}

		public string UnitTypeUid
		{
			get;
			private set;
		}

		public int Quantity
		{
			get;
			private set;
		}

		public DeployableContractRequest(string constructorBuildingId, string unitTypeUid, int quantity)
		{
			this.ConstructorBuildingId = constructorBuildingId;
			this.UnitTypeUid = unitTypeUid;
			this.Quantity = quantity;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("constructor", this.ConstructorBuildingId);
			startedSerializer.AddString("unitTypeId", this.UnitTypeUid);
			startedSerializer.Add<int>("quantity", this.Quantity);
			return startedSerializer.End().ToString();
		}

		protected internal DeployableContractRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableContractRequest)GCHandledObjects.GCHandleToObject(instance)).ConstructorBuildingId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableContractRequest)GCHandledObjects.GCHandleToObject(instance)).Quantity);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableContractRequest)GCHandledObjects.GCHandleToObject(instance)).UnitTypeUid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DeployableContractRequest)GCHandledObjects.GCHandleToObject(instance)).ConstructorBuildingId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DeployableContractRequest)GCHandledObjects.GCHandleToObject(instance)).Quantity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DeployableContractRequest)GCHandledObjects.GCHandleToObject(instance)).UnitTypeUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableContractRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
