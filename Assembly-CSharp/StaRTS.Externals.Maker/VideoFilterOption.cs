using System;

namespace StaRTS.Externals.Maker
{
	public class VideoFilterOption
	{
		public string UILabel
		{
			get;
			private set;
		}

		public string Value
		{
			get;
			private set;
		}

		public int Id
		{
			get;
			private set;
		}

		public VideoFilterOption(string label, string value, int id)
		{
			this.UILabel = label;
			this.Value = value;
			this.Id = id;
		}

		public VideoFilterOption(VideoFilterOption other) : this(other.UILabel, other.Value, other.Id)
		{
		}
	}
}
