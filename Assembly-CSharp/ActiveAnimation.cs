using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Internal/Active Animation")]
public class ActiveAnimation : MonoBehaviour, IUnitySerializable
{
	public static ActiveAnimation current;

	public List<EventDelegate> onFinished;

	[HideInInspector]
	public GameObject eventReceiver;

	[HideInInspector]
	public string callWhenFinished;

	private Animation mAnim;

	private Direction mLastDirection;

	private Direction mDisableDirection;

	private bool mNotify;

	private Animator mAnimator;

	private string mClip;

	private float playbackTime
	{
		get
		{
			return Mathf.Clamp01(this.mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		}
	}

	public bool isPlaying
	{
		get
		{
			if (!(this.mAnim == null))
			{
				using (IEnumerator enumerator = this.mAnim.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AnimationState animationState = (AnimationState)enumerator.get_Current();
						if (this.mAnim.IsPlaying(animationState.name))
						{
							if (this.mLastDirection == Direction.Forward)
							{
								if (animationState.time < animationState.length)
								{
									bool result = true;
									return result;
								}
							}
							else
							{
								if (this.mLastDirection != Direction.Reverse)
								{
									bool result = true;
									return result;
								}
								if (animationState.time > 0f)
								{
									bool result = true;
									return result;
								}
							}
						}
					}
				}
				return false;
			}
			if (this.mAnimator != null)
			{
				if (this.mLastDirection == Direction.Reverse)
				{
					if (this.playbackTime == 0f)
					{
						return false;
					}
				}
				else if (this.playbackTime == 1f)
				{
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public void Finish()
	{
		if (this.mAnim != null)
		{
			using (IEnumerator enumerator = this.mAnim.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AnimationState animationState = (AnimationState)enumerator.get_Current();
					if (this.mLastDirection == Direction.Forward)
					{
						animationState.time = animationState.length;
					}
					else if (this.mLastDirection == Direction.Reverse)
					{
						animationState.time = 0f;
					}
				}
			}
			this.mAnim.Sample();
			return;
		}
		if (this.mAnimator != null)
		{
			this.mAnimator.Play(this.mClip, 0, (this.mLastDirection == Direction.Forward) ? 1f : 0f);
		}
	}

	public void Reset()
	{
		if (this.mAnim != null)
		{
			using (IEnumerator enumerator = this.mAnim.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AnimationState animationState = (AnimationState)enumerator.get_Current();
					if (this.mLastDirection == Direction.Reverse)
					{
						animationState.time = animationState.length;
					}
					else if (this.mLastDirection == Direction.Forward)
					{
						animationState.time = 0f;
					}
				}
				return;
			}
		}
		if (this.mAnimator != null)
		{
			this.mAnimator.Play(this.mClip, 0, (this.mLastDirection == Direction.Reverse) ? 1f : 0f);
		}
	}

	private void Start()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	private void Update()
	{
		float deltaTime = RealTime.deltaTime;
		if (deltaTime == 0f)
		{
			return;
		}
		if (this.mAnimator != null)
		{
			this.mAnimator.Update((this.mLastDirection == Direction.Reverse) ? (-deltaTime) : deltaTime);
			if (this.isPlaying)
			{
				return;
			}
			this.mAnimator.enabled = false;
			base.enabled = false;
		}
		else
		{
			if (!(this.mAnim != null))
			{
				base.enabled = false;
				return;
			}
			bool flag = false;
			using (IEnumerator enumerator = this.mAnim.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AnimationState animationState = (AnimationState)enumerator.get_Current();
					if (this.mAnim.IsPlaying(animationState.name))
					{
						float num = animationState.speed * deltaTime;
						animationState.time += num;
						if (num < 0f)
						{
							if (animationState.time > 0f)
							{
								flag = true;
							}
							else
							{
								animationState.time = 0f;
							}
						}
						else if (animationState.time < animationState.length)
						{
							flag = true;
						}
						else
						{
							animationState.time = animationState.length;
						}
					}
				}
			}
			this.mAnim.Sample();
			if (flag)
			{
				return;
			}
			base.enabled = false;
		}
		if (this.mNotify)
		{
			this.mNotify = false;
			if (ActiveAnimation.current == null)
			{
				ActiveAnimation.current = this;
				EventDelegate.Execute(this.onFinished);
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
				}
				ActiveAnimation.current = null;
			}
			if (this.mDisableDirection != Direction.Toggle && this.mLastDirection == this.mDisableDirection)
			{
				NGUITools.SetActive(base.gameObject, false);
			}
		}
	}

