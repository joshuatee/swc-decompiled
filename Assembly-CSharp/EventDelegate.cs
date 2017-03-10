using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[System.Serializable]
public class EventDelegate : IUnitySerializable
{
	[System.Serializable]
	public class Parameter : IUnitySerializable
	{
		public UnityEngine.Object obj;

		public string field;

		[System.NonSerialized]
		private object mValue;

		[System.NonSerialized]
		public Type expectedType;

		[System.NonSerialized]
		public bool cached;

		[System.NonSerialized]
		public PropertyInfo propInfo;

		[System.NonSerialized]
		public FieldInfo fieldInfo;

		public object value
		{
			get
			{
				if (this.mValue != null)
				{
					return this.mValue;
				}
				if (!this.cached)
				{
					this.cached = true;
					this.fieldInfo = null;
					this.propInfo = null;
					if (this.obj != null && !string.IsNullOrEmpty(this.field))
					{
						Type type = this.obj.GetType();
						this.propInfo = type.GetRuntimeProperty(this.field);
						if (this.propInfo == null)
						{
							this.fieldInfo = type.GetRuntimeField(this.field);
						}
					}
				}
				if (this.propInfo != null)
				{
					return this.propInfo.GetValue(this.obj, null);
				}
				if (this.fieldInfo != null)
				{
					return this.fieldInfo.GetValue(this.obj);
				}
				if (this.obj != null)
				{
					return this.obj;
				}
				return Convert.ChangeType(null, this.expectedType);
			}
			set
			{
				this.mValue = value;
			}
		}

		public Type type
		{
			get
			{
				if (this.mValue != null)
				{
					return this.mValue.GetType();
				}
				if (this.obj == null)
				{
					return typeof(void);
				}
				return this.obj.GetType();
			}
		}

		public Parameter()
		{
			this.expectedType = typeof(void);
			base..ctor();
		}

		public Parameter(UnityEngine.Object obj, string field)
		{
			this.expectedType = typeof(void);
			base..ctor();
			this.obj = obj;
			this.field = field;
		}

		public Parameter(object val)
		{
			this.expectedType = typeof(void);
			base..ctor();
			this.mValue = val;
		}

		public override void Unity_Serialize(int depth)
		{
			if (depth <= 7)
			{
				SerializedStateWriter.Instance.WriteUnityEngineObject(this.obj);
			}
			SerializedStateWriter.Instance.WriteString(this.field);
		}

		public override void Unity_Deserialize(int depth)
		{
			if (depth <= 7)
			{
				this.obj = (SerializedStateReader.Instance.ReadUnityEngineObject() as UnityEngine.Object);
			}
			this.field = (SerializedStateReader.Instance.ReadString() as string);
		}

