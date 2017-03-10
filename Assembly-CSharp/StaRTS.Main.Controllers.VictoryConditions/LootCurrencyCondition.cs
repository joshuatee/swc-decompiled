using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.VictoryConditions
{
	public class LootCurrencyCondition : AbstractCondition, IEventObserver
	{
		private const int AMOUNT_ARG = 0;

		private const int CURRENCY_TYPE_ARG = 1;

		private int currencyLooted;

		private int threshold;

		private CurrencyType currencyType;

		public LootCurrencyCondition(ConditionVO vo, IConditionParent parent, int startingValue) : base(vo, parent)
		{
			this.currencyLooted = startingValue;
			this.threshold = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.currencyType = StringUtils.ParseEnum<CurrencyType>(this.prepareArgs[1]);
		}

		public override void Start()
		{
			this.events.RegisterObserver(this, EventId.LootCollected, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.LootCollected)
			{
				LootData lootData = cookie as LootData;
				int num = 0;
				switch (this.currencyType)
				{
				case CurrencyType.Credits:
					num = lootData.Credits;
					break;
				case CurrencyType.Materials:
					num = lootData.Materials;
					break;
				case CurrencyType.Contraband:
					num = lootData.Contraband;
					break;
				}
				if (num > 0)
				{
					this.parent.ChildUpdated(this, num);
					this.IncrementCollection(num);
				}
			}
			return EatResponse.NotEaten;
		}

		private void IncrementCollection(int delta)
		{
			this.currencyLooted += delta;
			if (this.IsConditionSatisfied())
			{
				this.parent.ChildSatisfied(this);
			}
		}

		public override void Destroy()
		{
			this.events.UnregisterObserver(this, EventId.LootCollected);
		}

		public override bool IsConditionSatisfied()
		{
			return this.currencyLooted >= this.threshold;
		}

		public override void GetProgress(out int current, out int total)
		{
			current = this.currencyLooted;
			total = this.threshold;
		}

		protected internal LootCurrencyCondition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((LootCurrencyCondition)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((LootCurrencyCondition)GCHandledObjects.GCHandleToObject(instance)).IncrementCollection(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootCurrencyCondition)GCHandledObjects.GCHandleToObject(instance)).IsConditionSatisfied());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootCurrencyCondition)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LootCurrencyCondition)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
