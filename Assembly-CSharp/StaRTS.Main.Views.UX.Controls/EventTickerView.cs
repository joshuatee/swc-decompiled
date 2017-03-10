using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Controls
{
	public class EventTickerView : IViewFrameTimeObserver
	{
		private const string ELEMENT_TICKER = "GalaxyConflictTicker";

		private const string ELEMENT_STATUS = "GalaxyConflictStatus";

		private const string ELEMENT_TICKER_BG = "SpriteConflictStatusBg";

		private const string ELEMENT_STATUS_TITLE = "LabelConflictStatusTitle";

		private const string ELEMENT_TICKER_CLONE_NAME = "GalaxyConflictTickerClone";

		private const string ELEMENT_LABEL_NAME_FORMAT = "LabelConflictStatusEvent{0}";

		private const string ELEMENT_BUTTON_NAME_FORMAT = "LabelConflictStatusEvent{0}Btn";

		private const string ANIM_TITLE_SHOW = "Show";

		private const string ANIM_SHOW_FORMAT = "Show{0}";

		private const float ANIM_SHOW_INTERVAL_TIME_IN_SECONDS = 3f;

		public const int TOTAL_EVENT_TICKER_COUNT = 4;

		private UXFactory factory;

		private UXElement elementTicker;

		private UXElement elementStatus;

		private UXLabel labelStatusTitle;

		private UXSprite elementBgSprite;

		private UXLabel[] tickerLabels;

		private UXButton[] tickerButtons;

		public List<EventTickerObject> tickerDataObjects;

		private bool animatedTickerTitle;

		private float currentViewFrameTimeElapsed;

		private int currentTickerViewIndex;

		private int currentTickerDataObjectIndex;

		private int maxTickerViewsUsed;

		private bool isAnimating;

		public Color DefaultTickerBgColor
		{
			get;
			private set;
		}

		public EventTickerView(UXFactory factory)
		{
			this.factory = factory;
			this.InitializeEventTickerElements();
		}

		public void DestroyElements()
		{
			if (this.elementTicker != null)
			{
				this.factory.DestroyElement(this.elementTicker);
				this.elementTicker = null;
			}
			this.UnregisterViewFrameTimeObserver();
		}

		private void RegisterViewFrameTimeObserver()
		{
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		private void UnregisterViewFrameTimeObserver()
		{
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public EventTickerObject GetTickerObject(int index)
		{
			if (this.tickerDataObjects != null && this.tickerDataObjects.Count > index)
			{
				return this.tickerDataObjects[index];
			}
			return null;
		}

		public void OnViewFrameTime(float dt)
		{
			this.currentViewFrameTimeElapsed += dt;
			this.UpdateViewForBgSprite(dt);
		}

		private void UpdateViewForBgSprite(float dt)
		{
			if (this.currentViewFrameTimeElapsed > 3f)
			{
				this.currentViewFrameTimeElapsed = 0f;
				this.currentTickerViewIndex++;
				this.currentTickerDataObjectIndex++;
				if (this.currentTickerViewIndex >= this.maxTickerViewsUsed)
				{
					this.currentTickerViewIndex = 0;
				}
				if (this.currentTickerDataObjectIndex >= this.tickerDataObjects.Count)
				{
					this.currentTickerDataObjectIndex = 0;
				}
				this.UpdateTickerObject(this.currentTickerViewIndex, this.currentTickerDataObjectIndex);
			}
		}

		public void UpdateTickerObject(int tickerViewIndex, int tickerDataObjectIndex)
		{
			if (tickerDataObjectIndex < this.tickerDataObjects.Count && this.tickerDataObjects[tickerDataObjectIndex] != null)
			{
				EventTickerObject eventTickerObject = this.tickerDataObjects[tickerDataObjectIndex];
				this.SetLabelText(eventTickerObject.message, tickerViewIndex);
				this.SetButtonOnClicked(eventTickerObject.onClickFunction, tickerViewIndex);
				this.SetLabelTextColor(eventTickerObject.textColor, tickerViewIndex);
				this.SetBgSpriteTickerColor(eventTickerObject.bgColor);
				this.SetButtonTag(eventTickerObject.planet, tickerViewIndex);
			}
		}

		public void AnimateStatusTitleText()
		{
			if (!this.animatedTickerTitle)
			{
				Animator component = this.elementStatus.Root.GetComponent<Animator>();
				component.SetTrigger("Show");
				this.animatedTickerTitle = true;
			}
		}

		public void SetButtonTag(object tag, int index)
		{
			this.tickerButtons[index].Tag = tag;
		}

		public void SetTitleText(string titleText)
		{
			this.labelStatusTitle.Text = titleText;
		}

		public void AnimateTickerAtIndex(int index)
		{
			if (!this.isAnimating && index != 0)
			{
				this.AnimateTickerLabelAtIndex(index);
				this.AnimateTickerBgAtIndex(index);
				this.isAnimating = true;
				this.maxTickerViewsUsed = index;
			}
		}

		private void AnimateTickerLabelAtIndex(int index)
		{
			string trigger = string.Format("Show{0}", new object[]
			{
				index.ToString()
			});
			Animator component = this.elementStatus.Root.GetComponent<Animator>();
			component.SetTrigger(trigger);
		}

		private void AnimateTickerBgAtIndex(int index)
		{
			this.UnregisterViewFrameTimeObserver();
			this.currentTickerViewIndex = 0;
			this.currentViewFrameTimeElapsed = 0f;
			this.RegisterViewFrameTimeObserver();
		}

		public void StoreTickerObject(EventTickerObject obj, int index)
		{
			if (this.tickerDataObjects.Count > index)
			{
				this.tickerDataObjects[index] = obj;
				return;
			}
			this.tickerDataObjects.Add(obj);
		}

		private void SetBgSpriteTickerColor(Color color)
		{
			this.elementBgSprite.Color = color;
		}

		public void SetLabelTextColor(Color textColor, int index)
		{
			this.tickerLabels[index].TextColor = textColor;
		}

		public void SetLabelText(string message, int index)
		{
			this.tickerLabels[index].Text = message;
		}

		public void SetButtonOnClicked(UXButtonClickedDelegate onClickedDelegate, int index)
		{
			this.tickerButtons[index].OnClicked = onClickedDelegate;
		}

		private void InitializeEventTickerElements()
		{
			this.elementTicker = this.factory.CloneElement<UXElement>(this.factory.GetElement<UXElement>("GalaxyConflictTicker"), "GalaxyConflictTickerClone", Service.Get<UXController>().WorldAnchor.Root);
			string name = UXUtils.FormatAppendedName("GalaxyConflictStatus", "GalaxyConflictTickerClone");
			this.elementStatus = this.factory.GetElement<UXElement>(name);
			string name2 = UXUtils.FormatAppendedName("LabelConflictStatusTitle", "GalaxyConflictTickerClone");
			this.labelStatusTitle = this.factory.GetElement<UXLabel>(name2);
			string name3 = UXUtils.FormatAppendedName("SpriteConflictStatusBg", "GalaxyConflictTickerClone");
			this.elementBgSprite = this.factory.GetElement<UXSprite>(name3);
			this.DefaultTickerBgColor = this.elementBgSprite.Color;
			this.tickerLabels = new UXLabel[4];
			this.tickerButtons = new UXButton[4];
			this.tickerDataObjects = new List<EventTickerObject>();
			for (int i = 0; i < 4; i++)
			{
				string originalName = string.Format("LabelConflictStatusEvent{0}", new object[]
				{
					i + 1
				});
				string name4 = UXUtils.FormatAppendedName(originalName, "GalaxyConflictTickerClone");
				string originalName2 = string.Format("LabelConflictStatusEvent{0}Btn", new object[]
				{
					i + 1
				});
				string name5 = UXUtils.FormatAppendedName(originalName2, "GalaxyConflictTickerClone");
				this.tickerLabels[i] = this.factory.GetElement<UXLabel>(name4);
				this.tickerLabels[i].Text = string.Empty;
				this.tickerButtons[i] = this.factory.GetElement<UXButton>(name5);
			}
		}

		protected internal EventTickerView(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).AnimateStatusTitleText();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).AnimateTickerAtIndex(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).AnimateTickerBgAtIndex(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).AnimateTickerLabelAtIndex(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).DestroyElements();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).DefaultTickerBgColor);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).GetTickerObject(*(int*)args));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).InitializeEventTickerElements();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).RegisterViewFrameTimeObserver();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).DefaultTickerBgColor = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).SetBgSpriteTickerColor(*(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).SetButtonOnClicked((UXButtonClickedDelegate)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).SetButtonTag(GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).SetLabelText(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).SetLabelTextColor(*(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).SetTitleText(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).StoreTickerObject((EventTickerObject)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).UnregisterViewFrameTimeObserver();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).UpdateTickerObject(*(int*)args, *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((EventTickerView)GCHandledObjects.GCHandleToObject(instance)).UpdateViewForBgSprite(*(float*)args);
			return -1L;
		}
	}
}
