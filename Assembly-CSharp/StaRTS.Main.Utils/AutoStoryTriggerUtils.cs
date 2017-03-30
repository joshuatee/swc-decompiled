using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.RUF.RUFTasks;
using StaRTS.Main.Story.Trigger;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;

namespace StaRTS.Main.Utils
{
	public class AutoStoryTriggerUtils
	{
		private const string TRIGGER_ACTIVATED = "1";

		private const string TRIGGER_NEVER_ACTIVATED = "0";

		public static void SaveTriggerValue(string savePrefName, string value)
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			sharedPlayerPrefs.SetPref(savePrefName, value);
		}

		public static void ClearTriggerValue(string savePrefName)
		{
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			sharedPlayerPrefs.SetPref(savePrefName, null);
		}

		public static AbstractRUFTask GetRUFTaskForAutoTrigger(StoryTriggerVO vo)
		{
			if (string.IsNullOrEmpty(vo.PrepareString))
			{
				Service.Get<Logger>().Error("AutoStoryTrigger: " + vo.Uid + " is missing prepare string");
				return null;
			}
			string[] array = vo.PrepareString.Split(new char[]
			{
				'|'
			});
			if (array.Length < 2)
			{
				Service.Get<Logger>().Error("AutoStoryTrigger: " + vo.Uid + " doesn't have enough arguments");
				return null;
			}
			string value = array[0];
			string prefName = array[1];
			if ("IF_PREF".Equals(value))
			{
				AutoStoryTrigger trigger = new AutoStoryTrigger(vo, Service.Get<QuestController>());
				return new AutoTriggerRUFTask(trigger);
			}
			if (!"RESUME".Equals(value))
			{
				return null;
			}
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			string pref = sharedPlayerPrefs.GetPref<string>(prefName);
			if (string.IsNullOrEmpty(pref))
			{
				return null;
			}
			StoryActionVO optional = Service.Get<IDataController>().GetOptional<StoryActionVO>(pref);
			if (optional == null)
			{
				return null;
			}
			return new ResumeTutorialRUFTask(optional.Uid);
		}
	}
}
