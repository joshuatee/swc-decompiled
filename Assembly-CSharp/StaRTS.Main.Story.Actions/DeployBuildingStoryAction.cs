using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Story.Actions
{
	public class DeployBuildingStoryAction : AbstractStoryAction
	{
		private const int BUILDING_UID_ARG = 0;

		private const int BOARDX_ARG = 1;

		private const int BOARDZ_ARG = 2;

		private int boardX;

		private int boardZ;

		private BuildingTypeVO btvo;

		public DeployBuildingStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(3);
			this.boardX = Convert.ToInt32(this.prepareArgs[1]);
			this.boardZ = Convert.ToInt32(this.prepareArgs[2]);
			IDataController dataController = Service.Get<IDataController>();
			this.btvo = dataController.Get<BuildingTypeVO>(this.prepareArgs[0]);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Entity building = Service.Get<EntityFactory>().CreateBuildingEntity(this.btvo, false, true, false);
			Service.Get<WorldController>().AddBuildingHelper(building, this.boardX, this.boardZ, false);
			this.parent.ChildComplete(this);
		}
	}
}
