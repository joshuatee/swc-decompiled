using StaRTS.Externals.Manimal;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Utils;
using System;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class Contract
	{
		public string Tag;

		private int internalRemainingTime;

		public string ProductUid
		{
			get;
			private set;
		}

		public DeliveryType DeliveryType
		{
			get;
			private set;
		}

		public int TotalTime
		{
			get;
			private set;
		}

		public ContractTO ContractTO
		{
			get;
			set;
		}

		public double LastUpdateTime
		{
			get;
			set;
		}

		public bool DoNotShiftTimesForUnfreeze
		{
			get;
			set;
		}

		public Contract(string productUid, DeliveryType deliveryType, int totalTime, double startTime) : this(productUid, deliveryType, totalTime, startTime, "")
		{
		}

		public Contract(string productUid, DeliveryType deliveryType, int totalTime, double startTime, string tag)
		{
			this.ProductUid = productUid;
			this.DeliveryType = deliveryType;
			this.TotalTime = totalTime;
			this.internalRemainingTime = totalTime;
			this.LastUpdateTime = startTime;
			this.DoNotShiftTimesForUnfreeze = false;
			this.Tag = tag;
		}

		public void UpdateRemainingTime()
		{
			this.internalRemainingTime = this.GetRemainingTimeForSim();
		}

		public bool IsReadyToBeFinished()
		{
			return this.internalRemainingTime <= 0;
		}

		public int GetRemainingTimeForView()
		{
			return this.internalRemainingTime;
		}

		public int GetRemainingTimeForSim()
		{
			int num = 0;
			if (this.ContractTO != null)
			{
				uint endTime = this.ContractTO.EndTime;
				uint time = ServerTime.Time;
				if (endTime > time)
				{
					num = GameUtils.GetTimeDifferenceSafe(endTime, time);
					if (num > this.TotalTime)
					{
						num = this.TotalTime;
					}
				}
			}
			return num;
		}

		public void AddString(StringBuilder sb)
		{
			sb.Append(this.ContractTO.Uid).Append("|").Append(this.ContractTO.BuildingKey).Append("|").Append(this.ContractTO.EndTime).Append("|").Append(this.ContractTO.ContractType.ToString()).Append("|").Append("\n");
		}

		protected internal Contract(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Contract)GCHandledObjects.GCHandleToObject(instance)).AddString((StringBuilder)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Contract)GCHandledObjects.GCHandleToObject(instance)).ContractTO);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Contract)GCHandledObjects.GCHandleToObject(instance)).DeliveryType);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Contract)GCHandledObjects.GCHandleToObject(instance)).DoNotShiftTimesForUnfreeze);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Contract)GCHandledObjects.GCHandleToObject(instance)).ProductUid);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Contract)GCHandledObjects.GCHandleToObject(instance)).TotalTime);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Contract)GCHandledObjects.GCHandleToObject(instance)).GetRemainingTimeForSim());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Contract)GCHandledObjects.GCHandleToObject(instance)).GetRemainingTimeForView());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Contract)GCHandledObjects.GCHandleToObject(instance)).IsReadyToBeFinished());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((Contract)GCHandledObjects.GCHandleToObject(instance)).ContractTO = (ContractTO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((Contract)GCHandledObjects.GCHandleToObject(instance)).DeliveryType = (DeliveryType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((Contract)GCHandledObjects.GCHandleToObject(instance)).DoNotShiftTimesForUnfreeze = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((Contract)GCHandledObjects.GCHandleToObject(instance)).LastUpdateTime = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((Contract)GCHandledObjects.GCHandleToObject(instance)).ProductUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((Contract)GCHandledObjects.GCHandleToObject(instance)).TotalTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((Contract)GCHandledObjects.GCHandleToObject(instance)).UpdateRemainingTime();
			return -1L;
		}
	}
}
