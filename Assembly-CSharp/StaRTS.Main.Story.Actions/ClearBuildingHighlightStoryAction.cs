using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class ClearBuildingHighlightStoryAction : AbstractStoryAction
	{
		private const int BUILDING_ID_ARG = 0;

		public ClearBuildingHighlightStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(1);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				if (buildingNode.BuildingComp.BuildingType.BuildingID.Equals(this.prepareArgs[0], 5))
				{
					Entity entity = buildingNode.Entity;
					Service.Get<BuildingController>().ClearBuildingHighlight(entity);
					Service.Get<UXController>().MiscElementsManager.HideHighlight();
					break;
				}
			}
			this.parent.ChildComplete(this);
		}

		protected internal ClearBuildingHighlightStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ClearBuildingHighlightStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ClearBuildingHighlightStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
