using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;

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
				Service.Get<Logger>().Warn(string.Concat(new object[]
				{
					"AllowPvE is not supported for objectiveType: ",
					this.objectiveVO.ObjectiveType,
					" id: ",
					this.objectiveVO.Uid
				}));
			}
		}
	}
}
