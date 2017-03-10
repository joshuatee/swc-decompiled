using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.VictoryConditions
{
	public class OwnResourceCondition : AbstractCondition, IEventObserver
	{
		private const int AMOUNT_ARG = 0;

		private const int RESOURCE_ARG = 1;

		private int threshold;

		private string resourceKey;

		private bool observingEvents;

		public OwnResourceCondition(ConditionVO vo, IConditionParent parent) : base(vo, parent)
		{
			this.resourceKey = this.prepareArgs[1];
			this.threshold = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
		}

		public override void GetProgress(out int current, out int total)
		{
			total = this.threshold;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (currentPlayer.Inventory.HasItem(this.resourceKey))
			{
				current = currentPlayer.Inventory.GetItemAmount(this.resourceKey);
				return;
			}
			current = 0;
		}

		public override void Start()
		{
			if (this.IsConditionSatisfied())
			{
				this.parent.ChildSatisfied(this);
				return;
			}
			this.events.RegisterObserver(this, EventId.InventoryResourceUpdated, EventPriority.Default);
			this.observingEvents = true;
		}

		public override void Destroy()
		{
			if (this.observingEvents)
			{
				this.events.UnregisterObserver(this, EventId.InventoryResourceUpdated);
			}
		}

		public override bool IsConditionSatisfied()
		{
			int num;
			int num2;
			this.GetProgress(out num, out num2);
			return num >= num2;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (this.resourceKey == (string)cookie && this.IsConditionSatisfied())
			{
				this.parent.ChildSatisfied(this);
			}
			return EatResponse.NotEaten;
		}

		protected internal OwnResourceCondition(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((OwnResourceCondition)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OwnResourceCondition)GCHandledObjects.GCHandleToObject(instance)).IsConditionSatisfied());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OwnResourceCondition)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((OwnResourceCondition)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
