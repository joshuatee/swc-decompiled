using System;

namespace StaRTS.Utils.Pooling
{
	public interface IObjectPool<T>
	{
		int Count
		{
			get;
		}

		int Capacity
		{
			get;
		}

		void EnsurePoolCapacity(int n);

		void ReturnToPool(T obj);

		T GetFromPool(bool allowBeyondCapacity);

		T GetFromPool();

		void Destroy();
	}
}
