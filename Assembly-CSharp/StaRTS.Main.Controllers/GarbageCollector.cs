using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class GarbageCollector : MonoBehaviour, IUnitySerializable
	{
		private int frame;

		private Action onCompleteCallback;

		private List<Action> onCompleteCallbacks;

		private bool waiting;

		public void RegisterCompleteCallback(Action onComplete)
		{
			if (onComplete != null)
			{
				if (this.onCompleteCallback == null || this.onCompleteCallback == onComplete)
				{
					this.onCompleteCallback = onComplete;
					return;
				}
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
					this.onCompleteCallback.Invoke();
					this.onCompleteCallback = null;
				}
				if (this.onCompleteCallbacks != null)
				{
					int i = 0;
					int count = this.onCompleteCallbacks.Count;
					while (i < count)
					{
						this.onCompleteCallbacks[i].Invoke();
						this.onCompleteCallbacks[i] = null;
						i++;
					}
					this.onCompleteCallbacks = null;
				}
				UnityEngine.Object.DestroyImmediate(this);
				break;
			default:
				if (Service.IsSet<StaRTSLogger>())
				{
					Service.Get<StaRTSLogger>().Error("Illegal garbage collect state");
				}
				break;
			}
			this.frame++;
		}

		private void Collect()
		{
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

		public GarbageCollector()
		{
			this.waiting = true;
			base..ctor();
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

		protected internal GarbageCollector(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GarbageCollector)GCHandledObjects.GCHandleToObject(instance)).CanSufferHitch());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GarbageCollector)GCHandledObjects.GCHandleToObject(instance)).Collect();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GarbageCollector)GCHandledObjects.GCHandleToObject(instance)).IsGameReloading());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((GarbageCollector)GCHandledObjects.GCHandleToObject(instance)).RegisterCompleteCallback((Action)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GarbageCollector)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GarbageCollector)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GarbageCollector)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((GarbageCollector)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((GarbageCollector)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((GarbageCollector)GCHandledObjects.GCHandleToObject(instance)).Update();
			return -1L;
		}
	}
}
