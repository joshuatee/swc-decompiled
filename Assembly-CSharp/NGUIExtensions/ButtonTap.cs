using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

namespace NGUIExtensions
{
	public class ButtonTap : MonoBehaviour, IUnitySerializable
	{
		public Vector3 downScale;

		public float downAlpha;

		public float transitionTime;

		private Vector3 defaultScale;

		private float defaultAlpha;

		public void OnPress(bool isDown)
		{
			float duration = this.transitionTime * 0.5f;
			TweenScale.Begin(base.gameObject, duration, this.downScale);
			TweenAlpha tweenAlpha = TweenAlpha.Begin(base.gameObject, duration, this.downAlpha);
			tweenAlpha.from = this.defaultAlpha;
			if (!isDown)
			{
				TweenScale.Begin(base.gameObject, duration, this.defaultScale);
				TweenAlpha.Begin(base.gameObject, duration, this.defaultAlpha);
				tweenAlpha.from = this.downAlpha;
			}
		}

		public ButtonTap()
		{
			this.downScale = new Vector3(0.95f, 0.95f, 0f);
			this.downAlpha = 0.8f;
			this.transitionTime = 0.25f;
			this.defaultScale = new Vector3(1f, 1f, 0f);
			this.defaultAlpha = 1f;
			base..ctor();
		}

		public override void Unity_Serialize(int depth)
		{
			if (depth <= 7)
			{
				this.downScale.Unity_Serialize(depth + 1);
			}
			SerializedStateWriter.Instance.Align();
			SerializedStateWriter.Instance.WriteSingle(this.downAlpha);
			SerializedStateWriter.Instance.WriteSingle(this.transitionTime);
		}

		public override void Unity_Deserialize(int depth)
		{
			if (depth <= 7)
			{
				this.downScale.Unity_Deserialize(depth + 1);
			}
			SerializedStateReader.Instance.Align();
			this.downAlpha = SerializedStateReader.Instance.ReadSingle();
			this.transitionTime = SerializedStateReader.Instance.ReadSingle();
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public unsafe override void Unity_NamedSerialize(int depth)
		{
			byte[] var_0_cp_0;
			int var_0_cp_1;
			if (depth <= 7)
			{
				ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
				var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
				var_0_cp_1 = 0;
				arg_23_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5680);
				this.downScale.Unity_NamedSerialize(depth + 1);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			SerializedNamedStateWriter.Instance.Align();
			SerializedNamedStateWriter.Instance.WriteSingle(this.downAlpha, &var_0_cp_0[var_0_cp_1] + 5690);
			SerializedNamedStateWriter.Instance.WriteSingle(this.transitionTime, &var_0_cp_0[var_0_cp_1] + 5700);
		}

		public unsafe override void Unity_NamedDeserialize(int depth)
		{
			byte[] var_0_cp_0;
			int var_0_cp_1;
			if (depth <= 7)
			{
				ISerializedNamedStateReader arg_23_0 = SerializedNamedStateReader.Instance;
				var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
				var_0_cp_1 = 0;
				arg_23_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 5680);
				this.downScale.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			SerializedNamedStateReader.Instance.Align();
			this.downAlpha = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5690);
			this.transitionTime = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5700);
		}

		protected internal ButtonTap(UIntPtr dummy) : base(dummy)
		{
		}

		public static float $Get0(object instance, int index)
		{
			ButtonTap expr_06_cp_0 = (ButtonTap)instance;
			switch (index)
			{
			case 0:
				return expr_06_cp_0.downScale.x;
			case 1:
				return expr_06_cp_0.downScale.y;
			case 2:
				return expr_06_cp_0.downScale.z;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static void $Set0(object instance, float value, int index)
		{
			ButtonTap expr_06_cp_0 = (ButtonTap)instance;
			switch (index)
			{
			case 0:
				expr_06_cp_0.downScale.x = value;
				return;
			case 1:
				expr_06_cp_0.downScale.y = value;
				return;
			case 2:
				expr_06_cp_0.downScale.z = value;
				return;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static float $Get1(object instance)
		{
			return ((ButtonTap)instance).downAlpha;
		}

		public static void $Set1(object instance, float value)
		{
			((ButtonTap)instance).downAlpha = value;
		}

		public static float $Get2(object instance)
		{
			return ((ButtonTap)instance).transitionTime;
		}

		public static void $Set2(object instance, float value)
		{
			((ButtonTap)instance).transitionTime = value;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ButtonTap)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ButtonTap)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ButtonTap)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ButtonTap)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ButtonTap)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ButtonTap)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
