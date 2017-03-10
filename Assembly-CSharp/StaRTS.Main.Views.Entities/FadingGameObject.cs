using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class FadingGameObject : AbstractFadingView
	{
		private GameObject gameObject;

		public GameObject GameObj
		{
			get
			{
				return this.gameObject;
			}
		}

		public FadingGameObject(GameObject gameObj, float delay, float fadeTime, FadingDelegate onStart, FadingDelegate onComplete) : base(gameObj, delay, fadeTime, onStart, onComplete)
		{
			this.gameObject = gameObj;
			base.InitData(this.gameObject, null);
		}

		protected internal FadingGameObject(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FadingGameObject)GCHandledObjects.GCHandleToObject(instance)).GameObj);
		}
	}
}
