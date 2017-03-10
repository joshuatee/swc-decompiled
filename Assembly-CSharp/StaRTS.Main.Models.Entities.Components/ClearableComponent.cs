using Net.RichardLord.Ash.Core;
using System;

namespace StaRTS.Main.Models.Entities.Components
{
	public class ClearableComponent : ComponentBase
	{
		public ClearableComponent()
		{
		}

		protected internal ClearableComponent(UIntPtr dummy) : base(dummy)
		{
		}
	}
}
