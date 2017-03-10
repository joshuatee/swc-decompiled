using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UX.Elements;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Anchors
{
	public class UXAnchor : UXElement
	{
		public UXAnchor(UXCamera uxCamera) : base(uxCamera, null, null)
		{
		}

		protected void Init(GameObject root, UIAnchor.Side side)
		{
			this.root = root;
			if (root.GetComponent<UIAnchor>() != null)
			{
				throw new Exception("UXAnchor must not have a UIAnchor added already");
			}
			UIAnchor uIAnchor = root.AddComponent<UIAnchor>();
			uIAnchor.side = side;
			this.uxCamera.AddNewAnchor(root);
		}

		protected internal UXAnchor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UXAnchor)GCHandledObjects.GCHandleToObject(instance)).Init((GameObject)GCHandledObjects.GCHandleToObject(*args), (UIAnchor.Side)(*(int*)(args + 1)));
			return -1L;
		}
	}
}
