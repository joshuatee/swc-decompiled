using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World
{
	public class Scaffolding : IEventObserver
	{
		private const string SCAFFOLD_ASSET = "scaffold_{0}-mod_{1}";

		private const int MIN_SCAFFOLD_SIZE = 2;

		private const int MAX_SCAFFOLD_SIZE = 6;

		private const string SCAFFOLD_NAME = "Scaffold_{0}_{1}{2}";

		private Dictionary<Entity, List<ScaffoldingData>> viewObjects;

		private HashSet<Entity> waitingForView;

		public Scaffolding()
		{
			this.viewObjects = new Dictionary<Entity, List<ScaffoldingData>>();
			this.waitingForView = new HashSet<Entity>();
			Service.Get<EventManager>().RegisterObserver(this, EventId.ContractStarted, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ContractContinued, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingViewReady, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ViewObjectsPurged, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingConstructed, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingLevelUpgraded, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingSwapped, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BuildingCancelled, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ShowScaffolding, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.TroopCancelled, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ChampionStartedRepairing, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ChampionRepaired, EventPriority.Default);
		}

		private void ShowScaffold(Entity building)
		{
			if (!this.viewObjects.ContainsKey(building))
			{
				BuildingComponent buildingComponent = building.Get<BuildingComponent>();
				if (buildingComponent == null)
				{
					return;
				}
				BuildingTypeVO buildingType = buildingComponent.BuildingType;
				FactionType faction = buildingType.Faction;
				string text;
				if (faction != FactionType.Empire)
				{
					text = "rbl";
				}
				else
				{
					text = "emp";
				}
				if (text != null)
				{
					List<string> list = new List<string>();
					List<object> list2 = new List<object>();
					List<AssetHandle> list3 = new List<AssetHandle>();
					int num = Math.Min(buildingType.SizeY, 6);
					for (int i = 2; i <= num; i++)
					{
						string item = string.Format("scaffold_{0}-mod_{1}", new object[]
						{
							text,
							i
						});
						list.Add(item);
						list2.Add(new ScaffoldingData(building, num - i, false));
						list3.Add(AssetHandle.Invalid);
					}
					int num2 = Math.Min(buildingType.SizeX, 6);
					for (int j = 2; j <= num2; j++)
					{
						string item2 = string.Format("scaffold_{0}-mod_{1}", new object[]
						{
							text,
							j
						});
						list.Add(item2);
						list2.Add(new ScaffoldingData(building, num2 - j, true));
						list3.Add(AssetHandle.Invalid);
					}
					if (list.Count > 0)
					{
						Service.Get<AssetManager>().MultiLoad(list3, list, new AssetSuccessDelegate(this.OnAssetSuccess), new AssetFailureDelegate(this.OnAssetFailure), list2, null, null);
						int k = 0;
						int count = list2.Count;
						while (k < count)
						{
							ScaffoldingData scaffoldingData = (ScaffoldingData)list2[k];
							scaffoldingData.Handle = list3[k];
							k++;
						}
						List<ScaffoldingData> value = new List<ScaffoldingData>();
						this.viewObjects.Add(building, value);
					}
				}
			}
		}

		private void OnAssetSuccess(object asset, object cookie)
		{
			GameObject gameObject = (GameObject)asset;
			ScaffoldingData scaffoldingData = (ScaffoldingData)cookie;
			scaffoldingData.GameObj = gameObject;
			Entity building = scaffoldingData.Building;
			if (this.viewObjects.ContainsKey(building))
			{
				int offset = scaffoldingData.Offset;
				bool flip = scaffoldingData.Flip;
				gameObject.name = string.Format("Scaffold_{0}_{1}{2}", new object[]
				{
					building.ID,
					flip ? "L" : "R",
					offset
				});
				BuildingComponent buildingComponent = building.Get<BuildingComponent>();
				BuildingTypeVO buildingType = buildingComponent.BuildingType;
				Transform transform = gameObject.transform;
				float num;
				float num2;
				if (flip)
				{
					num = (float)(-(float)offset);
					num2 = 0f;
					transform.localScale = new Vector3(-1f, 1f, 1f);
					transform.rotation = Quaternion.AngleAxis(-180f, Vector3.up);
				}
				else
				{
					num = 0f;
					num2 = (float)(-(float)offset);
					transform.rotation = Quaternion.AngleAxis(-90f, Vector3.up);
				}
				transform.localPosition = new Vector3((num + (float)buildingType.SizeX * 0.5f) * 3f, 0f, (num2 + (float)buildingType.SizeY * 0.5f) * 3f);
				List<ScaffoldingData> list = this.viewObjects[building];
				list.Add(scaffoldingData);
				GameObjectViewComponent gameObjectViewComponent = building.Get<GameObjectViewComponent>();
				if (gameObjectViewComponent != null)
				{
					this.AttachToView(gameObject, gameObjectViewComponent);
					return;
				}
				gameObject.SetActive(false);
				if (!this.waitingForView.Contains(building))
				{
					this.waitingForView.Add(building);
					return;
				}
			}
			else
			{
				this.UnloadScaffold(scaffoldingData);
			}
		}

		private void OnAssetFailure(object cookie)
		{
			this.UnloadScaffold((ScaffoldingData)cookie);
		}

		private void UnloadScaffold(ScaffoldingData data)
		{
			if (data.GameObj != null)
			{
				UnityEngine.Object.Destroy(data.GameObj);
				data.GameObj = null;
			}
			if (data.Handle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(data.Handle);
				data.Handle = AssetHandle.Invalid;
			}
		}

		private void HideScaffold(Entity building)
		{
			if (this.viewObjects.ContainsKey(building))
			{
				GameObjectViewComponent gameObjectViewComponent = building.Get<GameObjectViewComponent>();
				List<ScaffoldingData> list = this.viewObjects[building];
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					ScaffoldingData scaffoldingData = list[i];
					GameObject gameObj = scaffoldingData.GameObj;
					if (gameObj != null && gameObjectViewComponent != null)
					{
						gameObjectViewComponent.DetachGameObject(gameObj.name);
					}
					this.UnloadScaffold(scaffoldingData);
					i++;
				}
				list.Clear();
				this.viewObjects.Remove(building);
				if (this.waitingForView.Contains(building))
				{
					this.waitingForView.Remove(building);
				}
			}
		}

		private void AttachToView(GameObject gameObject, GameObjectViewComponent view)
		{
			view.AttachGameObject(gameObject.name, gameObject, gameObject.transform.localPosition, false, false);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.ChampionStartedRepairing)
			{
				switch (id)
				{
				case EventId.BuildingViewReady:
				{
					EntityViewParams entityViewParams = (EntityViewParams)cookie;
					Entity entity = entityViewParams.Entity;
					if (this.waitingForView.Contains(entity))
					{
						this.waitingForView.Remove(entity);
						GameObjectViewComponent view = entity.Get<GameObjectViewComponent>();
						List<ScaffoldingData> list = this.viewObjects[entity];
						int i = 0;
						int count = list.Count;
						while (i < count)
						{
							GameObject gameObj = list[i].GameObj;
							gameObj.SetActive(true);
							this.AttachToView(gameObj, view);
							i++;
						}
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
				case EventId.BuildingViewFailed:
					return EatResponse.NotEaten;
				case EventId.BuildingCancelled:
					this.HideScaffold(((ContractEventData)cookie).Entity);
					return EatResponse.NotEaten;
				case EventId.TroopCancelled:
					break;
				default:
					if (id == EventId.ViewObjectsPurged)
					{
						this.HideScaffold((Entity)cookie);
						return EatResponse.NotEaten;
					}
					if (id != EventId.ChampionStartedRepairing)
					{
						return EatResponse.NotEaten;
					}
					goto IL_E2;
				}
			}
			else if (id <= EventId.BuildingConstructed)
			{
				if (id != EventId.ChampionRepaired)
				{
					switch (id)
					{
					case EventId.BuildingLevelUpgraded:
					case EventId.BuildingSwapped:
					case EventId.BuildingConstructed:
					{
						ContractEventData contractEventData = (ContractEventData)cookie;
						this.HideScaffold(contractEventData.Entity);
						return EatResponse.NotEaten;
					}
					default:
						return EatResponse.NotEaten;
					}
				}
			}
			else if (id != EventId.ContractStarted)
			{
				if (id != EventId.ShowScaffolding)
				{
					return EatResponse.NotEaten;
				}
				goto IL_E2;
			}
			else
			{
				ContractEventData contractEventData2 = (ContractEventData)cookie;
				switch (contractEventData2.Contract.DeliveryType)
				{
				case DeliveryType.Building:
				case DeliveryType.UpgradeBuilding:
				case DeliveryType.SwapBuilding:
					this.ShowScaffold(contractEventData2.Entity);
					return EatResponse.NotEaten;
				default:
					return EatResponse.NotEaten;
				}
			}
			ContractEventData contractEventData3 = cookie as ContractEventData;
			SmartEntity smartEntity = (SmartEntity)contractEventData3.Entity;
			if (smartEntity.BuildingComp.BuildingType.Type == BuildingType.ChampionPlatform)
			{
				this.HideScaffold(contractEventData3.Entity);
				return EatResponse.NotEaten;
			}
			return EatResponse.NotEaten;
			IL_E2:
			this.ShowScaffold((Entity)cookie);
			return EatResponse.NotEaten;
		}

		protected internal Scaffolding(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Scaffolding)GCHandledObjects.GCHandleToObject(instance)).AttachToView((GameObject)GCHandledObjects.GCHandleToObject(*args), (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((Scaffolding)GCHandledObjects.GCHandleToObject(instance)).HideScaffold((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Scaffolding)GCHandledObjects.GCHandleToObject(instance)).OnAssetFailure(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Scaffolding)GCHandledObjects.GCHandleToObject(instance)).OnAssetSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Scaffolding)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((Scaffolding)GCHandledObjects.GCHandleToObject(instance)).ShowScaffold((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((Scaffolding)GCHandledObjects.GCHandleToObject(instance)).UnloadScaffold((ScaffoldingData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
