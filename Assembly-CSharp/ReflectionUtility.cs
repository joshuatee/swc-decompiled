using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRTBridge;

public static class ReflectionUtility
{
	[CompilerGenerated]
	[System.Serializable]
	private sealed class <>c
	{
		public static readonly ReflectionUtility.<>c <>9 = new ReflectionUtility.<>c();

		public static Func<FieldInfo, bool> <>9__3_0;

		public static Func<FieldInfo, bool> <>9__3_1;

		internal bool <GetFields>b__3_0(FieldInfo x)
		{
			return !x.IsStatic;
		}

		internal bool <GetFields>b__3_1(FieldInfo member)
		{
			return member.IsStatic;
		}
	}

	public static FieldInfo GetField(this Type type, string name)
	{
		return type.GetField(name, global::BindingFlags.Static);
	}

	public static FieldInfo GetField(this Type type, string name, global::BindingFlags bindingFlags)
	{
		bool flag = (bindingFlags & global::BindingFlags.FlattenHierarchy) > (global::BindingFlags)0;
		bool flag2 = (bindingFlags & global::BindingFlags.Static) > (global::BindingFlags)0;
		FieldInfo fieldInfo = flag ? type.GetRuntimeField(name) : type.GetTypeInfo().GetDeclaredField(name);
		if (!flag2 && fieldInfo.IsStatic)
		{
			fieldInfo = null;
		}
		return fieldInfo;
	}

	public static FieldInfo[] GetFields(this Type type)
	{
		return type.GetFields((global::BindingFlags)0);
	}

	public static FieldInfo[] GetFields(this Type type, global::BindingFlags bindingFlags)
	{
		if (type == null)
		{
			return null;
		}
		bool flag = (bindingFlags & global::BindingFlags.FlattenHierarchy) > (global::BindingFlags)0;
		bool includeStatics = (bindingFlags & global::BindingFlags.Static) > (global::BindingFlags)0;
		TypeInfo typeInfo = type.GetTypeInfo();
		List<FieldInfo> list = flag ? type.GetRuntimeFields().ToList<FieldInfo>() : typeInfo.DeclaredFields.ToList<FieldInfo>();
		if (!includeStatics)
		{
			IEnumerable<FieldInfo> arg_6E_0 = list;
			Func<FieldInfo, bool> arg_6E_1;
			if ((arg_6E_1 = ReflectionUtility.<>c.<>9__3_0) == null)
			{
				arg_6E_1 = (ReflectionUtility.<>c.<>9__3_0 = new Func<FieldInfo, bool>(ReflectionUtility.<>c.<>9.<GetFields>b__3_0));
			}
			list = arg_6E_0.Where(arg_6E_1).ToList<FieldInfo>();
		}
		else if (flag)
		{
			Type baseType = typeInfo.BaseType;
			if (baseType != null && baseType != typeof(object))
			{
				List<FieldInfo> arg_C5_0 = list;
				IEnumerable<FieldInfo> arg_C0_0 = baseType.GetFields(bindingFlags);
				Func<FieldInfo, bool> arg_C0_1;
				if ((arg_C0_1 = ReflectionUtility.<>c.<>9__3_1) == null)
				{
					arg_C0_1 = (ReflectionUtility.<>c.<>9__3_1 = new Func<FieldInfo, bool>(ReflectionUtility.<>c.<>9.<GetFields>b__3_1));
				}
				arg_C5_0.AddRange(arg_C0_0.Where(arg_C0_1));
			}
		}
		return (from x in list
		where x.IsStatic == includeStatics
		select x).ToArray<FieldInfo>();
	}

	public static MethodInfo GetMethod(this Type type, string name)
	{
		return type.GetMethod(name, (global::BindingFlags)0, new Type[0]);
	}

	public static MethodInfo GetMethod(this Type type, string name, Type[] types)
	{
		return type.GetMethod(name, (global::BindingFlags)0, types);
	}

	public static MethodInfo GetMethod(this Type type, string name, global::BindingFlags bindingFlags)
	{
		return type.GetMethod(name, bindingFlags, new Type[0]);
	}

