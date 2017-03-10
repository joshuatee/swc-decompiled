using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class PersistentAnimatedScreen : ScreenBase
	{
		private PersistentScreenState AnimatingState;

		private Animation openCloseAnim;

		private AnimationState animState;

		private uint animationTimer;

		protected bool shouldCloseOnAnimComplete;

		protected object closeModalResult;

		protected PersistentAnimatedScreen(string assetName) : base(assetName)
		{
			this.animationTimer = 0u;
			this.AnimatingState = PersistentScreenState.Closed;
			this.shouldCloseOnAnimComplete = false;
			this.closeModalResult = null;
		}

		protected void InitAnimations(string animationElementName, string animationName)
		{
			UXElement element = base.GetElement<UXElement>(animationElementName);
			this.openCloseAnim = element.Root.GetComponent<Animation>();
			this.animState = this.openCloseAnim[animationName];
		}

		private void PlayAnimation(bool openScreen)
		{
			if (openScreen)
			{
				this.AnimatingState = PersistentScreenState.Opening;
				this.animState.speed = 1f;
				this.animState.time = 0f;
			}
			else
			{
				this.AnimatingState = PersistentScreenState.Closing;
				this.animState.speed = -1f;
				this.animState.time = this.animState.length;
			}
			this.animationTimer = Service.Get<ViewTimerManager>().CreateViewTimer(this.animState.length, false, new TimerDelegate(this.OnAnimationComplete), null);
			this.openCloseAnim.Play();
			Service.Get<EventManager>().SendEvent(EventId.UpdateScrim, null);
		}

		public void InstantClose(bool shouldCloseOnAnimComplete, object modalResult)
		{
			this.ClearDefaultBackDelegate();
			this.shouldCloseOnAnimComplete = shouldCloseOnAnimComplete;
			this.closeModalResult = modalResult;
			this.AnimatingState = PersistentScreenState.Closing;
			this.OnAnimationComplete(0u, null);
			Service.Get<EventManager>().SendEvent(EventId.UpdateScrim, null);
		}

		protected virtual void OnAnimationComplete(uint id, object cookie)
		{
			PersistentScreenState animatingState = this.AnimatingState;
			if (animatingState != PersistentScreenState.Closing)
			{
				if (animatingState == PersistentScreenState.Opening)
				{
					this.AnimatingState = PersistentScreenState.Open;
					this.openCloseAnim.Play();
					this.animState.time = this.animState.length;
					this.openCloseAnim.Sample();
					this.openCloseAnim.Stop();
					this.OnOpen();
				}
			}
			else
			{
				this.AnimatingState = PersistentScreenState.Closed;
				this.openCloseAnim.Play();
				this.animState.time = 0f;
				this.openCloseAnim.Sample();
				this.openCloseAnim.Stop();
				this.OnClose();
				if (this.shouldCloseOnAnimComplete)
				{
					object modalResult = this.closeModalResult;
					this.Close(modalResult);
				}
			}
			this.animationTimer = 0u;
		}

		protected void ClearCloseOnAnimFlags()
		{
			this.closeModalResult = null;
			this.shouldCloseOnAnimComplete = false;
		}

		public virtual void AnimateOpen()
		{
			if (this.IsClosed())
			{
				this.OnOpening();
				this.PlayAnimation(true);
			}
		}

		public void AnimateClosed(bool closeOnFinish, object modalResult)
		{
			if (this.IsOpen())
			{
				this.PlayAnimation(false);
				this.ClearDefaultBackDelegate();
				this.shouldCloseOnAnimComplete = closeOnFinish;
				this.closeModalResult = modalResult;
				this.OnClosing();
			}
		}

		public override void SetupRootCollider()
		{
		}

		public bool IsOpen()
		{
			return this.AnimatingState == PersistentScreenState.Open;
		}

		public bool IsOpening()
		{
			return this.AnimatingState == PersistentScreenState.Opening;
		}

		public bool IsClosed()
		{
			return this.AnimatingState == PersistentScreenState.Closed;
		}

		public bool IsAnimClosing()
		{
			return this.AnimatingState == PersistentScreenState.Closing;
		}

		private void HandleBackButton(UXButton btn)
		{
			if (!Service.Get<QuestController>().HasPendingTriggers())
			{
				this.AnimateClosed(false, null);
			}
		}

		public void SetDefaultBackDelegate()
		{
			base.CurrentBackDelegate = new UXButtonClickedDelegate(this.HandleBackButton);
			base.CurrentBackButton = null;
		}

		protected void ClearDefaultBackDelegate()
		{
			base.CurrentBackDelegate = null;
			base.CurrentBackButton = null;
		}

		protected virtual void OnOpening()
		{
			this.SetDefaultBackDelegate();
			Service.Get<UXController>().HUD.Visible = false;
			Service.Get<UserInputInhibitor>().DenyAll();
		}

		protected virtual void OnOpen()
		{
			Service.Get<UserInputInhibitor>().AllowAll();
		}

		protected virtual void OnClosing()
		{
			this.ClearDefaultBackDelegate();
			Service.Get<UserInputInhibitor>().DenyAll();
		}

		protected virtual void OnClose()
		{
			if (Service.Get<GameStateMachine>().CurrentState is HomeState)
			{
				Service.Get<UXController>().HUD.Visible = true;
			}
			Service.Get<UserInputInhibitor>().AllowAll();
		}

		public override void OnDestroyElement()
		{
			Service.Get<ViewTimerManager>().EnsureTimerKilled(ref this.animationTimer);
			base.OnDestroyElement();
		}

		public override void Close(object modalResult)
		{
			this.ClearDefaultBackDelegate();
			base.Close(modalResult);
		}

		protected internal PersistentAnimatedScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).AnimateClosed(*(sbyte*)args != 0, GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).AnimateOpen();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).ClearCloseOnAnimFlags();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).ClearDefaultBackDelegate();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).HandleBackButton((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).InitAnimations(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).InstantClose(*(sbyte*)args != 0, GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).IsAnimClosing());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).IsClosed());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).IsOpen());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).IsOpening());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).OnClose();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).OnClosing();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).OnOpen();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).OnOpening();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).PlayAnimation(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).SetDefaultBackDelegate();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((PersistentAnimatedScreen)GCHandledObjects.GCHandleToObject(instance)).SetupRootCollider();
			return -1L;
		}
	}
}
