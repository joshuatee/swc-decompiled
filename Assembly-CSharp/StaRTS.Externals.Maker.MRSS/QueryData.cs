using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Externals.Maker.MRSS
{
	public class QueryData
	{
		private string[] tags;

		private string[] keywords;

		private List<KeyValuePair<List<string>, bool>> allResults;

		private KeyValuePair<List<string>, bool> intersection;

		private List<VideoDataManager.DataListQueryCompleteDelegate> callbacks;

		public VideoSection Section
		{
			get;
			private set;
		}

		public bool Active
		{
			get;
			set;
		}

		public QueryData(VideoSection section, string[] tags, string[] keywords, VideoDataManager.DataListQueryCompleteDelegate callback)
		{
			this.tags = tags;
			this.keywords = keywords;
			this.callbacks = new List<VideoDataManager.DataListQueryCompleteDelegate>();
			this.callbacks.Add(callback);
			this.Active = true;
			this.allResults = new List<KeyValuePair<List<string>, bool>>();
			this.Section = section;
		}

		public void AddCallback(VideoDataManager.DataListQueryCompleteDelegate cb)
		{
			this.callbacks.Add(cb);
		}

		private bool ParamsEqual(string[] paramList1, string[] paramList2)
		{
			if (paramList1 == paramList2)
			{
				return true;
			}
			if (paramList1 == null || paramList2 == null || paramList1.Length != paramList2.Length)
			{
				return false;
			}
			for (int i = 0; i < paramList1.Length; i++)
			{
				if (paramList1[i] != paramList2[i])
				{
					return false;
				}
			}
			return true;
		}

		public bool QueryMatch(VideoSection section, string[] tags, string[] keywords)
		{
			bool result = true;
			if (section != this.Section || !this.ParamsEqual(this.tags, tags) || !this.ParamsEqual(this.keywords, keywords))
			{
				result = false;
			}
			return result;
		}

		public void AddResult(List<string> results, bool sorted)
		{
			KeyValuePair<List<string>, bool> item = new KeyValuePair<List<string>, bool>(results, sorted);
			this.allResults.Add(item);
		}

		private bool IsQueryComplete()
		{
			int num = this.tags.Length;
			num++;
			return this.allResults.Count >= num;
		}

		private void FinishUp(List<string> result)
		{
			int i = 0;
			int count = this.callbacks.Count;
			while (i < count)
			{
				this.callbacks[i](result);
				i++;
			}
			this.callbacks.Clear();
			this.Active = false;
		}

		private bool FilterResponse(KeyValuePair<List<string>, bool> response)
		{
			if (response.get_Key() == null)
			{
				this.FinishUp(null);
				return false;
			}
			if (this.intersection.get_Key() == null)
			{
				this.intersection = new KeyValuePair<List<string>, bool>(new List<string>(response.get_Key()), response.get_Value());
			}
			else
			{
				List<string> key = this.intersection.get_Key();
				List<string> key2 = response.get_Key();
				if (response.get_Value())
				{
					key = response.get_Key();
					key2 = this.intersection.get_Key();
				}
				bool flag = this.intersection.get_Value() || response.get_Value();
				KeyValuePair<List<string>, bool> keyValuePair = new KeyValuePair<List<string>, bool>(new List<string>(), flag);
				int i = 0;
				int count = key.Count;
				while (i < count)
				{
					if (key2.Contains(key[i]))
					{
						keyValuePair.get_Key().Add(key[i]);
					}
					i++;
				}
				this.intersection = keyValuePair;
			}
			return true;
		}

		public void FilterResults()
		{
			if (!this.IsQueryComplete())
			{
				return;
			}
			for (int i = 0; i < this.allResults.Count; i++)
			{
				if (!this.FilterResponse(this.allResults[i]))
				{
					return;
				}
			}
			this.FinishUp(this.intersection.get_Key());
		}

		protected internal QueryData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((QueryData)GCHandledObjects.GCHandleToObject(instance)).AddCallback((VideoDataManager.DataListQueryCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((QueryData)GCHandledObjects.GCHandleToObject(instance)).AddResult((List<string>)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryData)GCHandledObjects.GCHandleToObject(instance)).FilterResponse((KeyValuePair<List<string>, bool>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((QueryData)GCHandledObjects.GCHandleToObject(instance)).FilterResults();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((QueryData)GCHandledObjects.GCHandleToObject(instance)).FinishUp((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryData)GCHandledObjects.GCHandleToObject(instance)).Active);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryData)GCHandledObjects.GCHandleToObject(instance)).Section);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryData)GCHandledObjects.GCHandleToObject(instance)).IsQueryComplete());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryData)GCHandledObjects.GCHandleToObject(instance)).ParamsEqual((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), (string[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QueryData)GCHandledObjects.GCHandleToObject(instance)).QueryMatch((VideoSection)(*(int*)args), (string[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]), (string[])GCHandledObjects.GCHandleToPinnedArrayObject(args[2])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((QueryData)GCHandledObjects.GCHandleToObject(instance)).Active = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((QueryData)GCHandledObjects.GCHandleToObject(instance)).Section = (VideoSection)(*(int*)args);
			return -1L;
		}
	}
}
