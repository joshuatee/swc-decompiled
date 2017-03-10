using Net.RichardLord.Ash.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WinRTBridge;

namespace Net.RichardLord.Ash.Core
{
	public class Game : IGame
	{
		private IFamilyFactory familyFactory;

		private EntityList entities;

		private SystemList<SimSystemBase> simSystems;

		private SystemList<ViewSystemBase> viewSystems;

		private bool updatingSim;

		private bool updatingView;

		private ushort simThread;

		private ushort viewThread;

		private List<IFamily> families;

		private List<INodeList> nodeLists;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action UpdateSimComplete;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action UpdateViewComplete;

		public bool Updating
		{
			get
			{
				return this.updatingSim || this.updatingView;
			}
		}

		public Game(IFamilyFactory familyFactory)
		{
			this.familyFactory = familyFactory;
			this.entities = new EntityList();
			this.simSystems = new SystemList<SimSystemBase>();
			this.viewSystems = new SystemList<ViewSystemBase>();
			this.families = new List<IFamily>();
			this.nodeLists = new List<INodeList>();
			this.RestartThreading();
		}

		public void AddEntity(Entity entity)
		{
			this.entities.Add(entity);
			entity.ComponentAdded += new Action<Entity, Type>(this.ComponentAdded);
			entity.ComponentRemoved += new Action<Entity, Type>(this.ComponentRemoved);
			int i = 0;
			int count = this.families.Count;
			while (i < count)
			{
				this.families[i].NewEntity(entity);
				i++;
			}
		}

		public void RemoveEntity(Entity entity)
		{
			entity.ComponentAdded -= new Action<Entity, Type>(this.ComponentAdded);
			entity.ComponentRemoved -= new Action<Entity, Type>(this.ComponentRemoved);
			entity.RemoveAll();
			int i = 0;
			int count = this.families.Count;
			while (i < count)
			{
				this.families[i].RemoveEntity(entity);
				i++;
			}
			this.entities.Remove(entity);
		}

		public void RemoveAllEntities()
		{
			while (this.entities.Head != null)
			{
				this.RemoveEntity(this.entities.Head);
			}
		}

		public EntityList GetAllEntities()
		{
			return this.entities;
		}

		private void ComponentAdded(Entity entity, Type componentClass)
		{
			int i = 0;
			int count = this.families.Count;
			while (i < count)
			{
				this.families[i].ComponentAddedToEntity(entity, componentClass);
				i++;
			}
		}

		private void ComponentRemoved(Entity entity, Type componentClass)
		{
			int i = 0;
			int count = this.families.Count;
			while (i < count)
			{
				this.families[i].ComponentRemovedFromEntity(entity, componentClass);
				i++;
			}
		}

		public NodeList<TNode> GetNodeList<TNode>() where TNode : Node<TNode>, new()
		{
			if (NodeListSingleton<TNode>.NodeList != null)
			{
				return NodeListSingleton<TNode>.NodeList;
			}
			NodeList<TNode> nodeList = new NodeList<TNode>();
			NodeListSingleton<TNode>.NodeList = nodeList;
			this.nodeLists.Add(nodeList);
			IFamily newFamily = this.familyFactory.GetNewFamily<TNode>();
			FamilySingleton<TNode>.Family = newFamily;
			this.families.Add(newFamily);
			newFamily.Setup(this);
			for (Entity entity = this.entities.Head; entity != null; entity = entity.Next)
			{
				newFamily.NewEntity(entity);
			}
			return nodeList;
		}

		public void ReleaseNodeList<TNode>() where TNode : Node<TNode>, new()
		{
			if (FamilySingleton<TNode>.Family != null)
			{
				FamilySingleton<TNode>.Family.CleanUp();
				FamilySingleton<TNode>.Family = null;
			}
			if (NodeListSingleton<TNode>.NodeList != null)
			{
				NodeListSingleton<TNode>.NodeList.RemoveAll();
				NodeListSingleton<TNode>.NodeList = null;
			}
		}

		public void AddSimSystem(SimSystemBase system, int priority, ushort schedulingPattern)
		{
			this.AddSystemToGame<SimSystemBase>(system, priority, schedulingPattern);
			this.simSystems.Add(system);
		}

		public void AddViewSystem(ViewSystemBase system, int priority, ushort schedulingPattern)
		{
			this.AddSystemToGame<ViewSystemBase>(system, priority, schedulingPattern);
			this.viewSystems.Add(system);
		}

		private void AddSystemToGame<T>(SystemBase<T> system, int priority, ushort schedulingPattern)
		{
			system.Priority = priority;
			system.SchedulingPattern = schedulingPattern;
			system.AddToGame(this);
		}

