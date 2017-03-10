using StaRTS.Assets;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Cameras
{
	public class QuadCamera : CameraBase
	{
		private const string CAMERA_FORMAT = "{0} Camera";

		private const string OBJECT_FORMAT = "{0} Quad";

		private const float FAR_CLIP = 1f;

		private GameObject quadGameObject;

		protected Material quadMaterial;

		private Mesh quadMesh;

		protected List<Camera> srcCameras;

		protected List<Camera> dstCameras;

		protected RenderTexture srcRenderTexture;

		protected RenderTexture dstRenderTexture;

		private string name;

		protected QuadCamera(string name, Vector3 position, int layer, int depth)
		{
			this.name = name;
			this.srcCameras = new List<Camera>();
			this.dstCameras = new List<Camera>();
			GameObject gameObject = new GameObject(string.Format("{0} Camera", new object[]
			{
				name
			}));
			gameObject.layer = layer;
			Transform transform = gameObject.transform;
			transform.position = position;
			transform.rotation = Quaternion.AngleAxis(90f, Vector3.right);
			this.unityCamera = gameObject.AddComponent<Camera>();
			this.unityCamera.enabled = false;
			this.unityCamera.orthographic = true;
			this.unityCamera.orthographicSize = (float)(Screen.height / 2);
			this.unityCamera.clearFlags = CameraClearFlags.Color;
			this.unityCamera.backgroundColor = Color.black;
			this.unityCamera.nearClipPlane = -1f;
			this.unityCamera.farClipPlane = 1f;
			this.unityCamera.cullingMask = 1 << layer;
			this.unityCamera.depth = (float)depth;
			this.unityCamera.useOcclusionCulling = false;
			this.unityCamera.eventMask = 0;
			this.unityCamera.renderingPath = RenderingPath.VertexLit;
		}

		public bool IsRendering()
		{
			return this.unityCamera.enabled;
		}

		protected RenderTexture PrepareCameras(List<Camera> cameras)
		{
			RenderTexture renderTexture = Service.Get<CameraManager>().GetRenderTexture(Screen.width, Screen.height);
			int i = 0;
			int count = cameras.Count;
			while (i < count)
			{
				Camera camera = cameras[i];
				if (camera != null)
				{
					camera.enabled = false;
					camera.targetTexture = renderTexture;
					camera.enabled = true;
				}
				i++;
			}
			return renderTexture;
		}

		protected void RestoreCameras(List<Camera> cameras, bool enable, bool destroy)
		{
			int i = 0;
			int count = cameras.Count;
			while (i < count)
			{
				Camera camera = cameras[i];
				if (!(camera == null))
				{
					camera.enabled = false;
					camera.targetTexture = null;
					camera.enabled = enable;
					if (destroy)
					{
						UnityEngine.Object.Destroy(camera.gameObject);
					}
				}
				i++;
			}
		}

		protected void CreateMaterial(string shaderName)
		{
			Shader shader = Service.Get<AssetManager>().Shaders.GetShader(shaderName);
			this.quadMaterial = UnityUtils.CreateMaterial(shader);
		}

		protected void StartRendering(bool needQuad)
		{
			if (needQuad)
			{
				this.quadGameObject = new GameObject(string.Format("{0} Quad", new object[]
				{
					this.name
				}));
				this.quadGameObject.transform.position = this.unityCamera.gameObject.transform.position - new Vector3((float)Screen.width * 0.5f, 0.5f, (float)Screen.height * 0.5f);
				this.quadGameObject.transform.localScale = new Vector3((float)Screen.width, 0f, (float)Screen.height);
				this.quadMesh = UnityUtils.CreateQuadMesh(0f);
				UnityUtils.SetupMeshMaterial(this.quadGameObject, this.quadMesh, this.quadMaterial);
				UnityUtils.SetLayerRecursively(this.quadGameObject, this.unityCamera.gameObject.layer);
			}
			else
			{
				this.quadGameObject = null;
			}
			this.unityCamera.enabled = true;
		}

		protected void DestroyRenderObjects()
		{
			this.unityCamera.enabled = false;
			if (this.srcRenderTexture != null)
			{
				Service.Get<CameraManager>().ReleaseRenderTexture(this.srcRenderTexture, false);
				this.srcRenderTexture = null;
			}
			if (this.dstRenderTexture != null)
			{
				Service.Get<CameraManager>().ReleaseRenderTexture(this.dstRenderTexture, false);
				this.dstRenderTexture = null;
			}
			if (this.quadMesh != null)
			{
				UnityUtils.DestroyMesh(this.quadMesh);
				this.quadMesh = null;
			}
			if (this.quadMaterial != null)
			{
				UnityUtils.DestroyMaterial(this.quadMaterial);
				this.quadMaterial = null;
			}
			if (this.quadGameObject != null)
			{
				UnityEngine.Object.Destroy(this.quadGameObject);
				this.quadGameObject = null;
				this.quadMaterial = null;
			}
		}

		protected internal QuadCamera(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((QuadCamera)GCHandledObjects.GCHandleToObject(instance)).CreateMaterial(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((QuadCamera)GCHandledObjects.GCHandleToObject(instance)).DestroyRenderObjects();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuadCamera)GCHandledObjects.GCHandleToObject(instance)).IsRendering());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuadCamera)GCHandledObjects.GCHandleToObject(instance)).PrepareCameras((List<Camera>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((QuadCamera)GCHandledObjects.GCHandleToObject(instance)).RestoreCameras((List<Camera>)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((QuadCamera)GCHandledObjects.GCHandleToObject(instance)).StartRendering(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
