using System;

namespace Net.RichardLord.Ash.Core
{
	internal class SystemList<T> where T : SystemBase<T>
	{
		internal T Head
		{
			get;
			set;
		}

		internal T Tail
		{
			get;
			set;
		}

		internal void Add(T system)
		{
			if (this.Head == null)
			{
				this.Tail = system;
				this.Head = system;
				SystemBase<T> arg_38_0 = system;
				SystemBase<T> arg_32_0 = system;
				T t = default(T);
				arg_32_0.Previous = t;
				arg_38_0.Next = t;
				return;
			}
			T t2 = this.Tail;
			while (t2 != null && t2.Priority > system.Priority)
			{
				t2 = t2.Previous;
			}
			if (t2 == this.Tail)
			{
				this.Tail.Next = system;
				system.Previous = this.Tail;
				system.Next = default(T);
				this.Tail = system;
				return;
			}
			if (t2 == null)
			{
				system.Next = this.Head;
				system.Previous = default(T);
				this.Head.Previous = system;
				this.Head = system;
				return;
			}
			system.Next = t2.Next;
			system.Previous = t2;
			t2.Next.Previous = system;
			t2.Next = system;
		}

		internal void Remove(T system)
		{
			if (this.Head == system)
			{
				this.Head = this.Head.Next;
			}
			if (this.Tail == system)
			{
				this.Tail = this.Tail.Previous;
			}
			if (system.Previous != null)
			{
				system.Previous.Next = system.Next;
			}
			if (system.Next != null)
			{
				system.Next.Previous = system.Previous;
			}
		}

		internal void RemoveAll()
		{
			while (this.Head != null)
			{
				T head = this.Head;
				this.Head = this.Head.Next;
				head.Previous = default(T);
				head.Next = default(T);
			}
			this.Tail = default(T);
		}

		internal T Get(Type type)
		{
			for (T t = this.Head; t != null; t = t.Next)
			{
				if (type.IsAssignableFrom(t.GetType()))
				{
					return t;
				}
			}
			return default(T);
		}
	}
}
