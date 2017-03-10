using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class ShowTranscriptStoryAction : AbstractStoryAction
	{
		private const int TEXT_STRING_ARG = 0;

		private const int TITLE_STRING_ARG = 1;

		public string Title
		{
			get
			{
				return this.prepareArgs[1];
			}
		}

		public string Text
		{
			get
			{
				return this.prepareArgs[0];
			}
		}

		public ShowTranscriptStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(2);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<EventManager>().SendEvent(EventId.ShowTranscript, this);
			this.parent.ChildComplete(this);
		}

		protected internal ShowTranscriptStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShowTranscriptStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShowTranscriptStoryAction)GCHandledObjects.GCHandleToObject(instance)).Text);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShowTranscriptStoryAction)GCHandledObjects.GCHandleToObject(instance)).Title);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ShowTranscriptStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
