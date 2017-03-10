using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class PlayPlanetIntroStoryAction : AbstractStoryAction
	{
		private const string INTRO_VIEWED_STATE = "1";

		private string planetUID;

		private string reactionUID;

		public override string Reaction
		{
			get
			{
				return this.reactionUID;
			}
		}

		public PlayPlanetIntroStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
			this.reactionUID = "";
			this.planetUID = Service.Get<CurrentPlayer>().GetFirstPlanetUnlockedUID();
			if (!string.IsNullOrEmpty(vo.PrepareString))
			{
				this.planetUID = this.prepareArgs[0];
			}
			PlanetVO optional = Service.Get<IDataController>().GetOptional<PlanetVO>(this.planetUID);
			if (optional == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(optional.IntroStoryAction))
			{
				return;
			}
			string pref = Service.Get<SharedPlayerPrefs>().GetPref<string>(optional.Uid + "Viewed");
			if ("1".Equals(pref))
			{
				return;
			}
			this.reactionUID = optional.IntroStoryAction;
		}

		public override void Prepare()
		{
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			Service.Get<SharedPlayerPrefs>().SetPref(this.planetUID + "Viewed", "1");
			base.Execute();
			this.parent.ChildComplete(this);
		}

		protected internal PlayPlanetIntroStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlayPlanetIntroStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayPlanetIntroStoryAction)GCHandledObjects.GCHandleToObject(instance)).Reaction);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlayPlanetIntroStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
