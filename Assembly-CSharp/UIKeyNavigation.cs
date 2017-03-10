using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Key Navigation")]
public class UIKeyNavigation : MonoBehaviour, IUnitySerializable
{
	public enum Constraint
	{
		None,
		Vertical,
		Horizontal,
		Explicit
	}

	public static BetterList<UIKeyNavigation> list = new BetterList<UIKeyNavigation>();

	public UIKeyNavigation.Constraint constraint;

	public GameObject onUp;

	public GameObject onDown;

	public GameObject onLeft;

	public GameObject onRight;

	public GameObject onClick;

	public GameObject onTab;

	public bool startsSelected;

	[System.NonSerialized]
	private bool mStarted;

	public static int mLastFrame = 0;

	public static UIKeyNavigation current
	{
		get
		{
			GameObject hoveredObject = UICamera.hoveredObject;
			if (hoveredObject == null)
			{
				return null;
			}
			return hoveredObject.GetComponent<UIKeyNavigation>();
		}
	}

	public bool isColliderEnabled
	{
		get
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return false;
			}
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	protected virtual void OnEnable()
	{
		UIKeyNavigation.list.Add(this);
		if (this.mStarted)
		{
			this.Start();
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (this.startsSelected && this.isColliderEnabled)
		{
			UICamera.hoveredObject = base.gameObject;
		}
	}

	protected virtual void OnDisable()
	{
		UIKeyNavigation.list.Remove(this);
	}

	private static bool IsActive(GameObject go)
	{
		if (!go || !go.activeInHierarchy)
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

	public GameObject GetLeft()
	{
		if (UIKeyNavigation.IsActive(this.onLeft))
		{
			return this.onLeft;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Vertical || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.left, 1f, 2f);
	}

	public GameObject GetRight()
	{
		if (UIKeyNavigation.IsActive(this.onRight))
		{
			return this.onRight;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Vertical || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.right, 1f, 2f);
	}

	public GameObject GetUp()
	{
		if (UIKeyNavigation.IsActive(this.onUp))
		{
			return this.onUp;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Horizontal || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.up, 2f, 1f);
	}

	public GameObject GetDown()
	{
		if (UIKeyNavigation.IsActive(this.onDown))
		{
			return this.onDown;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Horizontal || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.down, 2f, 1f);
	}

	public GameObject Get(Vector3 myDir, float x = 1f, float y = 1f)
	{
		Transform transform = base.transform;
		myDir = transform.TransformDirection(myDir);
		Vector3 center = UIKeyNavigation.GetCenter(base.gameObject);
		float num = 3.40282347E+38f;
		GameObject result = null;
		for (int i = 0; i < UIKeyNavigation.list.size; i++)
		{
			UIKeyNavigation uIKeyNavigation = UIKeyNavigation.list[i];
			if (!(uIKeyNavigation == this) && uIKeyNavigation.constraint != UIKeyNavigation.Constraint.Explicit && uIKeyNavigation.isColliderEnabled)
			{
				UIWidget component = uIKeyNavigation.GetComponent<UIWidget>();
				if (!(component != null) || component.alpha != 0f)
				{
					Vector3 direction = UIKeyNavigation.GetCenter(uIKeyNavigation.gameObject) - center;
					float num2 = Vector3.Dot(myDir, direction.normalized);
					if (num2 >= 0.707f)
					{
						direction = transform.InverseTransformDirection(direction);
						direction.x *= x;
						direction.y *= y;
						float sqrMagnitude = direction.sqrMagnitude;
						if (sqrMagnitude <= num)
						{
							result = uIKeyNavigation.gameObject;
							num = sqrMagnitude;
						}
					}
				}
			}
		}
		return result;
	}

	protected static Vector3 GetCenter(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		UICamera uICamera = UICamera.FindCameraForLayer(go.layer);
		if (uICamera != null)
		{
			Vector3 vector = go.transform.position;
			if (component != null)
			{
				Vector3[] worldCorners = component.worldCorners;
				vector = (worldCorners[0] + worldCorners[2]) * 0.5f;
			}
			vector = uICamera.cachedCamera.WorldToScreenPoint(vector);
			vector.z = 0f;
			return vector;
		}
		if (component != null)
		{
			Vector3[] worldCorners2 = component.worldCorners;
			return (worldCorners2[0] + worldCorners2[2]) * 0.5f;
		}
		return go.transform.position;
	}

	public virtual void OnNavigate(KeyCode key)
	{
		if (UIPopupList.isOpen)
		{
			return;
		}
		if (UIKeyNavigation.mLastFrame == Time.frameCount)
		{
			return;
		}
		UIKeyNavigation.mLastFrame = Time.frameCount;
		GameObject gameObject = null;
		switch (key)
		{
		case KeyCode.UpArrow:
			gameObject = this.GetUp();
			break;
		case KeyCode.DownArrow:
			gameObject = this.GetDown();
			break;
		case KeyCode.RightArrow:
			gameObject = this.GetRight();
			break;
		case KeyCode.LeftArrow:
			gameObject = this.GetLeft();
			break;
		}
		if (gameObject != null)
		{
			UICamera.hoveredObject = gameObject;
		}
	}

	public virtual void OnKey(KeyCode key)
	{
		if (UIPopupList.isOpen)
		{
			return;
		}
		if (UIKeyNavigation.mLastFrame == Time.frameCount)
		{
			return;
		}
		UIKeyNavigation.mLastFrame = Time.frameCount;
		if (key == KeyCode.Tab)
		{
			GameObject gameObject = this.onTab;
			if (gameObject == null)
			{
				if (UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift))
				{
					gameObject = this.GetLeft();
					if (gameObject == null)
					{
						gameObject = this.GetUp();
					}
					if (gameObject == null)
					{
						gameObject = this.GetDown();
					}
					if (gameObject == null)
					{
						gameObject = this.GetRight();
					}
				}
				else
				{
					gameObject = this.GetRight();
					if (gameObject == null)
					{
						gameObject = this.GetDown();
					}
					if (gameObject == null)
					{
						gameObject = this.GetUp();
					}
					if (gameObject == null)
					{
						gameObject = this.GetLeft();
					}
				}
			}
			if (gameObject != null)
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.hoveredObject = gameObject;
				UIInput component = gameObject.GetComponent<UIInput>();
				if (component != null)
				{
					component.isSelected = true;
				}
			}
		}
	}

