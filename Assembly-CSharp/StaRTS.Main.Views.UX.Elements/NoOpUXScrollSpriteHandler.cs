using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class NoOpUXScrollSpriteHandler : IUXScrollSpriteHandler
	{
		public NoOpUXScrollSpriteHandler()
		{
		}

		public void InitScrollSprites(UXFactory source, UIScrollView scrollView, float scrollPosition, bool isScrollable)
		{
		}

		public void HideScrollSprites()
		{
		}

		public void UpdateScrollSprites(UIScrollView scrollView, float scrollPosition, bool isScrollable)
		{
		}

		protected internal NoOpUXScrollSpriteHandler(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((NoOpUXScrollSpriteHandler)GCHandledObjects.GCHandleToObject(instance)).HideScrollSprites();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((NoOpUXScrollSpriteHandler)GCHandledObjects.GCHandleToObject(instance)).InitScrollSprites((UXFactory)GCHandledObjects.GCHandleToObject(*args), (UIScrollView)GCHandledObjects.GCHandleToObject(args[1]), *(float*)(args + 2), *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((NoOpUXScrollSpriteHandler)GCHandledObjects.GCHandleToObject(instance)).UpdateScrollSprites((UIScrollView)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(sbyte*)(args + 2) != 0);
			return -1L;
		}
	}
}
