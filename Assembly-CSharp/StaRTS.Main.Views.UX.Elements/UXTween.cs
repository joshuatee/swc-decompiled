using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXTween
	{
		public UIPlayTween NGUIPlayTween
		{
			get;
			private set;
		}

		public bool Enable
		{
			get
			{
				return !(this.NGUIPlayTween == null) && this.NGUIPlayTween.enabled;
			}
			set
			{
				if (this.NGUIPlayTween != null)
				{
					this.NGUIPlayTween.enabled = value;
				}
			}
		}

		public void Init(GameObject uiRoot)
		{
			if (uiRoot != null)
			{
				this.NGUIPlayTween = uiRoot.GetComponent<UIPlayTween>();
			}
		}

		public void ResetUIPlayTweenTargetToBegining()
		{
			if (this.NGUIPlayTween != null && this.NGUIPlayTween.tweenTarget != null)
			{
				GameObject tweenTarget = this.NGUIPlayTween.tweenTarget;
				TweenPosition component = tweenTarget.GetComponent<TweenPosition>();
				if (component != null)
				{
					component.enabled = false;
					component.ResetToBeginning();
					component.Sample(0f, true);
					component.tweenFactor = 0f;
				}
				TweenAlpha component2 = tweenTarget.GetComponent<TweenAlpha>();
				if (component2 != null)
				{
					component2.ResetToBeginning();
					component2.Sample(0f, true);
					component2.tweenFactor = 0f;
					component2.enabled = false;
				}
			}
		}

		public UXTween()
		{
		}

		protected internal UXTween(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTween)GCHandledObjects.GCHandleToObject(instance)).Enable);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTween)GCHandledObjects.GCHandleToObject(instance)).NGUIPlayTween);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UXTween)GCHandledObjects.GCHandleToObject(instance)).Init((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UXTween)GCHandledObjects.GCHandleToObject(instance)).ResetUIPlayTweenTargetToBegining();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UXTween)GCHandledObjects.GCHandleToObject(instance)).Enable = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((UXTween)GCHandledObjects.GCHandleToObject(instance)).NGUIPlayTween = (UIPlayTween)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
