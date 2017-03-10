using Net.RichardLord.Ash.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities
{
	public class EntityRef : MonoBehaviour, IUnitySerializable
	{
		public Entity Entity
		{
			get;
			set;
		}

		public EntityRef(Entity entity)
		{
			this.Entity = entity;
		}

		public EntityRef(IUnitySerializable self)
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

		protected internal EntityRef(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityRef)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EntityRef)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EntityRef)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EntityRef)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EntityRef)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EntityRef)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EntityRef)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
