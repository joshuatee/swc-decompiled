using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Sprite Animation"), ExecuteInEditMode, RequireComponent(typeof(UISprite))]
public class UISpriteAnimation : MonoBehaviour, IUnitySerializable
{
	[HideInInspector, SerializeField]
	protected int mFPS;

	[HideInInspector, SerializeField]
	protected string mPrefix;

	[HideInInspector, SerializeField]
	protected bool mLoop;

	[HideInInspector, SerializeField]
	protected bool mSnap;

	protected UISprite mSprite;

	protected float mDelta;

	protected int mIndex;

	protected bool mActive;

	protected List<string> mSpriteNames;

	public int frames
	{
		get
		{
			return this.mSpriteNames.Count;
		}
	}

	public int framesPerSecond
	{
		get
		{
			return this.mFPS;
		}
		set
		{
			this.mFPS = value;
		}
	}

	public string namePrefix
	{
		get
		{
			return this.mPrefix;
		}
		set
		{
			if (this.mPrefix != value)
			{
				this.mPrefix = value;
				this.RebuildSpriteList();
			}
		}
	}

	public bool loop
	{
		get
		{
			return this.mLoop;
		}
		set
		{
			this.mLoop = value;
		}
	}

	public bool isPlaying
	{
		get
		{
			return this.mActive;
		}
	}

	protected virtual void Start()
	{
		this.RebuildSpriteList();
	}

	protected virtual void Update()
	{
		if (this.mActive && this.mSpriteNames.Count > 1 && Application.isPlaying && this.mFPS > 0)
		{
			this.mDelta += RealTime.deltaTime;
			float num = 1f / (float)this.mFPS;
			if (num < this.mDelta)
			{
				this.mDelta = ((num > 0f) ? (this.mDelta - num) : 0f);
				int num2 = this.mIndex + 1;
				this.mIndex = num2;
				if (num2 >= this.mSpriteNames.Count)
				{
					this.mIndex = 0;
					this.mActive = this.mLoop;
				}
				if (this.mActive)
				{
					this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
					if (this.mSnap)
					{
						this.mSprite.MakePixelPerfect();
					}
				}
			}
		}
	}

	public void RebuildSpriteList()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			List<UISpriteData> spriteList = this.mSprite.atlas.spriteList;
			int i = 0;
			int count = spriteList.Count;
			while (i < count)
			{
				UISpriteData uISpriteData = spriteList[i];
				if (string.IsNullOrEmpty(this.mPrefix) || uISpriteData.name.StartsWith(this.mPrefix))
				{
					this.mSpriteNames.Add(uISpriteData.name);
				}
				i++;
			}
			this.mSpriteNames.Sort();
		}
	}

	public void Play()
	{
		this.mActive = true;
	}

	public void Pause()
	{
		this.mActive = false;
	}

	public void ResetToBeginning()
	{
		this.mActive = true;
		this.mIndex = 0;
		if (this.mSprite != null && this.mSpriteNames.Count > 0)
		{
			this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
			if (this.mSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	public UISpriteAnimation()
	{
		this.mFPS = 30;
		this.mPrefix = "";
		this.mLoop = true;
		this.mSnap = true;
		this.mActive = true;
		this.mSpriteNames = new List<string>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32(this.mFPS);
		SerializedStateWriter.Instance.WriteString(this.mPrefix);
		SerializedStateWriter.Instance.WriteBoolean(this.mLoop);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.mSnap);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		this.mFPS = SerializedStateReader.Instance.ReadInt32();
		this.mPrefix = (SerializedStateReader.Instance.ReadString() as string);
		this.mLoop = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.mSnap = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = this.mFPS;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 4441);
		SerializedNamedStateWriter.Instance.WriteString(this.mPrefix, &var_0_cp_0[var_0_cp_1] + 4446);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mLoop, &var_0_cp_0[var_0_cp_1] + 4454);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mSnap, &var_0_cp_0[var_0_cp_1] + 4460);
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.mFPS = arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4441);
		this.mPrefix = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 4446) as string);
		this.mLoop = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4454);
		SerializedNamedStateReader.Instance.Align();
		this.mSnap = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4460);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UISpriteAnimation(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UISpriteAnimation)instance).mLoop;
	}

	public static void $Set0(object instance, bool value)
	{
		((UISpriteAnimation)instance).mLoop = value;
	}

	public static bool $Get1(object instance)
	{
		return ((UISpriteAnimation)instance).mSnap;
	}

	public static void $Set1(object instance, bool value)
	{
		((UISpriteAnimation)instance).mSnap = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).frames);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).framesPerSecond);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).isPlaying);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).loop);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).namePrefix);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Pause();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Play();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).RebuildSpriteList();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).ResetToBeginning();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).framesPerSecond = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).loop = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).namePrefix = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UISpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
