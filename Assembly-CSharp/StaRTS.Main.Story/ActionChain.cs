using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Story.Actions;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Story
{
	public class ActionChain : IStoryReactor
	{
		private List<IStoryAction> actions;

		private int currentActionIndex;

		private int recursiveCounter;

		private const int RECURSIVE_LIMIT = 500;

		private bool destroying;

		public bool Valid
		{
			get;
			set;
		}

		public ActionChain(string firstActionUid)
		{
			this.Valid = true;
			this.actions = new List<IStoryAction>();
			IDataController dataController = Service.Get<IDataController>();
			string text = firstActionUid;
			this.recursiveCounter = 0;
			bool flag = true;
			while (!string.IsNullOrEmpty(text) && this.recursiveCounter < 500)
			{
				this.recursiveCounter++;
				if (this.recursiveCounter > 500)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Bad Metadata.  The story chain that starts with {0} has caused a loop.", new object[]
					{
						firstActionUid
					});
					this.Valid = false;
					return;
				}
				try
				{
					StoryActionVO vo = dataController.Get<StoryActionVO>(text);
					IStoryAction storyAction = StoryActionFactory.GenerateStoryAction(vo, this);
					this.actions.Add(storyAction);
					text = storyAction.Reaction;
					if (storyAction is EndChainStoryAction)
					{
						flag = false;
					}
				}
				catch (KeyNotFoundException ex)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Error in Story Chain Starting with {0}.  Could not find Action {1}. {2}", new object[]
					{
						firstActionUid,
						text,
						ex.get_Message()
					});
					this.Valid = false;
					return;
				}
			}
			if (flag)
			{
				IStoryAction item = StoryActionFactory.GenerateStoryAction(new StoryActionVO
				{
					ActionType = "EndChain",
					Uid = "autoEnd" + Service.Get<QuestController>().AutoIncrement()
				}, this);
				this.actions.Add(item);
			}
			this.currentActionIndex = -1;
			this.PrepareNextAction();
		}

		public void ChildComplete(IStoryAction action)
		{
			this.ExecuteNextAction();
		}

		public void ChildPrepared(IStoryAction action)
		{
			this.PrepareNextAction();
		}

		private void PrepareNextAction()
		{
			this.currentActionIndex++;
			if (this.currentActionIndex >= this.actions.Count)
			{
				this.currentActionIndex = -1;
				this.ExecuteNextAction();
				return;
			}
			this.actions[this.currentActionIndex].Prepare();
		}

		private void ExecuteNextAction()
		{
			if (this.destroying)
			{
				return;
			}
			this.currentActionIndex++;
			if (this.currentActionIndex >= this.actions.Count)
			{
				this.Destroy();
				return;
			}
			StoryActionVO vO = this.actions[this.currentActionIndex].VO;
			Service.Get<QuestController>().LogAction(vO);
			this.actions[this.currentActionIndex].Execute();
		}

		public void Destroy()
		{
			this.destroying = true;
			int i = this.currentActionIndex;
			int count = this.actions.Count;
			while (i < count)
			{
				this.actions[i].Destroy();
				i++;
			}
			this.actions.Clear();
		}

		protected internal ActionChain(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ActionChain)GCHandledObjects.GCHandleToObject(instance)).ChildComplete((IStoryAction)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ActionChain)GCHandledObjects.GCHandleToObject(instance)).ChildPrepared((IStoryAction)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ActionChain)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ActionChain)GCHandledObjects.GCHandleToObject(instance)).ExecuteNextAction();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ActionChain)GCHandledObjects.GCHandleToObject(instance)).Valid);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ActionChain)GCHandledObjects.GCHandleToObject(instance)).PrepareNextAction();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ActionChain)GCHandledObjects.GCHandleToObject(instance)).Valid = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
