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
using System.Globalization;
using WinRTBridge;

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
				uint num = <PrivateImplementationDetails>.ComputeStringHash(conditionType);
				if (num <= 1979967659u)
				{
					if (num <= 709505714u)
					{
						if (num != 399293899u)
						{
							if (num == 709505714u)
							{
								if (conditionType == "platform")
								{
									string[] array = audienceCondition.ConditionValue.Split(new char[]
									{
										'|'
									});
									if (array != null)
									{
										string[] array2 = array;
										int num2 = 0;
										if (num2 < array2.Length)
										{
											string text = array2[num2];
											Service.Get<StaRTSLogger>().WarnFormat("Make sure to add #define for missing platfrom {0}", new object[]
											{
												text
											});
											return false;
										}
									}
									return false;
								}
							}
						}
						else if (conditionType == "lacksLanguage")
						{
							string locale = Service.Get<Lang>().Locale;
							string[] array3 = audienceCondition.ConditionValue.Split(new char[]
							{
								'|'
							});
							if (Array.IndexOf<string>(array3, locale) != -1)
							{
								return false;
							}
						}
					}
					else if (num != 949377194u)
					{
						if (num != 1252504053u)
						{
							if (num == 1979967659u)
							{
								if (conditionType == "hasCountry")
								{
									string deviceCountryCode = Service.Get<EnvironmentController>().GetDeviceCountryCode();
									string[] array4 = audienceCondition.ConditionValue.Split(new char[]
									{
										'|'
									});
									if (Array.IndexOf<string>(array4, deviceCountryCode) == -1)
									{
										return false;
									}
								}
							}
						}
						else if (conditionType == "hasLanguage")
						{
							string locale2 = Service.Get<Lang>().Locale;
							string[] array5 = audienceCondition.ConditionValue.Split(new char[]
							{
								'|'
							});
							if (Array.IndexOf<string>(array5, locale2) == -1)
							{
								return false;
							}
						}
					}
					else if (conditionType == "squadWarParticipation")
					{
						uint num3 = Convert.ToUInt32(audienceCondition.ConditionValue, CultureInfo.InvariantCulture) * 3600u;
						int serverTime = (int)Service.Get<ServerAPI>().ServerTime;
						return (long)serverTime - (long)((ulong)Service.Get<CurrentPlayer>().LastWarParticipationTime) >= (long)((ulong)num3);
					}
				}
				else if (num <= 2708772247u)
				{
					if (num != 2704192874u)
					{
						if (num == 2708772247u)
						{
							if (conditionType == "playerInSquadWar")
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
						}
					}
					else if (conditionType == "thirdParty")
					{
						string conditionValue = audienceCondition.ConditionValue;
						return conditionValue == "tapjoy" && Service.Get<TapjoyPlugin>().enabled;
					}
				}
				else if (num != 2846430817u)
				{
					if (num != 3759229686u)
					{
						if (num != 4133241044u)
						{
							goto IL_436;
						}
						if (!(conditionType == "squadMembers"))
						{
							goto IL_436;
						}
					}
					else if (!(conditionType == "squadActiveMembers"))
					{
						goto IL_436;
					}
					string[] array6 = audienceCondition.ConditionValue.Split(new char[]
					{
						'|'
					});
					if (array6 == null || array6.Length != 2)
					{
						Service.Get<StaRTSLogger>().Error("Data error for SQUAD_MEMBER_CONDITION");
						return false;
					}
					int num4 = int.Parse(array6[0]);
					int num5 = int.Parse(array6[1]);
					if (currentSquad == null && num5 == 0)
					{
						return true;
					}
					if (currentSquad != null)
					{
						if (audienceCondition.ConditionType == "squadMembers" && currentSquad.MemberCount >= num4 && currentSquad.MemberCount <= num5)
						{
							return true;
						}
						if (audienceCondition.ConditionType == "squadActiveMembers" && currentSquad.ActiveMemberCount >= num4 && currentSquad.ActiveMemberCount <= num5)
						{
							return true;
						}
					}
					return false;
				}
				else if (conditionType == "lacksCountry")
				{
					string deviceCountryCode2 = Service.Get<EnvironmentController>().GetDeviceCountryCode();
					string[] array7 = audienceCondition.ConditionValue.Split(new char[]
					{
						'|'
					});
					if (Array.IndexOf<string>(array7, deviceCountryCode2) != -1)
					{
						return false;
					}
				}
				IL_436:
				i++;
			}
			return true;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AudienceConditionUtils.IsValidForClient((List<AudienceCondition>)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
