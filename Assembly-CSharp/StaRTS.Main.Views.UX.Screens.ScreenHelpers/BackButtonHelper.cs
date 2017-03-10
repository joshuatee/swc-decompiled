using StaRTS.Main.Views.UX.Elements;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers
{
	public class BackButtonHelper
	{
		public const string BTN_BACK = "BtnBack";

		protected UXButton backBtn;

		protected UXFactory uxFactory;

		public Action BackButtonCallBack;

		protected Stack<List<UXElement>> navigationVisibilityStack;

		public BackButtonHelper(UXFactory uxFactory)
		{
			this.uxFactory = uxFactory;
			this.navigationVisibilityStack = new Stack<List<UXElement>>();
		}

		public void InitWithSingleElementLayer(UXElement element)
		{
			this.Init();
			this.ResetLayers(element, false);
		}

		public void InitWithMultipleElementsLayer(List<UXElement> elements)
		{
			this.Init();
			this.ResetLayers(elements, false);
		}

		private void Init()
		{
			this.backBtn = this.uxFactory.GetElement<UXButton>("BtnBack");
			this.backBtn.OnClicked = new UXButtonClickedDelegate(this.BackButtonClicked);
		}

		public void ResetLayers(UXElement element, bool makeAllExistingElementsInvisible)
		{
			List<UXElement> elements = new List<UXElement>
			{
				element
			};
			this.ResetLayers(elements, makeAllExistingElementsInvisible);
		}

		public void ResetLayers(List<UXElement> elements, bool makeAllExistingElementsInvisible)
		{
			if (makeAllExistingElementsInvisible)
			{
				while (this.navigationVisibilityStack.Count > 0)
				{
					this.SetVisible(this.navigationVisibilityStack.Pop(), false);
				}
			}
			this.SetVisible(elements, true);
			this.navigationVisibilityStack.Push(elements);
			this.EvaluateBackButtonVisiblity();
		}

		public void AddElementToTopLayer(UXElement element)
		{
			element.Visible = true;
			this.navigationVisibilityStack.Peek().Add(element);
		}

		public void RemoveElementFromTopLayer(UXElement element)
		{
			this.navigationVisibilityStack.Peek().Remove(element);
			element.Visible = false;
		}

		public void AddLayer(UXElement element)
		{
			List<UXElement> elements = new List<UXElement>
			{
				element
			};
			this.AddLayer(elements);
		}

		public void AddLayer(List<UXElement> elements)
		{
			this.SetVisible(this.navigationVisibilityStack.Peek(), false);
			this.SetVisible(elements, true);
			this.navigationVisibilityStack.Push(elements);
			this.EvaluateBackButtonVisiblity();
		}

		public void GoBack()
		{
			this.BackButtonClicked(null);
		}

		public bool IsBackButtonEnabled()
		{
			return this.navigationVisibilityStack.Count > 1;
		}

		public UXButton GetBackButton()
		{
			return this.backBtn;
		}

		protected void EvaluateBackButtonVisiblity()
		{
			this.backBtn.Visible = (this.navigationVisibilityStack.Count > 1);
		}

		protected void BackButtonClicked(UXButton btn)
		{
			this.SetVisible(this.navigationVisibilityStack.Pop(), false);
			this.SetVisible(this.navigationVisibilityStack.Peek(), true);
			this.EvaluateBackButtonVisiblity();
			if (this.BackButtonCallBack != null)
			{
				this.BackButtonCallBack.Invoke();
			}
		}

		private void SetVisible(List<UXElement> elements, bool isVisible)
		{
			for (int i = 0; i < elements.Count; i++)
			{
				elements[i].Visible = isVisible;
			}
		}

		protected internal BackButtonHelper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).AddElementToTopLayer((UXElement)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).AddLayer((UXElement)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).AddLayer((List<UXElement>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).BackButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).EvaluateBackButtonVisiblity();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).GetBackButton());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).GoBack();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).InitWithMultipleElementsLayer((List<UXElement>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).InitWithSingleElementLayer((UXElement)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).IsBackButtonEnabled());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).RemoveElementFromTopLayer((UXElement)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).ResetLayers((UXElement)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).ResetLayers((List<UXElement>)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((BackButtonHelper)GCHandledObjects.GCHandleToObject(instance)).SetVisible((List<UXElement>)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}
	}
}
