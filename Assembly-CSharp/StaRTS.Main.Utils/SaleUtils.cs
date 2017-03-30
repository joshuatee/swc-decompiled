using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Utils
{
	public static class SaleUtils
	{
		public static SaleTypeVO GetCurrentActiveSale()
		{
			IDataController dataController = Service.Get<IDataController>();
			foreach (SaleTypeVO current in dataController.GetAll<SaleTypeVO>())
			{
				if (TimedEventUtils.IsTimedEventActive(current))
				{
					return current;
				}
			}
			return null;
		}

		public static List<SaleItemTypeVO> GetSaleItems(string[] saleItemUids)
		{
			List<SaleItemTypeVO> list = new List<SaleItemTypeVO>();
			IDataController dataController = Service.Get<IDataController>();
			foreach (SaleItemTypeVO current in dataController.GetAll<SaleItemTypeVO>())
			{
				if (Array.IndexOf<string>(saleItemUids, current.Uid) >= 0)
				{
					list.Add(current);
				}
			}
			return list;
		}
	}
}
