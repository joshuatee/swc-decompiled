using Net.RichardLord.Ash.Core;
using StaRTS.FX;
using StaRTS.GameBoard;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class EntityRenderController
	{
		public const string ANIM_PROP_MOTIVATOR = "Motivation";

		public const string FX_SMOKE = "effectSmoke";

		private const float ROTATION_OFFSET_TROOP = -1.57079637f;

		public EntityRenderController()
		{
			Service.Set<EntityRenderController>(this);
		}

		public void UpdateNewEntityView(SmartEntity entity)
		{
			GameObjectViewComponent gameObjectViewComp = entity.GameObjectViewComp;
			TransformComponent transformComp = entity.TransformComp;
			if (gameObjectViewComp == null || transformComp == null)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Entity is not ready for rendering. view: {0}, transform: {1}, entity: {2}", new object[]
				{
					(gameObjectViewComp == null) ? "null" : gameObjectViewComp.ToString(),
					(transformComp == null) ? "null" : transformComp.ToString(),
					entity.ID
				});
				return;
			}
			gameObjectViewComp.MainTransform.position = new Vector3(Units.BoardToWorldX(transformComp.CenterX()), gameObjectViewComp.MainTransform.position.y, Units.BoardToWorldZ(transformComp.CenterZ()));
			if (entity.TroopComp != null || entity.DroidComp != null)
			{
				this.RotateGameObject(entity, 0f, 0f, 0f);
				float speed = Service.Get<BattlePlaybackController>().CurrentPlaybackScale;
				this.UpdateAnimationSpeed(gameObjectViewComp, speed);
				this.UpdateAnimationState(gameObjectViewComp, entity.StateComp);
				return;
			}
			if (entity.TrapComp != null)
			{
				gameObjectViewComp.MainGameObject.SetActive(true);
				Service.Get<TrapController>().SetTrapState(entity.TrapComp, entity.TrapComp.CurrentState);
				Service.Get<TrapViewController>().UpdateTrapVisibility(entity);
				gameObjectViewComp.Rotate(transformComp.Rotation);
				return;
			}
			gameObjectViewComp.Rotate(transformComp.Rotation);
			gameObjectViewComp.MainGameObject.SetActive(true);
		}

		public void SetSpeed(float speed)
		{
			NodeList<EntityRenderNode> nodeList = Service.Get<EntityController>().GetNodeList<EntityRenderNode>();
			for (EntityRenderNode entityRenderNode = nodeList.Head; entityRenderNode != null; entityRenderNode = entityRenderNode.Next)
			{
				this.UpdateAnimationSpeed(entityRenderNode.View, speed);
			}
			NodeList<TrackingRenderNode> nodeList2 = Service.Get<EntityController>().GetNodeList<TrackingRenderNode>();
			for (TrackingRenderNode trackingRenderNode = nodeList2.Head; trackingRenderNode != null; trackingRenderNode = trackingRenderNode.Next)
			{
				trackingRenderNode.TrackingView.Speed = speed;
			}
		}

		public void UpdateAnimationSpeed(GameObjectViewComponent view, float speed)
		{
			Animator component = view.MainGameObject.GetComponent<Animator>();
			if (component != null)
			{
				component.speed = speed;
			}
		}

		public Vector3 MoveGameObject(GameObjectViewComponent view, PathView pathView, int troopWidth)
		{
			BoardCell<Entity> nextTurn = pathView.GetNextTurn();
			if (nextTurn != null && pathView.TimeToTarget > 0f)
			{
				float num = 0f;
				float num2 = 0f;
				pathView.GetTroopClusterOffset(ref num, ref num2);
				float num3 = Mathf.Min(pathView.TimeOnPathSegment / pathView.TimeToTarget, 1f);
				Vector3 startPos = pathView.StartPos;
				float num4 = (Units.BoardToWorldX(nextTurn.X) - startPos.x) * num3 + startPos.x + num;
				float num5 = (Units.BoardToWorldX(nextTurn.Z) - startPos.z) * num3 + startPos.z + num2;
				num4 += Units.BoardToWorldX((float)troopWidth / 2f);
				num5 += Units.BoardToWorldX((float)troopWidth / 2f);
				Transform mainTransform = view.MainTransform;
				Vector3 result = new Vector3(num4 - mainTransform.position.x, 0f, num5 - mainTransform.position.z);
				view.SetXYZ(num4, mainTransform.position.y, num5);
				return result;
			}
			return Vector3.zero;
		}

		public void RotateGameObject(SmartEntity entity, float dx, float dz, float dt)
		{
			GameObjectViewComponent gameObjectViewComp = entity.GameObjectViewComp;
			TransformComponent transformComp = entity.TransformComp;
			if (entity.TroopComp != null && entity.TroopComp.TroopShooterVO.TargetSelf && transformComp.RotationInitialized)
			{
				return;
			}
			float num = transformComp.Rotation;
			bool flag = dx != 0f || dz != 0f;
			SmartEntity smartEntity = null;
			GameObjectViewComponent gameObjectViewComponent = null;
			SecondaryTargetsComponent secondaryTargetsComp = entity.SecondaryTargetsComp;
			ShooterComponent shooterComp = entity.ShooterComp;
			if (secondaryTargetsComp != null && shooterComp != null)
			{
				ShooterController shooterController = Service.Get<ShooterController>();
				smartEntity = shooterController.GetTroopTarget(secondaryTargetsComp, shooterComp);
				if (smartEntity != null && (gameObjectViewComponent = smartEntity.GameObjectViewComp) == null)
				{
					smartEntity = shooterController.GetPrimaryTarget(shooterComp);
					if (smartEntity != null)
					{
						gameObjectViewComponent = smartEntity.GameObjectViewComp;
					}
				}
			}
			else
			{
				PathingComponent pathingComp = entity.PathingComp;
				if (pathingComp != null)
				{
					smartEntity = pathingComp.Target;
					if (smartEntity != null)
					{
						gameObjectViewComponent = smartEntity.GameObjectViewComp;
					}
				}
			}
			bool flag2 = !transformComp.RotationInitialized;
			if (flag2 && smartEntity == null && (entity.DefenderComp != null || TroopController.IsEntityHealer(entity)))
			{
				gameObjectViewComp.MainGameObject.SetActive(true);
				transformComp.RotationInitialized = true;
			}
			else
			{
				bool flag3 = flag2 || entity.StateComp.CurState == EntityState.Attacking || entity.StateComp.CurState == EntityState.Turning;
				if (flag3 && gameObjectViewComponent != null)
				{
					gameObjectViewComp.ComputeRotationToTarget(gameObjectViewComponent, ref num);
					flag = false;
					if (flag2)
					{
						gameObjectViewComp.MainGameObject.SetActive(true);
						transformComp.Rotation = num;
						transformComp.RotationInitialized = true;
					}
				}
			}
			if (flag)
			{
				num = Mathf.Atan2(dz, dx);
			}
			num = MathUtils.MinAngle(transformComp.Rotation, num);
			float num2 = (float)entity.WalkerComp.SpeedVO.RotationSpeed / 1000f;
			float smoothTime = 0.7853982f / num2;
			float num3 = (dt == 0f) ? transformComp.Rotation : Mathf.SmoothDampAngle(transformComp.Rotation, num, ref transformComp.RotationVelocity, smoothTime, num2, dt);
			num3 = MathUtils.WrapAngle(num3);
			gameObjectViewComp.Rotate(num3 + -1.57079637f);
			transformComp.Rotation = num3;
		}

		public void SetTroopRotation(Transform transform, float degrees)
		{
			float num = -90f;
			transform.rotation = Quaternion.AngleAxis(degrees - num, Vector3.up);
		}

		public void UpdateChampionAnimationStateInHomeOrWarBoardMode(SmartEntity entity, bool underRepair)
		{
			if (entity.GameObjectViewComp == null)
			{
				return;
			}
			EffectsTypeVO effectsTypeVO = Service.Get<IDataController>().Get<EffectsTypeVO>("effectSmoke");
			Animator component = entity.GameObjectViewComp.MainGameObject.GetComponent<Animator>();
			if (underRepair)
			{
				component.SetInteger("Motivation", 8);
				Service.Get<FXManager>().CreateAndAttachFXToEntity(entity, effectsTypeVO.AssetName, "effectSmoke" + entity.ID.ToString(), null, false, Vector3.zero, true);
				return;
			}
			component.SetInteger("Motivation", 0);
			Service.Get<FXManager>().RemoveAttachedFXFromEntity(entity, "effectSmoke" + entity.ID.ToString());
		}

		public void UpdateAnimationState(GameObjectViewComponent view, StateComponent stateComp)
		{
			this.UpdateAnimationState(view, stateComp, false);
		}

		public void UpdateAnimationState(GameObjectViewComponent view, StateComponent stateComp, bool isAbilityModeActive)
		{
			if (stateComp.Dirty || stateComp.ForceUpdateAnimation)
			{
				stateComp.ForceUpdateAnimation = false;
				if (!view.MainGameObject.activeInHierarchy)
				{
					return;
				}
				Animator component = view.MainGameObject.GetComponent<Animator>();
				if (component == null)
				{
					return;
				}
				while (stateComp.Dirty)
				{
					EntityState entityState = stateComp.DequeuePrevState();
					if (entityState == EntityState.AttackingReset)
					{
						component.Play("", 0, 0f);
					}
				}
				switch (stateComp.CurState)
				{
				case EntityState.Disable:
					component.SetInteger("Motivation", 0);
					return;
				case EntityState.Idle:
					component.SetInteger("Motivation", 0);
					return;
				case EntityState.Moving:
					if (stateComp.IsRunning)
					{
						component.SetInteger("Motivation", 9);
						return;
					}
					component.SetInteger("Motivation", 1);
					return;
				case EntityState.Tracking:
					break;
				case EntityState.Turning:
					component.SetInteger("Motivation", 1);
					return;
				case EntityState.WarmingUp:
					if (isAbilityModeActive)
					{
						component.SetInteger("Motivation", 5);
						return;
					}
					component.SetInteger("Motivation", 4);
					return;
				case EntityState.Attacking:
				case EntityState.AttackingReset:
					if (isAbilityModeActive)
					{
						component.SetInteger("Motivation", 6);
						return;
					}
					component.SetInteger("Motivation", 3);
					return;
				case EntityState.CoolingDown:
					if (isAbilityModeActive)
					{
						component.SetInteger("Motivation", 7);
						return;
					}
					component.SetInteger("Motivation", 4);
					return;
				case EntityState.Dying:
					component.SetInteger("Motivation", stateComp.DeathAnimationID);
					break;
				default:
					return;
				}
			}
		}

		protected internal EntityRenderController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityRenderController)GCHandledObjects.GCHandleToObject(instance)).MoveGameObject((GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args), (PathView)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EntityRenderController)GCHandledObjects.GCHandleToObject(instance)).RotateGameObject((SmartEntity)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EntityRenderController)GCHandledObjects.GCHandleToObject(instance)).SetSpeed(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EntityRenderController)GCHandledObjects.GCHandleToObject(instance)).SetTroopRotation((Transform)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EntityRenderController)GCHandledObjects.GCHandleToObject(instance)).UpdateAnimationSpeed((GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EntityRenderController)GCHandledObjects.GCHandleToObject(instance)).UpdateAnimationState((GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args), (StateComponent)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EntityRenderController)GCHandledObjects.GCHandleToObject(instance)).UpdateAnimationState((GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args), (StateComponent)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((EntityRenderController)GCHandledObjects.GCHandleToObject(instance)).UpdateChampionAnimationStateInHomeOrWarBoardMode((SmartEntity)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((EntityRenderController)GCHandledObjects.GCHandleToObject(instance)).UpdateNewEntityView((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
