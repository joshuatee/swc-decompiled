using StaRTS.Assets;
using StaRTS.Audio;
using StaRTS.Externals.BI;
using StaRTS.Externals.DMOAnalytics;
using StaRTS.Externals.GameServices;
using StaRTS.FX;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Performance;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using UnityEngine;

namespace StaRTS.Main.Controllers
{
	public class MainController
	{
		public MainController()
		{
			Service.Set<MainController>(this);
			GameStateMachine gameStateMachine = new GameStateMachine();
			gameStateMachine.SetState(new ApplicationLoadState());
			new PerformanceSampler();
		}

		public static void StaticInit()
		{
			CollisionFilters.StaticInit();
		}

		public static void StaticReset()
		{
			Camera[] allCameras = Camera.allCameras;
			int i = 0;
			int num = allCameras.Length;
			while (i < num)
			{
				allCameras[i].enabled = false;
				i++;
			}
			UnityUtils.StaticReset();
			GameServicesManager.StaticReset();
			MultipleEmittersPool.StaticReset();
			if (Service.IsSet<AudioManager>())
			{
				Service.Get<AudioManager>().CleanUp();
			}
			if (Service.IsSet<WWWManager>())
			{
				Service.Get<WWWManager>().CancelAll();
			}
			if (Service.IsSet<AssetManager>())
			{
				Service.Get<AssetManager>().ReleaseAll();
			}
			if (Service.IsSet<EntityController>())
			{
				Service.Get<EntityController>().StaticReset();
			}
			if (Service.IsSet<IDataController>())
			{
				Service.Get<IDataController>().Exterminate();
			}
			if (Service.IsSet<ISocialDataController>())
			{
				Service.Get<ISocialDataController>().StaticReset();
			}
			JsonParser.StaticReset();
			CollisionFilters.StaticReset();
			ProcessingScreen.StaticReset();
			YesNoScreen.StaticReset();
			DynamicRadiusView.StaticReset();
			if (Service.IsSet<Lang>())
			{
				Service.Get<Lang>().CustomKoreanFont = null;
			}
		}

		public static void CleanupReferences()
		{
			if (Service.IsSet<SquadController>())
			{
				Service.Get<SquadController>().Destroy();
			}
			if (Service.IsSet<ProjectileViewManager>())
			{
				Service.Get<ProjectileViewManager>().Destroy();
			}
			if (Service.IsSet<BILoggingController>())
			{
				Service.Get<BILoggingController>().Destroy();
			}
			if (Service.IsSet<DMOAnalyticsController>())
			{
				Service.Get<DMOAnalyticsController>().Destroy();
			}
		}

		protected internal MainController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			MainController.CleanupReferences();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			MainController.StaticInit();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			MainController.StaticReset();
			return -1L;
		}
	}
}
