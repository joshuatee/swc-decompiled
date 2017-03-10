using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils
{
	public static class CameraUtils
	{
		private const float DRAG_THRESHOLD_SCREEN_WIDTH_PERCENTAGE = 0.01f;

		public static bool HasDragged(Vector2 screenPosition, Vector2 lastScreenPosition)
		{
			float num = (float)Screen.width * 0.01f;
			num *= num;
			float num2 = lastScreenPosition.x - screenPosition.x;
			float num3 = lastScreenPosition.y - screenPosition.y;
			float num4 = num2 * num2 + num3 * num3;
			return num4 >= num;
		}

		public static float CalculateDistanceFromEyeToScreen(Camera unityCamera)
		{
			return (float)Screen.height * 0.5f / Mathf.Tan(0.5f * unityCamera.fieldOfView * 3.14159274f / 180f);
		}

		public static bool GetGroundPositionHelper(Camera unityCamera, Vector3 screenPosition, Vector3 rayOrigin, float distanceFromEyeToScreen, float groundOffset, ref Vector3 groundPosition)
		{
			Vector3 vector;
			CameraUtils.ScreenToRay(unityCamera, screenPosition, distanceFromEyeToScreen, out vector);
			if (vector.y < 0f)
			{
				float d = (groundOffset - rayOrigin.y) / vector.y;
				groundPosition = rayOrigin + vector * d;
				groundPosition.y = 0f;
				return true;
			}
			return false;
		}

		private static void ScreenToRay(Camera unityCamera, Vector2 screenPosition, float distanceFromEyeToScreen, out Vector3 outRayDirection)
		{
			Vector4 vector = new Vector4(screenPosition.x - 0.5f * (float)Screen.width, screenPosition.y - 0.5f * (float)Screen.height, -distanceFromEyeToScreen, 0f);
			Matrix4x4 cameraToWorldMatrix = unityCamera.cameraToWorldMatrix;
			vector = cameraToWorldMatrix * vector;
			outRayDirection = new Vector3(vector.x, vector.y, vector.z);
			outRayDirection = outRayDirection.normalized;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CameraUtils.CalculateDistanceFromEyeToScreen((Camera)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CameraUtils.HasDragged(*(*(IntPtr*)args), *(*(IntPtr*)(args + 1))));
		}
	}
}
