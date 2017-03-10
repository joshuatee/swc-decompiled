using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Configs;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Objectives;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class LoginResponse : AbstractResponse
	{
		public override ISerializable FromObject(object obj)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			if (dictionary.ContainsKey("triggers"))
			{
				Service.Get<QuestController>().SavePendingTriggers((List<object>)dictionary["triggers"]);
			}
			currentPlayer.FirstTimePlayer = (bool)dictionary["firstTimePlayer"];
			if (dictionary.ContainsKey("created"))
			{
				Service.Get<CurrentPlayer>().FirstLoginTime = Convert.ToUInt32(dictionary["created"] as string, CultureInfo.InvariantCulture);
			}
			string prefsString = dictionary["clientPrefs"] as string;
			ServerPlayerPrefs serverPlayerPrefs = new ServerPlayerPrefs(prefsString);
			if (dictionary.ContainsKey("scalars"))
			{
				Dictionary<string, object> dictionary2 = dictionary["scalars"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					if (dictionary2.ContainsKey("attackRating"))
					{
						currentPlayer.AttackRating = Convert.ToInt32(dictionary2["attackRating"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("defenseRating"))
					{
						currentPlayer.DefenseRating = Convert.ToInt32(dictionary2["defenseRating"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("attacksWon"))
					{
						currentPlayer.AttacksWon = Convert.ToInt32(dictionary2["attacksWon"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("defensesWon"))
					{
						currentPlayer.DefensesWon = Convert.ToInt32(dictionary2["defensesWon"], CultureInfo.InvariantCulture);
					}
				}
			}
			if (dictionary.ContainsKey("sharedPrefs"))
			{
				Service.Get<SharedPlayerPrefs>().Populate(dictionary["sharedPrefs"] as Dictionary<string, object>);
			}
			if (dictionary.ContainsKey("name"))
			{
				currentPlayer.PlayerName = (dictionary["name"] as string);
			}
			if (dictionary.ContainsKey("currentlyDefending"))
			{
				Dictionary<string, object> dictionary3 = dictionary["currentlyDefending"] as Dictionary<string, object>;
				if (dictionary3 == null)
				{
					currentPlayer.CurrentlyDefending = false;
				}
				else
				{
					currentPlayer.CurrentlyDefendingExpireTime = Convert.ToUInt32(dictionary3["expiration"], CultureInfo.InvariantCulture);
					if (currentPlayer.CurrentlyDefendingExpireTime > ServerTime.Time)
					{
						currentPlayer.CurrentlyDefending = true;
					}
					else
					{
						currentPlayer.CurrentlyDefending = false;
					}
				}
			}
			else
			{
				currentPlayer.CurrentlyDefending = false;
			}
			if (dictionary.ContainsKey("playerModel"))
			{
				Dictionary<string, object> dictionary4 = dictionary["playerModel"] as Dictionary<string, object>;
				FactionType faction = StringUtils.ParseEnum<FactionType>(dictionary4["faction"].ToString());
				currentPlayer.Faction = faction;
				currentPlayer.Map.FromObject(dictionary4["map"]);
				currentPlayer.BattleHistory.FromObject(dictionary4["battleLogs"]);
				if (dictionary4.ContainsKey("currentQuest"))
				{
					currentPlayer.RestoredQuest = (dictionary4["currentQuest"] as string);
				}
				currentPlayer.SpecOpIntros = new List<string>();
				if (dictionary4.ContainsKey("intros"))
				{
					List<object> list = dictionary4["intros"] as List<object>;
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						currentPlayer.SpecOpIntros.Add(list[i] as string);
						i++;
					}
				}
				if (dictionary4.ContainsKey("protectedUntil"))
				{
					currentPlayer.ProtectedUntil = Convert.ToUInt32(dictionary4["protectedUntil"], CultureInfo.InvariantCulture);
				}
				if (dictionary4.ContainsKey("protectionFrom"))
				{
					currentPlayer.ProtectionFrom = Convert.ToUInt32(dictionary4["protectionFrom"], CultureInfo.InvariantCulture);
				}
				if (dictionary4.ContainsKey("protectionCooldownUntil"))
				{
					Dictionary<string, object> dictionary5 = dictionary4["protectionCooldownUntil"] as Dictionary<string, object>;
					if (dictionary5 != null)
					{
						foreach (KeyValuePair<string, object> current in dictionary5)
						{
							currentPlayer.AddProtectionCooldownUntil(current.get_Key(), Convert.ToUInt32(current.get_Value(), CultureInfo.InvariantCulture));
						}
					}
				}
				Dictionary<string, object> dictionary6 = dictionary4["inventory"] as Dictionary<string, object>;
				if (dictionary6 != null && dictionary6.ContainsKey("capacity") && dictionary6["capacity"] != null)
				{
					currentPlayer.Inventory.FromObject(dictionary4["inventory"]);
				}
				if (dictionary4.ContainsKey("prizes"))
				{
					currentPlayer.Prizes.FromObject(dictionary4["prizes"]);
				}
				currentPlayer.CampaignProgress.FromObject(dictionary4);
				currentPlayer.TournamentProgress.FromObject(dictionary4);
				SquadStateManager stateManager = Service.Get<SquadController>().StateManager;
				if (dictionary4.ContainsKey("guildInfo"))
				{
					Dictionary<string, object> dictionary7 = dictionary4["guildInfo"] as Dictionary<string, object>;
					if (dictionary7 != null && dictionary7.ContainsKey("guildId"))
					{
						string squadID = Convert.ToString(dictionary7["guildId"], CultureInfo.InvariantCulture);
						currentPlayer.Squad = Service.Get<LeaderboardController>().GetOrCreateSquad(squadID);
						currentPlayer.Squad.FromLoginObject(dictionary7);
						if (dictionary7.ContainsKey("joinDate"))
						{
							stateManager.JoinDate = Convert.ToUInt32(dictionary7["joinDate"], CultureInfo.InvariantCulture);
						}
					}
				}
				if (dictionary.ContainsKey("lastTroopRequestTime"))
				{
					stateManager.TroopRequestDate = Convert.ToUInt32(dictionary["lastTroopRequestTime"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("lastWarTroopRequestTime"))
				{
					stateManager.WarTroopRequestDate = Convert.ToUInt32(dictionary["lastWarTroopRequestTime"], CultureInfo.InvariantCulture);
				}
				if (dictionary4.ContainsKey("donatedTroops"))
				{
					stateManager.Troops = SquadUtils.GetSquadDonatedTroopsFromObject(dictionary4["donatedTroops"]);
				}
				if (dictionary4.ContainsKey("deviceInfo"))
				{
					Dictionary<string, object> dictionary8 = dictionary4["deviceInfo"] as Dictionary<string, object>;
					if (dictionary8 != null && dictionary8.Count > 0)
					{
						PlayerSettings.SetNotificationsLevel(100);
					}
				}
				if (dictionary4.ContainsKey("contracts"))
				{
					Service.Get<ISupportController>().UpdateCurrentContractsListFromServer(dictionary4["contracts"]);
				}
				if (dictionary4.ContainsKey("unlockedPlanets"))
				{
					currentPlayer.UpdateUnlockedPlanetsFromServer(dictionary4["unlockedPlanets"]);
				}
				if (dictionary4.ContainsKey("playerObjectives"))
				{
					currentPlayer.Objectives.Clear();
					Dictionary<string, object> dictionary9 = dictionary4["playerObjectives"] as Dictionary<string, object>;
					if (dictionary9 != null)
					{
						foreach (KeyValuePair<string, object> current2 in dictionary9)
						{
							currentPlayer.Objectives.Add(current2.get_Key(), new ObjectiveGroup(current2.get_Key()).FromObject(current2.get_Value()) as ObjectiveGroup);
						}
					}
				}
				if (dictionary4.ContainsKey("perksInfo"))
				{
					currentPlayer.UpdatePerksInfo(dictionary4["perksInfo"]);
				}
				if (dictionary4.ContainsKey("activeArmory"))
				{
					currentPlayer.UpdateActiveArmory(dictionary4["activeArmory"]);
				}
				if (dictionary4.ContainsKey("armoryInfo"))
				{
					currentPlayer.UpdateArmoryInfo(dictionary4["armoryInfo"]);
				}
				if (dictionary4.ContainsKey("shards"))
				{
					currentPlayer.UpdateShardsInfo(dictionary4["shards"]);
				}
				if (dictionary4.ContainsKey("troopDonationProgress"))
				{
					currentPlayer.SetTroopDonationProgress(dictionary4["troopDonationProgress"]);
				}
				if (dictionary4.ContainsKey("raids"))
				{
					currentPlayer.UpdateCurrentRaid(dictionary4["raids"]);
				}
				if (dictionary4.ContainsKey("holonetRewards"))
				{
					currentPlayer.UpdateHolonetRewardsFromServer(dictionary4["holonetRewards"]);
				}
				if (dictionary4.ContainsKey("relocationStarCount"))
				{
					currentPlayer.SetRelocationStartsCount(Convert.ToInt32(dictionary4["relocationStarCount"], CultureInfo.InvariantCulture));
				}
				if (dictionary4.ContainsKey("frozenBuildings"))
				{
					Service.Get<ISupportController>().UpdateFrozenBuildingsListFromServer(dictionary4["frozenBuildings"]);
				}
				currentPlayer.DamagedBuildings = new Dictionary<string, int>();
				Dictionary<string, object> dictionary10 = dictionary4["DamagedBuildings"] as Dictionary<string, object>;
				if (dictionary10 != null)
				{
					foreach (KeyValuePair<string, object> current3 in dictionary10)
					{
						currentPlayer.DamagedBuildings.Add(current3.get_Key(), Convert.ToInt32(current3.get_Value(), CultureInfo.InvariantCulture));
					}
				}
				if (dictionary4.ContainsKey("upgrades"))
				{
					currentPlayer.UnlockedLevels.FromObject(dictionary4["upgrades"]);
				}
				if (dictionary4.ContainsKey("isConnectedAccount"))
				{
					currentPlayer.IsConnectedAccount = (bool)dictionary4["isConnectedAccount"];
				}
				if (dictionary4.ContainsKey("isRateIncentivized"))
				{
					currentPlayer.IsRateIncentivized = (bool)dictionary4["isRateIncentivized"];
				}
				if (dictionary4.ContainsKey("pushRewarded"))
				{
					currentPlayer.IsPushIncentivized = (bool)dictionary4["pushRewarded"];
				}
				if (dictionary4.ContainsKey("lastWarParticipationTime"))
				{
					currentPlayer.LastWarParticipationTime = Convert.ToUInt32(dictionary4["lastWarParticipationTime"], CultureInfo.InvariantCulture);
				}
				if (dictionary4.ContainsKey("identitySwitchTimes"))
				{
					Dictionary<string, object> dictionary11 = dictionary4["identitySwitchTimes"] as Dictionary<string, object>;
					if (dictionary11 != null)
					{
						currentPlayer.NumIdentities = dictionary11.Count;
					}
				}
				if (dictionary4.ContainsKey("openOffer"))
				{
					Dictionary<string, object> dictionary12 = dictionary4["openOffer"] as Dictionary<string, object>;
					if (dictionary12 != null && dictionary12.ContainsKey("offerUid"))
					{
						currentPlayer.OfferId = Convert.ToString(dictionary12["offerUid"], CultureInfo.InvariantCulture);
						currentPlayer.TriggerDate = Convert.ToUInt32(dictionary12["triggerDate"], CultureInfo.InvariantCulture);
					}
				}
			}
			if (dictionary.ContainsKey("liveness"))
			{
				Dictionary<string, object> dictionary13 = dictionary["liveness"] as Dictionary<string, object>;
				if (dictionary13 != null)
				{
					if (dictionary13.ContainsKey("lastLoginTime"))
					{
						string pref = serverPlayerPrefs.GetPref(ServerPref.LastLoginTime);
						uint loginTime = Convert.ToUInt32(dictionary13["lastLoginTime"], CultureInfo.InvariantCulture);
						if (!string.IsNullOrEmpty(pref))
						{
							currentPlayer.LastLoginTime = Convert.ToUInt32(pref, CultureInfo.InvariantCulture);
						}
						currentPlayer.LoginTime = loginTime;
						serverPlayerPrefs.SetPref(ServerPref.LastLoginTime, loginTime.ToString());
					}
					if (dictionary13.ContainsKey("sessionCountToday"))
					{
						int sessionCountToday = Convert.ToInt32(dictionary13["sessionCountToday"], CultureInfo.InvariantCulture);
						currentPlayer.SessionCountToday = sessionCountToday;
					}
					if (dictionary13.ContainsKey("installDate"))
					{
						currentPlayer.InstallDate = Convert.ToUInt32(dictionary13["installDate"], CultureInfo.InvariantCulture);
					}
				}
			}
			return this;
		}

		public LoginResponse()
		{
		}

		protected internal LoginResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LoginResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
