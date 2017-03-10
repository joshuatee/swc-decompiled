using StaRTS.Audio;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class SquadWarEndCelebrationScreen : ClosableScreen
	{
		private const string LABEL_TITLE = "LabelTitlePrimarySquadWarEndCeleb";

		private const string LABEL_SECONDARY_MESSAGE = "LabelTitleSecondarySquadWarEndCeleb";

		private const string LABEL_REWARD_MESSAGE = "LabelRewardSquadWarEndCeleb";

		private const string BUTTON_COLLECT = "BtnCollectSquadWarEndCeleb";

		private const string LABEL_COLLECT = "LabelCollectSquadWarEndCeleb";

		private const string SPRITE_RESULT_WIN_LOSS = "TextureFactionIconSquadWarEndCeleb";

		private const string SPRITE_RESULT_DRAW_REBEL = "TextureDrawIconRebelSquadWarEndCeleb";

		private const string SPRITE_RESULT_DRAW_EMPIRE = "TextureDrawIconEmpireSquadWarEndCeleb";

		private const string TEXTURE_REWARD_EMPIRE = "squadwars_end_celeb_empire";

		private const string TEXTURE_REWARD_REBEL = "squadwars_end_celeb_rebel";

		private const string LANG_TITLE = "WAR_END_RESULTS_TITLE";

		private const string LANG_REBELS_WIN = "WAR_END_RESULTS_REBELS_WIN";

		private const string LANG_EMPIRE_WIN = "WAR_END_RESULTS_EMPIRE_WIN";

		private const string LANG_REBELS_LOSS = "WAR_END_RESULTS_REBELS_LOSS";

		private const string LANG_EMPIRE_LOSS = "WAR_END_RESULTS_EMPIRE_LOSS";

		private const string LANG_DRAW = "WAR_END_RESULTS_DRAW";

		private const string LANG_LOSS_REWARD = "WAR_END_RESULTS_LOSS_REWARD";

		private const string LANG_DRAW_REWARD = "WAR_END_RESULTS_DRAW_REWARD";

		private const string LANG_WIN_CTA = "WAR_END_RESULTS_WIN_CTA";

		private const string LANG_LOSS_CTA = "WAR_END_RESULTS_LOSS_CTA";

		private const string LANG_DRAW_CTA = "WAR_END_RESULTS_DRAW_CTA";

		private const string ANIM_WIN_EMPIRE = "ShowWinEmpire";

		private const string ANIM_WIN_REBEL = "ShowWinRebel";

		private const string ANIM_LOSS = "ShowLoss";

		private const string ANIM_DRAW = "ShowDraw";

		private const string ANIM_CLOSE = "Close";

		private const string ANIM_DRAW_SF = "ShowDrawSF";

		private const int RESULT_LOSS = -1;

		private const int RESULT_DRAW = 0;

		private const int RESULT_WIN = 1;

		private int squadWarResult;

		private FactionType playerFaction;

		private string playerSquadName;

		private bool sameFaction;

		protected override bool WantTransitions
		{
			get
			{
				return false;
			}
		}

		public SquadWarEndCelebrationScreen(int squadWarResult, FactionType playerFaction, string playerSquadName, bool sameFaction) : base("gui_squadwar_end_celebration")
		{
			this.squadWarResult = squadWarResult;
			this.playerFaction = playerFaction;
			this.playerSquadName = playerSquadName;
			this.sameFaction = sameFaction;
		}

		protected override void OnScreenLoaded()
		{
			base.InitAnimator();
			Lang lang = Service.Get<Lang>();
			UXLabel element = base.GetElement<UXLabel>("LabelTitlePrimarySquadWarEndCeleb");
			UXLabel element2 = base.GetElement<UXLabel>("LabelTitleSecondarySquadWarEndCeleb");
			UXLabel element3 = base.GetElement<UXLabel>("LabelRewardSquadWarEndCeleb");
			UXLabel element4 = base.GetElement<UXLabel>("LabelCollectSquadWarEndCeleb");
			UXTexture element5 = base.GetElement<UXTexture>("TextureFactionIconSquadWarEndCeleb");
			element.Text = lang.Get("WAR_END_RESULTS_TITLE", new object[0]);
			element5.LoadTexture((this.playerFaction == FactionType.Empire) ? "squadwars_end_celeb_empire" : "squadwars_end_celeb_rebel");
			string id = "sfx_stinger_war_end_draw";
			if (this.squadWarResult == 1)
			{
				id = ((this.playerFaction == FactionType.Empire) ? "sfx_stinger_war_end_win_empire" : "sfx_stinger_war_end_win_rebel");
				element2.Text = lang.Get((this.playerFaction == FactionType.Empire) ? "WAR_END_RESULTS_EMPIRE_WIN" : "WAR_END_RESULTS_REBELS_WIN", new object[]
				{
					this.playerSquadName
				});
				element3.Visible = false;
				element4.Text = lang.Get("WAR_END_RESULTS_WIN_CTA", new object[0]);
				this.ResetAllTriggers();
				this.animator.SetTrigger((this.playerFaction == FactionType.Empire) ? "ShowWinEmpire" : "ShowWinRebel");
			}
			else if (this.squadWarResult == -1)
			{
				id = "sfx_stinger_war_end_loss";
				element2.Text = lang.Get((this.playerFaction == FactionType.Empire) ? "WAR_END_RESULTS_EMPIRE_LOSS" : "WAR_END_RESULTS_REBELS_LOSS", new object[0]);
				element3.Text = lang.Get("WAR_END_RESULTS_LOSS_REWARD", new object[0]);
				element4.Text = lang.Get("WAR_END_RESULTS_LOSS_CTA", new object[0]);
				this.ResetAllTriggers();
				this.animator.SetTrigger("ShowLoss");
			}
			else if (this.squadWarResult == 0)
			{
				element2.Text = lang.Get("WAR_END_RESULTS_DRAW", new object[0]);
				element3.Text = lang.Get("WAR_END_RESULTS_DRAW_REWARD", new object[0]);
				element4.Text = lang.Get("WAR_END_RESULTS_DRAW_CTA", new object[0]);
				base.GetElement<UXTexture>("TextureDrawIconEmpireSquadWarEndCeleb").LoadTexture("squadwars_end_celeb_empire");
				base.GetElement<UXTexture>("TextureDrawIconRebelSquadWarEndCeleb").LoadTexture("squadwars_end_celeb_rebel");
				this.ResetAllTriggers();
				if (this.sameFaction)
				{
					this.animator.SetTrigger("ShowDrawSF");
				}
				else
				{
					this.animator.SetTrigger("ShowDraw");
				}
			}
			Service.Get<AudioManager>().PlayAudio(id);
			UXButton element6 = base.GetElement<UXButton>("BtnCollectSquadWarEndCeleb");
			element6.OnClicked = new UXButtonClickedDelegate(this.OnCollectButtonClicked);
			base.CurrentBackDelegate = new UXButtonClickedDelegate(this.OnCollectButtonClicked);
			base.CurrentBackButton = element6;
		}

		private void ResetAllTriggers()
		{
			this.animator.ResetTrigger("ShowWinRebel");
			this.animator.ResetTrigger("ShowWinEmpire");
			this.animator.ResetTrigger("ShowDraw");
			this.animator.ResetTrigger("ShowDrawSF");
			this.animator.ResetTrigger("ShowLoss");
			this.animator.ResetTrigger("Close");
		}

		private void OnCollectButtonClicked(UXButton button)
		{
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			bool flag = warManager.ClaimCurrentPlayerCurrentWarReward();
			if (flag)
			{
				button.Enabled = false;
				return;
			}
			this.Close(null);
		}

		public void PlayCloseAnimation()
		{
			this.ResetAllTriggers();
			this.animator.SetTrigger("Close");
		}

		protected internal SquadWarEndCelebrationScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarEndCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).WantTransitions);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadWarEndCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).OnCollectButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadWarEndCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadWarEndCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).PlayCloseAnimation();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadWarEndCelebrationScreen)GCHandledObjects.GCHandleToObject(instance)).ResetAllTriggers();
			return -1L;
		}
	}
}
