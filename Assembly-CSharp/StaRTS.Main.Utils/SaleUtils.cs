using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

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

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SaleUtils.GetCurrentActiveSale());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SaleUtils.GetSaleItems((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args)));
		}
	}
}
