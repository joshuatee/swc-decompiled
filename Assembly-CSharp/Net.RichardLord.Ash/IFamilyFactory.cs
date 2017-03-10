using Net.RichardLord.Ash.Core;
using System;

namespace Net.RichardLord.Ash
{
	public interface IFamilyFactory
	{
		IFamily GetNewFamily<TNode>() where TNode : Node<TNode>, new();
	}
}
