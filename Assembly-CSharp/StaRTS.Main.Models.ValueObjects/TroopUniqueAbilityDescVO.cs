using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TroopUniqueAbilityDescVO : IValueObject
	{
		public static int COLUMN_unitID
		{
			get;
			private set;
		}

		public static int COLUMN_abilityTitle1
		{
			get;
			private set;
		}

		public static int COLUMN_abilityDesc1
		{
			get;
			private set;
		}

		public static int COLUMN_abilityTitle2
		{
			get;
			private set;
		}

		public static int COLUMN_abilityDesc2
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string TroopID
		{
			get;
			private set;
		}

		public string AbilityTitle1
		{
			get;
			private set;
		}

		public string AbilityDesc1
		{
			get;
			private set;
		}

		public string AbilityTitle2
		{
			get;
			private set;
		}

		public string AbilityDesc2
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.TroopID = row.TryGetString(TroopUniqueAbilityDescVO.COLUMN_unitID);
			this.AbilityTitle1 = row.TryGetString(TroopUniqueAbilityDescVO.COLUMN_abilityTitle1);
			this.AbilityDesc1 = row.TryGetString(TroopUniqueAbilityDescVO.COLUMN_abilityDesc1);
			this.AbilityTitle2 = row.TryGetString(TroopUniqueAbilityDescVO.COLUMN_abilityTitle2);
			this.AbilityDesc2 = row.TryGetString(TroopUniqueAbilityDescVO.COLUMN_abilityDesc2);
			if (!string.IsNullOrEmpty(this.TroopID))
			{
				IDataController dataController = Service.Get<IDataController>();
				Dictionary<string, TroopTypeVO>.ValueCollection all = dataController.GetAll<TroopTypeVO>();
				foreach (TroopTypeVO current in all)
				{
					if (current.TroopID == this.TroopID)
					{
						current.UniqueAbilityDescVO = this;
					}
				}
			}
		}

		public TroopUniqueAbilityDescVO()
		{
		}

		protected internal TroopUniqueAbilityDescVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).AbilityDesc1);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).AbilityDesc2);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).AbilityTitle1);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).AbilityTitle2);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopUniqueAbilityDescVO.COLUMN_abilityDesc1);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopUniqueAbilityDescVO.COLUMN_abilityDesc2);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopUniqueAbilityDescVO.COLUMN_abilityTitle1);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopUniqueAbilityDescVO.COLUMN_abilityTitle2);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TroopUniqueAbilityDescVO.COLUMN_unitID);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).TroopID);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).AbilityDesc1 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).AbilityDesc2 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).AbilityTitle1 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).AbilityTitle2 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			TroopUniqueAbilityDescVO.COLUMN_abilityDesc1 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			TroopUniqueAbilityDescVO.COLUMN_abilityDesc2 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			TroopUniqueAbilityDescVO.COLUMN_abilityTitle1 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			TroopUniqueAbilityDescVO.COLUMN_abilityTitle2 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			TroopUniqueAbilityDescVO.COLUMN_unitID = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).TroopID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((TroopUniqueAbilityDescVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
