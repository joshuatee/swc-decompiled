using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StaRTS.Main.Story.Actions
{
	public class ShowBuildingTooltipByTypeStoryAction : AbstractStoryAction
	{
		private const int ARG_BUILDING_TYPE = 0;

		private const int ARG_AREA_X = 1;

		private const int ARG_AREA_Z = 2;

		private const int ARG_AREA_W = 3;

		private const int ARG_AREA_H = 4;

		private bool show;

		private Rect area;

		private BuildingType type;

		public ShowBuildingTooltipByTypeStoryAction(StoryActionVO vo, IStoryReactor parent, bool show) : base(vo, parent)
		{
			this.show = show;
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(new int[]
			{
				1,
				5
			});
			this.type = StringUtils.ParseEnum<BuildingType>(this.prepareArgs[0]);
			if (this.prepareArgs.Length >= 5)
			{
				this.area = new Rect((float)Convert.ToInt32(this.prepareArgs[1]), (float)Convert.ToInt32(this.prepareArgs[2]), (float)Convert.ToInt32(this.prepareArgs[3]), (float)Convert.ToInt32(this.prepareArgs[4]));
			}
			else
			{
				this.area = new Rect(-21f, -21f, 42f, 42f);
			}
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			List<Entity> buildingListByType = Service.Get<BuildingLookupController>().GetBuildingListByType(this.type);
			for (int i = 0; i < buildingListByType.Count; i++)
			{
				Entity entity = buildingListByType[i];
				BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
				if (this.area.Contains(new Vector2((float)buildingComponent.BuildingTO.X, (float)buildingComponent.BuildingTO.Z)))
				{
					if (!ContractUtils.IsBuildingConstructing(entity))
					{
						if (entity.Has<HealthViewComponent>())
						{
							entity.Get<HealthViewComponent>().SetEnabled(this.show);
						}
						if (entity.Has<SupportViewComponent>())
						{
							entity.Get<SupportViewComponent>().SetEnabled(this.show);
						}
						if (entity.Has<GeneratorViewComponent>())
						{
							GeneratorViewComponent generatorViewComponent = entity.Get<GeneratorViewComponent>();
							if (this.show)
							{
								generatorViewComponent.SetEnabled(this.show);
								NodeList<GeneratorViewNode> nodeList = Service.Get<EntityController>().GetNodeList<GeneratorViewNode>();
								for (GeneratorViewNode generatorViewNode = nodeList.Head; generatorViewNode != null; generatorViewNode = generatorViewNode.Next)
								{
									if (generatorViewNode.Entity == entity)
									{
										Service.Get<ICurrencyController>().UpdateGeneratorAccruedCurrency((SmartEntity)entity);
									}
								}
							}
							else
							{
								generatorViewComponent.ShowCollectButton(false);
								generatorViewComponent.SetEnabled(this.show);
							}
						}
						if (this.show)
						{
							Service.Get<BuildingTooltipController>().EnsureBuildingTooltip((SmartEntity)buildingComponent.Entity);
						}
					}
				}
			}
			this.parent.ChildComplete(this);
		}
	}
}
