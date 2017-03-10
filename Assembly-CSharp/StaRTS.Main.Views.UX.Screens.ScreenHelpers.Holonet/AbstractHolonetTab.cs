using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Holonet;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet
{
	public class AbstractHolonetTab
	{
		public delegate void TimestampsHandler(uint[] timestamps);

		private const float HOLONET_TAB_ANIMATION_DURATION = 1.5f;

		protected HolonetScreen screen;

		protected EventManager eventManager;

		protected Lang lang;

		protected UXElement tabItem;

		protected UXCheckbox tabButton;

		protected UXElement badgeGroup;

		protected UXLabel badgeLabel;

		protected UXElement topLevelGroup;

		protected UXSprite highlight;

		protected UXLabel label;

		private List<UXTexture> deferredTextures;

		public HolonetControllerType HolonetControllerType;

		public AbstractHolonetTab(HolonetScreen screen, HolonetControllerType holonetControllerType)
		{
			this.screen = screen;
			this.HolonetControllerType = holonetControllerType;
			this.lang = Service.Get<Lang>();
			this.eventManager = Service.Get<EventManager>();
			this.deferredTextures = new List<UXTexture>();
		}

		protected void InitializeTab(string topLevelGroupName, string tabLabelId)
		{
			this.topLevelGroup = this.screen.GetElement<UXElement>(topLevelGroupName);
			this.tabItem = this.screen.NavTable.CloneTemplateItem(topLevelGroupName);
			this.tabButton = this.screen.NavTable.GetSubElement<UXCheckbox>(topLevelGroupName, "NavTapProperties");
			this.highlight = this.screen.NavTable.GetSubElement<UXSprite>(topLevelGroupName, "SpriteTabHighlight");
			this.badgeGroup = this.screen.NavTable.GetSubElement<UXElement>(topLevelGroupName, "BadgeGroup");
			this.badgeLabel = this.screen.NavTable.GetSubElement<UXLabel>(topLevelGroupName, "BadgeLabel");
			this.label = this.screen.NavTable.GetSubElement<UXLabel>(topLevelGroupName, "NavItemLabel");
			string text = Service.Get<Lang>().Get(tabLabelId, new object[0]);
			this.label.Text = text;
			this.AddSelectionButtonToNavTable();
			this.tabButton.OnSelected = new UXCheckboxSelectedDelegate(this.SetVisibleByTabButton);
			Service.Get<ViewTimerManager>().CreateViewTimer(1.5f, false, new TimerDelegate(this.OnAnimationTimerComplete), null);
		}

		public void OnAnimationTimerComplete(uint id, object cookie)
		{
			this.LoadDeferredTextures();
		}

		protected void DeferTexture(UXTexture texture, string assetName)
		{
			texture.DeferTextureForLoad(assetName);
			this.deferredTextures.Add(texture);
		}

		private void LoadDeferredTextures()
		{
			int i = 0;
			int count = this.deferredTextures.Count;
			while (i < count)
			{
				this.deferredTextures[i].LoadDeferred();
				i++;
			}
			this.deferredTextures.Clear();
		}

		protected virtual void AddSelectionButtonToNavTable()
		{
			this.screen.NavTable.AddItem(this.tabItem, this.screen.NavTable.Count);
		}

		public void EnableTabButton()
		{
			this.tabButton.SetSelectable(true);
		}

		public void DisableTabButton()
		{
			this.tabButton.SetSelectable(false);
		}

		public void SetBadgeCount(int count)
		{
			this.badgeGroup.Visible = (count > 0);
			this.badgeLabel.Text = count.ToString();
		}

		protected virtual void SetVisibleByTabButton(UXCheckbox button, bool selected)
		{
			if (selected)
			{
				Service.Get<EventManager>().SendEvent(EventId.HolonetChangeTabs, null);
				this.screen.OpenTab(this.HolonetControllerType);
			}
		}

		public virtual void OnTabOpen()
		{
			this.tabButton.Selected = true;
			this.tabButton.PlayTween(true);
			this.highlight.PlayTween(true);
			Service.Get<HolonetController>().SetLastViewed(this);
		}

		public virtual void OnTabClose()
		{
			this.tabButton.Selected = false;
			this.tabButton.PlayTween(false);
			this.highlight.PlayTween(false);
		}

		protected void PrepareButton(ICallToAction vo, int btnIndex, UXButton button, UXLabel btnLabel)
		{
			if (button == null || btnLabel == null)
			{
				return;
			}
			string text = (btnIndex == 1) ? vo.Btn1 : vo.Btn2;
			if (string.IsNullOrEmpty(text))
			{
				button.Visible = false;
				return;
			}
			button.Visible = true;
			if (btnIndex == 1)
			{
				button.OnClicked = new UXButtonClickedDelegate(this.FeaturedButton1Clicked);
			}
			else
			{
				button.OnClicked = new UXButtonClickedDelegate(this.FeaturedButton2Clicked);
			}
			button.Tag = vo;
			btnLabel.Text = this.lang.Get(text, new object[0]);
			if (this.IsButtonRewardAction(vo, btnIndex))
			{
				IDataController dataController = Service.Get<IDataController>();
				LimitedTimeRewardVO vo2;
				if (btnIndex == 1)
				{
					vo2 = dataController.Get<LimitedTimeRewardVO>(vo.Btn1Data);
				}
				else
				{
					vo2 = dataController.Get<LimitedTimeRewardVO>(vo.Btn2Data);
				}
				this.SetupRewardButton(ref button, ref btnLabel, vo2);
			}
		}

		protected bool HasRewardButton1BeenClaimed(LimitedTimeRewardVO vo)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return currentPlayer.HolonetRewards.Contains(vo.Uid);
		}

		protected void SetupRewardButton(ref UXButton btn, ref UXLabel btnLabel, LimitedTimeRewardVO vo)
		{
			if (this.HasRewardButton1BeenClaimed(vo))
			{
				btnLabel.Text = this.lang.Get("hn_reward_been_claimed", new object[0]);
				btn.Enabled = false;
			}
		}

		protected void SendCallToActionBI(string action, string uid, EventId id)
		{
			string cookie = action + "|" + uid + "|cta_button";
			this.eventManager.SendEvent(id, cookie);
		}

		protected virtual void FeaturedButton1Clicked(UXButton button)
		{
		}

		protected virtual void FeaturedButton2Clicked(UXButton button)
		{
		}

		private bool IsButtonRewardAction(ICallToAction vo, int index)
		{
			if (index == 1)
			{
				return vo.Btn1Action == "reward";
			}
			return vo.Btn2Action == "reward";
		}

		public virtual void OnDestroyTab()
		{
		}

		public virtual string GetBITabName()
		{
			return string.Empty;
		}

		protected internal AbstractHolonetTab(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).AddSelectionButtonToNavTable();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).DeferTexture((UXTexture)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).DisableTabButton();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).EnableTabButton();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).FeaturedButton1Clicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).FeaturedButton2Clicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).GetBITabName());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).HasRewardButton1BeenClaimed((LimitedTimeRewardVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).InitializeTab(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).IsButtonRewardAction((ICallToAction)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).LoadDeferredTextures();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnDestroyTab();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnTabClose();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnTabOpen();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).PrepareButton((ICallToAction)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), (UXButton)GCHandledObjects.GCHandleToObject(args[2]), (UXLabel)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SendCallToActionBI(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (EventId)(*(int*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetBadgeCount(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((AbstractHolonetTab)GCHandledObjects.GCHandleToObject(instance)).SetVisibleByTabButton((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}
	}
}