	public static MethodInfo GetMethod(this Type type, string name, global::BindingFlags bindingFlags, Type[] types)
	{
		bool flag = (bindingFlags & global::BindingFlags.Static) > (global::BindingFlags)0;
		MethodInfo methodInfo = null;
		TypeInfo typeInfo = type.GetTypeInfo();
		IEnumerable<MethodInfo> declaredMethods = typeInfo.GetDeclaredMethods(name);
		using (IEnumerator<MethodInfo> enumerator = declaredMethods.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MethodInfo current = enumerator.get_Current();
				ParameterInfo[] parameters = current.GetParameters();
				if (parameters.Length == types.Length)
				{
					bool flag2 = true;
					for (int i = 0; i < parameters.Length; i++)
					{
						if (types[i] != parameters[i].GetParameterType())
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						methodInfo = current;
						break;
					}
				}
			}
		}
		if (methodInfo == null)
		{
			methodInfo = type.GetRuntimeMethod(name, types);
		}
		if (methodInfo == null)
		{
			Type baseType = typeInfo.BaseType;
			if (baseType != null && baseType != typeof(object))
			{
				methodInfo = baseType.GetMethod(name, bindingFlags, types);
			}
		}
		if (!flag && methodInfo != null && methodInfo.IsStatic)
		{
			methodInfo = null;
		}
		return methodInfo;
	}

	public static Type GetParameterType(this ParameterInfo parameterInfo)
	{
		Type parameterType = parameterInfo.ParameterType;
		if (parameterType.GetTypeInfo().IsEnum)
		{
			return typeof(int);
		}
		return parameterType;
	}

	public static bool IsInstanceOfType(this Type type, Type target)
	{
		return target.GetTypeInfo().IsSubclassOf(type) || target.Equals(type);
	}

	public static bool IsInstanceOfType(this Type type, object target)
	{
		return type.IsInstanceOfType(target.GetType());
	}

	public static bool IsAssignableFrom(this Type type, Type target)
	{
		return type.GetTypeInfo().IsAssignableFrom(target.GetTypeInfo());
	}

	public static bool IsValueType(this Type type)
	{
		return type.GetTypeInfo().IsValueType;
	}

	public static PropertyInfo[] GetProperties(this Type type, global::BindingFlags bindingFlags)
	{
		if (type == null)
		{
			return null;
		}
		bool flag = (bindingFlags & global::BindingFlags.FlattenHierarchy) > (global::BindingFlags)0;
		TypeInfo typeInfo = type.GetTypeInfo();
		List<PropertyInfo> list = flag ? type.GetRuntimeProperties().ToList<PropertyInfo>() : typeInfo.DeclaredProperties.ToList<PropertyInfo>();
		return list.ToArray();
	}

	public static PropertyInfo[] GetProperties(this Type type)
	{
		if (type == null)
		{
			return null;
		}
		TypeInfo typeInfo = type.GetTypeInfo();
		List<PropertyInfo> list = typeInfo.DeclaredProperties.ToList<PropertyInfo>();
		return list.ToArray();
	}

	public static PropertyInfo GetProperty(this Type type, string propertyName)
	{
		return type.GetTypeInfo().GetDeclaredProperty(propertyName);
	}

	public static PropertyInfo GetProperty(this Type type, string propertyName, global::BindingFlags flags)
	{
		return type.GetProperties(flags).FirstOrDefault((PropertyInfo p) => p.Name == propertyName);
	}

	public static Type[] GetGenericArguments(this Type type)
	{
		return type.GetTypeInfo().GenericTypeArguments;
	}

	public static TypeInfo GetTypeInfo(this Type type)
	{
		IReflectableType reflectableType = (IReflectableType)type;
		return reflectableType.GetTypeInfo();
	}

	public static Type GetBaseType(this Type type)
	{
		return type.GetTypeInfo().BaseType;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetBaseType());
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetField(Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetField(Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (global::BindingFlags)(*(int*)(args + 2))));
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetFields());
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetFields((global::BindingFlags)(*(int*)(args + 1))));
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetGenericArguments());
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetMethod(Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetMethod(Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (global::BindingFlags)(*(int*)(args + 2))));
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetMethod(Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (Type[])GCHandledObjects.GCHandleToPinnedArrayObject(args[2])));
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetMethod(Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (global::BindingFlags)(*(int*)(args + 2)), (Type[])GCHandledObjects.GCHandleToPinnedArrayObject(args[3])));
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((ParameterInfo)GCHandledObjects.GCHandleToObject(*args)).GetParameterType());
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetProperties());
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetProperties((global::BindingFlags)(*(int*)(args + 1))));
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (global::BindingFlags)(*(int*)(args + 2))));
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).GetTypeInfo());
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).IsAssignableFrom((Type)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).IsInstanceOfType(GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).IsInstanceOfType((Type)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Type)GCHandledObjects.GCHandleToObject(*args)).IsValueType());
	}
}
