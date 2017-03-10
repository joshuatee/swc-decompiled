using StaRTS.Assets;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.Animations;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class TextCrawlStoryAction : AbstractStoryAction
	{
		private TextCrawlAnimation anim;

		public TextCrawlStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		private void OnAnimationDoneLoading(object cookie)
		{
			this.parent.ChildPrepared(this);
		}

		public override void Prepare()
		{
			char[] array = new char[]
			{
				'|'
			};
			string[] array2 = this.vo.PrepareString.Split(array, 0);
			string chapterNumber = array2[0];
			string chapterName = array2[1];
			string chapterBody = array2[2];
			this.anim = new TextCrawlAnimation(chapterNumber, chapterName, chapterBody, new AssetsCompleteDelegate(this.OnAnimationDoneLoading), null, new TextCrawlAnimation.TextCrawlAnimationCompleteDelegate(this.OnAnimationComplete));
		}

		private void OnAnimationComplete()
		{
			this.parent.ChildComplete(this);
		}

		public override void Execute()
		{
			this.anim.Start();
			base.Execute();
		}

		protected internal TextCrawlStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TextCrawlStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TextCrawlStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnAnimationComplete();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TextCrawlStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnAnimationDoneLoading(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TextCrawlStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
