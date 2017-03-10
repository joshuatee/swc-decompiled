using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers
{
	public class SocialTabInfo
	{
		public UXCheckbox TabButton
		{
			get;
			set;
		}

		public UXLabel TabLabel
		{
			get;
			set;
		}

		public Action LoadAction
		{
			get;
			private set;
		}

		public GridLoadHelper TabGridLoadHelper
		{
			get;
			set;
		}

		public EventId TabEventId
		{
			get;
			private set;
		}

		public string EventActionId
		{
			get;
			private set;
		}

		public PlayerListType ListType
		{
			get;
			private set;
		}

		public SocialTabInfo(Action loadAction, EventId eventId, string eventActionId, PlayerListType listType)
		{
			this.LoadAction = loadAction;
			this.TabEventId = eventId;
			this.EventActionId = eventActionId;
			this.ListType = listType;
		}

		protected internal SocialTabInfo(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).EventActionId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).ListType);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).LoadAction);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).TabButton);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).TabEventId);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).TabGridLoadHelper);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).TabLabel);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).EventActionId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).ListType = (PlayerListType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).LoadAction = (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).TabButton = (UXCheckbox)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).TabEventId = (EventId)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).TabGridLoadHelper = (GridLoadHelper)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SocialTabInfo)GCHandledObjects.GCHandleToObject(instance)).TabLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
