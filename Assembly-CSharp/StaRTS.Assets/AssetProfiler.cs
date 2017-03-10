using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Assets
{
	public class AssetProfiler
	{
		private const string FILE_PATH = "/src";

		private const string FILE_NAME = "starts-assetprofile.txt";

		private Dictionary<string, AssetProfilerFetchData> assets;

		private StringBuilder info;

		private float totalMBDownloaded;

		private float totalMBPulledFromCache;

		private float totalMBServerTraffic;

		private float enableTime;

		private bool enabled;

		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (value)
				{
					Service.Get<StaRTSLogger>().Error("Cannot use AssetProfiler in Release build");
					return;
				}
				if (this.enabled != value)
				{
					this.enabled = value;
					if (this.enabled)
					{
						Service.Get<StaRTSLogger>().Warn("AssetProfiler enabled, do not commit!");
						this.assets = new Dictionary<string, AssetProfilerFetchData>();
						this.info = new StringBuilder();
						this.enableTime = UnityUtils.GetRealTimeSinceStartUp();
						this.info.Append("time\tevent\tsize (KB)\ttotal (MB)\tname\tdltime\n");
					}
					else
					{
						this.AppendUnreceivedFetchedAssets();
						string[] array = this.info.ToString().Split(new char[]
						{
							'\n'
						});
						int i = 0;
						int num = array.Length;
						while (i < num)
						{
							Service.Get<StaRTSLogger>().Debug(array[i]);
							i++;
						}
						this.assets = null;
						this.info = null;
						this.enableTime = 0f;
						Service.Get<StaRTSLogger>().Debug("AssetProfiler disabled");
					}
					this.totalMBDownloaded = 0f;
					this.totalMBPulledFromCache = 0f;
					this.totalMBServerTraffic = 0f;
				}
			}
		}

		public void RecordEvent(string eventInfo)
		{
			if (!this.enabled)
			{
				return;
			}
			this.AppendLine(eventInfo);
		}

		public void RecordFetchEvent(string name, int bytes, bool serverTraffic, bool received)
		{
			if (!this.enabled)
			{
				return;
			}
			int num = bytes + 1023 >> 10;
			float num2 = (float)bytes / 1048576f;
			if (serverTraffic)
			{
				this.totalMBServerTraffic += num2;
				string line = string.Format("server\t{0}\t{1:F2}\t{2}", new object[]
				{
					num,
					this.totalMBServerTraffic,
					name
				});
				this.AppendLine(line);
				return;
			}
			if (received)
			{
				this.StripOffPath(ref name);
				string line2;
				if (this.assets.ContainsKey(name))
				{
					AssetProfilerFetchData assetProfilerFetchData = this.assets[name];
					float num3 = UnityUtils.GetRealTimeSinceStartUp() - assetProfilerFetchData.FetchTime;
					assetProfilerFetchData.FetchTime = 0f;
					if (assetProfilerFetchData.FetchCount > 1)
					{
						this.totalMBPulledFromCache += num2;
						line2 = string.Format("cache\t{0}\t{1:F2}\t{2}\t{3:F3}", new object[]
						{
							num,
							this.totalMBPulledFromCache,
							name,
							num3
						});
					}
					else
					{
						this.totalMBDownloaded += num2;
						line2 = string.Format("*FETCH*\t{0}\t{1:F2}\t{2}\t{3:F3}", new object[]
						{
							num,
							this.totalMBDownloaded,
							name,
							num3
						});
					}
				}
				else
				{
					line2 = "*UNKNOWN*\t\t\t" + name;
				}
				this.AppendLine(line2);
				return;
			}
			this.StripOffPath(ref name);
			AssetProfilerFetchData assetProfilerFetchData2;
			if (this.assets.ContainsKey(name))
			{
				assetProfilerFetchData2 = this.assets[name];
			}
			else
			{
				assetProfilerFetchData2 = new AssetProfilerFetchData(name);
				this.assets.Add(name, assetProfilerFetchData2);
			}
			AssetProfilerFetchData expr_193 = assetProfilerFetchData2;
			int fetchCount = expr_193.FetchCount;
			expr_193.FetchCount = fetchCount + 1;
			assetProfilerFetchData2.FetchTime = UnityUtils.GetRealTimeSinceStartUp();
		}

		private void StripOffPath(ref string name)
		{
			int num = name.LastIndexOf('/');
			if (num >= 0)
			{
				name = name.Substring(num + 1);
			}
		}

		private void AppendUnreceivedFetchedAssets()
		{
			foreach (AssetProfilerFetchData current in this.assets.Values)
			{
				if (current.FetchTime > 0f)
				{
					string line = "*UNRCVD*\t\t\t" + current.AssetName;
					this.AppendLine(line);
				}
			}
		}

		private void AppendLine(string line)
		{
			float num = UnityUtils.GetRealTimeSinceStartUp() - this.enableTime;
			string text = string.Format("{0:F3}", new object[]
			{
				num
			});
			this.info.Append(text);
			this.info.Append('\t');
			this.info.Append(line);
			this.info.Append('\n');
		}

		public AssetProfiler()
		{
		}

		protected internal AssetProfiler(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AssetProfiler)GCHandledObjects.GCHandleToObject(instance)).AppendLine(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AssetProfiler)GCHandledObjects.GCHandleToObject(instance)).AppendUnreceivedFetchedAssets();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetProfiler)GCHandledObjects.GCHandleToObject(instance)).Enabled);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AssetProfiler)GCHandledObjects.GCHandleToObject(instance)).RecordEvent(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AssetProfiler)GCHandledObjects.GCHandleToObject(instance)).RecordFetchEvent(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AssetProfiler)GCHandledObjects.GCHandleToObject(instance)).Enabled = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
