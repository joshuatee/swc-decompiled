using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Net.RichardLord.Ash.Core
{
	public class Entity
	{
		private readonly Dictionary<Type, ComponentBase> _components;

		public uint ID;

		internal event Action<Entity, Type> ComponentAdded
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.ComponentAdded = (Action<Entity, Type>)Delegate.Combine(this.ComponentAdded, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.ComponentAdded = (Action<Entity, Type>)Delegate.Remove(this.ComponentAdded, value);
			}
		}

		internal event Action<Entity, Type> ComponentRemoved
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.ComponentRemoved = (Action<Entity, Type>)Delegate.Combine(this.ComponentRemoved, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.ComponentRemoved = (Action<Entity, Type>)Delegate.Remove(this.ComponentRemoved, value);
			}
		}

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
			while (componentClass != null)
			{
				if (componentClass == typeof(ComponentBase))
				{
					break;
				}
				if (this._components.ContainsKey(componentClass))
				{
					this._components.Remove(componentClass);
				}
				this._components[componentClass] = component;
				component.Entity = this;
				if (this.ComponentAdded != null)
				{
					this.ComponentAdded(this, componentClass);
				}
				componentClass = componentClass.BaseType;
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
					this.ComponentRemoved(this, componentClass);
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
			return (!this._components.ContainsKey(componentClass)) ? null : this._components[componentClass];
		}

		public T Get<T>() where T : ComponentBase
		{
			Type typeFromHandle = typeof(T);
			return (!this._components.ContainsKey(typeFromHandle)) ? ((T)((object)null)) : ((T)((object)this._components[typeFromHandle]));
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
			return (T)((object)null);
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
				Type key = current.Key;
				ComponentBase componentBase = Activator.CreateInstance(key) as ComponentBase;
				foreach (PropertyInfo current2 in from property in key.GetProperties()
				where property.CanRead && property.CanWrite
				select property)
				{
					current2.SetValue(componentBase, current2.GetValue(current.Value, null), null);
				}
				entity.Add(componentBase, current.Key);
			}
			return entity;
		}
	}
}
