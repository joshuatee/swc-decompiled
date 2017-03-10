using StaRTS.Externals.Manimal;
using StaRTS.Main.Models;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class GameIdleController : IUserInputObserver, IEventObserver, IViewClockTimeObserver
	{
		public bool Enabled;

		private DateTime lastInput;

		private DateTime lastPause;

		public GameIdleController()
		{
			this.Enabled = true;
			base..ctor();
			Service.Set<GameIdleController>(this);
			this.lastInput = DateTime.get_Now();
			Service.Get<EventManager>().RegisterObserver(this, EventId.ApplicationPauseToggled, EventPriority.Default);
			Service.Get<UserInputManager>().RegisterObserver(this, UserInputLayer.Screen);
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
		}

		public void OnViewClockTime(float dt)
		{
			double totalSeconds = (DateTime.get_Now() - this.lastInput).get_TotalSeconds();
			if (this.Enabled && totalSeconds > (double)GameConstants.IDLE_RELOAD_TIME && this.CanShowIdleAlert())
			{
				this.ShowIdleAlert();
			}
		}

		private bool CanShowIdleAlert()
		{
			return !GameUtils.IsAppLoading();
		}

		public void ShowIdleAlert()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			string title = Service.Get<Lang>().Get("IDLE_TITLE", new object[0]);
			string message = Service.Get<Lang>().Get("IDLE_MESSAGE", new object[0]);
			AlertScreen.ShowModal(true, title, message, null, null);
			Service.Get<ServerAPI>().Enabled = false;
			Service.Get<EventManager>().SendEvent(EventId.UserIsIdle, null);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ApplicationPauseToggled)
			{
				if ((bool)cookie)
				{
					this.lastPause = DateTime.get_Now();
				}
				else
				{
					double totalSeconds = (DateTime.get_Now() - this.lastPause).get_TotalSeconds();
					if (this.Enabled && totalSeconds > (double)GameConstants.PAUSED_RELOAD_TIME)
					{
						Service.Get<Engine>().Reload();
					}
					else
					{
						Service.Get<EventManager>().SendEvent(EventId.SuccessfullyResumed, null);
					}
				}
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnPress(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			this.lastInput = DateTime.get_Now();
			return EatResponse.NotEaten;
		}

		public EatResponse OnDrag(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			this.lastInput = DateTime.get_Now();
			return EatResponse.NotEaten;
		}

		public EatResponse OnRelease(int id)
		{
			return EatResponse.NotEaten;
		}

		public EatResponse OnScroll(float delta, Vector2 screenPosition)
		{
			this.lastInput = DateTime.get_Now();
			return EatResponse.NotEaten;
		}

		protected internal GameIdleController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameIdleController)GCHandledObjects.GCHandleToObject(instance)).CanShowIdleAlert());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameIdleController)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameIdleController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameIdleController)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameIdleController)GCHandledObjects.GCHandleToObject(instance)).OnRelease(*(int*)args));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameIdleController)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args, *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GameIdleController)GCHandledObjects.GCHandleToObject(instance)).OnViewClockTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((GameIdleController)GCHandledObjects.GCHandleToObject(instance)).ShowIdleAlert();
			return -1L;
		}
	}
}
