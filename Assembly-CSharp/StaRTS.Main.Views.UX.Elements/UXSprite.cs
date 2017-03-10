using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.Projectors;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXSprite : UXElement
	{
		public const string CLEAR_SPRITE_NAME = "bkgClear";

		private UXSpriteComponent component;

		public string SpriteName
		{
			get
			{
				return this.component.SpriteName;
			}
			set
			{
				this.component.SpriteName = value;
			}
		}

		public UIAtlas Atlas
		{
			get
			{
				return this.component.Atlas;
			}
		}

		public Vector4 Border
		{
			get
			{
				return this.component.Border * this.uxCamera.Scale;
			}
		}

		public float Alpha
		{
			get
			{
				return this.component.Alpha;
			}
			set
			{
				this.component.Alpha = value;
			}
		}

		public Color Color
		{
			get
			{
				return this.component.Color;
			}
			set
			{
				this.component.Color = value;
			}
		}

		public float FillAmount
		{
			get
			{
				return this.component.FillAmount;
			}
			set
			{
				this.component.FillAmount = value;
			}
		}

		public UXSprite(UXCamera uxCamera, UXSpriteComponent component) : base(uxCamera, component.gameObject, null)
		{
			this.component = component;
		}

		public override void InternalDestroyComponent()
		{
			this.component.Sprite = null;
			UnityEngine.Object.Destroy(this.component);
		}

		public override void OnDestroyElement()
		{
			if (Service.IsSet<ProjectorManager>())
			{
				Service.Get<ProjectorManager>().DestroyProjector(this);
			}
		}

		public bool SetAtlasAndSprite(UIAtlas atlas, string name)
		{
			return this.component.SetAtlasAndSprite(atlas, name);
		}

		protected internal UXSprite(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSprite)GCHandledObjects.GCHandleToObject(instance)).Alpha);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSprite)GCHandledObjects.GCHandleToObject(instance)).Atlas);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSprite)GCHandledObjects.GCHandleToObject(instance)).Border);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSprite)GCHandledObjects.GCHandleToObject(instance)).Color);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSprite)GCHandledObjects.GCHandleToObject(instance)).FillAmount);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSprite)GCHandledObjects.GCHandleToObject(instance)).SpriteName);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXSprite)GCHandledObjects.GCHandleToObject(instance)).InternalDestroyComponent();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXSprite)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXSprite)GCHandledObjects.GCHandleToObject(instance)).Alpha = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXSprite)GCHandledObjects.GCHandleToObject(instance)).Color = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXSprite)GCHandledObjects.GCHandleToObject(instance)).FillAmount = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXSprite)GCHandledObjects.GCHandleToObject(instance)).SpriteName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSprite)GCHandledObjects.GCHandleToObject(instance)).SetAtlasAndSprite((UIAtlas)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}
	}
}
