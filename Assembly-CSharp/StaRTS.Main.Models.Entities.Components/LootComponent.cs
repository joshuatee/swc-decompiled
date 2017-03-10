using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class LootComponent : ComponentBase
	{
		public int[] LootQuantities
		{
			get;
			protected set;
		}

		public int AttackCount
		{
			get;
			protected set;
		}

		public LootComponent()
		{
			int num = 6;
			this.LootQuantities = new int[num];
			for (int i = 0; i < num; i++)
			{
				this.LootQuantities[i] = 0;
			}
		}

		public void SetLootQuantity(CurrencyType type, int quantity)
		{
			this.LootQuantities[(int)type] = quantity;
		}

		public void IncParticleCount()
		{
			int attackCount = this.AttackCount;
			this.AttackCount = attackCount + 1;
		}

		protected internal LootComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootComponent)GCHandledObjects.GCHandleToObject(instance)).AttackCount);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootComponent)GCHandledObjects.GCHandleToObject(instance)).LootQuantities);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((LootComponent)GCHandledObjects.GCHandleToObject(instance)).IncParticleCount();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LootComponent)GCHandledObjects.GCHandleToObject(instance)).AttackCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LootComponent)GCHandledObjects.GCHandleToObject(instance)).LootQuantities = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((LootComponent)GCHandledObjects.GCHandleToObject(instance)).SetLootQuantity((CurrencyType)(*(int*)args), *(int*)(args + 1));
			return -1L;
		}
	}
}
