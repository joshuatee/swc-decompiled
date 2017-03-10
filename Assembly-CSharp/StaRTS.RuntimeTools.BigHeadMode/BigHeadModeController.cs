using StaRTS.Audio;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.RuntimeTools.BigHeadMode
{
	public class BigHeadModeController : IEventObserver
	{
		private bool bigHeadModeActivated;

		private const string HEAD_SUFFIX = "bnd_head";

		private const string START_MSG = "BIG HEAD MODE ACTIVATED";

		private const string TOGGLE_ON_AUDIO = "sfx_stinger_victory_threestar";

		public BigHeadModeController()
		{
			this.bigHeadModeActivated = false;
			Service.Set<BigHeadModeController>(this);
			Service.Get<EventManager>().RegisterObserver(this, EventId.TroopViewReady, EventPriority.AfterDefault);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.TroopViewReady && this.bigHeadModeActivated)
			{
				EntityViewParams entityViewParams = (EntityViewParams)cookie;
				SmartEntity entity = entityViewParams.Entity;
				GameObjectViewComponent gameObjectViewComp = entity.GameObjectViewComp;
				GameObject gameObject = this.FindHead(gameObjectViewComp.MainGameObject);
				if (gameObject != null)
				{
					this.AddScaleWobbleComponent(gameObject);
				}
			}
			return EatResponse.NotEaten;
		}

		public void ToggleBigHeadMode()
		{
			this.bigHeadModeActivated = !this.bigHeadModeActivated;
			if (this.bigHeadModeActivated)
			{
				this.MakeHeadsBig();
				Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructionsWithColor("BIG HEAD MODE ACTIVATED", Color.red);
				Service.Get<AudioManager>().PlayAudio("sfx_stinger_victory_threestar");
				return;
			}
			this.Revert();
		}

		private void MakeHeadsBig()
		{
			GameObject[] array = this.FindAllHeads();
			if (array.Length != 0)
			{
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					this.AddScaleWobbleComponent(array[i]);
					i++;
				}
			}
		}

		private void Revert()
		{
			GameObject[] array = this.FindAllHeads();
			if (array.Length != 0)
			{
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					GameObject gameObject = array[i];
					if (gameObject.GetComponent<ScaleWobbleBehaviour>() != null)
					{
						UnityEngine.Object.Destroy(gameObject.GetComponent<ScaleWobbleBehaviour>());
					}
					i++;
				}
			}
		}

		private void AddScaleWobbleComponent(GameObject obj)
		{
			if (obj.GetComponent<ScaleWobbleBehaviour>() == null)
			{
				obj.AddComponent<ScaleWobbleBehaviour>();
			}
		}

		private GameObject FindHead(GameObject obj)
		{
			if (obj.GetComponent<Animator>())
			{
				Transform transform = this.FindChildEndsWith(obj.transform, "bnd_head");
				if (transform)
				{
					return transform.gameObject;
				}
			}
			return null;
		}

		private Transform FindChildEndsWith(Transform parent, string targetNameEnd)
		{
			if (parent.childCount > 0)
			{
				int i = 0;
				int childCount = parent.childCount;
				while (i < childCount)
				{
					Transform child = parent.GetChild(i);
					if (child.name.EndsWith(targetNameEnd))
					{
						return child;
					}
					Transform transform = this.FindChildEndsWith(child, targetNameEnd);
					if (transform)
					{
						return transform;
					}
					i++;
				}
			}
			return null;
		}

		private GameObject[] FindAllHeads()
		{
			GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
			List<GameObject> list = new List<GameObject>();
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				GameObject gameObject = this.FindHead(array[i]);
				if (gameObject != null)
				{
					list.Add(gameObject);
				}
				i++;
			}
			return list.ToArray();
		}

		protected internal BigHeadModeController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BigHeadModeController)GCHandledObjects.GCHandleToObject(instance)).AddScaleWobbleComponent((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BigHeadModeController)GCHandledObjects.GCHandleToObject(instance)).FindAllHeads());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BigHeadModeController)GCHandledObjects.GCHandleToObject(instance)).FindChildEndsWith((Transform)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BigHeadModeController)GCHandledObjects.GCHandleToObject(instance)).FindHead((GameObject)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((BigHeadModeController)GCHandledObjects.GCHandleToObject(instance)).MakeHeadsBig();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BigHeadModeController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((BigHeadModeController)GCHandledObjects.GCHandleToObject(instance)).Revert();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((BigHeadModeController)GCHandledObjects.GCHandleToObject(instance)).ToggleBigHeadMode();
			return -1L;
		}
	}
}
