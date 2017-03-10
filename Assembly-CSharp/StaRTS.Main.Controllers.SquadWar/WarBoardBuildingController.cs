using StaRTS.Assets;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Squads;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.SquadWar
{
	public class WarBoardBuildingController : IUserInputObserver, IViewFrameTimeObserver
	{
		private Dictionary<SquadWarBoardBuilding, SquadWarParticipantState> participantBuildingsPlayer;

		private Dictionary<SquadWarBoardBuilding, SquadWarParticipantState> participantBuildingsOpponent;

		private List<AssetHandle> assetHandles;

		private List<Material> unsharedMaterials;

		private string empireHQId;

		private string rebelHQId;

		private string smugglerHQId;

		private GameObject pressedBuilding;

		private uint pressTimerId;

		private Vector2 pressScreenPosition;

		private GameObject selectedBuilding;

		private OutlinedAsset outline;

		private bool dragged;

		private const string HOLO_DISTANCE = "_Distance";

		private const string HOLO_OPACITY = "_Opacity";

		private const float HOLO_DISTANCE_VALUE = 139.5f;

		private const float HOLO_OPACITY_DIM_VALUE = 0.3f;

		private const float MIN_SELECTION_X_ROTATION = -7.7f;

		private const float MAX_SELECTION_X_ROTATION = 1.9f;

		private const float FACTORY_OUTPOST_ROTATION_SPEED = 0.5f;

		private const string GAMEOBJECT_PARENT_PREFIX = "WarBoardBuilding_";

		private const int FINGER_ID = 0;

		private const float TIME_TO_SELECT = 1f;

		public FactionType currentFaction
		{
			get;
			private set;
		}

		public SquadWarSquadType currentSquadType
		{
			get;
			private set;
		}

		public WarBoardBuildingController()
		{
			Service.Set<WarBoardBuildingController>(this);
			this.participantBuildingsPlayer = new Dictionary<SquadWarBoardBuilding, SquadWarParticipantState>();
			this.participantBuildingsOpponent = new Dictionary<SquadWarBoardBuilding, SquadWarParticipantState>();
			this.unsharedMaterials = new List<Material>();
		}

		public void ShowBuildings(List<Transform> warBuildingLocators)
		{
			if (this.assetHandles == null)
			{
				this.assetHandles = new List<AssetHandle>();
			}
			if (this.outline == null)
			{
				this.outline = new OutlinedAsset();
			}
			this.pressedBuilding = null;
			this.pressTimerId = 0u;
			this.pressScreenPosition = Vector2.zero;
			this.selectedBuilding = null;
			this.dragged = false;
			this.CacheHQBuildingIds();
			BuildingUpgradeCatalog catalog = Service.Get<BuildingUpgradeCatalog>();
			AssetManager assetManager = Service.Get<AssetManager>();
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			SquadWarSquadData squadData = warManager.GetSquadData(SquadWarSquadType.PLAYER_SQUAD);
			this.AddBuildingsForParticipants(squadData, true, catalog, assetManager, warBuildingLocators);
			SquadWarSquadData squadData2 = warManager.GetSquadData(SquadWarSquadType.OPPONENT_SQUAD);
			this.AddBuildingsForParticipants(squadData2, false, catalog, assetManager, warBuildingLocators);
			Service.Get<UserInputManager>().RegisterObserver(this, UserInputLayer.World);
		}

		public void DestroyBuildings()
		{
			if (this.participantBuildingsPlayer != null)
			{
				foreach (KeyValuePair<SquadWarBoardBuilding, SquadWarParticipantState> current in this.participantBuildingsPlayer)
				{
					current.get_Key().Destroy();
				}
				this.participantBuildingsPlayer.Clear();
			}
			if (this.participantBuildingsOpponent != null)
			{
				foreach (KeyValuePair<SquadWarBoardBuilding, SquadWarParticipantState> current2 in this.participantBuildingsOpponent)
				{
					current2.get_Key().Destroy();
				}
				this.participantBuildingsOpponent.Clear();
			}
			if (this.assetHandles != null)
			{
				AssetManager assetManager = Service.Get<AssetManager>();
				int i = 0;
				int count = this.assetHandles.Count;
				while (i < count)
				{
					assetManager.Unload(this.assetHandles[i]);
					i++;
				}
				this.assetHandles.Clear();
			}
			if (this.unsharedMaterials != null)
			{
				int j = 0;
				int count2 = this.unsharedMaterials.Count;
				while (j < count2)
				{
					UnityUtils.DestroyMaterial(this.unsharedMaterials[j]);
					j++;
				}
				this.unsharedMaterials.Clear();
			}
			if (this.outline != null)
			{
				this.outline.Cleanup();
			}
			this.ResetPressedBuilding();
			this.selectedBuilding = null;
			Service.Get<UserInputManager>().UnregisterObserver(this, UserInputLayer.World);
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public SquadWarParticipantState GetParticipantState(GameObject building)
		{
			Dictionary<SquadWarBoardBuilding, SquadWarParticipantState> currentParticipantBuildingList = this.GetCurrentParticipantBuildingList();
			foreach (KeyValuePair<SquadWarBoardBuilding, SquadWarParticipantState> current in currentParticipantBuildingList)
			{
				if (current.get_Key().Building == building)
				{
					return current.get_Value();
				}
			}
			return null;
		}

		public void DisableBuilding(string squadMemberId)
		{
			this.DisableBuilding(this.FindBuildingForParticipant(squadMemberId));
		}

		public void DisableBuilding(GameObject building)
		{
			if (building != null)
			{
				Renderer componentInChildren = building.GetComponentInChildren<Renderer>();
				if (componentInChildren != null)
				{
					componentInChildren.sharedMaterial.SetFloat("_Opacity", 0.3f);
				}
			}
		}

		public GameObject SelectBuilding(string squadMemberId)
		{
			GameObject gameObject = this.FindBuildingForParticipant(squadMemberId);
			if (gameObject != null)
			{
				this.SelectBuilding(gameObject);
			}
			return gameObject;
		}

		public void DeselectBuilding()
		{
			if (this.selectedBuilding != null)
			{
				this.DeselectSelectedBuilding();
			}
		}

		private Dictionary<SquadWarBoardBuilding, SquadWarParticipantState> GetCurrentParticipantBuildingList()
		{
			if (this.currentSquadType == SquadWarSquadType.PLAYER_SQUAD)
			{
				return this.participantBuildingsPlayer;
			}
			return this.participantBuildingsOpponent;
		}

		public GameObject FindBuildingForParticipant(string squadMemberId)
		{
			Dictionary<SquadWarBoardBuilding, SquadWarParticipantState> currentParticipantBuildingList = this.GetCurrentParticipantBuildingList();
			foreach (KeyValuePair<SquadWarBoardBuilding, SquadWarParticipantState> current in currentParticipantBuildingList)
			{
				if (current.get_Value().SquadMemberId == squadMemberId)
				{
					return current.get_Key().Building;
				}
			}
			return null;
		}

		private void AddBuildingsForParticipants(SquadWarSquadData squadData, bool isForPlayerSquad, BuildingUpgradeCatalog catalog, AssetManager assetManager, List<Transform> warBuildingLocators)
		{
			List<SquadWarParticipantState> list = new List<SquadWarParticipantState>(squadData.Participants);
			list.Sort(new Comparison<SquadWarParticipantState>(this.SortParticipantsAsc));
			string upgradeGroup = (squadData.Faction == FactionType.Empire) ? this.empireHQId : this.rebelHQId;
			bool isEmpire = squadData.Faction == FactionType.Empire;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				BuildingTypeVO byLevel = catalog.GetByLevel(upgradeGroup, list[i].HQLevel);
				if (byLevel != null)
				{
					this.AddBuildingForParticipant(list[i], byLevel, isForPlayerSquad, isEmpire, i, assetManager, warBuildingLocators[i]);
				}
				i++;
			}
		}

		private void AddBuildingForParticipant(SquadWarParticipantState participantState, BuildingTypeVO buildingVO, bool isForPlayerSquad, bool isEmpire, int index, AssetManager assetManager, Transform locationTransform)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "WarBoardBuilding_" + participantState.SquadMemberName;
			SquadWarBoardBuilding key = new SquadWarBoardBuilding(participantState, gameObject, isEmpire);
			if (isForPlayerSquad)
			{
				this.participantBuildingsPlayer.Add(key, participantState);
			}
			else
			{
				this.participantBuildingsOpponent.Add(key, participantState);
			}
			this.LoadAsset(gameObject, locationTransform, buildingVO, assetManager);
		}

		public void ShowWarBuildings(SquadWarSquadType squadType, bool deselectSelectedBuilding)
		{
			bool flag = squadType == SquadWarSquadType.PLAYER_SQUAD;
			Dictionary<SquadWarBoardBuilding, SquadWarParticipantState> dictionary = flag ? this.participantBuildingsPlayer : this.participantBuildingsOpponent;
			if (dictionary != null)
			{
				foreach (KeyValuePair<SquadWarBoardBuilding, SquadWarParticipantState> current in this.participantBuildingsPlayer)
				{
					current.get_Key().ToggleVisibility(flag);
				}
				foreach (KeyValuePair<SquadWarBoardBuilding, SquadWarParticipantState> current2 in this.participantBuildingsOpponent)
				{
					current2.get_Key().ToggleVisibility(!flag);
				}
				this.currentSquadType = squadType;
				if (deselectSelectedBuilding)
				{
					this.DeselectSelectedBuilding();
				}
			}
		}

		public void UpdateVisiblity()
		{
			Dictionary<SquadWarBoardBuilding, SquadWarParticipantState> dictionary = (this.currentSquadType == SquadWarSquadType.PLAYER_SQUAD) ? this.participantBuildingsPlayer : this.participantBuildingsOpponent;
			foreach (KeyValuePair<SquadWarBoardBuilding, SquadWarParticipantState> current in dictionary)
			{
				if (current.get_Key() != null && current.get_Key().PlayerInfo != null)
				{
					current.get_Key().PlayerInfo.UpdateVisibility();
				}
			}
		}

		private void LoadAsset(GameObject parentObject, Transform locator, BuildingTypeVO buildingVO, AssetManager assetManager)
		{
			Transform transform = parentObject.transform;
			transform.parent = locator;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			BoxCollider boxCollider = parentObject.AddComponent<BoxCollider>();
			float x = Units.BoardToWorldZ(buildingVO.SizeY);
			float num = Units.BoardToWorldX(buildingVO.SizeX);
			boxCollider.size = new Vector3(x, num, num);
			AssetHandle item = AssetHandle.Invalid;
			assetManager.Load(ref item, buildingVO.AssetName, new AssetSuccessDelegate(this.OnAssetLoaded), null, transform);
			this.assetHandles.Add(item);
		}

		private void OnAssetLoaded(object asset, object cookie)
		{
			GameObject gameObject = (GameObject)asset;
			Transform parent = (Transform)cookie;
			Transform transform = gameObject.transform;
			transform.parent = parent;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.Euler(0f, -135f, 0f);
			transform.localScale = Vector3.one;
			this.CreateNonSharedMaterials(gameObject);
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				Renderer renderer = componentsInChildren[i];
				if (renderer.sharedMaterial != null)
				{
					Shader shader = Service.Get<AssetManager>().Shaders.GetShader("PL_2Color_Mask_HoloBldg");
					renderer.sharedMaterial.shader = shader;
				}
				i++;
			}
			AssetMeshDataMonoBehaviour component = gameObject.GetComponent<AssetMeshDataMonoBehaviour>();
			if (component != null)
			{
				component.ShadowGameObject.SetActive(false);
			}
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		private void CreateNonSharedMaterials(GameObject assetGameObject)
		{
			MeshRenderer[] componentsInChildren = assetGameObject.GetComponentsInChildren<MeshRenderer>(true);
			SkinnedMeshRenderer[] componentsInChildren2 = assetGameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			Dictionary<Material, Material> dictionary = new Dictionary<Material, Material>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				if (dictionary.ContainsKey(componentsInChildren[i].sharedMaterial))
				{
					componentsInChildren[i].sharedMaterial = dictionary[componentsInChildren[i].sharedMaterial];
				}
				else
				{
					Material sharedMaterial = componentsInChildren[i].sharedMaterial;
					Material material = UnityUtils.CreateMaterial(sharedMaterial.shader);
					material.mainTexture = sharedMaterial.mainTexture;
					material.mainTextureOffset = sharedMaterial.mainTextureOffset;
					material.mainTextureScale = sharedMaterial.mainTextureScale;
					dictionary.Add(componentsInChildren[i].sharedMaterial, material);
					componentsInChildren[i].material = material;
					this.unsharedMaterials.Add(material);
				}
				i++;
			}
			int j = 0;
			int num2 = componentsInChildren2.Length;
			while (j < num2)
			{
				if (dictionary.ContainsKey(componentsInChildren2[j].sharedMaterial))
				{
					componentsInChildren2[j].sharedMaterial = dictionary[componentsInChildren2[j].sharedMaterial];
				}
				else
				{
					Material material2 = UnityUtils.CreateMaterial(componentsInChildren2[j].sharedMaterial.shader);
					material2.CopyPropertiesFromMaterial(componentsInChildren2[j].sharedMaterial);
					dictionary.Add(componentsInChildren2[j].sharedMaterial, material2);
					componentsInChildren2[j].material = material2;
					this.unsharedMaterials.Add(material2);
				}
				j++;
			}
			dictionary.Clear();
		}

		private int SortParticipantsAsc(SquadWarParticipantState a, SquadWarParticipantState b)
		{
			return a.HQLevel - b.HQLevel;
		}

		private int SortBuffBases(SquadWarBuffBaseData a, SquadWarBuffBaseData b)
		{
			return a.BaseLevel - b.BaseLevel;
		}

		private void CacheHQBuildingIds()
		{
			if (this.HaveCachedBuildingIds())
			{
				return;
			}
			IDataController dataController = Service.Get<IDataController>();
			foreach (BuildingTypeVO current in dataController.GetAll<BuildingTypeVO>())
			{
				if (this.HaveCachedBuildingIds())
				{
					break;
				}
				if (current.Type == BuildingType.HQ && current.BuildingRequirement != null)
				{
					if (current.Faction == FactionType.Empire)
					{
						this.empireHQId = current.BuildingID;
					}
					else if (current.Faction == FactionType.Rebel)
					{
						this.rebelHQId = current.BuildingID;
					}
					else if (current.Faction == FactionType.Smuggler)
					{
						this.smugglerHQId = current.BuildingID;
					}
				}
			}
		}

		private bool HaveCachedBuildingIds()
		{
			return this.empireHQId != null && this.rebelHQId != null && this.smugglerHQId != null;
		}

		private void DeselectSelectedBuilding()
		{
			GameObject cookie = this.selectedBuilding;
			this.selectedBuilding = null;
			this.outline.RemoveOutline();
			Service.Get<EventManager>().SendEvent(EventId.WarBoardBuildingDeselected, cookie);
		}

		private bool ShouldParticipantBuildingBeDisabled(SquadWarParticipantState data)
		{
			return data.TurnsLeft <= 0 && data.VictoryPointsLeft <= 0;
		}

		public void SelectBuilding(GameObject building)
		{
			Quaternion rotationTarget = Service.Get<WarBoardViewController>().GetRotationTarget(building.transform);
			Quaternion planetRotation = Service.Get<WarBoardViewController>().GetPlanetRotation();
			float num = rotationTarget.eulerAngles.x - planetRotation.eulerAngles.x;
			if (num < -7.7f || num > 1.9f)
			{
				return;
			}
			SquadWarParticipantState participantState = this.GetParticipantState(building);
			if (participantState == null)
			{
				return;
			}
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			SquadWarStatusType currentStatus = warManager.GetCurrentStatus();
			if (currentStatus == SquadWarStatusType.PhaseCooldown)
			{
				return;
			}
			this.selectedBuilding = building;
			Transform transform = building.transform;
			if (transform.childCount > 0)
			{
				GameObject gameObject = transform.GetChild(0).gameObject;
				this.outline.Init(gameObject, "PL_2Color_Mask_HoloBldg_Outline");
			}
			if (participantState != null)
			{
				Service.Get<EventManager>().SendEvent(EventId.WarBoardParticipantBuildingSelected, building);
			}
		}

		private void OnBuildingPressedTimer(uint id, object cookie)
		{
			if (id == this.pressTimerId && this.pressedBuilding != null)
			{
				if (this.pressedBuilding != this.selectedBuilding)
				{
					this.DeselectSelectedBuilding();
					this.SelectBuilding(this.pressedBuilding);
				}
				this.pressTimerId = 0u;
				this.pressedBuilding = null;
			}
		}

		private void OnBuildingReleased()
		{
			if (this.pressedBuilding == null)
			{
				this.DeselectSelectedBuilding();
			}
			else
			{
				if (this.selectedBuilding != null && this.pressedBuilding != this.selectedBuilding)
				{
					this.DeselectSelectedBuilding();
				}
				this.SelectBuilding(this.pressedBuilding);
			}
			this.ResetPressedBuilding();
		}

		private void ResetPressedBuilding()
		{
			if (this.pressTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.pressTimerId);
				this.pressTimerId = 0u;
			}
			this.pressedBuilding = null;
		}

		public EatResponse OnPress(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (id != 0)
			{
				return EatResponse.NotEaten;
			}
			if (target != null)
			{
				this.pressedBuilding = target;
				this.pressTimerId = Service.Get<ViewTimerManager>().CreateViewTimer(1f, false, new TimerDelegate(this.OnBuildingPressedTimer), null);
			}
			this.pressScreenPosition = screenPosition;
			this.dragged = false;
			return EatResponse.NotEaten;
		}

		public EatResponse OnDrag(int id, GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (id != 0)
			{
				this.ResetPressedBuilding();
				return EatResponse.NotEaten;
			}
			if (!this.dragged && CameraUtils.HasDragged(screenPosition, this.pressScreenPosition))
			{
				this.dragged = true;
				this.ResetPressedBuilding();
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnRelease(int id)
		{
			if (id != 0)
			{
				return EatResponse.NotEaten;
			}
			if (!this.dragged)
			{
				this.OnBuildingReleased();
			}
			return EatResponse.NotEaten;
		}

		public EatResponse OnScroll(float delta, Vector2 screenPosition)
		{
			return EatResponse.NotEaten;
		}

		public void OnViewFrameTime(float dt)
		{
			this.UpdateVisiblity();
		}

		protected internal WarBoardBuildingController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).AddBuildingForParticipant((SquadWarParticipantState)GCHandledObjects.GCHandleToObject(*args), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0, *(int*)(args + 4), (AssetManager)GCHandledObjects.GCHandleToObject(args[5]), (Transform)GCHandledObjects.GCHandleToObject(args[6]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).AddBuildingsForParticipants((SquadWarSquadData)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, (BuildingUpgradeCatalog)GCHandledObjects.GCHandleToObject(args[2]), (AssetManager)GCHandledObjects.GCHandleToObject(args[3]), (List<Transform>)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).CacheHQBuildingIds();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).CreateNonSharedMaterials((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).DeselectBuilding();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).DeselectSelectedBuilding();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).DestroyBuildings();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).DisableBuilding(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).DisableBuilding((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).FindBuildingForParticipant(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).currentFaction);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).currentSquadType);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).GetCurrentParticipantBuildingList());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).GetParticipantState((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).HaveCachedBuildingIds());
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).LoadAsset((GameObject)GCHandledObjects.GCHandleToObject(*args), (Transform)GCHandledObjects.GCHandleToObject(args[1]), (BuildingTypeVO)GCHandledObjects.GCHandleToObject(args[2]), (AssetManager)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).OnAssetLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).OnBuildingReleased();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(int*)args, (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3))));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).OnRelease(*(int*)args));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args, *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).ResetPressedBuilding();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).SelectBuilding(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).SelectBuilding((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).currentFaction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).currentSquadType = (SquadWarSquadType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).ShouldParticipantBuildingBeDisabled((SquadWarParticipantState)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).ShowBuildings((List<Transform>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).ShowWarBuildings((SquadWarSquadType)(*(int*)args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).SortBuffBases((SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(*args), (SquadWarBuffBaseData)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).SortParticipantsAsc((SquadWarParticipantState)GCHandledObjects.GCHandleToObject(*args), (SquadWarParticipantState)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((WarBoardBuildingController)GCHandledObjects.GCHandleToObject(instance)).UpdateVisiblity();
			return -1L;
		}
	}
}
