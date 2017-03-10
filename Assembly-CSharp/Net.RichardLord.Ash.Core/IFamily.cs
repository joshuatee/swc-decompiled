using System;

namespace Net.RichardLord.Ash.Core
{
	public interface IFamily
	{
		void Setup(IGame game);

		void NewEntity(Entity entity);

		void RemoveEntity(Entity entity);

		void ComponentAddedToEntity(Entity entity, Type componentClass);

		void ComponentRemovedFromEntity(Entity entity, Type componentClass);

		void CleanUp();
	}
}
