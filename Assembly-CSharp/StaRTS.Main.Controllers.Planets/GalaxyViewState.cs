using System;

namespace StaRTS.Main.Controllers.Planets
{
	public enum GalaxyViewState
	{
		Loading,
		ManualRotate,
		PlanetTransitionWithinGalaxy,
		PlanetTransitionTowardCamera,
		PlanetTransitionTowardGalaxy,
		PlanetTransitionTowardLeft,
		PlanetTransitionFromLeftTowardGalaxy,
		PlanetTransitionTowardCenter,
		LeftView,
		PlanetViewSwitching,
		PlanetTransitionInstantStart,
		PlanetTransitionPanTo,
		PlanetView
	}
}
