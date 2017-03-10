using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXTextureComponent : MonoBehaviour, IUnitySerializable
	{
		public UITexture NGUITexture
		{
			get;
			set;
		}

		public UXTexture Texture
		{
			get;
			set;
		}

		public Texture MainTexture
		{
			get
			{
				if (this.NGUITexture != null)
				{
					return this.NGUITexture.mainTexture;
				}
				return null;
			}
			set
			{
				if (this.NGUITexture != null)
				{
					this.NGUITexture.mainTexture = value;
				}
			}
		}

		public UXTextureComponent()
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

		protected internal UXTextureComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).MainTexture);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).NGUITexture);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).Texture);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).MainTexture = (Texture)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).NGUITexture = (UITexture)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).Texture = (UXTexture)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXTextureComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
