using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class PlanetUtils
	{
		private const string ROTATION_SPEED = "_RotationSpeed";

		public static List<PlanetVO> GetAllPlayerFacingPlanets()
		{
			List<PlanetVO> list = new List<PlanetVO>();
			IDataController dataController = Service.Get<IDataController>();
			foreach (PlanetVO current in dataController.GetAll<PlanetVO>())
			{
				if (current.PlayerFacing)
				{
					list.Add(current);
				}
			}
			return list;
		}

		public static Material StopPlanetSpinning(GameObject spinningPlanetGameObject)
		{
			Material planetMaterial = PlanetUtils.GetPlanetMaterial(spinningPlanetGameObject);
			if (planetMaterial != null)
			{
				planetMaterial.SetFloat("_RotationSpeed", 0f);
			}
			return planetMaterial;
		}

		public static Material GetPlanetMaterial(GameObject planetGameObject)
		{
			if (planetGameObject != null)
			{
				MeshRenderer componentInChildren = planetGameObject.GetComponentInChildren<MeshRenderer>();
				return UnityUtils.EnsureMaterialCopy(componentInChildren);
			}
			return null;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetUtils.GetAllPlayerFacingPlanets());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetUtils.GetPlanetMaterial((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetUtils.StopPlanetSpinning((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
