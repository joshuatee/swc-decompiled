using System;

namespace StaRTS.Main.Views.UX.Tags
{
	public class MultiCurrencyTag
	{
		public int Credits
		{
			get;
			private set;
		}

		public int Materials
		{
			get;
			private set;
		}

		public int Contraband
		{
			get;
			private set;
		}

		public int Crystals
		{
			get;
			private set;
		}

		public string PurchaseContext
		{
			get;
			private set;
		}

		public MultiCurrencyTag(int credits, int materials, int contraband, int crystals, string purchaseContext)
		{
			this.Credits = credits;
			this.Materials = materials;
			this.Contraband = contraband;
			this.Crystals = crystals;
			this.PurchaseContext = purchaseContext;
		}
	}
}
