using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers
{
	public class EquipmentTabHelper : AbstractTabHelper
	{
		private const string GRID_FILTER = "FilterGrid";

		private const string TEMPLATE_FILTER = "FilterTemplate";

		private const string PANEL_FILTER = "FilterPanel";

		private const string BUTTON_FILTER = "BtnFilter";

		private const string LABEL_FILTER_BUTTON = "LabelBtnFilter";

		public EquipmentTabHelper() : base("FilterPanel", "FilterGrid", "FilterTemplate", "BtnFilter", "LabelBtnFilter")
		{
		}

		public bool IsEquipmentValidForTab(EquipmentVO vo, EquipmentTab selectedTab)
		{
			if (selectedTab == EquipmentTab.All)
			{
				return true;
			}
			TroopUpgradeCatalog troopUpgradeCatalog = Service.Get<TroopUpgradeCatalog>();
			IDataController dataController = Service.Get<IDataController>();
			if (vo.EffectUids == null)
			{
				return false;
			}
			int i = 0;
			int num = vo.EffectUids.Length;
			while (i < num)
			{
				EquipmentEffectVO optional = dataController.GetOptional<EquipmentEffectVO>(vo.EffectUids[i]);
				if (optional == null)
				{
					Service.Get<StaRTSLogger>().Error("CMS Error: EffectUids is empty for EquipmentEffectVO " + optional.Uid);
					return false;
				}
				switch (selectedTab)
				{
				case EquipmentTab.Troops:
					if (optional.AffectedTroopIds != null && optional.AffectedTroopIds.Length != 0)
					{
						int j = 0;
						int num2 = optional.AffectedTroopIds.Length;
						while (j < num2)
						{
							if (troopUpgradeCatalog.GetMinLevel(optional.AffectedTroopIds[j]).Type != TroopType.Hero)
							{
								return true;
							}
							j++;
						}
					}
					break;
				case EquipmentTab.Heroes:
					if (optional.AffectedTroopIds != null && optional.AffectedTroopIds.Length != 0)
					{
						int k = 0;
						int num3 = optional.AffectedTroopIds.Length;
						while (k < num3)
						{
							if (troopUpgradeCatalog.GetMinLevel(optional.AffectedTroopIds[k]).Type == TroopType.Hero)
							{
								return true;
							}
							k++;
						}
					}
					break;
				case EquipmentTab.Structures:
					if (optional.AffectedBuildingIds != null && optional.AffectedBuildingIds.Length != 0)
					{
						return true;
					}
					break;
				}
				i++;
			}
			return false;
		}

		protected internal EquipmentTabHelper(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EquipmentTabHelper)GCHandledObjects.GCHandleToObject(instance)).IsEquipmentValidForTab((EquipmentVO)GCHandledObjects.GCHandleToObject(*args), (EquipmentTab)(*(int*)(args + 1))));
		}
	}
}
