using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UserInput
{
	public class DefaultBackButtonManager : IBackButtonManager
	{
		public DefaultBackButtonManager()
		{
			Service.Set<IBackButtonManager>(this);
		}

		public void RegisterBackButtonHandler(IBackButtonHandler handler)
		{
		}

		public void UnregisterBackButtonHandler(IBackButtonHandler handler)
		{
		}

		protected internal DefaultBackButtonManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DefaultBackButtonManager)GCHandledObjects.GCHandleToObject(instance)).RegisterBackButtonHandler((IBackButtonHandler)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DefaultBackButtonManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterBackButtonHandler((IBackButtonHandler)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
