using StaRTS.Main.Controllers;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class CivilianTypeVO : IValueObject, ISpeedVO, IAssetVO, IHealthVO, IAudioVO
	{
		public int Acceleration;

		public int Credits;

		public int Materials;

		public int Contraband;

		public int Xp;

		public int SizeX;

		public int SizeY;

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

		public static int COLUMN_faction
		{
			get;
			private set;
		}

		public static int COLUMN_credits
		{
			get;
			private set;
		}

		public static int COLUMN_materials
		{
			get;
			private set;
		}

		public static int COLUMN_contraband
		{
			get;
			private set;
		}

		public static int COLUMN_health
		{
			get;
			private set;
		}

		public static int COLUMN_maxSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_newRotationSpeed
		{
			get;
			private set;
		}

		public static int COLUMN_size
		{
			get;
			private set;
		}

		public static int COLUMN_xp
		{
			get;
			private set;
		}

		public static int COLUMN_sizex
		{
			get;
			private set;
		}

		public static int COLUMN_sizey
		{
			get;
			private set;
		}

		public static int COLUMN_audioCharge
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

		public string Uid
		{
			get;
			set;
		}

		public FactionType Faction
		{
			get;
			set;
		}

		public int Health
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

		public int TrainingTime
		{
			get;
			set;
		}

		public int Size
		{
			get;
			set;
		}

		public List<StrIntPair> AudioPlacement
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

		public void ReadRow(Row row)
		{
			this.AssetName = row.TryGetString(CivilianTypeVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(CivilianTypeVO.COLUMN_bundleName);
			this.Uid = row.Uid;
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(CivilianTypeVO.COLUMN_faction));
			this.Credits = row.TryGetInt(CivilianTypeVO.COLUMN_credits);
			this.Materials = row.TryGetInt(CivilianTypeVO.COLUMN_materials);
			this.Contraband = row.TryGetInt(CivilianTypeVO.COLUMN_contraband);
			this.Health = row.TryGetInt(CivilianTypeVO.COLUMN_health);
			this.MaxSpeed = row.TryGetInt(CivilianTypeVO.COLUMN_maxSpeed);
			this.RotationSpeed = row.TryGetInt(CivilianTypeVO.COLUMN_newRotationSpeed);
			this.Size = row.TryGetInt(CivilianTypeVO.COLUMN_size);
			this.Xp = row.TryGetInt(CivilianTypeVO.COLUMN_xp);
			this.SizeX = row.TryGetInt(CivilianTypeVO.COLUMN_sizex);
			this.SizeY = row.TryGetInt(CivilianTypeVO.COLUMN_sizey);
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			this.AudioCharge = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(CivilianTypeVO.COLUMN_audioCharge));
			this.AudioAttack = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(CivilianTypeVO.COLUMN_audioAttack));
			this.AudioDeath = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(CivilianTypeVO.COLUMN_audioDeath));
			this.AudioPlacement = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(CivilianTypeVO.COLUMN_audioPlacement));
			if (this.RotationSpeed == 0)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Missing rotation speed for civilianTypeVO {0}", new object[]
				{
					this.Uid
				});
			}
		}

		public CivilianTypeVO()
		{
		}

		protected internal CivilianTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_audioAttack);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_audioCharge);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_audioDeath);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_audioPlacement);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_contraband);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_credits);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_faction);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_health);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_materials);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_maxSpeed);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_newRotationSpeed);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_size);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_sizex);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_sizey);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CivilianTypeVO.COLUMN_xp);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).Health);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).Size);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrainingTime);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAttack = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioCharge = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioDeath = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioImpact = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioMovementAway = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioPlacement = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioTrain = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_audioAttack = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_audioCharge = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_audioDeath = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_audioPlacement = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_health = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_materials = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_maxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_newRotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_size = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_sizex = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_sizey = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			CivilianTypeVO.COLUMN_xp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).Health = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).MaxSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).RotationSpeed = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).Size = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).TrainingTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((CivilianTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
