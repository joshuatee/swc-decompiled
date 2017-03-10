using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class ScreenInfo
	{
		public UXElement Screen
		{
			get;
			set;
		}

		public int Depth
		{
			get;
			set;
		}

		public int ScreenPanelThickness
		{
			get;
			set;
		}

		public bool IsModal
		{
			get;
			private set;
		}

		public bool VisibleScrim
		{
			get;
			private set;
		}

		public bool WasVisible
		{
			get;
			set;
		}

		public QueueScreenBehavior QueueBehavior
		{
			get;
			private set;
		}

		public ScreenInfo(UXElement screen, bool modal) : this(screen, modal, true, QueueScreenBehavior.Default)
		{
		}

		public ScreenInfo(UXElement screen, bool modal, bool visibleScrim, QueueScreenBehavior subType)
		{
			this.Screen = screen;
			this.Depth = 0;
			this.ScreenPanelThickness = 0;
			this.IsModal = modal;
			this.VisibleScrim = visibleScrim;
			this.QueueBehavior = subType;
			this.WasVisible = true;
		}

		public void OnEnqueued()
		{
			if (this.QueueBehavior == QueueScreenBehavior.DeferTillClosed)
			{
				this.QueueBehavior = QueueScreenBehavior.QueueAndDeferTillClosed;
				return;
			}
			if (this.QueueBehavior == QueueScreenBehavior.Default)
			{
				this.QueueBehavior = QueueScreenBehavior.Queue;
			}
		}

		public void OnDequeued()
		{
			if (this.QueueBehavior == QueueScreenBehavior.QueueAndDeferTillClosed)
			{
				this.QueueBehavior = QueueScreenBehavior.DeferTillClosed;
				return;
			}
			if (this.QueueBehavior == QueueScreenBehavior.Queue)
			{
				this.QueueBehavior = QueueScreenBehavior.Default;
			}
		}

		public bool ShouldDefer()
		{
			return this.QueueBehavior == QueueScreenBehavior.DeferTillClosed || this.QueueBehavior == QueueScreenBehavior.QueueAndDeferTillClosed;
		}

		public bool HasQueueBehavior()
		{
			return this.QueueBehavior == QueueScreenBehavior.Queue || this.QueueBehavior == QueueScreenBehavior.QueueAndDeferTillClosed;
		}

		public bool CanShowMoreScreens()
		{
			return this.QueueBehavior != QueueScreenBehavior.DeferTillClosed && this.QueueBehavior != QueueScreenBehavior.QueueAndDeferTillClosed;
		}

		public bool IsPersistentAndOpen()
		{
			bool result = false;
			if (this.Screen is PersistentAnimatedScreen)
			{
				PersistentAnimatedScreen persistentAnimatedScreen = (PersistentAnimatedScreen)this.Screen;
				if (persistentAnimatedScreen.IsOpen() || persistentAnimatedScreen.IsOpening())
				{
					result = true;
				}
			}
			return result;
		}

		protected internal ScreenInfo(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).CanShowMoreScreens());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).Depth);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).IsModal);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).QueueBehavior);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).Screen);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).ScreenPanelThickness);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).VisibleScrim);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).WasVisible);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).HasQueueBehavior());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).IsPersistentAndOpen());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).OnDequeued();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).OnEnqueued();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).Depth = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).IsModal = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).QueueBehavior = (QueueScreenBehavior)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).Screen = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).ScreenPanelThickness = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).VisibleScrim = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).WasVisible = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenInfo)GCHandledObjects.GCHandleToObject(instance)).ShouldDefer());
		}
	}
}
