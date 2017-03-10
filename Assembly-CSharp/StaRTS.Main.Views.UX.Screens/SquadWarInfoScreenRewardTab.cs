using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;

namespace StaRTS.Main.Views.UX.Screens
{
	public class SquadWarInfoScreenRewardTab : AbstractSquadWarInfoScreenTab
	{
		private const string LABEL = "LabelReward";

		private const string TEXTURE = "TextureReward";

		public SquadWarInfoScreenRewardTab(SquadWarInfoScreen parent, UXCheckbox tabButton, UXElement topGroup) : base(parent, tabButton, topGroup)
		{
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			string text = (faction == FactionType.Rebel) ? GameConstants.WAR_HELP_REWARD_REBEL : GameConstants.WAR_HELP_REWARD_EMPIRE;
			string[] array = text.Split(new char[]
			{
				'|'
			});
			if (array.Length != 2)
			{
				Service.Get<StaRTSLogger>().WarnFormat("GameConstant [War Help Reward {0}] is not formatted correctly: {1}", new object[]
				{
					faction,
					text
				});
				return;
			}
			base.PopulateBgTexture(array[0], "TextureReward");
			parent.GetElement<UXLabel>("LabelReward").Text = Service.Get<Lang>().Get(array[1], new object[0]);
		}

		protected internal SquadWarInfoScreenRewardTab(UIntPtr dummy) : base(dummy)
		{
		}
	}
}
