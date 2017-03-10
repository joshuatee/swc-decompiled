using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

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
				Service.Get<StaRTSLogger>().WarnFormat("buildingUiD {0} does not exist", new object[]
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

		protected internal StartPlaceBuildingStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StartPlaceBuildingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((StartPlaceBuildingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
