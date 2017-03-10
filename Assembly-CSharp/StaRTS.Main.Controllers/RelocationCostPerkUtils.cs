using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public static class RelocationCostPerkUtils
	{
		public static int GetRelocationCostDiscount(List<string> perkEffectIds)
		{
			int num = 0;
			if (perkEffectIds != null)
			{
				IDataController dataController = Service.Get<IDataController>();
				int i = 0;
				int count = perkEffectIds.Count;
				while (i < count)
				{
					PerkEffectVO perkEffectVO = dataController.Get<PerkEffectVO>(perkEffectIds[i]);
					if (RelocationCostPerkUtils.CanApplyEffect(perkEffectVO))
					{
						num += perkEffectVO.RelocationDiscount;
					}
					i++;
				}
			}
			return num;
		}

		private static bool CanApplyEffect(PerkEffectVO perkEffectVO)
		{
			string type = perkEffectVO.Type;
			BuildingType perkBuilding = perkEffectVO.PerkBuilding;
			return type == "relocation" && perkBuilding == BuildingType.NavigationCenter;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RelocationCostPerkUtils.CanApplyEffect((PerkEffectVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(RelocationCostPerkUtils.GetRelocationCostDiscount((List<string>)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
