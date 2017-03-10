using StaRTS.Assets;
using StaRTS.Utils.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using WinRTBridge;

namespace StaRTS.Utils
{
	public static class UnityUtils
	{
		public delegate void OnUnityLogCallback(string logString, string stackTrace, LogType type);

		private const float TWO_ROW_ASPECT_RATIO_THRESHOLD = 1.4f;

		private const int QUAD_VERTICES = 4;

		private const int TRI_VERTICES = 6;

		private static List<Mesh> meshes;

		private static List<Material> materials;

		private static List<Texture2D> textures;

		private static Dictionary<string, UnityUtils.OnUnityLogCallback> unityLogCallbacks;

		public static void StaticReset()
		{
			if (UnityUtils.meshes != null)
			{
				int i = 0;
				int count = UnityUtils.meshes.Count;
				while (i < count)
				{
					Mesh x = UnityUtils.meshes[i];
					if (x != null)
					{
						UnityEngine.Object.Destroy(UnityUtils.meshes[i]);
					}
					i++;
				}
				UnityUtils.meshes = null;
			}
			if (UnityUtils.materials != null)
			{
				int j = 0;
				int count2 = UnityUtils.materials.Count;
				while (j < count2)
				{
					Material material = UnityUtils.materials[j];
					if (material != null)
					{
						UnityEngine.Object.Destroy(material);
					}
					j++;
				}
				UnityUtils.materials = null;
			}
			if (UnityUtils.textures != null)
			{
				int k = 0;
				int count3 = UnityUtils.textures.Count;
				while (k < count3)
				{
					Texture2D texture2D = UnityUtils.textures[k];
					if (texture2D != null)
					{
						UnityEngine.Object.Destroy(texture2D);
					}
					k++;
				}
				UnityUtils.textures = null;
			}
			if (UnityUtils.unityLogCallbacks != null)
			{
				Application.logMessageReceived -= new Application.LogCallback(UnityUtils.HandleUnityLog);
				UnityUtils.unityLogCallbacks = null;
			}
		}

		public static void RegisterLogCallback(string name, UnityUtils.OnUnityLogCallback callback)
		{
			if (callback == null)
			{
				if (UnityUtils.unityLogCallbacks != null && UnityUtils.unityLogCallbacks.ContainsKey(name))
				{
					UnityUtils.unityLogCallbacks.Remove(name);
					return;
				}
			}
			else
			{
				if (UnityUtils.unityLogCallbacks == null)
				{
					UnityUtils.unityLogCallbacks = new Dictionary<string, UnityUtils.OnUnityLogCallback>();
					Application.logMessageReceived += new Application.LogCallback(UnityUtils.HandleUnityLog);
				}
				if (UnityUtils.unityLogCallbacks.ContainsKey(name))
				{
					UnityUtils.unityLogCallbacks[name] = callback;
					return;
				}
				UnityUtils.unityLogCallbacks.Add(name, callback);
			}
		}

		private static void HandleUnityLog(string logString, string stackTrace, LogType type)
		{
			foreach (UnityUtils.OnUnityLogCallback current in UnityUtils.unityLogCallbacks.Values)
			{
				current(logString, stackTrace, type);
			}
		}

		public static bool IsWideScreen()
		{
			return (float)Screen.width / (float)Screen.height > 1.4f;
		}

		public static void SetupVerticesForQuads(int quadCount, out Vector3[] vertices, out Vector2[] uv, out int[] triangles)
		{
			vertices = new Vector3[quadCount * 4];
			triangles = new int[quadCount * 6];
			uv = new Vector2[quadCount * 4];
			int num = 0;
			int num2 = 0;
			while (quadCount > 0)
			{
				triangles[num2++] = num;
				triangles[num2++] = 2 + num;
				triangles[num2++] = 1 + num;
				triangles[num2++] = num;
				triangles[num2++] = 3 + num;
				triangles[num2++] = 2 + num;
				num++;
				uv[num++].x = 1f;
				uv[num].x = 1f;
				uv[num++].y = 1f;
				uv[num++].y = 1f;
				quadCount--;
			}
		}

