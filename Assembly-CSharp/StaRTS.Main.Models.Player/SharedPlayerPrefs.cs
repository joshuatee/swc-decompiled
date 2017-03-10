using StaRTS.Externals.Manimal;
using StaRTS.Main.Models.Commands;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player
{
	public class SharedPlayerPrefs
	{
		private const int MAX_SERIALIZED_VALUE_LENGTH = 64;

		private Dictionary<string, string> localCache;

		public SharedPlayerPrefs()
		{
			Service.Set<SharedPlayerPrefs>(this);
			this.localCache = new Dictionary<string, string>();
		}

		public void Populate(Dictionary<string, object> map)
		{
			if (map != null)
			{
				foreach (string current in map.Keys)
				{
					this.localCache.Add(current, map[current] as string);
				}
			}
		}

		public T GetPref<T>(string prefName)
		{
			if (!string.IsNullOrEmpty(prefName) && this.localCache.ContainsKey(prefName))
			{
				return (T)((object)Convert.ChangeType(this.localCache[prefName], typeof(T), CultureInfo.InvariantCulture));
			}
			return default(T);
		}

		public void SetPref(string prefName, string value)
		{
			if (value != null && value.get_Length() > 64)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Value not saved.  SharedPref value is too large.\r\nPref:{0} Value:{1}\r\nSerialized length ({2}) is greater than the max value ({3}).", new object[]
				{
					prefName,
					value,
					value.get_Length(),
					64
				});
				return;
			}
			this.SetPrefInternal(prefName, value);
		}

		public void SetPrefUnlimitedLength(string prefName, string value)
		{
			this.SetPrefInternal(prefName, value);
		}

		private void SetPrefInternal(string prefName, string value)
		{
			bool flag = false;
			if (value != null)
			{
				string text;
				if (this.localCache.TryGetValue(prefName, out text))
				{
					if (text != value)
					{
						this.localCache[prefName] = value;
						flag = true;
					}
				}
				else
				{
					this.localCache.Add(prefName, value);
					flag = true;
				}
			}
			else if (this.localCache.Remove(prefName))
			{
				flag = true;
			}
			if (flag)
			{
				Service.Get<ServerAPI>().Enqueue(new SaveSharedPrefCommand(prefName, value));
			}
		}

		protected internal SharedPlayerPrefs(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SharedPlayerPrefs)GCHandledObjects.GCHandleToObject(instance)).Populate((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SharedPlayerPrefs)GCHandledObjects.GCHandleToObject(instance)).SetPref(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SharedPlayerPrefs)GCHandledObjects.GCHandleToObject(instance)).SetPrefInternal(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SharedPlayerPrefs)GCHandledObjects.GCHandleToObject(instance)).SetPrefUnlimitedLength(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}
	}
}
