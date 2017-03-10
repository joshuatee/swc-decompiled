using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Planets
{
	public class GalaxyManipulator : IUserInputObserver, IViewFrameTimeObserver
	{
		private const float RESET_PRESSED_ROTATION_THRESHOLD = 0.25f;

		private float movementTime;

		private float movementAmount;

		private int anchorFingerId;

		private Vector3 lastGroundPosition;

		private Vector3 initialPos;

		private Vector2 lastScreenPosition;

		private bool isPressed;

		private bool isMoving;

		private GameObject pressedGameObject;

		public GalaxyManipulator()
		{
			this.movementAmount = 0f;
			this.movementTime = 0f;
			this.anchorFingerId = -1;
			this.isPressed = false;
			this.pressedGameObject = null;
		}

		public void Enable()
		{
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			Service.Get<UserInputManager>().RegisterObserver(this, UserInputLayer.World);
		}

		public void Disable()
		{
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			Service.Get<UserInputManager>().UnregisterObserver(this, UserInputLayer.World);
		}

		public void OnViewFrameTime(float dt)
		{
			if (this.isMoving)
			{
				this.movementTime += dt;
			}
		}

		private MapFinger ChooseFinger(int id)
		{
			if (this.anchorFingerId < 0 || this.anchorFingerId == id)
			{
				this.anchorFingerId = id;
				return MapFinger.Anchor;
			}
			return MapFinger.Unknown;
		}

		private MapFinger TranslateToFinger(int id)
		{
			if (this.anchorFingerId == id)
			{
				return MapFinger.Anchor;
			}
			return MapFinger.Unknown;
		}

		public EatResponse OnPress(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (Service.Get<UserInputInhibitor>().IsDenying())
			{
				this.Reset();
				return EatResponse.NotEaten;
			}
			if (this.ChooseFinger(id) == MapFinger.Anchor)
			{
				if (!this.isPressed)
				{
					this.isPressed = true;
					this.lastScreenPosition = screenPosition;
					this.lastGroundPosition = groundPosition;
					this.initialPos = groundPosition;
					this.pressedGameObject = target;
				}
				return EatResponse.Eaten;
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnDrag(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (Service.Get<UserInputInhibitor>().IsDenying())
			{
				this.Reset();
				return EatResponse.NotEaten;
			}
			if (this.TranslateToFinger(id) == MapFinger.Anchor)
			{
				if (this.isPressed)
				{
					float num = Service.Get<GalaxyViewController>().UpdateGalaxyRotation(groundPosition, this.lastGroundPosition, this.initialPos);
					this.lastGroundPosition = groundPosition;
					this.lastScreenPosition = screenPosition;
					if (Mathf.Abs(num) >= 0.25f)
					{
						this.isMoving = true;
						if ((num > 0f && this.movementAmount < 0f) || (num < 0f && this.movementAmount > 0f))
						{
							this.ResetSwipe();
						}
						this.movementAmount += num;
						this.pressedGameObject = null;
					}
				}
				return EatResponse.Eaten;
			}
			return EatResponse.NotEaten;
		}

		private void ResetSwipe()
		{
			this.movementAmount = 0f;
			this.movementTime = 0f;
		}

		public EatResponse OnRelease(int id)
		{
			if (Service.Get<UserInputInhibitor>().IsDenying())
			{
				this.Reset();
				return EatResponse.NotEaten;
			}
			if (this.TranslateToFinger(id) == MapFinger.Anchor)
			{
				this.HandleRelease();
				return EatResponse.NotEaten;
			}
			return EatResponse.NotEaten;
		}

		private void HandleRelease()
		{
			if (this.pressedGameObject != null && this.pressedGameObject == this.GetCurrentHoveredObject())
			{
				Service.Get<GalaxyViewController>().OnGalaxyObjectClicked(this.pressedGameObject);
			}
			else
			{
				GalaxySwipeType type = GalaxySwipeType.None;
				if (Mathf.Abs(this.movementAmount) >= GameConstants.GALAXY_PLANET_SWIPE_MIN_MOVE && this.movementTime <= GameConstants.GALAXY_PLANET_SWIPE_MAX_TIME)
				{
					type = GalaxySwipeType.SwipeLeft;
					if (this.movementAmount > 0f)
					{
						type = GalaxySwipeType.SwipeRight;
					}
				}
				Service.Get<GalaxyViewController>().OnTouchReleased(type);
			}
			this.Reset();
		}

		private void Reset()
		{
			this.ResetSwipe();
			this.isPressed = false;
			this.isMoving = false;
			this.lastGroundPosition = Vector3.zero;
			this.lastScreenPosition = Vector2.zero;
			this.pressedGameObject = null;
		}

		private GameObject GetCurrentHoveredObject()
		{
			GameObject result = null;
			MainCamera mainCamera = Service.Get<CameraManager>().MainCamera;
			Ray ray = mainCamera.ScreenPointToRay(this.lastScreenPosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, mainCamera.Camera.cullingMask))
			{
				result = raycastHit.collider.gameObject;
			}
			return result;
		}

		public EatResponse OnScroll(float delta, Vector2 screenPosition)
		{
			return EatResponse.Eaten;
		}

		protected internal GalaxyManipulator(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).ChooseFinger(*(int*)args));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).Disable();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).Enable();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).GetCurrentHoveredObject());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).HandleRelease();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).OnRelease(*(int*)args));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args, *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).ResetSwipe();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GalaxyManipulator)GCHandledObjects.GCHandleToObject(instance)).TranslateToFinger(*(int*)args));
		}
	}
}
