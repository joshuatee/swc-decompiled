using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class OutlinedAsset : ShaderSwappedAsset
	{
		private const string PARAM_COLOR = "_OutlineColor";

		private const string PARAM_WIDTH = "_Outline";

		public void Init(GameObject gameObj)
		{
			base.Init(gameObj, "Outline_Unlit");
		}

		public void SetOutlineColor(Color color)
		{
			if (base.EnsureMaterials())
			{
				int i = 0;
				int count = this.shaderSwappedMaterials.Count;
				while (i < count)
				{
					this.SetColor(i, color);
					i++;
				}
			}
		}

		public void RemoveOutline()
		{
			base.RemoveAppliedShader();
		}

		private void SetColor(int i, Color color)
		{
			this.shaderSwappedMaterials[i].SetColor("_OutlineColor", color);
		}

		public void SetOutlineWidth(float width)
		{
			if (this.shaderSwappedMaterials == null)
			{
				return;
			}
			int i = 0;
			int count = this.shaderSwappedMaterials.Count;
			while (i < count)
			{
				this.shaderSwappedMaterials[i].SetFloat("_Outline", width);
				i++;
			}
		}

		public OutlinedAsset()
		{
		}

		protected internal OutlinedAsset(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((OutlinedAsset)GCHandledObjects.GCHandleToObject(instance)).Init((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((OutlinedAsset)GCHandledObjects.GCHandleToObject(instance)).RemoveOutline();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((OutlinedAsset)GCHandledObjects.GCHandleToObject(instance)).SetColor(*(int*)args, *(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((OutlinedAsset)GCHandledObjects.GCHandleToObject(instance)).SetOutlineColor(*(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((OutlinedAsset)GCHandledObjects.GCHandleToObject(instance)).SetOutlineWidth(*(float*)args);
			return -1L;
		}
	}
}
