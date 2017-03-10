using StaRTS.Externals.Maker.MRSS;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet
{
	public class QuerySourceTypes
	{
		private Stack<string> neededSourceTypes;

		private List<string> pendingSourceTypes;

		private int failedResponseCount;

		public bool Active
		{
			get;
			set;
		}

		public QuerySourceTypes()
		{
			this.failedResponseCount = 0;
			this.Active = false;
			this.neededSourceTypes = new Stack<string>();
			this.pendingSourceTypes = new List<string>();
		}

		private bool IsQueryComplete()
		{
			if (this.neededSourceTypes.Count > 0 || this.pendingSourceTypes.Count - this.failedResponseCount > 0)
			{
				return false;
			}
			this.Active = false;
			return true;
		}

		public bool QueryStart()
		{
			if (this.IsQueryComplete())
			{
				this.neededSourceTypes = new Stack<string>(Service.Get<VideoDataManager>().SourceTypes.Keys);
				this.pendingSourceTypes.Clear();
				this.failedResponseCount = 0;
				this.Active = true;
				this.QueryNextSourceType();
				return true;
			}
			return false;
		}

		private void QueryNextSourceType()
		{
			if (!this.IsQueryComplete() && this.neededSourceTypes.Count > 0)
			{
				string text = this.neededSourceTypes.Pop();
				this.pendingSourceTypes.Add(text);
				Service.Get<VideoDataManager>().SearchSubCategory(text, new VideoDataManager.DataListQueryCompleteDelegate(this.OnSourceTypeQueried));
			}
		}

		private void OnSourceTypeQueried(List<string> videoGuidList)
		{
			if (!this.Active)
			{
				return;
			}
			if (videoGuidList == null)
			{
				this.failedResponseCount++;
				this.QueryNextSourceType();
				return;
			}
			string text = null;
			int i = 0;
			int count = this.pendingSourceTypes.Count;
			while (i < count)
			{
				List<string> list = null;
				Service.Get<VideoDataManager>().Tags.TryGetValue(this.pendingSourceTypes[i], out list);
				if (list == videoGuidList)
				{
					text = this.pendingSourceTypes[i];
				}
				i++;
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.pendingSourceTypes.Remove(text);
				if (videoGuidList.Count > 0)
				{
					KeyValuePair<List<string>, string> keyValuePair = new KeyValuePair<List<string>, string>(videoGuidList, text);
					Service.Get<EventManager>().SendEvent(EventId.UIVideosSourceTypeResponse, keyValuePair);
				}
			}
			this.QueryNextSourceType();
		}

		protected internal QuerySourceTypes(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuerySourceTypes)GCHandledObjects.GCHandleToObject(instance)).Active);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuerySourceTypes)GCHandledObjects.GCHandleToObject(instance)).IsQueryComplete());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((QuerySourceTypes)GCHandledObjects.GCHandleToObject(instance)).OnSourceTypeQueried((List<string>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((QuerySourceTypes)GCHandledObjects.GCHandleToObject(instance)).QueryNextSourceType();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuerySourceTypes)GCHandledObjects.GCHandleToObject(instance)).QueryStart());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((QuerySourceTypes)GCHandledObjects.GCHandleToObject(instance)).Active = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
