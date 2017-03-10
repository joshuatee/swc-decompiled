using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class AbstractUXListComponent : MonoBehaviour, IEventObserver, IViewFrameTimeObserver, IUnitySerializable
	{
		private IUXScrollSpriteHandler scrollSpriteHandler;

		protected Action reposCallback;

		public UIPanel NGUIPanel
		{
			get;
			set;
		}

		public UIScrollView NGUIScrollView
		{
			get;
			set;
		}

		public UICenterOnChild NGUICenterOnChild
		{
			get;
			set;
		}

		public Vector4 ClipRegion
		{
			get
			{
				if (!(this.NGUIPanel == null))
				{
					return this.NGUIPanel.baseClipRegion;
				}
				return Vector4.zero;
			}
			set
			{
				if (this.NGUIPanel != null)
				{
					this.NGUIPanel.baseClipRegion = value;
				}
			}
		}

		public Action RepositionCallback
		{
			get
			{
				return this.reposCallback;
			}
			set
			{
				this.reposCallback = value;
			}
		}

		public virtual void Init()
		{
			this.scrollSpriteHandler = new NoOpUXScrollSpriteHandler();
			Service.Get<EventManager>().RegisterObserver(this, EventId.AllUXElementsCreated, EventPriority.Default);
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		public virtual void InternalDestroyComponent()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.AllUXElementsCreated);
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public virtual Vector3 GetItemDimension()
		{
			return Vector3.zero;
		}

		public virtual void Scroll(float location)
		{
		}

		public virtual void RepositionItems(bool delayedReposition)
		{
		}

		protected virtual void OnDrag()
		{
		}

		public virtual float GetCurrentScrollPosition(bool softClip)
		{
			return 0f;
		}

		protected void OnReposition()
		{
			if (this.reposCallback != null)
			{
				this.reposCallback.Invoke();
			}
		}

		public bool IsScrollable()
		{
			if (this.NGUIScrollView != null)
			{
				if (!this.NGUIScrollView.disableDragIfFits)
				{
					return true;
				}
				UIPanel panel = this.NGUIScrollView.panel;
				if (panel != null)
				{
					Vector4 finalClipRegion = panel.finalClipRegion;
					Bounds bounds = this.NGUIScrollView.bounds;
					float num = (finalClipRegion.z == 0f) ? ((float)Screen.width) : (finalClipRegion.z * 0.5f);
					float num2 = (finalClipRegion.w == 0f) ? ((float)Screen.height) : (finalClipRegion.w * 0.5f);
					if (this.NGUIScrollView.canMoveHorizontally)
					{
						if (bounds.min.x < finalClipRegion.x - num)
						{
							return true;
						}
						if (bounds.max.x > finalClipRegion.x + num)
						{
							return true;
						}
					}
					if (this.NGUIScrollView.canMoveVertically)
					{
						if (bounds.min.y < finalClipRegion.y - num2)
						{
							return true;
						}
						if (bounds.max.y > finalClipRegion.y + num2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public void InitScrollArrows(UXFactory source)
		{
			this.scrollSpriteHandler.InitScrollSprites(source, this.NGUIScrollView, this.GetCurrentScrollPosition(false), this.IsScrollable());
			Service.Get<EventManager>().UnregisterObserver(this, EventId.AllUXElementsCreated);
		}

		public void HideScrollArrows()
		{
			this.scrollSpriteHandler.HideScrollSprites();
		}

		public virtual void UpdateScrollArrows()
		{
			this.scrollSpriteHandler.UpdateScrollSprites(this.NGUIScrollView, this.GetCurrentScrollPosition(false), this.IsScrollable());
		}

		public virtual void OnViewFrameTime(float dt)
		{
			if (this.NGUIScrollView != null && this.NGUIScrollView.isDragging)
			{
				this.OnDrag();
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.AllUXElementsCreated)
			{
				this.InitScrollArrows((UXFactory)cookie);
			}
			return EatResponse.NotEaten;
		}

		public void ResetScrollViewPosition()
		{
			if (this.NGUIScrollView != null)
			{
				this.NGUIScrollView.ResetPosition();
			}
		}

		public AbstractUXListComponent()
		{
		}

		public override void Unity_Serialize(int depth)
		{
		}

		public override void Unity_Deserialize(int depth)
		{
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public override void Unity_NamedSerialize(int depth)
		{
		}

		public override void Unity_NamedDeserialize(int depth)
		{
		}

		protected internal AbstractUXListComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).ClipRegion);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).NGUICenterOnChild);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIPanel);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIScrollView);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).RepositionCallback);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).GetCurrentScrollPosition(*(sbyte*)args != 0));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).GetItemDimension());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).HideScrollArrows();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).InitScrollArrows((UXFactory)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).InternalDestroyComponent();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).IsScrollable());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).OnDrag();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).OnReposition();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).RepositionItems(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).ResetScrollViewPosition();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).Scroll(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).ClipRegion = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).NGUICenterOnChild = (UICenterOnChild)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIPanel = (UIPanel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).NGUIScrollView = (UIScrollView)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).RepositionCallback = (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((AbstractUXListComponent)GCHandledObjects.GCHandleToObject(instance)).UpdateScrollArrows();
			return -1L;
		}
	}
}
