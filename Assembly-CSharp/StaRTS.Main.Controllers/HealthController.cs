using Net.RichardLord.Ash.Core;
using StaRTS.FX;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class HealthController : IEventObserver
	{
		private const float FADE_OUT_DELAY = 5f;

		private const float FADE_OUT_TIME = 2f;

		private ViewFader entityFader;

		public HealthController()
		{
			Service.Set<HealthController>(this);
			this.entityFader = new ViewFader();
			Service.Get<EventManager>().RegisterObserver(this, EventId.ProcBuff, EventPriority.Default);
		}

		private int GetHealthMultiplier(IHealthComponent healthComp, List<int> damageMultipliers)
		{
			if (damageMultipliers != null)
			{
				int num = damageMultipliers[(int)healthComp.ArmorType];
				if (num >= 0)
				{
					return num;
				}
			}
			return 100;
		}

		public int ApplyHealthFragment(IHealthComponent healthComp, HealthFragment fragment, bool fromSplash)
		{
			return this.ApplyHealthFragment(healthComp, fragment, null, 100, fromSplash, false, null);
		}

		public int ApplyHealthFragment(IHealthComponent healthComponent, HealthFragment fragment, List<int> damageMultipliers, int damagePercentage, bool fromSplash, bool fromBeam, SmartEntity source)
		{
			if (healthComponent == null || healthComponent.IsDead() || damagePercentage == 0)
			{
				return 0;
			}
			int health = healthComponent.Health;
			int num = fragment.Quantity;
			if (fromSplash)
			{
				num = fragment.SplashQuantity;
			}
			int rawDamage = num;
			if (damageMultipliers != null)
			{
				int healthMultiplier = this.GetHealthMultiplier(healthComponent, damageMultipliers);
				num = num * healthMultiplier / 100;
			}
			SmartEntity target = (SmartEntity)((ComponentBase)healthComponent).Entity;
			BuffController buffController = Service.Get<BuffController>();
			if (fragment.Type == HealthType.Healing)
			{
				buffController.ApplyActiveBuffs(target, BuffModify.HealDefense, ref damagePercentage, 100);
			}
			else
			{
				buffController.ApplyActiveBuffs(target, BuffModify.Defense, ref damagePercentage, 100);
				if (fromSplash)
				{
					buffController.ApplyActiveBuffs(target, BuffModify.SplashDefense, ref damagePercentage, 100);
				}
			}
			num = IntMath.GetPercent(damagePercentage, num);
			if (fragment.Type == HealthType.Healing)
			{
				healthComponent.Health += num;
				if (healthComponent.Health > healthComponent.MaxHealth)
				{
					healthComponent.Health = healthComponent.MaxHealth;
				}
			}
			else
			{
				healthComponent.Health -= num;
				if (healthComponent.Health <= 0)
				{
					healthComponent.Health = 0;
				}
			}
			int num2 = healthComponent.Health - health;
			EntityHealthChangedData cookie = new EntityHealthChangedData(source, target, num2, rawDamage, fromBeam);
			Service.Get<EventManager>().SendEvent(EventId.EntityHealthChanged, cookie);
			this.HandleHealthChange(healthComponent, num2);
			return num2;
		}

		private void HandleHealthChange(IHealthComponent healthComponent, int delta)
		{
			if (healthComponent == null)
			{
				return;
			}
			SmartEntity smartEntity = (SmartEntity)((ComponentBase)healthComponent).Entity;
			if (smartEntity == null)
			{
				return;
			}
			LootController lootController = Service.Get<BattleController>().LootController;
			if (lootController == null)
			{
				return;
			}
			if (healthComponent is HealthComponent && delta < 0)
			{
				lootController.UpdateLootOnHealthChange(smartEntity, healthComponent as HealthComponent, delta);
			}
			if (!healthComponent.IsDead())
			{
				Service.Get<EntityViewManager>().CheckHealthView(smartEntity);
				return;
			}
			if (healthComponent != null && healthComponent.ArmorType == ArmorType.Shield && smartEntity.TroopShieldComp != null)
			{
				smartEntity.TroopShieldComp.Deactiviate();
				return;
			}
			if (smartEntity.TrapComp != null)
			{
				Service.Get<TrapController>().DestroyTrap(smartEntity);
			}
			this.KillEntity(smartEntity);
		}

		private void ProcBuffOnTarget(Buff buff, SmartEntity target)
		{
			BuffModify modify = buff.BuffType.Modify;
			if (modify != BuffModify.Health)
			{
				if (modify != BuffModify.Shield)
				{
					if (modify == BuffModify.MaxHealth)
					{
						HealthComponent healthComp = target.HealthComp;
						this.ApplyProcMaxHealthChange(healthComp, buff);
						return;
					}
				}
				else
				{
					IHealthComponent healthComp2 = null;
					if (target.ShieldBorderComp != null)
					{
						healthComp2 = target.HealthComp;
					}
					else if (target.TroopShieldComp != null && target.TroopShieldComp.IsActive())
					{
						healthComp2 = target.TroopShieldHealthComp;
					}
					this.ApplyProcHealthChange(healthComp2, buff);
				}
				return;
			}
			IHealthComponent arg_4F_0;
			if (target.TroopShieldComp == null || !target.TroopShieldComp.IsActive())
			{
				IHealthComponent healthComponent = target.HealthComp;
				arg_4F_0 = healthComponent;
			}
			else
			{
				IHealthComponent healthComponent = target.TroopShieldHealthComp;
				arg_4F_0 = healthComponent;
			}
			IHealthComponent healthComp3 = arg_4F_0;
			this.ApplyProcHealthChange(healthComp3, buff);
		}

		private void ApplyProcMaxHealthChange(HealthComponent healthComp, Buff buff)
		{
			if (healthComp == null)
			{
				return;
			}
			int maxHealth = healthComp.MaxHealth;
			buff.ApplyStacks(ref maxHealth, healthComp.MaxHealth);
			healthComp.MaxHealth = maxHealth;
			healthComp.Health = maxHealth;
		}

		private void ApplyProcHealthChange(IHealthComponent healthComp, Buff buff)
		{
			if (healthComp == null)
			{
				return;
			}
			SmartEntity source = null;
			int health = healthComp.Health;
			int num = health;
			buff.ApplyStacks(ref num, healthComp.MaxHealth);
			int num2 = num - health;
			if (num2 > 0)
			{
				HealthFragment fragment = new HealthFragment(source, HealthType.Healing, num2);
				this.ApplyHealthFragment(healthComp, fragment, false);
				return;
			}
			if (num2 < 0)
			{
				HealthFragment fragment2 = new HealthFragment(source, HealthType.Damaging, -num2);
				this.ApplyHealthFragment(healthComp, fragment2, false);
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ProcBuff)
			{
				BuffEventData buffEventData = (BuffEventData)cookie;
				this.ProcBuffOnTarget(buffEventData.BuffObj, buffEventData.Target);
			}
			return EatResponse.NotEaten;
		}

		public void KillEntity(Entity entity)
		{
			if (entity == null)
			{
				return;
			}
			SmartEntity smartEntity = (SmartEntity)entity;
			Service.Get<EventManager>().SendEvent(EventId.PreEntityKilled, entity);
			bool flag = smartEntity.TroopComp != null;
			bool flag2 = smartEntity.BuildingComp != null;
			bool flag3 = flag && smartEntity.TroopComp.TroopType.Type == TroopType.Hero;
			bool flag4 = smartEntity.TrapComp != null;
			bool flag5 = smartEntity.ChampionComp != null;
			BuildingType buildingType = BuildingType.Any;
			TroopTypeVO troopTypeVO = null;
			if (flag)
			{
				troopTypeVO = Service.Get<IDataController>().Get<TroopTypeVO>(smartEntity.TroopComp.TroopType.Uid);
			}
			StateComponent stateComp = smartEntity.StateComp;
			if (stateComp != null)
			{
				stateComp.CurState = EntityState.Dying;
				if (smartEntity.TroopComp != null && smartEntity.BuffComp != null && troopTypeVO.DeathAnimations != null)
				{
					int i = 0;
					int count = troopTypeVO.DeathAnimations.Count;
					while (i < count)
					{
						string key = troopTypeVO.DeathAnimations[i].get_Key();
						if (smartEntity.BuffComp.HasBuff(key))
						{
							int value = troopTypeVO.DeathAnimations[i].get_Value();
							stateComp.DeathAnimationID = value;
							break;
						}
						i++;
					}
				}
			}
			if (smartEntity.BuffComp != null)
			{
				smartEntity.BuffComp.Die();
			}
			entity.Remove<SecondaryTargetsComponent>();
			entity.Remove<TurretShooterComponent>();
			entity.Remove<ShooterComponent>();
			entity.Remove<HealthComponent>();
			entity.Remove<TrackingComponent>();
			entity.Remove<TrackingGameObjectViewComponent>();
			entity.Remove<HealthViewComponent>();
			entity.Remove<SupportViewComponent>();
			entity.Remove<TroopShieldComponent>();
			ShieldGeneratorComponent shieldGeneratorComp = smartEntity.ShieldGeneratorComp;
			ShieldBorderComponent shieldBorderComp = smartEntity.ShieldBorderComp;
			if (shieldGeneratorComp != null)
			{
				SmartEntity smartEntity2 = (SmartEntity)shieldGeneratorComp.ShieldBorderEntity;
				if (smartEntity2 != null)
				{
					Service.Get<ShieldController>().ShieldBorderKilled((SmartEntity)shieldGeneratorComp.ShieldBorderEntity);
					smartEntity2.Remove<ShieldBorderComponent>();
					this.KillEntity(smartEntity2);
				}
			}
			else if (shieldBorderComp != null)
			{
				Service.Get<ShieldController>().ShieldBorderKilled((SmartEntity)shieldBorderComp.Entity);
				SmartEntity smartEntity3 = (SmartEntity)shieldBorderComp.ShieldGeneratorEntity;
				if (smartEntity3 != null)
				{
					smartEntity3.Remove<ShieldGeneratorComponent>();
					this.KillEntity(smartEntity3);
				}
			}
			if (flag2 && smartEntity.DamageableComp != null)
			{
				entity.Remove<DamageableComponent>();
			}
			if (flag2 && !flag4 && smartEntity.SizeComp != null && smartEntity.GameObjectViewComp != null)
			{
				FXManager fXManager = Service.Get<FXManager>();
				fXManager.CreateRubbleAtEntityPosition(smartEntity);
				fXManager.CreateDestructionFX(smartEntity, true);
				buildingType = smartEntity.BuildingComp.BuildingType.Type;
			}
			Service.Get<EntityFactory>().RemoveEntity(entity, buildingType == BuildingType.Trap);
			entity.Remove<BoardItemComponent>();
			Service.Get<EventManager>().SendEvent(EventId.EntityKilled, entity);
			if (flag3)
			{
				Service.Get<EventManager>().SendEvent(EventId.HeroKilled, entity);
			}
			else if (flag5)
			{
				Service.Get<EventManager>().SendEvent(EventId.ChampionKilled, entity);
			}
			if (flag && troopTypeVO != null)
			{
				this.entityFader.FadeOut(entity, 5f, 2f, null, new FadingDelegate(this.OnFadeOutComplete));
				if (troopTypeVO.Type == TroopType.Vehicle && smartEntity.GameObjectViewComp != null)
				{
					Service.Get<FXManager>().CreateDestructionFX(smartEntity, false);
				}
				if (troopTypeVO.DeathProjectileType != null)
				{
					Vector3 spawnWorldLocation = Vector3.up * -1f;
					GameObjectViewComponent gameObjectViewComp = smartEntity.GameObjectViewComp;
					if (gameObjectViewComp != null)
					{
						spawnWorldLocation = gameObjectViewComp.MainTransform.position;
					}
					int num = 0;
					bool flag6;
					Service.Get<ProjectileController>().SpawnProjectileForDeath(spawnWorldLocation, smartEntity, troopTypeVO.DeathProjectileType, troopTypeVO.DeathProjectileDelay, troopTypeVO.DeathProjectileDistance, troopTypeVO.DeathProjectileDamage, out flag6, ref num);
					if (flag6 && gameObjectViewComp != null)
					{
						Service.Get<EntityRenderController>().SetTroopRotation(gameObjectViewComp.MainTransform, (float)num);
					}
				}
			}
			else if (!flag4)
			{
				Service.Get<EntityFactory>().DestroyEntity(entity, false, false);
			}
			if (flag2)
			{
				Service.Get<EventManager>().SendEvent(EventId.PostBuildingEntityKilled, buildingType);
			}
		}

		private void OnFadeOutComplete(object fadedObject)
		{
			Entity entity = (Entity)fadedObject;
			Service.Get<EntityFactory>().DestroyEntity(entity, true, false);
		}

		private void SwapModelsCallback(object asset, object cookie)
		{
			GameObjectViewComponent gameObjectViewComponent = cookie as GameObjectViewComponent;
			if (gameObjectViewComponent.MainGameObject == null)
			{
				return;
			}
			GameObject gameObject = asset as GameObject;
			Transform transform = gameObject.transform;
			transform.position = gameObjectViewComponent.MainTransform.position;
			transform.localEulerAngles = gameObject.transform.localEulerAngles + new Vector3(0f, Service.Get<Rand>().ViewRangeFloat(-45f, 45f), 0f);
			UnityEngine.Object.Destroy(gameObjectViewComponent.MainGameObject);
			gameObjectViewComponent.MainGameObject = gameObject;
		}

		protected internal HealthController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthController)GCHandledObjects.GCHandleToObject(instance)).ApplyHealthFragment((IHealthComponent)GCHandledObjects.GCHandleToObject(*args), (HealthFragment)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthController)GCHandledObjects.GCHandleToObject(instance)).ApplyHealthFragment((IHealthComponent)GCHandledObjects.GCHandleToObject(*args), (HealthFragment)GCHandledObjects.GCHandleToObject(args[1]), (List<int>)GCHandledObjects.GCHandleToObject(args[2]), *(int*)(args + 3), *(sbyte*)(args + 4) != 0, *(sbyte*)(args + 5) != 0, (SmartEntity)GCHandledObjects.GCHandleToObject(args[6])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HealthController)GCHandledObjects.GCHandleToObject(instance)).ApplyProcHealthChange((IHealthComponent)GCHandledObjects.GCHandleToObject(*args), (Buff)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HealthController)GCHandledObjects.GCHandleToObject(instance)).ApplyProcMaxHealthChange((HealthComponent)GCHandledObjects.GCHandleToObject(*args), (Buff)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthController)GCHandledObjects.GCHandleToObject(instance)).GetHealthMultiplier((IHealthComponent)GCHandledObjects.GCHandleToObject(*args), (List<int>)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((HealthController)GCHandledObjects.GCHandleToObject(instance)).HandleHealthChange((IHealthComponent)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((HealthController)GCHandledObjects.GCHandleToObject(instance)).KillEntity((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HealthController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((HealthController)GCHandledObjects.GCHandleToObject(instance)).OnFadeOutComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((HealthController)GCHandledObjects.GCHandleToObject(instance)).ProcBuffOnTarget((Buff)GCHandledObjects.GCHandleToObject(*args), (SmartEntity)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((HealthController)GCHandledObjects.GCHandleToObject(instance)).SwapModelsCallback(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
