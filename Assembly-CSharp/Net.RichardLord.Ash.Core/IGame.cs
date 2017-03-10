using System;

namespace Net.RichardLord.Ash.Core
{
	public interface IGame
	{
		event Action UpdateSimComplete;

		event Action UpdateViewComplete;

		bool Updating
		{
			get;
		}

		NodeList<TNode> GetNodeList<TNode>() where TNode : Node<TNode>, new();

		void ReleaseNodeList<TNode>() where TNode : Node<TNode>, new();

		void AddEntity(Entity entity);

		void AddSimSystem(SimSystemBase system, int priority, ushort schedulingPattern);

		void AddViewSystem(ViewSystemBase system, int priority, ushort schedulingPattern);

		void RemoveEntity(Entity entity);

		void RemoveAllEntities();

		SimSystemBase GetSimSystem(Type type);

		ViewSystemBase GetViewSystem(Type type);

		void RemoveSimSystem(SimSystemBase system);

		void RemoveViewSystem(ViewSystemBase system);

		void RemoveAllSystems();

		void UpdateSimSystems(uint dt);

		void UpdateViewSystems(float dt);
	}
}
