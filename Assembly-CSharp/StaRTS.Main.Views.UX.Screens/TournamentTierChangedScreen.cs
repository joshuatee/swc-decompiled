using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Commands.Tournament;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class TournamentTierChangedScreen : ScreenBase
	{
		private TournamentRank playerRank;

		private const string LABEL_TITLE = "DialogTournamentsTitle";

		private const string LABEL_NEW_TIER = "LabelYourTier";

		private const string LABEL_TIER = "LabelTierLevel";

		private const string ICON_TIER = "SpriteTierIcon";

		private const float ANIMATION_TIME = 3f;

		private const string ANIMATION_TRIGGER = "Show";

		public TournamentTierChangedScreen(TournamentRank rank) : base("gui_tournaments_celebration")
		{
			this.playerRank = rank;
		}

		protected override void OnScreenLoaded()
		{
			Animator component = base.Root.GetComponent<Animator>();
			if (component != null)
			{
				component.SetTrigger("Show");
			}
			this.InitLabels();
			TournamentTierVO tierVO = Service.Get<IDataController>().Get<TournamentTierVO>(this.playerRank.TierUid);
			base.GetElement<UXSprite>("SpriteTierIcon").SpriteName = Service.Get<TournamentController>().GetTierIconName(tierVO);
			Service.Get<ViewTimerManager>().CreateViewTimer(3f, false, new TimerDelegate(this.OnAnimationFinishedTimer), null);
		}

		private void InitLabels()
		{
			base.GetElement<UXLabel>("DialogTournamentsTitle").Text = this.lang.Get("TOURNAMENT_TIER_CHANGE_TITLE", new object[0]);
			base.GetElement<UXLabel>("LabelYourTier").Text = this.lang.Get("TOURNAMENT_TIER_NEW", new object[0]);
			TournamentTierVO tournamentTierVO = Service.Get<IDataController>().Get<TournamentTierVO>(this.playerRank.TierUid);
			base.GetElement<UXLabel>("LabelTierLevel").Text = this.lang.Get("CONFLICT_TIER_CHANGE_PERCENTILE", new object[]
			{
				this.lang.Get(tournamentTierVO.RankName, new object[0]),
				this.lang.Get(tournamentTierVO.DivisionSmall, new object[0]),
				Math.Round(this.playerRank.Percentile, 2)
			});
		}

		private void OnAnimationFinishedTimer(uint id, object cookie)
		{
			this.Close(null);
		}

		protected internal TournamentTierChangedScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TournamentTierChangedScreen)GCHandledObjects.GCHandleToObject(instance)).InitLabels();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TournamentTierChangedScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}
	}
}
