using System;
using WinRTBridge;

namespace Net.RichardLord.Ash.Core
{
	public class EntityList
	{
		private Entity _head;

		private Entity _tail;

		public Entity Head
		{
			get
			{
				return this._head;
			}
		}

		public Entity Tail
		{
			get
			{
				return this._tail;
			}
		}

		public void Add(Entity entity)
		{
			if (this.Head == null)
			{
				this._tail = entity;
				this._head = entity;
				entity.Next = (entity.Previous = null);
				return;
			}
			this._tail.Next = entity;
			entity.Previous = this._tail;
			entity.Next = null;
			this._tail = entity;
		}

		public void Remove(Entity entity)
		{
			if (this._head == entity)
			{
				this._head = this._head.Next;
			}
			if (this._tail == entity)
			{
				this._tail = this._tail.Previous;
			}
			if (entity.Previous != null)
			{
				entity.Previous.Next = entity.Next;
			}
			if (entity.Next != null)
			{
				entity.Next.Previous = entity.Previous;
			}
		}

		public void RemoveAll()
		{
			while (this.Head != null)
			{
				Entity head = this.Head;
				this._head = this.Head.Next;
				head.Previous = null;
				head.Next = null;
			}
			this._tail = null;
		}

		public EntityList()
		{
		}

		protected internal EntityList(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EntityList)GCHandledObjects.GCHandleToObject(instance)).Add((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityList)GCHandledObjects.GCHandleToObject(instance)).Head);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityList)GCHandledObjects.GCHandleToObject(instance)).Tail);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EntityList)GCHandledObjects.GCHandleToObject(instance)).Remove((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EntityList)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}
	}
}
