using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.FX
{
	public class AnimationEventManager
	{
		private const string FACTORY_SPARK = "FactorySpark";

		private const string DEPOT_SPARK = "StarShipDepotSpark";

		private const string EXITED_HYPERSPACE = "ExitedHyperspace";

		private const string HALFWAY_HYPERSPACE = "HalfwayHyperspace";

		private const string PLAY_CRATE_ANIM_VFX = "PlayCrateVFX";

		private const string CRATE_ANIM_REWARD_SHOW_DONE = "RewardShowComplete";

		private const string PLAY_CRATE_ANIM_AND_REWARD_SHOW_DONE = "PlayCrateVFXAndRewardShowComplete";

		private const string CRATE_IDLE_HOP_HOOK = "PlayCrateHopSound";

		private Dictionary<string, KeyValuePair<string, int>> animationEventMap;

		public AnimationEventManager()
		{
			Service.Set<AnimationEventManager>(this);
			this.animationEventMap = new Dictionary<string, KeyValuePair<string, int>>();
		}

		public KeyValuePair<string, int> ParseMessage(string message)
		{
			KeyValuePair<string, int> keyValuePair;
			if (this.animationEventMap.TryGetValue(message, out keyValuePair))
			{
				return keyValuePair;
			}
			char[] array = new char[]
			{
				':'
			};
			string[] array2 = message.Split(array);
			if (array2.Length == 2)
			{
				keyValuePair = new KeyValuePair<string, int>(array2[0], int.Parse(array2[1]));
			}
			else
			{
				keyValuePair = new KeyValuePair<string, int>(string.Empty, 0);
				Service.Get<StaRTSLogger>().Error("Incorrect Animation-Event Message");
			}
			this.animationEventMap.Add(message, keyValuePair);
			return keyValuePair;
		}

		public void ProcessAnimationEvent(EntityRef entityRefMB, string message)
		{
			if (message == "ExitedHyperspace")
			{
				if (!Service.IsSet<PlanetRelocationController>())
				{
					return;
				}
				Service.Get<PlanetRelocationController>().HyperspaceComplete();
				return;
			}
			else
			{
				if (message == "PlayCrateVFXAndRewardShowComplete")
				{
					Service.Get<EventManager>().SendEvent(EventId.PlayCrateAnimVfx, null);
					Service.Get<EventManager>().SendEvent(EventId.CrateRewardAnimDone, null);
					return;
				}
				if (message == "PlayCrateVFX")
				{
					Service.Get<EventManager>().SendEvent(EventId.PlayCrateAnimVfx, null);
					return;
				}
				if (message == "RewardShowComplete")
				{
					Service.Get<EventManager>().SendEvent(EventId.CrateRewardAnimDone, null);
					return;
				}
				if (message == "PlayCrateHopSound")
				{
					Service.Get<EventManager>().SendEvent(EventId.CrateRewardIdleHop, null);
					return;
				}
				if (entityRefMB == null)
				{
					return;
				}
				if (entityRefMB.Entity == null)
				{
					return;
				}
				GameObjectViewComponent gameObjectViewComp = ((SmartEntity)entityRefMB.Entity).GameObjectViewComp;
				if (gameObjectViewComp != null)
				{
					if (string.IsNullOrEmpty(message))
					{
						return;
					}
					KeyValuePair<string, int> keyValuePair = this.ParseMessage(message);
					if (string.IsNullOrEmpty(keyValuePair.get_Key()))
					{
						return;
					}
					if (keyValuePair.get_Key() == "FactorySpark")
					{
						if (!Service.IsSet<BuildingAnimationController>())
						{
							return;
						}
						Service.Get<BuildingAnimationController>().FactorySpark(keyValuePair.get_Value(), gameObjectViewComp);
						return;
					}
					else if (keyValuePair.get_Key() == "StarShipDepotSpark")
					{
						if (!Service.IsSet<BuildingAnimationController>())
						{
							return;
						}
						Service.Get<BuildingAnimationController>().DepotSpark(keyValuePair.get_Value(), gameObjectViewComp);
					}
				}
				return;
			}
		}

		protected internal AnimationEventManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AnimationEventManager)GCHandledObjects.GCHandleToObject(instance)).ParseMessage(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AnimationEventManager)GCHandledObjects.GCHandleToObject(instance)).ProcessAnimationEvent((EntityRef)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}
	}
}
