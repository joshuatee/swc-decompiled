using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WinRTBridge;

namespace Net.RichardLord.Ash.Core
{
	public class Entity
	{
		[CompilerGenerated]
		[System.Serializable]
		private sealed class <>c
		{
			public static readonly Entity.<>c <>9 = new Entity.<>c();

			public static Func<PropertyInfo, bool> <>9__35_0;

			internal bool <Clone>b__35_0(PropertyInfo property)
			{
				return property.CanRead && property.CanWrite;
			}
		}

		private readonly Dictionary<Type, ComponentBase> _components;

		public uint ID;

		[method: CompilerGenerated]
		[CompilerGenerated]
		internal event Action<Entity, Type> ComponentAdded;

		[method: CompilerGenerated]
		[CompilerGenerated]
		internal event Action<Entity, Type> ComponentRemoved;

		internal Entity Previous
		{
			get;
			set;
		}

		internal Entity Next
		{
			get;
			set;
		}

		internal Dictionary<Type, ComponentBase> Components
		{
			get
			{
				return this._components;
			}
		}

		public Entity()
		{
			this._components = new Dictionary<Type, ComponentBase>();
		}

		public Entity Add(ComponentBase component)
		{
			this.AddComponentAndDispatchAddEvent(component, component.GetType());
			return this;
		}

		public Entity Add(ComponentBase component, Type componentClass)
		{
			if (!componentClass.IsInstanceOfType(component))
			{
				throw new InvalidOperationException("Component is not an instance of " + componentClass + " or its parent types.");
			}
			this.AddComponentAndDispatchAddEvent(component, componentClass);
			return this;
		}

		public Entity Add<T>(ComponentBase component) where T : ComponentBase
		{
			return this.Add(component, typeof(T));
		}

		public Entity AddOrReplace<T>(ComponentBase component) where T : ComponentBase
		{
			if (this.Has<T>())
			{
				this.Remove<T>();
			}
			return this.Add<T>(component);
		}

		protected virtual Entity AddComponentAndDispatchAddEvent(ComponentBase component, Type componentClass)
		{
			if (component == null)
			{
				throw new NullReferenceException("Component cannot be null.");
			}
			while (componentClass != null && componentClass != typeof(ComponentBase))
			{
				if (this._components.ContainsKey(componentClass))
				{
					this._components.Remove(componentClass);
				}
				this._components[componentClass] = component;
				component.Entity = this;
				if (this.ComponentAdded != null)
				{
					this.ComponentAdded.Invoke(this, componentClass);
				}
				componentClass = componentClass.GetBaseType();
			}
			return this;
		}

		public object Remove<T>()
		{
			return this.Remove(typeof(T));
		}

		public virtual object Remove(Type componentClass)
		{
			if (this._components.ContainsKey(componentClass))
			{
				ComponentBase componentBase = this._components[componentClass];
				componentBase.OnRemove();
				this._components.Remove(componentClass);
				componentBase.Entity = null;
				if (this.ComponentRemoved != null)
				{
					this.ComponentRemoved.Invoke(this, componentClass);
				}
				return componentBase;
			}
			return null;
		}

		public void RemoveAll()
		{
			Type[] array = this._components.Keys.ToArray<Type>();
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				this.Remove(array[i]);
				i++;
			}
		}

		public object Get(Type componentClass)
		{
			if (!this._components.ContainsKey(componentClass))
			{
				return null;
			}
			return this._components[componentClass];
		}

		public T Get<T>() where T : ComponentBase
		{
			Type typeFromHandle = typeof(T);
			if (!this._components.ContainsKey(typeFromHandle))
			{
				return default(T);
			}
			return (T)((object)this._components[typeFromHandle]);
		}

		public T GetOrCreate<T>() where T : ComponentBase, new()
		{
			T t = this.Get<T>();
			if (t == null)
			{
				t = Activator.CreateInstance<T>();
				this.Add<T>(t);
			}
			return t;
		}

		public T DeepGet<T>() where T : ComponentBase
		{
			Type typeFromHandle = typeof(T);
			if (this._components.ContainsKey(typeFromHandle))
			{
				return (T)((object)this._components[typeFromHandle]);
			}
			foreach (object current in this._components.Values)
			{
				if (current is T)
				{
					return (T)((object)current);
				}
			}
			return default(T);
		}

		public List<ComponentBase> GetAll()
		{
			return this._components.Values.ToList<ComponentBase>();
		}

		public bool Has(Type componentClass)
		{
			return this._components.ContainsKey(componentClass);
		}

		public bool Has<T>() where T : ComponentBase
		{
			Type typeFromHandle = typeof(T);
			return this._components.ContainsKey(typeFromHandle);
		}

		public bool DeepHas<T>() where T : ComponentBase
		{
			Type typeFromHandle = typeof(T);
			if (this._components.ContainsKey(typeFromHandle))
			{
				return true;
			}
			foreach (object current in this._components.Values)
			{
				if (current is T)
				{
					return true;
				}
			}
			return false;
		}

		public Entity Clone()
		{
			Entity entity = new Entity();
			foreach (KeyValuePair<Type, ComponentBase> current in this._components)
			{
				Type key = current.get_Key();
				ComponentBase componentBase = Activator.CreateInstance(key) as ComponentBase;
				IEnumerable<PropertyInfo> arg_59_0 = key.GetProperties();
				Func<PropertyInfo, bool> arg_59_1;
				if ((arg_59_1 = Entity.<>c.<>9__35_0) == null)
				{
					arg_59_1 = (Entity.<>c.<>9__35_0 = new Func<PropertyInfo, bool>(Entity.<>c.<>9.<Clone>b__35_0));
				}
				using (IEnumerator<PropertyInfo> enumerator2 = arg_59_0.Where(arg_59_1).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						PropertyInfo current2 = enumerator2.get_Current();
						current2.SetValue(componentBase, current2.GetValue(current.get_Value(), null), null);
					}
				}
				entity.Add(componentBase, current.get_Key());
			}
			return entity;
		}

		protected internal Entity(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).Add((ComponentBase)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).Add((ComponentBase)GCHandledObjects.GCHandleToObject(*args), (Type)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Entity)GCHandledObjects.GCHandleToObject(instance)).ComponentAdded += (Action<Entity, Type>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Entity)GCHandledObjects.GCHandleToObject(instance)).ComponentRemoved += (Action<Entity, Type>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).AddComponentAndDispatchAddEvent((ComponentBase)GCHandledObjects.GCHandleToObject(*args), (Type)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).Clone());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).Get((Type)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).Components);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).Next);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).Previous);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).GetAll());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).Has((Type)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Entity)GCHandledObjects.GCHandleToObject(instance)).Remove((Type)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((Entity)GCHandledObjects.GCHandleToObject(instance)).ComponentAdded -= (Action<Entity, Type>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((Entity)GCHandledObjects.GCHandleToObject(instance)).ComponentRemoved -= (Action<Entity, Type>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((Entity)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((Entity)GCHandledObjects.GCHandleToObject(instance)).Next = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((Entity)GCHandledObjects.GCHandleToObject(instance)).Previous = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
