using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

public class UI2DSpriteAnimation : MonoBehaviour, IUnitySerializable
{
	[SerializeField]
	protected int framerate;

	public bool ignoreTimeScale;

	public bool loop;

	public Sprite[] frames;

	private SpriteRenderer mUnitySprite;

	private UI2DSprite mNguiSprite;

	private int mIndex;

	private float mUpdate;

	public bool isPlaying
	{
		get
		{
			return base.enabled;
		}
	}

	public int framesPerSecond
	{
		get
		{
			return this.framerate;
		}
		set
		{
			this.framerate = value;
		}
	}

	public void Play()
	{
		if (this.frames != null && this.frames.Length != 0)
		{
			if (!base.enabled && !this.loop)
			{
				int num = (this.framerate > 0) ? (this.mIndex + 1) : (this.mIndex - 1);
				if (num < 0 || num >= this.frames.Length)
				{
					this.mIndex = ((this.framerate < 0) ? (this.frames.Length - 1) : 0);
				}
			}
			base.enabled = true;
			this.UpdateSprite();
		}
	}

	public void Pause()
	{
		base.enabled = false;
	}

	public void ResetToBeginning()
	{
		this.mIndex = ((this.framerate < 0) ? (this.frames.Length - 1) : 0);
		this.UpdateSprite();
	}

	private void Start()
	{
		this.Play();
	}

	private void Update()
	{
		if (this.frames == null || this.frames.Length == 0)
		{
			base.enabled = false;
			return;
		}
		if (this.framerate != 0)
		{
			float num = this.ignoreTimeScale ? RealTime.time : Time.time;
			if (this.mUpdate < num)
			{
				this.mUpdate = num;
				int num2 = (this.framerate > 0) ? (this.mIndex + 1) : (this.mIndex - 1);
				if (!this.loop && (num2 < 0 || num2 >= this.frames.Length))
				{
					base.enabled = false;
					return;
				}
				this.mIndex = NGUIMath.RepeatIndex(num2, this.frames.Length);
				this.UpdateSprite();
			}
		}
	}

	private void UpdateSprite()
	{
		if (this.mUnitySprite == null && this.mNguiSprite == null)
		{
			this.mUnitySprite = base.GetComponent<SpriteRenderer>();
			this.mNguiSprite = base.GetComponent<UI2DSprite>();
			if (this.mUnitySprite == null && this.mNguiSprite == null)
			{
				base.enabled = false;
				return;
			}
		}
		float num = this.ignoreTimeScale ? RealTime.time : Time.time;
		if (this.framerate != 0)
		{
			this.mUpdate = num + Mathf.Abs(1f / (float)this.framerate);
		}
		if (this.mUnitySprite != null)
		{
			this.mUnitySprite.sprite = this.frames[this.mIndex];
			return;
		}
		if (this.mNguiSprite != null)
		{
			this.mNguiSprite.nextSprite = this.frames[this.mIndex];
		}
	}

	public UI2DSpriteAnimation()
	{
		this.framerate = 20;
		this.ignoreTimeScale = true;
		this.loop = true;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32(this.framerate);
		SerializedStateWriter.Instance.WriteBoolean(this.ignoreTimeScale);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.loop);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			if (this.frames == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.frames.Length);
				for (int i = 0; i < this.frames.Length; i++)
				{
					SerializedStateWriter.Instance.WriteUnityEngineObject(this.frames[i]);
				}
			}
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		this.framerate = SerializedStateReader.Instance.ReadInt32();
		this.ignoreTimeScale = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.loop = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.frames = new Sprite[SerializedStateReader.Instance.ReadInt32()];
			for (int i = 0; i < this.frames.Length; i++)
			{
				this.frames[i] = (SerializedStateReader.Instance.ReadUnityEngineObject() as Sprite);
			}
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (depth <= 7)
		{
			if (this.frames != null)
			{
				for (int i = 0; i < this.frames.Length; i++)
				{
					this.frames[i] = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.frames[i]) as Sprite);
				}
			}
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = this.framerate;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 2852);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.ignoreTimeScale, &var_0_cp_0[var_0_cp_1] + 2653);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.loop, &var_0_cp_0[var_0_cp_1] + 2862);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			if (this.frames == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2867, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2867, this.frames.Length);
				for (int i = 0; i < this.frames.Length; i++)
				{
					SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.frames[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.framerate = arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2852);
		this.ignoreTimeScale = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2653);
		SerializedNamedStateReader.Instance.Align();
		this.loop = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2862);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.frames = new Sprite[SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2867)];
			for (int i = 0; i < this.frames.Length; i++)
			{
				this.frames[i] = (SerializedNamedStateReader.Instance.ReadUnityEngineObject((IntPtr)0) as Sprite);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
	}

	protected internal UI2DSpriteAnimation(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UI2DSpriteAnimation)instance).ignoreTimeScale;
	}

	public static void $Set0(object instance, bool value)
	{
		((UI2DSpriteAnimation)instance).ignoreTimeScale = value;
	}

	public static bool $Get1(object instance)
	{
		return ((UI2DSpriteAnimation)instance).loop;
	}

	public static void $Set1(object instance, bool value)
	{
		((UI2DSpriteAnimation)instance).loop = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).framesPerSecond);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).isPlaying);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Pause();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Play();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).ResetToBeginning();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).framesPerSecond = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UI2DSpriteAnimation)GCHandledObjects.GCHandleToObject(instance)).UpdateSprite();
		return -1L;
	}
}
