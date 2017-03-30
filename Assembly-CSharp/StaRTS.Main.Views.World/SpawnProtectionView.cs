using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.GameBoard;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Animation;
using StaRTS.Utils.Animation.Anims;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StaRTS.Main.Views.World
{
	public class SpawnProtectionView : IEventObserver
	{
		public const string MESH_NAME = "SpawnProtectionView";

		private const string TEXTURE_UID = "textureSpawnProtection1";

		private const float TEXTURE_ANIMATION_SPEED = 0.4f;

		private const float FADE_IN_TIME = 1f;

		private const float IDLE_TIME = 3f;

		private const float FADE_OUT_TIME = 1f;

		public const float ELEVATION = 0.005f;

		private bool initialized;

		private bool gameObjectExists;

		private bool gameObjectVisible;

		private GameObject gameObject;

		private Mesh mesh;

		private Texture2D texture;

		private AssetHandle textureHandle;

		private Material material;

		private AnimColor animAlphaIn;

		private AnimColor animAlphaOut;

		private List<BoardCell<Entity>> meshCells;

		private EventManager events;

		public SpawnProtectionView()
		{
			this.events = Service.Get<EventManager>();
			this.events.RegisterObserver(this, EventId.MapDataProcessingStart, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.WorldInTransitionComplete, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.EnterEditMode, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.ExitEditMode, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.TroopNotPlacedInvalidArea, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.UserLiftedBuilding, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.ShieldBorderDestroyed, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.MissionStarted, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.ShieldDisabled, EventPriority.Default);
		}

		private void OnTextureLoaded(object asset, object cookie)
		{
			this.texture = (Texture2D)asset;
			if (this.material != null)
			{
				this.material.mainTexture = this.texture;
				if (this.gameObjectVisible)
				{
					this.DisplaySpawnProtection();
				}
			}
		}

		private void Destroy()
		{
			this.events.UnregisterObserver(this, EventId.MapDataProcessingStart);
			this.events.UnregisterObserver(this, EventId.WorldLoadComplete);
			this.events.UnregisterObserver(this, EventId.WorldInTransitionComplete);
			this.events.UnregisterObserver(this, EventId.TroopNotPlacedInvalidArea);
			this.events.UnregisterObserver(this, EventId.UserLiftedBuilding);
			this.events.UnregisterObserver(this, EventId.EnterEditMode);
			this.events.UnregisterObserver(this, EventId.ExitEditMode);
			this.events.UnregisterObserver(this, EventId.ShieldBorderDestroyed);
			this.events.UnregisterObserver(this, EventId.MissionStarted);
			this.IgnoreBoardChanges();
			if (this.textureHandle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.textureHandle);
				this.textureHandle = AssetHandle.Invalid;
			}
			this.texture = null;
			if (this.material != null)
			{
				UnityUtils.DestroyMaterial(this.material);
				this.material = null;
			}
			this.CleanUp();
		}

		private void CleanUp()
		{
			AnimController animController = Service.Get<AnimController>();
			if (this.animAlphaIn != null)
			{
				animController.CompleteAnim(this.animAlphaIn);
				this.animAlphaIn = null;
			}
			if (this.animAlphaOut != null)
			{
				animController.CompleteAnim(this.animAlphaOut);
				this.animAlphaOut = null;
			}
			if (this.gameObject != null)
			{
				MeshFilter component = this.gameObject.GetComponent<MeshFilter>();
				component.sharedMesh = null;
			}
			if (this.mesh != null)
			{
				UnityUtils.DestroyMesh(this.mesh);
				this.mesh = null;
			}
			if (this.gameObjectExists)
			{
				this.gameObject.SetActive(false);
				this.gameObjectExists = false;
			}
			this.gameObjectVisible = false;
			this.initialized = false;
		}

		private void Initialize()
		{
			if (this.initialized)
			{
				this.CleanUp();
			}
			this.CreateSpawnMesh();
			this.CreateSpawnGameObject();
			this.initialized = true;
		}

		private void ObserveBoardChanges()
		{
			this.events.RegisterObserver(this, EventId.BuildingPlacedOnBoard, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.BuildingMovedOnBoard, EventPriority.Default);
			this.events.RegisterObserver(this, EventId.BuildingRemovedFromBoard, EventPriority.Default);
		}

		private void IgnoreBoardChanges()
		{
			this.events.UnregisterObserver(this, EventId.BuildingPlacedOnBoard);
			this.events.UnregisterObserver(this, EventId.BuildingMovedOnBoard);
			this.events.UnregisterObserver(this, EventId.BuildingRemovedFromBoard);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			switch (id)
			{
			case EventId.MapDataProcessingStart:
				if (!Service.Get<WorldTransitioner>().IsCurrentWorldHome())
				{
					this.IgnoreBoardChanges();
				}
				return EatResponse.NotEaten;
			case EventId.MapDataProcessingEnd:
				IL_1B:
				switch (id)
				{
				case EventId.BuildingPlacedOnBoard:
				case EventId.BuildingMovedOnBoard:
				case EventId.BuildingRemovedFromBoard:
					break;
				default:
					switch (id)
					{
					case EventId.ShieldBorderDestroyed:
					case EventId.ShieldDisabled:
						goto IL_10A;
					case EventId.ShieldStarted:
						IL_47:
						if (id == EventId.EnterEditMode)
						{
							this.ObserveBoardChanges();
							this.Initialize();
							this.DisplaySpawnProtection();
							return EatResponse.NotEaten;
						}
						if (id == EventId.ExitEditMode)
						{
							this.IgnoreBoardChanges();
							return EatResponse.NotEaten;
						}
						if (id == EventId.TroopNotPlacedInvalidArea)
						{
							this.DisplaySpawnProtection();
							return EatResponse.NotEaten;
						}
						if (id == EventId.MissionStarted)
						{
							this.DisplaySpawnProtection();
							return EatResponse.NotEaten;
						}
						if (id != EventId.UserLiftedBuilding)
						{
							return EatResponse.NotEaten;
						}
						this.CleanUp();
						return EatResponse.NotEaten;
					}
					goto IL_47;
				}
				IL_10A:
				this.Initialize();
				this.DisplaySpawnProtection();
				return EatResponse.NotEaten;
			case EventId.WorldLoadComplete:
				if (!Service.Get<WorldTransitioner>().IsCurrentWorldHome())
				{
					this.Initialize();
				}
				return EatResponse.NotEaten;
			case EventId.WorldInTransitionComplete:
				if (!Service.Get<WorldTransitioner>().IsCurrentWorldHome() && Service.Get<BattleController>().GetCurrentBattle() != null)
				{
					CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
					if (currentBattle.IsPvP() || currentBattle.Type == BattleType.PvpAttackSquadWar)
					{
						this.DisplaySpawnProtection();
					}
				}
				return EatResponse.NotEaten;
			}
			goto IL_1B;
		}

		private void CreateSpawnMesh()
		{
			Board<Entity> board = Service.Get<BoardController>().Board;
			if (this.meshCells == null)
			{
				this.meshCells = new List<BoardCell<Entity>>();
			}
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			bool flag = currentState is HomeState || currentState is EditBaseState || currentState is ApplicationLoadState;
			int i = 0;
			int boardSize = board.BoardSize;
			while (i < boardSize)
			{
				for (int j = 0; j < boardSize; j++)
				{
					BoardCell<Entity> cellAt = board.GetCellAt(i, j, true);
					if ((cellAt.Flags & 4u) != 0u || (!flag && (cellAt.Flags & 16u) != 0u))
					{
						this.meshCells.Add(cellAt);
					}
				}
				i++;
			}
			int num = 23;
			int num2 = 47;
			int quadCount = this.meshCells.Count + num2 * 4;
			Vector3[] vertices;
			Vector2[] uv;
			int[] triangles;
			UnityUtils.SetupVerticesForQuads(quadCount, out vertices, out uv, out triangles);
			int num3 = 0;
			int k = 0;
			int count = this.meshCells.Count;
			while (k < count)
			{
				BoardCell<Entity> boardCell = this.meshCells[k];
				this.SetGridQuadVertices(boardCell.X, boardCell.Z, num3++, vertices);
				k++;
			}
			this.meshCells.Clear();
			int num4 = -num - 1;
			int num5 = num;
			int l = 0;
			int num6 = num4;
			int num7 = num5;
			while (l < num2)
			{
				this.SetGridQuadVertices(num4, num6, num3++, vertices);
				this.SetGridQuadVertices(num6, num5, num3++, vertices);
				this.SetGridQuadVertices(num5, num7, num3++, vertices);
				this.SetGridQuadVertices(num7, num4, num3++, vertices);
				l++;
				num6++;
				num7--;
			}
			this.mesh = UnityUtils.CreateMeshWithVertices(vertices, uv, triangles);
		}

		private void SetGridQuadVertices(int x, int z, int index, Vector3[] vertices)
		{
			float num = Units.BoardToWorldX(x);
			float num2 = Units.BoardToWorldZ(z);
			float maxX = num + 3f;
			float maxZ = num2 + 3f;
			UnityUtils.SetQuadVertices(num, num2, maxX, maxZ, index, vertices);
		}

		private void CreateSpawnGameObject()
		{
			MeshRenderer meshRenderer;
			MeshFilter meshFilter;
			if (this.gameObject == null)
			{
				this.gameObject = new GameObject("SpawnProtectionView");
				meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
				meshFilter = this.gameObject.AddComponent<MeshFilter>();
			}
			else
			{
				meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
				meshFilter = this.gameObject.GetComponent<MeshFilter>();
			}
			this.gameObject.SetActive(false);
			this.gameObject.transform.position = new Vector3(0f, 0.005f, 0f);
			this.gameObjectExists = true;
			this.gameObjectVisible = false;
			meshFilter.sharedMesh = this.mesh;
			if (this.material == null)
			{
				string shaderName = "Grid_Protection_PL";
				Shader shader = Service.Get<AssetManager>().Shaders.GetShader(shaderName);
				if (shader == null)
				{
					return;
				}
				this.material = UnityUtils.CreateMaterial(shader);
				meshRenderer.GetComponent<Renderer>().sharedMaterial = this.material;
				if (this.texture != null)
				{
					this.material.mainTexture = this.texture;
				}
				else if (this.textureHandle == AssetHandle.Invalid)
				{
					TextureVO textureVO = Service.Get<IDataController>().Get<TextureVO>("textureSpawnProtection1");
					Service.Get<AssetManager>().Load(ref this.textureHandle, textureVO.AssetName, new AssetSuccessDelegate(this.OnTextureLoaded), null, null);
				}
			}
			this.material.SetFloat("_Speed", 0.4f);
			this.material.color = Color.black;
			this.animAlphaIn = new AnimColor(this.material, 1f, Color.black, Color.white);
			this.animAlphaIn.EaseFunction = new Easing.EasingDelegate(Easing.QuintEaseOut);
			this.animAlphaOut = new AnimColor(this.material, 1f, Color.white, Color.black);
			this.animAlphaOut.Delay = 3f;
			this.animAlphaOut.EaseFunction = new Easing.EasingDelegate(Easing.QuintEaseIn);
			this.animAlphaOut.OnCompleteCallback = new Action<Anim>(this.OnAlphaOutComplete);
		}

		private void DisplaySpawnProtection()
		{
			if (!this.gameObjectExists)
			{
				return;
			}
			this.gameObjectVisible = true;
			if (this.texture == null)
			{
				return;
			}
			AnimController animController = Service.Get<AnimController>();
			this.gameObject.SetActive(true);
			animController.Animate(this.animAlphaIn);
			animController.Animate(this.animAlphaOut);
		}

		private void OnAlphaOutComplete(Anim anim)
		{
			if (this.gameObject != null)
			{
				this.gameObject.SetActive(false);
			}
			this.gameObjectVisible = false;
		}
	}
}
