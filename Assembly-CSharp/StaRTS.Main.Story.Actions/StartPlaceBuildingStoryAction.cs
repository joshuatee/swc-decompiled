using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;

namespace StaRTS.Main.Story.Actions
{
	public class StartPlaceBuildingStoryAction : AbstractStoryAction
	{
		private const int ARG_BUILDING_UID = 0;

		private string buildingUid;

		public StartPlaceBuildingStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(1);
			this.buildingUid = this.prepareArgs[0];
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(this.buildingUid);
			if (buildingTypeVO == null)
			{
				Service.Get<Logger>().WarnFormat("buildingUiD {0} does not exist", new object[]
				{
					this.buildingUid
				});
			}
			else
			{
				Service.Get<BuildingController>().PrepareAndPurchaseNewBuilding(buildingTypeVO);
				Entity selectedBuilding = Service.Get<BuildingController>().SelectedBuilding;
				Service.Get<UserInputInhibitor>().AllowOnly(selectedBuilding);
				Service.Get<UXController>().MiscElementsManager.EnableConfirmGroupAcceptButton(true);
			}
			this.parent.ChildComplete(this);
		}
	}
}
