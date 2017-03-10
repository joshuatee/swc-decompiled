using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class CircleBuildingStoryAction : AbstractStoryAction
	{
		private const int BUILDING_ID_ARG = 0;

		private int boardX;

		private int boardZ;

		private int width;

		private int depth;

		private bool buildingFound;

		public CircleBuildingStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
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
			Entity entity = null;
			NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				if (buildingNode.BuildingComp.BuildingType.BuildingID.Equals(this.prepareArgs[0], 5))
				{
					entity = buildingNode.Entity;
					this.buildingFound = true;
					break;
				}
			}
			if (entity != null)
			{
				this.width = Units.GridToBoardX(entity.Get<BuildingComponent>().BuildingType.SizeX);
				this.depth = Units.GridToBoardZ(entity.Get<BuildingComponent>().BuildingType.SizeY);
				this.boardX = entity.Get<BoardItemComponent>().BoardItem.BoardX + this.width / 2;
				this.boardZ = entity.Get<BoardItemComponent>().BoardItem.BoardZ + this.depth / 2;
			}
			if (this.buildingFound)
			{
				Service.Get<BuildingController>().HighlightBuilding(entity);
				Service.Get<UXController>().MiscElementsManager.HighlightRegion((float)this.boardX, (float)this.boardZ, this.width, this.depth);
				Vector3 zero = Vector3.zero;
				zero.x = Units.BoardToWorldX(this.boardX);
				zero.z = Units.BoardToWorldZ(this.boardZ);
				Service.Get<WorldInitializer>().View.PanToLocation(zero);
				Service.Get<UserInputInhibitor>().AllowOnly(entity);
			}
			this.parent.ChildComplete(this);
		}

		protected internal CircleBuildingStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CircleBuildingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CircleBuildingStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
