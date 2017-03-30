using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using System;

namespace StaRTS.Main.Controllers.Objectives
{
	public static class ObjectiveFactory
	{
		public static AbstractObjectiveProcessor GetProcessor(ObjectiveVO vo, ObjectiveManager parent)
		{
			switch (vo.ObjectiveType)
			{
			case ObjectiveType.Loot:
				return new LootObjectiveProcessor(vo, parent);
			case ObjectiveType.DestroyBuildingType:
				return new DestroyBuildingTypeObjectiveProcessor(vo, parent);
			case ObjectiveType.DestroyBuildingID:
				return new DestroyBuildingIdObjectiveProcessor(vo, parent);
			case ObjectiveType.DeployTroopType:
				return new DeployTroopTypeObjectiveProcessor(vo, parent);
			case ObjectiveType.DeployTroopID:
				return new DeployTroopIdObjectiveProcessor(vo, parent);
			case ObjectiveType.TrainTroopType:
				return new TrainTroopTypeObjectiveProcessor(vo, parent);
			case ObjectiveType.TrainTroopID:
				return new TrainTroopIdObjectiveProcessor(vo, parent);
			case ObjectiveType.ReceiveDonatedTroops:
				return new ReceiveSquadUnitObjectiveProcessor(vo, parent);
			case ObjectiveType.DeploySpecialAttack:
				return new DeploySpecialAttackObjectiveProcessor(vo, parent);
			case ObjectiveType.DeploySpecialAttackID:
				return new DeploySpecialAttackIdObjectiveProcessor(vo, parent);
			case ObjectiveType.TrainSpecialAttack:
				return new TrainSpecialAttackObjectiveProcessor(vo, parent);
			case ObjectiveType.TrainSpecialAttackID:
				return new TrainSpecialAttackIdObjectiveProcessor(vo, parent);
			case ObjectiveType.DonateTroopType:
				return new DonateTroopTypeObjectiveProcessor(vo, parent);
			case ObjectiveType.DonateTroopID:
				return new DonateTroopIdObjectiveProcessor(vo, parent);
			case ObjectiveType.DonateTroop:
				return new DonateTroopObjectiveProcessor(vo, parent);
			default:
				return new AbstractObjectiveProcessor(vo, parent);
			}
		}
	}
}
