using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Typewriter Effect"), RequireComponent(typeof(UILabel))]
public class TypewriterEffect : MonoBehaviour, IUnitySerializable
{
	private struct FadeEntry
	{
		public int index;

		public string text;

		public float alpha;
	}

	public static TypewriterEffect current;

	public int charsPerSecond;

	public float fadeInTime;

	public float delayOnPeriod;

	public float delayOnNewLine;

	public UIScrollView scrollView;

	public bool keepFullDimensions;

	public List<EventDelegate> onFinished;

	private UILabel mLabel;

	private string mFullText;

	private int mCurrentOffset;

	private float mNextChar;

	private bool mReset;

	private bool mActive;

	private BetterList<TypewriterEffect.FadeEntry> mFade;

	public bool isActive
	{
		get
		{
			return this.mActive;
		}
	}

	public void ResetToBeginning()
	{
		this.Finish();
		this.mReset = true;
		this.mActive = true;
		this.mNextChar = 0f;
		this.mCurrentOffset = 0;
		this.Update();
	}

	public void Finish()
	{
		if (this.mActive)
		{
			this.mActive = false;
			if (!this.mReset)
			{
				this.mCurrentOffset = this.mFullText.get_Length();
				this.mFade.Clear();
				this.mLabel.text = this.mFullText;
			}
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
		}
	}

	private void OnEnable()
	{
		this.mReset = true;
		this.mActive = true;
	}

	private void OnDisable()
	{
		this.Finish();
	}

