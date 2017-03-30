using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Crates;
using StaRTS.Main.Models.Commands.Objectives;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Objectives;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers.Objectives
{
	public class ObjectiveManager
	{
		private const int MAX_RETRIES = 3;

		private Dictionary<AbstractObjectiveProcessor, ObjectiveProgress> processorMap;

		private bool refreshing;

		private int retryCount;

		public ObjectiveManager()
		{
			Service.Set<ObjectiveManager>(this);
			this.Login();
		}

		private void Login()
		{
			this.processorMap = new Dictionary<AbstractObjectiveProcessor, ObjectiveProgress>();
			this.VerifyCurrentObjectivesAgainstMeta();
			this.FillProcessorMap();
		}

		private void VerifyCurrentObjectivesAgainstMeta()
		{
			IDataController dataController = Service.Get<IDataController>();
			Dictionary<string, ObjectiveGroup> objectives = Service.Get<CurrentPlayer>().Objectives;
			foreach (KeyValuePair<string, ObjectiveGroup> current in objectives)
			{
				bool flag = false;
				int i = 0;
				int count = current.Value.ProgressObjects.Count;
				while (i < count)
				{
					string objectiveUid = current.Value.ProgressObjects[i].ObjectiveUid;
					if (dataController.GetOptional<ObjectiveVO>(objectiveUid) == null)
					{
						flag = true;
						Service.Get<Logger>().WarnFormat("Planet {0} has an invalid objective {1}", new object[]
						{
							current.Key,
							objectiveUid
						});
					}
					i++;
				}
				if (flag)
				{
					ForceObjectivesUpdateCommand command = new ForceObjectivesUpdateCommand(current.Key);
					Service.Get<ServerAPI>().Sync(command);
				}
			}
		}

		public void Update()
		{
			if (this.refreshing)
			{
				return;
			}
			int serverTime = (int)Service.Get<ServerAPI>().ServerTime;
			Dictionary<string, ObjectiveGroup> objectives = Service.Get<CurrentPlayer>().Objectives;
			foreach (KeyValuePair<string, ObjectiveGroup> current in objectives)
			{
				if (serverTime > current.Value.EndTimestamp)
				{
					this.Expire(current.Key, current.Value);
					this.RefreshFromServer();
				}
				else if (serverTime > current.Value.GraceTimestamp)
				{
					this.Grace(current.Key, current.Value);
				}
			}
		}

		private void Grace(string planetUid, ObjectiveGroup group)
		{
			if (planetUid == Service.Get<CurrentPlayer>().PlanetId)
			{
				this.ClearProcessorMap(false);
			}
		}

		public void Relocate()
		{
			this.ClearProcessorMap(false);
			this.FillProcessorMap();
		}

		private void Expire(string planetUid, ObjectiveGroup group)
		{
			if (planetUid == Service.Get<CurrentPlayer>().PlanetId)
			{
				this.ClearProcessorMap(false);
			}
			group.ProgressObjects.Clear();
		}

		public void Progress(AbstractObjectiveProcessor processor, int amount)
		{
			if (Service.Get<PlanetRelocationController>().IsRelocationInProgress())
			{
				return;
			}
			if (!this.processorMap.ContainsKey(processor))
			{
				return;
			}
			EventManager eventManager = Service.Get<EventManager>();
			ObjectiveProgress objectiveProgress = this.processorMap[processor];
			objectiveProgress.Count = Math.Min(objectiveProgress.Count + amount, objectiveProgress.Target);
			if (objectiveProgress.Count >= objectiveProgress.Target)
			{
				objectiveProgress.State = ObjectiveState.Complete;
				eventManager.SendEvent(EventId.UpdateObjectiveToastData, objectiveProgress);
				eventManager.SendEvent(EventId.ObjectiveCompleted, null);
				this.processorMap.Remove(processor);
				processor.Destroy();
			}
			eventManager.SendEvent(EventId.ObjectivesUpdated, objectiveProgress);
		}

		public void Claim(ObjectiveProgress objective)
		{
			objective.State = ObjectiveState.Rewarded;
			Service.Get<EventManager>().SendEvent(EventId.ObjectivesUpdated, null);
			ClaimObjectiveRequest request = new ClaimObjectiveRequest(Service.Get<CurrentPlayer>().PlayerId, objective.PlanetId, objective.ObjectiveUid);
			ClaimObjectiveCommand claimObjectiveCommand = new ClaimObjectiveCommand(request);
			claimObjectiveCommand.AddSuccessCallback(new AbstractCommand<PlayerIdRequest, CrateDataResponse>.OnSuccessCallback(this.ClaimCallback));
			claimObjectiveCommand.AddFailureCallback(new AbstractCommand<PlayerIdRequest, CrateDataResponse>.OnFailureCallback(this.ClaimFailed));
			claimObjectiveCommand.Context = objective;
			Service.Get<ServerAPI>().Sync(claimObjectiveCommand);
		}

		public void ClaimCallback(CrateDataResponse response, object cookie)
		{
			if (response.CrateDataTO != null)
			{
				CrateData crateDataTO = response.CrateDataTO;
				List<string> resolvedSupplyIdList = GameUtils.GetResolvedSupplyIdList(crateDataTO);
				Service.Get<InventoryCrateRewardController>().GrantInventoryCrateReward(resolvedSupplyIdList, response.CrateDataTO);
			}
		}

		private void ClaimFailed(uint status, object cookie)
		{
			Service.Get<Logger>().DebugFormat("Failed to claim objectives from server ({0}).", new object[]
			{
				status
			});
			Service.Get<EventManager>().SendEvent(EventId.ClaimObjectiveFailed, null);
		}

		private void FillProcessorMap()
		{
			if (this.processorMap.Count > 0)
			{
				Service.Get<Logger>().Error("Attempting to fill an already-full processorMap!");
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			string planetId = currentPlayer.PlanetId;
			if (currentPlayer.Objectives.ContainsKey(planetId))
			{
				ObjectiveGroup objectiveGroup = currentPlayer.Objectives[planetId];
				int i = 0;
				int count = objectiveGroup.ProgressObjects.Count;
				while (i < count)
				{
					ObjectiveProgress objectiveProgress = objectiveGroup.ProgressObjects[i];
					if (objectiveProgress.State == ObjectiveState.Active)
					{
						AbstractObjectiveProcessor processor = ObjectiveFactory.GetProcessor(objectiveProgress.VO, this);
						this.processorMap.Add(processor, objectiveProgress);
					}
					i++;
				}
			}
		}

		private void ClearProcessorMap(bool sendExpirationEvent)
		{
			foreach (KeyValuePair<AbstractObjectiveProcessor, ObjectiveProgress> current in this.processorMap)
			{
				if (sendExpirationEvent)
				{
					Service.Get<EventManager>().SendEvent(EventId.UpdateObjectiveToastData, current.Value);
				}
				current.Key.Destroy();
			}
			this.processorMap.Clear();
		}

		public void RefreshFromServer()
		{
			this.retryCount = 0;
			this.AttemptRefreshFromServer();
		}

		private void AttemptRefreshFromServer()
		{
			this.refreshing = true;
			GetObjectivesCommand getObjectivesCommand = new GetObjectivesCommand(new PlayerIdRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId
			});
			getObjectivesCommand.AddSuccessCallback(new AbstractCommand<PlayerIdRequest, GetObjectivesResponse>.OnSuccessCallback(this.OnObjectivesRefreshed));
			getObjectivesCommand.AddFailureCallback(new AbstractCommand<PlayerIdRequest, GetObjectivesResponse>.OnFailureCallback(this.OnObjectivesFailed));
			Service.Get<ServerAPI>().Enqueue(getObjectivesCommand);
		}

		private void OnObjectivesRefreshed(GetObjectivesResponse response, object cookie)
		{
			this.ClearProcessorMap(false);
			this.FillProcessorMap();
			Service.Get<EventManager>().SendEvent(EventId.ObjectivesUpdated, null);
			this.refreshing = false;
		}

		private void OnObjectivesFailed(uint status, object cookie)
		{
			Service.Get<Logger>().ErrorFormat("Failed to refresh objectives from server ({0}).", new object[]
			{
				status
			});
			this.refreshing = false;
			this.ClearProcessorMap(false);
			Service.Get<CurrentPlayer>().Objectives.Clear();
			this.retryCount++;
			if (this.retryCount < 3)
			{
				this.AttemptRefreshFromServer();
			}
		}

		public int GetCompletedObjectivesCount()
		{
			int num = 0;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			foreach (KeyValuePair<string, ObjectiveGroup> current in currentPlayer.Objectives)
			{
				ObjectiveGroup value = current.Value;
				int i = 0;
				int count = value.ProgressObjects.Count;
				while (i < count)
				{
					ObjectiveProgress objectiveProgress = value.ProgressObjects[i];
					if (objectiveProgress.State == ObjectiveState.Complete)
					{
						num++;
					}
					i++;
				}
			}
			return num;
		}
	}
}
