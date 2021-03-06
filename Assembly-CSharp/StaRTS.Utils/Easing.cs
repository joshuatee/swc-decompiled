using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils
{
	public static class Easing
	{
		public delegate float EasingDelegate(float t, float b, float c, float d);

		public static float AlwaysStart(float t, float b, float c, float d)
		{
			return b;
		}

		public static float AlwaysFinal(float t, float b, float c, float d)
		{
			return b + c;
		}

		public static float Linear(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		public static float ExpoEaseOut(float t, float b, float c, float d)
		{
			if (t != d)
			{
				return c * (-Mathf.Pow(2f, -10f * t / d) + 1f) + b;
			}
			return b + c;
		}

		public static float ExpoEaseIn(float t, float b, float c, float d)
		{
			if (t != 0f)
			{
				return c * Mathf.Pow(2f, 10f * (t / d - 1f)) + b;
			}
			return b;
		}

		public static float ExpoEaseInOut(float t, float b, float c, float d)
		{
			if (t == 0f)
			{
				return b;
			}
			if (t == d)
			{
				return b + c;
			}
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * Mathf.Pow(2f, 10f * (t - 1f)) + b;
			}
			return c / 2f * (-Mathf.Pow(2f, -10f * (t -= 1f)) + 2f) + b;
		}

		public static float ExpoEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.ExpoEaseOut(t * 2f, b, c / 2f, d);
			}
			return Easing.ExpoEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float CircEaseOut(float t, float b, float c, float d)
		{
			return c * Mathf.Sqrt(1f - (t = t / d - 1f) * t) + b;
		}

		public static float CircEaseIn(float t, float b, float c, float d)
		{
			return -c * (Mathf.Sqrt(1f - (t /= d) * t) - 1f) + b;
		}

		public static float CircEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return -c / 2f * (Mathf.Sqrt(1f - t * t) - 1f) + b;
			}
			return c / 2f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
		}

		public static float CircEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.CircEaseOut(t * 2f, b, c / 2f, d);
			}
			return Easing.CircEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float QuadEaseOut(float t, float b, float c, float d)
		{
			return -c * (t /= d) * (t - 2f) + b;
		}

		public static float QuadEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t + b;
		}

		public static float QuadEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t + b;
			}
			return -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
		}

		public static float QuadEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.QuadEaseOut(t * 2f, b, c / 2f, d);
			}
			return Easing.QuadEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float SineEaseOut(float t, float b, float c, float d)
		{
			return c * Mathf.Sin(t / d * 1.57079637f) + b;
		}

		public static float SineEaseIn(float t, float b, float c, float d)
		{
			return -c * Mathf.Cos(t / d * 1.57079637f) + c + b;
		}

		public static float SineEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * Mathf.Sin(3.14159274f * t / 2f) + b;
			}
			return -c / 2f * (Mathf.Cos(3.14159274f * (t -= 1f) / 2f) - 2f) + b;
		}

		public static float SineEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.SineEaseOut(t * 2f, b, c / 2f, d);
			}
			return Easing.SineEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float CubicEaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t + 1f) + b;
		}

		public static float CubicEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t + b;
		}

		public static float CubicEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t + 2f) + b;
		}

		public static float CubicEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.CubicEaseOut(t * 2f, b, c / 2f, d);
			}
			return Easing.CubicEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float QuartEaseOut(float t, float b, float c, float d)
		{
			return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
		}

		public static float QuartEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t + b;
		}

		public static float QuartEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t + b;
			}
			return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
		}

		public static float QuartEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.QuartEaseOut(t * 2f, b, c / 2f, d);
			}
			return Easing.QuartEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float QuintEaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
		}

		public static float QuintEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t * t + b;
		}

		public static float QuintEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
		}

		public static float QuintEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.QuintEaseOut(t * 2f, b, c / 2f, d);
			}
			return Easing.QuintEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float ElasticEaseOut(float t, float b, float c, float d)
		{
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			float num = d * 0.3f;
			float num2 = num / 4f;
			return c * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - num2) * 6.28318548f / num) + c + b;
		}

		public static float ElasticEaseIn(float t, float b, float c, float d)
		{
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			float num = d * 0.3f;
			float num2 = num / 4f;
			return -(c * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num)) + b;
		}

		public static float ElasticEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) == 2f)
			{
				return b + c;
			}
			float num = d * 0.450000018f;
			float num2 = num / 4f;
			if (t < 1f)
			{
				return -0.5f * (c * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num)) + b;
			}
			return c * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num) * 0.5f + c + b;
		}

		public static float ElasticEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.ElasticEaseOut(t * 2f, b, c / 2f, d);
			}
			return Easing.ElasticEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float BounceEaseOut(float t, float b, float c, float d)
		{
			if ((t /= d) < 0.363636374f)
			{
				return c * (7.5625f * t * t) + b;
			}
			if (t < 0.727272749f)
			{
				return c * (7.5625f * (t -= 0.545454562f) * t + 0.75f) + b;
			}
			if (t < 0.909090936f)
			{
				return c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
			}
			return c * (7.5625f * (t -= 0.954545438f) * t + 0.984375f) + b;
		}

		public static float BounceEaseIn(float t, float b, float c, float d)
		{
			return c - Easing.BounceEaseOut(d - t, 0f, c, d) + b;
		}

		public static float BounceEaseInOut(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.BounceEaseIn(t * 2f, 0f, c, d) * 0.5f + b;
			}
			return Easing.BounceEaseOut(t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
		}

		public static float BounceEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.BounceEaseOut(t * 2f, b, c / 2f, d);
			}
			return Easing.BounceEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float BackEaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * (2.70158f * t + 1.70158f) + 1f) + b;
		}

		public static float BackEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * (2.70158f * t - 1.70158f) + b;
		}

		public static float BackEaseInOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		public static float BackEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Easing.BackEaseOut(t * 2f, b, c / 2f, d);
			}
			return Easing.BackEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.AlwaysFinal(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.AlwaysStart(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.BackEaseIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.BackEaseInOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.BackEaseOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.BackEaseOutIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.BounceEaseIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.BounceEaseInOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.BounceEaseOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.BounceEaseOutIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.CircEaseIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.CircEaseInOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.CircEaseOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.CircEaseOutIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.CubicEaseIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.CubicEaseInOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.CubicEaseOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.CubicEaseOutIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.ElasticEaseIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.ElasticEaseInOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.ElasticEaseOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.ElasticEaseOutIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.ExpoEaseIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.ExpoEaseInOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.ExpoEaseOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.ExpoEaseOutIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.Linear(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuadEaseIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuadEaseInOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuadEaseOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuadEaseOutIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuartEaseIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuartEaseInOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuartEaseOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuartEaseOutIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuintEaseIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuintEaseInOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuintEaseOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.QuintEaseOutIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.SineEaseIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.SineEaseInOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.SineEaseOut(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Easing.SineEaseOutIn(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3)));
		}
	}
}