		public override void Unity_RemapPPtrs(int depth)
		{
			if (this.obj != null)
			{
				this.obj = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.obj) as UnityEngine.Object);
			}
		}

		public unsafe override void Unity_NamedSerialize(int depth)
		{
			byte[] var_0_cp_0;
			int var_0_cp_1;
			if (depth <= 7)
			{
				ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
				UnityEngine.Object arg_23_1 = this.obj;
				var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
				var_0_cp_1 = 0;
				arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 2215);
			}
			SerializedNamedStateWriter.Instance.WriteString(this.field, &var_0_cp_0[var_0_cp_1] + 2219);
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
				this.obj = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2215) as UnityEngine.Object);
			}
			this.field = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 2219) as string);
		}

		protected internal Parameter(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventDelegate.Parameter)GCHandledObjects.GCHandleToObject(instance)).type);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventDelegate.Parameter)GCHandledObjects.GCHandleToObject(instance)).value);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EventDelegate.Parameter)GCHandledObjects.GCHandleToObject(instance)).value = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EventDelegate.Parameter)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EventDelegate.Parameter)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EventDelegate.Parameter)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EventDelegate.Parameter)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((EventDelegate.Parameter)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}

	public delegate void Callback();

	[SerializeField]
	protected internal MonoBehaviour mTarget;

	[SerializeField]
	protected internal string mMethodName;

	[SerializeField]
	protected internal EventDelegate.Parameter[] mParameters;

	public bool oneShot;

	[System.NonSerialized]
	private EventDelegate.Callback mCachedCallback;

	[System.NonSerialized]
	private bool mRawDelegate;

	[System.NonSerialized]
	private bool mCached;

	[System.NonSerialized]
	private MethodInfo mMethod;

	[System.NonSerialized]
	private ParameterInfo[] mParameterInfos;

	[System.NonSerialized]
	private object[] mArgs;

	private static int s_Hash = "EventDelegate".GetHashCode();

	public MonoBehaviour target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
			this.mCached = false;
			this.mMethod = null;
			this.mParameterInfos = null;
			this.mParameters = null;
		}
	}

	public string methodName
	{
		get
		{
			return this.mMethodName;
		}
		set
		{
			this.mMethodName = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
			this.mCached = false;
			this.mMethod = null;
			this.mParameterInfos = null;
			this.mParameters = null;
		}
	}

	public EventDelegate.Parameter[] parameters
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			return this.mParameters;
		}
	}

	public bool isValid
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			return (this.mRawDelegate && this.mCachedCallback != null) || (this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName));
		}
	}

	public bool isEnabled
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRawDelegate && this.mCachedCallback != null)
			{
				return true;
			}
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget;
			return monoBehaviour == null || monoBehaviour.enabled;
		}
	}

	public EventDelegate()
	{
	}

	public EventDelegate(EventDelegate.Callback call)
	{
		this.Set(call);
	}

	public EventDelegate(MonoBehaviour target, string methodName)
	{
		this.Set(target, methodName);
	}

	private static string GetMethodName(EventDelegate.Callback callback)
	{
		return callback.GetMethodInfo().Name;
	}

	private static bool IsValid(EventDelegate.Callback callback)
	{
		return callback != null && callback.GetMethodInfo() != null;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is EventDelegate.Callback)
		{
			EventDelegate.Callback callback = obj as EventDelegate.Callback;
			if (callback.Equals(this.mCachedCallback))
			{
				return true;
			}
			MonoBehaviour y = callback.get_Target() as MonoBehaviour;
			return this.mTarget == y && string.Equals(this.mMethodName, EventDelegate.GetMethodName(callback));
		}
		else
		{
			if (obj is EventDelegate)
			{
				EventDelegate eventDelegate = obj as EventDelegate;
				return this.mTarget == eventDelegate.mTarget && string.Equals(this.mMethodName, eventDelegate.mMethodName);
			}
			return false;
		}
	}

	public override int GetHashCode()
	{
		return EventDelegate.s_Hash;
	}

	private void Set(EventDelegate.Callback call)
	{
		this.Clear();
		if (call != null && EventDelegate.IsValid(call))
		{
			this.mTarget = (call.get_Target() as MonoBehaviour);
			if (this.mTarget == null)
			{
				this.mRawDelegate = true;
				this.mCachedCallback = call;
				this.mMethodName = null;
				return;
			}
			this.mMethodName = EventDelegate.GetMethodName(call);
			this.mRawDelegate = false;
		}
	}

	public void Set(MonoBehaviour target, string methodName)
	{
		this.Clear();
		this.mTarget = target;
		this.mMethodName = methodName;
	}

	private void Cache()
	{
		this.mCached = true;
		if (this.mRawDelegate)
		{
			return;
		}
		if ((this.mCachedCallback == null || this.mCachedCallback.get_Target() as MonoBehaviour != this.mTarget || EventDelegate.GetMethodName(this.mCachedCallback) != this.mMethodName) && this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName))
		{
			Type type = this.mTarget.GetType();
			try
			{
				IEnumerable<MethodInfo> runtimeMethods = type.GetRuntimeMethods();
				using (IEnumerator<MethodInfo> enumerator = runtimeMethods.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MethodInfo current = enumerator.get_Current();
						if (current.Name == this.mMethodName)
						{
							this.mMethod = current;
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Failed to bind ",
					type,
					".",
					this.mMethodName,
					"\n",
					ex.get_Message()
				}));
				return;
			}
			if (this.mMethod == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Could not find method '",
					this.mMethodName,
					"' on ",
					this.mTarget.GetType()
				}), this.mTarget);
				return;
			}
			if (this.mMethod.ReturnType != typeof(void))
			{
				Debug.LogError(string.Concat(new object[]
				{
					this.mTarget.GetType(),
					".",
					this.mMethodName,
					" must have a 'void' return type."
				}), this.mTarget);
				return;
			}
			this.mParameterInfos = this.mMethod.GetParameters();
			if (this.mParameterInfos.Length == 0)
			{
				this.mCachedCallback = (EventDelegate.Callback)this.mMethod.CreateDelegate(typeof(EventDelegate.Callback), this.mTarget);
				this.mArgs = null;
				this.mParameters = null;
				return;
			}
			this.mCachedCallback = null;
			if (this.mParameters == null || this.mParameters.Length != this.mParameterInfos.Length)
			{
				this.mParameters = new EventDelegate.Parameter[this.mParameterInfos.Length];
				int i = 0;
				int num = this.mParameters.Length;
				while (i < num)
				{
					this.mParameters[i] = new EventDelegate.Parameter();
					i++;
				}
			}
			int j = 0;
			int num2 = this.mParameters.Length;
			while (j < num2)
			{
				this.mParameters[j].expectedType = this.mParameterInfos[j].ParameterType;
				j++;
			}
		}
	}

	public bool Execute()
	{
		if (!this.mCached)
		{
			this.Cache();
		}
		if (this.mCachedCallback != null)
		{
			this.mCachedCallback();
			return true;
		}
		if (this.mMethod != null)
		{
			if (this.mParameters == null || this.mParameters.Length == 0)
			{
				this.mMethod.Invoke(this.mTarget, null);
			}
			else
			{
				if (this.mArgs == null || this.mArgs.Length != this.mParameters.Length)
				{
					this.mArgs = new object[this.mParameters.Length];
				}
				int i = 0;
				int num = this.mParameters.Length;
				while (i < num)
				{
					this.mArgs[i] = this.mParameters[i].value;
					i++;
				}
				try
				{
					this.mMethod.Invoke(this.mTarget, this.mArgs);
				}
				catch (ArgumentException ex)
				{
					string text = "Error calling ";
					if (this.mTarget == null)
					{
						text += this.mMethod.Name;
					}
					else
					{
						text = string.Concat(new object[]
						{
							text,
							this.mTarget.GetType(),
							".",
							this.mMethod.Name
						});
					}
					text = text + ": " + ex.get_Message();
					text += "\n  Expected: ";
					if (this.mParameterInfos.Length == 0)
					{
						text += "no arguments";
					}
					else
					{
						text += this.mParameterInfos[0];
						for (int j = 1; j < this.mParameterInfos.Length; j++)
						{
							text = text + ", " + this.mParameterInfos[j].ParameterType;
						}
					}
					text += "\n  Received: ";
					if (this.mParameters.Length == 0)
					{
						text += "no arguments";
					}
					else
					{
						text += this.mParameters[0].type;
						for (int k = 1; k < this.mParameters.Length; k++)
						{
							text = text + ", " + this.mParameters[k].type;
						}
					}
					text += "\n";
					Debug.LogError(text);
				}
				int l = 0;
				int num2 = this.mArgs.Length;
				while (l < num2)
				{
					if (this.mParameterInfos[l].IsIn || this.mParameterInfos[l].IsOut)
					{
						this.mParameters[l].value = this.mArgs[l];
					}
					this.mArgs[l] = null;
					l++;
				}
			}
			return true;
		}
		return false;
	}

	public void Clear()
	{
		this.mTarget = null;
		this.mMethodName = null;
		this.mRawDelegate = false;
		this.mCachedCallback = null;
		this.mParameters = null;
		this.mCached = false;
		this.mMethod = null;
		this.mParameterInfos = null;
		this.mArgs = null;
	}

	public override string ToString()
	{
		if (this.mTarget != null)
		{
			string text = this.mTarget.GetType().ToString();
			int num = text.LastIndexOf('.');
			if (num > 0)
			{
				text = text.Substring(num + 1);
			}
			if (!string.IsNullOrEmpty(this.methodName))
			{
				return text + "/" + this.methodName;
			}
			return text + "/[delegate]";
		}
		else
		{
			if (!this.mRawDelegate)
			{
				return null;
			}
			return "[delegate]";
		}
	}

	public static void Execute(List<EventDelegate> list)
	{
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null)
				{
					try
					{
						eventDelegate.Execute();
					}
					catch (Exception ex)
					{
						if (ex.get_InnerException() != null)
						{
							Debug.LogError(ex.get_InnerException().get_Message());
						}
						else
						{
							Debug.LogError(ex.get_Message());
						}
					}
					if (i >= list.Count)
					{
						break;
					}
					if (list[i] != eventDelegate)
					{
						continue;
					}
					if (eventDelegate.oneShot)
					{
						list.RemoveAt(i);
						continue;
					}
				}
			}
		}
	}

	public static bool IsValid(List<EventDelegate> list)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.isValid)
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	public static EventDelegate Set(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			EventDelegate eventDelegate = new EventDelegate(callback);
			list.Clear();
			list.Add(eventDelegate);
			return eventDelegate;
		}
		return null;
	}

	public static void Set(List<EventDelegate> list, EventDelegate del)
	{
		if (list != null)
		{
			list.Clear();
			list.Add(del);
		}
	}

	public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		return EventDelegate.Add(list, callback, false);
	}

	public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback, bool oneShot)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					return eventDelegate;
				}
				i++;
			}
			EventDelegate eventDelegate2 = new EventDelegate(callback);
			eventDelegate2.oneShot = oneShot;
			list.Add(eventDelegate2);
			return eventDelegate2;
		}
		Debug.LogWarning("Attempting to add a callback to a list that's null");
		return null;
	}

	public static void Add(List<EventDelegate> list, EventDelegate ev)
	{
		EventDelegate.Add(list, ev, ev.oneShot);
	}

	public static void Add(List<EventDelegate> list, EventDelegate ev, bool oneShot)
	{
		if (ev.mRawDelegate || ev.target == null || string.IsNullOrEmpty(ev.methodName))
		{
			EventDelegate.Add(list, ev.mCachedCallback, oneShot);
			return;
		}
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(ev))
				{
					return;
				}
				i++;
			}
			EventDelegate eventDelegate2 = new EventDelegate(ev.target, ev.methodName);
			eventDelegate2.oneShot = oneShot;
			if (ev.mParameters != null && ev.mParameters.Length != 0)
			{
				eventDelegate2.mParameters = new EventDelegate.Parameter[ev.mParameters.Length];
				for (int j = 0; j < ev.mParameters.Length; j++)
				{
					eventDelegate2.mParameters[j] = ev.mParameters[j];
				}
			}
			list.Add(eventDelegate2);
			return;
		}
		Debug.LogWarning("Attempting to add a callback to a list that's null");
	}

	public static bool Remove(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					list.RemoveAt(i);
					return true;
				}
				i++;
			}
		}
		return false;
	}

	public static bool Remove(List<EventDelegate> list, EventDelegate ev)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(ev))
				{
					list.RemoveAt(i);
					return true;
				}
				i++;
			}
		}
		return false;
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mTarget);
		}
		SerializedStateWriter.Instance.WriteString(this.mMethodName);
		if (depth <= 7)
		{
			if (this.mParameters == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.mParameters.Length);
				for (int i = 0; i < this.mParameters.Length; i++)
				{
					((this.mParameters[i] != null) ? this.mParameters[i] : new EventDelegate.Parameter()).Unity_Serialize(depth + 1);
				}
			}
		}
		SerializedStateWriter.Instance.WriteBoolean(this.oneShot);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.mTarget = (SerializedStateReader.Instance.ReadUnityEngineObject() as MonoBehaviour);
		}
		this.mMethodName = (SerializedStateReader.Instance.ReadString() as string);
		if (depth <= 7)
		{
			this.mParameters = new EventDelegate.Parameter[SerializedStateReader.Instance.ReadInt32()];
			for (int i = 0; i < this.mParameters.Length; i++)
			{
				this.mParameters[i] = new EventDelegate.Parameter();
				this.mParameters[i].Unity_Deserialize(depth + 1);
			}
		}
		this.oneShot = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.mTarget != null)
		{
			this.mTarget = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mTarget) as MonoBehaviour);
		}
		if (depth <= 7)
		{
			if (this.mParameters != null)
			{
				for (int i = 0; i < this.mParameters.Length; i++)
				{
					if (this.mParameters[i] != null)
					{
						this.mParameters[i].Unity_RemapPPtrs(depth + 1);
					}
				}
			}
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
		SerializedNamedStateWriter.Instance.WriteString(this.mMethodName, &var_0_cp_0[var_0_cp_1] + 2233);
		if (depth <= 7)
		{
			if (this.mParameters == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2245, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2245, this.mParameters.Length);
				for (int i = 0; i < this.mParameters.Length; i++)
				{
					EventDelegate.Parameter arg_C0_0 = (this.mParameters[i] != null) ? this.mParameters[i] : new EventDelegate.Parameter();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_C0_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		SerializedNamedStateWriter.Instance.WriteBoolean(this.oneShot, &var_0_cp_0[var_0_cp_1] + 2257);
		SerializedNamedStateWriter.Instance.Align();
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
			this.mTarget = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2225) as MonoBehaviour);
		}
		this.mMethodName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 2233) as string);
		if (depth <= 7)
		{
			this.mParameters = new EventDelegate.Parameter[SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2245)];
			for (int i = 0; i < this.mParameters.Length; i++)
			{
				this.mParameters[i] = new EventDelegate.Parameter();
				EventDelegate.Parameter arg_99_0 = this.mParameters[i];
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_99_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		this.oneShot = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2257);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal EventDelegate(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		EventDelegate.Add((List<EventDelegate>)GCHandledObjects.GCHandleToObject(*args), (EventDelegate)GCHandledObjects.GCHandleToObject(args[1]));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(EventDelegate.Add((List<EventDelegate>)GCHandledObjects.GCHandleToObject(*args), (EventDelegate.Callback)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		EventDelegate.Add((List<EventDelegate>)GCHandledObjects.GCHandleToObject(*args), (EventDelegate)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(EventDelegate.Add((List<EventDelegate>)GCHandledObjects.GCHandleToObject(*args), (EventDelegate.Callback)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Cache();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Equals(GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		EventDelegate.Execute((List<EventDelegate>)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Execute());
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).isEnabled);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).isValid);
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).methodName);
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).parameters);
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).target);
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).GetHashCode());
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(EventDelegate.GetMethodName((EventDelegate.Callback)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(EventDelegate.IsValid((List<EventDelegate>)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(EventDelegate.IsValid((EventDelegate.Callback)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(EventDelegate.Remove((List<EventDelegate>)GCHandledObjects.GCHandleToObject(*args), (EventDelegate)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(EventDelegate.Remove((List<EventDelegate>)GCHandledObjects.GCHandleToObject(*args), (EventDelegate.Callback)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		EventDelegate.Set((List<EventDelegate>)GCHandledObjects.GCHandleToObject(*args), (EventDelegate)GCHandledObjects.GCHandleToObject(args[1]));
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(EventDelegate.Set((List<EventDelegate>)GCHandledObjects.GCHandleToObject(*args), (EventDelegate.Callback)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Set((MonoBehaviour)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Set((EventDelegate.Callback)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).methodName = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).target = (MonoBehaviour)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).ToString());
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		((EventDelegate)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