		public static void SetQuadVertices(float minX, float minZ, float maxX, float maxZ, int quadIndex, Vector3[] vertices)
		{
			int num = quadIndex * 4;
			vertices[num].x = minX;
			vertices[num++].z = minZ;
			vertices[num].x = maxX;
			vertices[num++].z = minZ;
			vertices[num].x = maxX;
			vertices[num++].z = maxZ;
			vertices[num].x = minX;
			vertices[num++].z = maxZ;
		}

		public static Mesh CreateQuadMesh(float border)
		{
			Vector3[] array = new Vector3[4];
			Vector2[] array2 = new Vector2[4];
			int[] array3 = new int[6];
			array[0] = new Vector3(0f + border, 0f, 0f + border);
			array[1] = new Vector3(1f - border, 0f, 0f + border);
			array[2] = new Vector3(1f - border, 0f, 1f - border);
			array[3] = new Vector3(0f + border, 0f, 1f - border);
			array2[0] = new Vector2(0f, 0f);
			array2[1] = new Vector2(1f, 0f);
			array2[2] = new Vector2(1f, 1f);
			array2[3] = new Vector2(0f, 1f);
			array3[0] = 0;
			array3[1] = 2;
			array3[2] = 1;
			array3[3] = 0;
			array3[4] = 3;
			array3[5] = 2;
			return UnityUtils.CreateMeshWithVertices(array, array2, array3);
		}

		public static Mesh CreateMeshWithVertices(Vector3[] vertices, Vector2[] uv, int[] triangles)
		{
			Mesh mesh = UnityUtils.CreateMesh();
			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;
			mesh.RecalculateNormals();
			mesh.Optimize();
			return mesh;
		}

		public static Mesh CreateMesh()
		{
			Mesh mesh = new Mesh();
			if (UnityUtils.meshes == null)
			{
				UnityUtils.meshes = new List<Mesh>();
			}
			UnityUtils.meshes.Add(mesh);
			return mesh;
		}

		public static bool HasRendererMaterial(Renderer renderer)
		{
			return renderer != null && renderer.sharedMaterial != null;
		}

		public static Mesh EnsureMeshCopy(MeshFilter meshFilter)
		{
			Mesh mesh = meshFilter.mesh;
			if (UnityUtils.meshes == null)
			{
				UnityUtils.meshes = new List<Mesh>();
			}
			if (UnityUtils.meshes.IndexOf(mesh) < 0)
			{
				UnityUtils.meshes.Add(mesh);
			}
			return mesh;
		}

		public static void DestroyMesh(Mesh mesh)
		{
			if (mesh == null)
			{
				return;
			}
			if (UnityUtils.meshes != null)
			{
				UnityUtils.meshes.Remove(mesh);
			}
			UnityEngine.Object.Destroy(mesh);
		}

		public static Material CreateMaterial(Shader shader)
		{
			Material material = new Material(shader);
			if (UnityUtils.materials == null)
			{
				UnityUtils.materials = new List<Material>();
			}
			UnityUtils.materials.Add(material);
			return material;
		}

		public static Material EnsureMaterialCopy(Renderer renderer)
		{
			Material material = renderer.material;
			if (UnityUtils.materials == null)
			{
				UnityUtils.materials = new List<Material>();
			}
			if (UnityUtils.materials.IndexOf(material) < 0)
			{
				UnityUtils.materials.Add(material);
			}
			return material;
		}

		public static void DestroyMaterial(Material material)
		{
			if (material == null)
			{
				return;
			}
			if (UnityUtils.materials != null)
			{
				UnityUtils.materials.Remove(material);
			}
			UnityEngine.Object.Destroy(material);
		}

		public static Texture2D CreateTexture2D(int width, int height)
		{
			Texture2D texture2D = new Texture2D(width, height);
			if (UnityUtils.textures == null)
			{
				UnityUtils.textures = new List<Texture2D>();
			}
			UnityUtils.textures.Add(texture2D);
			return texture2D;
		}

		public static void DestroyTexture2D(Texture2D texture)
		{
			if (texture == null)
			{
				return;
			}
			if (UnityUtils.textures != null)
			{
				UnityUtils.textures.Remove(texture);
			}
			UnityEngine.Object.Destroy(texture);
		}

