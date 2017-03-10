using System;
using UnityEngine;
using WinRTBridge;

public class UIGeometry
{
	public BetterList<Vector3> verts;

	public BetterList<Vector2> uvs;

	public BetterList<Color32> cols;

	private BetterList<Vector3> mRtpVerts;

	private Vector3 mRtpNormal;

	private Vector4 mRtpTan;

	public bool hasVertices
	{
		get
		{
			return this.verts.size > 0;
		}
	}

	public bool hasTransformed
	{
		get
		{
			return this.mRtpVerts != null && this.mRtpVerts.size > 0 && this.mRtpVerts.size == this.verts.size;
		}
	}

	public void Clear()
	{
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.mRtpVerts.Clear();
	}

	public void ApplyTransform(Matrix4x4 widgetToPanel, bool generateNormals = true)
	{
		if (this.verts.size > 0)
		{
			this.mRtpVerts.Clear();
			int i = 0;
			int size = this.verts.size;
			while (i < size)
			{
				this.mRtpVerts.Add(widgetToPanel.MultiplyPoint3x4(this.verts[i]));
				i++;
			}
			if (generateNormals)
			{
				this.mRtpNormal = widgetToPanel.MultiplyVector(Vector3.back).normalized;
				Vector3 normalized = widgetToPanel.MultiplyVector(Vector3.right).normalized;
				this.mRtpTan = new Vector4(normalized.x, normalized.y, normalized.z, -1f);
				return;
			}
		}
		else
		{
			this.mRtpVerts.Clear();
		}
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		if (this.mRtpVerts != null && this.mRtpVerts.size > 0)
		{
			if (n == null)
			{
				for (int i = 0; i < this.mRtpVerts.size; i++)
				{
					v.Add(this.mRtpVerts.buffer[i]);
					u.Add(this.uvs.buffer[i]);
					c.Add(this.cols.buffer[i]);
				}
				return;
			}
			for (int j = 0; j < this.mRtpVerts.size; j++)
			{
				v.Add(this.mRtpVerts.buffer[j]);
				u.Add(this.uvs.buffer[j]);
				c.Add(this.cols.buffer[j]);
				n.Add(this.mRtpNormal);
				t.Add(this.mRtpTan);
			}
		}
	}

	public UIGeometry()
	{
		this.verts = new BetterList<Vector3>();
		this.uvs = new BetterList<Vector2>();
		this.cols = new BetterList<Color32>();
		this.mRtpVerts = new BetterList<Vector3>();
		base..ctor();
	}

	protected internal UIGeometry(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIGeometry)GCHandledObjects.GCHandleToObject(instance)).ApplyTransform(*(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIGeometry)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIGeometry)GCHandledObjects.GCHandleToObject(instance)).hasTransformed);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIGeometry)GCHandledObjects.GCHandleToObject(instance)).hasVertices);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIGeometry)GCHandledObjects.GCHandleToObject(instance)).WriteToBuffers((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]), (BetterList<Vector3>)GCHandledObjects.GCHandleToObject(args[3]), (BetterList<Vector4>)GCHandledObjects.GCHandleToObject(args[4]));
		return -1L;
	}
}
