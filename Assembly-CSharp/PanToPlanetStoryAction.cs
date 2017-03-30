using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models.Planets;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Story;
using StaRTS.Main.Story.Actions;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;

public class PanToPlanetStoryAction : AbstractStoryAction, IEventObserver
{
	private Planet planet;

	public PanToPlanetStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
	{
	}

	public EatResponse OnEvent(EventId id, object cookie)
	{
		if (id != EventId.GalaxyStatePanToPlanetComplete)
		{
			if (id == EventId.PlanetsLoadingComplete)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.PlanetsLoadingComplete);
				Service.Get<EventManager>().RegisterObserver(this, EventId.GalaxyStatePanToPlanetComplete, EventPriority.Default);
				Service.Get<GalaxyViewController>().PanToPlanet(this.planet);
			}
		}
		else
		{
			string value = cookie as string;
			if (!this.planet.VO.Uid.Equals(value))
			{
				return EatResponse.NotEaten;
			}
			Service.Get<EventManager>().UnregisterObserver(this, EventId.GalaxyStatePanToPlanetComplete);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.PlanetsLoadingComplete);
			this.parent.ChildComplete(this);
		}
		return EatResponse.NotEaten;
	}

	public override void Prepare()
	{
		string planetUID = Service.Get<CurrentPlayer>().PlanetId;
		if (!string.IsNullOrEmpty(this.vo.PrepareString))
		{
			if (this.vo.PrepareString.Equals("1stPlaName"))
			{
				string firstPlanetUnlockedUID = Service.Get<CurrentPlayer>().GetFirstPlanetUnlockedUID();
				if (!string.IsNullOrEmpty(firstPlanetUnlockedUID))
				{
					planetUID = firstPlanetUnlockedUID;
				}
			}
			else
			{
				planetUID = this.vo.PrepareString;
			}
		}
		this.planet = Service.Get<GalaxyPlanetController>().GetPlanet(planetUID);
		IState currentState = Service.Get<GameStateMachine>().CurrentState;
		if (this.planet == null)
		{
			if (currentState is GalaxyState)
			{
				Service.Get<Logger>().Error("PanToPlanetStoryAction: No Valid planet specified for: " + this.vo.Uid + ": prepare: " + this.vo.PrepareString);
			}
			else
			{
				Service.Get<Logger>().Error("PanToPlanetStoryAction: Can't do PanToPlanetStoryAction when not in Galaxy view");
			}
			this.parent.ChildComplete(this);
			return;
		}
		if (!(currentState is GalaxyState))
		{
			Service.Get<Logger>().Error("PanToPlanetStoryAction: We're not in Galaxy State");
			this.parent.ChildComplete(this);
			return;
		}
		this.parent.ChildPrepared(this);
	}

	public override void Execute()
	{
		base.Execute();
		if (Service.Get<GalaxyPlanetController>().AreAllPlanetsLoaded())
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.GalaxyStatePanToPlanetComplete, EventPriority.Default);
			Service.Get<GalaxyViewController>().PanToPlanet(this.planet);
		}
		else
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.PlanetsLoadingComplete, EventPriority.Default);
		}
	}
}
