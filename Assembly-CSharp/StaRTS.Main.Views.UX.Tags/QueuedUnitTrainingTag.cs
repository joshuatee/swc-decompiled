using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Tags
{
	public class QueuedUnitTrainingTag
	{
		public List<Contract> Contracts
		{
			get;
			set;
		}

		public IDeployableVO UnitVO
		{
			get;
			set;
		}

		public float TimeLeftFloat
		{
			get;
			set;
		}

		public int TimeLeft
		{
			get
			{
				if (this.Contracts.Count == 0)
				{
					return 0;
				}
				return this.Contracts[0].GetRemainingTimeForView();
			}
		}

		public int TimeTotal
		{
			get
			{
				if (this.Contracts.Count == 0)
				{
					return 0;
				}
				return this.Contracts[0].TotalTime;
			}
		}

		public QueuedUnitTrainingTag()
		{
			this.TimeLeftFloat = 0f;
			this.Contracts = new List<Contract>();
		}

		protected internal QueuedUnitTrainingTag(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueuedUnitTrainingTag)GCHandledObjects.GCHandleToObject(instance)).Contracts);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueuedUnitTrainingTag)GCHandledObjects.GCHandleToObject(instance)).TimeLeft);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueuedUnitTrainingTag)GCHandledObjects.GCHandleToObject(instance)).TimeLeftFloat);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueuedUnitTrainingTag)GCHandledObjects.GCHandleToObject(instance)).TimeTotal);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueuedUnitTrainingTag)GCHandledObjects.GCHandleToObject(instance)).UnitVO);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((QueuedUnitTrainingTag)GCHandledObjects.GCHandleToObject(instance)).Contracts = (List<Contract>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((QueuedUnitTrainingTag)GCHandledObjects.GCHandleToObject(instance)).TimeLeftFloat = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((QueuedUnitTrainingTag)GCHandledObjects.GCHandleToObject(instance)).UnitVO = (IDeployableVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
