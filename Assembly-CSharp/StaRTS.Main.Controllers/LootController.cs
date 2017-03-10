using Net.RichardLord.Ash.Core;
using StaRTS.FX;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class LootController : IEventObserver
	{
		private Dictionary<string, LootComponent> deadLootedEntities;

		private int[] totalLootAvailable;

		private int[] lootEarned;

		private int[] lastLootEarned;

		public void Initialize(int totalCredits, int totalMaterials, int totalContraband, Dictionary<string, int> buildingCreditsMap, Dictionary<string, int> buildingMaterialsMap, Dictionary<string, int> buildingContrabandMap)
		{
			this.deadLootedEntities = new Dictionary<string, LootComponent>();
			int num = 6;
			this.totalLootAvailable = new int[num];
			this.lootEarned = new int[num];
			this.lastLootEarned = new int[num];
			for (int i = 0; i < num; i++)
			{
				this.totalLootAvailable[i] = 0;
				this.lootEarned[i] = 0;
				this.lastLootEarned[i] = 0;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.EntityKilled, EventPriority.Default);
			this.CalculateTotalLoot(totalCredits, totalMaterials, totalContraband, buildingCreditsMap, buildingMaterialsMap, buildingContrabandMap);
			Service.Get<EventManager>().SendEvent(EventId.LootEarnedUpdated, null);
		}

		public void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.EntityKilled);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.EntityKilled)
			{
				return EatResponse.NotEaten;
			}
			Entity entity = cookie as Entity;
			if (entity == null)
			{
				return EatResponse.NotEaten;
			}
			LootComponent lootComponent = entity.Get<LootComponent>();
			BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
			if (lootComponent == null)
			{
				return EatResponse.NotEaten;
			}
			if (this.deadLootedEntities.ContainsKey(buildingComponent.BuildingTO.Key))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Entity {0} reported dead twice to LootController.", new object[]
				{
					buildingComponent.BuildingTO.Key
				});
				return EatResponse.NotEaten;
			}
			if (string.IsNullOrEmpty(buildingComponent.BuildingTO.Key))
			{
				Service.Get<StaRTSLogger>().Error("Recieved dead building with invalid BuildingTO Key!");
				return EatResponse.NotEaten;
			}
			this.deadLootedEntities[buildingComponent.BuildingTO.Key] = lootComponent;
			this.RefreshEarnedLoot();
			return EatResponse.NotEaten;
		}

		public void UpdateLootOnHealthChange(Entity entity, HealthComponent healthComp, int delta)
		{
			if (delta == 0)
			{
				return;
			}
			LootComponent lootComponent = entity.Get<LootComponent>();
			if (lootComponent == null || delta == 0)
			{
				return;
			}
			if (healthComp.IsDead())
			{
				return;
			}
			this.EarnLootFromDamage(lootComponent, healthComp, delta);
			Service.Get<EventManager>().SendEvent(EventId.LootEarnedUpdated, lootComponent);
		}

		private void ResetEarnedLoot()
		{
			int i = 0;
			int num = 6;
			while (i < num)
			{
				this.lootEarned[i] = 0;
				i++;
			}
		}

		private void EarnLootFromDamage(LootComponent lootComp, HealthComponent healthComp, int healthDelta)
		{
			int num = 0;
			int i = 0;
			int num2 = 6;
			while (i < num2)
			{
				int num3 = lootComp.LootQuantities[i];
				if (num3 != 0)
				{
					int num4 = GameUtils.CalculateDamagePercentage(healthComp.Health - healthDelta, healthComp.MaxHealth);
					int num5 = GameUtils.CalculateDamagePercentage(healthComp);
					int damagePercent = num5 - num4;
					int num6 = this.CalculateLootFromDamagePercentage(damagePercent, num3);
					this.lootEarned[i] += num6;
					Service.Get<CurrencyEffects>().PlayEffect(lootComp.Entity, (CurrencyType)i, num6);
					lootComp.IncParticleCount();
					num++;
				}
				i++;
			}
			Service.Get<EventManager>().SendEvent(EventId.LootEarnedUpdated, healthComp.Entity);
		}

		private void EarnLootFromDeath(LootComponent lootComp)
		{
			int i = 0;
			int num = 6;
			while (i < num)
			{
				this.lootEarned[i] += lootComp.LootQuantities[i];
				i++;
			}
		}

		private void EarnCalculatedLoot(LootComponent lootComp, int damagePercent)
		{
			if (damagePercent == 100)
			{
				this.EarnLootFromDeath(lootComp);
				return;
			}
			int i = 0;
			int num = 6;
			while (i < num)
			{
				int maxLoot = lootComp.LootQuantities[i];
				this.lootEarned[i] += this.CalculateLootFromDamagePercentage(damagePercent, maxLoot);
				i++;
			}
		}

		private int CalculateLootFromDamagePercentage(int damagePercent, int maxLoot)
		{
			return IntMath.Normalize(0, 100, damagePercent, 0, maxLoot);
		}

		public void RefreshEarnedLoot()
		{
			this.ResetEarnedLoot();
			Dictionary<string, int> buildingDamageMap = Service.Get<BattleController>().GetBuildingDamageMap();
			NodeList<LootNode> nodeList = Service.Get<EntityController>().GetNodeList<LootNode>();
			for (LootNode lootNode = nodeList.Head; lootNode != null; lootNode = lootNode.Next)
			{
				int damagePercent = 0;
				if (buildingDamageMap.ContainsKey(lootNode.BuildingComp.BuildingTO.Key))
				{
					damagePercent = buildingDamageMap[lootNode.BuildingComp.BuildingTO.Key];
				}
				this.EarnCalculatedLoot(lootNode.LootComp, damagePercent);
			}
			foreach (KeyValuePair<string, LootComponent> current in this.deadLootedEntities)
			{
				this.EarnLootFromDeath(current.get_Value());
			}
			Service.Get<EventManager>().SendEvent(EventId.LootEarnedUpdated, null);
		}

		private void CalculateTotalLoot(int totalCredits, int totalMaterials, int totalContraband, Dictionary<string, int> buildingCreditsMap, Dictionary<string, int> buildingMaterialsMap, Dictionary<string, int> buildingContrabandMap)
		{
			this.DistributLootWithHQWeighted(CurrencyType.Credits, buildingCreditsMap, totalCredits, GameConstants.HQ_LOOTABLE_CREDITS);
			this.DistributLootWithHQWeighted(CurrencyType.Materials, buildingMaterialsMap, totalMaterials, GameConstants.HQ_LOOTABLE_MATERIALS);
			this.DistributLootWithHQWeighted(CurrencyType.Contraband, buildingContrabandMap, totalContraband, GameConstants.HQ_LOOTABLE_CONTRABAND);
		}

		private void DistributLootWithHQWeighted(CurrencyType type, Dictionary<string, int> buildingOverride, int totalAmount, int hqFixedAmount)
		{
			NodeList<LootNode> nodeList = Service.Get<EntityController>().GetNodeList<LootNode>();
			List<LootNode> list = new List<LootNode>();
			int num = 0;
			int num2 = 0;
			for (LootNode lootNode = nodeList.Head; lootNode != null; lootNode = lootNode.Next)
			{
				if (lootNode.BuildingComp.BuildingType.Currency == type || lootNode.BuildingComp.BuildingType.Currency == CurrencyType.None)
				{
					if (buildingOverride != null && buildingOverride.ContainsKey(lootNode.BuildingComp.BuildingTO.Key))
					{
						int num3 = buildingOverride[lootNode.BuildingComp.BuildingTO.Key];
						lootNode.LootComp.SetLootQuantity(type, num3);
						this.totalLootAvailable[(int)type] += lootNode.LootComp.LootQuantities[(int)type];
						BuildingTypeVO buildingType = lootNode.BuildingComp.BuildingType;
						if (buildingType.Type == BuildingType.Resource || buildingType.Type == BuildingType.Storage)
						{
							Service.Get<StorageEffects>().UpdateFillStateFX(lootNode.Entity, buildingType, (float)num3 / (float)buildingType.Storage, 0f);
						}
					}
					else
					{
						list.Add(lootNode);
						num++;
						if (lootNode.BuildingComp.BuildingType.Type == BuildingType.HQ)
						{
							num2++;
						}
					}
				}
			}
			if (num2 * hqFixedAmount > totalAmount)
			{
				num2 = totalAmount / hqFixedAmount;
			}
			int num4 = num - num2;
			int num5 = 0;
			int num6 = 0;
			if (num4 > 0)
			{
				num5 = (totalAmount - hqFixedAmount * num2) % num4;
				num6 = (totalAmount - hqFixedAmount * num2) / num4;
			}
			int num7 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				LootNode lootNode2 = list[i];
				if (lootNode2.BuildingComp.BuildingType.Type == BuildingType.HQ && num7 < num2)
				{
					lootNode2.LootComp.SetLootQuantity(type, hqFixedAmount + num5);
					num5 = 0;
					num7++;
				}
				else
				{
					int num8 = num6;
					if (num2 == 0)
					{
						num8 += num5;
						num5 = 0;
					}
					lootNode2.LootComp.SetLootQuantity(type, num8);
				}
				this.totalLootAvailable[(int)type] += lootNode2.LootComp.LootQuantities[(int)type];
			}
		}

		public int GetLootEarned(CurrencyType lootType)
		{
			return this.lootEarned[(int)lootType];
		}

		public int GetTotalLootAvailable(CurrencyType lootType)
		{
			return this.totalLootAvailable[(int)lootType];
		}

		public float GetRemainingLoot(CurrencyType lootType)
		{
			return (float)(this.GetTotalLootAvailable(lootType) - this.GetLootEarned(lootType));
		}

		public int GetLootDelta(CurrencyType lootType)
		{
			return this.GetLootEarned(lootType) - this.lastLootEarned[(int)lootType];
		}

		public void ResetLastLootEarned(CurrencyType lootType)
		{
			this.lastLootEarned[(int)lootType] = this.GetLootEarned(lootType);
		}

		public LootController()
		{
		}

		protected internal LootController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootController)GCHandledObjects.GCHandleToObject(instance)).CalculateLootFromDamagePercentage(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).CalculateTotalLoot(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[3]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[4]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[5]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).DistributLootWithHQWeighted((CurrencyType)(*(int*)args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), *(int*)(args + 3));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).EarnCalculatedLoot((LootComponent)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).EarnLootFromDamage((LootComponent)GCHandledObjects.GCHandleToObject(*args), (HealthComponent)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).EarnLootFromDeath((LootComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootController)GCHandledObjects.GCHandleToObject(instance)).GetLootDelta((CurrencyType)(*(int*)args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootController)GCHandledObjects.GCHandleToObject(instance)).GetLootEarned((CurrencyType)(*(int*)args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootController)GCHandledObjects.GCHandleToObject(instance)).GetRemainingLoot((CurrencyType)(*(int*)args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootController)GCHandledObjects.GCHandleToObject(instance)).GetTotalLootAvailable((CurrencyType)(*(int*)args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).Initialize(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[3]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[4]), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[5]));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).RefreshEarnedLoot();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).ResetEarnedLoot();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).ResetLastLootEarned((CurrencyType)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((LootController)GCHandledObjects.GCHandleToObject(instance)).UpdateLootOnHealthChange((Entity)GCHandledObjects.GCHandleToObject(*args), (HealthComponent)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2));
			return -1L;
		}
	}
}
