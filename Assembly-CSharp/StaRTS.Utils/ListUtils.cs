using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StaRTS.Utils
{
	public class ListUtils
	{
		[CompilerGenerated]
		[System.Serializable]
		private sealed class <>c__1<T>
		{
			public static readonly ListUtils.<>c__1<T> <>9 = new ListUtils.<>c__1<T>();

			public static Comparison<T> <>9__1_0;

			internal int <SortListBasedOnToString>b__1_0(T val1, T val2)
			{
				return val1.ToString().CompareTo(val2.ToString());
			}
		}

		public static List<T> ConvertArrayList<T>(System.Collections.ArrayList data)
		{
			List<T> list = new List<T>(data.Count);
			for (int i = 0; i < data.Count; i++)
			{
				list.Add((T)((object)data[i]));
			}
			return list;
		}

		public static void SortListBasedOnToString<T>(List<T> list)
		{
			Comparison<T> arg_20_1;
			if ((arg_20_1 = ListUtils.<>c__1<T>.<>9__1_0) == null)
			{
				arg_20_1 = (ListUtils.<>c__1<T>.<>9__1_0 = new Comparison<T>(ListUtils.<>c__1<T>.<>9.<SortListBasedOnToString>b__1_0));
			}
			list.Sort(arg_20_1);
		}

		public static List<T> CreateAndFillListWithValue<T>(T val, int count)
		{
			List<T> list = new List<T>(count);
			for (int i = 0; i < count; i++)
			{
				list.Add(val);
			}
			return list;
		}

		public ListUtils()
		{
		}

		protected internal ListUtils(UIntPtr dummy)
		{
		}
	}
}
