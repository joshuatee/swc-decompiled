using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Externals.Maker
{
	public class VideosFilterChoice
	{
		private string label;

		private ChoiceType uiType;

		public int Id
		{
			get;
			private set;
		}

		public FilterType ValueType
		{
			get;
			private set;
		}

		public List<VideoFilterOption> Options
		{
			get;
			private set;
		}

		public VideosFilterChoice(string label, int id, List<VideoFilterOption> options, ChoiceType uiType, FilterType valueType)
		{
			this.label = label;
			this.Id = id;
			this.Options = options;
			this.uiType = uiType;
			this.ValueType = valueType;
			this.SetupFilters();
		}

		private ChoiceData GetChoiceData()
		{
			List<VideoFilterOption> list = new List<VideoFilterOption>();
			for (int i = 0; i < this.Options.Count; i++)
			{
				list.Add(new VideoFilterOption(this.Options[i]));
			}
			ChoiceData result = new ChoiceData(this.Options[0], list, this.Options[0], this.label, this.Id, this.uiType);
			return result;
		}

		public int GetChoiceIdFromLabel(string label)
		{
			for (int i = 0; i < this.Options.Count; i++)
			{
				if (this.Options[i].UILabel == label)
				{
					return this.Options[i].Id;
				}
			}
			return -1;
		}

		public string GetNextChoice(int choiceId, bool reverse, bool loop = false)
		{
			bool flag = false;
			int num = (!reverse) ? 0 : (this.Options.Count - 1);
			int num2 = 0;
			VideoFilterOption videoFilterOption;
			while (true)
			{
				videoFilterOption = this.Options[num];
				if (flag)
				{
					break;
				}
				if (videoFilterOption.Id == choiceId)
				{
					flag = true;
				}
				num2++;
				num = ((!reverse) ? (num + 1) : (num - 1));
				if (loop)
				{
					if (num < 0)
					{
						num = this.Options.Count - 1;
					}
					else if (num >= this.Options.Count)
					{
						num = 0;
					}
				}
				else if (num < 0 || num >= this.Options.Count || num2 > this.Options.Count)
				{
					goto IL_BD;
				}
			}
			return videoFilterOption.UILabel;
			IL_BD:
			return string.Empty;
		}

		public void SetupFilters()
		{
			ChoiceData choiceData = this.GetChoiceData();
			Service.Get<EventManager>().SendEvent(EventId.UIVideosQueryFilter, choiceData);
		}
	}
}
