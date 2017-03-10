using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class ScreenSizeController : MonoBehaviour, IUnitySerializable
	{
		public delegate void SetLoadingIconGridVisibilityDelegate(bool visible);

		private readonly object lock_;

		private float screenWidth;

		private float screenHeight;

		private const int CAMERA_PRERENDER_CALL_COUNT_MAX = 5;

		private int cameraPreRenderCallCount;

		private bool bNeedCameraReenable;

		private const int WAIT_COUNT_BEFORE_CAMERA_REENABLE = 20;

		private int cameraReenableWaitCount;

		private bool isCameraPreRenderEnabled;

		public bool bNativeRescaleTriggered;

		public bool isEnabled;

		private Dictionary<Camera, bool> cameraStatus;

		private static ScreenSizeController _instance;

		public static ScreenSizeController.SetLoadingIconGridVisibilityDelegate setLoadingIconGridVisibility;

		public float ScreenWidth
		{
			get
			{
				return this.screenWidth;
			}
		}

		public float ScreenHeight
		{
			get
			{
				return this.screenHeight;
			}
		}

		public static ScreenSizeController Instance
		{
			get
			{
				if (ScreenSizeController._instance == null)
				{
					GameObject gameObject = GameObject.Find("ScreenSizeController");
					if (gameObject != null)
					{
						ScreenSizeController._instance = gameObject.GetComponent<ScreenSizeController>();
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				return ScreenSizeController._instance;
			}
		}

		private void Start()
		{
			if (ScreenSizeController._instance != null)
			{
				UnityEngine.Object.DestroyImmediate(base.gameObject);
				return;
			}
			ScreenSizeController.Instance.DummyInit();
			this.screenWidth = (float)Screen.width;
			this.screenHeight = (float)Screen.height;
			this.cameraStatus = new Dictionary<Camera, bool>();
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		public void DummyInit()
		{
		}

		public void TriggerNativeRescale()
		{
			this.bNativeRescaleTriggered = true;
		}

		public void NativeRescaleTriggered()
		{
			if (!this.isCameraPreRenderEnabled)
			{
				if (this.cameraStatus != null)
				{
					this.cameraStatus.Clear();
				}
				Camera[] allCameras = Camera.allCameras;
				Camera[] array = allCameras;
				for (int i = 0; i < array.Length; i++)
				{
					Camera camera = array[i];
					this.cameraStatus.Add(camera, camera.enabled);
					camera.enabled = false;
				}
				this.bNeedCameraReenable = true;
				Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.cameraPreRender));
				this.isCameraPreRenderEnabled = true;
			}
			this.cameraReenableWaitCount = 0;
			this.cameraPreRenderCallCount = 0;
			this.bNativeRescaleTriggered = false;
		}

		private void cameraPreRender(Camera cam)
		{
			this.cameraPreRenderCallCount++;
			if (cam.targetTexture != null && !cam.targetTexture.IsCreated())
			{
				cam.targetTexture.Create();
			}
			if (this.cameraPreRenderCallCount > 5)
			{
				Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.cameraPreRender));
				this.isCameraPreRenderEnabled = false;
				this.cameraPreRenderCallCount = 0;
			}
		}

		[IteratorStateMachine(typeof(ScreenSizeController.<DoRescale>d__24))]
		public IEnumerator DoRescale()
		{
			yield return new WaitForEndOfFrame();
			object obj = this.lock_;
			lock (obj)
			{
				float num = (float)Screen.width;
				float num2 = (float)Screen.height;
				if (this.bNeedCameraReenable)
				{
					if (this.cameraReenableWaitCount > 20)
					{
						this.cameraReenableWaitCount = 0;
						foreach (Camera current in this.cameraStatus.Keys)
						{
							bool enabled = false;
							bool flag2 = this.cameraStatus.TryGetValue(current, out enabled);
							current.enabled = enabled;
						}
						this.bNeedCameraReenable = false;
					}
					else
					{
						this.cameraReenableWaitCount++;
					}
				}
				else if (num2 != this.screenHeight || num != this.screenWidth)
				{
					if (Service.IsSet<CameraManager>() && Service.Get<CameraManager>() != null)
					{
						Service.Get<CameraManager>().CalculateScale(num, num2);
					}
					if (Service.IsSet<EventManager>() && Service.Get<EventManager>() != null)
					{
						Vector2 vector = new Vector2(num, num2);
						Service.Get<EventManager>().SendEvent(EventId.ScreenSizeChanged, vector);
					}
					if (Service.IsSet<GalaxyViewController>())
					{
						GalaxyViewController galaxyViewController = Service.Get<GalaxyViewController>();
						if (galaxyViewController != null)
						{
							galaxyViewController.OnScreenSizeChanged((int)num, (int)num2);
						}
					}
					if (Service.IsSet<ScreenController>() && Service.Get<ScreenController>() != null)
					{
						StoreScreen storeScreen = Service.Get<ScreenController>().GetLastScreen() as StoreScreen;
						if (storeScreen != null)
						{
							storeScreen.RestoreIcons();
						}
					}
					if (Service.IsSet<EventManager>() && Service.Get<EventManager>() != null)
					{
						Service.Get<EventManager>().SendEvent(EventId.ForceGeometryReload, null);
					}
					this.screenWidth = num;
					this.screenHeight = num2;
				}
			}
			yield break;
		}

		public static void ChangeLoadingBarVisibility(bool visible)
		{
			if (ScreenSizeController.setLoadingIconGridVisibility != null)
			{
				ScreenSizeController.setLoadingIconGridVisibility(visible);
			}
		}

		private void Update()
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (this.bNativeRescaleTriggered)
			{
				this.NativeRescaleTriggered();
			}
			if (this.bNeedCameraReenable || this.screenHeight != (float)Screen.height || this.screenWidth != (float)Screen.width)
			{
				base.StartCoroutine(this.DoRescale());
			}
		}

		private void OnApplicationPause(bool paused)
		{
			this.isEnabled = !paused;
		}

		public ScreenSizeController()
		{
			this.lock_ = new object();
			base..ctor();
		}

		public override void Unity_Serialize(int depth)
		{
			SerializedStateWriter.Instance.WriteBoolean(this.bNativeRescaleTriggered);
			SerializedStateWriter.Instance.Align();
			SerializedStateWriter.Instance.WriteBoolean(this.isEnabled);
			SerializedStateWriter.Instance.Align();
		}

		public override void Unity_Deserialize(int depth)
		{
			this.bNativeRescaleTriggered = SerializedStateReader.Instance.ReadBoolean();
			SerializedStateReader.Instance.Align();
			this.isEnabled = SerializedStateReader.Instance.ReadBoolean();
			SerializedStateReader.Instance.Align();
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public unsafe override void Unity_NamedSerialize(int depth)
		{
			ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
			bool arg_1F_1 = this.bNativeRescaleTriggered;
			byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			int var_0_cp_1 = 0;
			arg_1F_0.WriteBoolean(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 5129);
			SerializedNamedStateWriter.Instance.Align();
			SerializedNamedStateWriter.Instance.WriteBoolean(this.isEnabled, &var_0_cp_0[var_0_cp_1] + 5153);
			SerializedNamedStateWriter.Instance.Align();
		}

		public unsafe override void Unity_NamedDeserialize(int depth)
		{
			ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
			byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			int var_0_cp_1 = 0;
			this.bNativeRescaleTriggered = arg_1A_0.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 5129);
			SerializedNamedStateReader.Instance.Align();
			this.isEnabled = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 5153);
			SerializedNamedStateReader.Instance.Align();
		}

		protected internal ScreenSizeController(UIntPtr dummy) : base(dummy)
		{
		}

		public static bool $Get0(object instance)
		{
			return ((ScreenSizeController)instance).bNativeRescaleTriggered;
		}

		public static void $Set0(object instance, bool value)
		{
			((ScreenSizeController)instance).bNativeRescaleTriggered = value;
		}

		public static bool $Get1(object instance)
		{
			return ((ScreenSizeController)instance).isEnabled;
		}

		public static void $Set1(object instance, bool value)
		{
			((ScreenSizeController)instance).isEnabled = value;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).cameraPreRender((Camera)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			ScreenSizeController.ChangeLoadingBarVisibility(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).DoRescale());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).DummyInit();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ScreenSizeController.Instance);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).ScreenHeight);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).ScreenWidth);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).NativeRescaleTriggered();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).OnApplicationPause(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).TriggerNativeRescale();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ScreenSizeController)GCHandledObjects.GCHandleToObject(instance)).Update();
			return -1L;
		}
	}
}
