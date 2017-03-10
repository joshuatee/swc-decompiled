using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class CrateTierVO : IValueObject, IGeometryVO
	{
		private List<string[]> allHq;

		public static int COLUMN_assetName
		{
			get;
			private set;
		}

		public static int COLUMN_hq4
		{
			get;
			private set;
		}

		public static int COLUMN_hq5
		{
			get;
			private set;
		}

		public static int COLUMN_hq6
		{
			get;
			private set;
		}

		public static int COLUMN_hq7
		{
			get;
			private set;
		}

		public static int COLUMN_hq8
		{
			get;
			private set;
		}

		public static int COLUMN_hq9
		{
			get;
			private set;
		}

		public static int COLUMN_hq10
		{
			get;
			private set;
		}

		public static int COLUMN_purchasable
		{
			get;
			private set;
		}

		public static int COLUMN_crystals
		{
			get;
			private set;
		}

		public static int COLUMN_bundleName
		{
			get;
			private set;
		}

		public static int COLUMN_iconCameraPosition
		{
			get;
			private set;
		}

		public static int COLUMN_iconLookatPosition
		{
			get;
			private set;
		}

		public static int COLUMN_iconCloseupCameraPosition
		{
			get;
			private set;
		}

		public static int COLUMN_iconCloseupLookatPosition
		{
			get;
			private set;
		}

		public static int COLUMN_iconAssetName
		{
			get;
			private set;
		}

		public static int COLUMN_iconBundleName
		{
			get;
			private set;
		}

		public static int COLUMN_additionalCrateTier
		{
			get;
			private set;
		}

		public static int COLUMN_additionalCrateCondition
		{
			get;
			private set;
		}

		public static int COLUMN_storeVisibilityConditions
		{
			get;
			private set;
		}

		public static int COLUMN_storePurchasableConditions
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string[] hq4
		{
			get;
			set;
		}

		public string[] hq5
		{
			get;
			set;
		}

		public string[] hq6
		{
			get;
			set;
		}

		public string[] hq7
		{
			get;
			set;
		}

		public string[] hq8
		{
			get;
			set;
		}

		public string[] hq9
		{
			get;
			set;
		}

		public string[] hq10
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			set;
		}

		public string BundleName
		{
			get;
			set;
		}

		public Vector3 IconCameraPosition
		{
			get;
			set;
		}

		public Vector3 IconLookatPosition
		{
			get;
			set;
		}

		public Vector3 IconCloseupCameraPosition
		{
			get;
			set;
		}

		public Vector3 IconCloseupLookatPosition
		{
			get;
			set;
		}

		public string IconAssetName
		{
			get;
			set;
		}

		public string IconBundleName
		{
			get;
			set;
		}

		public bool Purchasable
		{
			get;
			set;
		}

		public int Crystals
		{
			get;
			set;
		}

		public string AdditionalCrateTier
		{
			get;
			set;
		}

		public string AdditionalCrateCondition
		{
			get;
			set;
		}

		public string[] StoreVisibilityConditions
		{
			get;
			set;
		}

		public string[] StorePurchasableConditions
		{
			get;
			set;
		}

		public float IconRotationSpeed
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.allHq = new List<string[]>();
			this.hq4 = row.TryGetString(CrateTierVO.COLUMN_hq4, string.Empty).Split(new char[]
			{
				' '
			});
			this.allHq.Add(this.hq4);
			this.hq5 = row.TryGetString(CrateTierVO.COLUMN_hq5, string.Empty).Split(new char[]
			{
				' '
			});
			this.allHq.Add(this.hq5);
			this.hq6 = row.TryGetString(CrateTierVO.COLUMN_hq6, string.Empty).Split(new char[]
			{
				' '
			});
			this.allHq.Add(this.hq6);
			this.hq7 = row.TryGetString(CrateTierVO.COLUMN_hq7, string.Empty).Split(new char[]
			{
				' '
			});
			this.allHq.Add(this.hq7);
			this.hq8 = row.TryGetString(CrateTierVO.COLUMN_hq8, string.Empty).Split(new char[]
			{
				' '
			});
			this.allHq.Add(this.hq8);
			this.hq9 = row.TryGetString(CrateTierVO.COLUMN_hq9, string.Empty).Split(new char[]
			{
				' '
			});
			this.allHq.Add(this.hq9);
			this.hq10 = row.TryGetString(CrateTierVO.COLUMN_hq10, string.Empty).Split(new char[]
			{
				' '
			});
			this.allHq.Add(this.hq10);
			this.AssetName = row.TryGetString(CrateTierVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(CrateTierVO.COLUMN_bundleName);
			this.IconAssetName = row.TryGetString(CrateTierVO.COLUMN_iconAssetName, this.AssetName);
			this.IconBundleName = row.TryGetString(CrateTierVO.COLUMN_iconBundleName, this.BundleName);
			this.IconCameraPosition = row.TryGetVector3(CrateTierVO.COLUMN_iconCameraPosition);
			this.IconLookatPosition = row.TryGetVector3(CrateTierVO.COLUMN_iconLookatPosition);
			this.IconCloseupCameraPosition = row.TryGetVector3(CrateTierVO.COLUMN_iconCloseupCameraPosition, this.IconCameraPosition);
			this.IconCloseupLookatPosition = row.TryGetVector3(CrateTierVO.COLUMN_iconCloseupLookatPosition, this.IconLookatPosition);
			this.Purchasable = row.TryGetBool(CrateTierVO.COLUMN_purchasable);
			this.Crystals = row.TryGetInt(CrateTierVO.COLUMN_crystals, 0);
			this.AdditionalCrateTier = row.TryGetString(CrateTierVO.COLUMN_additionalCrateTier);
			this.AdditionalCrateCondition = row.TryGetString(CrateTierVO.COLUMN_additionalCrateCondition);
			this.StoreVisibilityConditions = row.TryGetStringArray(CrateTierVO.COLUMN_storeVisibilityConditions);
			this.StorePurchasableConditions = row.TryGetStringArray(CrateTierVO.COLUMN_storePurchasableConditions);
		}

		public string SupplyTableId(int hq, FactionType faction, int itemNumber)
		{
			int num = itemNumber - 1;
			string[] array = this.allHq[hq - 4];
			if (array == null || array.Length == 0)
			{
				return null;
			}
			if (faction == FactionType.Rebel)
			{
				return array[num * 2];
			}
			return array[num * 2 + 1];
		}

		public CrateTierVO()
		{
		}

		protected internal CrateTierVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).AdditionalCrateCondition);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).AdditionalCrateTier);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_additionalCrateCondition);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_additionalCrateTier);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_crystals);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_hq10);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_hq4);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_hq5);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_hq6);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_hq7);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_hq8);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_hq9);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_iconAssetName);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_iconBundleName);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_iconCameraPosition);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_iconCloseupCameraPosition);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_iconCloseupLookatPosition);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_iconLookatPosition);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_purchasable);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_storePurchasableConditions);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CrateTierVO.COLUMN_storeVisibilityConditions);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).Crystals);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq10);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq4);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq5);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq6);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq7);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq8);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq9);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).Purchasable);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).StorePurchasableConditions);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).StoreVisibilityConditions);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).AdditionalCrateCondition = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).AdditionalCrateTier = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			CrateTierVO.COLUMN_additionalCrateCondition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			CrateTierVO.COLUMN_additionalCrateTier = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			CrateTierVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			CrateTierVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			CrateTierVO.COLUMN_crystals = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			CrateTierVO.COLUMN_hq10 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			CrateTierVO.COLUMN_hq4 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			CrateTierVO.COLUMN_hq5 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			CrateTierVO.COLUMN_hq6 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			CrateTierVO.COLUMN_hq7 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			CrateTierVO.COLUMN_hq8 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			CrateTierVO.COLUMN_hq9 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			CrateTierVO.COLUMN_iconAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			CrateTierVO.COLUMN_iconBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			CrateTierVO.COLUMN_iconCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			CrateTierVO.COLUMN_iconCloseupCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			CrateTierVO.COLUMN_iconCloseupLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			CrateTierVO.COLUMN_iconLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			CrateTierVO.COLUMN_purchasable = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			CrateTierVO.COLUMN_storePurchasableConditions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			CrateTierVO.COLUMN_storeVisibilityConditions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).Crystals = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq10 = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq4 = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq5 = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq6 = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq7 = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq8 = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).hq9 = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).Purchasable = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).StorePurchasableConditions = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).StoreVisibilityConditions = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateTierVO)GCHandledObjects.GCHandleToObject(instance)).SupplyTableId(*(int*)args, (FactionType)(*(int*)(args + 1)), *(int*)(args + 2)));
		}
	}
}
