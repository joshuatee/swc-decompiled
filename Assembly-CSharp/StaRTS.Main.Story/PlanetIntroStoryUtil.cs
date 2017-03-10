using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Story
{
	public class PlanetIntroStoryUtil
	{
		public const string VIEW_POST_FIX = "Viewed";

		public const string INTRO_VIEWED_STATE = "1";

		public static bool ShouldPlanetIntroStoryBePlayed(string planetUID)
		{
			PlanetVO optional = Service.Get<IDataController>().GetOptional<PlanetVO>(planetUID);
			if (optional == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(optional.IntroStoryAction))
			{
				return false;
			}
			if (!Service.Get<CurrentPlayer>().IsPlanetUnlocked(planetUID))
			{
				return false;
			}
			string prefName = planetUID + "Viewed";
			string pref = Service.Get<SharedPlayerPrefs>().GetPref<string>(prefName);
			return !"1".Equals(pref);
		}

		public static void PlayPlanetIntroStoryChain(string planetUID)
		{
			PlanetVO optional = Service.Get<IDataController>().GetOptional<PlanetVO>(planetUID);
			if (optional == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(optional.IntroStoryAction))
			{
				return;
			}
			if (Service.Get<IDataController>().GetOptional<StoryActionVO>(optional.IntroStoryAction) == null)
			{
				return;
			}
			Service.Get<SharedPlayerPrefs>().SetPref(planetUID + "Viewed", "1");
			new ActionChain(optional.IntroStoryAction);
		}

		public PlanetIntroStoryUtil()
		{
		}

		protected internal PlanetIntroStoryUtil(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			PlanetIntroStoryUtil.PlayPlanetIntroStoryChain(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetIntroStoryUtil.ShouldPlanetIntroStoryBePlayed(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}
