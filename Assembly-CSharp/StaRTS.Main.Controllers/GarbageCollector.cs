using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StaRTS.Main.Controllers
{
	public class GarbageCollector : MonoBehaviour
	{
		private int frame;

		private Action onCompleteCallback;

		private List<Action> onCompleteCallbacks;

		private bool waiting = true;

		public void RegisterCompleteCallback(Action onComplete)
		{
			if (onComplete != null)
			{
				if (this.onCompleteCallback == null || this.onCompleteCallback == onComplete)
				{
					this.onCompleteCallback = onComplete;
				}
				else
				{
					if (this.onCompleteCallbacks == null)
					{
						this.onCompleteCallbacks = new List<Action>();
					}
					if (this.onCompleteCallbacks.IndexOf(onComplete) < 0)
					{
						this.onCompleteCallbacks.Add(onComplete);
					}
				}
			}
		}

		private void Update()
		{
			if (this.waiting)
			{
				if (!this.CanSufferHitch())
				{
					return;
				}
				this.waiting = false;
			}
			switch (this.frame)
			{
			case 0:
				if (this.IsGameReloading())
				{
					this.Collect();
				}
				else
				{
					Resources.UnloadUnusedAssets();
					this.frame++;
				}
				break;
			case 1:
				Resources.UnloadUnusedAssets();
				break;
			case 2:
				this.Collect();
				break;
			case 3:
				if (this.onCompleteCallback != null)
				{
					this.onCompleteCallback();
					this.onCompleteCallback = null;
				}
				if (this.onCompleteCallbacks != null)
				{
					int i = 0;
					int count = this.onCompleteCallbacks.Count;
					while (i < count)
					{
						this.onCompleteCallbacks[i]();
						this.onCompleteCallbacks[i] = null;
						i++;
					}
					this.onCompleteCallbacks = null;
				}
				UnityEngine.Object.DestroyImmediate(this);
				break;
			default:
				if (Service.IsSet<Logger>())
				{
					Service.Get<Logger>().Error("Illegal garbage collect state");
				}
				break;
			}
			this.frame++;
		}

		private void Collect()
		{
			GC.Collect();
		}

		private bool CanSufferHitch()
		{
			if (this.IsGameReloading())
			{
				return true;
			}
			if (Service.Get<GameStateMachine>().CurrentState is ApplicationLoadState)
			{
				return true;
			}
			WipeCamera wipeCamera = Service.Get<CameraManager>().WipeCamera;
			if (wipeCamera != null && wipeCamera.IsRendering())
			{
				return false;
			}
			if (Service.IsSet<WorldTransitioner>() && Service.Get<WorldTransitioner>().IsTransitioning())
			{
				return true;
			}
			MainCamera mainCamera = Service.Get<CameraManager>().MainCamera;
			return (mainCamera == null || (!mainCamera.IsStillMoving() && !mainCamera.IsStillRotating())) && (!Service.IsSet<UXController>() || Service.Get<UXController>().Intro == null) && (!Service.IsSet<UserInputManager>() || !Service.Get<UserInputManager>().IsPressed());
		}

		private bool IsGameReloading()
		{
			return !Service.IsSet<GameStateMachine>() || !Service.IsSet<CameraManager>();
		}
	}
}
