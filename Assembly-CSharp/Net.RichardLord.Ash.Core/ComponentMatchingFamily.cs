using Net.RichardLord.Ash.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Net.RichardLord.Ash.Core
{
	public class ComponentMatchingFamily<TNode> : IFamily where TNode : Node<TNode>, new()
	{
		private static readonly string[] reservedProperties = new string[]
		{
			"Entity",
			"Entities",
			"Previous",
			"Next"
		};

		private Dictionary<Entity, TNode> entities;

		private Dictionary<Type, string> components;

		private NodePool<TNode> nodePool;

		private IGame game;

		public void Setup(IGame game)
		{
			this.game = game;
			this.Init();
		}

		private void Init()
		{
			this.nodePool = new NodePool<TNode>();
			this.entities = new Dictionary<Entity, TNode>();
			this.components = new Dictionary<Type, string>();
			PropertyInfo[] properties = typeof(TNode).GetProperties((global::BindingFlags)5);
			int i = 0;
			int num = properties.Length;
			while (i < num)
			{
				PropertyInfo propertyInfo = properties[i];
				if (Array.IndexOf<string>(ComponentMatchingFamily<TNode>.reservedProperties, propertyInfo.Name) == -1 && !this.components.ContainsValue(propertyInfo.Name))
				{
					this.components.Add(propertyInfo.PropertyType, propertyInfo.Name);
				}
				i++;
			}
		}

		public void NewEntity(Entity entity)
		{
			this.AddIfMatch(entity);
		}

		public void ComponentAddedToEntity(Entity entity, Type componentClass)
		{
			this.AddIfMatch(entity);
		}

		public void ComponentRemovedFromEntity(Entity entity, Type componentClass)
		{
			if (this.components.ContainsKey(componentClass))
			{
				this.RemoveIfMatch(entity);
			}
		}

		public void RemoveEntity(Entity entity)
		{
			this.RemoveIfMatch(entity);
		}

		private void AddIfMatch(Entity entity)
		{
			if (!this.entities.ContainsKey(entity))
			{
				if (this.components.Keys.Any((Type componentClass) => !entity.Has(componentClass)))
				{
					return;
				}
				TNode tNode = this.nodePool.Get();
				tNode.Entity = entity;
				foreach (Type current in this.components.Keys)
				{
					tNode.SetProperty(this.components[current], entity.Get(current));
				}
				this.entities.Add(entity, tNode);
				NodeListSingleton<TNode>.NodeList.Add(tNode);
			}
		}

		private void RemoveIfMatch(Entity entity)
		{
			if (this.entities.ContainsKey(entity))
			{
				TNode node = this.entities[entity];
				this.entities.Remove(entity);
				NodeListSingleton<TNode>.NodeList.Remove(node);
				if (this.game.Updating)
				{
					this.nodePool.Cache(node);
					this.game.UpdateSimComplete += new Action(this.ReleaseNodePoolCache);
					this.game.UpdateViewComplete += new Action(this.ReleaseNodePoolCache);
					return;
				}
				this.nodePool.Dispose(node);
			}
		}

		private void ReleaseNodePoolCache()
		{
			this.game.UpdateSimComplete -= new Action(this.ReleaseNodePoolCache);
			this.game.UpdateViewComplete -= new Action(this.ReleaseNodePoolCache);
			this.nodePool.ReleaseCache();
		}

		public void CleanUp()
		{
			FamilySingleton<TNode>.Family = null;
		}
	}
}
