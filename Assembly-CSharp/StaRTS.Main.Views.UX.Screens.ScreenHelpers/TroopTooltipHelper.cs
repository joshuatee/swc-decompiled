using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers
{
	public class TroopTooltipHelper
	{
		private Dictionary<UXButton, string> buttonTooltipTextMap;

		public void RegisterButtonTooltip(UXButton button, IUpgradeableVO vo, BattleEntry battle)
		{
			string tooltipText = null;
			if (vo is SpecialAttackTypeVO)
			{
				tooltipText = LangUtils.GetStarshipDisplayName((SpecialAttackTypeVO)vo);
			}
			else if (vo is TroopTypeVO)
			{
				tooltipText = LangUtils.GetTroopDisplayName((TroopTypeVO)vo);
			}
			GeometryTag geometryTag = new GeometryTag(vo, tooltipText, battle);
			Service.Get<EventManager>().SendEvent(EventId.TooltipCreated, geometryTag);
			if (geometryTag.tooltipText != null)
			{
				this.RegisterButtonTooltip(button, geometryTag.tooltipText);
			}
		}

		public void RegisterButtonTooltip(UXButton button, string localizedText)
		{
			if (this.buttonTooltipTextMap == null)
			{
				this.buttonTooltipTextMap = new Dictionary<UXButton, string>();
			}
			this.buttonTooltipTextMap.Add(button, localizedText);
			button.OnPressed = new UXButtonPressedDelegate(this.OnTooltipButtonPressed);
			button.OnReleased = new UXButtonReleasedDelegate(this.OnTooltipButtonReleased);
		}

		private void OnTooltipButtonPressed(UXButton button)
		{
			string localizedText = this.buttonTooltipTextMap[button];
			Service.Get<UXController>().MiscElementsManager.ShowTroopTooltip(button, localizedText);
		}

		private void OnTooltipButtonReleased(UXButton button)
		{
			Service.Get<UXController>().MiscElementsManager.HideTroopTooltip();
		}

		public void Destroy()
		{
			if (this.buttonTooltipTextMap != null)
			{
				foreach (KeyValuePair<UXButton, string> current in this.buttonTooltipTextMap)
				{
					UXButton key = current.get_Key();
					key.OnPressed = null;
					key.OnReleased = null;
				}
				this.buttonTooltipTextMap = null;
			}
		}

		public TroopTooltipHelper()
		{
		}

		protected internal TroopTooltipHelper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TroopTooltipHelper)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TroopTooltipHelper)GCHandledObjects.GCHandleToObject(instance)).OnTooltipButtonPressed((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TroopTooltipHelper)GCHandledObjects.GCHandleToObject(instance)).OnTooltipButtonReleased((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TroopTooltipHelper)GCHandledObjects.GCHandleToObject(instance)).RegisterButtonTooltip((UXButton)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TroopTooltipHelper)GCHandledObjects.GCHandleToObject(instance)).RegisterButtonTooltip((UXButton)GCHandledObjects.GCHandleToObject(*args), (IUpgradeableVO)GCHandledObjects.GCHandleToObject(args[1]), (BattleEntry)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}
	}
}
