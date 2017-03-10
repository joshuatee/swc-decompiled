using StaRTS.FX;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World.Deploying
{
	public abstract class AbstractDeployer : IUserInputObserver
	{
		protected const int FINGER_ID = 0;

		private const string SPAWN_EFFECT_ID_PREFIX = "SpawnEffect";

		protected Vector3 currentWorldPosition;

		protected Vector2 pressScreenPosition;

		protected bool dragged;

		protected PlanetView worldView;

		public AbstractDeployer()
		{
			this.Reset();
			this.worldView = Service.Get<WorldInitializer>().View;
		}

		protected virtual bool EnterMode()
		{
			this.Reset();
			Service.Get<UserInputManager>().RegisterObserver(this, UserInputLayer.World);
			return true;
		}

		public virtual void ExitMode()
		{
			this.Reset();
			Service.Get<UserInputManager>().UnregisterObserver(this, UserInputLayer.World);
		}

		private void Reset()
		{
			this.currentWorldPosition = Vector3.zero;
			this.pressScreenPosition = -Vector2.one;
			this.dragged = false;
		}

		public EatResponse OnPress(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (id != 0)
			{
				return EatResponse.NotEaten;
			}
			if (!Service.Get<UserInputInhibitor>().IsDeployAllowed())
			{
				return EatResponse.NotEaten;
			}
			this.currentWorldPosition = groundPosition;
			this.pressScreenPosition = screenPosition;
			this.dragged = false;
			return this.OnPress(target, screenPosition, groundPosition);
		}

		public EatResponse OnDrag(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (id != 0)
			{
				return EatResponse.NotEaten;
			}
			if (!this.dragged && CameraUtils.HasDragged(screenPosition, this.pressScreenPosition))
			{
				this.dragged = true;
			}
			return this.OnDrag(target, screenPosition, groundPosition);
		}

		public EatResponse OnRelease(int id)
		{
			if (id != 0)
			{
				return EatResponse.NotEaten;
			}
			EatResponse result = this.OnRelease();
			this.pressScreenPosition = -Vector2.one;
			return result;
		}

		public EatResponse OnScroll(float delta, Vector2 screenPosition)
		{
			return EatResponse.NotEaten;
		}

		protected void PlaySpawnEffect(SmartEntity entity)
		{
			IDataController dataController = Service.Get<IDataController>();
			TroopTypeVO troopTypeVO = dataController.Get<TroopTypeVO>(entity.TroopComp.TroopType.Uid);
			string spawnEffectUid = troopTypeVO.SpawnEffectUid;
			if (!string.IsNullOrEmpty(spawnEffectUid))
			{
				EffectsTypeVO effectsTypeVO = Service.Get<IDataController>().Get<EffectsTypeVO>(spawnEffectUid);
				Service.Get<FXManager>().CreateAndAttachFXToEntity(entity, effectsTypeVO.AssetName, "SpawnEffect" + entity.ID.ToString(), null, false, Vector3.zero, true);
				Service.Get<BattleController>().CameraShakeObj.Shake(0.5f, 0.25f);
			}
		}

		protected bool IsNotDraggedAndReleasingOwnPress()
		{
			return !this.dragged && this.pressScreenPosition.x >= 0f && this.pressScreenPosition.y >= 0f;
		}

		public abstract EatResponse OnPress(GameObject target, Vector2 screenPosition, Vector3 groundPosition);

		public abstract EatResponse OnDrag(GameObject target, Vector2 screenPosition, Vector3 groundPosition);

		public abstract EatResponse OnRelease();

		protected internal AbstractDeployer(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).EnterMode());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).ExitMode();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).IsNotDraggedAndReleasingOwnPress());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).OnDrag((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).OnPress((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).OnRelease());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).OnRelease(*(int*)args));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args, *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).PlaySpawnEffect((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AbstractDeployer)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}
	}
}
