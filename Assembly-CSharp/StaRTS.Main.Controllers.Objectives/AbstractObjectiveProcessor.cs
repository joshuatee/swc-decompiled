using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Objectives
{
	public class AbstractObjectiveProcessor
	{
		protected ObjectiveManager parent;

		protected ObjectiveVO objectiveVO;

		public AbstractObjectiveProcessor(ObjectiveVO objectiveVO, ObjectiveManager parent)
		{
			this.objectiveVO = objectiveVO;
			this.parent = parent;
		}

		protected void RecordProgress(int amount)
		{
			this.parent.Progress(this, amount);
		}

		public virtual void Destroy()
		{
		}

		protected bool IsEventValidForBattleObjective()
		{
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			if (currentBattle != null)
			{
				if (currentBattle.Type == BattleType.ClientBattle || currentBattle.Type == BattleType.PveFue)
				{
					return false;
				}
				if (!this.objectiveVO.AllowPvE && (currentBattle.Type == BattleType.PveDefend || currentBattle.Type == BattleType.PveAttack || currentBattle.Type == BattleType.PveBuffBase || currentBattle.Type == BattleType.PvpAttackSquadWar))
				{
					return false;
				}
				if (currentBattle.IsReplay)
				{
					return false;
				}
			}
			return true;
		}

		protected void CheckUnusedPveFlag()
		{
			if (this.objectiveVO.AllowPvE)
			{
				Service.Get<StaRTSLogger>().Warn(string.Concat(new object[]
				{
					"AllowPvE is not supported for objectiveType: ",
					this.objectiveVO.ObjectiveType,
					" id: ",
					this.objectiveVO.Uid
				}));
			}
		}

		protected internal AbstractObjectiveProcessor(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).CheckUnusedPveFlag();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).IsEventValidForBattleObjective());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractObjectiveProcessor)GCHandledObjects.GCHandleToObject(instance)).RecordProgress(*(int*)args);
			return -1L;
		}
	}
}