		public SimSystemBase GetSimSystem(Type type)
		{
			return this.simSystems.Get(type);
		}

		public ViewSystemBase GetViewSystem(Type type)
		{
			return this.viewSystems.Get(type);
		}

		public T GetSimSystem<T>() where T : SimSystemBase
		{
			return this.simSystems.Get(typeof(T)) as T;
		}

		public T GetViewSystem<T>() where T : ViewSystemBase
		{
			return this.viewSystems.Get(typeof(T)) as T;
		}

		public bool IsSimSystemSet<T>() where T : SimSystemBase
		{
			return this.simSystems.Get(typeof(T)) is T;
		}

		public bool IsViewSystemSet<T>() where T : ViewSystemBase
		{
			return this.viewSystems.Get(typeof(T)) is T;
		}

		public void RemoveSimSystem(SimSystemBase system)
		{
			this.simSystems.Remove(system);
			system.RemoveFromGame(this);
		}

		public void RemoveSimSystem<T>() where T : SimSystemBase
		{
			this.RemoveSimSystem(this.GetSimSystem<T>());
		}

		public void RemoveViewSystem(ViewSystemBase system)
		{
			this.viewSystems.Remove(system);
			system.RemoveFromGame(this);
		}

		public void RemoveViewSystem<T>() where T : ViewSystemBase
		{
			this.RemoveViewSystem(this.GetViewSystem<T>());
		}

		public void RemoveAllSystems()
		{
			this.RemoveAllSimSystems();
			this.RemoveAllViewSystems();
		}

		public void RemoveAllSimSystems()
		{
			while (this.simSystems.Head != null)
			{
				this.RemoveSimSystem(this.simSystems.Head);
			}
		}

		public void RemoveAllViewSystems()
		{
			while (this.viewSystems.Head != null)
			{
				this.RemoveViewSystem(this.viewSystems.Head);
			}
		}

		public void UpdateSimSystems(uint dt)
		{
			this.updatingSim = true;
			for (SimSystemBase simSystemBase = this.simSystems.Head; simSystemBase != null; simSystemBase = simSystemBase.Next)
			{
				simSystemBase.AccumulateDT(dt);
				if ((simSystemBase.SchedulingPattern & this.simThread) != 0)
				{
					simSystemBase.Update();
				}
			}
			this.updatingSim = false;
			if (this.UpdateSimComplete != null)
			{
				this.UpdateSimComplete.Invoke();
			}
			this.simThread = (ushort)((int)this.simThread << 1 | 1);
		}

		public void UpdateViewSystems(float dt)
		{
			this.updatingView = true;
			for (ViewSystemBase viewSystemBase = this.viewSystems.Head; viewSystemBase != null; viewSystemBase = viewSystemBase.Next)
			{
				viewSystemBase.AccumulateDT(dt);
				if ((viewSystemBase.SchedulingPattern & this.viewThread) != 0)
				{
					viewSystemBase.Update();
				}
			}
			this.updatingView = false;
			if (this.UpdateViewComplete != null)
			{
				this.UpdateViewComplete.Invoke();
			}
			this.viewThread = (ushort)((int)this.viewThread << 1 | 1);
		}

		public void RestartThreading()
		{
			this.simThread = (this.viewThread = 1);
		}

		public void StaticReset()
		{
			int i = 0;
			int count = this.families.Count;
			while (i < count)
			{
				this.families[i].CleanUp();
				i++;
			}
			int j = 0;
			int count2 = this.nodeLists.Count;
			while (j < count2)
			{
				this.nodeLists[j].CleanUp();
				j++;
			}
		}

		protected internal Game(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).UpdateSimComplete += (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).UpdateViewComplete += (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).AddEntity((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).ComponentAdded((Entity)GCHandledObjects.GCHandleToObject(*args), (Type)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).ComponentRemoved((Entity)GCHandledObjects.GCHandleToObject(*args), (Type)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Game)GCHandledObjects.GCHandleToObject(instance)).Updating);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Game)GCHandledObjects.GCHandleToObject(instance)).GetAllEntities());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Game)GCHandledObjects.GCHandleToObject(instance)).GetSimSystem((Type)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Game)GCHandledObjects.GCHandleToObject(instance)).GetViewSystem((Type)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).UpdateSimComplete -= (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).UpdateViewComplete -= (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).RemoveAllEntities();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).RemoveAllSimSystems();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).RemoveAllSystems();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).RemoveAllViewSystems();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).RemoveEntity((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).RemoveSimSystem((SimSystemBase)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).RemoveViewSystem((ViewSystemBase)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).RestartThreading();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).StaticReset();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((Game)GCHandledObjects.GCHandleToObject(instance)).UpdateViewSystems(*(float*)args);
			return -1L;
		}
	}
}
