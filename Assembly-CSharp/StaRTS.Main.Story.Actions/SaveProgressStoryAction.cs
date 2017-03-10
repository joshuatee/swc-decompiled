using StaRTS.Externals.Manimal;
using StaRTS.Main.Models.Commands.Player.Fue;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class SaveProgressStoryAction : AbstractStoryAction
	{
		private const int QUEST_UID_ARG = 0;

		public SaveProgressStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(new int[]
			{
				default(int),
				1
			});
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			string text = this.vo.Uid;
			if (this.prepareArgs.Length != 0)
			{
				text = this.prepareArgs[0];
			}
			if (text == Service.Get<CurrentPlayer>().CurrentQuest)
			{
				Service.Get<ServerAPI>().Enabled = false;
				this.parent.ChildComplete(this);
				return;
			}
			if (!string.IsNullOrEmpty(text))
			{
				Service.Get<ServerAPI>().Enabled = true;
				Service.Get<CurrentPlayer>().CurrentQuest = text;
				Service.Get<ServerAPI>().Sync(new FueUpdateStateCommand());
				Service.Get<ServerAPI>().Enabled = false;
			}
			else
			{
				Service.Get<StaRTSLogger>().Error("Please use the EndFUE command to end the FUE and not SaveProgress!");
			}
			this.parent.ChildComplete(this);
		}

		protected internal SaveProgressStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SaveProgressStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SaveProgressStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
