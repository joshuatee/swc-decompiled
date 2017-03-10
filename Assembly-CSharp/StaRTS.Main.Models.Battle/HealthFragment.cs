using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Entities;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle
{
	public class HealthFragment
	{
		public HealthType Type
		{
			get;
			private set;
		}

		public int Quantity
		{
			get;
			private set;
		}

		public int SplashQuantity
		{
			get;
			private set;
		}

		public HealthFragment(SmartEntity source, HealthType type, int quantity)
		{
			this.Type = type;
			int splashQuantity = quantity;
			if (type != HealthType.Healing && source != null)
			{
				int modifyValueMax = quantity;
				Service.Get<BuffController>().ApplyActiveBuffs(source, BuffModify.Damage, ref quantity, modifyValueMax);
				splashQuantity = quantity;
				Service.Get<BuffController>().ApplyActiveBuffs(source, BuffModify.SplashDamage, ref splashQuantity, modifyValueMax);
			}
			this.Quantity = quantity;
			this.SplashQuantity = splashQuantity;
		}

		protected internal HealthFragment(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthFragment)GCHandledObjects.GCHandleToObject(instance)).Quantity);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthFragment)GCHandledObjects.GCHandleToObject(instance)).SplashQuantity);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthFragment)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HealthFragment)GCHandledObjects.GCHandleToObject(instance)).Quantity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((HealthFragment)GCHandledObjects.GCHandleToObject(instance)).SplashQuantity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((HealthFragment)GCHandledObjects.GCHandleToObject(instance)).Type = (HealthType)(*(int*)args);
			return -1L;
		}
	}
}
