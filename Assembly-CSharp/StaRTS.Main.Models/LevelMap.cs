using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class LevelMap : ISerializable
	{
		private Dictionary<string, int> levels;

		public IDictionary<string, int> Levels
		{
			get
			{
				return this.levels;
			}
		}

		public LevelMap()
		{
			this.levels = new Dictionary<string, int>();
		}

		public bool Has(IUpgradeableVO vo)
		{
			return this.Has(vo.UpgradeGroup);
		}

		public bool Has(string groupId)
		{
			return this.levels.ContainsKey(groupId);
		}

		public void SetLevel(IUpgradeableVO upgradeable)
		{
			this.SetLevel(upgradeable.UpgradeGroup, upgradeable.Lvl);
		}

		public void SetLevel(string groupName, int level)
		{
			if (this.levels.ContainsKey(groupName))
			{
				this.levels[groupName] = level;
				return;
			}
			this.levels.Add(groupName, level);
		}

		public int GetLevel(string groupName)
		{
			if (!this.levels.ContainsKey(groupName))
			{
				return 1;
			}
			return this.levels[groupName];
		}

		public int GetNextLevel(string groupName)
		{
			if (!this.levels.ContainsKey(groupName))
			{
				return 2;
			}
			return this.levels[groupName] + 1;
		}

		public string ToJson()
		{
			Serializer serializer = Serializer.Start();
			return serializer.End().ToString();
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				foreach (KeyValuePair<string, object> current in dictionary)
				{
					this.SetLevel(current.get_Key(), Convert.ToInt32(current.get_Value(), CultureInfo.InvariantCulture));
				}
			}
			return this;
		}

		protected internal LevelMap(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LevelMap)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LevelMap)GCHandledObjects.GCHandleToObject(instance)).Levels);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LevelMap)GCHandledObjects.GCHandleToObject(instance)).GetLevel(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LevelMap)GCHandledObjects.GCHandleToObject(instance)).GetNextLevel(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LevelMap)GCHandledObjects.GCHandleToObject(instance)).Has((IUpgradeableVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LevelMap)GCHandledObjects.GCHandleToObject(instance)).Has(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((LevelMap)GCHandledObjects.GCHandleToObject(instance)).SetLevel((IUpgradeableVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((LevelMap)GCHandledObjects.GCHandleToObject(instance)).SetLevel(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LevelMap)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