	protected virtual void OnClick()
	{
		if (NGUITools.GetActive(this.onClick))
		{
			UICamera.hoveredObject = this.onClick;
		}
	}

	public UIKeyNavigation()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32((int)this.constraint);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onUp);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onDown);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onLeft);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onRight);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onClick);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.onTab);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.startsSelected);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		this.constraint = (UIKeyNavigation.Constraint)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.onUp = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.onDown = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.onLeft = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.onRight = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.onClick = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.onTab = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.startsSelected = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.onUp != null)
		{
			this.onUp = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onUp) as GameObject);
		}
		if (this.onDown != null)
		{
			this.onDown = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onDown) as GameObject);
		}
		if (this.onLeft != null)
		{
			this.onLeft = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onLeft) as GameObject);
		}
		if (this.onRight != null)
		{
			this.onRight = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onRight) as GameObject);
		}
		if (this.onClick != null)
		{
			this.onClick = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onClick) as GameObject);
		}
		if (this.onTab != null)
		{
			this.onTab = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.onTab) as GameObject);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = (int)this.constraint;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 278);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onUp, &var_0_cp_0[var_0_cp_1] + 289);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onDown, &var_0_cp_0[var_0_cp_1] + 294);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onLeft, &var_0_cp_0[var_0_cp_1] + 301);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onRight, &var_0_cp_0[var_0_cp_1] + 308);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onClick, &var_0_cp_0[var_0_cp_1] + 257);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.onTab, &var_0_cp_0[var_0_cp_1] + 316);
		}
		SerializedNamedStateWriter.Instance.WriteBoolean(this.startsSelected, &var_0_cp_0[var_0_cp_1] + 322);
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.constraint = (UIKeyNavigation.Constraint)arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 278);
		if (depth <= 7)
		{
			this.onUp = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 289) as GameObject);
		}
		if (depth <= 7)
		{
			this.onDown = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 294) as GameObject);
		}
		if (depth <= 7)
		{
			this.onLeft = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 301) as GameObject);
		}
		if (depth <= 7)
		{
			this.onRight = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 308) as GameObject);
		}
		if (depth <= 7)
		{
			this.onClick = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 257) as GameObject);
		}
		if (depth <= 7)
		{
			this.onTab = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 316) as GameObject);
		}
		this.startsSelected = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 322);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIKeyNavigation(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)instance).onUp);
	}

	public static void $Set0(object instance, long value)
	{
		((UIKeyNavigation)instance).onUp = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)instance).onDown);
	}

	public static void $Set1(object instance, long value)
	{
		((UIKeyNavigation)instance).onDown = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)instance).onLeft);
	}

	public static void $Set2(object instance, long value)
	{
		((UIKeyNavigation)instance).onLeft = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get3(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)instance).onRight);
	}

	public static void $Set3(object instance, long value)
	{
		((UIKeyNavigation)instance).onRight = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get4(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)instance).onClick);
	}

	public static void $Set4(object instance, long value)
	{
		((UIKeyNavigation)instance).onClick = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get5(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)instance).onTab);
	}

	public static void $Set5(object instance, long value)
	{
		((UIKeyNavigation)instance).onTab = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get6(object instance)
	{
		return ((UIKeyNavigation)instance).startsSelected;
	}

	public static void $Set6(object instance, bool value)
	{
		((UIKeyNavigation)instance).startsSelected = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).Get(*(*(IntPtr*)args), *(float*)(args + 1), *(float*)(args + 2)));
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIKeyNavigation.current);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).isColliderEnabled);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIKeyNavigation.GetCenter((GameObject)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).GetDown());
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).GetLeft());
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).GetRight());
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).GetUp());
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIKeyNavigation.IsActive((GameObject)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).OnKey((KeyCode)(*(int*)args));
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).OnNavigate((KeyCode)(*(int*)args));
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIKeyNavigation)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
