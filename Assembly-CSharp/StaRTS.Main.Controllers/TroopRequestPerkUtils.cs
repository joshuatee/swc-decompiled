using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public static class TroopRequestPerkUtils
	{
		public static int GetTroopRequestPerkTimeReduction(List<string> perkEffectIds)
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
					string type = perkEffectVO.Type;
					BuildingType perkBuilding = perkEffectVO.PerkBuilding;
					if (type == "troopRequestTime" && perkBuilding == BuildingType.Squad)
					{
						num += perkEffectVO.TroopRequestTimeDiscount;
					}
					i++;
				}
			}
			return num;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopRequestPerkUtils.GetTroopRequestPerkTimeReduction((List<string>)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