	private void Play(string clipName, Direction playDirection)
	{
		if (playDirection == Direction.Toggle)
		{
			playDirection = ((this.mLastDirection != Direction.Forward) ? Direction.Forward : Direction.Reverse);
		}
		if (this.mAnim != null)
		{
			base.enabled = true;
			this.mAnim.enabled = false;
			bool flag = string.IsNullOrEmpty(clipName);
			if (flag)
			{
				if (!this.mAnim.isPlaying)
				{
					this.mAnim.Play();
				}
			}
			else if (!this.mAnim.IsPlaying(clipName))
			{
				this.mAnim.Play(clipName);
			}
			using (IEnumerator enumerator = this.mAnim.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AnimationState animationState = (AnimationState)enumerator.get_Current();
					if (string.IsNullOrEmpty(clipName) || animationState.name == clipName)
					{
						float num = Mathf.Abs(animationState.speed);
						animationState.speed = num * (float)playDirection;
						if (playDirection == Direction.Reverse && animationState.time == 0f)
						{
							animationState.time = animationState.length;
						}
						else if (playDirection == Direction.Forward && animationState.time == animationState.length)
						{
							animationState.time = 0f;
						}
					}
				}
			}
			this.mLastDirection = playDirection;
			this.mNotify = true;
			this.mAnim.Sample();
			return;
		}
		if (this.mAnimator != null)
		{
			if (base.enabled && this.isPlaying && this.mClip == clipName)
			{
				this.mLastDirection = playDirection;
				return;
			}
			base.enabled = true;
			this.mNotify = true;
			this.mLastDirection = playDirection;
			this.mClip = clipName;
			this.mAnimator.Play(this.mClip, 0, (playDirection == Direction.Forward) ? 0f : 1f);
		}
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (!NGUITools.GetActive(anim.gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, true);
			UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				componentsInChildren[i].Refresh();
				i++;
			}
		}
		ActiveAnimation activeAnimation = anim.GetComponent<ActiveAnimation>();
		if (activeAnimation == null)
		{
			activeAnimation = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		activeAnimation.mAnim = anim;
		activeAnimation.mDisableDirection = (Direction)disableCondition;
		activeAnimation.onFinished.Clear();
		activeAnimation.Play(clipName, playDirection);
		if (activeAnimation.mAnim != null)
		{
			activeAnimation.mAnim.Sample();
		}
		else if (activeAnimation.mAnimator != null)
		{
			activeAnimation.mAnimator.Update(0f);
		}
		return activeAnimation;
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public static ActiveAnimation Play(Animation anim, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public static ActiveAnimation Play(Animator anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (enableBeforePlay != EnableCondition.IgnoreDisabledState && !NGUITools.GetActive(anim.gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, true);
			UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				componentsInChildren[i].Refresh();
				i++;
			}
		}
		ActiveAnimation activeAnimation = anim.GetComponent<ActiveAnimation>();
		if (activeAnimation == null)
		{
			activeAnimation = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		activeAnimation.mAnimator = anim;
		activeAnimation.mDisableDirection = (Direction)disableCondition;
		activeAnimation.onFinished.Clear();
		activeAnimation.Play(clipName, playDirection);
		if (activeAnimation.mAnim != null)
		{
			activeAnimation.mAnim.Sample();
		}
		else if (activeAnimation.mAnimator != null)
		{
			activeAnimation.mAnimator.Update(0f);
		}
		return activeAnimation;
	}

	public ActiveAnimation()
	{
		this.onFinished = new List<EventDelegate>();
		this.mClip = "";
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
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
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.eventReceiver);
		}
		SerializedStateWriter.Instance.WriteString(this.callWhenFinished);
	}

	public override void Unity_Deserialize(int depth)
	{
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
		if (depth <= 7)
		{
			this.eventReceiver = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.callWhenFinished = (SerializedStateReader.Instance.ReadString() as string);
	}

	public override void Unity_RemapPPtrs(int depth)
	{
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
		if (this.eventReceiver != null)
		{
			this.eventReceiver = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.eventReceiver) as GameObject);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			if (this.onFinished == null)
			{
				ISerializedNamedStateWriter arg_26_0 = SerializedNamedStateWriter.Instance;
				var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
				var_0_cp_1 = 0;
				arg_26_0.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85, this.onFinished.Count);
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate arg_8C_0 = (this.onFinished[i] != null) ? this.onFinished[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_8C_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.eventReceiver, &var_0_cp_0[var_0_cp_1] + 1165);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.callWhenFinished, &var_0_cp_0[var_0_cp_1] + 1179);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateReader arg_1B_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			int num = arg_1B_0.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 85);
			this.onFinished = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_47_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_47_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onFinished.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			this.eventReceiver = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1165) as GameObject);
		}
		this.callWhenFinished = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 1179) as string);
	}

	protected internal ActiveAnimation(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((ActiveAnimation)instance).eventReceiver);
	}

	public static void $Set0(object instance, long value)
	{
		((ActiveAnimation)instance).eventReceiver = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).Finish();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).isPlaying);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).playbackTime);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(ActiveAnimation.Play((Animation)GCHandledObjects.GCHandleToObject(*args), (Direction)(*(int*)(args + 1))));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(ActiveAnimation.Play((Animation)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (Direction)(*(int*)(args + 2))));
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(ActiveAnimation.Play((Animation)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (Direction)(*(int*)(args + 2)), (EnableCondition)(*(int*)(args + 3)), (DisableCondition)(*(int*)(args + 4))));
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(ActiveAnimation.Play((Animator)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (Direction)(*(int*)(args + 2)), (EnableCondition)(*(int*)(args + 3)), (DisableCondition)(*(int*)(args + 4))));
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).Play(Marshal.PtrToStringUni(*(IntPtr*)args), (Direction)(*(int*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).Reset();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((ActiveAnimation)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
