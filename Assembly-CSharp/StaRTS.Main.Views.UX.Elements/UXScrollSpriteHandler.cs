using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXScrollSpriteHandler : IUXScrollSpriteHandler
	{
		private const string SCROLL_ARROW_PREFIX = "Sprite";

		private const string SCROLL_ARROW_UP_POSTFIX = "ScrollUp";

		private const string SCROLL_ARROW_DOWN_POSTFIX = "ScrollDown";

		private const string SCROLL_ARROW_RIGHT_POSTFIX = "ScrollRight";

		private const string SCROLL_ARROW_LEFT_POSTFIX = "ScrollLeft";

		private UXSprite scrollBackSprite;

		private UXSprite scrollForwardSprite;

		public UXScrollSpriteHandler()
		{
		}

		public void InitScrollSprites(UXFactory source, UIScrollView scrollView, float scrollPosition, bool isScrollable)
		{
			if (scrollView != null && source != null)
			{
				string text = "Sprite" + scrollView.name;
				string text2 = text;
				string text3 = text;
				UIScrollView.Movement movement = scrollView.movement;
				if (movement != UIScrollView.Movement.Horizontal)
				{
					if (movement == UIScrollView.Movement.Vertical)
					{
						text2 += "ScrollUp";
						text3 += "ScrollDown";
					}
				}
				else
				{
					text2 += "ScrollLeft";
					text3 += "ScrollRight";
				}
				this.scrollBackSprite = source.GetOptionalElement<UXSprite>(text2);
				this.scrollForwardSprite = source.GetOptionalElement<UXSprite>(text3);
				this.UpdateScrollSprites(scrollView, scrollPosition, isScrollable);
			}
		}

		public void HideScrollSprites()
		{
			if (this.scrollBackSprite != null && this.scrollForwardSprite != null)
			{
				this.scrollBackSprite.Visible = false;
				this.scrollForwardSprite.Visible = false;
			}
		}

		public void UpdateScrollSprites(UIScrollView scrollView, float scrollPosition, bool isScrollable)
		{
			if (scrollView != null && this.scrollBackSprite != null && this.scrollForwardSprite != null)
			{
				if (isScrollable)
				{
					if (scrollPosition == 0f)
					{
						this.scrollBackSprite.Visible = false;
						this.scrollForwardSprite.Visible = true;
						return;
					}
					if (scrollPosition == 1f)
					{
						this.scrollBackSprite.Visible = true;
						this.scrollForwardSprite.Visible = false;
						return;
					}
					this.scrollBackSprite.Visible = true;
					this.scrollForwardSprite.Visible = true;
					return;
				}
				else
				{
					this.scrollBackSprite.Visible = false;
					this.scrollForwardSprite.Visible = false;
				}
			}
		}

		protected internal UXScrollSpriteHandler(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UXScrollSpriteHandler)GCHandledObjects.GCHandleToObject(instance)).HideScrollSprites();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UXScrollSpriteHandler)GCHandledObjects.GCHandleToObject(instance)).InitScrollSprites((UXFactory)GCHandledObjects.GCHandleToObject(*args), (UIScrollView)GCHandledObjects.GCHandleToObject(args[1]), *(float*)(args + 2), *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UXScrollSpriteHandler)GCHandledObjects.GCHandleToObject(instance)).UpdateScrollSprites((UIScrollView)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(sbyte*)(args + 2) != 0);
			return -1L;
		}
	}
}
