using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour, IUnitySerializable
{
	private static bool mRayDebug = false;

	private static List<string> mLines = new List<string>();

	private static NGUIDebug mInstance = null;

	public static bool debugRaycast
	{
		get
		{
			return NGUIDebug.mRayDebug;
		}
		set
		{
			NGUIDebug.mRayDebug = value;
			if (value && Application.isPlaying)
			{
				NGUIDebug.CreateInstance();
			}
		}
	}

	public static void CreateInstance()
	{
		if (NGUIDebug.mInstance == null)
		{
			GameObject gameObject = new GameObject("_NGUI Debug");
			NGUIDebug.mInstance = gameObject.AddComponent<NGUIDebug>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	private static void LogString(string text)
	{
		if (Application.isPlaying)
		{
			if (NGUIDebug.mLines.Count > 20)
			{
				NGUIDebug.mLines.RemoveAt(0);
			}
			NGUIDebug.mLines.Add(text);
			NGUIDebug.CreateInstance();
			return;
		}
		Debug.Log(text);
	}

	public static void Log(params object[] objs)
	{
		string text = "";
		for (int i = 0; i < objs.Length; i++)
		{
			if (i == 0)
			{
				text += objs[i].ToString();
			}
			else
			{
				text = text + ", " + objs[i].ToString();
			}
		}
		NGUIDebug.LogString(text);
	}

	public static void Clear()
	{
		NGUIDebug.mLines.Clear();
	}

	public static void DrawBounds(Bounds b)
	{
		Vector3 center = b.center;
		Vector3 vector = b.center - b.extents;
		Vector3 vector2 = b.center + b.extents;
		Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector2.x, vector.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector.x, vector2.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector2.x, vector.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector.x, vector2.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.red);
	}

	private void OnGUI()
	{
		Rect position = new Rect(5f, 5f, 1000f, 22f);
		if (NGUIDebug.mRayDebug)
		{
			UICamera.ControlScheme currentScheme = UICamera.currentScheme;
			string text = "Scheme: " + currentScheme;
			GUI.color = Color.black;
			GUI.Label(position, text);
			position.y -= 1f;
			position.x -= 1f;
			GUI.color = Color.white;
			GUI.Label(position, text);
			position.y += 18f;
			position.x += 1f;
			text = "Hover: " + NGUITools.GetHierarchy(UICamera.hoveredObject).Replace("\"", "");
			GUI.color = Color.black;
			GUI.Label(position, text);
			position.y -= 1f;
			position.x -= 1f;
			GUI.color = Color.white;
			GUI.Label(position, text);
			position.y += 18f;
			position.x += 1f;
			text = "Selection: " + NGUITools.GetHierarchy(UICamera.selectedObject).Replace("\"", "");
			GUI.color = Color.black;
			GUI.Label(position, text);
			position.y -= 1f;
			position.x -= 1f;
			GUI.color = Color.white;
			GUI.Label(position, text);
			position.y += 18f;
			position.x += 1f;
			text = "Controller: " + NGUITools.GetHierarchy(UICamera.controllerNavigationObject).Replace("\"", "");
			GUI.color = Color.black;
			GUI.Label(position, text);
			position.y -= 1f;
			position.x -= 1f;
			GUI.color = Color.white;
			GUI.Label(position, text);
			position.y += 18f;
			position.x += 1f;
			text = "Active events: " + UICamera.CountInputSources();
			if (UICamera.disableController)
			{
				text += ", disabled controller";
			}
			if (UICamera.inputHasFocus)
			{
				text += ", input focus";
			}
			GUI.color = Color.black;
			GUI.Label(position, text);
			position.y -= 1f;
			position.x -= 1f;
			GUI.color = Color.white;
			GUI.Label(position, text);
			position.y += 18f;
			position.x += 1f;
		}
		int i = 0;
		int count = NGUIDebug.mLines.Count;
		while (i < count)
		{
			GUI.color = Color.black;
			GUI.Label(position, NGUIDebug.mLines[i]);
			position.y -= 1f;
			position.x -= 1f;
			GUI.color = Color.white;
			GUI.Label(position, NGUIDebug.mLines[i]);
			position.y += 18f;
			position.x += 1f;
			i++;
		}
	}

	public NGUIDebug()
	{
	}

	public override void Unity_Serialize(int depth)
	{
	}

	public override void Unity_Deserialize(int depth)
	{
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public override void Unity_NamedSerialize(int depth)
	{
	}

	public override void Unity_NamedDeserialize(int depth)
	{
	}

	protected internal NGUIDebug(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		NGUIDebug.Clear();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		NGUIDebug.CreateInstance();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		NGUIDebug.DrawBounds(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(NGUIDebug.debugRaycast);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		NGUIDebug.Log((object[])GCHandledObjects.GCHandleToPinnedArrayObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		NGUIDebug.LogString(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((NGUIDebug)GCHandledObjects.GCHandleToObject(instance)).OnGUI();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		NGUIDebug.debugRaycast = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((NGUIDebug)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((NGUIDebug)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((NGUIDebug)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((NGUIDebug)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((NGUIDebug)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
