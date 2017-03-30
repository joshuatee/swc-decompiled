using System;
using System.Collections.Generic;

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
			if (response.Key == null)
			{
				this.FinishUp(null);
				return false;
			}
			if (this.intersection.Key == null)
			{
				this.intersection = new KeyValuePair<List<string>, bool>(new List<string>(response.Key), response.Value);
			}
			else
			{
				List<string> key = this.intersection.Key;
				List<string> key2 = response.Key;
				if (response.Value)
				{
					key = response.Key;
					key2 = this.intersection.Key;
				}
				bool value = this.intersection.Value || response.Value;
				KeyValuePair<List<string>, bool> keyValuePair = new KeyValuePair<List<string>, bool>(new List<string>(), value);
				int i = 0;
				int count = key.Count;
				while (i < count)
				{
					if (key2.Contains(key[i]))
					{
						keyValuePair.Key.Add(key[i]);
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
			this.FinishUp(this.intersection.Key);
		}
	}
}