		public static Material CreateColorMaterial(Color32 color)
		{
			Material material = null;
			AssetManager assetManager = Service.Get<AssetManager>();
			if (assetManager != null)
			{
				GameShaders shaders = assetManager.Shaders;
				if (shaders != null)
				{
					Shader shader = shaders.GetShader("SimpleSolidColor");
					material = UnityUtils.CreateMaterial(shader);
				}
			}
			if (material == null)
			{
				throw new Exception("Unable to create color material");
			}
			material.SetColor("_Pigment", color);
			return material;
		}

		public static Material CreateBuildingColorMaterial()
		{
			Material material = null;
			AssetManager assetManager = Service.Get<AssetManager>();
			if (assetManager != null)
			{
				GameShaders shaders = assetManager.Shaders;
				if (shaders != null)
				{
					Shader shader = shaders.GetShader("Grid_Building_Color_PL");
					material = UnityUtils.CreateMaterial(shader);
				}
			}
			if (material == null)
			{
				throw new Exception("Unable to create color material");
			}
			return material;
		}

		public static void SetupMeshMaterial(GameObject gameObject, Mesh mesh, Material material)
		{
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = mesh;
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = material;
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
		}

		public static void SetCombineInstance(ref CombineInstance combineInstance, Mesh m, Matrix4x4 t)
		{
			combineInstance.mesh = m;
			combineInstance.transform = t;
		}

		public static Camera FindCamera(string cameraName)
		{
			Camera[] allCameras = Camera.allCameras;
			int i = 0;
			int num = allCameras.Length;
			while (i < num)
			{
				Camera camera = allCameras[i];
				if (camera.name == cameraName)
				{
					return camera;
				}
				i++;
			}
			return null;
		}

		public static GameObject FindGameObject(GameObject parent, string name)
		{
			if (parent.name == name)
			{
				return parent;
			}
			Transform transform = parent.transform;
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				GameObject gameObject = UnityUtils.FindGameObject(transform.GetChild(i).gameObject, name);
				if (gameObject != null)
				{
					return gameObject;
				}
				i++;
			}
			return null;
		}

		public static void RemoveColliders(GameObject gameObject)
		{
			Collider component = gameObject.GetComponent<Collider>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
			int i = 0;
			int childCount = gameObject.transform.childCount;
			while (i < childCount)
			{
				UnityUtils.RemoveColliders(gameObject.transform.GetChild(i).gameObject);
				i++;
			}
		}

		public static void EnableColliders(GameObject gameObject, bool enable)
		{
			Collider component = gameObject.GetComponent<Collider>();
			if (component != null)
			{
				component.enabled = enable;
			}
			int i = 0;
			int childCount = gameObject.transform.childCount;
			while (i < childCount)
			{
				UnityUtils.EnableColliders(gameObject.transform.GetChild(i).gameObject, enable);
				i++;
			}
		}

