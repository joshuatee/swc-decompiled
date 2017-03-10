using StaRTS.Main.Configs;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Player
{
	public class ServerPlayerPrefs
	{
		private const char CLIENT_PREFS_DELIMITER = ',';

		private string[] prefs;

		private static readonly ServerPref[] PREFS_ACROSS_ALL_ACCOUNTS = new ServerPref[]
		{
			ServerPref.Locale,
			ServerPref.NewspaperArticlesViewed,
			ServerPref.NumRateAppViewed,
			ServerPref.RatedApp,
			ServerPref.LastPaymentTime,
			ServerPref.BattlesAdCount,
			ServerPref.SquadIntroViewed,
			ServerPref.FactionFlippingViewed,
			ServerPref.FactionFlippingSkipConfirmation,
			ServerPref.FactionFlipped,
			ServerPref.LastChatViewTime
		};

		private const string PREF_SERVER_PLAYER_PREFS = "serverPlayerPrefs";

		public ServerPlayerPrefs(string prefsString)
		{
			Service.Set<ServerPlayerPrefs>(this);
			this.prefs = new string[29];
			string[] inputPrefs;
			if (!string.IsNullOrEmpty(prefsString))
			{
				inputPrefs = prefsString.Split(new char[]
				{
					','
				});
			}
			else
			{
				inputPrefs = new string[0];
			}
			this.ParsePref(inputPrefs, ServerPref.Locale, Service.Get<Lang>().Locale);
			this.ParsePref(inputPrefs, ServerPref.AgeGate, "0");
			this.ParsePref(inputPrefs, ServerPref.LoginPopups, "0");
			this.ParsePref(inputPrefs, ServerPref.LastLoginTime, "0");
			this.ParsePref(inputPrefs, ServerPref.NewspaperArticlesViewed, "");
			this.SetPref(ServerPref.LastTroopRequestTime, "0");
			this.ParsePref(inputPrefs, ServerPref.NumRateAppViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.RatedApp, "0");
			this.ParsePref(inputPrefs, ServerPref.NumInventoryItemsNotViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.NumInventoryCratesNotViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.NumInventoryTroopsNotViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.NumInventoryCurrencyNotViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.ChapterMissionViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.SpecopsMissionViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.TournamentViewed, "");
			this.ParsePref(inputPrefs, ServerPref.LastPaymentTime, "0");
			this.ParsePref(inputPrefs, ServerPref.BattlesAdCount, "0");
			this.ParsePref(inputPrefs, ServerPref.SquadIntroViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.TournamentTierChangeTimeViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.FactionFlippingViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.FactionFlippingSkipConfirmation, "0");
			this.ParsePref(inputPrefs, ServerPref.FactionFlipped, "0");
			this.ParsePref(inputPrefs, ServerPref.LastChatViewTime, "0");
			this.ParsePref(inputPrefs, ServerPref.LastPushAuthPromptSquadJoinedTime, "0");
			this.ParsePref(inputPrefs, ServerPref.LastPushAuthPromptTroopRequestTime, "0");
			this.ParsePref(inputPrefs, ServerPref.PushAuthPromptedCount, "0");
			this.ParsePref(inputPrefs, ServerPref.PlanetsTutorialViewed, "0");
			this.ParsePref(inputPrefs, ServerPref.FirstRelocationPlanet, "0");
			this.HandleLocalPrefOverride();
		}

		private void ParsePref(string[] inputPrefs, ServerPref pref, string defaultValue)
		{
			if ((ServerPref)inputPrefs.Length > pref && !string.IsNullOrEmpty(inputPrefs[(int)pref]))
			{
				this.SetPref(pref, inputPrefs[(int)pref]);
				return;
			}
			this.SetPref(pref, defaultValue);
		}

		public string GetPref(ServerPref pref)
		{
			return this.prefs[(int)pref];
		}

		public void SetPref(ServerPref pref, string newValue)
		{
			this.prefs[(int)pref] = newValue;
			if (pref == ServerPref.Locale)
			{
				PlayerSettings.SetLocaleCopy(newValue);
			}
		}

		public string Serialize()
		{
			string text = "";
			for (int i = 0; i < this.prefs.Length; i++)
			{
				text = text + this.prefs[i] + ",";
			}
			return text;
		}

		public void SavePrefsLocally()
		{
			string value = this.Serialize();
			PlayerPrefs.SetString("serverPlayerPrefs", value);
		}

		private void HandleLocalPrefOverride()
		{
			if (PlayerPrefs.HasKey("serverPlayerPrefs"))
			{
				string @string = PlayerPrefs.GetString("serverPlayerPrefs");
				string[] array = @string.Split(new char[]
				{
					','
				});
				for (int i = 0; i < ServerPlayerPrefs.PREFS_ACROSS_ALL_ACCOUNTS.Length; i++)
				{
					ServerPref serverPref = ServerPlayerPrefs.PREFS_ACROSS_ALL_ACCOUNTS[i];
					string newValue = array[(int)serverPref];
					this.SetPref(serverPref, newValue);
				}
				PlayerPrefs.DeleteKey("serverPlayerPrefs");
			}
		}

		protected internal ServerPlayerPrefs(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerPlayerPrefs)GCHandledObjects.GCHandleToObject(instance)).GetPref((ServerPref)(*(int*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ServerPlayerPrefs)GCHandledObjects.GCHandleToObject(instance)).HandleLocalPrefOverride();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ServerPlayerPrefs)GCHandledObjects.GCHandleToObject(instance)).ParsePref((string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), (ServerPref)(*(int*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ServerPlayerPrefs)GCHandledObjects.GCHandleToObject(instance)).SavePrefsLocally();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerPlayerPrefs)GCHandledObjects.GCHandleToObject(instance)).Serialize());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ServerPlayerPrefs)GCHandledObjects.GCHandleToObject(instance)).SetPref((ServerPref)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}
	}
}
