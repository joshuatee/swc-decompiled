using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle
{
	public class Target
	{
		public const float DEFAULT_TARGET_WORLD_Y = 1.25f;

		public SmartEntity TargetEntity;

		public int TargetBoardX;

		public int TargetBoardZ;

		public Vector3 TargetWorldLocation;

		public static Target CreateTargetForTurret(SmartEntity targetEntity)
		{
			if (targetEntity == null)
			{
				return null;
			}
			TransformComponent transformComp = targetEntity.TransformComp;
			if (transformComp == null)
			{
				return null;
			}
			Target target = new Target();
			target.TargetEntity = targetEntity;
			target.TargetBoardX = transformComp.CenterGridX();
			target.TargetBoardZ = transformComp.CenterGridZ();
			GameObjectViewComponent gameObjectViewComp = targetEntity.GameObjectViewComp;
			if (gameObjectViewComp == null)
			{
				Target.SetDefaultTargetWorldLocation(target, 1.25f);
				return target;
			}
			target.TargetWorldLocation = UnityUtils.GetGameObjectBounds(gameObjectViewComp.MainGameObject).center;
			return target;
		}

		public static Target CreateTargetForTroop(SmartEntity troopEntity, SmartEntity targetEntity, SecondaryTargetsComponent secondaryTargets)
		{
			if (targetEntity == null)
			{
				return null;
			}
			Target target;
			if (secondaryTargets.ObstacleTarget != null)
			{
				target = new Target();
				target.TargetEntity = targetEntity;
				target.TargetBoardX = secondaryTargets.ObstacleTargetPoint.x;
				target.TargetBoardZ = secondaryTargets.ObstacleTargetPoint.z;
				Target.SetDefaultTargetWorldLocation(target, 1.25f);
				return target;
			}
			TransformComponent transformComponent = troopEntity.TroopComp.TroopType.IsHealer ? troopEntity.TransformComp : targetEntity.TransformComp;
			if (transformComponent == null)
			{
				return null;
			}
			target = new Target();
			target.TargetEntity = targetEntity;
			target.TargetBoardX = transformComponent.CenterGridX();
			target.TargetBoardZ = transformComponent.CenterGridZ();
			Target.SetDefaultTargetWorldLocation(target, 1.25f);
			return target;
		}

		public static Target CreateTargetWithWorldLocation(SmartEntity targetEntity, Vector3 targetWorldLoc)
		{
			return new Target
			{
				TargetEntity = targetEntity,
				TargetWorldLocation = targetWorldLoc
			};
		}

		private static void SetDefaultTargetWorldLocation(Target target, float targetWorldY)
		{
			target.TargetWorldLocation = new Vector3(Units.BoardToWorldX(target.TargetBoardX), targetWorldY, Units.BoardToWorldZ(target.TargetBoardZ));
		}

		public Target()
		{
		}

		protected internal Target(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Target.CreateTargetForTroop((SmartEntity)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1]), (SecondaryTargetsComponent)GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Target.CreateTargetForTurret((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Target.CreateTargetWithWorldLocation((SmartEntity)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			Target.SetDefaultTargetWorldLocation((Target)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}
	}
}
