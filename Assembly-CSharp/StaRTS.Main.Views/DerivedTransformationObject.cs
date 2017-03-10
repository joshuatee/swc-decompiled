using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views
{
	public class DerivedTransformationObject
	{
		public GameObject BaseTransformationObject
		{
			get;
			private set;
		}

		public Vector3 PositionOffset
		{
			get;
			private set;
		}

		public Quaternion RelativeRotation
		{
			get;
			private set;
		}

		public bool IsUpdateRotation
		{
			get;
			private set;
		}

		public bool AddRelativeRotation
		{
			get;
			private set;
		}

		private DerivedTransformationObject(GameObject baseTransformationObject, Vector3 positionOffset, bool isUpdateRotation, bool addRelativeRotation, Quaternion relativeRoation)
		{
			this.BaseTransformationObject = baseTransformationObject;
			this.PositionOffset = positionOffset;
			this.RelativeRotation = relativeRoation;
			this.IsUpdateRotation = isUpdateRotation;
			this.AddRelativeRotation = addRelativeRotation;
		}

		public DerivedTransformationObject(GameObject baseTransformationObject, Vector3 positionOffset, Quaternion relativeRoation) : this(baseTransformationObject, positionOffset, true, true, relativeRoation)
		{
		}

		public DerivedTransformationObject(GameObject baseTransformationObject, Vector3 positionOffset, bool isUpdateRotation) : this(baseTransformationObject, positionOffset, isUpdateRotation, false, Quaternion.identity)
		{
		}

		protected internal DerivedTransformationObject(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DerivedTransformationObject)GCHandledObjects.GCHandleToObject(instance)).AddRelativeRotation);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DerivedTransformationObject)GCHandledObjects.GCHandleToObject(instance)).BaseTransformationObject);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DerivedTransformationObject)GCHandledObjects.GCHandleToObject(instance)).IsUpdateRotation);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DerivedTransformationObject)GCHandledObjects.GCHandleToObject(instance)).PositionOffset);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DerivedTransformationObject)GCHandledObjects.GCHandleToObject(instance)).RelativeRotation);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DerivedTransformationObject)GCHandledObjects.GCHandleToObject(instance)).AddRelativeRotation = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DerivedTransformationObject)GCHandledObjects.GCHandleToObject(instance)).BaseTransformationObject = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((DerivedTransformationObject)GCHandledObjects.GCHandleToObject(instance)).IsUpdateRotation = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DerivedTransformationObject)GCHandledObjects.GCHandleToObject(instance)).PositionOffset = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((DerivedTransformationObject)GCHandledObjects.GCHandleToObject(instance)).RelativeRotation = *(*(IntPtr*)args);
			return -1L;
		}
	}
}
