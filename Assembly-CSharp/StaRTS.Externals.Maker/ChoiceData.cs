using System;
using System.Collections.Generic;

namespace StaRTS.Externals.Maker
{
	public struct ChoiceData
	{
		public VideoFilterOption ChoiceDefault;

		public List<VideoFilterOption> Choices;

		public VideoFilterOption ChoiceMade;

		public string UILabel;

		public int Id;

		public ChoiceType UIType;

		public ChoiceData(VideoFilterOption defaultChoice, List<VideoFilterOption> choices, VideoFilterOption choice, string label, int id, ChoiceType uiType)
		{
			this.ChoiceDefault = defaultChoice;
			this.Choices = choices;
			this.ChoiceMade = choice;
			this.UILabel = label;
			this.Id = id;
			this.UIType = uiType;
		}
	}
}
