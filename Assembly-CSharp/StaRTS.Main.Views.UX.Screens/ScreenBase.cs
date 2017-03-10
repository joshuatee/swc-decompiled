using StaRTS.Assets;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class ScreenBase : UXFactory
	{
		private const string TRANSITION_IN = "guiAnim_dialogIn";

		private const string TRANSITION_OUT = "guiAnim_dialogOut";

		private const string MAIN_WIDGET_POSTFIX = "_main_widget";

		private const string CURRENCY_TRAY = "CurrencyTray";

		private object modalResult;

		private string assetName;

		private AssetHandle assetHandle;

		protected Lang lang;

		private UIPanel rootPanel;

		private ScreenTransition transition;

		private bool closing;

		public List<UXButton> BackButtons;

		private bool isWaitingToShow;

		public OnScreenModalResult OnModalResult
		{
			get;
			set;
		}

		public object ModalResultCookie
		{
			get;
			set;
		}

		public OnTransInComplete OnTransitionInComplete
		{
			get;
			set;
		}

		public bool TransitionedIn
		{
			get;
			private set;
		}

		public string AssetName
		{
			get
			{
				return this.assetName;
			}
		}

		public bool IsClosing
		{
			get
			{
				return this.closing;
			}
		}

		protected virtual bool WantTransitions
		{
			get
			{
				return true;
			}
		}

		protected virtual bool AllowGarbageCollection
		{
			get
			{
				return true;
			}
		}

		public virtual bool ShowCurrencyTray
		{
			get
			{
				return false;
			}
		}

		public bool AllowFUEBackButton
		{
			get;
			set;
		}

		public bool IsAlwaysOnTop
		{
			get;
			set;
		}

		public UXButtonClickedDelegate CurrentBackDelegate
		{
			get;
			set;
		}

		public UXButton CurrentBackButton
		{
			get;
			set;
		}

		public ScreenBase(string assetName) : base(Service.Get<CameraManager>().UXCamera)
		{
			this.isWaitingToShow = false;
			this.assetName = assetName;
			this.lang = Service.Get<Lang>();
			this.transition = null;
			this.closing = false;
			this.AllowFUEBackButton = false;
			this.BackButtons = new List<UXButton>();
			this.TransitionedIn = false;
			base.Load(ref this.assetHandle, assetName, new UXFactoryLoadDelegate(this.OnScreenLoadSuccess), null, null);
		}

		public override void SetupRootCollider()
		{
			this.SetupRootPanel();
			if (this.WantTransitions)
			{
				Animation animation = this.root.GetComponent<Animation>();
				if (animation == null && Service.IsSet<UXController>())
				{
					animation = Service.Get<UXController>().MiscElementsManager.AddScreenTransitionAnimation(this.root);
				}
				if (animation != null)
				{
					this.transition = new ScreenTransition(animation);
				}
			}
			this.TransitionIn();
		}

		private void SetupRootPanel()
		{
			this.rootPanel = this.root.GetComponent<UIPanel>();
			if (this.rootPanel == null)
			{
				this.rootPanel = this.root.GetComponentInChildren<UIPanel>();
				if (this.rootPanel == null)
				{
					this.rootPanel = this.root.GetComponentInParent<UIPanel>();
				}
			}
		}

		public int GetRootPanelDepth()
		{
			if (this.rootPanel == null)
			{
				this.SetupRootPanel();
			}
			if (!(this.rootPanel == null))
			{
				return this.rootPanel.depth;
			}
			return 0;
		}

		private void TransitionIn()
		{
			if (this.transition != null)
			{
				this.transition.PlayTransition("guiAnim_dialogIn", new OnScreenTransitionComplete(this.TransitionInComplete), true);
				return;
			}
			this.TransitionInComplete();
		}

		private void TransitionOut()
		{
			this.TransitionedIn = false;
			if (this.transition != null && this.WantTransitions)
			{
				this.transition.PlayTransition("guiAnim_dialogOut", new OnScreenTransitionComplete(this.TransitionOutComplete), false);
				return;
			}
			this.TransitionOutComplete();
		}

		protected float GetAlpha()
		{
			if (!(this.rootPanel != null))
			{
				return 1f;
			}
			return this.rootPanel.alpha;
		}

		private void DestroyTransition()
		{
			if (this.transition != null)
			{
				this.transition.Destroy();
				this.transition = null;
			}
		}

		private void OnScreenLoadSuccess(object cookie)
		{
			if (!this.isWaitingToShow)
			{
				this.HandleScreenLoaded();
			}
		}

		public void OnScreenAddedToQueue()
		{
			this.isWaitingToShow = true;
		}

		public void OnScreeenPoppedFromQueue()
		{
			this.isWaitingToShow = false;
			this.HandleScreenLoaded();
		}

		private void HandleScreenLoaded()
		{
			if (base.IsLoaded())
			{
				this.OnScreenLoaded();
				if (this.ShowCurrencyTray)
				{
					this.UpdateCurrencyTrayAttachment();
				}
				Service.Get<EventManager>().SendEvent(EventId.ScreenLoaded, this);
			}
		}

		public void UpdateCurrencyTrayAttachment()
		{
			UXElement uXElement = base.GetOptionalElement<UXElement>("CurrencyTray");
			if (uXElement == null)
			{
				if (base.HasCollider())
				{
					uXElement = this;
				}
				else
				{
					uXElement = base.GetElement<UXElement>(this.root.name + "_main_widget");
				}
			}
			if (uXElement != null)
			{
				Service.Get<UXController>().MiscElementsManager.AttachCurrencyTrayToScreen(uXElement, this.GetDisplayCurrencyTrayType());
				return;
			}
			Service.Get<StaRTSLogger>().Warn("Cannot attach currency tray");
		}

		protected virtual CurrencyTrayType GetDisplayCurrencyTrayType()
		{
			return CurrencyTrayType.Default;
		}

		protected virtual void OnScreenLoaded()
		{
			this.CustomDisableElementsContaining("fxspritebtnglowinbtn");
			this.CustomDisableElementsContaining("fxspritetapbtn");
			this.CustomDisableElementsContaining("fxspriteglowinbtn");
			this.CustomDisableElementsContaining("SpriteBodyBgGradientGlow");
		}

		private void CustomDisableElementsContaining(string partialName)
		{
			List<UXElement> list = base.FindElement(partialName);
			if (list != null && list.Count > 0)
			{
				foreach (UXElement current in list)
				{
					current.Enabled = false;
				}
			}
		}

		public void CloseNoTransition(object modalResult)
		{
			this.DestroyTransition();
			this.Close(modalResult);
		}

		public virtual void Close(object modalResult)
		{
			if (this.closing)
			{
				return;
			}
			this.closing = true;
			if (Service.IsSet<ScreenController>() && !(Service.Get<GameStateMachine>().CurrentState is ApplicationLoadState) && !(Service.Get<GameStateMachine>().CurrentState is GalaxyState))
			{
				Service.Get<ScreenController>().RecalculateHudVisibility();
			}
			Service.Get<EventManager>().SendEvent(EventId.ScreenClosing, this);
			this.modalResult = modalResult;
			this.TransitionOut();
		}

		private void TransitionInComplete()
		{
			bool arg_06_0 = this.AllowGarbageCollection;
			this.TransitionedIn = true;
			if (this.OnTransitionInComplete != null)
			{
				this.OnTransitionInComplete();
				this.OnTransitionInComplete = null;
			}
		}

		private void TransitionOutComplete()
		{
			if (this.OnModalResult != null)
			{
				this.OnModalResult(this.modalResult, this.ModalResultCookie);
			}
			Service.Get<ViewTimerManager>().CreateViewTimer(0.001f, false, new TimerDelegate(this.DestroyScreenOnTimer), null);
		}

		protected void CleanupScreenTransition(bool visible)
		{
			if (!this.WantTransitions)
			{
				Animation component = base.Root.GetComponent<Animation>();
				if (component != null)
				{
					if (component.isPlaying)
					{
						component.Stop();
					}
					ScreenTransition.ForceAlpha(component, (float)(visible ? 1 : 0));
				}
			}
		}

		private void DestroyScreenOnTimer(uint timerId, object cookie)
		{
			this.DestroyScreen();
		}

		public override void OnDestroyElement()
		{
			this.DestroyTransition();
			if (this.assetHandle != AssetHandle.Invalid)
			{
				base.Unload(this.assetHandle, this.assetName);
				this.assetHandle = AssetHandle.Invalid;
			}
			if (Service.IsSet<ScreenController>())
			{
				Service.Get<ScreenController>().RemoveScreen(this);
				Service.Get<ScreenController>().RecalculateCurrencyTrayVisibility();
			}
			base.OnDestroyElement();
		}

		public void DestroyScreen()
		{
			base.DestroyFactory();
		}

		protected internal ScreenBase(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).CleanupScreenTransition(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).Close(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).CloseNoTransition(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).CustomDisableElementsContaining(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).DestroyScreen();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).DestroyTransition();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).AllowFUEBackButton);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).AllowGarbageCollection);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).CurrentBackButton);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).CurrentBackDelegate);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).IsAlwaysOnTop);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).IsClosing);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).ModalResultCookie);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).OnModalResult);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).OnTransitionInComplete);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).ShowCurrencyTray);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).TransitionedIn);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).WantTransitions);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).GetAlpha());
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).GetDisplayCurrencyTrayType());
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).GetRootPanelDepth());
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).HandleScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).OnScreeenPoppedFromQueue();
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).OnScreenAddedToQueue();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoadSuccess(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).AllowFUEBackButton = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).CurrentBackButton = (UXButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).CurrentBackDelegate = (UXButtonClickedDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).IsAlwaysOnTop = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).ModalResultCookie = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).OnModalResult = (OnScreenModalResult)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).OnTransitionInComplete = (OnTransInComplete)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).TransitionedIn = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).SetupRootCollider();
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).SetupRootPanel();
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).TransitionIn();
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).TransitionInComplete();
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).TransitionOut();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).TransitionOutComplete();
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((ScreenBase)GCHandledObjects.GCHandleToObject(instance)).UpdateCurrencyTrayAttachment();
			return -1L;
		}
	}
}
