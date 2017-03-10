using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class SkinOverrideTypeVO : IValueObject
	{
		public static int COLUMN_projectileType
		{
			get;
			private set;
		}

		public static int COLUMN_shotCount
		{
			get;
			private set;
		}

		public static int COLUMN_chargeTime
		{
			get;
			private set;
		}

		public static int COLUMN_animationDelay
		{
			get;
			private set;
		}

		public static int COLUMN_shotDelay
		{
			get;
			private set;
		}

		public static int COLUMN_reload
		{
			get;
			private set;
		}

		public static int COLUMN_gunSequence
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public ProjectileTypeVO ProjectileType
		{
			get;
			set;
		}

		public uint ShotCount
		{
			get;
			set;
		}

		public uint WarmupDelay
		{
			get;
			set;
		}

		public uint AnimationDelay
		{
			get;
			set;
		}

		public uint ShotDelay
		{
			get;
			set;
		}

		public uint CooldownDelay
		{
			get;
			set;
		}

		public int[] GunSequence
		{
			get;
			set;
		}

		public Dictionary<int, int> Sequences
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			IDataController dataController = Service.Get<IDataController>();
			string text = row.TryGetString(SkinOverrideTypeVO.COLUMN_projectileType);
			if (!string.IsNullOrEmpty(text))
			{
				this.ProjectileType = dataController.Get<ProjectileTypeVO>(row.TryGetString(SkinOverrideTypeVO.COLUMN_projectileType));
			}
			this.WarmupDelay = row.TryGetUint(SkinOverrideTypeVO.COLUMN_chargeTime);
			this.AnimationDelay = row.TryGetUint(SkinOverrideTypeVO.COLUMN_animationDelay);
			this.ShotDelay = row.TryGetUint(SkinOverrideTypeVO.COLUMN_shotDelay);
			this.CooldownDelay = row.TryGetUint(SkinOverrideTypeVO.COLUMN_reload);
			this.ShotCount = row.TryGetUint(SkinOverrideTypeVO.COLUMN_shotCount);
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			SequencePair gunSequences = valueObjectController.GetGunSequences(this.Uid, row.TryGetString(SkinOverrideTypeVO.COLUMN_gunSequence));
			this.GunSequence = gunSequences.GunSequence;
			this.Sequences = gunSequences.Sequences;
		}

		public SkinOverrideTypeVO()
		{
		}

		protected internal SkinOverrideTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinOverrideTypeVO.COLUMN_animationDelay);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinOverrideTypeVO.COLUMN_chargeTime);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinOverrideTypeVO.COLUMN_gunSequence);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinOverrideTypeVO.COLUMN_projectileType);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinOverrideTypeVO.COLUMN_reload);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinOverrideTypeVO.COLUMN_shotCount);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SkinOverrideTypeVO.COLUMN_shotDelay);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinOverrideTypeVO)GCHandledObjects.GCHandleToObject(instance)).GunSequence);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinOverrideTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinOverrideTypeVO)GCHandledObjects.GCHandleToObject(instance)).Sequences);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinOverrideTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SkinOverrideTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			SkinOverrideTypeVO.COLUMN_animationDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			SkinOverrideTypeVO.COLUMN_chargeTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			SkinOverrideTypeVO.COLUMN_gunSequence = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			SkinOverrideTypeVO.COLUMN_projectileType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			SkinOverrideTypeVO.COLUMN_reload = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			SkinOverrideTypeVO.COLUMN_shotCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			SkinOverrideTypeVO.COLUMN_shotDelay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((SkinOverrideTypeVO)GCHandledObjects.GCHandleToObject(instance)).GunSequence = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SkinOverrideTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileType = (ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SkinOverrideTypeVO)GCHandledObjects.GCHandleToObject(instance)).Sequences = (Dictionary<int, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((SkinOverrideTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
