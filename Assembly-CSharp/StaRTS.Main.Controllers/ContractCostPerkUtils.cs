using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public static class ContractCostPerkUtils
	{
		public static float GetDiscountedCostMultiplier(BuildingTypeVO contractBuildingVO, List<string> perkEffectIds)
		{
			float num = 0f;
			if (perkEffectIds != null)
			{
				IDataController dataController = Service.Get<IDataController>();
				int i = 0;
				int count = perkEffectIds.Count;
				while (i < count)
				{
					PerkEffectVO perkEffectVO = dataController.Get<PerkEffectVO>(perkEffectIds[i]);
					if (ContractCostPerkUtils.CanApplyEffect(perkEffectVO, contractBuildingVO))
					{
						num += perkEffectVO.ContractDiscount;
					}
					i++;
				}
			}
			return 1f - num;
		}

		public static bool CanApplyEffect(PerkEffectVO perkEffectVO, BuildingTypeVO contractBuildingVO)
		{
			string type = perkEffectVO.Type;
			BuildingType perkBuilding = perkEffectVO.PerkBuilding;
			return contractBuildingVO != null && (type == "contractCost" && perkBuilding == contractBuildingVO.Type);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractCostPerkUtils.CanApplyEffect((PerkEffectVO)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ContractCostPerkUtils.GetDiscountedCostMultiplier((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args), (List<string>)GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
