using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Planets;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UserInput
{
	public class UserInputManager : IEventObserver
	{
		protected List<IUserInputObserver>[] observers;

		protected int[] fingerIds;

		protected bool[] lastIsPressed;

		protected Vector2[] lastScreenPosition;

		protected UserInputLayer[] lastLayer;

		protected Entity[] lastEntity;

		protected Planet[] lastPlanet;

		protected WorldCamera activeWorldCamera;

		protected UXCamera uxCamera;

		protected bool inited;

		protected bool enabled;

		private MutableIterator miter;

		private MutableIterator niter;

		private HashSet<GameObject> clickThrough;

		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		public UserInputManager(int maxFingers)
		{
			Service.Set<UserInputManager>(this);
			int num = 4;
			this.observers = new List<IUserInputObserver>[num];
			for (UserInputLayer userInputLayer = UserInputLayer.InternalLowest; userInputLayer <= UserInputLayer.Screen; userInputLayer++)
			{
				int num2 = userInputLayer - UserInputLayer.InternalLowest;
				this.observers[num2] = new List<IUserInputObserver>();
			}
			this.miter = new MutableIterator();
			this.niter = new MutableIterator();
			this.fingerIds = new int[maxFingers];
			this.lastIsPressed = new bool[maxFingers];
			this.lastScreenPosition = new Vector2[maxFingers];
			this.lastLayer = new UserInputLayer[maxFingers];
			this.lastEntity = new Entity[maxFingers];
			this.lastPlanet = new Planet[maxFingers];
			for (int i = 0; i < maxFingers; i++)
			{
				this.ResetTouch(i);
			}
			this.clickThrough = new HashSet<GameObject>();
			this.inited = false;
			this.enabled = false;
		}

		public void Init()
		{
			this.inited = true;
			CameraManager cameraManager = Service.Get<CameraManager>();
			this.uxCamera = cameraManager.UXCamera;
			this.activeWorldCamera = cameraManager.MainCamera;
			Service.Get<EventManager>().RegisterObserver(this, EventId.DenyUserInput, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ApplicationPauseToggled, EventPriority.Default);
			this.Enable(false);
		}

		protected void ResetTouch(int touchIndex)
		{
			this.fingerIds[touchIndex] = -1;
			this.lastIsPressed[touchIndex] = false;
			this.lastScreenPosition[touchIndex] = -Vector2.one;
			this.lastLayer[touchIndex] = UserInputLayer.InternalNone;
			this.lastEntity[touchIndex] = null;
			this.lastPlanet[touchIndex] = null;
		}

		public void ResetLastScreenPosition()
		{
			int i = 0;
			int num = this.lastScreenPosition.Length;
			while (i < num)
			{
				this.lastScreenPosition[i] = -Vector2.one;
				i++;
			}
		}

		private void ReleaseAllFingers()
		{
			int i = 0;
			int num = this.lastIsPressed.Length;
			while (i < num)
			{
				if (this.lastIsPressed[i])
				{
					this.SendOnRelease(i, UserInputLayer.InternalLowest);
				}
				this.ResetTouch(i);
				i++;
			}
		}

		public bool IsPressed()
		{
			int i = 0;
			int num = this.lastIsPressed.Length;
			while (i < num)
			{
				if (this.lastIsPressed[i])
				{
					return true;
				}
				i++;
			}
			return false;
		}

		protected GameObject GetLastGameObject(int touchIndex)
		{
			Entity entity = this.lastEntity[touchIndex];
			if (entity != null)
			{
				GameObjectViewComponent gameObjectViewComponent = entity.Get<GameObjectViewComponent>();
				if (gameObjectViewComponent != null)
				{
					return gameObjectViewComponent.MainGameObject;
				}
			}
			else
			{
				Planet planet = this.lastPlanet[touchIndex];
				if (planet != null)
				{
					return planet.PlanetGameObject;
				}
			}
			return null;
		}

		protected void SetLastGameObject(int touchIndex, GameObject gameObject)
		{
			Entity entity = null;
			Planet planet = null;
			if (gameObject != null)
			{
				EntityRef component = gameObject.GetComponent<EntityRef>();
				if (component != null)
				{
					entity = component.Entity;
				}
				else
				{
					PlanetRef component2 = gameObject.GetComponent<PlanetRef>();
					if (component2 != null)
					{
						planet = component2.Planet;
					}
				}
			}
			this.lastPlanet[touchIndex] = planet;
			this.lastEntity[touchIndex] = entity;
		}

		public void Enable(bool enable)
		{
			if (enable == this.enabled)
			{
				return;
			}
			if (!enable && this.enabled)
			{
				this.ReleaseAllFingers();
			}
			this.enabled = enable;
			if (this.inited)
			{
				this.uxCamera.ReceiveEvents = this.enabled;
			}
		}

		public void RegisterObserver(IUserInputObserver observer, UserInputLayer layer)
		{
			if (observer == null)
			{
				return;
			}
			int num = layer - UserInputLayer.InternalLowest;
			if (this.observers[num].IndexOf(observer) < 0)
			{
				this.observers[num].Add(observer);
			}
		}

		public void UnregisterObserver(IUserInputObserver observer, UserInputLayer layer)
		{
			int num = layer - UserInputLayer.InternalLowest;
			int num2 = this.observers[num].IndexOf(observer);
			if (num2 >= 0)
			{
				this.observers[num].RemoveAt(num2);
				this.miter.OnRemove(num2);
				this.niter.OnRemove(num2);
			}
		}

		public void UnregisterAll()
		{
			this.Enable(false);
			for (UserInputLayer userInputLayer = UserInputLayer.InternalLowest; userInputLayer <= UserInputLayer.Screen; userInputLayer++)
			{
				int num = userInputLayer - UserInputLayer.InternalLowest;
				this.observers[num] = new List<IUserInputObserver>();
			}
			this.miter.Reset();
			this.niter.Reset();
			this.clickThrough.Clear();
		}

		public void ReleaseSubordinates(IUserInputObserver observer, UserInputLayer layer, int id)
		{
			int num = layer - UserInputLayer.InternalLowest;
			int num2 = this.observers[num].IndexOf(observer);
			if (num2 >= 0)
			{
				bool flag = false;
				this.niter.Init(this.observers[num]);
				this.niter.Index = num2 + 1;
				while (this.niter.Active())
				{
					observer = this.observers[num][this.niter.Index];
					if (observer.OnRelease(id) == EatResponse.Eaten)
					{
						flag = true;
						break;
					}
					this.niter.Next();
				}
				this.niter.Reset();
				num--;
				while (num >= 0 && !flag)
				{
					this.niter.Init(this.observers[num]);
					while (this.niter.Active())
					{
						observer = this.observers[num][this.niter.Index];
						if (observer.OnRelease(id) == EatResponse.Eaten)
						{
							flag = true;
							break;
						}
						this.niter.Next();
					}
					this.niter.Reset();
					num--;
				}
			}
		}

		public virtual void OnUpdate()
		{
		}

		protected void SendOnPress(int touchIndex, GameObject target, Vector2 screenPosition, Vector3 groundPosition, UserInputLayer lowestLayer)
		{
			for (UserInputLayer userInputLayer = UserInputLayer.Screen; userInputLayer >= lowestLayer; userInputLayer--)
			{
				int num = userInputLayer - UserInputLayer.InternalLowest;
				this.miter.Init(this.observers[num]);
				while (this.miter.Active())
				{
					IUserInputObserver userInputObserver = this.observers[num][this.miter.Index];
					if (userInputObserver.OnPress(touchIndex, target, screenPosition, groundPosition) == EatResponse.Eaten)
					{
						this.miter.Reset();
						return;
					}
					this.miter.Next();
				}
				this.miter.Reset();
			}
		}

		protected void SendOnDrag(int touchIndex, GameObject target, Vector2 screenPosition, Vector3 groundPosition, UserInputLayer lowestLayer)
		{
			for (UserInputLayer userInputLayer = UserInputLayer.Screen; userInputLayer >= lowestLayer; userInputLayer--)
			{
				int num = userInputLayer - UserInputLayer.InternalLowest;
				this.miter.Init(this.observers[num]);
				while (this.miter.Active())
				{
					IUserInputObserver userInputObserver = this.observers[num][this.miter.Index];
					if (userInputObserver.OnDrag(touchIndex, target, screenPosition, groundPosition) == EatResponse.Eaten)
					{
						this.miter.Reset();
						return;
					}
					this.miter.Next();
				}
				this.miter.Reset();
			}
		}

		protected void SendOnRelease(int touchIndex, UserInputLayer lowestLayer)
		{
			for (UserInputLayer userInputLayer = UserInputLayer.Screen; userInputLayer >= lowestLayer; userInputLayer--)
			{
				int num = userInputLayer - UserInputLayer.InternalLowest;
				this.miter.Init(this.observers[num]);
				while (this.miter.Active())
				{
					IUserInputObserver userInputObserver = this.observers[num][this.miter.Index];
					if (userInputObserver.OnRelease(touchIndex) == EatResponse.Eaten)
					{
						this.miter.Reset();
						return;
					}
					this.miter.Next();
				}
				this.miter.Reset();
			}
		}

		protected void SendOnScroll(float delta, Vector2 screenPosition, UserInputLayer lowestLayer)
		{
			for (UserInputLayer userInputLayer = UserInputLayer.Screen; userInputLayer >= lowestLayer; userInputLayer--)
			{
				int num = userInputLayer - UserInputLayer.InternalLowest;
				this.miter.Init(this.observers[num]);
				while (this.miter.Active())
				{
					IUserInputObserver userInputObserver = this.observers[num][this.miter.Index];
					if (userInputObserver.OnScroll(delta, screenPosition) == EatResponse.Eaten)
					{
						this.miter.Reset();
						return;
					}
					this.miter.Next();
				}
				this.miter.Reset();
			}
		}

		public void RegisterClickThrough(GameObject gameObject)
		{
			if (gameObject != null && !this.clickThrough.Contains(gameObject))
			{
				this.clickThrough.Add(gameObject);
			}
		}

		public void UnregisterClickThrough(GameObject gameObject)
		{
			if (gameObject != null && this.clickThrough.Contains(gameObject))
			{
				this.clickThrough.Remove(gameObject);
			}
		}

		public void SetActiveWorldCamera(WorldCamera camera)
		{
			this.activeWorldCamera = camera;
		}

		protected bool Raycast(UserInputLayer layer, Vector3 screenPosition, ref GameObject gameObjectHit, ref Vector3 groundPosition)
		{
			bool flag = false;
			switch (layer)
			{
			case UserInputLayer.InternalLowest:
				flag = this.activeWorldCamera.GetGroundPosition(screenPosition, ref groundPosition);
				break;
			case UserInputLayer.World:
				flag = this.RaycastHelper(this.activeWorldCamera, screenPosition, ref gameObjectHit);
				flag = (this.activeWorldCamera.GetGroundPosition(screenPosition, ref groundPosition) | flag);
				break;
			case UserInputLayer.UX:
			{
				GameObject gameObject = gameObjectHit;
				flag = this.RaycastHelper(this.uxCamera, screenPosition, ref gameObject);
				if (flag && this.clickThrough.Contains(gameObject))
				{
					return false;
				}
				if (flag && UnityUtils.ShouldIgnoreUIGameObjectInRaycast(gameObject))
				{
					return false;
				}
				gameObjectHit = gameObject;
				break;
			}
			}
			return flag;
		}

		private bool RaycastHelper(CameraBase camera, Vector3 screenPosition, ref GameObject gameObjectHit)
		{
			if (camera.Camera.enabled)
			{
				Ray ray = camera.ScreenPointToRay(screenPosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, camera.Camera.cullingMask))
				{
					gameObjectHit = raycastHit.collider.gameObject;
					return true;
				}
			}
			return false;
		}

		protected UserInputLayer GetLowestLayer(UserInputLayer thisLayer)
		{
			UserInputLayer result = UserInputLayer.InternalLowest;
			if (thisLayer == UserInputLayer.UX)
			{
				result = thisLayer + 1;
			}
			return result;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.ApplicationPauseToggled)
			{
				if (id == EventId.DenyUserInput && this.enabled)
				{
					this.ReleaseAllFingers();
				}
			}
			else if (this.enabled)
			{
				this.ReleaseAllFingers();
			}
			return EatResponse.NotEaten;
		}

		protected internal UserInputManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).Enable(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).Enabled);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).GetLastGameObject(*(int*)args));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).GetLowestLayer((UserInputLayer)(*(int*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).IsPressed());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).OnUpdate();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).RegisterClickThrough((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).RegisterObserver((IUserInputObserver)GCHandledObjects.GCHandleToObject(*args), (UserInputLayer)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).ReleaseAllFingers();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).ReleaseSubordinates((IUserInputObserver)GCHandledObjects.GCHandleToObject(*args), (UserInputLayer)(*(int*)(args + 1)), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).ResetLastScreenPosition();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).ResetTouch(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).SendOnDrag(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3)), (UserInputLayer)(*(int*)(args + 4)));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).SendOnPress(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3)), (UserInputLayer)(*(int*)(args + 4)));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).SendOnRelease(*(int*)args, (UserInputLayer)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).SendOnScroll(*(float*)args, *(*(IntPtr*)(args + 1)), (UserInputLayer)(*(int*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).SetActiveWorldCamera((WorldCamera)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).SetLastGameObject(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterAll();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterClickThrough((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((UserInputManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterObserver((IUserInputObserver)GCHandledObjects.GCHandleToObject(*args), (UserInputLayer)(*(int*)(args + 1)));
			return -1L;
		}
	}
}
