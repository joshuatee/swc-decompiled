using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Objectives
{
	public class DeploySpecialAttackIdObjectiveProcessor : AbstractObjectiveProcessor, IEventObserver
	{
		private string specialAttackID;

		public DeploySpecialAttackIdObjectiveProcessor(ObjectiveVO vo, ObjectiveManager parent) : base(vo, parent)
		{
			this.specialAttackID = vo.Item;
			Service.Get<EventManager>().RegisterObserver(this, EventId.SpecialAttackSpawned);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.SpecialAttackSpawned && base.IsEventValidForBattleObjective())
			{
				SpecialAttack specialAttack = (SpecialAttack)cookie;
				if (specialAttack.VO.SpecialAttackID == this.specialAttackID)
				{
					this.parent.Progress(this, 1);
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.SpecialAttackSpawned);
			base.Destroy();
		}

		protected internal DeploySpecialAttackIdObjectiveProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeploySpecialAttackIdObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeploySpecialAttackIdObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