	private void Update()
	{
		if (!this.mActive)
		{
			return;
		}
		if (this.mReset)
		{
			this.mCurrentOffset = 0;
			this.mReset = false;
			this.mLabel = base.GetComponent<UILabel>();
			this.mFullText = this.mLabel.processedText;
			this.mFade.Clear();
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
		}
		if (string.IsNullOrEmpty(this.mFullText))
		{
			return;
		}
		while (this.mCurrentOffset < this.mFullText.get_Length() && this.mNextChar <= RealTime.time)
		{
			int num = this.mCurrentOffset;
			this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
			if (this.mLabel.supportEncoding)
			{
				while (NGUIText.ParseSymbol(this.mFullText, ref this.mCurrentOffset))
				{
				}
			}
			this.mCurrentOffset++;
			if (this.mCurrentOffset > this.mFullText.get_Length())
			{
				break;
			}
			float num2 = 1f / (float)this.charsPerSecond;
			char c = (num < this.mFullText.get_Length()) ? this.mFullText.get_Chars(num) : '\n';
			if (c == '\n')
			{
				num2 += this.delayOnNewLine;
			}
			else if (num + 1 == this.mFullText.get_Length() || this.mFullText.get_Chars(num + 1) <= ' ')
			{
				if (c == '.')
				{
					if (num + 2 < this.mFullText.get_Length() && this.mFullText.get_Chars(num + 1) == '.' && this.mFullText.get_Chars(num + 2) == '.')
					{
						num2 += this.delayOnPeriod * 3f;
						num += 2;
					}
					else
					{
						num2 += this.delayOnPeriod;
					}
				}
				else if (c == '!' || c == '?')
				{
					num2 += this.delayOnPeriod;
				}
			}
			if (this.mNextChar == 0f)
			{
				this.mNextChar = RealTime.time + num2;
			}
			else
			{
				this.mNextChar += num2;
			}
			if (this.fadeInTime != 0f)
			{
				TypewriterEffect.FadeEntry item = default(TypewriterEffect.FadeEntry);
				item.index = num;
				item.alpha = 0f;
				item.text = this.mFullText.Substring(num, this.mCurrentOffset - num);
				this.mFade.Add(item);
			}
			else
			{
				this.mLabel.text = (this.keepFullDimensions ? (this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset)) : this.mFullText.Substring(0, this.mCurrentOffset));
				if (!this.keepFullDimensions && this.scrollView != null)
				{
					this.scrollView.UpdatePosition();
				}
			}
		}
		if (this.mCurrentOffset >= this.mFullText.get_Length())
		{
			this.mLabel.text = this.mFullText;
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
			this.mActive = false;
			return;
		}
		if (this.mFade.size != 0)
		{
			int i = 0;
			while (i < this.mFade.size)
			{
				TypewriterEffect.FadeEntry fadeEntry = this.mFade[i];
				fadeEntry.alpha += RealTime.deltaTime / this.fadeInTime;
				if (fadeEntry.alpha < 1f)
				{
					this.mFade[i] = fadeEntry;
					i++;
				}
				else
				{
					this.mFade.RemoveAt(i);
				}
			}
			if (this.mFade.size == 0)
			{
				if (this.keepFullDimensions)
				{
					this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset);
					return;
				}
				this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset);
				return;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.mFade.size; j++)
				{
					TypewriterEffect.FadeEntry fadeEntry2 = this.mFade[j];
					if (j == 0)
					{
						stringBuilder.Append(this.mFullText.Substring(0, fadeEntry2.index));
					}
					stringBuilder.Append('[');
					stringBuilder.Append(NGUIText.EncodeAlpha(fadeEntry2.alpha));
					stringBuilder.Append(']');
					stringBuilder.Append(fadeEntry2.text);
				}
				if (this.keepFullDimensions)
				{
					stringBuilder.Append("[00]");
					stringBuilder.Append(this.mFullText.Substring(this.mCurrentOffset));
				}
				this.mLabel.text = stringBuilder.ToString();
			}
		}
	}

	public TypewriterEffect()
	{
		this.charsPerSecond = 20;
		this.onFinished = new List<EventDelegate>();
		this.mFullText = "";
		this.mReset = true;
		this.mFade = new BetterList<TypewriterEffect.FadeEntry>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32(this.charsPerSecond);
		SerializedStateWriter.Instance.WriteSingle(this.fadeInTime);
		SerializedStateWriter.Instance.WriteSingle(this.delayOnPeriod);
		SerializedStateWriter.Instance.WriteSingle(this.delayOnNewLine);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.scrollView);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.keepFullDimensions);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			if (this.onFinished == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onFinished.Count);
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					((this.onFinished[i] != null) ? this.onFinished[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		this.charsPerSecond = SerializedStateReader.Instance.ReadInt32();
		this.fadeInTime = SerializedStateReader.Instance.ReadSingle();
		this.delayOnPeriod = SerializedStateReader.Instance.ReadSingle();
		this.delayOnNewLine = SerializedStateReader.Instance.ReadSingle();
		if (depth <= 7)
		{
			this.scrollView = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIScrollView);
		}
		this.keepFullDimensions = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onFinished = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				eventDelegate.Unity_Deserialize(depth + 1);
				this.onFinished.Add(eventDelegate);
			}
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.scrollView != null)
		{
			this.scrollView = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.scrollView) as UIScrollView);
		}
		if (depth <= 7)
		{
			if (this.onFinished != null)
			{
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate eventDelegate = this.onFinished[i];
					if (eventDelegate != null)
					{
						eventDelegate.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_19_0 = SerializedNamedStateWriter.Instance;
		int arg_19_1 = this.charsPerSecond;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_19_0.WriteInt32(arg_19_1, &var_0_cp_0[var_0_cp_1]);
		SerializedNamedStateWriter.Instance.WriteSingle(this.fadeInTime, &var_0_cp_0[var_0_cp_1] + 15);
		SerializedNamedStateWriter.Instance.WriteSingle(this.delayOnPeriod, &var_0_cp_0[var_0_cp_1] + 26);
		SerializedNamedStateWriter.Instance.WriteSingle(this.delayOnNewLine, &var_0_cp_0[var_0_cp_1] + 40);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.scrollView, &var_0_cp_0[var_0_cp_1] + 55);
		}
		SerializedNamedStateWriter.Instance.WriteBoolean(this.keepFullDimensions, &var_0_cp_0[var_0_cp_1] + 66);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			if (this.onFinished == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85, this.onFinished.Count);
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate arg_116_0 = (this.onFinished[i] != null) ? this.onFinished[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_116_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_14_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.charsPerSecond = arg_14_0.ReadInt32(&var_0_cp_0[var_0_cp_1]);
		this.fadeInTime = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 15);
		this.delayOnPeriod = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 26);
		this.delayOnNewLine = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 40);
		if (depth <= 7)
		{
			this.scrollView = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 55) as UIScrollView);
		}
		this.keepFullDimensions = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 66);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85);
			this.onFinished = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_D6_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_D6_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onFinished.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
	}

	protected internal TypewriterEffect(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance)
	{
		return ((TypewriterEffect)instance).fadeInTime;
	}

	public static void $Set0(object instance, float value)
	{
		((TypewriterEffect)instance).fadeInTime = value;
	}

	public static float $Get1(object instance)
	{
		return ((TypewriterEffect)instance).delayOnPeriod;
	}

	public static void $Set1(object instance, float value)
	{
		((TypewriterEffect)instance).delayOnPeriod = value;
	}

	public static float $Get2(object instance)
	{
		return ((TypewriterEffect)instance).delayOnNewLine;
	}

	public static void $Set2(object instance, float value)
	{
		((TypewriterEffect)instance).delayOnNewLine = value;
	}

	public static long $Get3(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((TypewriterEffect)instance).scrollView);
	}

	public static void $Set3(object instance, long value)
	{
		((TypewriterEffect)instance).scrollView = (UIScrollView)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get4(object instance)
	{
		return ((TypewriterEffect)instance).keepFullDimensions;
	}

	public static void $Set4(object instance, bool value)
	{
		((TypewriterEffect)instance).keepFullDimensions = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).Finish();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).isActive);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).ResetToBeginning();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((TypewriterEffect)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
