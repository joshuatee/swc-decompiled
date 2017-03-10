using StaRTS.Assets;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Views.Cameras;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Globalization;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Squads
{
	public abstract class AbstractSquadWarBoardElement : UXFactory, IViewFrameTimeObserver
	{
		private const char DELIMITER = ',';

		private string assetName;

		private AssetHandle assetHandle;

		protected Transform rootTrans;

		protected Transform transformToTrack;

		private Vector3 localOffset;

		protected WarBoardCamera camera;

		public AbstractSquadWarBoardElement(string assetName)
		{
			this.localOffset = new Vector3(0f, 2.5f, 1.5f);
			base..ctor(Service.Get<CameraManager>().UXCamera);
			this.assetName = assetName;
			if (!string.IsNullOrEmpty(GameConstants.WARBOARD_LABEL_OFFSET))
			{
				string[] array = GameConstants.WARBOARD_LABEL_OFFSET.Split(new char[]
				{
					','
				});
				if (array.Length == 3)
				{
					this.localOffset = new Vector3(Convert.ToSingle(array[0], CultureInfo.InvariantCulture), Convert.ToSingle(array[1], CultureInfo.InvariantCulture), Convert.ToSingle(array[2], CultureInfo.InvariantCulture));
				}
			}
			this.assetHandle = AssetHandle.Invalid;
			base.Load(ref this.assetHandle, assetName, new UXFactoryLoadDelegate(this.OnScreenLoaded), new UXFactoryLoadDelegate(this.LoadFailed), null);
			this.camera = Service.Get<CameraManager>().WarBoardCamera;
		}

		protected virtual void OnScreenLoaded(object cookie)
		{
			this.Visible = true;
			this.rootTrans = this.root.transform;
			GameObject worldUIParent = Service.Get<UXController>().WorldUIParent;
			this.rootTrans.parent = worldUIParent.transform;
			this.rootTrans.localRotation = Quaternion.identity;
			this.rootTrans.localScale = Vector3.one;
		}

		private void LoadFailed(object cookie)
		{
			this.Destroy();
		}

		public virtual void OnViewFrameTime(float dt)
		{
			if (this.transformToTrack != null)
			{
				Vector3 b = this.transformToTrack.rotation * this.localOffset;
				Vector3 a = this.camera.WorldPositionToScreenPoint(this.transformToTrack.position + b);
				this.rootTrans.localPosition = a / this.uxCamera.Scale;
			}
		}

		public virtual void Destroy()
		{
			if (this.assetHandle != AssetHandle.Invalid)
			{
				base.Unload(this.assetHandle, this.assetName);
				this.assetHandle = AssetHandle.Invalid;
			}
			base.DestroyFactory();
			this.rootTrans = null;
			this.transformToTrack = null;
			this.camera = null;
		}

		protected internal AbstractSquadWarBoardElement(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractSquadWarBoardElement)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractSquadWarBoardElement)GCHandledObjects.GCHandleToObject(instance)).LoadFailed(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractSquadWarBoardElement)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractSquadWarBoardElement)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}
	}
}
