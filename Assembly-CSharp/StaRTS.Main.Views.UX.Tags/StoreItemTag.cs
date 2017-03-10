using StaRTS.Externals.IAP;
using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX.Elements;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Tags
{
	public class StoreItemTag
	{
		public BuildingTypeVO BuildingInfo
		{
			get;
			set;
		}

		public UXElement MainElement
		{
			get;
			set;
		}

		public UXButton MainButton
		{
			get;
			set;
		}

		public UXLabel InfoLabel
		{
			get;
			set;
		}

		public UXButton InfoGroup
		{
			get;
			set;
		}

		public int CurQuantity
		{
			get;
			set;
		}

		public int MaxQuantity
		{
			get;
			set;
		}

		public bool CanPurchase
		{
			get;
			set;
		}

		public BuildingTypeVO ReqBuilding
		{
			get;
			set;
		}

		public bool ReqMet
		{
			get;
			set;
		}

		public int Amount
		{
			get;
			set;
		}

		public int Price
		{
			get;
			set;
		}

		public CurrencyType Currency
		{
			get;
			set;
		}

		public string IconName
		{
			get;
			set;
		}

		public string Uid
		{
			get;
			set;
		}

		public InAppPurchaseTypeVO IAPType
		{
			get;
			set;
		}

		public InAppPurchaseProductInfo IAPProduct
		{
			get;
			set;
		}

		public StoreItemTag()
		{
		}

		protected internal StoreItemTag(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).Amount);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).BuildingInfo);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).CanPurchase);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).CurQuantity);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).Currency);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).IAPProduct);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).IAPType);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).IconName);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).InfoGroup);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).InfoLabel);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).MainButton);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).MainElement);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).MaxQuantity);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).Price);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).ReqBuilding);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).ReqMet);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).Amount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).BuildingInfo = (BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).CanPurchase = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).CurQuantity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).Currency = (CurrencyType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).IAPProduct = (InAppPurchaseProductInfo)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).IAPType = (InAppPurchaseTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).IconName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).InfoGroup = (UXButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).InfoLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).MainButton = (UXButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).MainElement = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).MaxQuantity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).Price = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).ReqBuilding = (BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).ReqMet = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((StoreItemTag)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
