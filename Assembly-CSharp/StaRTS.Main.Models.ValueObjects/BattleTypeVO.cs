using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Battle;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class BattleTypeVO : BattleDeploymentData, IValueObject, IAssetVO
	{
		public static int COLUMN_battleName
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

		public static int COLUMN_planet
		{
			get;
			private set;
		}

		public static int COLUMN_defenderName
		{
			get;
			private set;
		}

		public static int COLUMN_troopData
		{
			get;
			private set;
		}

		public static int COLUMN_specialAttackData
		{
			get;
			private set;
		}

		public static int COLUMN_heroData
		{
			get;
			private set;
		}

		public static int COLUMN_championData
		{
			get;
			private set;
		}

		public static int COLUMN_multipleHeroDeploys
		{
			get;
			private set;
		}

		public static int COLUMN_overridePlayerUnits
		{
			get;
			private set;
		}

		public static int COLUMN_battleTime
		{
			get;
			private set;
		}

		public static int COLUMN_encounterProfile
		{
			get;
			private set;
		}

		public static int COLUMN_BattleScript
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string BattleName
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

		public string Planet
		{
			get;
			set;
		}

		public string DefenderId
		{
			get;
			set;
		}

		public int BattleTime
		{
			get;
			set;
		}

		public bool MultipleHeroDeploys
		{
			get;
			set;
		}

		public bool OverridePlayerUnits
		{
			get;
			set;
		}

		public string EncounterProfile
		{
			get;
			set;
		}

		public string BattleScript
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.BattleName = row.TryGetString(BattleTypeVO.COLUMN_battleName);
			this.AssetName = row.TryGetString(BattleTypeVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(BattleTypeVO.COLUMN_bundleName);
			this.Planet = row.TryGetString(BattleTypeVO.COLUMN_planet);
			this.DefenderId = row.TryGetString(BattleTypeVO.COLUMN_defenderName);
			if (this.DefenderId == null || this.DefenderId.get_Length() == 0)
			{
				this.DefenderId = "icoNeutral";
			}
			base.TroopData = this.GetData(row, BattleTypeVO.COLUMN_troopData);
			base.SpecialAttackData = this.GetData(row, BattleTypeVO.COLUMN_specialAttackData);
			base.HeroData = this.GetData(row, BattleTypeVO.COLUMN_heroData);
			base.ChampionData = this.GetData(row, BattleTypeVO.COLUMN_championData);
			this.MultipleHeroDeploys = row.TryGetBool(BattleTypeVO.COLUMN_multipleHeroDeploys);
			this.OverridePlayerUnits = row.TryGetBool(BattleTypeVO.COLUMN_overridePlayerUnits);
			this.BattleTime = row.TryGetInt(BattleTypeVO.COLUMN_battleTime);
			this.EncounterProfile = row.TryGetString(BattleTypeVO.COLUMN_encounterProfile);
			this.BattleScript = row.TryGetString(BattleTypeVO.COLUMN_BattleScript);
		}

		private Dictionary<string, int> GetData(Row row, int columnIndex)
		{
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			List<StrIntPair> strIntPairs = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(columnIndex));
			if (strIntPairs == null)
			{
				return null;
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			int i = 0;
			int count = strIntPairs.Count;
			while (i < count)
			{
				StrIntPair strIntPair = strIntPairs[i];
				dictionary.Add(strIntPair.StrKey, strIntPair.IntVal);
				i++;
			}
			return dictionary;
		}

		public BattleTypeVO()
		{
		}

		protected internal BattleTypeVO(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).BattleName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).BattleScript);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).BattleTime);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_battleName);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_BattleScript);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_battleTime);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_championData);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_defenderName);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_encounterProfile);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_heroData);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_multipleHeroDeploys);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_overridePlayerUnits);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_planet);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_specialAttackData);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleTypeVO.COLUMN_troopData);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).DefenderId);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).EncounterProfile);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).MultipleHeroDeploys);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).OverridePlayerUnits);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).Planet);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).GetData((Row)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).BattleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).BattleScript = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).BattleTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			BattleTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			BattleTypeVO.COLUMN_battleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			BattleTypeVO.COLUMN_BattleScript = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			BattleTypeVO.COLUMN_battleTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			BattleTypeVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			BattleTypeVO.COLUMN_championData = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			BattleTypeVO.COLUMN_defenderName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			BattleTypeVO.COLUMN_encounterProfile = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			BattleTypeVO.COLUMN_heroData = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			BattleTypeVO.COLUMN_multipleHeroDeploys = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			BattleTypeVO.COLUMN_overridePlayerUnits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			BattleTypeVO.COLUMN_planet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			BattleTypeVO.COLUMN_specialAttackData = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			BattleTypeVO.COLUMN_troopData = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).DefenderId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).EncounterProfile = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).MultipleHeroDeploys = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).OverridePlayerUnits = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).Planet = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((BattleTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
