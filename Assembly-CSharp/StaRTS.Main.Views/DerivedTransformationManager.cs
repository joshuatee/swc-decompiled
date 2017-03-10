using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views
{
	public class DerivedTransformationManager : IViewFrameTimeObserver
	{
		private Dictionary<GameObject, DerivedTransformationObject> objectMap;

		private bool alreadyLoggedGameObjectNullError;

		public DerivedTransformationManager()
		{
			Service.Set<DerivedTransformationManager>(this);
			this.objectMap = new Dictionary<GameObject, DerivedTransformationObject>();
		}

		public void AddDerivedTransformation(GameObject gameObject, DerivedTransformationObject derivedTransformationObject)
		{
			if (gameObject != null && derivedTransformationObject.BaseTransformationObject != null)
			{
				this.objectMap.Add(gameObject, derivedTransformationObject);
				if (this.objectMap.Count == 1)
				{
					Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
				}
			}
		}

		public void RemoveDerivedTransformation(GameObject gameObject)
		{
			if (gameObject != null)
			{
				this.objectMap.Remove(gameObject);
				if (this.objectMap.Count == 0)
				{
					Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
					return;
				}
			}
			else
			{
				Service.Get<StaRTSLogger>().Warn("Null object is being passed to RemoveDerivedTransformation");
			}
		}

		public void OnViewFrameTime(float dt)
		{
			foreach (KeyValuePair<GameObject, DerivedTransformationObject> current in this.objectMap)
			{
				GameObject key = current.get_Key();
				DerivedTransformationObject value = current.get_Value();
				if (key == null)
				{
					if (!this.alreadyLoggedGameObjectNullError)
					{
						string text = "destroyed";
						if (value.BaseTransformationObject != null)
						{
							text = value.BaseTransformationObject.name;
						}
						Service.Get<StaRTSLogger>().ErrorFormat("GameObject being added to DerivedTransformationManager is being destroyed. Associated base object is {0}.", new object[]
						{
							text
						});
						this.alreadyLoggedGameObjectNullError = true;
					}
				}
				else if (!(value.BaseTransformationObject == null))
				{
					Transform transform = key.transform;
					transform.position = value.BaseTransformationObject.transform.position + value.PositionOffset;
					if (value.IsUpdateRotation)
					{
						if (value.AddRelativeRotation)
						{
							transform.rotation = GameUtils.ApplyRelativeRotation(value.RelativeRotation, value.BaseTransformationObject.transform.rotation);
						}
						else
						{
							transform.rotation = value.BaseTransformationObject.transform.rotation;
						}
					}
				}
			}
		}

		protected internal DerivedTransformationManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DerivedTransformationManager)GCHandledObjects.GCHandleToObject(instance)).AddDerivedTransformation((GameObject)GCHandledObjects.GCHandleToObject(*args), (DerivedTransformationObject)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DerivedTransformationManager)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DerivedTransformationManager)GCHandledObjects.GCHandleToObject(instance)).RemoveDerivedTransformation((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
