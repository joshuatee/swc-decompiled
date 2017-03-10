using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class PerkVO : IValueObject
	{
		public static int COLUMN_perkGroup
		{
			get;
			private set;
		}

		public static int COLUMN_perkTier
		{
			get;
			private set;
		}

		public static int COLUMN_squadLevelUnlock
		{
			get;
			private set;
		}

		public static int COLUMN_repCost
		{
			get;
			private set;
		}

		public static int COLUMN_activationCost
		{
			get;
			private set;
		}

		public static int COLUMN_perkEffects
		{
			get;
			private set;
		}

		public static int COLUMN_activeDuration
		{
			get;
			private set;
		}

		public static int COLUMN_cooldownDuration
		{
			get;
			private set;
		}

		public static int COLUMN_sortOrder
		{
			get;
			private set;
		}

		public static int COLUMN_sortTabs
		{
			get;
			private set;
		}

		public static int COLUMN_textureIdRebel
		{
			get;
			private set;
		}

		public static int COLUMN_textureIdEmpire
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string PerkGroup
		{
			get;
			private set;
		}

		public int PerkTier
		{
			get;
			private set;
		}

		public int SquadLevelUnlock
		{
			get;
			private set;
		}

		public int ReputationCost
		{
			get;
			private set;
		}

		public string[] ActivationCost
		{
			get;
			private set;
		}

		public string[] PerkEffects
		{
			get;
			private set;
		}

		public string[] FilterTabs
		{
			get;
			private set;
		}

		public int ActiveDurationMinutes
		{
			get;
			private set;
		}

		public int CooldownDurationMinutes
		{
			get;
			private set;
		}

		public int SortOrder
		{
			get;
			private set;
		}

		public string TextureIdRebel
		{
			get;
			private set;
		}

		public string TextureIdEmpire
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.PerkGroup = row.TryGetString(PerkVO.COLUMN_perkGroup);
			this.PerkTier = row.TryGetInt(PerkVO.COLUMN_perkTier);
			this.SquadLevelUnlock = row.TryGetInt(PerkVO.COLUMN_squadLevelUnlock);
			this.ReputationCost = row.TryGetInt(PerkVO.COLUMN_repCost);
			this.ActivationCost = row.TryGetStringArray(PerkVO.COLUMN_activationCost);
			this.FilterTabs = row.TryGetStringArray(PerkVO.COLUMN_sortTabs);
			this.PerkEffects = row.TryGetStringArray(PerkVO.COLUMN_perkEffects);
			this.ActiveDurationMinutes = row.TryGetInt(PerkVO.COLUMN_activeDuration);
			this.CooldownDurationMinutes = row.TryGetInt(PerkVO.COLUMN_cooldownDuration);
			this.SortOrder = row.TryGetInt(PerkVO.COLUMN_sortOrder);
			this.TextureIdRebel = row.TryGetString(PerkVO.COLUMN_textureIdRebel);
			this.TextureIdEmpire = row.TryGetString(PerkVO.COLUMN_textureIdEmpire);
		}

		public PerkVO()
		{
		}

		protected internal PerkVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).ActivationCost);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).ActiveDurationMinutes);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_activationCost);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_activeDuration);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_cooldownDuration);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_perkEffects);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_perkGroup);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_perkTier);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_repCost);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_sortOrder);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_sortTabs);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_squadLevelUnlock);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_textureIdEmpire);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PerkVO.COLUMN_textureIdRebel);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).CooldownDurationMinutes);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).FilterTabs);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).PerkEffects);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).PerkGroup);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).PerkTier);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).ReputationCost);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).SortOrder);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).SquadLevelUnlock);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).TextureIdEmpire);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).TextureIdRebel);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PerkVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).ActivationCost = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).ActiveDurationMinutes = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			PerkVO.COLUMN_activationCost = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			PerkVO.COLUMN_activeDuration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			PerkVO.COLUMN_cooldownDuration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			PerkVO.COLUMN_perkEffects = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			PerkVO.COLUMN_perkGroup = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			PerkVO.COLUMN_perkTier = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			PerkVO.COLUMN_repCost = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			PerkVO.COLUMN_sortOrder = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			PerkVO.COLUMN_sortTabs = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			PerkVO.COLUMN_squadLevelUnlock = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			PerkVO.COLUMN_textureIdEmpire = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			PerkVO.COLUMN_textureIdRebel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).CooldownDurationMinutes = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).FilterTabs = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).PerkEffects = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).PerkGroup = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).PerkTier = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).ReputationCost = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).SortOrder = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).SquadLevelUnlock = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).TextureIdEmpire = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).TextureIdRebel = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((PerkVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
