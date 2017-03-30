using System;

namespace Net.RichardLord.Ash.Core
{
	public class ComponentMatchingFamilyFactory : IFamilyFactory
	{
		public IFamily GetNewFamily<TNode>() where TNode : Node<TNode>, new()
		{
			return new ComponentMatchingFamily<TNode>();
		}
	}
}
