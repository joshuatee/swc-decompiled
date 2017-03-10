using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/NGUI Event System (UICamera)"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour, IUnitySerializable
{
	public enum ControlScheme
	{
		Mouse,
		Touch,
		Controller
	}

	public enum ClickNotification
	{
		None,
		Always,
		BasedOnDelta
	}

	public class MouseOrTouch
	{
		public KeyCode key;

		public Vector2 pos;

		public Vector2 lastPos;

		public Vector2 delta;

		public Vector2 totalDelta;

		public Camera pressedCam;

		public GameObject last;

		public GameObject current;

		public GameObject pressed;

		public GameObject dragged;

		public float pressTime;

		public float clickTime;

		public UICamera.ClickNotification clickNotification;

		public bool touchBegan;

		public bool pressStarted;

		public bool dragStarted;

		public int ignoreDelta;

		public float deltaTime
		{
			get
			{
				return RealTime.time - this.pressTime;
			}
		}

		public bool isOverUI
		{
			get
			{
				return this.current != null && this.current != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(this.current) != null;
			}
		}

		public MouseOrTouch()
		{
			this.clickNotification = UICamera.ClickNotification.Always;
			this.touchBegan = true;
			base..ctor();
		}

		protected internal MouseOrTouch(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UICamera.MouseOrTouch)GCHandledObjects.GCHandleToObject(instance)).deltaTime);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UICamera.MouseOrTouch)GCHandledObjects.GCHandleToObject(instance)).isOverUI);
		}
	}

	public enum EventType
	{
		World_3D,
		UI_3D,
		World_2D,
		UI_2D
	}

	public delegate bool GetKeyStateFunc(KeyCode key);

	public delegate float GetAxisFunc(string name);

	public delegate bool GetAnyKeyFunc();

	public delegate void OnScreenResize();

	public delegate void OnCustomInput();

	public delegate void OnSchemeChange();

	public delegate void MoveDelegate(Vector2 delta);

	public delegate void VoidDelegate(GameObject go);

	public delegate void BoolDelegate(GameObject go, bool state);

	public delegate void FloatDelegate(GameObject go, float delta);

	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

	private struct DepthEntry
	{
		public int depth;

		public RaycastHit hit;

		public Vector3 point;

		public GameObject go;
	}

	public class Touch
	{
		public int fingerId;

		public TouchPhase phase;

		public Vector2 position;

		public int tapCount;

		public Touch()
		{
		}

		protected internal Touch(UIntPtr dummy)
		{
		}
	}

	public delegate int GetTouchCountCallback();

	public delegate UICamera.Touch GetTouchCallback(int index);

	[CompilerGenerated]
	[System.Serializable]
	private sealed class <>c
	{
		public static readonly UICamera.<>c <>9 = new UICamera.<>c();

		public static BetterList<UICamera.DepthEntry>.CompareFunc <>9__159_0;

		public static BetterList<UICamera.DepthEntry>.CompareFunc <>9__159_1;

		internal int <Raycast>b__159_0(UICamera.DepthEntry r1, UICamera.DepthEntry r2)
		{
			return r2.depth.CompareTo(r1.depth);
		}

		internal int <Raycast>b__159_1(UICamera.DepthEntry r1, UICamera.DepthEntry r2)
		{
			return r2.depth.CompareTo(r1.depth);
		}
	}

	public static BetterList<UICamera> list = new BetterList<UICamera>();

	public static UICamera.GetKeyStateFunc GetKeyDown = new UICamera.GetKeyStateFunc(Input.GetKeyDown);

	public static UICamera.GetKeyStateFunc GetKeyUp = new UICamera.GetKeyStateFunc(Input.GetKeyUp);

	public static UICamera.GetKeyStateFunc GetKey = new UICamera.GetKeyStateFunc(Input.GetKey);

	public static UICamera.GetAxisFunc GetAxis = new UICamera.GetAxisFunc(Input.GetAxis);

	public static UICamera.GetAnyKeyFunc GetAnyKeyDown;

	public static UICamera.OnScreenResize onScreenResize;

	public UICamera.EventType eventType;

	public bool eventsGoToColliders;

	public LayerMask eventReceiverMask;

	public bool debug;

	public bool useMouse;

	public bool useTouch;

	public bool allowMultiTouch;

	public bool useKeyboard;

	public bool useController;

	public bool stickyTooltip;

	public float tooltipDelay;

	public bool longPressTooltip;

	public float mouseDragThreshold;

	public float mouseClickThreshold;

	public float touchDragThreshold;

	public float touchClickThreshold;

	public float rangeDistance;

	public string horizontalAxisName;

	public string verticalAxisName;

	public string horizontalPanAxisName;

	public string verticalPanAxisName;

	public string scrollAxisName;

	public bool commandClick;

	public KeyCode submitKey0;

	public KeyCode submitKey1;

	public KeyCode cancelKey0;

	public KeyCode cancelKey1;

	public bool autoHideCursor;

	public static UICamera.OnCustomInput onCustomInput;

	public static bool showTooltips = true;

	private static bool mDisableController = false;

	private static Vector2 mLastPos = Vector2.zero;

	public static Vector3 lastWorldPosition = Vector3.zero;

	public static RaycastHit lastHit;

	public static UICamera current = null;

	public static Camera currentCamera = null;

	public static UICamera.OnSchemeChange onSchemeChange;

	private static UICamera.ControlScheme mLastScheme = UICamera.ControlScheme.Mouse;

	public static int currentTouchID = -100;

	private static KeyCode mCurrentKey = KeyCode.Alpha0;

	public static UICamera.MouseOrTouch currentTouch = null;

	private static bool mInputFocus = false;

	private static GameObject mGenericHandler;

	public static GameObject fallThrough;

	public static UICamera.VoidDelegate onClick;

	public static UICamera.VoidDelegate onDoubleClick;

	public static UICamera.BoolDelegate onHover;

	public static UICamera.BoolDelegate onPress;

	public static UICamera.BoolDelegate onSelect;

	public static UICamera.FloatDelegate onScroll;

	public static UICamera.VectorDelegate onDrag;

	public static UICamera.VoidDelegate onDragStart;

	public static UICamera.ObjectDelegate onDragOver;

	public static UICamera.ObjectDelegate onDragOut;

	public static UICamera.VoidDelegate onDragEnd;

	public static UICamera.ObjectDelegate onDrop;

	public static UICamera.KeyCodeDelegate onKey;

	public static UICamera.KeyCodeDelegate onNavigate;

	public static UICamera.VectorDelegate onPan;

	public static UICamera.BoolDelegate onTooltip;

	public static UICamera.MoveDelegate onMouseMove;

	private static UICamera.MouseOrTouch[] mMouse = new UICamera.MouseOrTouch[]
	{
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch()
	};

	public static UICamera.MouseOrTouch controller = new UICamera.MouseOrTouch();

	public static List<UICamera.MouseOrTouch> activeTouches = new List<UICamera.MouseOrTouch>();

	private static List<int> mTouchIDs = new List<int>();

	private static int mWidth = 0;

	private static int mHeight = 0;

	private static GameObject mTooltip = null;

	private Camera mCam;

	private static float mTooltipTime = 0f;

	private float mNextRaycast;

	public static bool isDragging = false;

	private static GameObject mRayHitObject;

	private static GameObject mHover;

	private static GameObject mSelected;

	private static UICamera.DepthEntry mHit = default(UICamera.DepthEntry);

	private static BetterList<UICamera.DepthEntry> mHits = new BetterList<UICamera.DepthEntry>();

	private static Plane m2DPlane = new Plane(Vector3.back, 0f);

	private static float mNextEvent = 0f;

	private static int mNotifying = 0;

	private static bool mUsingTouchEvents = true;

	public static UICamera.GetTouchCountCallback GetInputTouchCount;

	public static UICamera.GetTouchCallback GetInputTouch;

	[Obsolete("Use new OnDragStart / OnDragOver / OnDragOut / OnDragEnd events instead")]
	public bool stickyPress
	{
		get
		{
			return true;
		}
	}

	public static bool disableController
	{
		get
		{
			return UICamera.mDisableController && !UIPopupList.isOpen;
		}
		set
		{
			UICamera.mDisableController = value;
		}
	}

	[Obsolete("Use lastEventPosition instead. It handles controller input properly.")]
	public static Vector2 lastTouchPosition
	{
		get
		{
			return UICamera.mLastPos;
		}
		set
		{
			UICamera.mLastPos = value;
		}
	}

	public static Vector2 lastEventPosition
	{
		get
		{
			UICamera.ControlScheme currentScheme = UICamera.currentScheme;
			if (currentScheme == UICamera.ControlScheme.Controller)
			{
				GameObject hoveredObject = UICamera.hoveredObject;
				if (hoveredObject != null)
				{
					Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(hoveredObject.transform);
					Camera camera = NGUITools.FindCameraForLayer(hoveredObject.layer);
					return camera.WorldToScreenPoint(bounds.center);
				}
			}
			return UICamera.mLastPos;
		}
		set
		{
			UICamera.mLastPos = value;
		}
	}

	public static UICamera first
	{
		get
		{
			if (UICamera.list == null || UICamera.list.size == 0)
			{
				return null;
			}
			return UICamera.list[0];
		}
	}

	public static UICamera.ControlScheme currentScheme
	{
		get
		{
			if (UICamera.mCurrentKey == KeyCode.None)
			{
				return UICamera.ControlScheme.Touch;
			}
			if (UICamera.mCurrentKey >= KeyCode.JoystickButton0)
			{
				return UICamera.ControlScheme.Controller;
			}
			if (UICamera.current != null && UICamera.mLastScheme == UICamera.ControlScheme.Controller && (UICamera.mCurrentKey == UICamera.current.submitKey0 || UICamera.mCurrentKey == UICamera.current.submitKey1))
			{
				return UICamera.ControlScheme.Controller;
			}
			return UICamera.ControlScheme.Mouse;
		}
		set
		{
			if (value == UICamera.ControlScheme.Mouse)
			{
				UICamera.currentKey = KeyCode.Mouse0;
			}
			else if (value == UICamera.ControlScheme.Controller)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
			}
			else if (value == UICamera.ControlScheme.Touch)
			{
				UICamera.currentKey = KeyCode.None;
			}
			else
			{
				UICamera.currentKey = KeyCode.Alpha0;
			}
			UICamera.mLastScheme = value;
		}
	}

	public static KeyCode currentKey
	{
		get
		{
			return UICamera.mCurrentKey;
		}
		set
		{
			if (UICamera.mCurrentKey != value)
			{
				UICamera.ControlScheme controlScheme = UICamera.mLastScheme;
				UICamera.mCurrentKey = value;
				UICamera.mLastScheme = UICamera.currentScheme;
				if (controlScheme != UICamera.mLastScheme)
				{
					UICamera.HideTooltip();
					if (UICamera.mLastScheme == UICamera.ControlScheme.Mouse)
					{
						Cursor.lockState = CursorLockMode.None;
						Cursor.visible = true;
					}
					else if (UICamera.current != null && UICamera.current.autoHideCursor)
					{
						Cursor.visible = false;
						Cursor.lockState = CursorLockMode.Locked;
						UICamera.mMouse[0].ignoreDelta = 2;
					}
					if (UICamera.onSchemeChange != null)
					{
						UICamera.onSchemeChange();
					}
				}
			}
		}
	}

	public static Ray currentRay
	{
		get
		{
			if (!(UICamera.currentCamera != null) || UICamera.currentTouch == null)
			{
				return default(Ray);
			}
			return UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		}
	}

	public static bool inputHasFocus
	{
		get
		{
			if (UICamera.mInputFocus)
			{
				if (UICamera.mSelected && UICamera.mSelected.activeInHierarchy)
				{
					return true;
				}
				UICamera.mInputFocus = false;
			}
			return false;
		}
	}

	[Obsolete("Use delegates instead such as UICamera.onClick, UICamera.onHover, etc.")]
	public static GameObject genericEventHandler
	{
		get
		{
			return UICamera.mGenericHandler;
		}
		set
		{
			UICamera.mGenericHandler = value;
		}
	}

	private bool handlesEvents
	{
		get
		{
			return UICamera.eventHandler == this;
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.GetComponent<Camera>();
			}
			return this.mCam;
		}
	}

	public static GameObject tooltipObject
	{
		get
		{
			return UICamera.mTooltip;
		}
	}

	public static bool isOverUI
	{
		get
		{
			if (UICamera.currentTouch != null)
			{
				return UICamera.currentTouch.isOverUI;
			}
			int i = 0;
			int count = UICamera.activeTouches.Count;
			while (i < count)
			{
				UICamera.MouseOrTouch mouseOrTouch = UICamera.activeTouches[i];
				if (mouseOrTouch.pressed != null && mouseOrTouch.pressed != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(mouseOrTouch.pressed) != null)
				{
					return true;
				}
				i++;
			}
			return (UICamera.mMouse[0].current != null && UICamera.mMouse[0].current != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(UICamera.mMouse[0].current) != null) || (UICamera.controller.pressed != null && UICamera.controller.pressed != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(UICamera.controller.pressed) != null);
		}
	}

	public static GameObject hoveredObject
	{
		get
		{
			if (UICamera.currentTouch != null && UICamera.currentTouch.dragStarted)
			{
				return UICamera.currentTouch.current;
			}
			if (UICamera.mHover && UICamera.mHover.activeInHierarchy)
			{
				return UICamera.mHover;
			}
			UICamera.mHover = null;
			return null;
		}
		set
		{
			if (UICamera.mHover == value)
			{
				return;
			}
			bool flag = false;
			UICamera uICamera = UICamera.current;
			if (UICamera.currentTouch == null)
			{
				flag = true;
				UICamera.currentTouchID = -100;
				UICamera.currentTouch = UICamera.controller;
			}
			UICamera.ShowTooltip(null);
			if (UICamera.mSelected && UICamera.currentScheme == UICamera.ControlScheme.Controller)
			{
				UICamera.Notify(UICamera.mSelected, "OnSelect", false);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, false);
				}
				UICamera.mSelected = null;
			}
			if (UICamera.mHover)
			{
				UICamera.Notify(UICamera.mHover, "OnHover", false);
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.mHover, false);
				}
			}
			UICamera.mHover = value;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			if (UICamera.mHover)
			{
				if (UICamera.mHover != UICamera.controller.current && UICamera.mHover.GetComponent<UIKeyNavigation>() != null)
				{
					UICamera.controller.current = UICamera.mHover;
				}
				if (flag)
				{
					UICamera uICamera2 = (UICamera.mHover != null) ? UICamera.FindCameraForLayer(UICamera.mHover.layer) : UICamera.list[0];
					if (uICamera2 != null)
					{
						UICamera.current = uICamera2;
						UICamera.currentCamera = uICamera2.cachedCamera;
					}
				}
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.mHover, true);
				}
				UICamera.Notify(UICamera.mHover, "OnHover", true);
			}
			if (flag)
			{
				UICamera.current = uICamera;
				UICamera.currentCamera = ((uICamera != null) ? uICamera.cachedCamera : null);
				UICamera.currentTouch = null;
				UICamera.currentTouchID = -100;
			}
		}
	}

	public static GameObject controllerNavigationObject
	{
		get
		{
			if (UICamera.controller.current && UICamera.controller.current.activeInHierarchy)
			{
				return UICamera.controller.current;
			}
			if (UICamera.currentScheme == UICamera.ControlScheme.Controller && UICamera.current != null && UICamera.current.useController && UIKeyNavigation.list.size > 0)
			{
				for (int i = 0; i < UIKeyNavigation.list.size; i++)
				{
					UIKeyNavigation uIKeyNavigation = UIKeyNavigation.list[i];
					if (uIKeyNavigation && uIKeyNavigation.constraint != UIKeyNavigation.Constraint.Explicit && uIKeyNavigation.startsSelected)
					{
						UICamera.hoveredObject = uIKeyNavigation.gameObject;
						UICamera.controller.current = UICamera.mHover;
						return UICamera.mHover;
					}
				}
				if (UICamera.mHover == null)
				{
					for (int j = 0; j < UIKeyNavigation.list.size; j++)
					{
						UIKeyNavigation uIKeyNavigation2 = UIKeyNavigation.list[j];
						if (uIKeyNavigation2 && uIKeyNavigation2.constraint != UIKeyNavigation.Constraint.Explicit)
						{
							UICamera.hoveredObject = uIKeyNavigation2.gameObject;
							UICamera.controller.current = UICamera.mHover;
							return UICamera.mHover;
						}
					}
				}
			}
			UICamera.controller.current = null;
			return null;
		}
		set
		{
			if (UICamera.controller.current != value && UICamera.controller.current)
			{
				UICamera.Notify(UICamera.controller.current, "OnHover", false);
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.controller.current, false);
				}
				UICamera.controller.current = null;
			}
			UICamera.hoveredObject = value;
		}
	}

	public static GameObject selectedObject
	{
		get
		{
			if (UICamera.mSelected && UICamera.mSelected.activeInHierarchy)
			{
				return UICamera.mSelected;
			}
			UICamera.mSelected = null;
			return null;
		}
		set
		{
			if (UICamera.mSelected == value)
			{
				UICamera.hoveredObject = value;
				UICamera.controller.current = value;
				return;
			}
			UICamera.ShowTooltip(null);
			bool flag = false;
			UICamera uICamera = UICamera.current;
			if (UICamera.currentTouch == null)
			{
				flag = true;
				UICamera.currentTouchID = -100;
				UICamera.currentTouch = UICamera.controller;
			}
			UICamera.mInputFocus = false;
			if (UICamera.mSelected)
			{
				UICamera.Notify(UICamera.mSelected, "OnSelect", false);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, false);
				}
			}
			UICamera.mSelected = value;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			if (value != null)
			{
				UIKeyNavigation component = value.GetComponent<UIKeyNavigation>();
				if (component != null)
				{
					UICamera.controller.current = value;
				}
			}
			if (UICamera.mSelected & flag)
			{
				UICamera uICamera2 = (UICamera.mSelected != null) ? UICamera.FindCameraForLayer(UICamera.mSelected.layer) : UICamera.list[0];
				if (uICamera2 != null)
				{
					UICamera.current = uICamera2;
					UICamera.currentCamera = uICamera2.cachedCamera;
				}
			}
			if (UICamera.mSelected)
			{
				UICamera.mInputFocus = (UICamera.mSelected.activeInHierarchy && UICamera.mSelected.GetComponent<UIInput>() != null);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, true);
				}
				UICamera.Notify(UICamera.mSelected, "OnSelect", true);
			}
			if (flag)
			{
				UICamera.current = uICamera;
				UICamera.currentCamera = ((uICamera != null) ? uICamera.cachedCamera : null);
				UICamera.currentTouch = null;
				UICamera.currentTouchID = -100;
			}
		}
	}

	[Obsolete("Use either 'CountInputSources()' or 'activeTouches.Count'")]
	public static int touchCount
	{
		get
		{
			return UICamera.CountInputSources();
		}
	}

	public static int dragCount
	{
		get
		{
			int num = 0;
			int i = 0;
			int count = UICamera.activeTouches.Count;
			while (i < count)
			{
				UICamera.MouseOrTouch mouseOrTouch = UICamera.activeTouches[i];
				if (mouseOrTouch.dragged != null)
				{
					num++;
				}
				i++;
			}
			for (int j = 0; j < UICamera.mMouse.Length; j++)
			{
				if (UICamera.mMouse[j].dragged != null)
				{
					num++;
				}
			}
			if (UICamera.controller.dragged != null)
			{
				num++;
			}
			return num;
		}
	}

	public static Camera mainCamera
	{
		get
		{
			UICamera eventHandler = UICamera.eventHandler;
			if (!(eventHandler != null))
			{
				return null;
			}
			return eventHandler.cachedCamera;
		}
	}

	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < UICamera.list.size; i++)
			{
				UICamera uICamera = UICamera.list.buffer[i];
				if (!(uICamera == null) && uICamera.enabled && NGUITools.GetActive(uICamera.gameObject))
				{
					return uICamera;
				}
			}
			return null;
		}
	}

	public static bool IsPressed(GameObject go)
	{
		for (int i = 0; i < 3; i++)
		{
			if (UICamera.mMouse[i].pressed == go)
			{
				return true;
			}
		}
		int j = 0;
		int count = UICamera.activeTouches.Count;
		while (j < count)
		{
			UICamera.MouseOrTouch mouseOrTouch = UICamera.activeTouches[j];
			if (mouseOrTouch.pressed == go)
			{
				return true;
			}
			j++;
		}
		return UICamera.controller.pressed == go;
	}

	public static int CountInputSources()
	{
		int num = 0;
		int i = 0;
		int count = UICamera.activeTouches.Count;
		while (i < count)
		{
			UICamera.MouseOrTouch mouseOrTouch = UICamera.activeTouches[i];
			if (mouseOrTouch.pressed != null)
			{
				num++;
			}
			i++;
		}
		for (int j = 0; j < UICamera.mMouse.Length; j++)
		{
			if (UICamera.mMouse[j].pressed != null)
			{
				num++;
			}
		}
		if (UICamera.controller.pressed != null)
		{
			num++;
		}
		return num;
	}

	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	private static Rigidbody FindRootRigidbody(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody component = trans.GetComponent<Rigidbody>();
			if (component != null)
			{
				return component;
			}
			trans = trans.parent;
		}
		return null;
	}

	private static Rigidbody2D FindRootRigidbody2D(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody2D component = trans.GetComponent<Rigidbody2D>();
			if (component != null)
			{
				return component;
			}
			trans = trans.parent;
		}
		return null;
	}

	public static void Raycast(UICamera.MouseOrTouch touch)
	{
		if (!UICamera.Raycast(touch.pos))
		{
			UICamera.mRayHitObject = UICamera.fallThrough;
		}
		if (UICamera.mRayHitObject == null)
		{
			UICamera.mRayHitObject = UICamera.mGenericHandler;
		}
		touch.last = touch.current;
		touch.current = UICamera.mRayHitObject;
		UICamera.mLastPos = touch.pos;
	}

	public static bool Raycast(Vector3 inPos)
	{
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uICamera = UICamera.list.buffer[i];
			if (uICamera.enabled && NGUITools.GetActive(uICamera.gameObject))
			{
				UICamera.currentCamera = uICamera.cachedCamera;
				Vector3 vector = UICamera.currentCamera.ScreenToViewportPoint(inPos);
				if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y) && vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f)
				{
					Ray ray = UICamera.currentCamera.ScreenPointToRay(inPos);
					int layerMask = UICamera.currentCamera.cullingMask & uICamera.eventReceiverMask;
					float num = (uICamera.rangeDistance > 0f) ? uICamera.rangeDistance : (UICamera.currentCamera.farClipPlane - UICamera.currentCamera.nearClipPlane);
					if (uICamera.eventType == UICamera.EventType.World_3D)
					{
						if (Physics.Raycast(ray, out UICamera.lastHit, num, layerMask))
						{
							UICamera.lastWorldPosition = UICamera.lastHit.point;
							UICamera.mRayHitObject = UICamera.lastHit.collider.gameObject;
							if (!UICamera.list[0].eventsGoToColliders)
							{
								Rigidbody rigidbody = UICamera.FindRootRigidbody(UICamera.mRayHitObject.transform);
								if (rigidbody != null)
								{
									UICamera.mRayHitObject = rigidbody.gameObject;
								}
							}
							return true;
						}
					}
					else if (uICamera.eventType == UICamera.EventType.UI_3D)
					{
						RaycastHit[] array = Physics.RaycastAll(ray, num, layerMask);
						if (array.Length > 1)
						{
							int j = 0;
							while (j < array.Length)
							{
								GameObject gameObject = array[j].collider.gameObject;
								UIWidget component = gameObject.GetComponent<UIWidget>();
								if (component != null)
								{
									if (component.isVisible)
									{
										if (component.hitCheck == null || component.hitCheck(array[j].point))
										{
											goto IL_219;
										}
									}
								}
								else
								{
									UIRect uIRect = NGUITools.FindInParents<UIRect>(gameObject);
									if (!(uIRect != null) || uIRect.finalAlpha >= 0.001f)
									{
										goto IL_219;
									}
								}
								IL_292:
								j++;
								continue;
								IL_219:
								UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject);
								if (UICamera.mHit.depth != 2147483647)
								{
									UICamera.mHit.hit = array[j];
									UICamera.mHit.point = array[j].point;
									UICamera.mHit.go = array[j].collider.gameObject;
									UICamera.mHits.Add(UICamera.mHit);
									goto IL_292;
								}
								goto IL_292;
							}
							BetterList<UICamera.DepthEntry> arg_2C7_0 = UICamera.mHits;
							BetterList<UICamera.DepthEntry>.CompareFunc arg_2C7_1;
							if ((arg_2C7_1 = UICamera.<>c.<>9__159_0) == null)
							{
								arg_2C7_1 = (UICamera.<>c.<>9__159_0 = new BetterList<UICamera.DepthEntry>.CompareFunc(UICamera.<>c.<>9.<Raycast>b__159_0));
							}
							arg_2C7_0.Sort(arg_2C7_1);
							for (int k = 0; k < UICamera.mHits.size; k++)
							{
								if (UICamera.IsVisible(ref UICamera.mHits.buffer[k]))
								{
									UICamera.lastHit = UICamera.mHits[k].hit;
									UICamera.mRayHitObject = UICamera.mHits[k].go;
									UICamera.lastWorldPosition = UICamera.mHits[k].point;
									UICamera.mHits.Clear();
									return true;
								}
							}
							UICamera.mHits.Clear();
						}
						else if (array.Length == 1)
						{
							GameObject gameObject2 = array[0].collider.gameObject;
							UIWidget component2 = gameObject2.GetComponent<UIWidget>();
							if (component2 != null)
							{
								if (!component2.isVisible)
								{
									goto IL_6EF;
								}
								if (component2.hitCheck != null && !component2.hitCheck(array[0].point))
								{
									goto IL_6EF;
								}
							}
							else
							{
								UIRect uIRect2 = NGUITools.FindInParents<UIRect>(gameObject2);
								if (uIRect2 != null && uIRect2.finalAlpha < 0.001f)
								{
									goto IL_6EF;
								}
							}
							if (UICamera.IsVisible(array[0].point, array[0].collider.gameObject))
							{
								UICamera.lastHit = array[0];
								UICamera.lastWorldPosition = array[0].point;
								UICamera.mRayHitObject = UICamera.lastHit.collider.gameObject;
								return true;
							}
						}
					}
					else if (uICamera.eventType == UICamera.EventType.World_2D)
					{
						if (UICamera.m2DPlane.Raycast(ray, out num))
						{
							Vector3 point = ray.GetPoint(num);
							Collider2D collider2D = Physics2D.OverlapPoint(point, layerMask);
							if (collider2D)
							{
								UICamera.lastWorldPosition = point;
								UICamera.mRayHitObject = collider2D.gameObject;
								if (!uICamera.eventsGoToColliders)
								{
									Rigidbody2D rigidbody2D = UICamera.FindRootRigidbody2D(UICamera.mRayHitObject.transform);
									if (rigidbody2D != null)
									{
										UICamera.mRayHitObject = rigidbody2D.gameObject;
									}
								}
								return true;
							}
						}
					}
					else if (uICamera.eventType == UICamera.EventType.UI_2D && UICamera.m2DPlane.Raycast(ray, out num))
					{
						UICamera.lastWorldPosition = ray.GetPoint(num);
						Collider2D[] array2 = Physics2D.OverlapPointAll(UICamera.lastWorldPosition, layerMask);
						if (array2.Length > 1)
						{
							int l = 0;
							while (l < array2.Length)
							{
								GameObject gameObject3 = array2[l].gameObject;
								UIWidget component3 = gameObject3.GetComponent<UIWidget>();
								if (component3 != null)
								{
									if (component3.isVisible)
									{
										if (component3.hitCheck == null || component3.hitCheck(UICamera.lastWorldPosition))
										{
											goto IL_583;
										}
									}
								}
								else
								{
									UIRect uIRect3 = NGUITools.FindInParents<UIRect>(gameObject3);
									if (!(uIRect3 != null) || uIRect3.finalAlpha >= 0.001f)
									{
										goto IL_583;
									}
								}
								IL_5CF:
								l++;
								continue;
								IL_583:
								UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject3);
								if (UICamera.mHit.depth != 2147483647)
								{
									UICamera.mHit.go = gameObject3;
									UICamera.mHit.point = UICamera.lastWorldPosition;
									UICamera.mHits.Add(UICamera.mHit);
									goto IL_5CF;
								}
								goto IL_5CF;
							}
							BetterList<UICamera.DepthEntry> arg_604_0 = UICamera.mHits;
							BetterList<UICamera.DepthEntry>.CompareFunc arg_604_1;
							if ((arg_604_1 = UICamera.<>c.<>9__159_1) == null)
							{
								arg_604_1 = (UICamera.<>c.<>9__159_1 = new BetterList<UICamera.DepthEntry>.CompareFunc(UICamera.<>c.<>9.<Raycast>b__159_1));
							}
							arg_604_0.Sort(arg_604_1);
							for (int m = 0; m < UICamera.mHits.size; m++)
							{
								if (UICamera.IsVisible(ref UICamera.mHits.buffer[m]))
								{
									UICamera.mRayHitObject = UICamera.mHits[m].go;
									UICamera.mHits.Clear();
									return true;
								}
							}
							UICamera.mHits.Clear();
						}
						else if (array2.Length == 1)
						{
							GameObject gameObject4 = array2[0].gameObject;
							UIWidget component4 = gameObject4.GetComponent<UIWidget>();
							if (component4 != null)
							{
								if (!component4.isVisible)
								{
									goto IL_6EF;
								}
								if (component4.hitCheck != null && !component4.hitCheck(UICamera.lastWorldPosition))
								{
									goto IL_6EF;
								}
							}
							else
							{
								UIRect uIRect4 = NGUITools.FindInParents<UIRect>(gameObject4);
								if (uIRect4 != null && uIRect4.finalAlpha < 0.001f)
								{
									goto IL_6EF;
								}
							}
							if (UICamera.IsVisible(UICamera.lastWorldPosition, gameObject4))
							{
								UICamera.mRayHitObject = gameObject4;
								return true;
							}
						}
					}
				}
			}
			IL_6EF:;
		}
		return false;
	}

	private static bool IsVisible(Vector3 worldPoint, GameObject go)
	{
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(go);
		while (uIPanel != null)
		{
			if (!uIPanel.IsVisible(worldPoint))
			{
				return false;
			}
			uIPanel = uIPanel.parentPanel;
		}
		return true;
	}

	private static bool IsVisible(ref UICamera.DepthEntry de)
	{
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(de.go);
		while (uIPanel != null)
		{
			if (!uIPanel.IsVisible(de.point))
			{
				return false;
			}
			uIPanel = uIPanel.parentPanel;
		}
		return true;
	}

	public static bool IsHighlighted(GameObject go)
	{
		return UICamera.hoveredObject == go;
	}

	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uICamera = UICamera.list.buffer[i];
			Camera cachedCamera = uICamera.cachedCamera;
			if (cachedCamera != null && (cachedCamera.cullingMask & num) != 0)
			{
				return uICamera;
			}
		}
		return null;
	}

	private static int GetDirection(KeyCode up, KeyCode down)
	{
		if (UICamera.GetKeyDown(up))
		{
			UICamera.currentKey = up;
			return 1;
		}
		if (UICamera.GetKeyDown(down))
		{
			UICamera.currentKey = down;
			return -1;
		}
		return 0;
	}

	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (UICamera.GetKeyDown(up0))
		{
			UICamera.currentKey = up0;
			return 1;
		}
		if (UICamera.GetKeyDown(up1))
		{
			UICamera.currentKey = up1;
			return 1;
		}
		if (UICamera.GetKeyDown(down0))
		{
			UICamera.currentKey = down0;
			return -1;
		}
		if (UICamera.GetKeyDown(down1))
		{
			UICamera.currentKey = down1;
			return -1;
		}
		return 0;
	}

	private static int GetDirection(string axis)
	{
		float time = RealTime.time;
		if (UICamera.mNextEvent < time && !string.IsNullOrEmpty(axis))
		{
			float num = UICamera.GetAxis(axis);
			if (num > 0.75f)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
				UICamera.mNextEvent = time + 0.25f;
				return 1;
			}
			if (num < -0.75f)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
				UICamera.mNextEvent = time + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	public static void Notify(GameObject go, string funcName, object obj)
	{
		if (UICamera.mNotifying > 10)
		{
			return;
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller && UIPopupList.isOpen && UIPopupList.current.source == go && UIPopupList.isOpen)
		{
			go = UIPopupList.current.gameObject;
		}
		if (go && go.activeInHierarchy)
		{
			UICamera.mNotifying++;
			go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			if (UICamera.mGenericHandler != null && UICamera.mGenericHandler != go)
			{
				UICamera.mGenericHandler.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			}
			UICamera.mNotifying--;
		}
	}

	public static UICamera.MouseOrTouch GetMouse(int button)
	{
		return UICamera.mMouse[button];
	}

	public static UICamera.MouseOrTouch GetTouch(int id, bool createIfMissing = false)
	{
		if (id < 0)
		{
			return UICamera.GetMouse(-id - 1);
		}
		int i = 0;
		int count = UICamera.mTouchIDs.Count;
		while (i < count)
		{
			if (UICamera.mTouchIDs[i] == id)
			{
				return UICamera.activeTouches[i];
			}
			i++;
		}
		if (createIfMissing)
		{
			UICamera.MouseOrTouch mouseOrTouch = new UICamera.MouseOrTouch();
			mouseOrTouch.pressTime = RealTime.time;
			mouseOrTouch.touchBegan = true;
			UICamera.activeTouches.Add(mouseOrTouch);
			UICamera.mTouchIDs.Add(id);
			return mouseOrTouch;
		}
		return null;
	}

	public static void RemoveTouch(int id)
	{
		int i = 0;
		int count = UICamera.mTouchIDs.Count;
		while (i < count)
		{
			if (UICamera.mTouchIDs[i] == id)
			{
				UICamera.mTouchIDs.RemoveAt(i);
				UICamera.activeTouches.RemoveAt(i);
				return;
			}
			i++;
		}
	}

	private void Awake()
	{
		UICamera.mWidth = Screen.width;
		UICamera.mHeight = Screen.height;
		UICamera.currentScheme = UICamera.ControlScheme.Touch;
		UICamera.mMouse[0].pos = Input.mousePosition;
		for (int i = 1; i < 3; i++)
		{
			UICamera.mMouse[i].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[i].lastPos = UICamera.mMouse[0].pos;
		}
		UICamera.mLastPos = UICamera.mMouse[0].pos;
	}

	private void OnEnable()
	{
		UICamera.list.Add(this);
		UICamera.list.Sort(new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc));
	}

	private void OnDisable()
	{
		UICamera.list.Remove(this);
	}

	private void Start()
	{
		if (this.eventType != UICamera.EventType.World_3D && this.cachedCamera.transparencySortMode != TransparencySortMode.Orthographic)
		{
			this.cachedCamera.transparencySortMode = TransparencySortMode.Orthographic;
		}
		if (Application.isPlaying)
		{
			if (UICamera.fallThrough == null)
			{
				UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
				if (uIRoot != null)
				{
					UICamera.fallThrough = uIRoot.gameObject;
				}
				else
				{
					Transform transform = base.transform;
					UICamera.fallThrough = ((transform.parent != null) ? transform.parent.gameObject : base.gameObject);
				}
			}
			this.cachedCamera.eventMask = 0;
		}
	}

	private void Update()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		UICamera.current = this;
		NGUIDebug.debugRaycast = this.debug;
		if (this.useTouch)
		{
			this.ProcessTouches();
		}
		else if (this.useMouse)
		{
			this.ProcessMouse();
		}
		if (UICamera.onCustomInput != null)
		{
			UICamera.onCustomInput();
		}
		if ((this.useKeyboard || this.useController) && !UICamera.disableController)
		{
			this.ProcessOthers();
		}
		if (this.useMouse && UICamera.mHover != null)
		{
			float num = (!string.IsNullOrEmpty(this.scrollAxisName)) ? UICamera.GetAxis(this.scrollAxisName) : 0f;
			if (num != 0f)
			{
				if (UICamera.onScroll != null)
				{
					UICamera.onScroll(UICamera.mHover, num);
				}
				UICamera.Notify(UICamera.mHover, "OnScroll", num);
			}
			if (UICamera.showTooltips && UICamera.mTooltipTime != 0f && !UIPopupList.isOpen && UICamera.mMouse[0].dragged == null && (UICamera.mTooltipTime < RealTime.time || UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift)))
			{
				UICamera.currentTouch = UICamera.mMouse[0];
				UICamera.currentTouchID = -1;
				UICamera.ShowTooltip(UICamera.mHover);
			}
		}
		if (UICamera.mTooltip != null && !NGUITools.GetActive(UICamera.mTooltip))
		{
			UICamera.ShowTooltip(null);
		}
		UICamera.current = null;
		UICamera.currentTouchID = -100;
	}

	private void LateUpdate()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		int width = Screen.width;
		int height = Screen.height;
		if (width != UICamera.mWidth || height != UICamera.mHeight)
		{
			UICamera.mWidth = width;
			UICamera.mHeight = height;
			UIRoot.Broadcast("UpdateAnchors");
			if (UICamera.onScreenResize != null)
			{
				UICamera.onScreenResize();
			}
		}
	}

	public void ProcessMouse()
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < 3; i++)
		{
			if (Input.GetMouseButtonDown(i))
			{
				UICamera.currentKey = KeyCode.Mouse0 + i;
				flag2 = true;
				flag = true;
			}
			else if (Input.GetMouseButton(i))
			{
				UICamera.currentKey = KeyCode.Mouse0 + i;
				flag = true;
			}
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Touch)
		{
			return;
		}
		UICamera.currentTouch = UICamera.mMouse[0];
		Vector2 vector = Input.mousePosition;
		if (UICamera.currentTouch.ignoreDelta == 0)
		{
			UICamera.currentTouch.delta = vector - UICamera.currentTouch.pos;
		}
		else
		{
			UICamera.currentTouch.ignoreDelta--;
			UICamera.currentTouch.delta.x = 0f;
			UICamera.currentTouch.delta.y = 0f;
		}
		float sqrMagnitude = UICamera.currentTouch.delta.sqrMagnitude;
		UICamera.currentTouch.pos = vector;
		UICamera.mLastPos = vector;
		bool flag3 = false;
		if (UICamera.currentScheme != UICamera.ControlScheme.Mouse)
		{
			if (sqrMagnitude < 0.001f)
			{
				return;
			}
			UICamera.currentKey = KeyCode.Mouse0;
			flag3 = true;
		}
		else if (sqrMagnitude > 0.001f)
		{
			flag3 = true;
		}
		for (int j = 1; j < 3; j++)
		{
			UICamera.mMouse[j].pos = UICamera.currentTouch.pos;
			UICamera.mMouse[j].delta = UICamera.currentTouch.delta;
		}
		if ((flag | flag3) || this.mNextRaycast < RealTime.time)
		{
			this.mNextRaycast = RealTime.time + 0.02f;
			UICamera.Raycast(UICamera.currentTouch);
			for (int k = 0; k < 3; k++)
			{
				UICamera.mMouse[k].current = UICamera.currentTouch.current;
			}
		}
		bool flag4 = UICamera.currentTouch.last != UICamera.currentTouch.current;
		bool flag5 = UICamera.currentTouch.pressed != null;
		if (!flag5)
		{
			UICamera.hoveredObject = UICamera.currentTouch.current;
		}
		UICamera.currentTouchID = -1;
		if (flag4)
		{
			UICamera.currentKey = KeyCode.Mouse0;
		}
		if ((!flag & flag3) && (!this.stickyTooltip | flag4))
		{
			if (UICamera.mTooltipTime != 0f)
			{
				UICamera.mTooltipTime = Time.unscaledTime + this.tooltipDelay;
			}
			else if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
		}
		if (flag3 && UICamera.onMouseMove != null)
		{
			UICamera.onMouseMove(UICamera.currentTouch.delta);
			UICamera.currentTouch = null;
		}
		if (flag4 && (flag2 || (flag5 && !flag)))
		{
			UICamera.hoveredObject = null;
		}
		for (int l = 0; l < 3; l++)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(l);
			bool mouseButtonUp = Input.GetMouseButtonUp(l);
			if (mouseButtonDown | mouseButtonUp)
			{
				UICamera.currentKey = KeyCode.Mouse0 + l;
			}
			UICamera.currentTouch = UICamera.mMouse[l];
			UICamera.currentTouchID = -1 - l;
			UICamera.currentKey = KeyCode.Mouse0 + l;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
				UICamera.currentTouch.pressTime = RealTime.time;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
		}
		if (!flag & flag4)
		{
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.mTooltipTime = RealTime.time + this.tooltipDelay;
			UICamera.currentTouchID = -1;
			UICamera.currentKey = KeyCode.Mouse0;
			UICamera.hoveredObject = UICamera.currentTouch.current;
		}
		UICamera.currentTouch = null;
		UICamera.mMouse[0].last = UICamera.mMouse[0].current;
		for (int m = 1; m < 3; m++)
		{
			UICamera.mMouse[m].last = UICamera.mMouse[0].last;
		}
	}

	public void ProcessTouches()
	{
		int num = (UICamera.GetInputTouchCount == null) ? Input.touchCount : UICamera.GetInputTouchCount();
		for (int i = 0; i < num; i++)
		{
			TouchPhase phase;
			int fingerId;
			Vector2 position;
			int tapCount;
			if (UICamera.GetInputTouch == null)
			{
				UnityEngine.Touch touch = Input.GetTouch(i);
				phase = touch.phase;
				fingerId = touch.fingerId;
				position = touch.position;
				tapCount = touch.tapCount;
			}
			else
			{
				UICamera.Touch touch2 = UICamera.GetInputTouch(i);
				phase = touch2.phase;
				fingerId = touch2.fingerId;
				position = touch2.position;
				tapCount = touch2.tapCount;
			}
			UICamera.currentTouchID = (this.allowMultiTouch ? fingerId : 1);
			UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID, true);
			bool flag = phase == TouchPhase.Began || UICamera.currentTouch.touchBegan;
			bool flag2 = phase == TouchPhase.Canceled || phase == TouchPhase.Ended;
			UICamera.currentTouch.touchBegan = false;
			UICamera.currentTouch.delta = position - UICamera.currentTouch.pos;
			UICamera.currentTouch.pos = position;
			UICamera.currentKey = KeyCode.None;
			UICamera.Raycast(UICamera.currentTouch);
			if (flag)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			if (tapCount > 1)
			{
				UICamera.currentTouch.clickTime = RealTime.time;
			}
			this.ProcessTouch(flag, flag2);
			if (flag2)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
			if (!this.allowMultiTouch)
			{
				break;
			}
		}
		if (num == 0)
		{
			if (UICamera.mUsingTouchEvents)
			{
				UICamera.mUsingTouchEvents = false;
				return;
			}
			if (this.useMouse)
			{
				this.ProcessMouse();
				return;
			}
		}
		else
		{
			UICamera.mUsingTouchEvents = true;
		}
	}

	private void ProcessFakeTouches()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		bool mouseButtonUp = Input.GetMouseButtonUp(0);
		bool mouseButton = Input.GetMouseButton(0);
		if (mouseButtonDown | mouseButtonUp | mouseButton)
		{
			UICamera.currentTouchID = 1;
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.currentTouch.touchBegan = mouseButtonDown;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressTime = RealTime.time;
				UICamera.activeTouches.Add(UICamera.currentTouch);
			}
			Vector2 vector = Input.mousePosition;
			UICamera.currentTouch.delta = vector - UICamera.currentTouch.pos;
			UICamera.currentTouch.pos = vector;
			UICamera.Raycast(UICamera.currentTouch);
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			UICamera.currentKey = KeyCode.None;
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			if (mouseButtonUp)
			{
				UICamera.activeTouches.Remove(UICamera.currentTouch);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
		}
	}

	public void ProcessOthers()
	{
		UICamera.currentTouchID = -100;
		UICamera.currentTouch = UICamera.controller;
		bool flag = false;
		bool flag2 = false;
		if (this.submitKey0 != KeyCode.None && UICamera.GetKeyDown(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		else if (this.submitKey1 != KeyCode.None && UICamera.GetKeyDown(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag = true;
		}
		else if ((this.submitKey0 == KeyCode.Return || this.submitKey1 == KeyCode.Return) && UICamera.GetKeyDown(KeyCode.KeypadEnter))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		if (this.submitKey0 != KeyCode.None && UICamera.GetKeyUp(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag2 = true;
		}
		else if (this.submitKey1 != KeyCode.None && UICamera.GetKeyUp(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag2 = true;
		}
		else if ((this.submitKey0 == KeyCode.Return || this.submitKey1 == KeyCode.Return) && UICamera.GetKeyUp(KeyCode.KeypadEnter))
		{
			UICamera.currentKey = this.submitKey0;
			flag2 = true;
		}
		if (flag)
		{
			UICamera.currentTouch.pressTime = RealTime.time;
		}
		if ((flag | flag2) && UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			UICamera.currentTouch.current = UICamera.controllerNavigationObject;
			this.ProcessTouch(flag, flag2);
			UICamera.currentTouch.last = UICamera.currentTouch.current;
		}
		KeyCode keyCode = KeyCode.None;
		if (this.useController)
		{
			if (!UICamera.disableController && UICamera.currentScheme == UICamera.ControlScheme.Controller && (UICamera.currentTouch.current == null || !UICamera.currentTouch.current.activeInHierarchy))
			{
				UICamera.currentTouch.current = UICamera.controllerNavigationObject;
			}
			if (!string.IsNullOrEmpty(this.verticalAxisName))
			{
				int direction = UICamera.GetDirection(this.verticalAxisName);
				if (direction != 0)
				{
					UICamera.ShowTooltip(null);
					UICamera.currentScheme = UICamera.ControlScheme.Controller;
					UICamera.currentTouch.current = UICamera.controllerNavigationObject;
					if (UICamera.currentTouch.current != null)
					{
						keyCode = ((direction > 0) ? KeyCode.UpArrow : KeyCode.DownArrow);
						if (UICamera.onNavigate != null)
						{
							UICamera.onNavigate(UICamera.currentTouch.current, keyCode);
						}
						UICamera.Notify(UICamera.currentTouch.current, "OnNavigate", keyCode);
					}
				}
			}
			if (!string.IsNullOrEmpty(this.horizontalAxisName))
			{
				int direction2 = UICamera.GetDirection(this.horizontalAxisName);
				if (direction2 != 0)
				{
					UICamera.ShowTooltip(null);
					UICamera.currentScheme = UICamera.ControlScheme.Controller;
					UICamera.currentTouch.current = UICamera.controllerNavigationObject;
					if (UICamera.currentTouch.current != null)
					{
						keyCode = ((direction2 > 0) ? KeyCode.RightArrow : KeyCode.LeftArrow);
						if (UICamera.onNavigate != null)
						{
							UICamera.onNavigate(UICamera.currentTouch.current, keyCode);
						}
						UICamera.Notify(UICamera.currentTouch.current, "OnNavigate", keyCode);
					}
				}
			}
			float num = (!string.IsNullOrEmpty(this.horizontalPanAxisName)) ? UICamera.GetAxis(this.horizontalPanAxisName) : 0f;
			float num2 = (!string.IsNullOrEmpty(this.verticalPanAxisName)) ? UICamera.GetAxis(this.verticalPanAxisName) : 0f;
			if (num != 0f || num2 != 0f)
			{
				UICamera.ShowTooltip(null);
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentTouch.current = UICamera.controllerNavigationObject;
				if (UICamera.currentTouch.current != null)
				{
					Vector2 vector = new Vector2(num, num2);
					vector *= Time.unscaledDeltaTime;
					if (UICamera.onPan != null)
					{
						UICamera.onPan(UICamera.currentTouch.current, vector);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnPan", vector);
				}
			}
		}
		if ((UICamera.GetAnyKeyDown != null) ? UICamera.GetAnyKeyDown() : Input.anyKeyDown)
		{
			int i = 0;
			int num3 = NGUITools.keys.Length;
			while (i < num3)
			{
				KeyCode keyCode2 = NGUITools.keys[i];
				if (keyCode != keyCode2 && UICamera.GetKeyDown(keyCode2) && (this.useKeyboard || keyCode2 >= KeyCode.Mouse0) && (this.useController || keyCode2 < KeyCode.JoystickButton0) && (this.useMouse || (keyCode2 < KeyCode.Mouse0 && keyCode2 > KeyCode.Mouse6)))
				{
					UICamera.currentKey = keyCode2;
					if (UICamera.onKey != null)
					{
						UICamera.onKey(UICamera.currentTouch.current, keyCode2);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnKey", keyCode2);
				}
				i++;
			}
		}
		UICamera.currentTouch = null;
	}

	private void ProcessPress(bool pressed, float click, float drag)
	{
		if (pressed)
		{
			if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
			UICamera.currentTouch.pressStarted = true;
			if (UICamera.onPress != null && UICamera.currentTouch.pressed)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && UICamera.hoveredObject == null && UICamera.currentTouch.current != null)
			{
				UICamera.hoveredObject = UICamera.currentTouch.current;
			}
			UICamera.currentTouch.pressed = UICamera.currentTouch.current;
			UICamera.currentTouch.dragged = UICamera.currentTouch.current;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UICamera.currentTouch.totalDelta = Vector2.zero;
			UICamera.currentTouch.dragStarted = false;
			if (UICamera.onPress != null && UICamera.currentTouch.pressed)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, true);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", true);
			if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
			if (UICamera.mSelected != UICamera.currentTouch.pressed)
			{
				UICamera.mInputFocus = false;
				if (UICamera.mSelected)
				{
					UICamera.Notify(UICamera.mSelected, "OnSelect", false);
					if (UICamera.onSelect != null)
					{
						UICamera.onSelect(UICamera.mSelected, false);
					}
				}
				UICamera.mSelected = UICamera.currentTouch.pressed;
				if (UICamera.currentTouch.pressed != null)
				{
					UIKeyNavigation component = UICamera.currentTouch.pressed.GetComponent<UIKeyNavigation>();
					if (component != null)
					{
						UICamera.controller.current = UICamera.currentTouch.pressed;
					}
				}
				if (UICamera.mSelected)
				{
					UICamera.mInputFocus = (UICamera.mSelected.activeInHierarchy && UICamera.mSelected.GetComponent<UIInput>() != null);
					if (UICamera.onSelect != null)
					{
						UICamera.onSelect(UICamera.mSelected, true);
					}
					UICamera.Notify(UICamera.mSelected, "OnSelect", true);
					return;
				}
			}
		}
		else if (UICamera.currentTouch.pressed != null && (UICamera.currentTouch.delta.sqrMagnitude != 0f || UICamera.currentTouch.current != UICamera.currentTouch.last))
		{
			UICamera.currentTouch.totalDelta += UICamera.currentTouch.delta;
			float sqrMagnitude = UICamera.currentTouch.totalDelta.sqrMagnitude;
			bool flag = false;
			if (!UICamera.currentTouch.dragStarted && UICamera.currentTouch.last != UICamera.currentTouch.current)
			{
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
				UICamera.isDragging = true;
				if (UICamera.onDragStart != null)
				{
					UICamera.onDragStart(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
				if (UICamera.onDragOver != null)
				{
					UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOver", UICamera.currentTouch.dragged);
				UICamera.isDragging = false;
			}
			else if (!UICamera.currentTouch.dragStarted && drag < sqrMagnitude)
			{
				flag = true;
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
			}
			if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.mTooltip != null)
				{
					UICamera.ShowTooltip(null);
				}
				UICamera.isDragging = true;
				bool flag2 = UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None;
				if (flag)
				{
					if (UICamera.onDragStart != null)
					{
						UICamera.onDragStart(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				else if (UICamera.currentTouch.last != UICamera.currentTouch.current)
				{
					if (UICamera.onDragOut != null)
					{
						UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				if (UICamera.onDrag != null)
				{
					UICamera.onDrag(UICamera.currentTouch.dragged, UICamera.currentTouch.delta);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDrag", UICamera.currentTouch.delta);
				UICamera.currentTouch.last = UICamera.currentTouch.current;
				UICamera.isDragging = false;
				if (flag2)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
					return;
				}
				if (UICamera.currentTouch.clickNotification == UICamera.ClickNotification.BasedOnDelta && click < sqrMagnitude)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
			}
		}
	}

	private void ProcessRelease(bool isMouse, float drag)
	{
		if (UICamera.currentTouch == null)
		{
			return;
		}
		UICamera.currentTouch.pressStarted = false;
		if (UICamera.currentTouch.pressed != null)
		{
			if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.onDragOut != null)
				{
					UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
				if (UICamera.onDragEnd != null)
				{
					UICamera.onDragEnd(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragEnd", null);
			}
			if (UICamera.onPress != null)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			if (isMouse && this.HasCollider(UICamera.currentTouch.pressed))
			{
				if (UICamera.mHover == UICamera.currentTouch.current)
				{
					if (UICamera.onHover != null)
					{
						UICamera.onHover(UICamera.currentTouch.current, true);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnHover", true);
				}
				else
				{
					UICamera.hoveredObject = UICamera.currentTouch.current;
				}
			}
			if (UICamera.currentTouch.dragged == UICamera.currentTouch.current || (UICamera.currentScheme != UICamera.ControlScheme.Controller && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.totalDelta.sqrMagnitude < drag))
			{
				if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.pressed == UICamera.currentTouch.current)
				{
					UICamera.ShowTooltip(null);
					float time = RealTime.time;
					if (UICamera.onClick != null)
					{
						UICamera.onClick(UICamera.currentTouch.pressed);
					}
					UICamera.Notify(UICamera.currentTouch.pressed, "OnClick", null);
					if (UICamera.currentTouch.clickTime + 0.35f > time)
					{
						if (UICamera.onDoubleClick != null)
						{
							UICamera.onDoubleClick(UICamera.currentTouch.pressed);
						}
						UICamera.Notify(UICamera.currentTouch.pressed, "OnDoubleClick", null);
					}
					UICamera.currentTouch.clickTime = time;
				}
			}
			else if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.onDrop != null)
				{
					UICamera.onDrop(UICamera.currentTouch.current, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.current, "OnDrop", UICamera.currentTouch.dragged);
			}
		}
		UICamera.currentTouch.dragStarted = false;
		UICamera.currentTouch.pressed = null;
		UICamera.currentTouch.dragged = null;
	}

	private bool HasCollider(GameObject go)
	{
		if (go == null)
		{
			return false;
		}
		Collider component = go.GetComponent<Collider>();
		if (component != null)
		{
			return component.enabled;
		}
		Collider2D component2 = go.GetComponent<Collider2D>();
		return component2 != null && component2.enabled;
	}

	public void ProcessTouch(bool pressed, bool released)
	{
		if (pressed)
		{
			UICamera.mTooltipTime = Time.unscaledTime + this.tooltipDelay;
		}
		bool flag = UICamera.currentScheme == UICamera.ControlScheme.Mouse;
		float num = flag ? this.mouseDragThreshold : this.touchDragThreshold;
		float num2 = flag ? this.mouseClickThreshold : this.touchClickThreshold;
		num *= num;
		num2 *= num2;
		if (UICamera.currentTouch.pressed != null)
		{
			if (released)
			{
				this.ProcessRelease(flag, num);
			}
			this.ProcessPress(pressed, num2, num);
			if (UICamera.currentTouch.pressed == UICamera.currentTouch.current && UICamera.mTooltipTime != 0f && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && !UICamera.currentTouch.dragStarted && UICamera.currentTouch.deltaTime > this.tooltipDelay)
			{
				UICamera.mTooltipTime = 0f;
				UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				if (this.longPressTooltip)
				{
					UICamera.ShowTooltip(UICamera.currentTouch.pressed);
				}
				UICamera.Notify(UICamera.currentTouch.current, "OnLongPress", null);
				return;
			}
		}
		else if (flag | pressed | released)
		{
			this.ProcessPress(pressed, num2, num);
			if (released)
			{
				this.ProcessRelease(flag, num);
			}
		}
	}

	public static bool ShowTooltip(GameObject go)
	{
		if (UICamera.mTooltip != go)
		{
			if (UICamera.mTooltip != null)
			{
				if (UICamera.onTooltip != null)
				{
					UICamera.onTooltip(UICamera.mTooltip, false);
				}
				UICamera.Notify(UICamera.mTooltip, "OnTooltip", false);
			}
			UICamera.mTooltip = go;
			UICamera.mTooltipTime = 0f;
			if (UICamera.mTooltip != null)
			{
				if (UICamera.onTooltip != null)
				{
					UICamera.onTooltip(UICamera.mTooltip, true);
				}
				UICamera.Notify(UICamera.mTooltip, "OnTooltip", true);
			}
			return true;
		}
		return false;
	}

	public static bool HideTooltip()
	{
		return UICamera.ShowTooltip(null);
	}

	public UICamera()
	{
		this.eventType = UICamera.EventType.UI_3D;
		this.eventReceiverMask = -1;
		this.useMouse = true;
		this.useTouch = true;
		this.allowMultiTouch = true;
		this.useKeyboard = true;
		this.useController = true;
		this.stickyTooltip = true;
		this.tooltipDelay = 1f;
		this.mouseDragThreshold = 4f;
		this.mouseClickThreshold = 10f;
		this.touchDragThreshold = 40f;
		this.touchClickThreshold = 40f;
		this.rangeDistance = -1f;
		this.horizontalAxisName = "Horizontal";
		this.verticalAxisName = "Vertical";
		this.scrollAxisName = "Mouse ScrollWheel";
		this.commandClick = true;
		this.submitKey0 = KeyCode.Return;
		this.submitKey1 = KeyCode.JoystickButton0;
		this.cancelKey0 = KeyCode.Escape;
		this.cancelKey1 = KeyCode.JoystickButton1;
		this.autoHideCursor = true;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32((int)this.eventType);
		SerializedStateWriter.Instance.WriteBoolean(this.eventsGoToColliders);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.eventReceiverMask.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.debug);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.useMouse);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.useTouch);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.allowMultiTouch);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.useKeyboard);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.useController);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.stickyTooltip);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.tooltipDelay);
		SerializedStateWriter.Instance.WriteBoolean(this.longPressTooltip);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.mouseDragThreshold);
		SerializedStateWriter.Instance.WriteSingle(this.mouseClickThreshold);
		SerializedStateWriter.Instance.WriteSingle(this.touchDragThreshold);
		SerializedStateWriter.Instance.WriteSingle(this.touchClickThreshold);
		SerializedStateWriter.Instance.WriteSingle(this.rangeDistance);
		SerializedStateWriter.Instance.WriteString(this.horizontalAxisName);
		SerializedStateWriter.Instance.WriteString(this.verticalAxisName);
		SerializedStateWriter.Instance.WriteString(this.horizontalPanAxisName);
		SerializedStateWriter.Instance.WriteString(this.verticalPanAxisName);
		SerializedStateWriter.Instance.WriteString(this.scrollAxisName);
		SerializedStateWriter.Instance.WriteBoolean(this.commandClick);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.submitKey0);
		SerializedStateWriter.Instance.WriteInt32((int)this.submitKey1);
		SerializedStateWriter.Instance.WriteInt32((int)this.cancelKey0);
		SerializedStateWriter.Instance.WriteInt32((int)this.cancelKey1);
		SerializedStateWriter.Instance.WriteBoolean(this.autoHideCursor);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		this.eventType = (UICamera.EventType)SerializedStateReader.Instance.ReadInt32();
		this.eventsGoToColliders = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.eventReceiverMask.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.debug = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.useMouse = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.useTouch = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.allowMultiTouch = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.useKeyboard = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.useController = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.stickyTooltip = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.tooltipDelay = SerializedStateReader.Instance.ReadSingle();
		this.longPressTooltip = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.mouseDragThreshold = SerializedStateReader.Instance.ReadSingle();
		this.mouseClickThreshold = SerializedStateReader.Instance.ReadSingle();
		this.touchDragThreshold = SerializedStateReader.Instance.ReadSingle();
		this.touchClickThreshold = SerializedStateReader.Instance.ReadSingle();
		this.rangeDistance = SerializedStateReader.Instance.ReadSingle();
		this.horizontalAxisName = (SerializedStateReader.Instance.ReadString() as string);
		this.verticalAxisName = (SerializedStateReader.Instance.ReadString() as string);
		this.horizontalPanAxisName = (SerializedStateReader.Instance.ReadString() as string);
		this.verticalPanAxisName = (SerializedStateReader.Instance.ReadString() as string);
		this.scrollAxisName = (SerializedStateReader.Instance.ReadString() as string);
		this.commandClick = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.submitKey0 = (KeyCode)SerializedStateReader.Instance.ReadInt32();
		this.submitKey1 = (KeyCode)SerializedStateReader.Instance.ReadInt32();
		this.cancelKey0 = (KeyCode)SerializedStateReader.Instance.ReadInt32();
		this.cancelKey1 = (KeyCode)SerializedStateReader.Instance.ReadInt32();
		this.autoHideCursor = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = (int)this.eventType;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 3080);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.eventsGoToColliders, &var_0_cp_0[var_0_cp_1] + 3090);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3110);
			this.eventReceiverMask.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.debug, &var_0_cp_0[var_0_cp_1] + 3128);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.useMouse, &var_0_cp_0[var_0_cp_1] + 3134);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.useTouch, &var_0_cp_0[var_0_cp_1] + 3143);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.allowMultiTouch, &var_0_cp_0[var_0_cp_1] + 3152);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.useKeyboard, &var_0_cp_0[var_0_cp_1] + 3168);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.useController, &var_0_cp_0[var_0_cp_1] + 3180);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.stickyTooltip, &var_0_cp_0[var_0_cp_1] + 3194);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.tooltipDelay, &var_0_cp_0[var_0_cp_1] + 3208);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.longPressTooltip, &var_0_cp_0[var_0_cp_1] + 3221);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.mouseDragThreshold, &var_0_cp_0[var_0_cp_1] + 3238);
		SerializedNamedStateWriter.Instance.WriteSingle(this.mouseClickThreshold, &var_0_cp_0[var_0_cp_1] + 3257);
		SerializedNamedStateWriter.Instance.WriteSingle(this.touchDragThreshold, &var_0_cp_0[var_0_cp_1] + 3277);
		SerializedNamedStateWriter.Instance.WriteSingle(this.touchClickThreshold, &var_0_cp_0[var_0_cp_1] + 3296);
		SerializedNamedStateWriter.Instance.WriteSingle(this.rangeDistance, &var_0_cp_0[var_0_cp_1] + 3316);
		SerializedNamedStateWriter.Instance.WriteString(this.horizontalAxisName, &var_0_cp_0[var_0_cp_1] + 3330);
		SerializedNamedStateWriter.Instance.WriteString(this.verticalAxisName, &var_0_cp_0[var_0_cp_1] + 3349);
		SerializedNamedStateWriter.Instance.WriteString(this.horizontalPanAxisName, &var_0_cp_0[var_0_cp_1] + 3366);
		SerializedNamedStateWriter.Instance.WriteString(this.verticalPanAxisName, &var_0_cp_0[var_0_cp_1] + 3388);
		SerializedNamedStateWriter.Instance.WriteString(this.scrollAxisName, &var_0_cp_0[var_0_cp_1] + 3408);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.commandClick, &var_0_cp_0[var_0_cp_1] + 3423);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.submitKey0, &var_0_cp_0[var_0_cp_1] + 3436);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.submitKey1, &var_0_cp_0[var_0_cp_1] + 3447);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.cancelKey0, &var_0_cp_0[var_0_cp_1] + 3458);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.cancelKey1, &var_0_cp_0[var_0_cp_1] + 3469);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.autoHideCursor, &var_0_cp_0[var_0_cp_1] + 3480);
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.eventType = (UICamera.EventType)arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3080);
		this.eventsGoToColliders = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3090);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3110);
			this.eventReceiverMask.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.debug = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3128);
		SerializedNamedStateReader.Instance.Align();
		this.useMouse = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3134);
		SerializedNamedStateReader.Instance.Align();
		this.useTouch = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3143);
		SerializedNamedStateReader.Instance.Align();
		this.allowMultiTouch = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3152);
		SerializedNamedStateReader.Instance.Align();
		this.useKeyboard = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3168);
		SerializedNamedStateReader.Instance.Align();
		this.useController = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3180);
		SerializedNamedStateReader.Instance.Align();
		this.stickyTooltip = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3194);
		SerializedNamedStateReader.Instance.Align();
		this.tooltipDelay = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 3208);
		this.longPressTooltip = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3221);
		SerializedNamedStateReader.Instance.Align();
		this.mouseDragThreshold = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 3238);
		this.mouseClickThreshold = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 3257);
		this.touchDragThreshold = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 3277);
		this.touchClickThreshold = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 3296);
		this.rangeDistance = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 3316);
		this.horizontalAxisName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 3330) as string);
		this.verticalAxisName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 3349) as string);
		this.horizontalPanAxisName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 3366) as string);
		this.verticalPanAxisName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 3388) as string);
		this.scrollAxisName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 3408) as string);
		this.commandClick = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3423);
		SerializedNamedStateReader.Instance.Align();
		this.submitKey0 = (KeyCode)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3436);
		this.submitKey1 = (KeyCode)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3447);
		this.cancelKey0 = (KeyCode)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3458);
		this.cancelKey1 = (KeyCode)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3469);
		this.autoHideCursor = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3480);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UICamera(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UICamera)instance).eventsGoToColliders;
	}

	public static void $Set0(object instance, bool value)
	{
		((UICamera)instance).eventsGoToColliders = value;
	}

	public static bool $Get1(object instance)
	{
		return ((UICamera)instance).debug;
	}

	public static void $Set1(object instance, bool value)
	{
		((UICamera)instance).debug = value;
	}

	public static bool $Get2(object instance)
	{
		return ((UICamera)instance).useMouse;
	}

	public static void $Set2(object instance, bool value)
	{
		((UICamera)instance).useMouse = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UICamera)instance).useTouch;
	}

	public static void $Set3(object instance, bool value)
	{
		((UICamera)instance).useTouch = value;
	}

	public static bool $Get4(object instance)
	{
		return ((UICamera)instance).allowMultiTouch;
	}

	public static void $Set4(object instance, bool value)
	{
		((UICamera)instance).allowMultiTouch = value;
	}

	public static bool $Get5(object instance)
	{
		return ((UICamera)instance).useKeyboard;
	}

	public static void $Set5(object instance, bool value)
	{
		((UICamera)instance).useKeyboard = value;
	}

	public static bool $Get6(object instance)
	{
		return ((UICamera)instance).useController;
	}

	public static void $Set6(object instance, bool value)
	{
		((UICamera)instance).useController = value;
	}

	public static bool $Get7(object instance)
	{
		return ((UICamera)instance).stickyTooltip;
	}

	public static void $Set7(object instance, bool value)
	{
		((UICamera)instance).stickyTooltip = value;
	}

	public static float $Get8(object instance)
	{
		return ((UICamera)instance).tooltipDelay;
	}

	public static void $Set8(object instance, float value)
	{
		((UICamera)instance).tooltipDelay = value;
	}

	public static bool $Get9(object instance)
	{
		return ((UICamera)instance).longPressTooltip;
	}

	public static void $Set9(object instance, bool value)
	{
		((UICamera)instance).longPressTooltip = value;
	}

	public static float $Get10(object instance)
	{
		return ((UICamera)instance).mouseDragThreshold;
	}

	public static void $Set10(object instance, float value)
	{
		((UICamera)instance).mouseDragThreshold = value;
	}

	public static float $Get11(object instance)
	{
		return ((UICamera)instance).mouseClickThreshold;
	}

	public static void $Set11(object instance, float value)
	{
		((UICamera)instance).mouseClickThreshold = value;
	}

	public static float $Get12(object instance)
	{
		return ((UICamera)instance).touchDragThreshold;
	}

	public static void $Set12(object instance, float value)
	{
		((UICamera)instance).touchDragThreshold = value;
	}

	public static float $Get13(object instance)
	{
		return ((UICamera)instance).touchClickThreshold;
	}

	public static void $Set13(object instance, float value)
	{
		((UICamera)instance).touchClickThreshold = value;
	}

	public static float $Get14(object instance)
	{
		return ((UICamera)instance).rangeDistance;
	}

	public static void $Set14(object instance, float value)
	{
		((UICamera)instance).rangeDistance = value;
	}

	public static bool $Get15(object instance)
	{
		return ((UICamera)instance).commandClick;
	}

	public static void $Set15(object instance, bool value)
	{
		((UICamera)instance).commandClick = value;
	}

	public static bool $Get16(object instance)
	{
		return ((UICamera)instance).autoHideCursor;
	}

	public static void $Set16(object instance, bool value)
	{
		((UICamera)instance).autoHideCursor = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.CompareFunc((UICamera)GCHandledObjects.GCHandleToObject(*args), (UICamera)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.CountInputSources());
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.FindCameraForLayer(*(int*)args));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.FindRootRigidbody((Transform)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.FindRootRigidbody2D((Transform)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UICamera)GCHandledObjects.GCHandleToObject(instance)).cachedCamera);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.controllerNavigationObject);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.currentKey);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.currentRay);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.currentScheme);
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.disableController);
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.dragCount);
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.eventHandler);
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.first);
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.genericEventHandler);
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UICamera)GCHandledObjects.GCHandleToObject(instance)).handlesEvents);
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.hoveredObject);
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.inputHasFocus);
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.isOverUI);
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.lastEventPosition);
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.lastTouchPosition);
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.mainCamera);
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.selectedObject);
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UICamera)GCHandledObjects.GCHandleToObject(instance)).stickyPress);
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.tooltipObject);
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.touchCount);
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.GetDirection(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.GetDirection((KeyCode)(*(int*)args), (KeyCode)(*(int*)(args + 1))));
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.GetDirection((KeyCode)(*(int*)args), (KeyCode)(*(int*)(args + 1)), (KeyCode)(*(int*)(args + 2)), (KeyCode)(*(int*)(args + 3))));
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.GetMouse(*(int*)args));
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.GetTouch(*(int*)args, *(sbyte*)(args + 1) != 0));
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UICamera)GCHandledObjects.GCHandleToObject(instance)).HasCollider((GameObject)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.HideTooltip());
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.IsHighlighted((GameObject)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.IsPressed((GameObject)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.IsVisible(*(*(IntPtr*)args), (GameObject)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke38(long instance, long* args)
	{
		UICamera.Notify((GameObject)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke39(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke40(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke41(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).ProcessFakeTouches();
		return -1L;
	}

	public unsafe static long $Invoke42(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).ProcessMouse();
		return -1L;
	}

	public unsafe static long $Invoke43(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).ProcessOthers();
		return -1L;
	}

	public unsafe static long $Invoke44(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).ProcessPress(*(sbyte*)args != 0, *(float*)(args + 1), *(float*)(args + 2));
		return -1L;
	}

	public unsafe static long $Invoke45(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).ProcessRelease(*(sbyte*)args != 0, *(float*)(args + 1));
		return -1L;
	}

	public unsafe static long $Invoke46(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).ProcessTouch(*(sbyte*)args != 0, *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke47(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).ProcessTouches();
		return -1L;
	}

	public unsafe static long $Invoke48(long instance, long* args)
	{
		UICamera.Raycast((UICamera.MouseOrTouch)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke49(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.Raycast(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke50(long instance, long* args)
	{
		UICamera.RemoveTouch(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke51(long instance, long* args)
	{
		UICamera.controllerNavigationObject = (GameObject)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke52(long instance, long* args)
	{
		UICamera.currentKey = (KeyCode)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke53(long instance, long* args)
	{
		UICamera.currentScheme = (UICamera.ControlScheme)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke54(long instance, long* args)
	{
		UICamera.disableController = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke55(long instance, long* args)
	{
		UICamera.genericEventHandler = (GameObject)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke56(long instance, long* args)
	{
		UICamera.hoveredObject = (GameObject)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke57(long instance, long* args)
	{
		UICamera.lastEventPosition = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke58(long instance, long* args)
	{
		UICamera.lastTouchPosition = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke59(long instance, long* args)
	{
		UICamera.selectedObject = (GameObject)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke60(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UICamera.ShowTooltip((GameObject)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke61(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke62(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke63(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke64(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke65(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke66(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke67(long instance, long* args)
	{
		((UICamera)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
