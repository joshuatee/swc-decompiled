using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXSpriteComponent : MonoBehaviour, IUnitySerializable
	{
		private string origSpriteName;

		public UISprite NGUISprite
		{
			get;
			set;
		}

		public UXSprite Sprite
		{
			get;
			set;
		}

		public string SpriteName
		{
			get
			{
				if (!(this.NGUISprite == null))
				{
					return this.NGUISprite.spriteName;
				}
				return null;
			}
			set
			{
				if (this.NGUISprite != null)
				{
					if (this.origSpriteName == null)
					{
						this.origSpriteName = this.NGUISprite.spriteName;
					}
					this.NGUISprite.spriteName = value;
					if (!this.NGUISprite.isValid)
					{
						this.NGUISprite.spriteName = this.origSpriteName;
					}
				}
			}
		}

		public UIAtlas Atlas
		{
			get
			{
				if (!(this.NGUISprite == null))
				{
					return this.NGUISprite.atlas;
				}
				return null;
			}
		}

		public Vector4 Border
		{
			get
			{
				if (!(this.NGUISprite == null))
				{
					return this.NGUISprite.border;
				}
				return Vector4.zero;
			}
		}

		public float FillAmount
		{
			get
			{
				if (!(this.NGUISprite == null))
				{
					return this.NGUISprite.fillAmount;
				}
				return 0f;
			}
			set
			{
				if (this.NGUISprite != null)
				{
					this.NGUISprite.fillAmount = value;
				}
			}
		}

		public float Alpha
		{
			get
			{
				if (!(this.NGUISprite == null))
				{
					return this.NGUISprite.alpha;
				}
				return 0f;
			}
			set
			{
				if (this.NGUISprite != null)
				{
					this.NGUISprite.alpha = value;
				}
			}
		}

		public Color Color
		{
			get
			{
				if (!(this.NGUISprite == null))
				{
					return this.NGUISprite.color;
				}
				return Color.white;
			}
			set
			{
				if (this.NGUISprite != null)
				{
					this.NGUISprite.color = value;
				}
			}
		}

		public bool SetAtlasAndSprite(UIAtlas atlas, string name)
		{
			if (atlas.GetSprite(name) == null)
			{
				return false;
			}
			this.NGUISprite.atlas = atlas;
			this.NGUISprite.spriteName = name;
			return true;
		}

		public UXSpriteComponent()
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

		protected internal UXSpriteComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Alpha);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Atlas);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Border);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Color);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).FillAmount);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).NGUISprite);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Sprite);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).SpriteName);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Alpha = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Color = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).FillAmount = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).NGUISprite = (UISprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Sprite = (UXSprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).SpriteName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).SetAtlasAndSprite((UIAtlas)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((UXSpriteComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
