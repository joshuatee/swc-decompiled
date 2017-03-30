using StaRTS.Externals.EnvironmentManager;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers.Holonet
{
	public static class AudienceConditionUtils
	{
		private const string SQUAD_WAR_PARTICIPATE_CONDITION = "squadWarParticipation";

		private const string PLAYER_IS_IN_WAR = "playerInSquadWar";

		private const string SQUAD_MEMBER_CONDITION = "squadMembers";

		private const string SQUAD_ACTIVE_MEMBER_CONDITION = "squadActiveMembers";

		private const string HAS_COUNTRY_CONDITION = "hasCountry";

		private const string LACKS_COUNTRY_CONDITION = "lacksCountry";

		private const string HAS_LANGUAGE_CONDITION = "hasLanguage";

		private const string LACKS_LANGUAGE_CONDITION = "lacksLanguage";

		private const string PLATFORM_CONDITION = "platform";

		private const string THIRD_PARTY_CONDITION = "thirdParty";

		private const char CONDITION_DELIMITER = '|';

		private const string IOS_PLATFORM = "i";

		private const string ANDROID_PLATFORM = "a";

		private const string AMAZON_PLATFORM = "am";

		private const string FB_GAMEROOM_PLATFORM = "fb";

		private const string ADCOLONY = "adcolony";

		private const string TAPJOY = "tapjoy";

		public static bool IsValidForClient(List<AudienceCondition> audienceConditions)
		{
			Squad currentSquad = Service.Get<SquadController>().StateManager.GetCurrentSquad();
			string playerId = Service.Get<CurrentPlayer>().PlayerId;
			int i = 0;
			int count = audienceConditions.Count;
			while (i < count)
			{
				AudienceCondition audienceCondition = audienceConditions[i];
				string conditionType = audienceCondition.ConditionType;
				switch (conditionType)
				{
				case "playerInSquadWar":
				{
					bool flag = audienceCondition.ConditionValue.ToLower() == "true";
					if (currentSquad != null)
					{
						SquadMember squadMemberById = SquadUtils.GetSquadMemberById(currentSquad, playerId);
						if (squadMemberById != null)
						{
							SquadWarManager warManager = Service.Get<SquadController>().WarManager;
							return warManager.IsSquadMemberInWarOrMatchmaking(squadMemberById) == flag;
						}
					}
					return false;
				}
				case "squadWarParticipation":
				{
					uint num2 = Convert.ToUInt32(audienceCondition.ConditionValue) * 3600u;
					int serverTime = (int)Service.Get<ServerAPI>().ServerTime;
					return (long)serverTime - (long)((ulong)Service.Get<CurrentPlayer>().LastWarParticipationTime) >= (long)((ulong)num2);
				}
				case "squadActiveMembers":
				case "squadMembers":
				{
					string[] array = audienceCondition.ConditionValue.Split(new char[]
					{
						'|'
					});
					if (array == null || array.Length != 2)
					{
						Service.Get<Logger>().Error("Data error for SQUAD_MEMBER_CONDITION");
						return false;
					}
					int num3 = int.Parse(array[0]);
					int num4 = int.Parse(array[1]);
					if (currentSquad == null && num4 == 0)
					{
						return true;
					}
					if (currentSquad != null)
					{
						if (audienceCondition.ConditionType == "squadMembers" && currentSquad.MemberCount >= num3 && currentSquad.MemberCount <= num4)
						{
							return true;
						}
						if (audienceCondition.ConditionType == "squadActiveMembers" && currentSquad.ActiveMemberCount >= num3 && currentSquad.ActiveMemberCount <= num4)
						{
							return true;
						}
					}
					return false;
				}
				case "hasCountry":
				{
					string deviceCountryCode = Service.Get<EnvironmentController>().GetDeviceCountryCode();
					string[] array2 = audienceCondition.ConditionValue.Split(new char[]
					{
						'|'
					});
					if (Array.IndexOf<string>(array2, deviceCountryCode) == -1)
					{
						return false;
					}
					break;
				}
				case "lacksCountry":
				{
					string deviceCountryCode2 = Service.Get<EnvironmentController>().GetDeviceCountryCode();
					string[] array3 = audienceCondition.ConditionValue.Split(new char[]
					{
						'|'
					});
					if (Array.IndexOf<string>(array3, deviceCountryCode2) != -1)
					{
						return false;
					}
					break;
				}
				case "hasLanguage":
				{
					string locale = Service.Get<Lang>().Locale;
					string[] array4 = audienceCondition.ConditionValue.Split(new char[]
					{
						'|'
					});
					if (Array.IndexOf<string>(array4, locale) == -1)
					{
						return false;
					}
					break;
				}
				case "lacksLanguage":
				{
					string locale2 = Service.Get<Lang>().Locale;
					string[] array5 = audienceCondition.ConditionValue.Split(new char[]
					{
						'|'
					});
					if (Array.IndexOf<string>(array5, locale2) != -1)
					{
						return false;
					}
					break;
				}
				case "thirdParty":
				{
					string conditionValue = audienceCondition.ConditionValue;
					return conditionValue == "tapjoy" && Service.Get<TapjoyPlugin>().enabled;
				}
				case "platform":
				{
					string[] array6 = audienceCondition.ConditionValue.Split(new char[]
					{
						'|'
					});
					if (array6 != null)
					{
						string[] array7 = array6;
						for (int j = 0; j < array7.Length; j++)
						{
							string a = array7[j];
							if (a == "a")
							{
								return true;
							}
						}
					}
					return false;
				}
				}
				i++;
			}
			return true;
		}
	}
}
