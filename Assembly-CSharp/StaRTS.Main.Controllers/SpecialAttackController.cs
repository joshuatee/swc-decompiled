using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.DataStructures;
using StaRTS.GameBoard;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class SpecialAttackController : AbstractDeployableController, IEventObserver
	{
		private const string PARAM_SPAWN_POSITION = "spawnposition";

		private const uint SHADOW_LIFE_FACTOR = 3u;

		private const float DETACH_FADE_OUT_DELAY = 1f;

		private const float DETACH_FADE_OUT_TIME = 2f;

		private const string LANDING_EFFECT = "fx_landing_lrg";

		private const string TAKEOFF_EFFECT = "fx_takeoff_lrg";

		private const uint LANDING_EFFECT_DELAY = 3u;

		private const uint TAKEOFF_EFFECT_DELAY = 2u;

		private const uint FINISH_EFFECT_DELAY = 2u;

		private const uint DEFAULT_WORLD_Y_SPAWN_POS = 100u;

		private const string ATTACHMENT_LOCATOR = "locator_attach";

		private Queue<SpecialAttack> specialAttacksDeployed;

		private List<SpecialAttack> specialAttacksExpended;

		private int numAttackScheduled;

		private int numShipsScheduled;

		private int numHoveringInbound;

		private Dictionary<ProjectileTypeVO, int> projectilesInFlight;

		private Dictionary<string, AssetHandle> preloadedAssets;

		private List<GameObject> preloadAssetPool;

		private bool thereAreProjectilesInFlight;

		private float speed;

		private List<uint> simTimers;

		private List<uint> viewTimers;

		private EventManager events;

		private IntPosition boardPosition;

		private ViewFader viewObjectFader;

		public SpecialAttackController()
		{
			Service.Set<SpecialAttackController>(this);
			this.specialAttacksDeployed = new Queue<SpecialAttack>();
			this.specialAttacksExpended = new List<SpecialAttack>();
			this.projectilesInFlight = new Dictionary<ProjectileTypeVO, int>();
			this.preloadedAssets = new Dictionary<string, AssetHandle>();
			this.preloadAssetPool = new List<GameObject>();
			this.events = Service.Get<EventManager>();
			this.simTimers = new List<uint>();
			this.viewTimers = new List<uint>();
			this.viewObjectFader = new ViewFader();
			this.Reset();
		}

		public SpecialAttack DeploySpecialAttack(SpecialAttackTypeVO specialAttackType, TeamType teamType, Vector3 worldPosition)
		{
			this.boardPosition = Units.WorldToBoardIntPosition(worldPosition);
			if ((specialAttackType.IsDropship || specialAttackType.HasDropoff) && teamType == TeamType.Attacker)
			{
				if (Service.Get<ShieldController>().IsTargetCellUnderShield(worldPosition))
				{
					Service.Get<EventManager>().SendEvent(EventId.TroopPlacedInsideShieldError, this.boardPosition);
					return null;
				}
				if (!Service.Get<TroopController>().FindValidDropShipTroopPlacementCell(this.boardPosition, teamType, 1, out this.boardPosition))
				{
					Service.Get<EventManager>().SendEvent(EventId.TroopPlacedInsideBuildingError, this.boardPosition);
					return null;
				}
			}
			BoardCell<Entity> cellAt = Service.Get<BoardController>().Board.GetCellAt(this.boardPosition.x, this.boardPosition.z);
			if (cellAt == null)
			{
				Service.Get<EventManager>().SendEvent(EventId.TroopNotPlacedInvalidArea, this.boardPosition);
				return null;
			}
			base.EnsureBattlePlayState();
			Vector3 targetWorldPos = Units.BoardToWorldVec(this.boardPosition);
			SpecialAttack specialAttack = new SpecialAttack(specialAttackType, teamType, targetWorldPos, cellAt.X, cellAt.Z);
			specialAttack.TargetShield = Service.Get<ShieldController>().GetActiveShieldAffectingBoardPos(specialAttack.TargetBoardX, specialAttack.TargetBoardZ);
			this.specialAttacksDeployed.Enqueue(specialAttack);
			Service.Get<EventManager>().SendEvent(EventId.SpecialAttackSpawned, specialAttack);
			return specialAttack;
		}

		private void OnDelayedLoad(uint id, object cookie)
		{
			this.numShipsScheduled--;
			SpecialAttack specialAttack = cookie as SpecialAttack;
			if (specialAttack == null)
			{
				return;
			}
			string assetName = specialAttack.VO.AssetName;
			WorldPreloadAsset preloadedAsset = Service.Get<WorldPreloader>().GetPreloadedAsset(assetName);
			if (preloadedAsset == null)
			{
				Service.Get<AssetManager>().Load(ref specialAttack.Handle, assetName, new AssetSuccessDelegate(this.OnAssetSuccess), null, specialAttack);
			}
			else
			{
				specialAttack.Handle = preloadedAsset.Handle;
				if (preloadedAsset.GameObj != null)
				{
					this.OnAssetSuccess(preloadedAsset.GameObj, specialAttack);
				}
			}
			uint num = specialAttack.VO.AnimationDelay;
			if (specialAttack.VO.IsDropship || specialAttack.VO.HasDropoff)
			{
				this.numHoveringInbound++;
				this.EnsureShadowAnimator(specialAttack, null);
				specialAttack.ShadowAnimator.PlayShadowAnim(true);
				Service.Get<ViewTimerManager>().CreateViewTimer(3f, false, new TimerDelegate(this.OnDropshipLandedTimer), specialAttack);
				Service.Get<ViewTimerManager>().CreateViewTimer(3u * num, false, new TimerDelegate(this.UnregisterShadowAnimator), specialAttack.ShadowAnimator);
			}
			if (specialAttack.VO.IsDropship)
			{
				this.simTimers.Add(Service.Get<SimTimerManager>().CreateSimTimer(num, false, new TimerDelegate(this.OnSpawnDropshipTroopsTimer), specialAttack));
			}
			else
			{
				uint shotCount = specialAttack.VO.ShotCount;
				int num2 = 0;
				while ((long)num2 < (long)((ulong)shotCount))
				{
					this.simTimers.Add(Service.Get<SimTimerManager>().CreateSimTimer(num, false, new TimerDelegate(this.SpawnProjectile), specialAttack));
					this.numAttackScheduled++;
					num += specialAttack.VO.ShotDelay;
					num2++;
				}
			}
			this.viewTimers.Add(Service.Get<ViewTimerManager>().CreateViewTimer(specialAttack.VO.DestroyDelay, false, new TimerDelegate(this.DestroyFromCookie), specialAttack));
		}

		public void UpdateSpecialAttacks()
		{
			while (this.specialAttacksDeployed.Count > 0)
			{
				SpecialAttack specialAttack = this.specialAttacksDeployed.Dequeue();
				this.specialAttacksExpended.Add(specialAttack);
				this.numShipsScheduled++;
				uint spawnDelay = specialAttack.SpawnDelay;
				this.simTimers.Add(Service.Get<SimTimerManager>().CreateSimTimer(spawnDelay, false, new TimerDelegate(this.OnDelayedLoad), specialAttack));
			}
		}

		private void EnsureShadowAnimator(SpecialAttack specialAttack, GameObject gameObject)
		{
			if (specialAttack.ShadowAnimator == null)
			{
				specialAttack.ShadowAnimator = new ShadowAnim(gameObject);
				return;
			}
			specialAttack.ShadowAnimator.EnsureShadowAnimSetup(gameObject);
		}

		private bool DoesSpecialAttackNeedTakeoffAndLanding(SpecialAttack specialAttack)
		{
			bool result = false;
			if (specialAttack.VO != null && (specialAttack.VO.IsDropship || specialAttack.VO.HasDropoff))
			{
				result = true;
			}
			return result;
		}

		private void OnAssetSuccess(object asset, object cookie)
		{
			SpecialAttack specialAttack = (SpecialAttack)cookie;
			specialAttack.StarshipGameObject = (asset as GameObject);
			this.EnsureShadowAnimator(specialAttack, specialAttack.StarshipGameObject);
			specialAttack.EffectAnimator = new LandingTakeOffEffectAnim(specialAttack.StarshipGameObject);
			AssetManager assetManager = Service.Get<AssetManager>();
			if (this.DoesSpecialAttackNeedTakeoffAndLanding(specialAttack))
			{
				assetManager.Load(ref specialAttack.EffectAnimator.LandingHandle, "fx_landing_lrg", new AssetSuccessDelegate(this.OnLandingFxSuccess), null, specialAttack);
				assetManager.Load(ref specialAttack.EffectAnimator.TakeoffHandle, "fx_takeoff_lrg", new AssetSuccessDelegate(this.OnTakeOffFxSuccess), null, specialAttack);
			}
			if (specialAttack.StarshipGameObject == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(specialAttack.VO.DropoffAttachedAssetName))
			{
				assetManager.Load(ref specialAttack.AttachmentHandle, specialAttack.VO.DropoffAttachedAssetName, new AssetSuccessDelegate(this.OnAttachmentLoadedSuccess), null, specialAttack);
			}
			Rand rand = Service.Get<Rand>();
			float angle = (float)(rand.SimRange(-specialAttack.VO.AngleOfRollVariance, specialAttack.VO.AngleOfRollVariance + 1) + specialAttack.VO.AngleOfRoll);
			AttackFormation attackFormation = specialAttack.VO.AttackFormation;
			float angle2;
			if (attackFormation == AttackFormation.Semicircle)
			{
				int num = Convert.ToInt32((long)((ulong)specialAttack.AttackerIndex * (ulong)((long)(2 * specialAttack.VO.AngleOfAttackVariance) / (long)((ulong)specialAttack.VO.NumberOfAttackers))), CultureInfo.InvariantCulture);
				angle2 = (float)(specialAttack.VO.AngleOfAttack - specialAttack.VO.AngleOfAttackVariance + num);
			}
			else
			{
				angle2 = (float)(rand.SimRange(-specialAttack.VO.AngleOfAttackVariance, specialAttack.VO.AngleOfAttackVariance + 1) + specialAttack.VO.AngleOfAttack);
			}
			specialAttack.StarshipGameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.AngleAxis(angle2, Vector3.up);
			specialAttack.StarshipGameObject.transform.position = new Vector3(specialAttack.TargetWorldPos.x, specialAttack.StarshipGameObject.transform.position.y, specialAttack.TargetWorldPos.z);
			AssetMeshDataMonoBehaviour component = specialAttack.StarshipGameObject.GetComponent<AssetMeshDataMonoBehaviour>();
			if (component != null && component.GunLocatorGameObjects != null && component.GunLocatorGameObjects.Count > 0)
			{
				specialAttack.GunLocators = component.GunLocatorGameObjects;
			}
			else
			{
				Service.Get<StaRTSLogger>().ErrorFormat("SpecialAttack gun locator game objects are not set in meta: {0}", new object[]
				{
					specialAttack.VO.Uid
				});
				specialAttack.GunLocators = new List<GameObject>
				{
					specialAttack.StarshipGameObject
				};
			}
			this.UpdateSpeed(specialAttack);
			specialAttack.StarshipGameObject.GetComponent<Animation>().Play("Fly");
		}

		public void ResetTimers()
		{
			int i = 0;
			int count = this.simTimers.Count;
			while (i < count)
			{
				Service.Get<SimTimerManager>().KillSimTimer(this.simTimers[i]);
				i++;
			}
			int j = 0;
			int count2 = this.viewTimers.Count;
			while (j < count2)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.viewTimers[j]);
				j++;
			}
			this.viewTimers.Clear();
			this.simTimers.Clear();
		}

		public void Reset()
		{
			this.DestroyAll();
			this.specialAttacksDeployed.Clear();
			this.specialAttacksExpended.Clear();
			this.projectilesInFlight.Clear();
			this.thereAreProjectilesInFlight = false;
			this.numAttackScheduled = 0;
			this.numShipsScheduled = 0;
			this.numHoveringInbound = 0;
			this.speed = 1f;
		}

		public void DestroyAll()
		{
			while (this.specialAttacksDeployed.Count > 0)
			{
				this.Destroy(this.specialAttacksDeployed.Dequeue());
			}
			int i = 0;
			int count = this.specialAttacksExpended.Count;
			while (i < count)
			{
				this.Destroy(this.specialAttacksExpended[i]);
				i++;
			}
			this.specialAttacksExpended.Clear();
			this.ResetTimers();
		}

		public void SetSpeed(float speed)
		{
			this.speed = speed;
			foreach (SpecialAttack current in this.specialAttacksDeployed)
			{
				this.UpdateSpeed(current);
			}
			int i = 0;
			int count = this.specialAttacksExpended.Count;
			while (i < count)
			{
				this.UpdateSpeed(this.specialAttacksExpended[i]);
				i++;
			}
		}

		private void OnGameObjectFadeComplete(object fadedObject)
		{
			if (fadedObject != null)
			{
				GameObject obj = (GameObject)fadedObject;
				UnityEngine.Object.Destroy(obj);
			}
		}

		private void DestroyFromCookie(uint id, object cookie)
		{
			SpecialAttack specialAttack = (SpecialAttack)cookie;
			this.Destroy(specialAttack);
			this.specialAttacksExpended.Remove(specialAttack);
			this.viewTimers.Remove(id);
		}

		private void Destroy(SpecialAttack specialAttack)
		{
			AssetManager assetManager = Service.Get<AssetManager>();
			if (specialAttack.StarshipGameObject != null)
			{
				UnityEngine.Object.Destroy(specialAttack.StarshipGameObject);
				specialAttack.StarshipGameObject = null;
			}
			if (specialAttack.StarshipDetachableGameObject != null)
			{
				this.viewObjectFader.FadeOut(specialAttack.StarshipDetachableGameObject, 1f, 2f, null, new FadingDelegate(this.OnGameObjectFadeComplete));
				specialAttack.StarshipDetachableGameObject = null;
			}
			if (specialAttack.Handle != AssetHandle.Invalid)
			{
				assetManager.Unload(specialAttack.Handle);
				specialAttack.Handle = AssetHandle.Invalid;
			}
			if (specialAttack.Handle != AssetHandle.Invalid)
			{
				assetManager.Unload(specialAttack.AttachmentHandle);
				specialAttack.AttachmentHandle = AssetHandle.Invalid;
			}
			if (specialAttack.EffectAnimator != null)
			{
				if (specialAttack.EffectAnimator.LandingEffect != null)
				{
					UnityEngine.Object.Destroy(specialAttack.EffectAnimator.LandingEffect);
					specialAttack.EffectAnimator.LandingEffect = null;
				}
				if (specialAttack.EffectAnimator.TakeOffEffect != null)
				{
					UnityEngine.Object.Destroy(specialAttack.EffectAnimator.TakeOffEffect);
					specialAttack.EffectAnimator.TakeOffEffect = null;
				}
				if (specialAttack.EffectAnimator.LandingHandle != AssetHandle.Invalid)
				{
					assetManager.Unload(specialAttack.EffectAnimator.LandingHandle);
					specialAttack.EffectAnimator.LandingHandle = AssetHandle.Invalid;
				}
				if (specialAttack.EffectAnimator.TakeoffHandle != AssetHandle.Invalid)
				{
					assetManager.Unload(specialAttack.EffectAnimator.TakeoffHandle);
					specialAttack.EffectAnimator.TakeoffHandle = AssetHandle.Invalid;
				}
			}
		}

		private void UpdateSpeed(SpecialAttack specialAttack)
		{
			if (specialAttack.StarshipGameObject == null)
			{
				return;
			}
			using (IEnumerator enumerator = specialAttack.StarshipGameObject.GetComponent<Animation>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AnimationState animationState = (AnimationState)enumerator.get_Current();
					animationState.speed = this.speed;
				}
			}
		}

		private void UnregisterShadowAnimator(uint id, object cookie)
		{
			ShadowAnim shadowAnim = (ShadowAnim)cookie;
			shadowAnim.PlayShadowAnim(false);
		}

		private GameObject IntantiateEffect(object asset)
		{
			GameObject gameObject = (GameObject)asset;
			gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			if (gameObject.GetComponent<ParticleSystem>() != null)
			{
				gameObject.GetComponent<ParticleSystem>().Stop(true);
			}
			return gameObject;
		}

		private void OnLandingFxSuccess(object asset, object cookie)
		{
			GameObject landingEffect = this.IntantiateEffect(asset);
			SpecialAttack specialAttack = (SpecialAttack)cookie;
			specialAttack.EffectAnimator.LandingEffect = landingEffect;
		}

		private void OnAttachmentLoadedSuccess(object asset, object cookie)
		{
			GameObject gameObject = asset as GameObject;
			SpecialAttack specialAttack = (SpecialAttack)cookie;
			specialAttack.StarshipDetachableGameObject = gameObject;
			specialAttack.SetupDetachableShadowAnimator();
			Transform attachTransform = GameUtils.FindAssetMetaDataTransform(specialAttack.StarshipGameObject, "locator_attach");
			this.AttachGameObject(attachTransform, gameObject, true);
			specialAttack.UpdateDetachableShadowAnimator(SpecialAttackDetachableObjectState.Attached);
		}

		private void AttachGameObject(Transform attachTransform, GameObject attachmentObject, bool resetRotation)
		{
			if (attachmentObject == null)
			{
				return;
			}
			Transform transform = attachmentObject.transform;
			transform.parent = attachTransform;
			if (resetRotation)
			{
				transform.localRotation = Quaternion.identity;
			}
			transform.localPosition = Vector3.zero;
		}

		private void OnTakeOffFxSuccess(object asset, object cookie)
		{
			GameObject takeOffEffect = this.IntantiateEffect(asset);
			SpecialAttack specialAttack = (SpecialAttack)cookie;
			specialAttack.EffectAnimator.TakeOffEffect = takeOffEffect;
		}

		private void OnDropshipLandedTimer(uint id, object cookie)
		{
			SpecialAttack specialAttack = (SpecialAttack)cookie;
			if (specialAttack.EffectAnimator == null)
			{
				Service.Get<StaRTSLogger>().WarnFormat("DropShip SpecialAttack EffectAnimator is null: {0}", new object[]
				{
					specialAttack.VO.Uid
				});
				return;
			}
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(specialAttack.EffectAnimator);
			this.PlayParticle(specialAttack.EffectAnimator.LandingEffect);
			Service.Get<ViewTimerManager>().CreateViewTimer(2f, false, new TimerDelegate(this.OnDropshipTakeOffTimer), specialAttack);
			Service.Get<ViewTimerManager>().CreateViewTimer(2f, false, new TimerDelegate(this.OnDropshipFinishedTimer), specialAttack);
		}

		private void OnDropshipTakeOffTimer(uint id, object cookie)
		{
			SpecialAttack specialAttack = (SpecialAttack)cookie;
			GameObject takeOffEffect = specialAttack.EffectAnimator.TakeOffEffect;
			this.PlayParticle(takeOffEffect);
			Service.Get<EventManager>().SendEvent(EventId.SpecialAttackDropshipFlyingAway, specialAttack);
		}

		private void PlayParticle(GameObject effectObject)
		{
			if (effectObject != null)
			{
				ParticleSystem component = effectObject.GetComponent<ParticleSystem>();
				if (component != null)
				{
					component.Play(true);
				}
			}
		}

		private void OnDropshipFinishedTimer(uint id, object cookie)
		{
			SpecialAttack specialAttack = (SpecialAttack)cookie;
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(specialAttack.EffectAnimator);
		}

		private void OnSpawnDropshipTroopsTimer(uint id, object cookie)
		{
			SpecialAttack specialAttack = cookie as SpecialAttack;
			IntPosition spawnPosition = Units.WorldToBoardIntPosition(specialAttack.TargetWorldPos);
			if (Service.Get<GameStateMachine>().CurrentState is BattlePlayState || Service.Get<GameStateMachine>().CurrentState is BattlePlaybackState)
			{
				this.DeployDropShipTroops(spawnPosition, specialAttack.VO.LinkedUnit, specialAttack.VO.UnitCount, specialAttack.TeamType);
			}
		}

		public void PreloadSpecialAttackMiscAssets()
		{
			this.UnloadPreloads();
			IDataController dataController = Service.Get<IDataController>();
			Dictionary<string, int> allPlayerDeployableSpecialAttacks = Service.Get<BattleController>().GetAllPlayerDeployableSpecialAttacks();
			List<string> list = new List<string>();
			if (allPlayerDeployableSpecialAttacks == null || allPlayerDeployableSpecialAttacks.Count == 0)
			{
				return;
			}
			foreach (string current in allPlayerDeployableSpecialAttacks.Keys)
			{
				SpecialAttackTypeVO specialAttackTypeVO = dataController.Get<SpecialAttackTypeVO>(current);
				string dropoffAttachedAssetName = specialAttackTypeVO.DropoffAttachedAssetName;
				if (!string.IsNullOrEmpty(dropoffAttachedAssetName))
				{
					list.Add(dropoffAttachedAssetName);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			List<object> list2 = new List<object>();
			List<AssetHandle> list3 = new List<AssetHandle>();
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				list2.Add(list[i]);
				list3.Add(AssetHandle.Invalid);
				i++;
			}
			Service.Get<AssetManager>().MultiLoad(list3, list, new AssetSuccessDelegate(this.OnSpecialAttackPreloadsComplete), null, list2, null, null);
			int j = 0;
			int count2 = list.Count;
			while (j < count2)
			{
				this.preloadedAssets.Add(list[j], list3[j]);
				j++;
			}
		}

		public void UnloadPreloads()
		{
			AssetManager assetManager = Service.Get<AssetManager>();
			int count = this.preloadAssetPool.Count;
			for (int i = 0; i < count; i++)
			{
				UnityEngine.Object.Destroy(this.preloadAssetPool[i]);
			}
			foreach (AssetHandle current in this.preloadedAssets.Values)
			{
				assetManager.Unload(current);
			}
			this.preloadedAssets.Clear();
			this.preloadAssetPool.Clear();
		}

		private void OnSpecialAttackPreloadsComplete(object asset, object cookie)
		{
			GameObject gameObject = asset as GameObject;
			if (gameObject != null)
			{
				gameObject.SetActive(false);
				this.preloadAssetPool.Add(gameObject);
			}
		}

		public void DeployDropShipTroops(IntPosition spawnPosition, string troopUid, uint count, TeamType teamType)
		{
			int num = 0;
			IDataController dataController = Service.Get<IDataController>();
			TroopTypeVO troopVO = dataController.Get<TroopTypeVO>(troopUid);
			TroopController troopController = Service.Get<TroopController>();
			int num2 = 0;
			while ((long)num2 < (long)((ulong)count))
			{
				troopController.DeployTroopWithOffset(troopVO, ref num, spawnPosition, true, teamType);
				num2++;
			}
			this.numHoveringInbound--;
		}

		private void SpawnProjectile(uint id, object cookie)
		{
			SpecialAttack specialAttack = (SpecialAttack)cookie;
			StaRTSLogger staRTSLogger = Service.Get<StaRTSLogger>();
			if (specialAttack == null)
			{
				staRTSLogger.Error("SpawnProjectile: specialAttack is null");
				this.SendSpecialAttackFired(null);
				return;
			}
			if (specialAttack.GetGunLocator() == null)
			{
				staRTSLogger.Error("SpawnProjectile: specialAttack.GetGunLocator() is null " + specialAttack.VO.Uid);
				this.SendSpecialAttackFired(specialAttack.VO);
				return;
			}
			if (specialAttack.GetGunLocator().transform == null)
			{
				staRTSLogger.Error("SpawnProjectile: specialAttack.GetGunLocator().transform is null " + specialAttack.VO.Uid);
				this.SendSpecialAttackFired(specialAttack.VO);
				return;
			}
			Vector3 position = specialAttack.GetGunLocator().transform.position;
			if (specialAttack.VO == null)
			{
				staRTSLogger.Error("SpawnProjectile: specialAttack.VO is null");
				this.SendSpecialAttackFired(specialAttack.VO);
				return;
			}
			float num = specialAttack.VO.NumberOfAttackers;
			int quantity = (int)((float)specialAttack.VO.Damage / num);
			specialAttack.ApplySpecialAttackBuffs(ref quantity);
			HealthFragment payload = new HealthFragment(null, HealthType.Damaging, quantity);
			ProjectileController projectileController = Service.Get<ProjectileController>();
			if (specialAttack.TargetShield != null && !specialAttack.VO.ProjectileType.PassThroughShield)
			{
				Entity shieldBorderEntity = specialAttack.TargetShield.ShieldBorderEntity;
				if (shieldBorderEntity == null)
				{
					staRTSLogger.Error("SpawnProjectile: shieldTarget is null");
					this.SendSpecialAttackFired(specialAttack.VO);
					return;
				}
				Vector3 targetWorldPos = specialAttack.TargetWorldPos;
				Vector3 targetWorldLoc = Vector3.zero;
				if (!Service.Get<ShieldController>().GetRayShieldIntersection(position, specialAttack.TargetWorldPos, specialAttack.TargetShield, out targetWorldLoc))
				{
					targetWorldLoc = targetWorldPos;
				}
				Target target = Target.CreateTargetWithWorldLocation((SmartEntity)shieldBorderEntity, targetWorldLoc);
				int spawnBoardX = Units.WorldToBoardX((float)((int)position.x));
				int spawnBoardZ = Units.WorldToBoardZ((float)((int)position.z));
				projectileController.SpawnProjectileForTarget(specialAttack.VO.HitDelay, spawnBoardX, spawnBoardZ, position, target, payload, specialAttack.TeamType, null, specialAttack.VO.ProjectileType, false, specialAttack.SpecialAttackBuffs, specialAttack.VO.Faction, null);
				this.AddProjectileInFlight(specialAttack.VO.ProjectileType);
			}
			else
			{
				Bullet bullet = projectileController.SpawnProjectileForTargetPosition(specialAttack.VO.HitDelay, position, specialAttack.TargetBoardX, specialAttack.TargetBoardZ, payload, specialAttack.TeamType, null, specialAttack.VO.ProjectileType, specialAttack.SpecialAttackBuffs, specialAttack.VO.Faction, specialAttack.VO.HasDropoff);
				if (specialAttack.VO.HasDropoff)
				{
					bullet.Cookie = specialAttack;
					ProjectileView projectileView = Service.Get<ProjectileViewManager>().SpawnProjectile(bullet);
					Transform transform = projectileView.GetTransform();
					this.AttachGameObject(transform, specialAttack.StarshipDetachableGameObject, false);
					specialAttack.UpdateDetachableShadowAnimator(SpecialAttackDetachableObjectState.Falling);
				}
				this.AddProjectileInFlight(specialAttack.VO.ProjectileType);
			}
			this.SendSpecialAttackFired(specialAttack.VO);
		}

		private void SendSpecialAttackFired(SpecialAttackTypeVO vo)
		{
			this.numAttackScheduled--;
			if (vo != null)
			{
				Service.Get<EventManager>().SendEvent(EventId.SpecialAttackFired, vo);
			}
		}

		private void AddProjectileInFlight(ProjectileTypeVO vo)
		{
			if (!this.thereAreProjectilesInFlight)
			{
				this.events.RegisterObserver(this, EventId.ProjectileViewPathComplete, EventPriority.Default);
				this.events.RegisterObserver(this, EventId.ProjectileImpacted, EventPriority.Default);
				this.events.RegisterObserver(this, EventId.BattleEndProcessing, EventPriority.Default);
				this.thereAreProjectilesInFlight = true;
			}
			if (!this.projectilesInFlight.ContainsKey(vo))
			{
				this.projectilesInFlight.Add(vo, 1);
				return;
			}
			Dictionary<ProjectileTypeVO, int> dictionary = this.projectilesInFlight;
			dictionary[vo]++;
		}

		private void RemoveProjectileInFlight(ProjectileTypeVO vo)
		{
			if (!this.projectilesInFlight.ContainsKey(vo))
			{
				return;
			}
			Dictionary<ProjectileTypeVO, int> dictionary = this.projectilesInFlight;
			dictionary[vo]--;
			if (this.projectilesInFlight[vo] == 0)
			{
				this.projectilesInFlight.Remove(vo);
			}
			if (this.projectilesInFlight.Keys.Count == 0)
			{
				this.events.UnregisterObserver(this, EventId.ProjectileViewPathComplete);
				this.events.UnregisterObserver(this, EventId.ProjectileImpacted);
				this.thereAreProjectilesInFlight = false;
			}
		}

		private void OnDetachableGameObjectImpact(SpecialAttack attack)
		{
			attack.UpdateDetachableShadowAnimator(SpecialAttackDetachableObjectState.OnGround);
			if (attack.StarshipDetachableGameObject.transform.parent != null)
			{
				attack.StarshipDetachableGameObject.transform.parent = null;
			}
			Vector3 position = attack.StarshipDetachableGameObject.transform.position;
			attack.StarshipDetachableGameObject.transform.position = Vector3.Scale(position, Vector3.one - Vector3.up);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.ProjectileImpacted)
			{
				if (id != EventId.ProjectileViewPathComplete)
				{
					if (id == EventId.BattleEndProcessing)
					{
						bool flag = (bool)cookie;
						if (flag)
						{
							this.Reset();
						}
					}
				}
				else
				{
					Bullet bullet = (Bullet)cookie;
					if (bullet.Cookie != null)
					{
						SpecialAttack specialAttack = (SpecialAttack)bullet.Cookie;
						if (specialAttack.StarshipDetachableGameObject != null)
						{
							this.OnDetachableGameObjectImpact(specialAttack);
						}
					}
				}
			}
			else
			{
				Bullet bullet2 = (Bullet)cookie;
				this.RemoveProjectileInFlight(bullet2.ProjectileType);
				if (bullet2.Cookie != null)
				{
					SpecialAttack specialAttack2 = (SpecialAttack)bullet2.Cookie;
					if (specialAttack2.VO.HasDropoff)
					{
						this.numHoveringInbound--;
					}
				}
			}
			return EatResponse.NotEaten;
		}

		public bool HasUnexpendedSpecialAttacks()
		{
			return this.specialAttacksDeployed.Count > 0 || this.numShipsScheduled > 0 || this.numAttackScheduled > 0 || this.numHoveringInbound > 0 || this.thereAreProjectilesInFlight;
		}

		protected internal SpecialAttackController(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).AddProjectileInFlight((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).AttachGameObject((Transform)GCHandledObjects.GCHandleToObject(*args), (GameObject)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).DeploySpecialAttack((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args), (TeamType)(*(int*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).Destroy((SpecialAttack)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).DestroyAll();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).DoesSpecialAttackNeedTakeoffAndLanding((SpecialAttack)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).EnsureShadowAnimator((SpecialAttack)GCHandledObjects.GCHandleToObject(*args), (GameObject)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).HasUnexpendedSpecialAttacks());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).IntantiateEffect(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).OnAssetSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).OnAttachmentLoadedSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).OnDetachableGameObjectImpact((SpecialAttack)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).OnGameObjectFadeComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).OnLandingFxSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).OnSpecialAttackPreloadsComplete(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).OnTakeOffFxSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).PlayParticle((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).PreloadSpecialAttackMiscAssets();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).RemoveProjectileInFlight((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).ResetTimers();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).SendSpecialAttackFired((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).SetSpeed(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).UnloadPreloads();
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).UpdateSpecialAttacks();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((SpecialAttackController)GCHandledObjects.GCHandleToObject(instance)).UpdateSpeed((SpecialAttack)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
