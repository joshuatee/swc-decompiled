using StaRTS.Assets;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Story
{
	public class HoloCharacter
	{
		private readonly Vector3 HOLO_CONTAINER_POSITION;

		private const float CAM_NEAR_CLIP = -2f;

		private const float CAM_FAR_CLIP = 2f;

		private const float CAM_ORTHOGRAPHIC_SIZE = 1.5f;

		private const string HOLO_PREFIX = "Holo_";

		private const string HOLOCAM_NAME = "HoloCam";

		private const string ASSET_HOLOGRAM = "hologram";

		private const string CHAR_MATERIAL_PARENT = "Char";

		private const string CHAR_MATERIAL_W_MASK = "Char04_Bands";

		private const string TEXTURE_MASK_NAME = "_MaskTex";

		private const string POSITIONER_NAME = "uiAnchor";

		private const string HOLO_AUDIO_ON = "sfx_ui_hologram_on";

		public const string HOLO_AUDIO_OFF = "sfx_ui_hologram_off";

		private const string ANIM_TRIGGER_OFF = "Off";

		private const string ANIM_TRIGGER_TURN_ON = "TurnOn";

		private const string ANIM_TRIGGER_TURN_OFF = "TurnOff";

		private const string ANIM_TRIGGER_TRANS_OUT = "TransOut";

		private const string ANIM_TRIGGER_TRANS_CONTINUE = "TransContinue";

		private const float DELAY_TURN_OFF = 1f;

		private const float DELAY_SHOW_EVENT = 0.7f;

		private const float DELAY_SWITCH = 0.4f;

		private static readonly Vector3 HOLO_OFFSET = new Vector3(0.34f, 1.07f, 0f);

		public HolocommScreen.HoloCallback destructionCallback;

		private GameObject holoAssetPositioner;

		private GameObject holo;

		private Transform holoTransform;

		private AssetHandle holoHandle;

		private AssetHandle charHandle;

		private AssetHandle prevCharHandle;

		private Texture2D holoCharacterTexture;

		private GameObject holoContainer;

		private Camera holoCam;

		private Animator holoAnimator;

		private string characterAssetName;

		private Transform charMaterialParent;

		private AssetManager assetManager;

		private ViewTimerManager viewTimerManager;

		private EventManager eventManager;

		private uint timerId;

		private bool destroyed;

		private bool turnedOn;

		private Vector3 holoPosition;

		public string CharacterId
		{
			get;
			private set;
		}

		public Action OnDoneLoading
		{
			get;
			set;
		}

		public Camera Camera
		{
			get
			{
				return this.holoCam;
			}
		}

		public HoloCharacter(string characterId, Vector3 holoPosition)
		{
			this.HOLO_CONTAINER_POSITION = new Vector3(0f, 2000f, 0f);
			base..ctor();
			this.OnDoneLoading = null;
			this.holoPosition = holoPosition;
			this.destroyed = false;
			this.turnedOn = false;
			this.timerId = 0u;
			this.viewTimerManager = Service.Get<ViewTimerManager>();
			this.eventManager = Service.Get<EventManager>();
			this.assetManager = Service.Get<AssetManager>();
			this.assetManager.Load(ref this.holoHandle, "hologram", new AssetSuccessDelegate(this.OnHologramLoaded), new AssetFailureDelegate(this.OnHologramLoadFailed), null);
			this.LoadCharacter(characterId, false);
		}

		public void ResizeCurrentCharacter()
		{
			if (this.holoContainer == null || this.holoTransform == null || this.holoCharacterTexture == null)
			{
				return;
			}
			Transform transform = this.holoContainer.transform;
			transform.localPosition = this.HOLO_CONTAINER_POSITION;
			Transform transform2 = this.holoCam.transform;
			transform2.parent = transform;
			transform2.localPosition = Vector3.zero;
			transform2.localScale = Vector3.one;
			transform2.localRotation = Quaternion.identity;
			this.holoAssetPositioner.transform.parent = this.holoContainer.transform;
			float num = Math.Min((float)Screen.width / 1920f, (float)Screen.height / 1080f) * 1.7f;
			num = Math.Min(1f, num);
			Vector3 localScale = new Vector3(num, num, 1f);
			this.holoAssetPositioner.transform.localScale = localScale;
			this.holoAssetPositioner.transform.position = this.holoCam.ViewportToWorldPoint(this.holoPosition);
			this.holoTransform.parent = this.holoAssetPositioner.transform;
			this.holoTransform.localScale = Vector3.one;
			this.holoTransform.localRotation = Quaternion.identity;
			this.charMaterialParent = this.holoTransform.FindChild("Char");
			this.holoTransform.transform.localPosition = HoloCharacter.HOLO_OFFSET;
			if (this.holoCharacterTexture != null)
			{
				this.UpdateCharacterMaterials();
			}
		}

		public void ResizeCurrentCharacter(Vector3 holoPosition, int width, int height)
		{
			if (this.holoContainer == null || this.holo == null || this.holoCharacterTexture == null)
			{
				return;
			}
			this.holoPosition = holoPosition;
			Transform transform = this.holoContainer.transform;
			transform.localPosition = this.HOLO_CONTAINER_POSITION;
			this.holoCam = new GameObject("HoloCam").AddComponent<Camera>();
			this.holoCam.clearFlags = CameraClearFlags.Depth;
			this.holoCam.depth = 1f;
			this.holoCam.cullingMask = 2048;
			this.holoCam.orthographic = true;
			this.holoCam.nearClipPlane = -2f;
			this.holoCam.farClipPlane = 2f;
			this.holoCam.orthographicSize = 1.5f;
			Transform transform2 = this.holoCam.transform;
			transform2.parent = transform;
			transform2.localPosition = Vector3.zero;
			transform2.localScale = Vector3.one;
			transform2.localRotation = Quaternion.identity;
			this.holoAssetPositioner.transform.parent = this.holoContainer.transform;
			float num = Math.Min((float)Screen.width / 1920f, (float)Screen.height / 1080f) * 1.7f;
			num = Math.Min(0.84375f, num);
			this.holoAssetPositioner.transform.localScale = new Vector3(num, num, 1f);
			this.holoAssetPositioner.transform.position = this.holoCam.ViewportToWorldPoint(holoPosition);
			Transform transform3 = this.holo.transform;
			transform3.parent = this.holoAssetPositioner.transform;
			transform3.localScale = Vector3.one;
			transform3.localRotation = Quaternion.identity;
			this.charMaterialParent = transform3.FindChild("Char");
			transform3.transform.localPosition = HoloCharacter.HOLO_OFFSET;
		}

		private void OnHologramLoaded(object asset, object cookie)
		{
			this.holoContainer = new GameObject("Holo_" + this.CharacterId);
			Transform transform = this.holoContainer.transform;
			transform.localPosition = this.HOLO_CONTAINER_POSITION;
			this.holoCam = new GameObject("HoloCam").AddComponent<Camera>();
			this.holoCam.clearFlags = CameraClearFlags.Depth;
			this.holoCam.depth = 1f;
			this.holoCam.cullingMask = 2048;
			this.holoCam.orthographic = true;
			this.holoCam.nearClipPlane = -2f;
			this.holoCam.farClipPlane = 2f;
			this.holoCam.orthographicSize = 1.5f;
			Transform transform2 = this.holoCam.transform;
			transform2.parent = transform;
			transform2.localPosition = Vector3.zero;
			transform2.localScale = Vector3.one;
			transform2.localRotation = Quaternion.identity;
			this.holo = (GameObject)asset;
			this.holoAnimator = this.holo.GetComponent<Animator>();
			this.holoAssetPositioner = new GameObject("uiAnchor");
			this.holoTransform = this.holo.transform;
			this.holoAssetPositioner.transform.parent = this.holoContainer.transform;
			float num = Math.Min((float)Screen.width / 1920f, (float)Screen.height / 1080f) * 1.7f;
			num = Math.Min(1f, num);
			this.holoAssetPositioner.transform.localScale = new Vector3(num, num, 1f);
			this.holoAssetPositioner.transform.position = this.holoCam.ViewportToWorldPoint(this.holoPosition);
			this.holoTransform.parent = this.holoAssetPositioner.transform;
			this.holoTransform.localScale = Vector3.one;
			this.holoTransform.localRotation = Quaternion.identity;
			this.charMaterialParent = this.holoTransform.FindChild("Char");
			this.holoTransform.transform.localPosition = HoloCharacter.HOLO_OFFSET;
			if (this.holoCharacterTexture != null)
			{
				this.UpdateCharacterMaterials();
			}
			this.TryAnimateTurnOn();
		}

		private float GetScaleForAspectRatio(float x)
		{
			return 1.565723f + -1.6636014f / (1f + Mathf.Pow(x / 0.6395921f, 0.9056507f));
		}

		private void OnHologramLoadFailed(object cookie)
		{
			Service.Get<StaRTSLogger>().ErrorFormat("Failed to load hologram asset", new object[0]);
			this.eventManager.SendEvent(EventId.ShowHologramComplete, null);
		}

		private void LoadCharacter(string characterId, bool doTransition)
		{
			this.CharacterId = characterId;
			CharacterVO characterVO = Service.Get<IDataController>().Get<CharacterVO>(characterId);
			this.characterAssetName = characterVO.AssetName;
			this.assetManager.Load(ref this.charHandle, this.characterAssetName, new AssetSuccessDelegate(this.OnCharacterLoaded), new AssetFailureDelegate(this.OnCharacterLoadFailed), doTransition);
		}

		private void OnCharacterLoaded(object asset, object cookie)
		{
			this.holoCharacterTexture = (Texture2D)asset;
			if (this.holo == null)
			{
				return;
			}
			bool flag = (bool)cookie;
			if (flag)
			{
				this.holoAnimator.SetTrigger("TransOut");
				this.KillActiveTimer();
				this.timerId = this.viewTimerManager.CreateViewTimer(0.4f, false, new TimerDelegate(this.OnTransitionOutTimer), null);
				return;
			}
			this.UpdateCharacterMaterials();
			this.TryAnimateTurnOn();
		}

		private void CallDoneLoadingCallback()
		{
			if (this.OnDoneLoading != null)
			{
				this.OnDoneLoading.Invoke();
			}
		}

		private void OnTransitionOutTimer(uint timerId, object cookie)
		{
			this.UpdateCharacterMaterials();
			this.holoAnimator.SetTrigger("TransContinue");
		}

		private void UnloadAndResetPrevHandle()
		{
			if (this.prevCharHandle != AssetHandle.Invalid)
			{
				this.assetManager.Unload(this.prevCharHandle);
				this.prevCharHandle = AssetHandle.Invalid;
			}
		}

		private void UpdateCharacterMaterials()
		{
			this.UnloadAndResetPrevHandle();
			this.CallDoneLoadingCallback();
			using (IEnumerator enumerator = this.charMaterialParent.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.get_Current();
					transform.GetComponent<Renderer>().sharedMaterial.mainTexture = this.holoCharacterTexture;
				}
			}
		}

		private void OnCharacterLoadFailed(object cookie)
		{
			Service.Get<StaRTSLogger>().ErrorFormat("Failed to load character asset for hologram: {0}", new object[]
			{
				cookie
			});
			this.holoCharacterTexture = null;
			this.CharacterId = null;
			this.eventManager.SendEvent(EventId.ShowHologramComplete, null);
			this.CallDoneLoadingCallback();
		}

		private void TryAnimateTurnOn()
		{
			if (this.turnedOn || this.holo == null || this.holoCharacterTexture == null)
			{
				return;
			}
			this.eventManager.SendEvent(EventId.HoloEvent, "sfx_ui_hologram_on");
			UnityUtils.SetLayerRecursively(this.holo, 11);
			this.holoAnimator.SetTrigger("TurnOn");
			this.timerId = this.viewTimerManager.CreateViewTimer(0.7f, false, new TimerDelegate(this.SendShowEvent), null);
			this.turnedOn = true;
		}

		private void SendShowEvent(uint timerId, object cookie)
		{
			this.eventManager.SendEvent(EventId.ShowHologramComplete, null);
		}

		public void ChangeCharacter(string characterId)
		{
			if (this.CharacterId == characterId)
			{
				this.CallDoneLoadingCallback();
				return;
			}
			this.DestroyCharacterAssets();
			this.LoadCharacter(characterId, true);
		}

		public void Animate(string animName)
		{
			if (!this.destroyed)
			{
				this.holoAnimator.Play(animName);
			}
		}

		private void AnimateTurnOff()
		{
			this.eventManager.SendEvent(EventId.HoloEvent, "sfx_ui_hologram_off");
			this.KillActiveTimer();
			if (this.holoAnimator != null)
			{
				this.holoAnimator.SetTrigger("TurnOff");
				this.timerId = this.viewTimerManager.CreateViewTimer(1f, false, new TimerDelegate(this.OnTurnOffTimer), null);
				return;
			}
			this.OnTurnOffTimer(0u, null);
		}

		private void OnTurnOffTimer(uint timerId, object cookie)
		{
			if (this.destructionCallback != null)
			{
				this.destructionCallback();
			}
			this.eventManager.SendEvent(EventId.HideHologramComplete, null);
			this.Destroy();
		}

		private void KillActiveTimer()
		{
			if (this.timerId != 0u)
			{
				this.viewTimerManager.KillViewTimer(this.timerId);
				this.timerId = 0u;
			}
		}

		public void CloseAndDestroy(HolocommScreen.HoloCallback callback)
		{
			this.destructionCallback = callback;
			this.AnimateTurnOff();
		}

		public void Destroy()
		{
			if (this.destroyed)
			{
				return;
			}
			this.KillActiveTimer();
			if (this.holo != null)
			{
				UnityEngine.Object.Destroy(this.holo);
				UnityEngine.Object.Destroy(this.holoAssetPositioner);
				UnityEngine.Object.Destroy(this.holoCam.gameObject);
				UnityEngine.Object.Destroy(this.holoContainer);
				this.holo = null;
				this.holoTransform = null;
				this.holoCam = null;
				this.holoAssetPositioner = null;
				this.holoContainer = null;
				this.holoAnimator = null;
			}
			if (this.holoHandle != AssetHandle.Invalid)
			{
				this.assetManager.Unload(this.holoHandle);
				this.holoHandle = AssetHandle.Invalid;
			}
			this.OnDoneLoading = null;
			this.CharacterId = null;
			this.destructionCallback = null;
			this.destroyed = true;
			this.turnedOn = false;
			this.DestroyCharacterAssets();
			this.UnloadAndResetPrevHandle();
		}

		private void DestroyCharacterAssets()
		{
			this.holoCharacterTexture = null;
			if (this.charHandle != AssetHandle.Invalid)
			{
				this.prevCharHandle = this.charHandle;
				this.charHandle = AssetHandle.Invalid;
			}
		}

		protected internal HoloCharacter(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).Animate(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).AnimateTurnOff();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).CallDoneLoadingCallback();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).ChangeCharacter(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).CloseAndDestroy((HolocommScreen.HoloCallback)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).DestroyCharacterAssets();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).Camera);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).CharacterId);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).OnDoneLoading);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).GetScaleForAspectRatio(*(float*)args));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).KillActiveTimer();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).LoadCharacter(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).OnCharacterLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).OnCharacterLoadFailed(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).OnHologramLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).OnHologramLoadFailed(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).ResizeCurrentCharacter();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).ResizeCurrentCharacter(*(*(IntPtr*)args), *(int*)(args + 1), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).CharacterId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).OnDoneLoading = (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).TryAnimateTurnOn();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).UnloadAndResetPrevHandle();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((HoloCharacter)GCHandledObjects.GCHandleToObject(instance)).UpdateCharacterMaterials();
			return -1L;
		}
	}
}
