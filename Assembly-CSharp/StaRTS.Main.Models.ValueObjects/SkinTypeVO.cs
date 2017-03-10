using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class SkinTypeVO : IValueObject, IAssetVO, IAudioVO, IGeometryVO, ISpeedVO
	{
		public static int COLUMN_unitID
		{
			get;
			private set;
		}

		public static int COLUMN_override
		{
			get;
			private set;
		}

		public static int COLUMN_assetName
		{
			get;
			private set;
		}

		public static int COLUMN_bundleName
		{
			get;
			private set;
		}

		public static int COLUMN_audioAttack
		{
			get;
			private set;
		}

		public static int COLUMN_audioDeath
		{
			get;
			private set;
		}

		public static int COLUMN_audioPlacement
		{
			get;
			private set;
		}

		public static int COLUMN_audioImpact
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

		public static int COLUMN_iconRotationSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_maxSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_rotationSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_hologramUid
		{
			get;
			private set;
		}

		public static int COLUMN_textureUid
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string UnitId
		{
			get;
			set;
		}

		public SkinOverrideTypeVO Override
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

		public List<StrIntPair> AudioCharge
		{
			get;
			set;
		}

		public List<StrIntPair> AudioAttack
		{
			get;
			set;
		}

		public List<StrIntPair> AudioDeath
		{
			get;
			set;
		}

		public List<StrIntPair> AudioPlacement
		{
			get;
			set;
		}

		public List<StrIntPair> AudioMovement
		{
			get;
			set;
		}

		public List<StrIntPair> AudioMovementAway
		{
			get;
			set;
		}

		public List<StrIntPair> AudioImpact
		{
			get;
			set;
		}

		public List<StrIntPair> AudioTrain
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

		public string IconBundleName
		{
			get;
			set;
		}

		public string IconAssetName
		{
			get;
			set;
		}

		public float IconRotationSpeed
		{
			get;
			set;
		}

		public int MaxSpeed
		{
			get;
			set;
		}

		public int RotationSpeed
		{
			get;
			set;
		}

		public MobilizationHologramVO MobilizationHologram
		{
			get;
			set;
		}

		public TextureVO CardTexture
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.UnitId = row.TryGetString(SkinTypeVO.COLUMN_unitID);
			string text = row.TryGetString(SkinTypeVO.COLUMN_override);
			if (!string.IsNullOrEmpty(text))
			{
				IDataController dataController = Service.Get<IDataController>();
				this.Override = dataController.Get<SkinOverrideTypeVO>(text);
			}
			this.AssetName = row.TryGetString(SkinTypeVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(SkinTypeVO.COLUMN_bundleName);
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			this.AudioAttack = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(SkinTypeVO.COLUMN_audioAttack));
			this.AudioDeath = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(SkinTypeVO.COLUMN_audioDeath));
			this.AudioPlacement = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(SkinTypeVO.COLUMN_audioPlacement));
			this.AudioImpact = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(SkinTypeVO.COLUMN_audioImpact));
			this.IconCameraPosition = row.TryGetVector3(SkinTypeVO.COLUMN_iconCameraPosition);
			this.IconLookatPosition = row.TryGetVector3(SkinTypeVO.COLUMN_iconLookatPosition);
			this.IconCloseupCameraPosition = row.TryGetVector3(SkinTypeVO.COLUMN_iconCloseupCameraPosition);
			this.IconCloseupLookatPosition = row.TryGetVector3(SkinTypeVO.COLUMN_iconCloseupLookatPosition);
			this.IconAssetName = row.TryGetString(SkinTypeVO.COLUMN_iconAssetName, this.AssetName);
			this.IconBundleName = row.TryGetString(SkinTypeVO.COLUMN_iconBundleName, this.BundleName);
			this.IconRotationSpeed = row.TryGetFloat(SkinTypeVO.COLUMN_iconRotationSpeed);
			this.MaxSpeed = row.TryGetInt(SkinTypeVO.COLUMN_maxSpeed);
			this.RotationSpeed = row.TryGetInt(SkinTypeVO.COLUMN_rotationSpeed);
			string text2 = row.TryGetString(SkinTypeVO.COLUMN_hologramUid);
			if (!string.IsNullOrEmpty(text2))
			{
				IDataController dataController2 = Service.Get<IDataController>();
				this.MobilizationHologram = dataController2.Get<MobilizationHologramVO>(text2);
			}
			string text3 = row.TryGetString(SkinTypeVO.COLUMN_textureUid);
			if (!string.IsNullOrEmpty(text3))
			{
				IDataController dataController3 = Service.Get<IDataController>();
				this.CardTexture = dataController3.Get<TextureVO>(text3);
			}
		}

		public SkinTypeVO()
		{
		}

		protected internal SkinTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).CardTexture);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_audioAttack);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_audioDeath);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_audioImpact);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_audioPlacement);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_hologramUid);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_iconAssetName);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_iconBundleName);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_iconCameraPosition);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_iconCloseupCameraPosition);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_iconCloseupLookatPosition);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_iconLookatPosition);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_iconRotationSpeed);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_maxSpeed);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_override);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_rotationSpeed);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_textureUid);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinTypeVO.COLUMN_unitID);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).MobilizationHologram);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).Override);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).UnitId);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).CardTexture = (TextureVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			SkinTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			SkinTypeVO.COLUMN_audioAttack = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			SkinTypeVO.COLUMN_audioDeath = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			SkinTypeVO.COLUMN_audioImpact = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			SkinTypeVO.COLUMN_audioPlacement = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			SkinTypeVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			SkinTypeVO.COLUMN_hologramUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			SkinTypeVO.COLUMN_iconAssetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			SkinTypeVO.COLUMN_iconBundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			SkinTypeVO.COLUMN_iconCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			SkinTypeVO.COLUMN_iconCloseupCameraPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			SkinTypeVO.COLUMN_iconCloseupLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			SkinTypeVO.COLUMN_iconLookatPosition = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			SkinTypeVO.COLUMN_iconRotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			SkinTypeVO.COLUMN_maxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			SkinTypeVO.COLUMN_override = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			SkinTypeVO.COLUMN_rotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			SkinTypeVO.COLUMN_textureUid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			SkinTypeVO.COLUMN_unitID = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconBundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupCameraPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconCloseupLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconLookatPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).IconRotationSpeed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).MobilizationHologram = (MobilizationHologramVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).Override = (SkinOverrideTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((SkinTypeVO)GCHandledObjects.GCHandleToObject(instance)).UnitId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