		public static void SetLayerRecursively(GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			Transform transform = gameObject.transform;
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				UnityUtils.SetLayerRecursively(transform.GetChild(i).gameObject, layer);
				i++;
			}
		}

		public static void EnsureScreenFillingComponent(GameObject gameObject, int depth, float uxCameraScale, bool bScreenSizeOverride = false, int w = 0, int h = 0)
		{
			UIStretch component = gameObject.GetComponent<UIStretch>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
			UIWidget uIWidget = gameObject.GetComponent<UIWidget>();
			if (uIWidget == null)
			{
				uIWidget = gameObject.AddComponent<UIWidget>();
			}
			uIWidget.depth = depth;
			uIWidget.autoResizeBoxCollider = true;
			if (bScreenSizeOverride)
			{
				uIWidget.width = (int)((float)w / uxCameraScale) + 5;
				uIWidget.height = (int)((float)h / uxCameraScale) + 5;
				return;
			}
			uIWidget.width = (int)((float)Screen.width / uxCameraScale) + 2;
			uIWidget.height = (int)((float)Screen.height / uxCameraScale) + 2;
		}

		public static void EnsureScreenWidthFillingComponent(GameObject gameObject, int depth, float uxCameraScale, bool bScreenSizeOverride = false, int w = 0)
		{
			UIStretch component = gameObject.GetComponent<UIStretch>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
			UIWidget uIWidget = gameObject.GetComponent<UIWidget>();
			if (uIWidget == null)
			{
				uIWidget = gameObject.AddComponent<UIWidget>();
			}
			uIWidget.depth = depth;
			uIWidget.autoResizeBoxCollider = true;
			if (bScreenSizeOverride)
			{
				uIWidget.width = (int)((float)w / uxCameraScale) + 5;
				return;
			}
			uIWidget.width = (int)((float)Screen.width / uxCameraScale) + 2;
		}

		public static bool ShouldIgnoreUIGameObjectInRaycast(GameObject uiObject)
		{
			UIWidget component = uiObject.GetComponent<UIWidget>();
			return component != null && !component.isVisible;
		}

		public static float GetRealTimeSinceStartUp()
		{
			return Time.realtimeSinceStartup;
		}

		public static Bounds GetGameObjectBounds(GameObject gameObject)
		{
			return UnityUtils.GetGameObjectBounds(gameObject, false);
		}

		public static Bounds GetGameObjectBounds(GameObject gameObject, bool ignoreScale)
		{
			float num = 1f;
			if (ignoreScale)
			{
				num = gameObject.transform.localScale.x;
				gameObject.transform.localScale = Vector3.one;
			}
			Bounds result = new Bounds(gameObject.transform.position, Vector3.zero);
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
			int i = 0;
			int num2 = componentsInChildren.Length;
			while (i < num2)
			{
				Renderer renderer = componentsInChildren[i];
				string name = renderer.name;
				if (!name.Contains("hadow") && !name.Contains("trail") && !(renderer is ParticleSystemRenderer))
				{
					result.Encapsulate(renderer.bounds);
				}
				i++;
			}
			if (ignoreScale)
			{
				gameObject.transform.localScale = new Vector3(num, num, num);
			}
			return result;
		}

		public static void ScaleParticleSize(ParticleSystem system, float originalSize, float scale)
		{
			if (system != null)
			{
				system.startSize = originalSize * scale;
			}
		}

		public static Bounds GetGameObjectLocalBounds(GameObject gameObject, bool ignoreScale)
		{
			float num = 1f;
			if (ignoreScale)
			{
				num = gameObject.transform.localScale.x;
				gameObject.transform.localScale = Vector3.one;
			}
			Bounds result = new Bounds(Vector3.zero, Vector3.zero);
			MeshFilter[] componentsInChildren = gameObject.GetComponentsInChildren<MeshFilter>();
			int i = 0;
			int num2 = componentsInChildren.Length;
			while (i < num2)
			{
				MeshFilter meshFilter = componentsInChildren[i];
				result.Encapsulate(meshFilter.sharedMesh.bounds);
				i++;
			}
			if (ignoreScale)
			{
				gameObject.transform.localScale = new Vector3(num, num, num);
			}
			return result;
		}

		public static Bounds GetRelativeGameObjectBounds(GameObject gameObject)
		{
			Bounds result = default(Bounds);
			bool flag = false;
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				if (!(componentsInChildren[i].bounds.extents == Vector3.zero))
				{
					if (!flag)
					{
						flag = true;
						result = new Bounds(componentsInChildren[i].transform.position, Vector3.zero);
					}
					if (flag)
					{
						result.Encapsulate(componentsInChildren[i].bounds);
					}
				}
				i++;
			}
			return result;
		}

		public static bool GetRayEllipsoidIntersection(Vector3 rayOrigin, Vector3 rayDir, Vector3 ellipseOrigin, Vector3 ellipseRadius, out Vector3 intersectionPoint)
		{
			Vector3 vector = UnityUtils.Divide(rayDir, ellipseRadius);
			Vector3 vector2 = UnityUtils.Divide(rayOrigin - ellipseOrigin, ellipseRadius);
			float num = Vector3.Dot(vector, vector);
			float num2 = 2f * Vector3.Dot(vector2, vector);
			float num3 = Vector3.Dot(vector2, vector2) - 1f;
			float num4 = num2 * num2 - 4f * num * num3;
			if (num4 < 0f)
			{
				intersectionPoint = Vector3.zero;
				return false;
			}
			float d = (-num2 - Mathf.Sqrt(num4)) / (2f * num);
			intersectionPoint = rayOrigin + d * rayDir;
			return true;
		}

		public static Vector3 Divide(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
		}

		public static GameObject[] GetChildren(GameObject gameObject)
		{
			Transform transform = gameObject.transform;
			GameObject[] array = new GameObject[transform.childCount];
			int num = 0;
			using (IEnumerator enumerator = transform.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform2 = (Transform)enumerator.get_Current();
					array[num] = transform2.gameObject;
					num++;
				}
			}
			return array;
		}

		public static GameObject CreateChildGameObject(string name, GameObject parentGameObject)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.layer = parentGameObject.layer;
			UnityUtils.ChangeGameObjectParent(gameObject, parentGameObject);
			return gameObject;
		}

		public static void ChangeGameObjectParent(GameObject gameObject, GameObject parentGameObject)
		{
			Transform transform = gameObject.transform;
			transform.parent = parentGameObject.transform;
			transform.localPosition = Vector3.zero;
			transform.localScale = Vector3.one;
			transform.localRotation = Quaternion.identity;
		}

		public static WrapMode GetAnimationWrapMode(Animation anim, AnimationState animationState)
		{
			if (animationState.wrapMode == WrapMode.Default)
			{
				return anim.wrapMode;
			}
			return animationState.wrapMode;
		}

		public static void PlayParticleEmitter(ParticleSystem particle)
		{
			if (particle.emission.rate.constantMax > 0f)
			{
				particle.Play(false);
				return;
			}
			particle.Emit(1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			UnityUtils.ChangeGameObjectParent((GameObject)GCHandledObjects.GCHandleToObject(*args), (GameObject)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.CreateBuildingColorMaterial());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.CreateChildGameObject(Marshal.PtrToStringUni(*(IntPtr*)args), (GameObject)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.CreateColorMaterial(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.CreateMaterial((Shader)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.CreateMesh());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.CreateMeshWithVertices((Vector3[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), (Vector2[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]), (int[])GCHandledObjects.GCHandleToPinnedArrayObject(args[2])));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.CreateQuadMesh(*(float*)args));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.CreateTexture2D(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			UnityUtils.DestroyMaterial((Material)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			UnityUtils.DestroyMesh((Mesh)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			UnityUtils.DestroyTexture2D((Texture2D)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.Divide(*(*(IntPtr*)args), *(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			UnityUtils.EnableColliders((GameObject)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.EnsureMaterialCopy((Renderer)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.EnsureMeshCopy((MeshFilter)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			UnityUtils.EnsureScreenFillingComponent((GameObject)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(float*)(args + 2), *(sbyte*)(args + 3) != 0, *(int*)(args + 4), *(int*)(args + 5));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			UnityUtils.EnsureScreenWidthFillingComponent((GameObject)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(float*)(args + 2), *(sbyte*)(args + 3) != 0, *(int*)(args + 4));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.FindCamera(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.FindGameObject((GameObject)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.GetAnimationWrapMode((Animation)GCHandledObjects.GCHandleToObject(*args), (AnimationState)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.GetChildren((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.GetGameObjectBounds((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.GetGameObjectBounds((GameObject)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.GetGameObjectLocalBounds((GameObject)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.GetRealTimeSinceStartUp());
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.GetRelativeGameObjectBounds((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			UnityUtils.HandleUnityLog(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (LogType)(*(int*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.HasRendererMaterial((Renderer)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.IsWideScreen());
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			UnityUtils.PlayParticleEmitter((ParticleSystem)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			UnityUtils.RegisterLogCallback(Marshal.PtrToStringUni(*(IntPtr*)args), (UnityUtils.OnUnityLogCallback)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			UnityUtils.RemoveColliders((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			UnityUtils.ScaleParticleSize((ParticleSystem)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(float*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			UnityUtils.SetLayerRecursively((GameObject)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			UnityUtils.SetQuadVertices(*(float*)args, *(float*)(args + 1), *(float*)(args + 2), *(float*)(args + 3), *(int*)(args + 4), (Vector3[])GCHandledObjects.GCHandleToPinnedArrayObject(args[5]));
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			UnityUtils.SetupMeshMaterial((GameObject)GCHandledObjects.GCHandleToObject(*args), (Mesh)GCHandledObjects.GCHandleToObject(args[1]), (Material)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(UnityUtils.ShouldIgnoreUIGameObjectInRaycast((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			UnityUtils.StaticReset();
			return -1L;
		}
	}
}
