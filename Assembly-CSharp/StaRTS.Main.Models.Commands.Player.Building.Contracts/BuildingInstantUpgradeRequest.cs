using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Building.Contracts
{
	public class BuildingInstantUpgradeRequest : BuildingContractRequest
	{
		protected override bool CalculateChecksumManually
		{
			get
			{
				return true;
			}
		}

		public BuildingInstantUpgradeRequest(string buildingKey, string buildingUid, string tag)
		{
			this.buildingKey = buildingKey;
			this.tag = tag;
			ContractTO contractTO = new ContractTO();
			contractTO.Uid = buildingUid;
			contractTO.BuildingKey = buildingKey;
			contractTO.EndTime = ServerTime.Time;
			contractTO.ContractType = ContractType.Upgrade;
			Contract contract = new Contract(buildingUid, DeliveryType.UpgradeBuilding, 0, 0.0);
			contract.ContractTO = contractTO;
			BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(buildingUid);
			bool simulateTroopContractUpdate = buildingTypeVO.Type != BuildingType.Starport;
			base.CalculateChecksum(contract, true, simulateTroopContractUpdate);
		}

		protected internal BuildingInstantUpgradeRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingInstantUpgradeRequest)GCHandledObjects.GCHandleToObject(instance)).CalculateChecksumManually);
		}
	}
}
