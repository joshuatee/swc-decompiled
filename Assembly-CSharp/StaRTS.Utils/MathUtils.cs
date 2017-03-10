using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils
{
	public static class MathUtils
	{
		public const float TWO_PI = 6.28318548f;

		public const float HALF_PI = 1.57079637f;

		public const float QUARTER_PI = 0.7853982f;

		public static readonly float SQRT2 = Mathf.Sqrt(2f);

		public static readonly float SQRT3 = Mathf.Sqrt(3f);

		public static readonly float FLOAT_COMPARE_EPSILON = Mathf.Epsilon * 10f;

		public static float WrapAngle(float angle)
		{
			return MathUtils.Normalize(angle, 0f, 6.28318548f);
		}

		public static float Normalize(float value, float start, float end)
		{
			float num = end - start;
			float num2 = value - start;
			return value - Mathf.Floor(num2 / num) * num;
		}

		public static float FloatMod(float value, float end)
		{
			return value - Mathf.Floor(value / end) * end;
		}

		public static float NormalizeRange(float value, float min, float max)
		{
			return (value - min) / (max - min);
		}

		public static float NormalizeRange(float value, float min, float max, float newMin, float newMax)
		{
			return newMin + MathUtils.NormalizeRange(value, min, max) * (newMax - newMin);
		}

		public static float MinAngle(float current, float target)
		{
			float num = MathUtils.WrapAngle(target - current);
			if (num > 3.14159274f)
			{
				num -= 6.28318548f;
			}
			else if (num < -3.14159274f)
			{
				num += 6.28318548f;
			}
			return current + num;
		}

		public static int IntMultSqrt2(int val)
		{
			return val * 1414 / 1000;
		}

		public static bool IsGreaterThanZero(float a)
		{
			return a >= MathUtils.FLOAT_COMPARE_EPSILON;
		}

		public static float Sum(float[] samples)
		{
			float num = 0f;
			for (int i = 0; i < samples.Length; i++)
			{
				num += samples[i];
			}
			return num;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MathUtils.FloatMod(*(float*)args, *(float*)(args + 1)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MathUtils.IntMultSqrt2(*(int*)args));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MathUtils.IsGreaterThanZero(*(float*)args));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MathUtils.MinAngle(*(float*)args, *(float*)(args + 1)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MathUtils.Normalize(*(float*)args, *(float*)(args + 1), *(float*)(args + 2)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MathUtils.NormalizeRange(*(float*)args, *(float*)(args + 1), *(float*)(args + 2)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MathUtils.NormalizeRange(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3), *(float*)(args + 4)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MathUtils.Sum((float[])GCHandledObjects.GCHandleToPinnedArrayObject(*args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MathUtils.WrapAngle(*(float*)args));
		}
	}
}
