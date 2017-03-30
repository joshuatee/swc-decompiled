using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaRTS.Main.Controllers.Performance
{
	public class PerformanceSampler
	{
		private Dictionary<string, NamedSample> sampleMap;

		public PerformanceSampler()
		{
			Service.Set<PerformanceSampler>(this);
			this.sampleMap = null;
		}

		public void BeginSample(string sampleName)
		{
			if (this.sampleMap == null)
			{
				Service.Get<Logger>().Warn("PerformanceSampler enabled, do not commit!");
				this.sampleMap = new Dictionary<string, NamedSample>();
			}
			NamedSample namedSample;
			if (this.sampleMap.ContainsKey(sampleName))
			{
				namedSample = this.sampleMap[sampleName];
			}
			else
			{
				namedSample = new NamedSample(sampleName);
				this.sampleMap.Add(sampleName, namedSample);
			}
			namedSample.lastTime = UnityUtils.GetRealTimeSinceStartUp();
		}

		public float EndSample(string sampleName)
		{
			float realTimeSinceStartUp = UnityUtils.GetRealTimeSinceStartUp();
			float num = 0f;
			if (this.sampleMap != null && this.sampleMap.ContainsKey(sampleName))
			{
				NamedSample namedSample = this.sampleMap[sampleName];
				namedSample.count++;
				num = realTimeSinceStartUp - namedSample.lastTime;
				if (num > namedSample.peakTime)
				{
					namedSample.peakTime = num;
				}
				namedSample.totalTime += num;
			}
			return num;
		}

		public void LogAndReset()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Samples:");
			if (this.sampleMap != null)
			{
				List<NamedSample> list = new List<NamedSample>();
				foreach (NamedSample current in this.sampleMap.Values)
				{
					current.average = ((current.count != 0) ? (1000f * current.totalTime / (float)current.count) : 0f);
					list.Add(current);
				}
				list.Sort(new Comparison<NamedSample>(PerformanceSampler.CompareSampleAverage));
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					NamedSample namedSample = list[i];
					stringBuilder.Append(string.Format("\n{0}: {1:F2}s / {2} = {3:F2}ms, {4:F2}ms peak", new object[]
					{
						namedSample.name,
						namedSample.totalTime,
						namedSample.count,
						namedSample.average,
						namedSample.peakTime * 1000f
					}));
					i++;
				}
				this.sampleMap = null;
				Service.Get<Logger>().Debug("PerformanceSampler disabled, writing to log...");
			}
			Service.Get<Logger>().Debug(stringBuilder.ToString());
		}

		private static int CompareSampleAverage(NamedSample a, NamedSample b)
		{
			return (int)((b.average - a.average) * 100f);
		}
	}
}
