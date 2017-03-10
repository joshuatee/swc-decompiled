using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class AbstractStoryAction : IStoryAction
	{
		protected StoryActionVO vo;

		protected IStoryReactor parent;

		protected string[] prepareArgs;

		public StoryActionVO VO
		{
			get
			{
				return this.vo;
			}
		}

		public virtual string Reaction
		{
			get
			{
				return this.vo.Reaction;
			}
		}

		public AbstractStoryAction(StoryActionVO vo, IStoryReactor parent)
		{
			this.vo = vo;
			this.parent = parent;
			if (!string.IsNullOrEmpty(vo.PrepareString))
			{
				this.prepareArgs = vo.PrepareString.Split(new char[]
				{
					'|'
				});
				return;
			}
			this.prepareArgs = new string[0];
		}

		public virtual void Prepare()
		{
		}

		public virtual void Execute()
		{
			if (!string.IsNullOrEmpty(this.vo.LogType) && !string.IsNullOrEmpty(this.vo.LogTag))
			{
				Service.Get<EventManager>().SendEvent(EventId.LogStoryActionExecuted, this.vo);
			}
		}

		protected void VerifyArgumentCount(int requiredArguments)
		{
			if (this.prepareArgs.Length != requiredArguments)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("The StoryAction {0} has an incorrect number of arguments: {1}", new object[]
				{
					this.vo.Uid,
					this.vo.PrepareString
				});
			}
		}

		protected void VerifyArgumentCount(int[] requiredArguments)
		{
			bool flag = false;
			for (int i = 0; i < requiredArguments.Length; i++)
			{
				if (requiredArguments[i] == this.prepareArgs.Length)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("The StoryAction {0} has an incorrect number of arguments: {1}", new object[]
				{
					this.vo.Uid,
					this.vo.PrepareString
				});
			}
		}

		public virtual void Destroy()
		{
		}

		protected internal AbstractStoryAction(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractStoryAction)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractStoryAction)GCHandledObjects.GCHandleToObject(instance)).Reaction);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractStoryAction)GCHandledObjects.GCHandleToObject(instance)).VO);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AbstractStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractStoryAction)GCHandledObjects.GCHandleToObject(instance)).VerifyArgumentCount(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AbstractStoryAction)GCHandledObjects.GCHandleToObject(instance)).VerifyArgumentCount((int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args));
			return -1L;
		}
	}
}
