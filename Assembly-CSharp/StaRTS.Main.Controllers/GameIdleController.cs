using StaRTS.Externals.EnvironmentManager;
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

namespace StaRTS.Main.Controllers
{
	public class GameIdleController : IEventObserver, IUserInputObserver, IViewClockTimeObserver
	{
		public bool Enabled = true;

		private DateTime lastInput;

		private DateTime lastPause;

		public GameIdleController()
		{
			Service.Set<GameIdleController>(this);
			this.lastInput = DateTime.Now;
			Service.Get<EventManager>().RegisterObserver(this, EventId.ApplicationPauseToggled, EventPriority.Default);
			Service.Get<UserInputManager>().RegisterObserver(this, UserInputLayer.Screen);
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
		}

		public void OnViewClockTime(float dt)
		{
			double totalSeconds = (DateTime.Now - this.lastInput).TotalSeconds;
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
					this.lastPause = DateTime.Now;
				}
				else
				{
					Service.Get<EnvironmentController>().SetupAutoRotation();
					double totalSeconds = (DateTime.Now - this.lastPause).TotalSeconds;
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
			this.lastInput = DateTime.Now;
			return EatResponse.NotEaten;
		}

		public EatResponse OnDrag(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			this.lastInput = DateTime.Now;
			return EatResponse.NotEaten;
		}

		public EatResponse OnRelease(int id)
		{
			return EatResponse.NotEaten;
		}

		public EatResponse OnScroll(float delta, Vector2 screenPosition)
		{
			this.lastInput = DateTime.Now;
			return EatResponse.NotEaten;
		}
	}
}
