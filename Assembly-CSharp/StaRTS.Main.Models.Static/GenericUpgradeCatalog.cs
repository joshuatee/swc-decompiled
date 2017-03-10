using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Static
{
	public abstract class GenericUpgradeCatalog<T> : AbstractUpgradeCatalog where T : IValueObject, IUpgradeableVO
	{
		public const int MIN_UPGRADE_LEVEL = 1;

		private Dictionary<string, List<T>> upgradeGroups;

		private Dictionary<string, T> maxLevels;

		private Dictionary<string, T> maxLevelsForRewards;

		public GenericUpgradeCatalog()
		{
			this.InitService();
			IDataController dataController = Service.Get<IDataController>();
			this.upgradeGroups = new Dictionary<string, List<T>>();
			this.maxLevels = new Dictionary<string, T>();
			this.maxLevelsForRewards = new Dictionary<string, T>();
			foreach (T current in dataController.GetAll<T>())
			{
				List<T> list;
				if (this.upgradeGroups.ContainsKey(current.UpgradeGroup))
				{
					list = this.upgradeGroups[current.UpgradeGroup];
				}
				else
				{
					list = new List<T>();
					this.upgradeGroups.Add(current.UpgradeGroup, list);
				}
				if (this.maxLevels.ContainsKey(current.UpgradeGroup))
				{
					T t = this.maxLevels[current.UpgradeGroup];
					if (current.Lvl > t.Lvl && current.PlayerFacing)
					{
						this.maxLevels[current.UpgradeGroup] = current;
					}
				}
				else
				{
					this.maxLevels[current.UpgradeGroup] = current;
				}
				if (this.maxLevelsForRewards.ContainsKey(current.UpgradeGroup))
				{
					T t2 = this.maxLevelsForRewards[current.UpgradeGroup];
					if (current.Lvl > t2.Lvl)
					{
						this.maxLevelsForRewards[current.UpgradeGroup] = current;
					}
				}
				else
				{
					this.maxLevelsForRewards[current.UpgradeGroup] = current;
				}
				list.Add(current);
			}
			foreach (KeyValuePair<string, List<T>> current2 in this.upgradeGroups)
			{
				current2.get_Value().Sort(new Comparison<T>(GenericUpgradeCatalog<T>.SortByLevelAscending));
				int arg_23A_0 = current2.get_Value().Count;
				T t3 = this.maxLevels[current2.get_Key()];
				if (arg_23A_0 != t3.Lvl)
				{
					for (int i = 1; i < current2.get_Value().Count; i++)
					{
						t3 = current2.get_Value()[i];
						int arg_283_0 = t3.Lvl;
						t3 = current2.get_Value()[i - 1];
						if (arg_283_0 == t3.Lvl)
						{
							StaRTSLogger arg_330_0 = Service.Get<StaRTSLogger>();
							string arg_330_1 = "Duplicate levels in group {4}: {0} ({1}) AND {2} ({3})";
							object[] expr_298 = new object[5];
							int arg_2B7_1 = 0;
							t3 = current2.get_Value()[i];
							expr_298[arg_2B7_1] = t3.Uid;
							int arg_2DC_1 = 1;
							t3 = current2.get_Value()[i];
							expr_298[arg_2DC_1] = t3.Lvl;
							int arg_2FE_1 = 2;
							t3 = current2.get_Value()[i - 1];
							expr_298[arg_2FE_1] = t3.Uid;
							int arg_325_1 = 3;
							t3 = current2.get_Value()[i - 1];
							expr_298[arg_325_1] = t3.Lvl;
							expr_298[4] = current2.get_Key();
							arg_330_0.ErrorFormat(arg_330_1, expr_298);
						}
						else
						{
							t3 = current2.get_Value()[i];
							int arg_378_0 = t3.Lvl;
							t3 = current2.get_Value()[i - 1];
							if (arg_378_0 != t3.Lvl + 1)
							{
								StaRTSLogger arg_425_0 = Service.Get<StaRTSLogger>();
								string arg_425_1 = "In Group {4} expected {0} ({1}) to be one level higher than {2} ({3})";
								object[] expr_38D = new object[5];
								int arg_3AC_1 = 0;
								t3 = current2.get_Value()[i];
								expr_38D[arg_3AC_1] = t3.Uid;
								int arg_3D1_1 = 1;
								t3 = current2.get_Value()[i];
								expr_38D[arg_3D1_1] = t3.Lvl;
								int arg_3F3_1 = 2;
								t3 = current2.get_Value()[i - 1];
								expr_38D[arg_3F3_1] = t3.Uid;
								int arg_41A_1 = 3;
								t3 = current2.get_Value()[i - 1];
								expr_38D[arg_41A_1] = t3.Lvl;
								expr_38D[4] = current2.get_Key();
								arg_425_0.ErrorFormat(arg_425_1, expr_38D);
							}
						}
					}
				}
			}
		}

		protected abstract void InitService();

		public List<T> GetUpgradeGroupLevels(string upgradeGroup)
		{
			return this.upgradeGroups[upgradeGroup];
		}

		public List<string> GetIDCollection()
		{
			return new List<string>(this.upgradeGroups.Keys);
		}

		public T GetByLevel(string upgradeGroup, int level)
		{
			List<T> upgradeGroupLevels = this.GetUpgradeGroupLevels(upgradeGroup);
			int num = level - 1;
			if (num < upgradeGroupLevels.Count)
			{
				return upgradeGroupLevels[num];
			}
			return default(T);
		}

		public T GetByLevel(T vo, int level)
		{
			return this.GetByLevel(vo.UpgradeGroup, level);
		}

		public T GetNextLevel(T vo)
		{
			return this.GetByLevel(vo.UpgradeGroup, vo.Lvl + 1);
		}

		public T GetPrevLevel(T vo)
		{
			return this.GetByLevel(vo.UpgradeGroup, vo.Lvl - 1);
		}

		protected override IUpgradeableVO InternalGetByLevel(string upgradeGroup, int level)
		{
			return this.GetUpgradeGroupLevels(upgradeGroup)[level - 1];
		}

		public Dictionary<string, List<T>>.KeyCollection AllUpgradeGroups()
		{
			return this.upgradeGroups.Keys;
		}

		public T GetMaxLevel(string upgradeGroup)
		{
			return this.maxLevels[upgradeGroup];
		}

		public T GetMaxLevel(T upgradeable)
		{
			return this.GetMaxLevel(upgradeable.UpgradeGroup);
		}

		public T GetMaxRewardableLevel(string upgradeGroup)
		{
			return this.maxLevelsForRewards[upgradeGroup];
		}

		public T GetMaxRewardableLevel(T upgradeable)
		{
			return this.GetMaxRewardableLevel(upgradeable.UpgradeGroup);
		}

		public T GetMinLevel(string upgradeGroup)
		{
			return this.GetByLevel(upgradeGroup, 1);
		}

		public T GetMinLevel(T upgradeable)
		{
			return this.GetMinLevel(upgradeable.UpgradeGroup);
		}

		public bool CanUpgradeTo(LevelMap levelMap, T upgradeable)
		{
			return upgradeable.Lvl - levelMap.GetLevel(upgradeable.UpgradeGroup) == 1;
		}

		public static int SortByLevelAscending(T a, T b)
		{
			return a.Lvl - b.Lvl;
		}

		protected internal GenericUpgradeCatalog(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GenericUpgradeCatalog<BuildingTypeVO>)GCHandledObjects.GCHandleToObject(instance)).GetIDCollection());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GenericUpgradeCatalog<BuildingTypeVO>)GCHandledObjects.GCHandleToObject(instance)).InitService();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GenericUpgradeCatalog<BuildingTypeVO>)GCHandledObjects.GCHandleToObject(instance)).InternalGetByLevel(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GenericUpgradeCatalog<EquipmentVO>)GCHandledObjects.GCHandleToObject(instance)).GetIDCollection());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GenericUpgradeCatalog<EquipmentVO>)GCHandledObjects.GCHandleToObject(instance)).InitService();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GenericUpgradeCatalog<EquipmentVO>)GCHandledObjects.GCHandleToObject(instance)).InternalGetByLevel(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GenericUpgradeCatalog<SpecialAttackTypeVO>)GCHandledObjects.GCHandleToObject(instance)).GetIDCollection());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((GenericUpgradeCatalog<SpecialAttackTypeVO>)GCHandledObjects.GCHandleToObject(instance)).InitService();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GenericUpgradeCatalog<SpecialAttackTypeVO>)GCHandledObjects.GCHandleToObject(instance)).InternalGetByLevel(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GenericUpgradeCatalog<TroopTypeVO>)GCHandledObjects.GCHandleToObject(instance)).GetIDCollection());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((GenericUpgradeCatalog<TroopTypeVO>)GCHandledObjects.GCHandleToObject(instance)).InitService();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GenericUpgradeCatalog<TroopTypeVO>)GCHandledObjects.GCHandleToObject(instance)).InternalGetByLevel(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}
	}
}
