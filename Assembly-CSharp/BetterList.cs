using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using WinRTBridge;

public class BetterList<T>
{
	public delegate int CompareFunc(T left, T right);

	public T[] buffer;

	public int size;

	[DebuggerHidden]
	public T this[int i]
	{
		get
		{
			return this.buffer[i];
		}
		set
		{
			this.buffer[i] = value;
		}
	}

	[DebuggerHidden, DebuggerStepThrough, IteratorStateMachine(typeof(BetterList<>.<GetEnumerator>d__2))]
	public IEnumerator<T> GetEnumerator()
	{
		if (this.buffer != null)
		{
			int num;
			for (int i = 0; i < this.size; i = num)
			{
				yield return this.buffer[i];
				num = i + 1;
			}
		}
		yield break;
	}

	private void AllocateMore()
	{
		T[] array = (this.buffer != null) ? new T[Mathf.Max(this.buffer.Length << 1, 32)] : new T[32];
		if (this.buffer != null && this.size > 0)
		{
			this.buffer.CopyTo(array, 0);
		}
		this.buffer = array;
	}

	private void Trim()
	{
		if (this.size > 0)
		{
			if (this.size < this.buffer.Length)
			{
				T[] array = new T[this.size];
				for (int i = 0; i < this.size; i++)
				{
					array[i] = this.buffer[i];
				}
				this.buffer = array;
				return;
			}
		}
		else
		{
			this.buffer = null;
		}
	}

	public void Clear()
	{
		this.size = 0;
	}

	public void Release()
	{
		this.size = 0;
		this.buffer = null;
	}

	public void Add(T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		T[] arg_36_0 = this.buffer;
		int num = this.size;
		this.size = num + 1;
		arg_36_0[num] = item;
	}

	public void Insert(int index, T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		if (index > -1 && index < this.size)
		{
			for (int i = this.size; i > index; i--)
			{
				this.buffer[i] = this.buffer[i - 1];
			}
			this.buffer[index] = item;
			this.size++;
			return;
		}
		this.Add(item);
	}

	public bool Contains(T item)
	{
		if (this.buffer == null)
		{
			return false;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	public int IndexOf(T item)
	{
		if (this.buffer == null)
		{
			return -1;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return i;
			}
		}
		return -1;
	}

	public bool Remove(T item)
	{
		if (this.buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.size; i++)
			{
				if (@default.Equals(this.buffer[i], item))
				{
					this.size--;
					this.buffer[i] = default(T);
					for (int j = i; j < this.size; j++)
					{
						this.buffer[j] = this.buffer[j + 1];
					}
					this.buffer[this.size] = default(T);
					return true;
				}
			}
		}
		return false;
	}

	public void RemoveAt(int index)
	{
		if (this.buffer != null && index > -1 && index < this.size)
		{
			this.size--;
			this.buffer[index] = default(T);
			for (int i = index; i < this.size; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
			this.buffer[this.size] = default(T);
		}
	}

	public T Pop()
	{
		if (this.buffer != null && this.size != 0)
		{
			T[] arg_27_0 = this.buffer;
			int num = this.size - 1;
			this.size = num;
			T result = arg_27_0[num];
			this.buffer[this.size] = default(T);
			return result;
		}
		return default(T);
	}

	public T[] ToArray()
	{
		this.Trim();
		return this.buffer;
	}

	[DebuggerHidden, DebuggerStepThrough]
	public void Sort(BetterList<T>.CompareFunc comparer)
	{
		int num = 0;
		int num2 = this.size - 1;
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = num; i < num2; i++)
			{
				if (comparer(this.buffer[i], this.buffer[i + 1]) > 0)
				{
					T t = this.buffer[i];
					this.buffer[i] = this.buffer[i + 1];
					this.buffer[i + 1] = t;
					flag = true;
				}
				else if (!flag)
				{
					num = ((i == 0) ? 0 : (i - 1));
				}
			}
		}
	}

	public BetterList()
	{
	}

	protected internal BetterList(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((BetterList<int>)GCHandledObjects.GCHandleToObject(instance)).AllocateMore();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((BetterList<int>)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((BetterList<int>)GCHandledObjects.GCHandleToObject(instance)).Release();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((BetterList<int>)GCHandledObjects.GCHandleToObject(instance)).RemoveAt(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((BetterList<int>)GCHandledObjects.GCHandleToObject(instance)).Trim();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((BetterList<string>)GCHandledObjects.GCHandleToObject(instance)).AllocateMore();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((BetterList<string>)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((BetterList<string>)GCHandledObjects.GCHandleToObject(instance)).Release();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((BetterList<string>)GCHandledObjects.GCHandleToObject(instance)).RemoveAt(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((BetterList<string>)GCHandledObjects.GCHandleToObject(instance)).Trim();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((BetterList<UIDrawCall>)GCHandledObjects.GCHandleToObject(instance)).AllocateMore();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((BetterList<UIDrawCall>)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((BetterList<UIDrawCall>)GCHandledObjects.GCHandleToObject(instance)).Release();
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((BetterList<UIDrawCall>)GCHandledObjects.GCHandleToObject(instance)).RemoveAt(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((BetterList<UIDrawCall>)GCHandledObjects.GCHandleToObject(instance)).Trim();
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((BetterList<UITextList.Paragraph>)GCHandledObjects.GCHandleToObject(instance)).AllocateMore();
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((BetterList<UITextList.Paragraph>)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((BetterList<UITextList.Paragraph>)GCHandledObjects.GCHandleToObject(instance)).Release();
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((BetterList<UITextList.Paragraph>)GCHandledObjects.GCHandleToObject(instance)).RemoveAt(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((BetterList<UITextList.Paragraph>)GCHandledObjects.GCHandleToObject(instance)).Trim();
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((BetterList<Color32>)GCHandledObjects.GCHandleToObject(instance)).AllocateMore();
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((BetterList<Color32>)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((BetterList<Color32>)GCHandledObjects.GCHandleToObject(instance)).Release();
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((BetterList<Color32>)GCHandledObjects.GCHandleToObject(instance)).RemoveAt(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		((BetterList<Color32>)GCHandledObjects.GCHandleToObject(instance)).Trim();
		return -1L;
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		((BetterList<Vector2>)GCHandledObjects.GCHandleToObject(instance)).AllocateMore();
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		((BetterList<Vector2>)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		((BetterList<Vector2>)GCHandledObjects.GCHandleToObject(instance)).Release();
		return -1L;
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		((BetterList<Vector2>)GCHandledObjects.GCHandleToObject(instance)).RemoveAt(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		((BetterList<Vector2>)GCHandledObjects.GCHandleToObject(instance)).Trim();
		return -1L;
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(instance)).AllocateMore();
		return -1L;
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(instance)).Release();
		return -1L;
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(instance)).RemoveAt(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(instance)).Trim();
		return -1L;
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		((BetterList<Vector4>)GCHandledObjects.GCHandleToObject(instance)).AllocateMore();
		return -1L;
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		((BetterList<Vector4>)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		((BetterList<Vector4>)GCHandledObjects.GCHandleToObject(instance)).Release();
		return -1L;
	}

	public unsafe static long $Invoke38(long instance, long* args)
	{
		((BetterList<Vector4>)GCHandledObjects.GCHandleToObject(instance)).RemoveAt(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke39(long instance, long* args)
	{
		((BetterList<Vector4>)GCHandledObjects.GCHandleToObject(instance)).Trim();
		return -1L;
	}
}
