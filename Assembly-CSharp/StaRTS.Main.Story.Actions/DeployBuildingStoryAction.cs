using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Entities;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

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
			this.boardX = Convert.ToInt32(this.prepareArgs[1], CultureInfo.InvariantCulture);
			this.boardZ = Convert.ToInt32(this.prepareArgs[2], CultureInfo.InvariantCulture);
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

		protected internal DeployBuildingStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeployBuildingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DeployBuildingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
