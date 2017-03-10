using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[System.Serializable]
public class PropertyReference : IUnitySerializable
{
	[SerializeField]
	protected internal Component mTarget;

	[SerializeField]
	protected internal string mName;

	private FieldInfo mField;

	private PropertyInfo mProperty;

	private static int s_Hash = "PropertyBinding".GetHashCode();

	public Component target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	public string name
	{
		get
		{
			return this.mName;
		}
		set
		{
			this.mName = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	public bool isValid
	{
		get
		{
			return this.mTarget != null && !string.IsNullOrEmpty(this.mName);
		}
	}

	public bool isEnabled
	{
		get
		{
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget as MonoBehaviour;
			return monoBehaviour == null || monoBehaviour.enabled;
		}
	}

	public PropertyReference()
	{
	}

	public PropertyReference(Component target, string fieldName)
	{
		this.mTarget = target;
		this.mName = fieldName;
	}

	public Type GetPropertyType()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			return this.mProperty.PropertyType;
		}
		if (this.mField != null)
		{
			return this.mField.FieldType;
		}
		return typeof(void);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is PropertyReference)
		{
			PropertyReference propertyReference = obj as PropertyReference;
			return this.mTarget == propertyReference.mTarget && string.Equals(this.mName, propertyReference.mName);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return PropertyReference.s_Hash;
	}

	public void Set(Component target, string methodName)
	{
		this.mTarget = target;
		this.mName = methodName;
	}

	public void Clear()
	{
		this.mTarget = null;
		this.mName = null;
	}

	public void Reset()
	{
		this.mField = null;
		this.mProperty = null;
	}

	public override string ToString()
	{
		return PropertyReference.ToString(this.mTarget, this.name);
	}

	public static string ToString(Component comp, string property)
	{
		if (!(comp != null))
		{
			return null;
		}
		string text = comp.GetType().ToString();
		int num = text.LastIndexOf('.');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		if (!string.IsNullOrEmpty(property))
		{
			return text + "." + property;
		}
		return text + ".[property]";
	}

	[DebuggerHidden, DebuggerStepThrough]
	public object Get()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			if (this.mProperty.CanRead)
			{
				return this.mProperty.GetValue(this.mTarget, null);
			}
		}
		else if (this.mField != null)
		{
			return this.mField.GetValue(this.mTarget);
		}
		return null;
	}

	[DebuggerHidden, DebuggerStepThrough]
	public bool Set(object value)
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty == null && this.mField == null)
		{
			return false;
		}
		if (value == null)
		{
			try
			{
				if (this.mProperty == null)
				{
					this.mField.SetValue(this.mTarget, null);
					bool result = true;
					return result;
				}
				if (this.mProperty.CanWrite)
				{
					this.mProperty.SetValue(this.mTarget, null, null);
					bool result = true;
					return result;
				}
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
		}
		if (!this.Convert(ref value))
		{
			if (Application.isPlaying)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Unable to convert ",
					value.GetType(),
					" to ",
					this.GetPropertyType()
				}));
			}
		}
		else
		{
			if (this.mField != null)
			{
				this.mField.SetValue(this.mTarget, value);
				return true;
			}
			if (this.mProperty.CanWrite)
			{
				this.mProperty.SetValue(this.mTarget, value, null);
				return true;
			}
		}
		return false;
	}

	[DebuggerHidden, DebuggerStepThrough]
	private bool Cache()
	{
		if (this.mTarget != null && !string.IsNullOrEmpty(this.mName))
		{
			Type type = this.mTarget.GetType();
			this.mField = type.GetRuntimeField(this.mName);
			this.mProperty = type.GetRuntimeProperty(this.mName);
		}
		else
		{
			this.mField = null;
			this.mProperty = null;
		}
		return this.mField != null || this.mProperty != null;
	}

	private bool Convert(ref object value)
	{
		if (this.mTarget == null)
		{
			return false;
		}
		Type propertyType = this.GetPropertyType();
		Type from;
		if (value == null)
		{
			if (!propertyType.GetTypeInfo().IsClass)
			{
				return false;
			}
			from = propertyType;
		}
		else
		{
			from = value.GetType();
		}
		return PropertyReference.Convert(ref value, from, propertyType);
	}

	public static bool Convert(Type from, Type to)
	{
		object obj = null;
		return PropertyReference.Convert(ref obj, from, to);
	}

	public static bool Convert(object value, Type to)
	{
		if (value == null)
		{
			value = null;
			return PropertyReference.Convert(ref value, to, to);
		}
		return PropertyReference.Convert(ref value, value.GetType(), to);
	}

	public static bool Convert(ref object value, Type from, Type to)
	{
		if (to.GetTypeInfo().IsAssignableFrom(from.GetTypeInfo()))
		{
			return true;
		}
		if (to == typeof(string))
		{
			value = ((value != null) ? value.ToString() : "null");
			return true;
		}
		if (value == null)
		{
			return false;
		}
		float num2;
		if (to == typeof(int))
		{
			if (from == typeof(string))
			{
				int num;
				if (int.TryParse((string)value, ref num))
				{
					value = num;
					return true;
				}
			}
			else if (from == typeof(float))
			{
				value = Mathf.RoundToInt((float)value);
				return true;
			}
		}
		else if (to == typeof(float) && from == typeof(string) && float.TryParse((string)value, ref num2))
		{
			value = num2;
			return true;
		}
		return false;
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mTarget);
		}
		SerializedStateWriter.Instance.WriteString(this.mName);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.mTarget = (SerializedStateReader.Instance.ReadUnityEngineObject() as Component);
		}
		this.mName = (SerializedStateReader.Instance.ReadString() as string);
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.mTarget != null)
		{
			this.mTarget = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mTarget) as Component);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.mTarget;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 2225);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.mName, &var_0_cp_0[var_0_cp_1] + 2281);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateReader arg_1E_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			this.mTarget = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2225) as Component);
		}
		this.mName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 2281) as string);
	}

	protected internal PropertyReference(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Cache());
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(PropertyReference.Convert(GCHandledObjects.GCHandleToObject(*args), (Type)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(PropertyReference.Convert((Type)GCHandledObjects.GCHandleToObject(*args), (Type)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Equals(GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Get());
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).isEnabled);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).isValid);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).name);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).target);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).GetHashCode());
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).GetPropertyType());
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Reset();
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Set(GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Set((Component)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).name = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).target = (Component)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(PropertyReference.ToString((Component)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).ToString());
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((PropertyReference)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
