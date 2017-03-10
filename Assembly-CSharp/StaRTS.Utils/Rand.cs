using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Utils
{
	public class Rand
	{
		private Random viewRandom;

		private RandSimSeed simSeed;

		public RandSimSeed SimSeed
		{
			get
			{
				return this.simSeed;
			}
			set
			{
				this.simSeed = value;
				this.ValidateSimSeed(ref this.simSeed.SimSeedA);
				this.ValidateSimSeed(ref this.simSeed.SimSeedB);
			}
		}

		public float ViewValue
		{
			get
			{
				return (float)this.viewRandom.NextDouble();
			}
		}

		public Rand()
		{
			Service.Set<Rand>(this);
			this.viewRandom = new Random((int)DateTime.get_Now().get_Ticks());
			this.RandomizeSimSeed();
		}

		public RandSimSeed RandomizeSimSeed()
		{
			this.simSeed.SimSeedA = (uint)(this.viewRandom.Next(65536) << 16 | this.viewRandom.Next(65536));
			this.simSeed.SimSeedB = (uint)(this.viewRandom.Next(65536) << 16 | this.viewRandom.Next(65536));
			this.ValidateSimSeed(ref this.simSeed.SimSeedA);
			this.ValidateSimSeed(ref this.simSeed.SimSeedB);
			return this.simSeed;
		}

		private uint SimValue()
		{
			this.simSeed.SimSeedA = 36969u * (this.simSeed.SimSeedA & 65535u) + (this.simSeed.SimSeedA >> 16);
			this.simSeed.SimSeedB = 18000u * (this.simSeed.SimSeedB & 65535u) + (this.simSeed.SimSeedB >> 16);
			return (this.simSeed.SimSeedA << 16) + this.simSeed.SimSeedB;
		}

		private void ValidateSimSeed(ref uint seed)
		{
			if (seed == 0u)
			{
				seed = 1u;
			}
		}

		public int ViewRangeInt(int lowInclusive, int highExclusive)
		{
			return this.viewRandom.Next(lowInclusive, highExclusive);
		}

		public float ViewRangeFloat(float lowInclusive, float highExclusive)
		{
			double num = this.viewRandom.NextDouble();
			double num2 = (double)(highExclusive - lowInclusive);
			return lowInclusive + (float)(num * num2);
		}

		public int SimRange(int lowInclusive, int highExclusive)
		{
			uint num = this.SimValue();
			uint num2 = (uint)(highExclusive - lowInclusive);
			return lowInclusive + (int)(num % num2);
		}

		protected internal Rand(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Rand)GCHandledObjects.GCHandleToObject(instance)).SimSeed);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Rand)GCHandledObjects.GCHandleToObject(instance)).ViewValue);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Rand)GCHandledObjects.GCHandleToObject(instance)).RandomizeSimSeed());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Rand)GCHandledObjects.GCHandleToObject(instance)).SimSeed = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Rand)GCHandledObjects.GCHandleToObject(instance)).SimRange(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Rand)GCHandledObjects.GCHandleToObject(instance)).ViewRangeFloat(*(float*)args, *(float*)(args + 1)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Rand)GCHandledObjects.GCHandleToObject(instance)).ViewRangeInt(*(int*)args, *(int*)(args + 1)));
		}
	}
}
