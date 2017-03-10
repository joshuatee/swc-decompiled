using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Perks
{
	public class CurrencyPerkEffectDataTO
	{
		public float RateBonus
		{
			get;
			private set;
		}

		public uint StartTime
		{
			get;
			private set;
		}

		public uint EndTime
		{
			get;
			private set;
		}

		public int Duration
		{
			get;
			private set;
		}

		public CurrencyPerkEffectDataTO(float rate, uint start, uint end)
		{
			this.RateBonus = rate;
			this.StartTime = start;
			this.EndTime = end;
			if (start > end)
			{
				Service.Get<StaRTSLogger>().Error(string.Concat(new object[]
				{
					"Bad CurrencyPerkEffectDataTO time data: End ",
					end,
					" - Start ",
					start
				}));
			}
			this.Duration = (int)(end - start);
		}

		public void AdjustRateBonus(float delta)
		{
			this.RateBonus += delta;
		}

		protected internal CurrencyPerkEffectDataTO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CurrencyPerkEffectDataTO)GCHandledObjects.GCHandleToObject(instance)).AdjustRateBonus(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyPerkEffectDataTO)GCHandledObjects.GCHandleToObject(instance)).Duration);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyPerkEffectDataTO)GCHandledObjects.GCHandleToObject(instance)).RateBonus);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CurrencyPerkEffectDataTO)GCHandledObjects.GCHandleToObject(instance)).Duration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((CurrencyPerkEffectDataTO)GCHandledObjects.GCHandleToObject(instance)).RateBonus = *(float*)args;
			return -1L;
		}
	}
}
