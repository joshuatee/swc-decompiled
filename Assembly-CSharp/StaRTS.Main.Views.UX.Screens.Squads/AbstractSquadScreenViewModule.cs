using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.Squads
{
	public class AbstractSquadScreenViewModule
	{
		protected SquadSlidingScreen screen;

		protected Lang lang;

		protected AbstractSquadScreenViewModule(SquadSlidingScreen screen)
		{
			this.screen = screen;
			this.lang = Service.Get<Lang>();
		}

		public virtual void OnScreenLoaded()
		{
		}

		public virtual void ShowView()
		{
		}

		public virtual void HideView()
		{
		}

		public virtual void RefreshView()
		{
		}

		public virtual void OnDestroyElement()
		{
		}

		public virtual bool IsVisible()
		{
			return false;
		}

		protected internal AbstractSquadScreenViewModule(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractSquadScreenViewModule)GCHandledObjects.GCHandleToObject(instance)).HideView();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractSquadScreenViewModule)GCHandledObjects.GCHandleToObject(instance)).IsVisible());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractSquadScreenViewModule)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractSquadScreenViewModule)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AbstractSquadScreenViewModule)GCHandledObjects.GCHandleToObject(instance)).RefreshView();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractSquadScreenViewModule)GCHandledObjects.GCHandleToObject(instance)).ShowView();
			return -1L;
		}
	}
}
