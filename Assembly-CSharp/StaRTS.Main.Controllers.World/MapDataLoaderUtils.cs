using StaRTS.FX;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Battle.Replay;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers.World
{
	public class MapDataLoaderUtils
	{
		public static List<IAssetVO> GetBattlePreloads(BattleInitializationData battleData)
		{
			List<IAssetVO> list = new List<IAssetVO>();
			IDataController dc = Service.Get<IDataController>();
			SkinController skinController = Service.Get<SkinController>();
			BattleTypeVO battleTypeVO = (battleData == null) ? null : battleData.BattleVO;
			if (battleTypeVO == null || !battleTypeVO.OverridePlayerUnits)
			{
				Inventory inventory = Service.Get<CurrentPlayer>().Inventory;
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(inventory.Troop, list, battleData.AttackerEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<SpecialAttackTypeVO>(inventory.SpecialAttack, list, battleData.AttackerEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(inventory.Hero, list, battleData.AttackerEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(inventory.Champion, list, battleData.AttackerEquipment, dc, skinController);
			}
			if (battleTypeVO != null)
			{
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(battleTypeVO.TroopData, list, battleData.AttackerEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<SpecialAttackTypeVO>(battleTypeVO.SpecialAttackData, list, battleData.AttackerEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(battleTypeVO.HeroData, list, battleData.AttackerEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(battleTypeVO.ChampionData, list, battleData.AttackerEquipment, dc, skinController);
			}
			MapDataLoaderUtils.AddFXPreloads(list);
			return list;
		}

		public static List<IAssetVO> GetBattleRecordPreloads(BattleRecord battleRecord)
		{
			List<IAssetVO> list = new List<IAssetVO>();
			if (battleRecord == null)
			{
				Service.Get<Logger>().Error("Battle Record is null in MapDataLoaderUtils.GetBattleRecordPreloads.");
				return list;
			}
			IDataController dc = Service.Get<IDataController>();
			SkinController skinController = Service.Get<SkinController>();
			BattleDeploymentData attackerDeploymentData = battleRecord.AttackerDeploymentData;
			if (attackerDeploymentData != null)
			{
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(attackerDeploymentData.TroopData, list, battleRecord.AttackerEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<SpecialAttackTypeVO>(attackerDeploymentData.SpecialAttackData, list, battleRecord.AttackerEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(attackerDeploymentData.HeroData, list, battleRecord.AttackerEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(attackerDeploymentData.ChampionData, list, battleRecord.AttackerEquipment, dc, skinController);
			}
			BattleDeploymentData defenderDeploymentData = battleRecord.DefenderDeploymentData;
			if (defenderDeploymentData != null)
			{
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(defenderDeploymentData.TroopData, list, battleRecord.DefenderEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<SpecialAttackTypeVO>(defenderDeploymentData.SpecialAttackData, list, battleRecord.DefenderEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(defenderDeploymentData.HeroData, list, battleRecord.DefenderEquipment, dc, skinController);
				MapDataLoaderUtils.AddDeployablesToList<TroopTypeVO>(defenderDeploymentData.ChampionData, list, battleRecord.DefenderEquipment, dc, skinController);
			}
			MapDataLoaderUtils.AddFXPreloads(list);
			return list;
		}

		private static void AddFXPreloads(List<IAssetVO> assets)
		{
			List<IAssetVO> effectAssetTypes = Service.Get<CurrencyEffects>().GetEffectAssetTypes("setupTypeLooting");
			assets.AddRange(effectAssetTypes);
			assets.AddRange(Service.Get<ShieldEffects>().GetEffectAssetTypes());
			assets.AddRange(FXUtils.GetEffectAssetTypes());
			Service.Get<UXController>().MiscElementsManager.EnsureHealthSliderPool();
		}

		private static void AddDeployablesToList<T>(InventoryStorage storage, List<IAssetVO> assets, List<string> equipment, IDataController dc, SkinController skinController) where T : IValueObject
		{
			Dictionary<string, InventoryEntry> internalStorage = storage.GetInternalStorage();
			foreach (KeyValuePair<string, InventoryEntry> current in internalStorage)
			{
				MapDataLoaderUtils.AddDeployableToList<T>(current.Key, current.Value.Amount, assets, equipment, dc, skinController);
			}
		}

		private static void AddDeployablesToList<T>(Dictionary<string, int> deployables, List<IAssetVO> assets, List<string> equipment, IDataController dc, SkinController skinController) where T : IValueObject
		{
			if (deployables != null)
			{
				foreach (KeyValuePair<string, int> current in deployables)
				{
					MapDataLoaderUtils.AddDeployableToList<T>(current.Key, current.Value, assets, equipment, dc, skinController);
				}
			}
		}

		private static void AddDeployableToList<T>(string uid, int amount, List<IAssetVO> assets, List<string> equipment, IDataController dc, SkinController skinController) where T : IValueObject
		{
			if (amount > 0)
			{
				IAssetVO assetVO = dc.GetOptional<T>(uid) as IAssetVO;
				if (assetVO != null)
				{
					if (assetVO is TroopTypeVO)
					{
						MapDataLoaderUtils.AddSpawnEffect((TroopTypeVO)assetVO, assets, dc);
						SkinTypeVO applicableSkin = skinController.GetApplicableSkin((TroopTypeVO)assetVO, equipment);
						if (applicableSkin != null)
						{
							assetVO = applicableSkin;
						}
					}
					for (int i = 0; i < amount; i++)
					{
						assets.Add(assetVO);
					}
				}
			}
		}

		private static void AddSpawnEffect(TroopTypeVO troopType, List<IAssetVO> assets, IDataController dc)
		{
			if (troopType != null)
			{
				string spawnEffectUid = troopType.SpawnEffectUid;
				if (!string.IsNullOrEmpty(spawnEffectUid))
				{
					EffectsTypeVO item = dc.Get<EffectsTypeVO>(spawnEffectUid);
					assets.Add(item);
				}
			}
		}
	}
}
