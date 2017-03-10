using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public class FactionIconUpgradeController : IEventObserver
	{
		private const string FACTION_SPRITE_EMPIRE_PREFIX = "FactionUpEmp";

		private const string FACTION_SPRITE_REBEL_PREFIX = "FactionUpReb";

		private const string FACTION_EMPIRE = "Empire";

		private const string FACTION_REBEL = "Rebel";

		private const int FACTION_COL_LENGTH = 5;

		private const string POSTFIX_DIGETS = "D2";

		private List<IconUpgradeVO> iconUpgradeList;

		public FactionIconUpgradeController()
		{
			Service.Set<FactionIconUpgradeController>(this);
			Service.Get<EventManager>().RegisterObserver(this, EventId.WorldInTransitionComplete, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.HomeStateTransitionComplete, EventPriority.Default);
		}

		public string GetIcon(string faction, int rating)
		{
			FactionType factionType = FactionType.Rebel;
			if (faction.ToLower() == "Empire".ToLower())
			{
				factionType = FactionType.Empire;
			}
			return this.GetIcon(factionType, rating);
		}

		public string GetIcon(FactionType factionType, int rating)
		{
			string result;
			if (this.UseUpgradeImage(rating))
			{
				string iconPostfixFromRating = this.GetIconPostfixFromRating(rating);
				if (factionType == FactionType.Rebel)
				{
					result = "FactionUpReb" + iconPostfixFromRating;
				}
				else
				{
					result = "FactionUpEmp" + iconPostfixFromRating;
				}
			}
			else
			{
				result = UXUtils.GetIconNameFromFactionType(factionType);
			}
			return result;
		}

		public bool UseUpgradeImage(int rating)
		{
			return GameConstants.ENABLE_FACTION_ICON_UPGRADES && this.RatingToLevel(rating) > 0;
		}

		private string GetIconPostfixFromRating(int rating)
		{
			int num = this.RatingToLevel(rating);
			string result = "";
			if (num > 0)
			{
				int num2 = (num - 1) / 5 + 1;
				int num3 = (num - 1) % 5 + 1;
				result = num2.ToString("D2") + num3.ToString("D2");
			}
			return result;
		}

		public int GetCurrentPlayerVictoryRating()
		{
			CurrentPlayer player = Service.Get<CurrentPlayer>();
			return GameUtils.CalculatePlayerVictoryRating(player);
		}

		public int GetTotalVictoryRatingToCurrentLevel()
		{
			List<IconUpgradeVO> sortedFactionIconData = this.GetSortedFactionIconData();
			int count = sortedFactionIconData.Count;
			int result = 0;
			int num = this.RatingToLevel(this.GetCurrentPlayerVictoryRating());
			if (count > 0 && num > 0)
			{
				IconUpgradeVO iconUpgradeVO = sortedFactionIconData[num];
				result = iconUpgradeVO.Rating;
			}
			return result;
		}

		public int GetTotalVictoryRatingToNextLevel()
		{
			List<IconUpgradeVO> sortedFactionIconData = this.GetSortedFactionIconData();
			int count = sortedFactionIconData.Count;
			int result = 0;
			int num = this.RatingToLevel(this.GetCurrentPlayerVictoryRating());
			num++;
			if (num >= count)
			{
				num = count - 1;
			}
			if (count > 0)
			{
				IconUpgradeVO iconUpgradeVO = sortedFactionIconData[num];
				result = iconUpgradeVO.Rating;
			}
			return result;
		}

		private int SortAscending(IconUpgradeVO a, IconUpgradeVO b)
		{
			return a.Level.CompareTo(b.Level);
		}

		private List<IconUpgradeVO> GetSortedFactionIconData()
		{
			if (this.iconUpgradeList == null)
			{
				this.iconUpgradeList = new List<IconUpgradeVO>();
				Dictionary<string, IconUpgradeVO>.ValueCollection all = Service.Get<IDataController>().GetAll<IconUpgradeVO>();
				foreach (IconUpgradeVO current in all)
				{
					this.iconUpgradeList.Add(current);
				}
				this.iconUpgradeList.Sort(new Comparison<IconUpgradeVO>(this.SortAscending));
			}
			return this.iconUpgradeList;
		}

		public int RatingToLevel(int rating)
		{
			List<IconUpgradeVO> sortedFactionIconData = this.GetSortedFactionIconData();
			int count = sortedFactionIconData.Count;
			int result = 0;
			if (count > 0)
			{
				int i = 0;
				IconUpgradeVO iconUpgradeVO = sortedFactionIconData[i];
				result = iconUpgradeVO.Level;
				for (i = 1; i < count; i++)
				{
					iconUpgradeVO = sortedFactionIconData[i];
					if (rating < iconUpgradeVO.Rating)
					{
						break;
					}
					result = iconUpgradeVO.Level;
				}
				if (i == count)
				{
					result = sortedFactionIconData[count - 1].Level;
				}
			}
			return result;
		}

		public int RatingToDisplayLevel(int rating)
		{
			return this.RatingToLevel(rating) + 1;
		}

		public int GetCurrentPlayerLevel()
		{
			return this.RatingToLevel(this.GetCurrentPlayerVictoryRating());
		}

		public int GetCurrentPlayerDisplayLevel()
		{
			return this.RatingToDisplayLevel(this.GetCurrentPlayerVictoryRating());
		}

		public int GetCurrentPlayerDisplayNextLevel()
		{
			return this.GetCurrentPlayerDisplayLevel() + 1;
		}

		public bool ShouldShowIconUpgradeCelebration()
		{
			int pref = Service.Get<SharedPlayerPrefs>().GetPref<int>("fi_bl");
			return GameConstants.ENABLE_FACTION_ICON_UPGRADES && pref < this.GetCurrentPlayerLevel();
		}

		public void ShowIconUpgradeCelebrationScreen()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			string icon = this.GetIcon(currentPlayer.Faction, this.GetCurrentPlayerVictoryRating());
			Service.Get<ScreenController>().AddScreen(new FactionIconCelebScreen(icon), false, QueueScreenBehavior.QueueAndDeferTillClosed);
			this.SaveIconProgress();
		}

		private void SaveIconProgress()
		{
			Service.Get<SharedPlayerPrefs>().SetPref("fi_bl", this.GetCurrentPlayerLevel().ToString());
			Service.Get<EventManager>().SendEvent(EventId.FactionIconUpgraded, null);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if ((id == EventId.HomeStateTransitionComplete || id == EventId.WorldInTransitionComplete) && Service.Get<GameStateMachine>().CurrentState is HomeState && this.ShouldShowIconUpgradeCelebration())
			{
				this.ShowIconUpgradeCelebrationScreen();
			}
			return EatResponse.NotEaten;
		}

		protected internal FactionIconUpgradeController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).GetCurrentPlayerDisplayLevel());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).GetCurrentPlayerDisplayNextLevel());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).GetCurrentPlayerLevel());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).GetCurrentPlayerVictoryRating());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).GetIcon((FactionType)(*(int*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).GetIcon(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).GetIconPostfixFromRating(*(int*)args));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).GetSortedFactionIconData());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).GetTotalVictoryRatingToCurrentLevel());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).GetTotalVictoryRatingToNextLevel());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).RatingToDisplayLevel(*(int*)args));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).RatingToLevel(*(int*)args));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).SaveIconProgress();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).ShouldShowIconUpgradeCelebration());
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).ShowIconUpgradeCelebrationScreen();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).SortAscending((IconUpgradeVO)GCHandledObjects.GCHandleToObject(*args), (IconUpgradeVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FactionIconUpgradeController)GCHandledObjects.GCHandleToObject(instance)).UseUpgradeImage(*(int*)args));
		}
	}
}
