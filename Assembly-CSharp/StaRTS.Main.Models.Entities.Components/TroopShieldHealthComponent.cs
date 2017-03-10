using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TroopShieldHealthComponent : ComponentBase, IHealthComponent
	{
		public const int HEALTH_QUANTITY_DEATH = 0;

		public int Health
		{
			get;
			set;
		}

		public int MaxHealth
		{
			get;
			set;
		}

		public ArmorType ArmorType
		{
			get;
			set;
		}

		public TroopShieldHealthComponent(int health, ArmorType armorType)
		{
			this.MaxHealth = health;
			this.Health = this.MaxHealth;
			this.ArmorType = armorType;
		}

		public TroopShieldHealthComponent()
		{
			this.Health = 0;
		}

		public bool IsDead()
		{
			return this.Health <= 0;
		}

		protected internal TroopShieldHealthComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopShieldHealthComponent)GCHandledObjects.GCHandleToObject(instance)).ArmorType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopShieldHealthComponent)GCHandledObjects.GCHandleToObject(instance)).Health);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopShieldHealthComponent)GCHandledObjects.GCHandleToObject(instance)).MaxHealth);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopShieldHealthComponent)GCHandledObjects.GCHandleToObject(instance)).IsDead());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TroopShieldHealthComponent)GCHandledObjects.GCHandleToObject(instance)).ArmorType = (ArmorType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TroopShieldHealthComponent)GCHandledObjects.GCHandleToObject(instance)).Health = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TroopShieldHealthComponent)GCHandledObjects.GCHandleToObject(instance)).MaxHealth = *(int*)args;
			return -1L;
		}
	}
}
