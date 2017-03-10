using StaRTS.Assets;
using StaRTS.Audio;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Squads;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Tween;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.SquadWar
{
	public class WarBoardViewController : IEventObserver, IUserInputObserver
	{
		public const int WARBOARD_OFFSET = -10000;

		private const float BOARD_MIN_ROTATION_DEFAULT = -7f;

		private const float BOARD_MAX_ROTATION_DEFAULT = 11f;

		private const float BOARD_ROTATION_RATE = 0.05f;

		private const float ENTRY_ROTATION_DURATION = 1.5f;

		private const float CENTERING_DURATION = 0.75f;

		private const float INSTANT_CENTERING_TOLLERANCE = 0.5f;

		private const float CENTERING_ROTATION_OFFSET = -1.72057f;

		private readonly Keyframe[] animationKeys;

		private readonly Keyframe[] easeOutKeys;

		private const float DRAG_FLYOUT_THRESHOLD = 1f;

		private const float INERTIA_MIN = 0.1f;

		private const float INERTIA_SAMPLE_DISTANCE_FACTOR = 1.5f;

		private const float INERTIA_TIME_FACTOR = 0.3f;

		private const float INERTIA_TIME_MAX = 0.4f;

		private const int INVALID_FINGER_ID = -1;

		private const string LOCATOR_PREFIX = "locator";

		private const string LOCATOR_GROUP = "planet_surface";

		public static readonly Vector3 WARBOARD_MASTER_START = new Vector3(-10000f, -10000f, 0f);

		private const string WARBOARD_MASTER_PIVOT_NAME = "MasterWarboardParent";

		private GameObject warboardMaster;

		private Transform warboardMasterTransform;

		private GameObject warBoard;

		private Transform warBoardTransform;

		private AssetHandle warBoardHandle;

		private bool warBoardLoadSequenceFinished;

		private GameObject warboardBackgroundGameObject;

		private Transform warboardBackgroundTransform;

		private AssetHandle warboardBackground;

		private bool warboardBackgroundLoadSequenceFinished;

		private bool warboardAudioFinished;

		private int anchorFingerId;

		private Vector3 anchorGroundPosition;

		private Vector3 anchorLastGroundPosition;

		private float originalRotation;

		private SquadWarFlyout flyout;

		public List<Transform> WarBuildingLocators;

		private Transform locatorGroup;

		private float rotateAmount;

		private float minRotation;

		private float maxRotation;

		private RotateTween rotateTween;

		private Transform centeringTarget;

		private float[] rotationSamples;

		public WarBoardViewController()
		{
			this.animationKeys = new Keyframe[]
			{
				new Keyframe(0f, 0f, 0f, 0f),
				new Keyframe(0.5157318f, 0.4470715f, 2.989397f, 2.989397f),
				new Keyframe(0.6571707f, 0.8017763f, 1.748387f, 1.748387f),
				new Keyframe(1f, 1f, 0f, 0f)
			};
			this.easeOutKeys = new Keyframe[]
			{
				new Keyframe(0f, 0f, 1f, 1f),
				new Keyframe(1f, 1f)
			};
			this.rotationSamples = new float[4];
			base..ctor();
			Service.Set<WarBoardViewController>(this);
			this.anchorFingerId = -1;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.PreloadedAudioSuccess && id != EventId.PreloadedAudioFailure)
			{
				switch (id)
				{
				case EventId.WarBoardParticipantBuildingSelected:
				{
					GameObject gameObject = (GameObject)cookie;
					SquadWarParticipantState participantState = Service.Get<WarBoardBuildingController>().GetParticipantState(gameObject);
					bool flag = this.flyout != null && this.flyout.IsShowingParticipantOptions(participantState);
					if (flag)
					{
						Service.Get<WarBoardBuildingController>().DeselectBuilding();
					}
					else if (this.flyout != null)
					{
						this.flyout.ShowParticipantOptions(gameObject, participantState);
					}
					break;
				}
				case EventId.WarBoardBuffBaseBuildingSelected:
				{
					UXCheckbox uXCheckbox = (UXCheckbox)cookie;
					SquadWarBuffBaseData data = (SquadWarBuffBaseData)uXCheckbox.Tag;
					bool flag2 = this.flyout != null && this.flyout.IsShowingBuffBaseOptions(data);
					if (flag2)
					{
						if (this.flyout != null)
						{
							this.flyout.Hide();
						}
					}
					else
					{
						if (this.flyout != null)
						{
							this.flyout.ShowBuffBaseOptions(uXCheckbox, data);
						}
						Service.Get<WarBoardBuildingController>().DeselectBuilding();
					}
					break;
				}
				case EventId.WarBoardBuildingDeselected:
				{
					GameObject building = (GameObject)cookie;
					SquadWarParticipantState participantState2 = Service.Get<WarBoardBuildingController>().GetParticipantState(building);
					if ((participantState2 == null || this.flyout.IsShowingParticipantOptions(participantState2)) && this.flyout != null)
					{
						this.flyout.Hide();
					}
					break;
				}
				}
			}
			else
			{
				AudioTypeVO audioTypeVO = (AudioTypeVO)cookie;
				if (audioTypeVO != null && audioTypeVO.Uid == "sfx_ui_squadwar_warboard_open")
				{
					this.WarBoardAudioLoadFinished();
					Service.Get<EventManager>().UnregisterObserver(this, EventId.PreloadedAudioSuccess);
					Service.Get<EventManager>().UnregisterObserver(this, EventId.PreloadedAudioFailure);
				}
			}
			return EatResponse.NotEaten;
		}

		public void ShowWarBoard()
		{
			this.anchorFingerId = -1;
			this.warBoardLoadSequenceFinished = false;
			this.warboardBackgroundLoadSequenceFinished = false;
			this.warboardAudioFinished = false;
			this.warboardMaster = new GameObject("MasterWarboardParent");
			this.warboardMasterTransform = this.warboardMaster.transform;
			this.warboardMasterTransform.position = WarBoardViewController.WARBOARD_MASTER_START;
			EventManager eventManager = Service.Get<EventManager>();
			if (this.warBoardHandle == AssetHandle.Invalid)
			{
				WarScheduleVO currentWarScheduleData = Service.Get<SquadController>().WarManager.GetCurrentWarScheduleData();
				PlanetVO planetVO = Service.Get<IDataController>().Get<PlanetVO>(currentWarScheduleData.WarPlanetUid);
				Service.Get<AssetManager>().Load(ref this.warBoardHandle, planetVO.WarBoardAssetName, new AssetSuccessDelegate(this.OnWarBoardLoaded), new AssetFailureDelegate(this.OnWarBoardLoadFail), null);
			}
			if (this.warboardBackground == AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Load(ref this.warboardBackground, "warboard_room_bkg", new AssetSuccessDelegate(this.OnWarboardBackgroundLoaded), null, null);
			}
			if (!Service.Get<AudioManager>().LoadAudio("sfx_ui_squadwar_warboard_open"))
			{
				this.WarBoardAudioLoadFinished();
			}
			else
			{
				eventManager.RegisterObserver(this, EventId.PreloadedAudioSuccess);
				eventManager.RegisterObserver(this, EventId.PreloadedAudioFailure);
			}
			if (this.WarBuildingLocators == null)
			{
				this.WarBuildingLocators = new List<Transform>();
			}
			CameraManager cameraManager = Service.Get<CameraManager>();
			if (!Service.Get<WorldTransitioner>().IsTransitioning())
			{
				cameraManager.WarBoardCamera.Enable();
				cameraManager.WarBoardCamera.GroundOffset = -10000f;
				cameraManager.MainCamera.Disable();
			}
			Service.Get<UserInputManager>().SetActiveWorldCamera(cameraManager.WarBoardCamera);
			eventManager.RegisterObserver(this, EventId.WarBoardParticipantBuildingSelected);
			eventManager.RegisterObserver(this, EventId.WarBoardBuffBaseBuildingSelected);
			eventManager.RegisterObserver(this, EventId.WarBoardBuildingDeselected);
			this.flyout = new SquadWarFlyout();
			this.rotateAmount = 0f;
			this.minRotation = 0f;
		}

		public void HideWarBoard()
		{
			UnityEngine.Object.Destroy(this.warBoard);
			this.warBoard = null;
			this.warBoardTransform = null;
			if (this.warBoardHandle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.warBoardHandle);
				this.warBoardHandle = AssetHandle.Invalid;
			}
			UnityEngine.Object.Destroy(this.warboardBackgroundGameObject);
			this.warboardBackgroundGameObject = null;
			if (this.warboardBackground != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.warboardBackground);
				this.warboardBackground = AssetHandle.Invalid;
			}
			if (this.WarBuildingLocators != null)
			{
				this.WarBuildingLocators.Clear();
				this.WarBuildingLocators = null;
			}
			UnityEngine.Object.Destroy(this.warboardMaster);
			this.warboardMaster = null;
			this.warboardMasterTransform = null;
			CameraManager cameraManager = Service.Get<CameraManager>();
			cameraManager.WarBoardCamera.Disable();
			cameraManager.MainCamera.Enable();
			Service.Get<UserInputManager>().SetActiveWorldCamera(cameraManager.MainCamera);
			Service.Get<UserInputManager>().UnregisterObserver(this, UserInputLayer.InternalLowest);
			Service.Get<WarBoardBuildingController>().DestroyBuildings();
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.WarBoardParticipantBuildingSelected);
			eventManager.UnregisterObserver(this, EventId.WarBoardBuffBaseBuildingSelected);
			eventManager.UnregisterObserver(this, EventId.WarBoardBuildingDeselected);
			this.flyout.Destroy();
			if (this.rotateTween != null)
			{
				this.rotateTween.Destroy();
				this.rotateTween = null;
			}
			this.anchorFingerId = -1;
			Service.Get<EventManager>().SendEvent(EventId.WarBoardDestroyed, null);
		}

		private void OnWarBoardLoaded(object asset, object cookie)
		{
			this.warBoard = (GameObject)asset;
			this.warBoardLoadSequenceFinished = true;
			this.maxRotation = 11f;
			this.minRotation = -7f;
			if (this.warBoard != null)
			{
				this.warBoardTransform = this.warBoard.transform;
				this.warBoardTransform.parent = this.warboardMasterTransform;
				this.warBoardTransform.localPosition = Vector3.zero;
				this.warBoardTransform.localRotation = Quaternion.identity;
				this.warBoard.SetActive(false);
				this.WarBuildingLocators.Clear();
				this.locatorGroup = this.warBoard.transform.FindChild("planet_surface");
				if (this.locatorGroup != null)
				{
					SquadController squadController = Service.Get<SquadController>();
					SquadWarManager warManager = squadController.WarManager;
					int numParticipants = warManager.NumParticipants;
					Transform transform = null;
					for (int i = 0; i < numParticipants; i++)
					{
						string name = "locator" + i.ToString();
						transform = this.locatorGroup.FindChild(name);
						this.WarBuildingLocators.Add(transform);
						if (i == 1)
						{
							this.maxRotation = -transform.localEulerAngles.x + -1.72057f;
						}
					}
					if (transform != null)
					{
						this.minRotation = -transform.localEulerAngles.x + -1.72057f;
					}
					AnimationCurve animationCurve = new AnimationCurve(this.animationKeys);
					if (this.rotateTween != null)
					{
						this.rotateTween.Destroy();
						this.rotateTween = null;
					}
					Service.Get<WarBoardBuildingController>().ShowBuildings(this.WarBuildingLocators);
					Service.Get<UserInputManager>().RegisterObserver(this, UserInputLayer.InternalLowest);
				}
			}
			this.CheckForAllLoadsComplete();
		}

		private void OnRotationUpdate(RotateTween tween)
		{
			bool flag = false;
			float relativeRotation = this.GetRelativeRotation(this.locatorGroup.rotation.eulerAngles.x);
			if (relativeRotation > this.maxRotation)
			{
				flag = true;
				this.locatorGroup.rotation = Quaternion.Euler(new Vector3(this.maxRotation, 0f, 0f));
			}
			else if (relativeRotation < this.minRotation)
			{
				flag = true;
				this.locatorGroup.rotation = Quaternion.Euler(new Vector3(this.minRotation, 0f, 0f));
			}
			if (flag)
			{
				if (this.rotateTween != null)
				{
					this.rotateTween.Destroy();
					this.rotateTween = null;
				}
				else if (tween != null)
				{
					tween.Destroy();
					tween = null;
				}
			}
			this.UpdateSelections();
		}

		private float GetRelativeRotation(float degAngle)
		{
			if (degAngle > 180f)
			{
				degAngle -= 360f;
			}
			return degAngle;
		}

		private void OnWarBoardLoadFail(object cookie)
		{
			this.warBoardLoadSequenceFinished = true;
			this.CheckForAllLoadsComplete();
		}

		private void OnWarboardBackgroundLoaded(object asset, object cookie)
		{
			this.warboardBackgroundGameObject = (GameObject)asset;
			this.warboardBackgroundLoadSequenceFinished = true;
			if (this.warboardBackgroundGameObject != null)
			{
				this.warboardBackgroundGameObject = UnityEngine.Object.Instantiate<GameObject>(this.warboardBackgroundGameObject);
				this.warboardBackgroundTransform = this.warboardBackgroundGameObject.transform;
				this.warboardBackgroundTransform.parent = this.warboardMaster.transform;
				this.warboardBackgroundTransform.rotation = Quaternion.identity;
				this.warboardBackgroundTransform.localPosition = Vector3.zero;
				this.warboardBackgroundTransform.localScale = Vector3.one;
				this.warboardBackgroundGameObject.SetActive(false);
			}
			this.CheckForAllLoadsComplete();
		}

		private void OnWarboardBackgroundLoadFail(object cookie)
		{
			this.warboardBackgroundLoadSequenceFinished = true;
			this.CheckForAllLoadsComplete();
		}

		private void WarBoardAudioLoadFinished()
		{
			this.warboardAudioFinished = true;
			this.CheckForAllLoadsComplete();
		}

		private void CheckForAllLoadsComplete()
		{
			if (!this.warBoardLoadSequenceFinished || !this.warboardBackgroundLoadSequenceFinished || !this.warboardAudioFinished)
			{
				return;
			}
			if (this.warBoard != null)
			{
				this.warBoard.SetActive(true);
			}
			if (this.warboardBackgroundGameObject != null)
			{
				this.warboardBackgroundGameObject.SetActive(true);
			}
			Service.Get<EventManager>().SendEvent(EventId.WarBoardLoadComplete, null);
			this.InitializeWarBoardBuildings();
		}

		private void InitializeWarBoardBuildings()
		{
			SquadWarSquadType currentDisplaySquad = this.GetCurrentDisplaySquad();
			Service.Get<WarBoardBuildingController>().ShowWarBuildings(currentDisplaySquad, true);
		}

		public SquadWarSquadType GetCurrentDisplaySquad()
		{
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			SquadWarStatusType currentStatus = warManager.GetCurrentStatus();
			SquadWarSquadType result = SquadWarSquadType.PLAYER_SQUAD;
			if (currentStatus == SquadWarStatusType.PhasePrep || currentStatus == SquadWarStatusType.PhasePrepGrace)
			{
				result = SquadWarSquadType.PLAYER_SQUAD;
			}
			else if (currentStatus == SquadWarStatusType.PhaseAction || currentStatus == SquadWarStatusType.PhaseActionGrace || currentStatus == SquadWarStatusType.PhaseCooldown)
			{
				result = SquadWarSquadType.OPPONENT_SQUAD;
			}
			return result;
		}

		public void SelectAndCenterOn(string squadMemberId)
		{
			GameObject gameObject = Service.Get<WarBoardBuildingController>().FindBuildingForParticipant(squadMemberId);
			if (gameObject != null)
			{
				this.CenterBoardOn(gameObject.transform);
			}
		}

		private void CenterBoardOn(Transform transform)
		{
			Transform y = this.centeringTarget;
			this.centeringTarget = transform;
			AnimationCurve animationCurve = new AnimationCurve(this.animationKeys);
			float duration = 0.75f;
			Quaternion rotation = this.locatorGroup.rotation;
			Quaternion rotationTarget = this.GetRotationTarget(transform);
			float num = Mathf.Abs(rotation.eulerAngles.x - rotationTarget.eulerAngles.x);
			if (this.centeringTarget == y)
			{
				return;
			}
			if (num < 0.5f)
			{
				Service.Get<WarBoardBuildingController>().DeselectBuilding();
				this.OnCenterComplete(null);
				return;
			}
			Service.Get<WarBoardBuildingController>().DeselectBuilding();
			if (this.rotateTween != null)
			{
				this.rotateTween.Destroy();
				this.rotateTween = null;
			}
			this.rotateTween = new RotateTween(this.locatorGroup, duration, rotation, rotationTarget, animationCurve, new Action<RotateTween>(this.OnCenterComplete), null);
		}

		public Quaternion GetRotationTarget(Transform transform)
		{
			float x = -transform.parent.localEulerAngles.x + -1.72057f;
			return Quaternion.Euler(new Vector3(x, 0f, 0f));
		}

		public void OnCenterComplete(RotateTween tween)
		{
			Service.Get<WarBoardBuildingController>().SelectBuilding(this.centeringTarget.gameObject);
		}

		public EatResponse OnPress(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (this.rotateTween != null)
			{
				this.rotateTween.Destroy();
				this.rotateTween = null;
			}
			if (this.anchorFingerId == -1)
			{
				this.anchorFingerId = id;
				this.anchorGroundPosition = groundPosition;
				this.anchorLastGroundPosition = groundPosition;
				this.originalRotation = this.locatorGroup.eulerAngles.x;
				return EatResponse.Eaten;
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnDrag(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (this.anchorFingerId == id)
			{
				this.anchorGroundPosition = groundPosition;
				this.HandleDrag();
				return EatResponse.Eaten;
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnRelease(int id)
		{
			if (this.anchorFingerId == id)
			{
				this.anchorFingerId = -1;
				this.HandleDrag();
				float num = MathUtils.Sum(this.rotationSamples);
				Array.Clear(this.rotationSamples, 0, this.rotationSamples.Length);
				if (Mathf.Abs(num) > 0.1f)
				{
					if (this.rotateTween != null)
					{
						this.rotateTween.Destroy();
						this.rotateTween = null;
					}
					Quaternion rotation = this.locatorGroup.rotation;
					float x = this.locatorGroup.eulerAngles.x;
					float x2 = x + num * 1.5f;
					Quaternion endRotation = default(Quaternion);
					endRotation.eulerAngles = new Vector3(x2, 0f, 0f);
					float duration = Mathf.Min(Mathf.Abs(num) * 0.3f, 0.4f);
					this.locatorGroup.eulerAngles = new Vector3(this.rotateAmount, 0f, 0f);
					this.rotateTween = new RotateTween(this.locatorGroup, duration, rotation, endRotation, new AnimationCurve(this.easeOutKeys), new Action<RotateTween>(this.OnRotationUpdate), new Action<RotateTween>(this.OnRotationUpdate));
				}
				return EatResponse.Eaten;
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnScroll(float delta, Vector2 screenPosition)
		{
			return EatResponse.NotEaten;
		}

		private void HandleDrag()
		{
			if (this.anchorFingerId == -1)
			{
				return;
			}
			this.MoveWarBoard();
			this.anchorLastGroundPosition = this.anchorGroundPosition;
		}

		private void MoveWarBoard()
		{
			Vector3 vector = this.anchorGroundPosition - this.anchorLastGroundPosition;
			vector *= 0.05f;
			this.RotateWarBoard(vector.z);
		}

		private void RotateWarBoard(float rotation)
		{
			this.SampleRotation(rotation);
			this.rotateAmount = this.GetRelativeRotation(this.locatorGroup.eulerAngles.x);
			this.rotateAmount += rotation;
			this.rotateAmount = Mathf.Clamp(this.rotateAmount, this.minRotation, this.maxRotation);
			this.locatorGroup.eulerAngles = new Vector3(this.rotateAmount, 0f, 0f);
			this.UpdateSelections();
		}

		private void SampleRotation(float newSample)
		{
			for (int i = this.rotationSamples.Length - 1; i > 0; i--)
			{
				this.rotationSamples[i] = this.rotationSamples[i - 1];
			}
			this.rotationSamples[0] = newSample;
		}

		private void UpdateSelections()
		{
			bool flag = this.flyout != null || !this.flyout.Visible;
			float num = Mathf.Abs(this.originalRotation - this.locatorGroup.eulerAngles.x);
			if (flag && num > 1f)
			{
				Service.Get<WarBoardBuildingController>().DeselectBuilding();
			}
		}

		public Quaternion GetPlanetRotation()
		{
			return this.locatorGroup.rotation;
		}

		protected internal WarBoardViewController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).CenterBoardOn((Transform)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).CheckForAllLoadsComplete();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).GetCurrentDisplaySquad());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).GetPlanetRotation());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).GetRelativeRotation(*(float*)args));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).GetRotationTarget((Transform)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).HandleDrag();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).HideWarBoard();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).InitializeWarBoardBuildings();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).MoveWarBoard();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnCenterComplete((RotateTween)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnRelease(*(int*)args));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnRotationUpdate((RotateTween)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args, *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnWarboardBackgroundLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnWarboardBackgroundLoadFail(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnWarBoardLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).OnWarBoardLoadFail(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).RotateWarBoard(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).SampleRotation(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).SelectAndCenterOn(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).ShowWarBoard();
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).UpdateSelections();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((WarBoardViewController)GCHandledObjects.GCHandleToObject(instance)).WarBoardAudioLoadFinished();
			return -1L;
		}
	}
}
