using Net.RichardLord.Ash.Core;
using System;

namespace StaRTS.Main.Models.Entities.Components
{
	public class GeneratorComponent : ComponentBase, IResourceFillable
	{
		public float CurrentFullnessPercentage
		{
			get;
			set;
		}

		public float PreviousFullnessPercentage
		{
			get;
			set;
		}
	}
}
