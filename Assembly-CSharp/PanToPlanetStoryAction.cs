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
using WinRTBridge;

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
			string text = cookie as string;
			if (!this.planet.VO.Uid.Equals(text))
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
				Service.Get<StaRTSLogger>().Error("PanToPlanetStoryAction: No Valid planet specified for: " + this.vo.Uid + ": prepare: " + this.vo.PrepareString);
			}
			else
			{
				Service.Get<StaRTSLogger>().Error("PanToPlanetStoryAction: Can't do PanToPlanetStoryAction when not in Galaxy view");
			}
			this.parent.ChildComplete(this);
			return;
		}
		if (!(currentState is GalaxyState))
		{
			Service.Get<StaRTSLogger>().Error("PanToPlanetStoryAction: We're not in Galaxy State");
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
			return;
		}
		Service.Get<EventManager>().RegisterObserver(this, EventId.PlanetsLoadingComplete, EventPriority.Default);
	}

	protected internal PanToPlanetStoryAction(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((PanToPlanetStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PanToPlanetStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((PanToPlanetStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
		return -1L;
	}
}
