using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.IAP
{
	public class WindowsIAPManager : IInAppPurchaseManager
	{
		private UnityIAPManager uIAPMAnager;

		public void Init()
		{
			this.uIAPMAnager = new UnityIAPManager();
		}

		public void GetProducts()
		{
			string text = "ws";
			List<string> list = new List<string>();
			Dictionary<string, InAppPurchaseTypeVO>.ValueCollection allIAPTypes = Service.Get<InAppPurchaseController>().GetAllIAPTypes();
			if (allIAPTypes != null)
			{
				foreach (InAppPurchaseTypeVO current in allIAPTypes)
				{
					if (current.Type == text)
					{
						list.Add(current.ProductId);
					}
				}
			}
			this.uIAPMAnager.InitializePurchasing(list);
		}

		public void Purchase(string productID)
		{
			this.uIAPMAnager.MakePurchase(productID);
		}

		public void Consume(string productID)
		{
		}

		public void RestorePurchases()
		{
		}

		public void OnApplicationResume()
		{
		}

		public WindowsIAPManager()
		{
		}

		protected internal WindowsIAPManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((WindowsIAPManager)GCHandledObjects.GCHandleToObject(instance)).Consume(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WindowsIAPManager)GCHandledObjects.GCHandleToObject(instance)).GetProducts();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WindowsIAPManager)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WindowsIAPManager)GCHandledObjects.GCHandleToObject(instance)).OnApplicationResume();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((WindowsIAPManager)GCHandledObjects.GCHandleToObject(instance)).Purchase(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((WindowsIAPManager)GCHandledObjects.GCHandleToObject(instance)).RestorePurchases();
			return -1L;
		}
	}
}
