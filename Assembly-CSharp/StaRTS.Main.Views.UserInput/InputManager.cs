using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UserInput
{
	public class InputManager : UserInputManager
	{
		private const int MAX_FINGERS = 2;

		private const float SCROLL_THRESHOLD = 0.03f;

		private float curScroll;

		public InputManager() : base(2)
		{
		}

		public override void OnUpdate()
		{
			if (this.inited && this.enabled)
			{
				if (Input.touches.Length != 0)
				{
					this.HandleTouchChanges();
					return;
				}
				this.HandleMouseChanges();
				this.HandleScrollWheel();
			}
		}

		private void HandleTouchChanges()
		{
			int i = 0;
			int num = Input.touches.Length;
			while (i < num)
			{
				Touch touch = Input.touches[i];
				int fingerId = touch.fingerId;
				int num2 = this.FindTouch(fingerId);
				switch (touch.phase)
				{
				case TouchPhase.Began:
					if (num2 < 0)
					{
						num2 = this.FindTouch(-1);
						if (num2 >= 0)
						{
							Vector2 position = touch.position;
							UserInputLayer userInputLayer = UserInputLayer.InternalNone;
							GameObject gameObject = null;
							Vector3 zero = Vector3.zero;
							if (base.Raycast(UserInputLayer.UX, position, ref gameObject, ref zero))
							{
								userInputLayer = UserInputLayer.UX;
							}
							else if (base.Raycast(UserInputLayer.World, position, ref gameObject, ref zero))
							{
								userInputLayer = ((gameObject == null) ? UserInputLayer.InternalLowest : UserInputLayer.World);
							}
							this.fingerIds[num2] = fingerId;
							this.lastIsPressed[num2] = true;
							this.lastScreenPosition[num2] = position;
							this.lastLayer[num2] = userInputLayer;
							base.SetLastGameObject(num2, gameObject);
							UserInputLayer lowestLayer = (userInputLayer == UserInputLayer.UX) ? UserInputLayer.Screen : UserInputLayer.InternalLowest;
							base.SendOnPress(num2, gameObject, position, zero, lowestLayer);
						}
					}
					break;
				case TouchPhase.Moved:
				case TouchPhase.Stationary:
					if (num2 >= 0)
					{
						Vector2 position2 = touch.position;
						GameObject target = null;
						Vector3 zero2 = Vector3.zero;
						base.Raycast(UserInputLayer.InternalLowest, position2, ref target, ref zero2);
						target = base.GetLastGameObject(num2);
						this.lastScreenPosition[num2] = position2;
						UserInputLayer lowestLayer2 = (this.lastLayer[num2] == UserInputLayer.UX) ? UserInputLayer.Screen : UserInputLayer.InternalLowest;
						base.SendOnDrag(num2, target, position2, zero2, lowestLayer2);
					}
					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					if (num2 >= 0)
					{
						UserInputLayer lowestLayer3 = (this.lastLayer[num2] == UserInputLayer.UX) ? UserInputLayer.Screen : UserInputLayer.InternalLowest;
						base.ResetTouch(num2);
						base.SendOnRelease(num2, lowestLayer3);
					}
					break;
				}
				i++;
			}
		}

		private int FindTouch(int fingerId)
		{
			for (int i = 0; i < 2; i++)
			{
				if (this.fingerIds[i] == fingerId)
				{
					return i;
				}
			}
			return -1;
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
			UserInputLayer lowestLayer = (userInputLayer == UserInputLayer.UX) ? UserInputLayer.Screen : UserInputLayer.InternalLowest;
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

		protected internal InputManager(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((InputManager)GCHandledObjects.GCHandleToObject(instance)).FindTouch(*(int*)args));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((InputManager)GCHandledObjects.GCHandleToObject(instance)).HandleMouseChanges();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((InputManager)GCHandledObjects.GCHandleToObject(instance)).HandleScrollWheel();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((InputManager)GCHandledObjects.GCHandleToObject(instance)).HandleTouchChanges();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((InputManager)GCHandledObjects.GCHandleToObject(instance)).OnUpdate();
			return -1L;
		}
	}
}
