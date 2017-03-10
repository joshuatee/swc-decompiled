using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;

namespace StaRTS.Main.Views.UX.Screens
{
	public class SquadWarInfoScreenWarTab : AbstractSquadWarInfoScreenTab
	{
		private const string TEXTURE = "TextureWar";

		private const string GRID = "GridWar";

		private const string ITEM = "ItemWar";

		private const string ITEM_TEXTURE = "TextureItemWar";

		private const string ITEM_TITLE = "LabelTitleWar";

		private const string ITEM_BODY = "LabelBodyWar";

		public SquadWarInfoScreenWarTab(SquadWarInfoScreen parent, UXCheckbox tabButton, UXElement topGroup) : base(parent, tabButton, topGroup)
		{
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			string text = (faction == FactionType.Rebel) ? GameConstants.WAR_HELP_WAR_REBEL : GameConstants.WAR_HELP_WAR_EMPIRE;
			string[] array = text.Split(new char[]
			{
				'|'
			});
			if (array.Length < 2)
			{
				Service.Get<StaRTSLogger>().WarnFormat("GameConstant [War Help War {0}] is not formatted correctly: {1}", new object[]
				{
					faction,
					text
				});
				return;
			}
			base.PopulateBgTexture(array[0], "TextureWar");
			base.PopulateGrid(array, "GridWar", "ItemWar", "TextureItemWar", "LabelTitleWar", "LabelBodyWar");
		}

		protected internal SquadWarInfoScreenWarTab(UIntPtr dummy) : base(dummy)
		{
		}
	}
}
