using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class Buff
	{
		private int msRemaining;

		private int msToNextProc;

		private int armorTypeIndex;

		public BuffTypeVO BuffType
		{
			get;
			private set;
		}

		public int ProcCount
		{
			get;
			private set;
		}

		public int StackSize
		{
			get;
			private set;
		}

		public BuffVisualPriority VisualPriority
		{
			get;
			private set;
		}

		public Buff(BuffTypeVO buffType, ArmorType armorType, BuffVisualPriority visualPriority)
		{
			this.BuffType = buffType;
			this.ProcCount = 0;
			this.StackSize = 0;
			this.msRemaining = this.BuffType.Duration;
			this.msToNextProc = this.BuffType.MillisecondsToFirstProc;
			this.armorTypeIndex = (int)armorType;
			this.VisualPriority = visualPriority;
		}

		public void AddStack()
		{
			if (this.BuffType.MaxStacks == 0u || this.StackSize < (int)this.BuffType.MaxStacks)
			{
				int stackSize = this.StackSize;
				this.StackSize = stackSize + 1;
			}
			if (this.BuffType.IsRefreshing)
			{
				this.msRemaining = this.BuffType.Duration;
				this.msToNextProc = this.BuffType.MillisecondsToFirstProc;
			}
		}

		public bool RemoveStack()
		{
			if (this.StackSize != 0)
			{
				int num = this.StackSize - 1;
				this.StackSize = num;
				return num == 0;
			}
			return true;
		}

		public void UpgradeBuff(BuffTypeVO newBuffType)
		{
			this.UpgradeTime(ref this.msRemaining, this.BuffType.Duration, newBuffType.Duration);
			if (this.ProcCount == 0)
			{
				this.UpgradeTime(ref this.msToNextProc, this.BuffType.MillisecondsToFirstProc, newBuffType.MillisecondsToFirstProc);
			}
			else
			{
				this.UpgradeTime(ref this.msToNextProc, this.BuffType.MillisecondsPerProc, newBuffType.MillisecondsPerProc);
			}
			this.BuffType = newBuffType;
		}

		private void UpgradeTime(ref int msTimeLeft, int oldTotalTime, int newTotalTime)
		{
			if (msTimeLeft < 0 || newTotalTime < 0)
			{
				msTimeLeft = newTotalTime;
				return;
			}
			msTimeLeft += newTotalTime - oldTotalTime;
			if (msTimeLeft < 0)
			{
				msTimeLeft = 0;
			}
		}

		public void UpdateBuff(uint dt, out bool proc, out bool done)
		{
			proc = false;
			done = false;
			int num = 0;
			if (this.msRemaining >= 0)
			{
				this.msRemaining -= (int)dt;
				if (this.msRemaining <= 0)
				{
					done = true;
					num = this.msRemaining;
					this.msRemaining = 0;
				}
			}
			if (this.msToNextProc >= 0)
			{
				this.msToNextProc -= (int)dt;
				if (this.msToNextProc <= 0)
				{
					if (this.msToNextProc <= num)
					{
						proc = true;
						int procCount = this.ProcCount;
						this.ProcCount = procCount + 1;
					}
					num = this.msToNextProc;
					this.msToNextProc = this.BuffType.MillisecondsPerProc + num;
					if (this.msToNextProc < 0 && this.BuffType.MillisecondsPerProc >= 0)
					{
						this.msToNextProc = 0;
					}
				}
			}
		}

		public void ApplyStacks(ref int modifyValue, int modifyValueMax)
		{
			int num = this.BuffType.Values[this.armorTypeIndex];
			switch (this.BuffType.ApplyAs)
			{
			case BuffApplyAs.Relative:
				modifyValue += this.StackSize * num;
				return;
			case BuffApplyAs.Absolute:
				modifyValue = this.StackSize * num;
				return;
			case BuffApplyAs.RelativePercent:
				for (int i = 0; i < this.StackSize; i++)
				{
					modifyValue += IntMath.GetPercent(num, modifyValue);
				}
				return;
			case BuffApplyAs.AbsolutePercent:
				for (int j = 0; j < this.StackSize; j++)
				{
					modifyValue = IntMath.GetPercent(num, modifyValue);
				}
				return;
			case BuffApplyAs.RelativePercentOfMax:
				modifyValue += this.StackSize * IntMath.GetPercent(num, modifyValueMax);
				return;
			case BuffApplyAs.AbsolutePercentOfMax:
				modifyValue = this.StackSize * IntMath.GetPercent(num, modifyValueMax);
				return;
			default:
				return;
			}
		}

		protected internal Buff(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Buff)GCHandledObjects.GCHandleToObject(instance)).AddStack();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Buff)GCHandledObjects.GCHandleToObject(instance)).BuffType);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Buff)GCHandledObjects.GCHandleToObject(instance)).ProcCount);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Buff)GCHandledObjects.GCHandleToObject(instance)).StackSize);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Buff)GCHandledObjects.GCHandleToObject(instance)).VisualPriority);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Buff)GCHandledObjects.GCHandleToObject(instance)).RemoveStack());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((Buff)GCHandledObjects.GCHandleToObject(instance)).BuffType = (BuffTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((Buff)GCHandledObjects.GCHandleToObject(instance)).ProcCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((Buff)GCHandledObjects.GCHandleToObject(instance)).StackSize = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((Buff)GCHandledObjects.GCHandleToObject(instance)).VisualPriority = (BuffVisualPriority)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((Buff)GCHandledObjects.GCHandleToObject(instance)).UpgradeBuff((BuffTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
