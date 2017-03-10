using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TournamentTierVO : IValueObject
	{
		public static int COLUMN_order
		{
			get;
			private set;
		}

		public static int COLUMN_percentage
		{
			get;
			private set;
		}

		public static int COLUMN_points
		{
			get;
			private set;
		}

		public static int COLUMN_rankName
		{
			get;
			private set;
		}

		public static int COLUMN_spriteNameEmpire
		{
			get;
			private set;
		}

		public static int COLUMN_spriteNameRebel
		{
			get;
			private set;
		}

		public static int COLUMN_division
		{
			get;
			private set;
		}

		public static int COLUMN_divisionSmall
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public int Order
		{
			get;
			private set;
		}

		public float Percentage
		{
			get;
			private set;
		}

		public int Points
		{
			get;
			private set;
		}

		public string RankName
		{
			get;
			private set;
		}

		public string SpriteNameEmpire
		{
			get;
			private set;
		}

		public string SpriteNameRebel
		{
			get;
			private set;
		}

		public string Division
		{
			get;
			set;
		}

		public string DivisionSmall
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Order = row.TryGetInt(TournamentTierVO.COLUMN_order);
			this.Percentage = row.TryGetFloat(TournamentTierVO.COLUMN_percentage);
			this.Points = row.TryGetInt(TournamentTierVO.COLUMN_points);
			this.RankName = row.TryGetString(TournamentTierVO.COLUMN_rankName);
			this.SpriteNameEmpire = row.TryGetString(TournamentTierVO.COLUMN_spriteNameEmpire);
			this.SpriteNameRebel = row.TryGetString(TournamentTierVO.COLUMN_spriteNameRebel);
			this.Division = row.TryGetString(TournamentTierVO.COLUMN_division);
			this.DivisionSmall = row.TryGetString(TournamentTierVO.COLUMN_divisionSmall);
		}

		public TournamentTierVO()
		{
		}

		protected internal TournamentTierVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentTierVO.COLUMN_division);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentTierVO.COLUMN_divisionSmall);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentTierVO.COLUMN_order);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentTierVO.COLUMN_percentage);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentTierVO.COLUMN_points);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentTierVO.COLUMN_rankName);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentTierVO.COLUMN_spriteNameEmpire);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TournamentTierVO.COLUMN_spriteNameRebel);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).Division);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).DivisionSmall);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).Order);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).Percentage);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).Points);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).RankName);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).SpriteNameEmpire);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).SpriteNameRebel);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			TournamentTierVO.COLUMN_division = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			TournamentTierVO.COLUMN_divisionSmall = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			TournamentTierVO.COLUMN_order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			TournamentTierVO.COLUMN_percentage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			TournamentTierVO.COLUMN_points = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			TournamentTierVO.COLUMN_rankName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			TournamentTierVO.COLUMN_spriteNameEmpire = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			TournamentTierVO.COLUMN_spriteNameRebel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).Division = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).DivisionSmall = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).Order = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).Percentage = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).Points = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).RankName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).SpriteNameEmpire = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).SpriteNameRebel = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((TournamentTierVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
