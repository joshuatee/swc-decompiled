using System;

namespace StaRTS.Utils.Core
{
	internal class ServiceWrapper<T> : IServiceWrapper
	{
		public static T instance;

		public void Unset()
		{
			ServiceWrapper<T>.instance = default(T);
		}
	}
}
