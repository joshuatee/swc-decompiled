using StaRTS.Externals.FileManagement;
using StaRTS.Main.Configs;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Assets
{
	public class AssetManager : IViewFrameTimeObserver
	{
		public const string ASSETBUNDLE_DIR = "assetbundles/";

		public const string ASSETBUNDLE_TARGET = "wsaplayer";

		private const string BATTLES_DIR = "battles/";

		public const string ASSETBUNDLE_EXT = ".assetbundle";

		private const string JOE_ASSETBUNDLE_EXT = ".json.wsaplayer.assetbundle";

		private const string LOCAL_COMPRESSED_BUNDLE_EXT = ".local.wsaplayer.assetbundle";

		private const string LOCAL_UNCOMPRESSED_BUNDLE_EXT = ".local_uncompressed.wsaplayer.assetbundle";

		private const string ASSETBUNDLE_PATH_PREFIX = "assetbundles/wsaplayer/";

		private const float MAX_CALLBACKS_TIME_PER_FRAME = 0.011f;

		private const int MAX_CALLBACKS_PER_FRAME = 10;

		private const float LAZY_LOAD_START_DELAY = 3f;

		private const float UNLOAD_PRELOADABLES_DELAY = 5f;

		private const int MAX_CONCURRENT_LOADS = 5;

		private Queue<AssetInfo> assetsPendingLoad;

		private int numAssetsLoading;

		private const string CLEARABLE_BUNDLE_PREFIX = "models_";

		private const string PLANET_LOADING_PRFIX = "planet_loading_";

		private Dictionary<string, ManifestEntry> manifest;

		private Dictionary<string, AssetInfo> assetInfos;

		private Dictionary<string, HashSet<string>> bundleContents;

		private List<string> dependencyBundles;

		private HashSet<int> dependencyPrefabIds;

		private AssetHandle nextRequestHandle;

		private Dictionary<AssetHandle, string> requestedAssets;

		private List<AssetRequest> callbackQueue;

		private MutableIterator callbackQueueIter;

		private LoadingPhase phase;

		private Dictionary<string, AssetHandle> preloadables;

		private HashSet<string> customPreloadables;

		private Dictionary<string, AssetHandle> lazyloadables;

		private bool unloadedPreloadables;

		public GameShaders Shaders
		{
			get;
			private set;
		}

		public AssetProfiler Profiler
		{
			get;
			private set;
		}

		public AssetManager()
		{
			Service.Set<AssetManager>(this);
			this.phase = LoadingPhase.Initialized;
			this.preloadables = new Dictionary<string, AssetHandle>();
			this.customPreloadables = new HashSet<string>();
			this.lazyloadables = new Dictionary<string, AssetHandle>();
			this.unloadedPreloadables = false;
			this.bundleContents = new Dictionary<string, HashSet<string>>();
			this.manifest = new Dictionary<string, ManifestEntry>();
			this.dependencyBundles = new List<string>();
			this.dependencyPrefabIds = new HashSet<int>();
			this.assetsPendingLoad = new Queue<AssetInfo>();
			this.numAssetsLoading = 0;
			this.AddToManifest(AssetType.UXObject, "gui_loading_screen", "gui_loading_screen.local.wsaplayer.assetbundle");
			this.AddToManifest(AssetType.UXObject, "gui_dialog_small", "gui_dialog_small.local.wsaplayer.assetbundle");
			this.AddToManifest(AssetType.UXObject, "gui_shared", "gui_shared.local.wsaplayer.assetbundle");
			this.AddToManifest(AssetType.UnityObject, "shared_shaders", "shared_shaders.local.wsaplayer.assetbundle");
			this.AddToManifest(AssetType.UXObject, "gui_armory", "gui_armory.local.wsaplayer.assetbundle");
			this.AddToManifest(AssetType.UXObject, "gui_equipment_unlocked", "gui_equipment_unlocked.local.wsaplayer.assetbundle");
			this.assetInfos = new Dictionary<string, AssetInfo>();
			this.callbackQueue = new List<AssetRequest>();
			this.callbackQueueIter = new MutableIterator();
			this.nextRequestHandle = AssetHandle.FirstAvailable;
			this.requestedAssets = new Dictionary<AssetHandle, string>();
			this.Shaders = null;
			this.Profiler = new AssetProfiler();
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		public void RegisterDependencyBundle(string bundleName)
		{
			if (!this.manifest.ContainsKey(bundleName))
			{
				Service.Get<StaRTSLogger>().Error("Dependency bundle not found in the manifest: " + bundleName);
				return;
			}
			this.dependencyBundles.Add(this.manifest[bundleName].AssetPath);
		}

		public void UnloadDependencyBundle(string bundleName)
		{
			if (this.manifest.ContainsKey(bundleName))
			{
				ManifestEntry manifestEntry = this.manifest[bundleName];
				this.dependencyBundles.Contains(manifestEntry.AssetPath);
			}
		}

		private bool IsDependencyBundle(string bundleName)
		{
			return this.dependencyBundles.IndexOf(bundleName) >= 0;
		}

		private void AddToManifest(AssetType assetType, string assetName, string assetPath)
		{
			if (string.IsNullOrEmpty(assetName) || string.IsNullOrEmpty(assetPath) || this.manifest.ContainsKey(assetName))
			{
				return;
			}
			if ((assetPath.EndsWith(".local.wsaplayer.assetbundle") || assetPath.EndsWith(".local_uncompressed.wsaplayer.assetbundle")) && !this.customPreloadables.Contains(assetName))
			{
				this.customPreloadables.Add(assetName);
			}
			this.manifest.Add(assetName, new ManifestEntry(assetType, assetPath));
			if (this.InBundle(assetType, assetPath))
			{
				HashSet<string> hashSet;
				if (!this.manifest.ContainsKey(assetPath))
				{
					this.manifest.Add(assetPath, new ManifestEntry(AssetType.Bundle, assetPath));
					hashSet = new HashSet<string>();
					this.bundleContents.Add(assetPath, hashSet);
				}
				else
				{
					hashSet = this.bundleContents[assetPath];
				}
				hashSet.Add(assetName);
			}
		}

		public void AddJsonFileToManifest(string assetName, string assetPath)
		{
			string text = assetName;
			if (!text.EndsWith(".json"))
			{
				text += ".json";
			}
			if (!string.IsNullOrEmpty(assetPath))
			{
				text = assetPath + "/" + text;
			}
			this.AddToManifest(AssetType.Text, assetName, text);
		}

		public void AddJoeFileToManifest(string assetName, string assetPath)
		{
			string text = assetName.Replace(".json.joe", ".json.wsaplayer.assetbundle").ToLower();
			if (!string.IsNullOrEmpty(assetPath))
			{
				text = assetPath + "/" + text;
			}
			this.AddToManifest(AssetType.Bytes, assetName, text);
		}

		public void Add3DModelToManifest(string assetName)
		{
			this.AddToManifest(AssetType.ClonedUnityObject, assetName, this.DeduceAssetPath(assetName));
		}

		private bool InBundle(AssetType assetType, string assetPath)
		{
			return assetType != AssetType.Bundle && (assetPath.EndsWith(".assetbundle") || assetPath.EndsWith(".local.wsaplayer.assetbundle") || assetPath.EndsWith(".local_uncompressed.wsaplayer.assetbundle"));
		}

		public void SetupManifest(bool inFue)
		{
			IDataController dataController = Service.Get<IDataController>();
			foreach (AssetTypeVO current in dataController.GetAll<AssetTypeVO>())
			{
				switch (current.Category)
				{
				case AssetCategory.PreloadStandard:
					if (!this.preloadables.ContainsKey(current.AssetName))
					{
						this.preloadables.Add(current.AssetName, AssetHandle.Invalid);
					}
					break;
				case AssetCategory.PreloadFue:
					if (inFue && !this.preloadables.ContainsKey(current.AssetName))
					{
						this.preloadables.Add(current.AssetName, AssetHandle.Invalid);
					}
					break;
				case AssetCategory.LazyloadStandard:
					if (!this.lazyloadables.ContainsKey(current.AssetName))
					{
						this.lazyloadables.Add(current.AssetName, AssetHandle.Invalid);
					}
					break;
				case AssetCategory.LazyloadFue:
					if (inFue && !this.lazyloadables.ContainsKey(current.AssetName))
					{
						this.lazyloadables.Add(current.AssetName, AssetHandle.Invalid);
					}
					break;
				}
			}
			foreach (UITypeVO current2 in dataController.GetAll<UITypeVO>())
			{
				this.AddToManifest(AssetType.UXObject, current2.AssetName, this.DeduceAssetPath(current2.BundleName));
			}
			foreach (CommonAssetVO current3 in dataController.GetAll<CommonAssetVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current3.AssetName, this.DeduceAssetPath(current3.BundleName));
			}
			dataController.Unload<CommonAssetVO>();
			foreach (BattleTypeVO current4 in dataController.GetAll<BattleTypeVO>())
			{
				this.AddToManifest(AssetType.Text, current4.AssetName, "battles/" + current4.AssetName + ".json");
			}
			foreach (PlanetVO current5 in dataController.GetAll<PlanetVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current5.AssetName, this.DeduceAssetPath(current5.BundleName));
				this.AddToManifest(AssetType.ClonedUnityObject, current5.AssetName + "-lod1", this.DeduceAssetPath(current5.BundleName + "-lod1"));
				this.AddToManifest(AssetType.UXObject, current5.LoadingScreenAssetName, this.DeduceAssetPath(current5.LoadingScreenBundleName));
				if (current5.IconAssetName != null && current5.IconBundleName != null)
				{
					this.AddToManifest(AssetType.ClonedUnityObject, current5.IconAssetName, this.DeduceAssetPath(current5.IconBundleName));
				}
				this.AddToManifest(AssetType.ClonedUnityObject, current5.GalaxyAssetName, this.DeduceAssetPath(current5.GalaxyBundleName));
				string text = "planet_loading_" + current5.Abbreviation;
				this.AddToManifest(AssetType.ClonedUnityObject, text, this.DeduceAssetPath(text));
				this.AddToManifest(AssetType.ClonedUnityObject, current5.TimeOfDayAsset, this.DeduceAssetPath(current5.TimeOfDayAsset));
				this.AddToManifest(AssetType.ClonedUnityObject, current5.WarBoardLightingAsset, this.DeduceAssetPath(current5.WarBoardLightingAsset));
				this.AddToManifest(AssetType.BundledTexture, current5.FooterTexture, this.DeduceAssetPath(current5.FooterTexture));
				this.AddToManifest(AssetType.BundledTexture, current5.FooterConflictTexture, this.DeduceAssetPath(current5.FooterConflictTexture));
				this.AddToManifest(AssetType.BundledTexture, current5.LeaderboardButtonTexture, this.DeduceAssetPath(current5.LeaderboardAssetBundle));
				this.AddToManifest(AssetType.BundledTexture, current5.LeaderboardTileTexture, this.DeduceAssetPath(current5.LeaderboardAssetBundle));
				if (!string.IsNullOrEmpty(current5.WarBoardAssetName) && !string.IsNullOrEmpty(current5.WarBoardBundleName))
				{
					this.AddToManifest(AssetType.ClonedUnityObject, current5.WarBoardAssetName, this.DeduceAssetPath(current5.WarBoardBundleName));
				}
			}
			foreach (BuildingTypeVO current6 in dataController.GetAll<BuildingTypeVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current6.AssetName, this.DeduceAssetPath(current6.BundleName));
				if (current6.FillStateAssetName != null && current6.FillStateBundleName != null)
				{
					this.AddToManifest(AssetType.ClonedUnityObject, current6.FillStateAssetName, this.DeduceAssetPath(current6.FillStateBundleName));
				}
				if (current6.IconAssetName != null && current6.IconBundleName != null)
				{
					this.AddToManifest(AssetType.ClonedUnityObject, current6.IconAssetName, this.DeduceAssetPath(current6.IconBundleName));
				}
				if (current6.Type == BuildingType.Clearable)
				{
					foreach (PlanetVO current7 in dataController.GetAll<PlanetVO>())
					{
						if (current7.Uid != "planet1" && current6.PlayerFacing)
						{
							string clearableAssetName = GameUtils.GetClearableAssetName(current6, current7);
							string bundleName = "models_" + current7.Abbreviation;
							this.AddToManifest(AssetType.ClonedUnityObject, clearableAssetName, this.DeduceAssetPath(bundleName));
						}
					}
				}
			}
			foreach (ObjectiveSeriesVO current8 in dataController.GetAll<ObjectiveSeriesVO>())
			{
				if (current8.SpecialEvent)
				{
					this.AddToManifest(AssetType.BundledTexture, current8.EventIcon, this.DeduceAssetPath(current8.BundleName));
					this.AddToManifest(AssetType.BundledTexture, current8.EventPlayArt, this.DeduceAssetPath(current8.BundleName));
					this.AddToManifest(AssetType.BundledTexture, current8.EventDetailsArt, this.DeduceAssetPath(current8.BundleName));
				}
			}
			foreach (RaidVO current9 in dataController.GetAll<RaidVO>())
			{
				this.AddToManifest(AssetType.UnityObject, current9.BuildingHoloAssetName, this.DeduceAssetPath(current9.BuildingHoloAssetBundle));
			}
			foreach (TroopTypeVO current10 in dataController.GetAll<TroopTypeVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current10.AssetName, this.DeduceAssetPath(current10.BundleName));
				if (current10.IconAssetName != null && current10.IconBundleName != null)
				{
					this.AddToManifest(AssetType.ClonedUnityObject, current10.IconAssetName, this.DeduceAssetPath(current10.IconBundleName));
				}
			}
			foreach (SkinTypeVO current11 in dataController.GetAll<SkinTypeVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current11.AssetName, this.DeduceAssetPath(current11.BundleName));
				if (current11.IconAssetName != null && current11.IconBundleName != null)
				{
					this.AddToManifest(AssetType.ClonedUnityObject, current11.IconAssetName, this.DeduceAssetPath(current11.IconBundleName));
				}
			}
			foreach (CivilianTypeVO current12 in dataController.GetAll<CivilianTypeVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current12.AssetName, this.DeduceAssetPath(current12.BundleName));
			}
			foreach (TransportTypeVO current13 in dataController.GetAll<TransportTypeVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current13.AssetName, this.DeduceAssetPath(current13.BundleName));
			}
			foreach (SpecialAttackTypeVO current14 in dataController.GetAll<SpecialAttackTypeVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current14.AssetName, this.DeduceAssetPath(current14.BundleName));
				if (current14.IconAssetName != null && current14.IconBundleName != null)
				{
					this.AddToManifest(AssetType.ClonedUnityObject, current14.IconAssetName, this.DeduceAssetPath(current14.IconBundleName));
				}
				if (current14.DropoffAttachedAssetName != null && current14.DropoffAttachedBundleName != null)
				{
					this.AddToManifest(AssetType.ClonedUnityObject, current14.DropoffAttachedAssetName, this.DeduceAssetPath(current14.DropoffAttachedBundleName));
				}
			}
			foreach (BuildingConnectorTypeVO current15 in dataController.GetAll<BuildingConnectorTypeVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current15.AssetNameNE, this.DeduceAssetPath(current15.BundleNameNE));
				this.AddToManifest(AssetType.ClonedUnityObject, current15.AssetNameNW, this.DeduceAssetPath(current15.BundleNameNW));
				this.AddToManifest(AssetType.ClonedUnityObject, current15.AssetNameBoth, this.DeduceAssetPath(current15.BundleNameBoth));
			}
			foreach (AudioTypeVO current16 in dataController.GetAll<AudioTypeVO>())
			{
				this.AddToManifest(AssetType.AudioClip, current16.AssetName, this.DeduceAssetPath(current16.BundleName));
			}
			foreach (BuffTypeVO current17 in dataController.GetAll<BuffTypeVO>())
			{
				if (!string.IsNullOrEmpty(current17.AssetName) && !string.IsNullOrEmpty(current17.BundleName))
				{
					for (int i = 1; i <= 7; i++)
					{
						string assetName = string.Format(current17.AssetName, new object[]
						{
							i
						});
						this.AddToManifest(AssetType.UnityObject, assetName, "assetbundles/wsaplayer/" + current17.BundleName + ".assetbundle");
					}
				}
				if (!string.IsNullOrEmpty(current17.ProjectileAttachmentBundle))
				{
					string assetPath = this.DeduceAssetPath(current17.ProjectileAttachmentBundle);
					string rebelMuzzleAssetName = current17.RebelMuzzleAssetName;
					string rebelImpactAssetName = current17.RebelImpactAssetName;
					string empireMuzzleAssetName = current17.EmpireMuzzleAssetName;
					string empireImpactAssetName = current17.EmpireImpactAssetName;
					if (!string.IsNullOrEmpty(rebelMuzzleAssetName))
					{
						this.AddToManifest(AssetType.UnityObject, rebelMuzzleAssetName, assetPath);
					}
					if (!string.IsNullOrEmpty(rebelImpactAssetName))
					{
						this.AddToManifest(AssetType.UnityObject, rebelImpactAssetName, assetPath);
					}
					if (!string.IsNullOrEmpty(empireMuzzleAssetName))
					{
						this.AddToManifest(AssetType.UnityObject, empireMuzzleAssetName, assetPath);
					}
					if (!string.IsNullOrEmpty(empireImpactAssetName))
					{
						this.AddToManifest(AssetType.UnityObject, empireImpactAssetName, assetPath);
					}
				}
			}
			foreach (EffectsTypeVO current18 in dataController.GetAll<EffectsTypeVO>())
			{
				this.AddToManifest(AssetType.UnityObject, current18.AssetName, this.DeduceAssetPath(current18.BundleName));
			}
			foreach (ShaderTypeVO current19 in dataController.GetAll<ShaderTypeVO>())
			{
				this.AddToManifest(AssetType.UnityObject, current19.AssetName, this.DeduceAssetPath(current19.BundleName));
			}
			foreach (CharacterVO current20 in dataController.GetAll<CharacterVO>())
			{
				this.AddToManifest(AssetType.BundledTexture, current20.AssetName, this.DeduceAssetPath(current20.BundleName));
			}
			foreach (TextureVO current21 in dataController.GetAll<TextureVO>())
			{
				this.AddToManifest(AssetType.BundledTexture, current21.AssetName, this.DeduceAssetPath(current21.BundleName));
			}
			foreach (CampaignVO current22 in dataController.GetAll<CampaignVO>())
			{
				if (!string.IsNullOrEmpty(current22.BundleName))
				{
					this.AddToManifest(AssetType.UnityObject, current22.AssetName, this.DeduceAssetPath(current22.BundleName));
				}
			}
			foreach (CampaignMissionVO current23 in dataController.GetAll<CampaignMissionVO>())
			{
				if (!string.IsNullOrEmpty(current23.RaidBriefingBGTextureName))
				{
					this.AddToManifest(AssetType.UnityObject, current23.RaidBriefingBGTextureName, this.DeduceAssetPath(current23.RaidBriefingBGTextureName));
				}
			}
			foreach (MobilizationHologramVO current24 in dataController.GetAll<MobilizationHologramVO>())
			{
				this.AddToManifest(AssetType.UnityObject, current24.AssetName, "assetbundles/wsaplayer/" + current24.BundleName + ".assetbundle");
			}
			foreach (CrateTierVO current25 in dataController.GetAll<CrateTierVO>())
			{
				this.Add3DModelToManifest(current25.AssetName);
			}
			foreach (CrateVO current26 in dataController.GetAll<CrateVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current26.AssetName, this.DeduceAssetPath(current26.BundleName));
				this.Add3DModelToManifest(current26.RewardAnimationAssetName);
			}
			foreach (DataCardTierVO current27 in dataController.GetAll<DataCardTierVO>())
			{
				this.Add3DModelToManifest(current27.AssetName);
			}
			foreach (CurrencyIconVO current28 in dataController.GetAll<CurrencyIconVO>())
			{
				this.Add3DModelToManifest(current28.AssetName);
			}
			foreach (ShardVO current29 in dataController.GetAll<ShardVO>())
			{
				this.AddToManifest(AssetType.ClonedUnityObject, current29.AssetName, "assetbundles/wsaplayer/" + current29.BundleName + ".assetbundle");
			}
		}

		private string DeduceAssetPath(string bundleName)
		{
			string result;
			if (AssetConstants.LOCAL_UNCOMPRESSED_BUNDLE_NAMES.Contains(bundleName))
			{
				result = bundleName + ".local_uncompressed.wsaplayer.assetbundle";
			}
			else if (AssetConstants.LOCAL_COMPRESSED_BUNDLE_NAMES.Contains(bundleName))
			{
				result = bundleName + ".local.wsaplayer.assetbundle";
			}
			else
			{
				result = "assetbundles/wsaplayer/" + bundleName + ".assetbundle";
			}
			return result;
		}

		public void LoadGameShaders(AssetsCompleteDelegate onCompleteCallback, object onCompleteCookie)
		{
			this.Shaders = new GameShaders(onCompleteCallback, onCompleteCookie);
		}

		public void ReleaseAll()
		{
			this.Shaders = null;
			this.Profiler = null;
			List<AssetBundle> list = new List<AssetBundle>();
			List<string> list2 = new List<string>();
			List<AssetInfo> list3 = null;
			foreach (string current in this.assetInfos.Keys)
			{
				AssetInfo assetInfo = this.assetInfos[current];
				if (assetInfo.AssetType == AssetType.Bundle)
				{
					AssetBundle assetBundle = assetInfo.AssetObject as AssetBundle;
					if (assetBundle == null)
					{
						if (assetInfo.AllContentsExtracted)
						{
							if (list3 == null)
							{
								list3 = new List<AssetInfo>();
							}
							list3.Add(assetInfo);
						}
					}
					else
					{
						list.Add(assetBundle);
						list2.Add(assetInfo.AssetName);
					}
				}
				else
				{
					ManifestEntry manifestEntry = this.manifest[assetInfo.AssetName];
					if (!this.InBundle(assetInfo.AssetType, manifestEntry.AssetPath) && assetInfo.AssetObject is UnityEngine.Object)
					{
						UnityEngine.Object obj = (UnityEngine.Object)assetInfo.AssetObject;
						UnityEngine.Object.Destroy(obj);
					}
				}
			}
			this.assetInfos.Clear();
			this.callbackQueue.Clear();
			this.callbackQueueIter.Reset();
			this.assetsPendingLoad.Clear();
			this.numAssetsLoading = 0;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Engine.bundles.Add(list[i]);
				i++;
			}
			if (list3 != null)
			{
				int j = 0;
				int count2 = list3.Count;
				while (j < count2)
				{
					this.UnloadPrefabs(list3[j], true);
					j++;
				}
			}
			this.manifest.Clear();
			this.bundleContents.Clear();
			this.dependencyBundles.Clear();
			this.dependencyPrefabIds.Clear();
			list2.Clear();
		}

		private AssetInfo GetContainingBundle(AssetInfo assetInfo, string assetPath)
		{
			if (!this.InBundle(assetInfo.AssetType, assetPath))
			{
				return null;
			}
			if (!this.assetInfos.ContainsKey(assetPath))
			{
				return null;
			}
			return this.assetInfos[assetPath];
		}

		private void ReferenceAssetRecursively(AssetInfo assetInfo, string assetPath)
		{
			int loadCount = assetInfo.LoadCount;
			assetInfo.LoadCount = loadCount + 1;
			AssetInfo containingBundle = this.GetContainingBundle(assetInfo, assetPath);
			if (containingBundle != null)
			{
				AssetInfo expr_1D = containingBundle;
				loadCount = expr_1D.LoadCount;
				expr_1D.LoadCount = loadCount + 1;
			}
			string assetName = assetInfo.AssetName;
			if (this.preloadables.ContainsKey(assetName) && assetInfo.LoadCount > 1)
			{
				this.Unload(this.preloadables[assetName]);
				this.preloadables.Remove(assetName);
			}
		}

		public void Load(ref AssetHandle handle, string assetName, AssetSuccessDelegate onSuccess, AssetFailureDelegate onFailure, object cookie)
		{
			this.Load(ref handle, assetName, onSuccess, onFailure, cookie, 0);
		}

		public void Load(ref AssetHandle handle, string assetName, AssetSuccessDelegate onSuccess, AssetFailureDelegate onFailure, object cookie, int delayLoadFrames)
		{
			if (handle != AssetHandle.Invalid)
			{
				throw new Exception("AssetManager: load requres invalid input handle");
			}
			AssetHandle assetHandle = this.nextRequestHandle;
			this.nextRequestHandle = assetHandle + 1;
			handle = assetHandle;
			this.requestedAssets.Add(handle, assetName);
			if (string.IsNullOrEmpty(assetName))
			{
				Service.Get<StaRTSLogger>().Error("Asset name cannot be null or empty");
				if (onFailure != null)
				{
					this.callbackQueue.Add(new AssetRequest(handle, assetName, null, onFailure, cookie));
				}
				return;
			}
			if (!this.manifest.ContainsKey(assetName))
			{
				Service.Get<StaRTSLogger>().Error("Asset not found in the manifest: " + assetName);
				if (onFailure != null)
				{
					this.callbackQueue.Add(new AssetRequest(handle, assetName, null, onFailure, cookie));
				}
				return;
			}
			ManifestEntry manifestEntry = this.manifest[assetName];
			AssetType assetType = manifestEntry.AssetType;
			string assetPath = manifestEntry.AssetPath;
			bool flag = false;
			AssetInfo assetInfo = null;
			if (this.assetInfos.ContainsKey(assetName))
			{
				assetInfo = this.assetInfos[assetName];
				if (assetInfo.AssetRequests == null)
				{
					if (assetInfo.AssetObject != null)
					{
						if (onSuccess != null)
						{
							AssetRequest assetRequest = new AssetRequest(handle, assetName, onSuccess, null, cookie);
							assetRequest.DelayLoadFrameCount = delayLoadFrames;
							this.callbackQueue.Add(assetRequest);
						}
					}
					else if (assetInfo.AllContentsExtracted)
					{
						assetInfo.AllContentsExtracted = false;
						flag = true;
					}
					else if (onFailure != null)
					{
						this.callbackQueue.Add(new AssetRequest(handle, assetName, null, onFailure, cookie));
					}
					if (!flag)
					{
						this.ReferenceAssetRecursively(assetInfo, assetPath);
						return;
					}
				}
			}
			AssetRequest assetRequest2 = new AssetRequest(handle, assetName, onSuccess, onFailure, cookie);
			assetRequest2.DelayLoadFrameCount = delayLoadFrames;
			if (!flag && assetInfo != null)
			{
				assetInfo.AssetRequests.Add(assetRequest2);
				this.ReferenceAssetRecursively(assetInfo, assetPath);
				return;
			}
			if (!flag)
			{
				assetInfo = new AssetInfo(assetName, assetType);
				this.assetInfos.Add(assetName, assetInfo);
			}
			AssetInfo expr_1B2 = assetInfo;
			int loadCount = expr_1B2.LoadCount;
			expr_1B2.LoadCount = loadCount + 1;
			assetInfo.AssetRequests = new List<AssetRequest>();
			assetInfo.AssetRequests.Add(assetRequest2);
			bool flag2 = this.InBundle(assetType, assetPath);
			if (flag2)
			{
				if (this.phase == LoadingPhase.PreLoading && !this.preloadables.ContainsKey(assetName) && !this.customPreloadables.Contains(assetName))
				{
					Service.Get<StaRTSLogger>().Warn("Asset not flagged for preload: " + assetName);
				}
				AssetHandle bundleHandle = AssetHandle.Invalid;
				this.Load(ref bundleHandle, assetPath, new AssetSuccessDelegate(this.OnBundleSuccess), new AssetFailureDelegate(this.OnBundleFailure), assetName);
				assetInfo.BundleHandle = bundleHandle;
				return;
			}
			if (!this.IsAtMaxConcurrentLoads())
			{
				this.numAssetsLoading++;
				Service.Get<Engine>().StartCoroutine(this.Fetch(assetPath, assetInfo));
				return;
			}
			this.assetsPendingLoad.Enqueue(assetInfo);
		}

		private bool IsAtMaxConcurrentLoads()
		{
			int num = (GameConstants.MAX_CONCURRENT_ASSET_LOADS > 0) ? GameConstants.MAX_CONCURRENT_ASSET_LOADS : 5;
			return this.numAssetsLoading >= num;
		}

		public void Unload(AssetHandle handle)
		{
			if (!this.requestedAssets.ContainsKey(handle))
			{
				Service.Get<StaRTSLogger>().Error("Unload: invalid request handle: " + (uint)handle);
				return;
			}
			int i = 0;
			int count = this.callbackQueue.Count;
			while (i < count)
			{
				if (this.callbackQueue[i].Handle == handle)
				{
					this.RemoveFromCallbackQueue(i);
					break;
				}
				i++;
			}
			string text = this.requestedAssets[handle];
			if (string.IsNullOrEmpty(text))
			{
				Service.Get<StaRTSLogger>().Error("Unload: asset name cannot be null or empty");
				return;
			}
			if (!this.assetInfos.ContainsKey(text))
			{
				Service.Get<StaRTSLogger>().Error("Unload: not loaded: " + text);
				return;
			}
			if (!this.manifest.ContainsKey(text))
			{
				Service.Get<StaRTSLogger>().Error("Unload: asset not in the manifest: " + text);
				return;
			}
			AssetInfo assetInfo = this.assetInfos[text];
			ManifestEntry manifestEntry = this.manifest[text];
			string assetPath = manifestEntry.AssetPath;
			if (assetInfo.AssetRequests != null)
			{
				if (assetInfo.UnloadHandles == null)
				{
					assetInfo.UnloadHandles = new List<AssetHandle>();
				}
				assetInfo.UnloadHandles.Add(handle);
				int j = 0;
				int count2 = assetInfo.AssetRequests.Count;
				while (j < count2)
				{
					if (assetInfo.AssetRequests[j].Handle == handle)
					{
						assetInfo.AssetRequests.RemoveAt(j);
						return;
					}
					j++;
				}
				return;
			}
			if (assetInfo.LoadCount == 0)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Asset {0} has negative loadCount={1}", new object[]
				{
					text,
					assetInfo.LoadCount
				});
				return;
			}
			AssetInfo expr_18F = assetInfo;
			int loadCount = expr_18F.LoadCount;
			expr_18F.LoadCount = loadCount - 1;
			if (assetInfo.AssetType != AssetType.Bundle || assetInfo.LoadCount == 0)
			{
				this.requestedAssets.Remove(handle);
			}
			bool flag = this.InBundle(assetInfo.AssetType, assetPath);
			if (assetInfo.LoadCount <= 0)
			{
				object assetObject = assetInfo.AssetObject;
				assetInfo.AssetObject = null;
				this.assetInfos.Remove(text);
				if (assetInfo.AssetType == AssetType.Bundle)
				{
					AssetBundle assetBundle = assetObject as AssetBundle;
					if (assetBundle == null)
					{
						if (assetInfo.AllContentsExtracted)
						{
							this.UnloadPrefabs(assetInfo, false);
						}
					}
					else
					{
						assetBundle.Unload(true);
					}
				}
				else if (!flag)
				{
					UnityEngine.Object @object = assetObject as UnityEngine.Object;
					if (@object != null)
					{
						UnityEngine.Object.Destroy(@object);
					}
				}
			}
			if (flag)
			{
				this.Unload(assetInfo.BundleHandle);
				if (assetInfo.LoadCount == 0 && !this.preloadables.ContainsKey(text))
				{
					HashSet<string> hashSet = this.bundleContents[assetPath];
					hashSet.Add(text);
				}
			}
		}

		private void UnloadPrefabs(AssetInfo assetInfo, bool forReload)
		{
			if (assetInfo.Prefabs != null)
			{
				int i = 0;
				int num = assetInfo.Prefabs.Length;
				while (i < num)
				{
					UnityEngine.Object @object = assetInfo.Prefabs[i];
					if (@object != null && (forReload || !this.dependencyPrefabIds.Contains(@object.GetInstanceID())))
					{
						UnityEngine.Object.DestroyImmediate(@object, true);
					}
					i++;
				}
				assetInfo.Prefabs = null;
			}
		}

		public void MultiLoad(List<AssetHandle> handles, List<string> assetNames, AssetSuccessDelegate onSuccess, AssetFailureDelegate onFailure, List<object> cookies, AssetsCompleteDelegate onComplete, object completeCookie)
		{
			if (assetNames == null || cookies == null || handles == null || assetNames.Count == 0 || assetNames.Count != cookies.Count || assetNames.Count != handles.Count)
			{
				throw new Exception("AssetManager: multi-load requires matching input lists");
			}
			int count = assetNames.Count;
			RefCount refCount = new RefCount(count);
			for (int i = 0; i < count; i++)
			{
				MultiAssetInfo cookie = new MultiAssetInfo(assetNames[i], onSuccess, onFailure, cookies[i], refCount, onComplete, completeCookie);
				AssetHandle value = handles[i];
				this.Load(ref value, assetNames[i], new AssetSuccessDelegate(this.OnMultiLoadSuccess), new AssetFailureDelegate(this.OnMultiLoadFailure), cookie);
				handles[i] = value;
			}
		}

		public void RegisterPreloadableAsset(string assetName)
		{
			LoadingPhase loadingPhase = this.phase;
			if ((loadingPhase == LoadingPhase.Initialized || loadingPhase == LoadingPhase.PreLoading) && !this.customPreloadables.Contains(assetName))
			{
				this.customPreloadables.Add(assetName);
			}
		}

		public void PreloadAssets(AssetsCompleteDelegate onComplete, object completeCookie)
		{
			if (this.phase == LoadingPhase.Initialized)
			{
				this.phase = LoadingPhase.PreLoading;
				List<string> list = new List<string>();
				List<object> list2 = new List<object>();
				List<AssetHandle> list3 = new List<AssetHandle>();
				foreach (string current in this.preloadables.Keys)
				{
					list.Add(current);
					list2.Add(new InternalLoadCookie(current));
					list3.Add(AssetHandle.Invalid);
				}
				if (list.Count == 0)
				{
					if (onComplete != null)
					{
						onComplete(completeCookie);
						return;
					}
				}
				else
				{
					this.MultiLoad(list3, list, null, null, list2, onComplete, completeCookie);
					int i = 0;
					int count = list3.Count;
					while (i < count)
					{
						this.preloadables[list[i]] = list3[i];
						i++;
					}
				}
			}
		}

		public void DonePreloading()
		{
			if (this.phase == LoadingPhase.PreLoading)
			{
				this.phase = LoadingPhase.OnDemand;
				this.customPreloadables.Clear();
				Service.Get<ViewTimerManager>().CreateViewTimer(3f, false, new TimerDelegate(this.OnLazyLoadStartTimer), null);
			}
		}

		private void OnLazyLoadStartTimer(uint id, object cookie)
		{
			this.phase = LoadingPhase.LazyLoading;
			List<string> list = new List<string>();
			List<object> list2 = new List<object>();
			List<AssetHandle> list3 = new List<AssetHandle>();
			foreach (string current in this.lazyloadables.Keys)
			{
				list.Add(current);
				list2.Add(new InternalLoadCookie(current));
				list3.Add(AssetHandle.Invalid);
			}
			if (list.Count == 0)
			{
				this.OnLazyLoadComplete(null);
				return;
			}
			this.MultiLoad(list3, list, new AssetSuccessDelegate(this.OnLazyLoadSuccess), new AssetFailureDelegate(this.OnLazyLoadFailure), list2, new AssetsCompleteDelegate(this.OnLazyLoadComplete), null);
			int i = 0;
			int count = list3.Count;
			while (i < count)
			{
				this.lazyloadables[list[i]] = list3[i];
				i++;
			}
		}

		public void UnloadPreloadables()
		{
			if (this.unloadedPreloadables)
			{
				return;
			}
			this.unloadedPreloadables = true;
			Service.Get<ViewTimerManager>().CreateViewTimer(5f, false, new TimerDelegate(this.OnUnloadPreloadablesTimer), null);
		}

		private void OnUnloadPreloadablesTimer(uint id, object cookie)
		{
			foreach (AssetHandle current in this.preloadables.Values)
			{
				this.Unload(current);
			}
			this.preloadables.Clear();
		}

		private void OnLazyLoadSuccess(object asset, object cookie)
		{
			string assetName = ((InternalLoadCookie)cookie).AssetName;
			this.Unload(this.lazyloadables[assetName]);
			this.lazyloadables.Remove(assetName);
		}

		private void OnLazyLoadFailure(object cookie)
		{
			this.OnLazyLoadSuccess(null, cookie);
		}

		private void OnLazyLoadComplete(object cookie)
		{
			this.phase = LoadingPhase.OnDemand;
		}

		private bool IsUncompressedLocalBundle(string assetPath)
		{
			return assetPath.EndsWith(".local_uncompressed.wsaplayer.assetbundle");
		}

		private string GetBundleUrl(string assetPath, out int version)
		{
			if (!assetPath.EndsWith(".local.wsaplayer.assetbundle") && !assetPath.EndsWith(".local_uncompressed.wsaplayer.assetbundle"))
			{
				version = Service.Get<FMS>().GetFileVersion(assetPath);
				return Service.Get<FMS>().GetFileUrl(assetPath);
			}
			version = 0;
			string text = Path.Combine(new string[]
			{
				Application.streamingAssetsPath,
				assetPath
			});
			if (this.IsUncompressedLocalBundle(assetPath))
			{
				return text;
			}
			if (!text.StartsWith("file://"))
			{
				return "file://" + text;
			}
			return text;
		}

		[IteratorStateMachine(typeof(AssetManager.<Fetch>d__71))]
		private IEnumerator Fetch(string assetPath, AssetInfo assetInfo)
		{
			string text = null;
			object obj = null;
			int num;
			text = this.GetBundleUrl(assetPath, out num);
			string text2;
			if (string.IsNullOrEmpty(text))
			{
				text2 = string.Format("Unable to map '{0}' to a valid url", new object[]
				{
					assetPath
				});
			}
			else
			{
				bool enabled = this.Profiler.Enabled;
				WWW wWW;
				if (!enabled && assetInfo.AssetType == AssetType.Bundle && num != 0)
				{
					wWW = WWW.LoadFromCacheOrDownload(text, num);
				}
				else
				{
					this.Profiler.RecordFetchEvent(assetPath, 0, false, false);
					wWW = new WWW(text);
				}
				WWWManager.Add(wWW);
				yield return wWW;
				if (!WWWManager.Remove(wWW))
				{
					yield break;
				}
				text2 = wWW.error;
				if (string.IsNullOrEmpty(text2))
				{
					if (enabled)
					{
						this.Profiler.RecordFetchEvent(assetPath, wWW.bytesDownloaded, false, true);
					}
					switch (assetInfo.AssetType)
					{
					case AssetType.Bundle:
					{
						AssetBundle assetBundle = wWW.assetBundle;
						if (assetBundle != null)
						{
							obj = assetBundle;
							if (this.IsDependencyBundle(assetPath))
							{
								UnityEngine.Object[] array = assetBundle.LoadAllAssets();
								int i = 0;
								int num2 = array.Length;
								while (i < num2)
								{
									this.dependencyPrefabIds.Add(array[i].GetInstanceID());
									i++;
								}
							}
						}
						break;
					}
					case AssetType.Text:
						obj = wWW.text;
						break;
					case AssetType.Bytes:
						obj = wWW.bytes;
						break;
					case AssetType.AudioClip:
						obj = wWW.audioClip;
						break;
					default:
						obj = null;
						break;
					}
				}
				wWW.Dispose();
				wWW = null;
			}
			if (obj == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Failed to fetch asset {0} ({1})", new object[]
				{
					text,
					text2
				});
			}
			this.numAssetsLoading--;
			this.OnAssetLoaded(obj, assetInfo);
			yield break;
		}

		private void OnAssetLoaded(object asset, AssetInfo assetInfo)
		{
			assetInfo.AssetObject = asset;
			List<AssetRequest> assetRequests = assetInfo.AssetRequests;
			assetInfo.AssetRequests = null;
			if (assetInfo.UnloadHandles != null)
			{
				int i = 0;
				int count = assetInfo.UnloadHandles.Count;
				while (i < count)
				{
					this.Unload(assetInfo.UnloadHandles[i]);
					i++;
				}
				assetInfo.UnloadHandles = null;
			}
			if (asset != null)
			{
				int j = 0;
				int count2 = assetRequests.Count;
				while (j < count2)
				{
					AssetRequest assetRequest = assetRequests[j];
					if (assetRequest.OnSuccess != null)
					{
						AssetRequest assetRequest2 = new AssetRequest(assetRequest.Handle, assetRequest.AssetName, assetRequest.OnSuccess, null, assetRequest.Cookie);
						assetRequest2.DelayLoadFrameCount = assetRequest.DelayLoadFrameCount;
						this.callbackQueue.Add(assetRequest2);
					}
					j++;
				}
				return;
			}
			int k = 0;
			int count3 = assetRequests.Count;
			while (k < count3)
			{
				AssetRequest assetRequest3 = assetRequests[k];
				if (assetRequest3.OnFailure != null)
				{
					this.callbackQueue.Add(new AssetRequest(assetRequest3.Handle, assetRequest3.AssetName, null, assetRequest3.OnFailure, assetRequest3.Cookie));
				}
				k++;
			}
		}

		private object Prepare(AssetInfo assetInfo)
		{
			object obj = assetInfo.AssetObject;
			if (this.IsAssetCloned(assetInfo))
			{
				GameObject gameObject = obj as GameObject;
				obj = null;
				if (gameObject != null)
				{
					gameObject = this.CloneGameObject(gameObject);
					UnityUtils.RemoveColliders(gameObject);
					obj = gameObject;
				}
				else if (assetInfo.AssetObject != null)
				{
					Service.Get<StaRTSLogger>().Error("Loaded asset of unexpected type " + assetInfo.AssetObject.GetType().get_Name());
				}
			}
			return obj;
		}

		public GameObject CloneGameObject(GameObject gameObject)
		{
			string name = gameObject.name;
			gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject.name = name;
			return gameObject;
		}

		private void OnBundleSuccess(object asset, object cookie)
		{
			string text = cookie as string;
			AssetInfo assetInfo = this.assetInfos[text];
			AssetBundle assetBundle = asset as AssetBundle;
			ManifestEntry manifestEntry = this.manifest[text];
			string assetPath = manifestEntry.AssetPath;
			asset = null;
			if (assetBundle == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Unable to load bundle {0} for asset {1}", new object[]
				{
					assetPath,
					text
				});
				if (this.phase == LoadingPhase.Initialized)
				{
					string biMessage = "Unable to load asset " + text;
					AlertScreen.ShowModalWithBI(true, null, Service.Get<Lang>().Get(LangUtils.DESYNC_BATCH_MAX_RETRY, new object[0]), biMessage);
					return;
				}
			}
			else
			{
				bool flag = true;
				if (this.phase == LoadingPhase.LazyLoading && this.lazyloadables.ContainsKey(text))
				{
					flag = false;
					List<AssetRequest> assetRequests = assetInfo.AssetRequests;
					if (assetRequests != null)
					{
						int i = 0;
						int count = assetRequests.Count;
						while (i < count)
						{
							AssetRequest request = assetRequests[i];
							if (!this.IsForInternalLoad(request))
							{
								flag = true;
								break;
							}
							i++;
						}
					}
				}
				if (flag)
				{
					asset = ((assetInfo.AssetType == AssetType.Bytes) ? assetBundle.LoadAsset(text + ".bytes") : assetBundle.LoadAsset(text));
					if (asset == null)
					{
						Service.Get<StaRTSLogger>().ErrorFormat("Unable to load asset {0} from bundle {1} (main asset {2})", new object[]
						{
							text,
							assetPath,
							assetBundle.mainAsset
						});
					}
					else
					{
						HashSet<string> hashSet = this.bundleContents[assetPath];
						hashSet.Remove(text);
					}
				}
			}
			this.OnAssetLoaded(asset, assetInfo);
		}

		private void OnBundleFailure(object cookie)
		{
			this.OnBundleSuccess(null, cookie);
		}

		private void OnMultiLoadSuccess(object asset, object cookie)
		{
			MultiAssetInfo multiAssetInfo = (MultiAssetInfo)cookie;
			if (multiAssetInfo.OnSuccess != null)
			{
				multiAssetInfo.OnSuccess(asset, multiAssetInfo.Cookie);
			}
			if (multiAssetInfo.RefCount.Release() == 0 && multiAssetInfo.OnComplete != null)
			{
				multiAssetInfo.OnComplete(multiAssetInfo.CompleteCookie);
			}
		}

		private void OnMultiLoadFailure(object cookie)
		{
			MultiAssetInfo multiAssetInfo = (MultiAssetInfo)cookie;
			if (multiAssetInfo.OnFailure != null)
			{
				multiAssetInfo.OnFailure(multiAssetInfo.Cookie);
			}
			if (multiAssetInfo.RefCount.Release() == 0 && multiAssetInfo.OnComplete != null)
			{
				multiAssetInfo.OnComplete(multiAssetInfo.CompleteCookie);
			}
		}

		private bool IsForInternalLoad(AssetRequest request)
		{
			return request.Cookie is MultiAssetInfo && ((MultiAssetInfo)request.Cookie).Cookie is InternalLoadCookie;
		}

		private AssetRequest RemoveFromCallbackQueue(int i)
		{
			AssetRequest result = this.callbackQueue[i];
			this.callbackQueue.RemoveAt(i);
			this.callbackQueueIter.OnRemove(i);
			return result;
		}

		private AssetRequest PeekCallbackQueue(int i)
		{
			return this.callbackQueue[i];
		}

		public void OnViewFrameTime(float dt)
		{
			if (!this.IsAtMaxConcurrentLoads() && this.assetsPendingLoad.Count > 0)
			{
				AssetInfo assetInfo = this.assetsPendingLoad.Dequeue();
				if (assetInfo.AssetName != null && this.manifest.ContainsKey(assetInfo.AssetName))
				{
					this.numAssetsLoading++;
					Service.Get<Engine>().StartCoroutine(this.Fetch(this.manifest[assetInfo.AssetName].AssetPath, assetInfo));
				}
			}
			int count = this.callbackQueue.Count;
			if (count == 0)
			{
				return;
			}
			float realTimeSinceStartUp = UnityUtils.GetRealTimeSinceStartUp();
			int num = 0;
			this.callbackQueueIter.Init(count);
			while (this.callbackQueueIter.Active())
			{
				AssetRequest assetRequest = this.PeekCallbackQueue(this.callbackQueueIter.Index);
				if (assetRequest.DelayLoadFrameCount > 0)
				{
					AssetRequest expr_BB = assetRequest;
					int delayLoadFrameCount = expr_BB.DelayLoadFrameCount - 1;
					expr_BB.DelayLoadFrameCount = delayLoadFrameCount;
				}
				else
				{
					assetRequest = this.RemoveFromCallbackQueue(this.callbackQueueIter.Index);
					num++;
					string assetName = assetRequest.AssetName;
					if (string.IsNullOrEmpty(assetName) || !this.manifest.ContainsKey(assetName))
					{
						if (assetRequest.OnFailure != null)
						{
							assetRequest.OnFailure(assetRequest.Cookie);
						}
					}
					else if (this.assetInfos.ContainsKey(assetName))
					{
						if (assetRequest.OnSuccess != null)
						{
							AssetInfo assetInfo2 = this.assetInfos[assetName];
							bool flag = true;
							if (this.phase == LoadingPhase.PreLoading && this.preloadables.ContainsKey(assetInfo2.AssetName) && this.IsForInternalLoad(assetRequest))
							{
								flag = false;
							}
							else if (this.phase == LoadingPhase.LazyLoading && this.lazyloadables.ContainsKey(assetInfo2.AssetName) && this.IsForInternalLoad(assetRequest))
							{
								flag = false;
							}
							object asset = flag ? this.Prepare(assetInfo2) : assetInfo2.AssetObject;
							assetRequest.OnSuccess(asset, assetRequest.Cookie);
						}
						else
						{
							assetRequest.OnFailure(assetRequest.Cookie);
						}
					}
					float num2 = UnityUtils.GetRealTimeSinceStartUp() - realTimeSinceStartUp;
					if (num2 == 0f)
					{
						if (num >= 10)
						{
							this.callbackQueueIter.Next();
							break;
						}
					}
					else if (num2 >= 0.011f)
					{
						this.callbackQueueIter.Next();
						break;
					}
				}
				this.callbackQueueIter.Next();
			}
			this.callbackQueueIter.Reset();
		}

		public bool IsAssetCloned(string assetName)
		{
			bool result = false;
			if (this.assetInfos.ContainsKey(assetName))
			{
				result = this.IsAssetCloned(this.assetInfos[assetName]);
			}
			return result;
		}

		private bool IsAssetCloned(AssetInfo assetInfo)
		{
			return assetInfo.AssetType == AssetType.ClonedUnityObject;
		}

		public byte[] GetBinaryContents(object asset)
		{
			TextAsset textAsset = asset as TextAsset;
			if (!(textAsset != null))
			{
				return null;
			}
			return textAsset.bytes;
		}

		public bool HasFinishedDownloading()
		{
			foreach (string current in this.assetInfos.Keys)
			{
				AssetInfo assetInfo = this.assetInfos[current];
				if (assetInfo.AssetRequests != null)
				{
					return false;
				}
			}
			return true;
		}

		protected internal AssetManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).Add3DModelToManifest(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).AddJoeFileToManifest(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).AddJsonFileToManifest(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).AddToManifest((AssetType)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).CloneGameObject((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).DeduceAssetPath(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).DonePreloading();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).Fetch(Marshal.PtrToStringUni(*(IntPtr*)args), (AssetInfo)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).Profiler);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).Shaders);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).GetBinaryContents(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).GetContainingBundle((AssetInfo)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).HasFinishedDownloading());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).InBundle((AssetType)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).IsAssetCloned(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).IsAssetCloned((AssetInfo)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).IsAtMaxConcurrentLoads());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).IsDependencyBundle(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).IsForInternalLoad((AssetRequest)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).IsUncompressedLocalBundle(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).LoadGameShaders((AssetsCompleteDelegate)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).MultiLoad((List<AssetHandle>)GCHandledObjects.GCHandleToObject(*args), (List<string>)GCHandledObjects.GCHandleToObject(args[1]), (AssetSuccessDelegate)GCHandledObjects.GCHandleToObject(args[2]), (AssetFailureDelegate)GCHandledObjects.GCHandleToObject(args[3]), (List<object>)GCHandledObjects.GCHandleToObject(args[4]), (AssetsCompleteDelegate)GCHandledObjects.GCHandleToObject(args[5]), GCHandledObjects.GCHandleToObject(args[6]));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).OnAssetLoaded(GCHandledObjects.GCHandleToObject(*args), (AssetInfo)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).OnBundleFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).OnBundleSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).OnLazyLoadComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).OnLazyLoadFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).OnLazyLoadSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).OnMultiLoadFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).OnMultiLoadSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).PeekCallbackQueue(*(int*)args));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).PreloadAssets((AssetsCompleteDelegate)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).Prepare((AssetInfo)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).ReferenceAssetRecursively((AssetInfo)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).RegisterDependencyBundle(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).RegisterPreloadableAsset(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).ReleaseAll();
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AssetManager)GCHandledObjects.GCHandleToObject(instance)).RemoveFromCallbackQueue(*(int*)args));
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).Profiler = (AssetProfiler)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).Shaders = (GameShaders)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).SetupManifest(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).Unload((AssetHandle)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).UnloadDependencyBundle(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).UnloadPrefabs((AssetInfo)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((AssetManager)GCHandledObjects.GCHandleToObject(instance)).UnloadPreloadables();
			return -1L;
		}
	}
}
