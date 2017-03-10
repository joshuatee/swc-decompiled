using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class AbstractStoryTrigger : IStoryTrigger, ISerializable, IStoryReactor
	{
		public const string UID_KEY = "uid";

		protected StoryTriggerVO vo;

		protected ITriggerReactor parent;

		protected string[] prepareArgs;

		protected IStoryAction updateAction;

		protected bool updateActionPrepared;

		public string Reaction
		{
			get;
			set;
		}

		public StoryTriggerVO VO
		{
			get
			{
				return this.vo;
			}
		}

		public bool HasReaction
		{
			get
			{
				return !string.IsNullOrEmpty(this.Reaction);
			}
		}

		public AbstractStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent)
		{
			this.vo = vo;
			this.parent = parent;
			this.Reaction = vo.Reaction;
			if (!string.IsNullOrEmpty(vo.PrepareString))
			{
				this.prepareArgs = vo.PrepareString.Split(new char[]
				{
					'|'
				});
			}
			else
			{
				this.prepareArgs = new string[0];
			}
			if (!string.IsNullOrEmpty(vo.UpdateAction))
			{
				IDataController dataController = Service.Get<IDataController>();
				try
				{
					StoryActionVO storyActionVO = dataController.Get<StoryActionVO>(vo.UpdateAction);
					this.updateAction = StoryActionFactory.GenerateStoryAction(storyActionVO, this);
					if (!string.IsNullOrEmpty(this.updateAction.VO.Reaction))
					{
						Service.Get<StaRTSLogger>().ErrorFormat("Story chaining is not currently supported for UIActions. {0}, {1}", new object[]
						{
							vo.Uid,
							vo.UpdateAction
						});
					}
				}
				catch (KeyNotFoundException ex)
				{
					Service.Get<StaRTSLogger>().ErrorFormat("Error in StoryTrigger {0}.  Could not find UiAction {1}.", new object[]
					{
						vo.Uid,
						vo.UpdateAction
					});
					throw ex;
				}
			}
		}

		public virtual void Activate()
		{
			Service.Get<StaRTSLogger>().DebugFormat("Activating trigger {0}", new object[]
			{
				this.vo.Uid
			});
			if (this.updateAction != null)
			{
				this.updateAction.Prepare();
			}
		}

		public virtual void ChildPrepared(IStoryAction action)
		{
			this.updateActionPrepared = true;
		}

		public virtual void ChildComplete(IStoryAction action)
		{
		}

		public void UpdateAction()
		{
			if (this.updateActionPrepared)
			{
				this.updateAction.Execute();
			}
		}

		public virtual void Destroy()
		{
			Service.Get<StaRTSLogger>().DebugFormat("Destroying trigger {0}", new object[]
			{
				this.vo.Uid
			});
			this.vo = null;
			this.parent = null;
			this.prepareArgs = null;
			this.updateAction = null;
		}

		public string ToJson()
		{
			Serializer serializer = Serializer.Start();
			this.AddData(ref serializer);
			return serializer.End().ToString();
		}

		protected virtual void AddData(ref Serializer serializer)
		{
			serializer.AddString("uid", this.vo.Uid);
		}

		public virtual ISerializable FromObject(object obj)
		{
			return this;
		}

		public virtual bool IsPreSatisfied()
		{
			return false;
		}

		protected internal AbstractStoryTrigger(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).ChildComplete((IStoryAction)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).ChildPrepared((IStoryAction)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).HasReaction);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Reaction);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).VO);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).IsPreSatisfied());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Reaction = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AbstractStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).UpdateAction();
			return -1L;
		}
	}
}
