using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Objectives
{
	public class LootObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private const int CREDITS = 0;

		private const int MATERIAL = 1;

		private const int CONTRABAND = 2;

		private int itemType;

		public LootObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			string item = vo.Item;
			if (!(item == "credits"))
			{
				if (!(item == "materials"))
				{
					if (item == "contraband")
					{
						this.itemType = 2;
					}
				}
				else
				{
					this.itemType = 1;
				}
			}
			else
			{
				this.itemType = 0;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.BattleEndProcessing);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.BattleEndProcessing && base.IsEventValidForBattleObjective())
			{
				CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
				int num = 0;
				if (this.itemType == 0)
				{
					num = currentBattle.LootCreditsEarned;
				}
				else if (this.itemType == 1)
				{
					num = currentBattle.LootMaterialsEarned;
				}
				else if (this.itemType == 2)
				{
					num = currentBattle.LootContrabandEarned;
				}
				if (num > 0)
				{
					this.parent.Progress(this, num);
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.BattleEndProcessing);
			base.Destroy();
		}

		protected internal LootObjectiveProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((LootObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
