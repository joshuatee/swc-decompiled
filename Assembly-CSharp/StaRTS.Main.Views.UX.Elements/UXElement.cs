using NGUIExtensions;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXElement
	{
		private const string ANIMATOR_MISSING = "Animator missing for : '{0}'";

		private const string ANIMATOR_NOT_INITIALIZED = "Animator not set. Call InitAnimator(). On: '{0}'";

		private const string COLLIDER_MISSING = "Collider missing for : '{0}'";

		protected UXCamera uxCamera;

		protected GameObject root;

		protected UIButton enabler;

		protected UXButton uxButton;

		protected UXTween uxTween;

		private UIWidget NGUIWidget;

		private UIPanel rootPanel;

		protected Animator animator;

		public bool SendDestroyEvent;

		public object Tag
		{
			get;
			set;
		}

		public bool OrigVisible
		{
			get;
			private set;
		}

		public GameObject Root
		{
			get
			{
				return this.root;
			}
		}

		public UIWidget GetUIWidget
		{
			get
			{
				return this.NGUIWidget;
			}
		}

		public UXCamera UXCamera
		{
			get
			{
				return this.uxCamera;
			}
			set
			{
				this.uxCamera = value;
			}
		}

		public virtual bool Visible
		{
			get
			{
				return this.root != null && this.root.activeSelf;
			}
			set
			{
				if (this.root != null)
				{
					if (value && !this.root.activeSelf)
					{
						this.root.SetActive(true);
						return;
					}
					if (!value && this.root.activeSelf)
					{
						this.root.SetActive(false);
					}
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return this.enabler == null || this.enabler.isEnabled;
			}
			set
			{
				if (this.enabler != null)
				{
					this.enabler.isEnabled = value;
				}
				if (this.root != null)
				{
					ButtonTap component = this.root.GetComponent<ButtonTap>();
					if (component != null)
					{
						component.enabled = value;
					}
				}
				if (this.uxButton != null)
				{
					this.uxButton.Enabled = value;
				}
			}
		}

		public UXButtonClickedDelegate OnElementClicked
		{
			get
			{
				return this.uxButton.OnClicked;
			}
			set
			{
				if (this.uxButton != null)
				{
					this.uxButton.OnClicked = value;
				}
			}
		}

		public UXElement Parent
		{
			set
			{
				if (this.root != null)
				{
					this.root.transform.parent = ((value == null) ? null : value.Root.transform);
				}
			}
		}

		public Vector3 WorldPosition
		{
			get
			{
				if (this.root != null)
				{
					return this.root.transform.position;
				}
				return Vector3.zero;
			}
		}

		public Vector3 Position
		{
			get
			{
				if (this.root != null)
				{
					Vector3 a = this.uxCamera.Camera.WorldToScreenPoint(this.root.transform.position);
					return a * this.uxCamera.Scale;
				}
				return Vector3.zero;
			}
			set
			{
				if (this.root != null)
				{
					Vector3 vector = value;
					vector.x = Mathf.Round(vector.x / this.uxCamera.Scale);
					vector.y = Mathf.Round(vector.y / this.uxCamera.Scale);
					vector.z = Mathf.Round(vector.z / this.uxCamera.Scale);
					this.root.transform.position = this.uxCamera.Camera.ScreenToWorldPoint(vector);
				}
			}
		}

		public Vector3 LocalPosition
		{
			get
			{
				if (!(this.root == null))
				{
					return this.root.transform.localPosition * this.uxCamera.Scale;
				}
				return Vector3.zero;
			}
			set
			{
				if (this.root != null)
				{
					Vector3 vector = value;
					vector.x = Mathf.Round(vector.x / this.uxCamera.Scale);
					vector.y = Mathf.Round(vector.y / this.uxCamera.Scale);
					vector.z = Mathf.Round(vector.z / this.uxCamera.Scale);
					this.root.transform.localPosition = vector;
				}
			}
		}

		public Vector3 LocalScale
		{
			get
			{
				if (!(this.root == null))
				{
					return this.root.transform.localScale * this.uxCamera.Scale;
				}
				return Vector3.one;
			}
			set
			{
				if (this.root != null)
				{
					this.root.transform.localScale = value / this.uxCamera.Scale;
				}
			}
		}

		public float Width
		{
			get
			{
				if (!(this.NGUIWidget == null))
				{
					return (float)this.NGUIWidget.width * this.uxCamera.Scale;
				}
				return 0f;
			}
			set
			{
				if (this.NGUIWidget != null)
				{
					this.NGUIWidget.width = (int)Mathf.Round(value / this.uxCamera.Scale);
				}
			}
		}

		public float Height
		{
			get
			{
				if (!(this.NGUIWidget == null))
				{
					return (float)this.NGUIWidget.height * this.uxCamera.Scale;
				}
				return 0f;
			}
			set
			{
				if (this.NGUIWidget != null)
				{
					this.NGUIWidget.height = (int)Mathf.Round(value / this.uxCamera.Scale);
				}
			}
		}

		public float ColliderXUnscaled
		{
			get
			{
				if (this.HasCollider())
				{
					return this.root.GetComponent<BoxCollider>().center.x;
				}
				Service.Get<StaRTSLogger>().WarnFormat("Collider missing for : '{0}'", new object[]
				{
					(this.root == null) ? "null root" : this.root.name
				});
				return 1f;
			}
		}

		public float ColliderWidthUnscaled
		{
			get
			{
				if (this.HasCollider())
				{
					return this.root.GetComponent<BoxCollider>().size.x;
				}
				Service.Get<StaRTSLogger>().WarnFormat("Collider missing for : '{0}'", new object[]
				{
					(this.root == null) ? "null root" : this.root.name
				});
				return 1f;
			}
		}

		public float ColliderWidth
		{
			get
			{
				if (this.root != null && this.root.GetComponent<Collider>() != null)
				{
					return this.uxCamera.ScaleColliderHorizontally(this.root.GetComponent<Collider>().bounds.size.x);
				}
				return 0f;
			}
		}

		public float ColliderHeight
		{
			get
			{
				if (this.root != null && this.root.GetComponent<Collider>() != null)
				{
					return this.uxCamera.ScaleColliderVertically(this.root.GetComponent<Collider>().bounds.size.y);
				}
				return 0f;
			}
		}

		public int WidgetDepth
		{
			get
			{
				int result = 0;
				if (this.root != null)
				{
					UXElement.GetRootDepth(this.root, ref result);
				}
				return result;
			}
			set
			{
				if (this.root != null)
				{
					int rootDepth = 0;
					if (UXElement.GetRootDepth(this.root, ref rootDepth))
					{
						UXElement.SetHierarchyDepth(this.root, value, rootDepth);
					}
				}
			}
		}

		public UXElement(UXCamera uxCamera, GameObject root, UIButton enabler)
		{
			this.uxCamera = uxCamera;
			this.enabler = enabler;
			this.InternalSetRoot(root);
		}

		public void InternalSetRoot(GameObject root)
		{
			this.root = root;
			this.NGUIWidget = ((root == null) ? null : root.GetComponent<UIWidget>());
			if (root != null)
			{
				this.OrigVisible = this.Visible;
			}
		}

		public void EnablePlayTween()
		{
			if (this.uxTween != null)
			{
				this.uxTween.Enable = true;
			}
		}

		public void DisablePlayTween()
		{
			if (this.uxTween != null)
			{
				this.uxTween.Enable = false;
			}
		}

		public bool IsPlayTweenEnabled()
		{
			return this.uxTween != null && this.uxTween.Enable;
		}

		public void ResetPlayTweenTarget()
		{
			if (this.uxTween != null)
			{
				this.uxTween.ResetUIPlayTweenTargetToBegining();
			}
		}

		public void InitTweenComponent()
		{
			if (this.Root != null)
			{
				if (this.uxTween == null)
				{
					this.uxTween = new UXTween();
				}
				this.uxTween.Init(this.Root);
			}
		}

		public void PlayTween(bool play)
		{
			UIPlayTween component = this.root.GetComponent<UIPlayTween>();
			if (component == null)
			{
				return;
			}
			component.Play(play);
		}

		public void AddUXButton(UXButton btn)
		{
			this.uxButton = btn;
		}

		public bool HasCollider()
		{
			return this.root != null && this.root.GetComponent<Collider>() != null;
		}

		public void SetRootName(string name)
		{
			if (this.root != null)
			{
				this.root.name = name;
			}
		}

		private GameObject GetPanelUnifiedAnchorTarget(UIPanel panel)
		{
			GameObject result = null;
			if (panel != null)
			{
				if (panel.topAnchor.target != null)
				{
					result = panel.topAnchor.target.gameObject;
				}
				else if (panel.leftAnchor.target != null)
				{
					result = panel.leftAnchor.target.gameObject;
				}
				else if (panel.bottomAnchor.target != null)
				{
					result = panel.bottomAnchor.target.gameObject;
				}
				else if (panel.rightAnchor.target != null)
				{
					result = panel.rightAnchor.target.gameObject;
				}
			}
			return result;
		}

		private UIPanel GetRootPanel()
		{
			if (this.rootPanel == null && this.Root != null)
			{
				this.rootPanel = this.Root.GetComponent<UIPanel>();
			}
			return this.rootPanel;
		}

		public void RefreshPanel()
		{
			UIPanel uIPanel = this.GetRootPanel();
			if (uIPanel != null)
			{
				uIPanel.Refresh();
			}
		}

		public int GetPanelAnchorOffset(UXAnchorSection anchorSection)
		{
			int result = 0;
			UIPanel uIPanel = this.GetRootPanel();
			if (uIPanel != null)
			{
				switch (anchorSection)
				{
				case UXAnchorSection.Top:
					result = uIPanel.topAnchor.absolute;
					break;
				case UXAnchorSection.Left:
					result = uIPanel.leftAnchor.absolute;
					break;
				case UXAnchorSection.Bottom:
					result = uIPanel.bottomAnchor.absolute;
					break;
				case UXAnchorSection.Right:
					result = uIPanel.rightAnchor.absolute;
					break;
				}
			}
			return result;
		}

		public void SetPanelUnifiedAnchorOffsets(int left, int bottom, int right, int top)
		{
			UIPanel uIPanel = this.GetRootPanel();
			if (uIPanel != null)
			{
				GameObject panelUnifiedAnchorTarget = this.GetPanelUnifiedAnchorTarget(uIPanel);
				uIPanel.SetAnchor(panelUnifiedAnchorTarget, left, bottom, right, top);
			}
		}

		public void SetPanelUnifiedAnchorBottomOffset(int bottom)
		{
			UIPanel uIPanel = this.GetRootPanel();
			if (uIPanel != null)
			{
				int absolute = uIPanel.leftAnchor.absolute;
				int absolute2 = uIPanel.rightAnchor.absolute;
				int absolute3 = uIPanel.topAnchor.absolute;
				this.SetPanelUnifiedAnchorOffsets(absolute, bottom, absolute2, absolute3);
			}
		}

		public void SetPanelUnifiedAnchorTopOffset(int top)
		{
			UIPanel uIPanel = this.GetRootPanel();
			if (uIPanel != null)
			{
				int absolute = uIPanel.leftAnchor.absolute;
				int absolute2 = uIPanel.rightAnchor.absolute;
				int absolute3 = uIPanel.bottomAnchor.absolute;
				this.SetPanelUnifiedAnchorOffsets(absolute, absolute3, absolute2, top);
			}
		}

		public void SetPanelUnifiedAnchorLeftOffset(int left)
		{
			UIPanel uIPanel = this.GetRootPanel();
			if (uIPanel != null)
			{
				int absolute = uIPanel.rightAnchor.absolute;
				int absolute2 = uIPanel.topAnchor.absolute;
				int absolute3 = uIPanel.bottomAnchor.absolute;
				this.SetPanelUnifiedAnchorOffsets(left, absolute3, absolute, absolute2);
			}
		}

		public void SetPanelUnifiedAnchorRightOffset(int right)
		{
			UIPanel uIPanel = this.GetRootPanel();
			if (uIPanel != null)
			{
				int absolute = uIPanel.leftAnchor.absolute;
				int absolute2 = uIPanel.topAnchor.absolute;
				int absolute3 = uIPanel.bottomAnchor.absolute;
				this.SetPanelUnifiedAnchorOffsets(absolute, absolute3, right, absolute2);
			}
		}

		private static bool GetRootDepth(GameObject gameObject, ref int depth)
		{
			UIPanel component = gameObject.GetComponent<UIPanel>();
			if (component != null)
			{
				depth = component.depth;
				return true;
			}
			UIWidget component2 = gameObject.GetComponent<UIWidget>();
			if (component2 != null)
			{
				depth = component2.depth;
				return true;
			}
			bool flag = false;
			Transform transform = gameObject.transform;
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				int num = 0;
				if (UXElement.GetRootDepth(transform.GetChild(i).gameObject, ref num) && (!flag || num < depth))
				{
					depth = num;
					flag = true;
				}
				i++;
			}
			return flag;
		}

		private static void SetHierarchyDepth(GameObject gameObject, int depth, int rootDepth)
		{
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				int num = depth + component.depth - rootDepth;
				Vector3 localPosition = gameObject.transform.localPosition;
				localPosition.z = (float)(-(float)num) / 10000f;
				gameObject.transform.localPosition = localPosition;
				component.depth = num;
			}
			UIPanel component2 = gameObject.GetComponent<UIPanel>();
			if (component2 != null)
			{
				component2.depth = depth + component2.depth - rootDepth;
			}
			Transform transform = gameObject.transform;
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				UXElement.SetHierarchyDepth(transform.GetChild(i).gameObject, depth, rootDepth);
				i++;
			}
		}

		public GameObject CloneRoot(string name, GameObject parent)
		{
			if (this.root == null)
			{
				return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.root);
			gameObject.layer = parent.layer;
			UXUtils.AppendNameRecursively(gameObject, name, true);
			gameObject.SetActive(true);
			gameObject.transform.parent = parent.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			return gameObject;
		}

		public virtual void InternalDestroyComponent()
		{
		}

		public virtual void OnDestroyElement()
		{
			if (this.SendDestroyEvent)
			{
				Service.Get<EventManager>().SendEvent(EventId.ElementDestroyed, this);
			}
			this.enabler = null;
		}

		protected void SendClickEvent()
		{
			if (this.root != null)
			{
				Service.Get<EventManager>().SendEvent(EventId.ButtonClicked, this.root.name);
			}
		}

		public void InitAnimator()
		{
			if (this.Root != null)
			{
				this.animator = this.Root.GetComponent<Animator>();
				if (this.animator == null)
				{
					Service.Get<StaRTSLogger>().WarnFormat("Animator missing for : '{0}'", new object[]
					{
						this.root ? this.root.name : "null root"
					});
				}
			}
		}

		private bool IsAnimatorSet()
		{
			if (this.animator == null)
			{
				if (this.root != null)
				{
					Service.Get<StaRTSLogger>().WarnFormat("Animator not set. Call InitAnimator(). On: '{0}'", new object[]
					{
						this.root.name
					});
				}
				return false;
			}
			return true;
		}

		public void SetTrigger(string triggerName)
		{
			if (this.IsAnimatorSet())
			{
				this.animator.SetTrigger(triggerName);
			}
		}

		public void ResetTrigger(string triggerName)
		{
			if (this.IsAnimatorSet())
			{
				this.animator.ResetTrigger(triggerName);
			}
		}

		public bool IsCurrentAnimatorState(string stateName)
		{
			return this.IsAnimatorSet() && this.animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
		}

		public bool IsAnimatorTransitioning()
		{
			return this.IsAnimatorSet() && this.animator.IsInTransition(0);
		}

		public Vector3[] GetWorldCorners()
		{
			if (this.NGUIWidget != null)
			{
				return this.NGUIWidget.worldCorners;
			}
			return null;
		}

		public void SkipBoundsCalculations(bool skip)
		{
			Transform transform = this.root.transform;
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				UIWidget component = transform.GetChild(i).GetComponent<UIWidget>();
				if (component != null)
				{
					component.skipBoundsCalculations = skip;
				}
				i++;
			}
		}

		protected internal UXElement(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).AddUXButton((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).CloneRoot(Marshal.PtrToStringUni(*(IntPtr*)args), (GameObject)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).DisablePlayTween();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).EnablePlayTween();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).ColliderHeight);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).ColliderWidth);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).ColliderWidthUnscaled);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).ColliderXUnscaled);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).Enabled);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).GetUIWidget);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).Height);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).LocalPosition);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).LocalScale);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).OnElementClicked);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).OrigVisible);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).Position);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).Root);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).Tag);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).UXCamera);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).Visible);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).WidgetDepth);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).Width);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).WorldPosition);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).GetPanelAnchorOffset((UXAnchorSection)(*(int*)args)));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).GetPanelUnifiedAnchorTarget((UIPanel)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).GetRootPanel());
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).GetWorldCorners());
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).HasCollider());
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).InitAnimator();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).InitTweenComponent();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).InternalDestroyComponent();
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).InternalSetRoot((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).IsAnimatorSet());
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).IsAnimatorTransitioning());
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).IsCurrentAnimatorState(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXElement)GCHandledObjects.GCHandleToObject(instance)).IsPlayTweenEnabled());
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).PlayTween(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).RefreshPanel();
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).ResetPlayTweenTarget();
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).ResetTrigger(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).SendClickEvent();
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).Enabled = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).Height = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).LocalPosition = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).LocalScale = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).OnElementClicked = (UXButtonClickedDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).OrigVisible = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).Parent = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).Position = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).Tag = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).UXCamera = (UXCamera)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).Visible = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).WidgetDepth = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).Width = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			UXElement.SetHierarchyDepth((GameObject)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).SetPanelUnifiedAnchorBottomOffset(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).SetPanelUnifiedAnchorLeftOffset(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).SetPanelUnifiedAnchorOffsets(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3));
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).SetPanelUnifiedAnchorRightOffset(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).SetPanelUnifiedAnchorTopOffset(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).SetRootName(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).SetTrigger(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((UXElement)GCHandledObjects.GCHandleToObject(instance)).SkipBoundsCalculations(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
