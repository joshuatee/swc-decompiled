using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UserInput
{
	public class MouseManager : UserInputManager
	{
		private const int MAX_FINGERS = 1;

		private const float SCROLL_THRESHOLD = 0.03f;

		private float curScroll;

		public MouseManager() : base(1)
		{
		}

		public override void OnUpdate()
		{
			if (this.inited && this.enabled)
			{
				this.HandleMouseChanges();
				this.HandleScrollWheel();
			}
		}

		private void HandleMouseChanges()
		{
			bool mouseButton = Input.GetMouseButton(0);
			Vector3 mousePosition = Input.mousePosition;
			Vector2 vector = new Vector2(mousePosition.x, mousePosition.y);
			UserInputLayer userInputLayer = UserInputLayer.InternalNone;
			GameObject gameObject = null;
			Vector3 zero = Vector3.zero;
			bool flag = this.lastIsPressed[0] != mouseButton;
			bool flag2 = this.lastScreenPosition[0] != vector;
			if (mouseButton | flag2)
			{
				if (mouseButton && !flag)
				{
					if (this.lastLayer[0] != UserInputLayer.UX)
					{
						base.Raycast(UserInputLayer.World, vector, ref gameObject, ref zero);
					}
					userInputLayer = this.lastLayer[0];
					gameObject = base.GetLastGameObject(0);
				}
				else if (base.Raycast(UserInputLayer.UX, vector, ref gameObject, ref zero))
				{
					userInputLayer = UserInputLayer.UX;
				}
				else if (base.Raycast(UserInputLayer.World, vector, ref gameObject, ref zero))
				{
					userInputLayer = ((gameObject == null) ? UserInputLayer.InternalLowest : UserInputLayer.World);
				}
			}
			else
			{
				userInputLayer = this.lastLayer[0];
			}
			this.lastIsPressed[0] = mouseButton;
			this.lastScreenPosition[0] = vector;
			this.lastLayer[0] = userInputLayer;
			base.SetLastGameObject(0, gameObject);
			UserInputLayer lowestLayer = base.GetLowestLayer(userInputLayer);
			if (flag)
			{
				if (mouseButton)
				{
					base.SendOnPress(0, gameObject, vector, zero, lowestLayer);
				}
				else
				{
					base.SendOnRelease(0, lowestLayer);
				}
			}
			if (mouseButton)
			{
				base.SendOnDrag(0, gameObject, vector, zero, lowestLayer);
			}
		}

		private void HandleScrollWheel()
		{
			if (!this.lastIsPressed[0] && this.lastLayer[0] != UserInputLayer.UX && this.lastScreenPosition[0].x >= 0f && this.lastScreenPosition[0].x < (float)Screen.width && this.lastScreenPosition[0].y >= 0f && this.lastScreenPosition[0].y < (float)Screen.height)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis != 0f)
				{
					float num = (float)((axis > 0f) ? 1 : -1);
					this.curScroll += axis;
					if (num * this.curScroll >= 0.03f)
					{
						this.curScroll = 0f;
						base.SendOnScroll(axis, this.lastScreenPosition[0], UserInputLayer.InternalLowest);
					}
				}
			}
		}

		protected internal MouseManager(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((MouseManager)GCHandledObjects.GCHandleToObject(instance)).HandleMouseChanges();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((MouseManager)GCHandledObjects.GCHandleToObject(instance)).HandleScrollWheel();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((MouseManager)GCHandledObjects.GCHandleToObject(instance)).OnUpdate();
			return -1L;
		}
	}
}
