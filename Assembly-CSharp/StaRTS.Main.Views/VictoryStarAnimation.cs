using StaRTS.Main.Controllers;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Animation;
using StaRTS.Utils.Animation.Anims;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views
{
	public class VictoryStarAnimation
	{
		private const float ANIMATION_TIME = 2f;

		private const float TWEEN_TIME = 0.5f;

		private const float SCREEN_SIZE_FACTOR = 0.5f;

		private const float VERTICAL_MOVEMENT = 100f;

		private int starNumber;

		private string message;

		private UXElement starAnimator;

		private UXElement starAnchor;

		private Vector3 originalLocation;

		private StarAnimationCompleteDelegate onComplete;

		public VictoryStarAnimation(int starNumber, string message)
		{
			this.starNumber = starNumber;
			this.message = message;
		}

		public void Start(StarAnimationCompleteDelegate onComplete)
		{
			this.onComplete = onComplete;
			HUD hUD = Service.Get<UXController>().HUD;
			UXLabel damageStarLabel = hUD.GetDamageStarLabel();
			damageStarLabel.Text = this.message;
			this.starAnimator = hUD.GetDamageStarAnimator();
			this.starAnchor = hUD.GetDamageStarAnchor();
			Animator component = this.starAnimator.Root.GetComponent<Animator>();
			if (component == null)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Unable to play star anim #{0}", new object[]
				{
					this.starNumber
				});
				if (onComplete != null)
				{
					onComplete(this.starNumber);
					return;
				}
			}
			else
			{
				this.starAnchor.Visible = true;
				this.starAnimator.Visible = true;
				component.enabled = true;
				component.SetTrigger("Show");
				Service.Get<ViewTimerManager>().CreateViewTimer(2f, false, new TimerDelegate(this.OnAnimationFinishedTimer), null);
			}
		}

		private void OnAnimationFinishedTimer(uint id, object cookie)
		{
			this.originalLocation = this.starAnimator.LocalPosition;
			AnimUXPosition animUXPosition = new AnimUXPosition(this.starAnimator, 0.5f, Vector3.right * (float)Screen.width * 0.5f + Vector3.up * 100f * this.starAnimator.UXCamera.Scale);
			animUXPosition.EaseFunction = new Easing.EasingDelegate(Easing.SineEaseIn);
			animUXPosition.OnCompleteCallback = new Action<Anim>(this.OnTweenFinished);
			Service.Get<AnimController>().Animate(animUXPosition);
		}

		private void OnTweenFinished(Anim anim)
		{
			Service.Get<UXController>().HUD.UpdateDamageStars(this.starNumber);
			this.starAnimator.LocalPosition = this.originalLocation;
			this.starAnimator.Visible = false;
			this.starAnchor.Visible = false;
			if (this.onComplete != null)
			{
				this.onComplete(this.starNumber);
			}
		}

		protected internal VictoryStarAnimation(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VictoryStarAnimation)GCHandledObjects.GCHandleToObject(instance)).OnTweenFinished((Anim)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VictoryStarAnimation)GCHandledObjects.GCHandleToObject(instance)).Start((StarAnimationCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
