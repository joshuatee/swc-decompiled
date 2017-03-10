using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class CircleButtonStoryAction : AbstractStoryAction, IEventObserver
	{
		private const int BUTTON_NAME_ARG = 0;

		private const int ARROW_POS_ARG = 1;

		private const int PERSIST_ARG = 2;

		private const string PERSIST = "persist";

		private UXButton highlightedButton;

		private UXCheckbox highlightedCheckbox;

		private UXButtonClickedDelegate originalButtonClicked;

		private UXCheckboxSelectedDelegate originalCheckboxSelected;

		private bool startedHidingHighlight;

		public CircleButtonStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
			this.startedHidingHighlight = false;
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(new int[]
			{
				1,
				2,
				3
			});
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			string elementName = this.prepareArgs[0];
			string arrowPosition = "";
			if (this.prepareArgs.Length > 1)
			{
				arrowPosition = this.prepareArgs[1].ToLower();
			}
			EventManager eventManager = Service.Get<EventManager>();
			UXElement uXElement = Service.Get<ScreenController>().FindElement<UXElement>(elementName);
			this.highlightedButton = (uXElement as UXButton);
			this.highlightedCheckbox = (uXElement as UXCheckbox);
			if (this.highlightedCheckbox != null && this.highlightedCheckbox.Selected && this.highlightedCheckbox.RadioGroup != 0)
			{
				this.parent.ChildComplete(this);
				return;
			}
			if (uXElement == null)
			{
				this.parent.ChildComplete(this);
				return;
			}
			StoreScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<StoreScreen>();
			if (highestLevelScreen != null && !highestLevelScreen.TransitionedIn)
			{
				eventManager.RegisterObserver(this, EventId.ScreenRefreshed, EventPriority.Default);
				eventManager.RegisterObserver(this, EventId.ScreenTransitionInComplete, EventPriority.Default);
				eventManager.RegisterObserver(this, EventId.StoreScreenReady, EventPriority.Default);
				return;
			}
			if (this.prepareArgs.Length == 3 && "persist".Equals(this.prepareArgs[2], 5))
			{
				this.parent.ChildComplete(this);
			}
			else if (this.highlightedCheckbox != null)
			{
				eventManager.RegisterObserver(this, EventId.ScreenRefreshed, EventPriority.Default);
				eventManager.RegisterObserver(this, EventId.ScreenTransitionInComplete, EventPriority.Default);
				Service.Get<UserInputInhibitor>().AllowOnly(this.highlightedCheckbox);
				if (this.highlightedCheckbox.OnSelected != new UXCheckboxSelectedDelegate(this.OnSelectedHighlighted))
				{
					this.originalCheckboxSelected = this.highlightedCheckbox.OnSelected;
					this.highlightedCheckbox.OnSelected = new UXCheckboxSelectedDelegate(this.OnSelectedHighlighted);
				}
			}
			else
			{
				eventManager.RegisterObserver(this, EventId.ScreenRefreshed, EventPriority.Default);
				eventManager.RegisterObserver(this, EventId.ScreenTransitionInComplete, EventPriority.Default);
				Service.Get<UserInputInhibitor>().AllowOnly(this.highlightedButton);
				if (this.highlightedButton.OnClicked != new UXButtonClickedDelegate(this.OnClickedHighlighted))
				{
					this.originalButtonClicked = this.highlightedButton.OnClicked;
					this.highlightedButton.OnClicked = new UXButtonClickedDelegate(this.OnClickedHighlighted);
				}
			}
			Service.Get<UXController>().MiscElementsManager.HighlightButton(uXElement, arrowPosition);
		}

		private void OnClickedHighlighted(UXButton button)
		{
			if (!this.startedHidingHighlight && button == this.highlightedButton)
			{
				this.AddHighlightListeners();
				this.startedHidingHighlight = true;
				Service.Get<UserInputInhibitor>().AllowAll();
				if (this.originalButtonClicked != null)
				{
					this.originalButtonClicked(this.highlightedButton);
				}
				Service.Get<UXController>().MiscElementsManager.HideHighlight();
			}
		}

		private void OnSelectedHighlighted(UXCheckbox checkbox, bool selected)
		{
			if (!this.startedHidingHighlight && checkbox == this.highlightedCheckbox && (selected || checkbox.RadioGroup == 0))
			{
				this.AddHighlightListeners();
				this.startedHidingHighlight = true;
				Service.Get<UserInputInhibitor>().AllowAll();
				if (this.originalCheckboxSelected != null)
				{
					this.originalCheckboxSelected(this.highlightedCheckbox, selected);
				}
				Service.Get<UXController>().MiscElementsManager.HideHighlight();
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.ScreenRefreshed)
			{
				if (id != EventId.StoreScreenReady && id != EventId.ScreenRefreshed)
				{
					return EatResponse.NotEaten;
				}
			}
			else if (id != EventId.ScreenTransitionInComplete)
			{
				if (id == EventId.ButtonHighlightHidden)
				{
					this.RemoveListeners();
					if (this.highlightedButton != null)
					{
						this.highlightedButton.OnClicked = this.originalButtonClicked;
					}
					else if (this.highlightedCheckbox != null)
					{
						this.highlightedCheckbox.OnSelected = this.originalCheckboxSelected;
					}
					this.parent.ChildComplete(this);
					return EatResponse.NotEaten;
				}
				return EatResponse.NotEaten;
			}
			this.Execute();
			return EatResponse.NotEaten;
		}

		private void AddHighlightListeners()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.ButtonHighlightHidden, EventPriority.Default);
		}

		private void RemoveListeners()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.ButtonHighlightHidden);
			eventManager.UnregisterObserver(this, EventId.ScreenRefreshed);
			eventManager.UnregisterObserver(this, EventId.ScreenTransitionInComplete);
			eventManager.UnregisterObserver(this, EventId.StoreScreenReady);
		}

		public override void Destroy()
		{
			Service.Get<UXController>().MiscElementsManager.HideHighlight();
			base.Destroy();
		}

		protected internal CircleButtonStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CircleButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).AddHighlightListeners();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CircleButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CircleButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CircleButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnClickedHighlighted((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CircleButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((CircleButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnSelectedHighlighted((UXCheckbox)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((CircleButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((CircleButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).RemoveListeners();
			return -1L;
		}
	}
}
