using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TrapViewComponent : ComponentBase
	{
		public Animator Anim
		{
			get;
			private set;
		}

		public Animator TurretAnim
		{
			get;
			set;
		}

		public List<TrapState> PendingStates
		{
			get;
			private set;
		}

		public GameObject Contents
		{
			get
			{
				return this.Anim.transform.GetChild(0).gameObject;
			}
		}

		public TrapViewComponent(Animator anim)
		{
			this.Anim = anim;
			this.PendingStates = new List<TrapState>();
		}

		protected internal TrapViewComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapViewComponent)GCHandledObjects.GCHandleToObject(instance)).Anim);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapViewComponent)GCHandledObjects.GCHandleToObject(instance)).Contents);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapViewComponent)GCHandledObjects.GCHandleToObject(instance)).PendingStates);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrapViewComponent)GCHandledObjects.GCHandleToObject(instance)).TurretAnim);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TrapViewComponent)GCHandledObjects.GCHandleToObject(instance)).Anim = (Animator)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TrapViewComponent)GCHandledObjects.GCHandleToObject(instance)).PendingStates = (List<TrapState>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TrapViewComponent)GCHandledObjects.GCHandleToObject(instance)).TurretAnim = (Animator)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
