using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class Engine : MonoBehaviour, IUnitySerializable
	{
		private const string MAIN_SCENE_NAME = "MainScene";

		public const uint SIM_TIME_PER_FRAME = 33u;

		public const float VIEW_PHYSICS_TIME_PER_FRAME = 0.033f;

		public static int NumReloads = 0;

		private bool reloadWait;

		private UserInputManager inputManager;

		private SimTimeEngine simTimeEngine;

		private ViewTimeEngine viewTimeEngine;

		public static List<AssetBundle> bundles = new List<AssetBundle>();

		public bool willReload;

		public bool reloadCalled;

		private bool isPaused;

		private void Start()
		{
			this.isPaused = false;
			base.enabled = false;
			this.ForceGarbageCollection(new Action(this.OnGarbageCollectedStart));
		}

		private void OnGarbageCollectedStart()
		{
			this.CleanupBundles();
			Application.runInBackground = true;
			Application.targetFrameRate = 30;
			Service.Set<Engine>(this);
			this.InitLogger();
			uint monoUsedSize = Profiler.GetMonoUsedSize();
			uint monoHeapSize = Profiler.GetMonoHeapSize();
			Service.Get<StaRTSLogger>().DebugFormat("Managed memory on load: {0:F1}MB/{1:F1}MB", new object[]
			{
				monoUsedSize / 1048576f,
				monoHeapSize / 1048576f
			});
			this.reloadWait = false;
			this.inputManager = new InputManager();
			this.simTimeEngine = new SimTimeEngine(33u);
			this.viewTimeEngine = new ViewTimeEngine(0.033f);
			new MainController();
			base.enabled = true;
		}

		private void CleanupBundles()
		{
			for (int i = 0; i < Engine.bundles.Count; i++)
			{
				Engine.bundles[i].Unload(true);
			}
			Engine.bundles.Clear();
		}

		private void Update()
		{
			if (this.willReload)
			{
				if (!this.reloadCalled)
				{
					this.reloadCalled = true;
					base.StartCoroutine(this.DoReload());
				}
				return;
			}
			this.inputManager.OnUpdate();
			this.simTimeEngine.OnUpdate();
			this.viewTimeEngine.OnUpdate();
		}

		private void InitLogger()
		{
			new StaRTSLogger
			{
				ErrorLevels = (LogLevel)3
			}.Start();
		}

		public void Reload()
		{
			this.willReload = true;
		}

		[IteratorStateMachine(typeof(Engine.<DoReload>d__18))]
		private IEnumerator DoReload()
		{
			yield return new WaitForEndOfFrame();
			Service.Get<StaRTSLogger>().Debug("Reloading the game...");
			Engine.NumReloads++;
			this.inputManager.UnregisterAll();
			this.simTimeEngine.UnregisterAll();
			this.viewTimeEngine.UnregisterAll();
			this.reloadWait = true;
			this.StartCoroutine(this.ReloadNow());
			yield break;
		}

		[IteratorStateMachine(typeof(Engine.<ReloadNow>d__19))]
		private IEnumerator ReloadNow()
		{
			if (this.reloadWait)
			{
				this.reloadWait = false;
				yield return null;
			}
			uint monoUsedSize = Profiler.GetMonoUsedSize();
			uint monoHeapSize = Profiler.GetMonoHeapSize();
			Service.Get<StaRTSLogger>().DebugFormat("Managed memory before reload: {0:F1}MB/{1:F1}MB", new object[]
			{
				monoUsedSize / 1048576f,
				monoHeapSize / 1048576f
			});
			MainController.StaticReset();
			MainController.CleanupReferences();
			Service.ResetAll();
			this.OnGarbageCollectedReload();
			this.willReload = false;
			this.reloadCalled = false;
			yield break;
		}

		private void OnGarbageCollectedReload()
		{
			SceneManager.LoadSceneAsync("MainScene");
		}

		public void ForceGarbageCollection(Action onComplete)
		{
			GarbageCollector garbageCollector = base.gameObject.GetComponent<GarbageCollector>();
			if (garbageCollector == null)
			{
				garbageCollector = base.gameObject.AddComponent<GarbageCollector>();
			}
			garbageCollector.RegisterCompleteCallback(onComplete);
		}

		public void OnApplicationPause(bool paused)
		{
			if (paused == this.isPaused)
			{
				return;
			}
			if (Service.IsSet<EventManager>())
			{
				this.isPaused = paused;
				Service.Get<EventManager>().SendEvent(EventId.ApplicationPauseToggled, paused);
			}
		}

		public void OnApplicationQuit()
		{
			if (Service.IsSet<EventManager>())
			{
				Service.Get<EventManager>().SendEvent(EventId.ApplicationQuit, null);
			}
		}

		public Engine()
		{
		}

		public override void Unity_Serialize(int depth)
		{
			SerializedStateWriter.Instance.WriteBoolean(this.willReload);
			SerializedStateWriter.Instance.Align();
			SerializedStateWriter.Instance.WriteBoolean(this.reloadCalled);
			SerializedStateWriter.Instance.Align();
		}

		public override void Unity_Deserialize(int depth)
		{
			this.willReload = SerializedStateReader.Instance.ReadBoolean();
			SerializedStateReader.Instance.Align();
			this.reloadCalled = SerializedStateReader.Instance.ReadBoolean();
			SerializedStateReader.Instance.Align();
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public unsafe override void Unity_NamedSerialize(int depth)
		{
			ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
			bool arg_1F_1 = this.willReload;
			byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			int var_0_cp_1 = 0;
			arg_1F_0.WriteBoolean(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 5105);
			SerializedNamedStateWriter.Instance.Align();
			SerializedNamedStateWriter.Instance.WriteBoolean(this.reloadCalled, &var_0_cp_0[var_0_cp_1] + 5116);
			SerializedNamedStateWriter.Instance.Align();
		}

		public unsafe override void Unity_NamedDeserialize(int depth)
		{
			ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
			byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			int var_0_cp_1 = 0;
			this.willReload = arg_1A_0.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 5105);
			SerializedNamedStateReader.Instance.Align();
			this.reloadCalled = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 5116);
			SerializedNamedStateReader.Instance.Align();
		}

		protected internal Engine(UIntPtr dummy) : base(dummy)
		{
		}

		public static bool $Get0(object instance)
		{
			return ((Engine)instance).willReload;
		}

		public static void $Set0(object instance, bool value)
		{
			((Engine)instance).willReload = value;
		}

		public static bool $Get1(object instance)
		{
			return ((Engine)instance).reloadCalled;
		}

		public static void $Set1(object instance, bool value)
		{
			((Engine)instance).reloadCalled = value;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).CleanupBundles();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Engine)GCHandledObjects.GCHandleToObject(instance)).DoReload());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).ForceGarbageCollection((Action)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).InitLogger();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).OnApplicationPause(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).OnApplicationQuit();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).OnGarbageCollectedReload();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).OnGarbageCollectedStart();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).Reload();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Engine)GCHandledObjects.GCHandleToObject(instance)).ReloadNow());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((Engine)GCHandledObjects.GCHandleToObject(instance)).Update();
			return -1L;
		}
	}
}
