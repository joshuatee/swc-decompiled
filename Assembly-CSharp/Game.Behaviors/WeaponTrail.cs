using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

namespace Game.Behaviors
{
	public class WeaponTrail : MonoBehaviour, IUnitySerializable
	{
		public class Point
		{
			public float TimeCreated;

			public Vector3 BasePosition;

			public Vector3 TipPosition;

			public bool LineBreak;

			public Point()
			{
			}

			protected internal Point(UIntPtr dummy)
			{
			}
		}

		public delegate Vector3 ToVector3<T>(T v);

		public delegate float InterpolateFunction(float a, float b, float c, float d);

		[SerializeField]
		protected internal bool emit;

		[SerializeField]
		protected internal Material material;

		[SerializeField]
		protected internal float lifeTime;

		[SerializeField]
		protected internal Color[] colors;

		[SerializeField]
		protected internal float[] sizes;

		[SerializeField]
		protected internal float minVertexDistance;

		[SerializeField]
		protected internal float maxVertexDistance;

		[SerializeField]
		protected internal float maxAngle;

		[SerializeField]
		protected internal int subdivisions;

		[SerializeField]
		protected internal Transform weaponBase;

		[SerializeField]
		protected internal Transform weaponTip;

		private List<WeaponTrail.Point> points;

		private List<WeaponTrail.Point> smoothedPoints;

		private GameObject trailGO;

		private Mesh trailMesh;

		private Vector3 lastPosition;

		private bool lastFrameEmit;

		private bool started;

		private const string TRAIL_OBJECT_NAME = "Trail";

		private const string TRAIL_MESH_NAME_SUFFIX = "TrailMesh";

		public bool Emit
		{
			set
			{
				this.emit = value;
			}
		}

		private void Start()
		{
			this.started = true;
			this.lastPosition = base.transform.position;
			this.trailGO = new GameObject("Trail");
			this.trailGO.transform.parent = null;
			this.trailGO.transform.position = Vector3.zero;
			this.trailGO.transform.rotation = Quaternion.identity;
			this.trailGO.transform.localScale = Vector3.one;
			this.trailGO.AddComponent(typeof(MeshFilter));
			this.trailGO.AddComponent(typeof(MeshRenderer));
			this.trailGO.GetComponent<Renderer>().sharedMaterial = this.material;
			this.trailMesh = new Mesh();
			this.trailMesh.name = base.name + "TrailMesh";
			this.trailGO.GetComponent<MeshFilter>().sharedMesh = this.trailMesh;
		}

		public void Restart()
		{
			if (this.trailGO == null && this.started)
			{
				this.lastPosition = base.transform.position;
				this.trailGO = new GameObject("Trail");
				this.trailGO.transform.parent = null;
				this.trailGO.transform.position = Vector3.zero;
				this.trailGO.transform.rotation = Quaternion.identity;
				this.trailGO.transform.localScale = Vector3.one;
				this.trailGO.AddComponent(typeof(MeshFilter));
				this.trailGO.AddComponent(typeof(MeshRenderer));
				this.trailGO.GetComponent<Renderer>().sharedMaterial = this.material;
				this.trailMesh.name = base.name + "TrailMesh";
				this.trailGO.GetComponent<MeshFilter>().sharedMesh = this.trailMesh;
			}
		}

		public void OnDisable()
		{
			UnityEngine.Object.Destroy(this.trailGO);
			this.trailGO = null;
		}

		public void ChangeLifeTime(float time)
		{
			this.lifeTime = time;
		}

		public void LateUpdate()
		{
			if (!Camera.main)
			{
				return;
			}
			float magnitude = (this.lastPosition - base.transform.position).magnitude;
			if (this.emit)
			{
				if (magnitude > this.minVertexDistance)
				{
					bool flag = false;
					if (this.points.Count < 3)
					{
						flag = true;
					}
					else
					{
						Vector3 from = this.points[this.points.Count - 2].TipPosition - this.points[this.points.Count - 3].TipPosition;
						Vector3 to = this.points[this.points.Count - 1].TipPosition - this.points[this.points.Count - 2].TipPosition;
						if (Vector3.Angle(from, to) > this.maxAngle || magnitude > this.maxVertexDistance)
						{
							flag = true;
						}
					}
					if (flag)
					{
						WeaponTrail.Point point = new WeaponTrail.Point();
						point.BasePosition = this.weaponBase.position;
						point.TipPosition = this.weaponTip.position;
						point.TimeCreated = Time.time;
						this.points.Add(point);
						this.lastPosition = base.transform.position;
						if (this.points.Count == 1)
						{
							this.smoothedPoints.Add(point);
						}
						else if (this.points.Count > 1)
						{
							for (int i = 0; i < 1 + this.subdivisions; i++)
							{
								this.smoothedPoints.Add(point);
							}
						}
						if (this.points.Count >= 4)
						{
							IEnumerable<Vector3> collection = WeaponTrail.NewCatmullRom(new Vector3[]
							{
								this.points[this.points.Count - 4].TipPosition,
								this.points[this.points.Count - 3].TipPosition,
								this.points[this.points.Count - 2].TipPosition,
								this.points[this.points.Count - 1].TipPosition
							}, this.subdivisions, false);
							IEnumerable<Vector3> collection2 = WeaponTrail.NewCatmullRom(new Vector3[]
							{
								this.points[this.points.Count - 4].BasePosition,
								this.points[this.points.Count - 3].BasePosition,
								this.points[this.points.Count - 2].BasePosition,
								this.points[this.points.Count - 1].BasePosition
							}, this.subdivisions, false);
							List<Vector3> list = new List<Vector3>(collection);
							List<Vector3> list2 = new List<Vector3>(collection2);
							float timeCreated = this.points[this.points.Count - 4].TimeCreated;
							float timeCreated2 = this.points[this.points.Count - 1].TimeCreated;
							for (int j = 0; j < list.Count; j++)
							{
								int num = this.smoothedPoints.Count - (list.Count - j);
								if (num > -1 && num < this.smoothedPoints.Count)
								{
									WeaponTrail.Point point2 = new WeaponTrail.Point();
									point2.BasePosition = list2[j];
									point2.TipPosition = list[j];
									point2.TimeCreated = Mathf.Lerp(timeCreated, timeCreated2, (float)j / (float)list.Count);
									this.smoothedPoints[num] = point2;
								}
							}
						}
					}
					else
					{
						this.points[this.points.Count - 1].BasePosition = this.weaponBase.position;
						this.points[this.points.Count - 1].TipPosition = this.weaponTip.position;
						this.smoothedPoints[this.smoothedPoints.Count - 1].BasePosition = this.weaponBase.position;
						this.smoothedPoints[this.smoothedPoints.Count - 1].TipPosition = this.weaponTip.position;
					}
				}
				else
				{
					if (this.points.Count > 0)
					{
						this.points[this.points.Count - 1].BasePosition = this.weaponBase.position;
						this.points[this.points.Count - 1].TipPosition = this.weaponTip.position;
					}
					if (this.smoothedPoints.Count > 0)
					{
						this.smoothedPoints[this.smoothedPoints.Count - 1].BasePosition = this.weaponBase.position;
						this.smoothedPoints[this.smoothedPoints.Count - 1].TipPosition = this.weaponTip.position;
					}
				}
			}
			if (!this.emit && this.lastFrameEmit && this.points.Count > 0)
			{
				this.points[this.points.Count - 1].LineBreak = true;
			}
			this.lastFrameEmit = this.emit;
			List<WeaponTrail.Point> list3 = new List<WeaponTrail.Point>();
			foreach (WeaponTrail.Point current in this.points)
			{
				if (Time.time - current.TimeCreated > this.lifeTime)
				{
					list3.Add(current);
				}
			}
			foreach (WeaponTrail.Point current2 in list3)
			{
				this.points.Remove(current2);
			}
			list3 = new List<WeaponTrail.Point>();
			foreach (WeaponTrail.Point current3 in this.smoothedPoints)
			{
				if (Time.time - current3.TimeCreated > this.lifeTime)
				{
					list3.Add(current3);
				}
			}
			foreach (WeaponTrail.Point current4 in list3)
			{
				this.smoothedPoints.Remove(current4);
			}
			List<WeaponTrail.Point> list4 = this.smoothedPoints;
			if (list4.Count > 1)
			{
				Vector3[] array = new Vector3[list4.Count * 2];
				Vector2[] array2 = new Vector2[list4.Count * 2];
				int[] array3 = new int[(list4.Count - 1) * 6];
				Color[] array4 = new Color[list4.Count * 2];
				for (int k = 0; k < list4.Count; k++)
				{
					WeaponTrail.Point point3 = list4[k];
					float num2 = (Time.time - point3.TimeCreated) / this.lifeTime;
					Color color = Color.Lerp(Color.white, Color.clear, num2);
					if (this.colors != null && this.colors.Length != 0)
					{
						float num3 = num2 * (float)(this.colors.Length - 1);
						float num4 = Mathf.Floor(num3);
						float num5 = Mathf.Clamp(Mathf.Ceil(num3), 1f, (float)(this.colors.Length - 1));
						float t = Mathf.InverseLerp(num4, num5, num3);
						if (num4 >= (float)this.colors.Length)
						{
							num4 = (float)(this.colors.Length - 1);
						}
						if (num4 < 0f)
						{
							num4 = 0f;
						}
						if (num5 >= (float)this.colors.Length)
						{
							num5 = (float)(this.colors.Length - 1);
						}
						if (num5 < 0f)
						{
							num5 = 0f;
						}
						color = Color.Lerp(this.colors[(int)num4], this.colors[(int)num5], t);
					}
					float num6 = 0f;
					if (this.sizes != null && this.sizes.Length != 0)
					{
						float num7 = num2 * (float)(this.sizes.Length - 1);
						float num8 = Mathf.Floor(num7);
						float num9 = Mathf.Clamp(Mathf.Ceil(num7), 1f, (float)(this.sizes.Length - 1));
						float t2 = Mathf.InverseLerp(num8, num9, num7);
						if (num8 >= (float)this.sizes.Length)
						{
							num8 = (float)(this.sizes.Length - 1);
						}
						if (num8 < 0f)
						{
							num8 = 0f;
						}
						if (num9 >= (float)this.sizes.Length)
						{
							num9 = (float)(this.sizes.Length - 1);
						}
						if (num9 < 0f)
						{
							num9 = 0f;
						}
						num6 = Mathf.Lerp(this.sizes[(int)num8], this.sizes[(int)num9], t2);
					}
					Vector3 a = point3.TipPosition - point3.BasePosition;
					array[k * 2] = point3.BasePosition - a * (num6 * 0.5f);
					array[k * 2 + 1] = point3.TipPosition + a * (num6 * 0.5f);
					array4[k * 2] = (array4[k * 2 + 1] = color);
					float x = (float)k / (float)list4.Count;
					array2[k * 2] = new Vector2(x, 0f);
					array2[k * 2 + 1] = new Vector2(x, 1f);
					if (k > 0)
					{
						array3[(k - 1) * 6] = k * 2 - 2;
						array3[(k - 1) * 6 + 1] = k * 2 - 1;
						array3[(k - 1) * 6 + 2] = k * 2;
						array3[(k - 1) * 6 + 3] = k * 2 + 1;
						array3[(k - 1) * 6 + 4] = k * 2;
						array3[(k - 1) * 6 + 5] = k * 2 - 1;
					}
				}
				this.trailMesh.Clear();
				this.trailMesh.vertices = array;
				this.trailMesh.colors = array4;
				this.trailMesh.uv = array2;
				this.trailMesh.triangles = array3;
				return;
			}
			this.trailMesh.Clear();
		}

		private static Vector3 CastVector3Func(Vector3 v)
		{
			return v;
		}

		private static Vector3 TransformDotPosition(Transform t)
		{
			return t.position;
		}

		public static IEnumerable<Vector3> NewCatmullRom(Transform[] nodes, int slices, bool loop)
		{
			return WeaponTrail.NewCatmullRom<Transform>(nodes, new WeaponTrail.ToVector3<Transform>(WeaponTrail.TransformDotPosition), slices, loop);
		}

		public static IEnumerable<Vector3> NewCatmullRom(Vector3[] points, int slices, bool loop)
		{
			return WeaponTrail.NewCatmullRom<Vector3>(points, new WeaponTrail.ToVector3<Vector3>(WeaponTrail.CastVector3Func), slices, loop);
		}

		[IteratorStateMachine(typeof(WeaponTrail.<NewCatmullRom>d__34<>))]
		private static IEnumerable<Vector3> NewCatmullRom<T>(IList nodes, WeaponTrail.ToVector3<T> toVector3, int slices, bool loop)
		{
			if (nodes.get_Count() >= 2)
			{
				yield return toVector3((T)((object)nodes.get_Item(0)));
				int num = nodes.get_Count() - 1;
				int num2 = 0;
				while (loop || num2 < num)
				{
					if (loop && num2 > num)
					{
						num2 = 0;
					}
					int num3 = (num2 == 0) ? (loop ? num : num2) : (num2 - 1);
					int num4 = num2;
					int num5 = (num2 == num) ? (loop ? 0 : num2) : (num2 + 1);
					int num6 = (num5 == num) ? (loop ? 0 : num5) : (num5 + 1);
					int num7 = slices + 1;
					int num8;
					for (int i = 1; i <= num7; i = num8 + 1)
					{
						yield return WeaponTrail.CatmullRom(toVector3((T)((object)nodes.get_Item(num3))), toVector3((T)((object)nodes.get_Item(num4))), toVector3((T)((object)nodes.get_Item(num5))), toVector3((T)((object)nodes.get_Item(num6))), (float)i, (float)num7);
						num8 = i;
					}
					num8 = num2;
					num2 = num8 + 1;
				}
			}
			yield break;
		}

		private static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime, float duration)
		{
			float num = elapsedTime / duration;
			float num2 = num * num;
			float num3 = num2 * num;
			return previous * (-0.5f * num3 + num2 - 0.5f * num) + start * (1.5f * num3 + -2.5f * num2 + 1f) + end * (-1.5f * num3 + 2f * num2 + 0.5f * num) + next * (0.5f * num3 - 0.5f * num2);
		}

		public WeaponTrail()
		{
			this.emit = true;
			this.lifeTime = 1f;
			this.minVertexDistance = 0.2f;
			this.maxVertexDistance = 2f;
			this.maxAngle = 10f;
			this.subdivisions = 4;
			this.points = new List<WeaponTrail.Point>();
			this.smoothedPoints = new List<WeaponTrail.Point>();
			this.lastFrameEmit = true;
			base..ctor();
		}

		public override void Unity_Serialize(int depth)
		{
			SerializedStateWriter.Instance.WriteBoolean(this.emit);
			SerializedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				SerializedStateWriter.Instance.WriteUnityEngineObject(this.material);
			}
			SerializedStateWriter.Instance.WriteSingle(this.lifeTime);
			if (depth <= 7)
			{
				if (this.colors == null)
				{
					SerializedStateWriter.Instance.WriteInt32(0);
				}
				else
				{
					SerializedStateWriter.Instance.WriteInt32(this.colors.Length);
					for (int i = 0; i < this.colors.Length; i++)
					{
						this.colors[i].Unity_Serialize(depth + 1);
					}
				}
			}
			if (depth <= 7)
			{
				if (this.sizes == null)
				{
					SerializedStateWriter.Instance.WriteInt32(0);
				}
				else
				{
					SerializedStateWriter.Instance.WriteInt32(this.sizes.Length);
					for (int i = 0; i < this.sizes.Length; i++)
					{
						SerializedStateWriter.Instance.WriteSingle(this.sizes[i]);
					}
				}
			}
			SerializedStateWriter.Instance.WriteSingle(this.minVertexDistance);
			SerializedStateWriter.Instance.WriteSingle(this.maxVertexDistance);
			SerializedStateWriter.Instance.WriteSingle(this.maxAngle);
			SerializedStateWriter.Instance.WriteInt32(this.subdivisions);
			if (depth <= 7)
			{
				SerializedStateWriter.Instance.WriteUnityEngineObject(this.weaponBase);
			}
			if (depth <= 7)
			{
				SerializedStateWriter.Instance.WriteUnityEngineObject(this.weaponTip);
			}
		}

		public override void Unity_Deserialize(int depth)
		{
			this.emit = SerializedStateReader.Instance.ReadBoolean();
			SerializedStateReader.Instance.Align();
			if (depth <= 7)
			{
				this.material = (SerializedStateReader.Instance.ReadUnityEngineObject() as Material);
			}
			this.lifeTime = SerializedStateReader.Instance.ReadSingle();
			if (depth <= 7)
			{
				this.colors = new Color[SerializedStateReader.Instance.ReadInt32()];
				for (int i = 0; i < this.colors.Length; i++)
				{
					Color color = default(Color);
					color.Unity_Deserialize(depth + 1);
					this.colors[i] = color;
				}
			}
			if (depth <= 7)
			{
				this.sizes = new float[SerializedStateReader.Instance.ReadInt32()];
				for (int i = 0; i < this.sizes.Length; i++)
				{
					this.sizes[i] = SerializedStateReader.Instance.ReadSingle();
				}
			}
			this.minVertexDistance = SerializedStateReader.Instance.ReadSingle();
			this.maxVertexDistance = SerializedStateReader.Instance.ReadSingle();
			this.maxAngle = SerializedStateReader.Instance.ReadSingle();
			this.subdivisions = SerializedStateReader.Instance.ReadInt32();
			if (depth <= 7)
			{
				this.weaponBase = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
			}
			if (depth <= 7)
			{
				this.weaponTip = (SerializedStateReader.Instance.ReadUnityEngineObject() as Transform);
			}
		}

		public override void Unity_RemapPPtrs(int depth)
		{
			if (this.material != null)
			{
				this.material = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.material) as Material);
			}
			if (this.weaponBase != null)
			{
				this.weaponBase = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.weaponBase) as Transform);
			}
			if (this.weaponTip != null)
			{
				this.weaponTip = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.weaponTip) as Transform);
			}
		}

		public unsafe override void Unity_NamedSerialize(int depth)
		{
			ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
			bool arg_1F_1 = this.emit;
			byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			int var_0_cp_1 = 0;
			arg_1F_0.WriteBoolean(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 4999);
			SerializedNamedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.material, &var_0_cp_0[var_0_cp_1] + 3028);
			}
			SerializedNamedStateWriter.Instance.WriteSingle(this.lifeTime, &var_0_cp_0[var_0_cp_1] + 5004);
			if (depth <= 7)
			{
				if (this.colors == null)
				{
					SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 5013, 0);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				else
				{
					SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 5013, this.colors.Length);
					for (int i = 0; i < this.colors.Length; i++)
					{
						Color[] arg_CB_0_cp_0 = this.colors;
						int arg_CB_0_cp_1 = i;
						SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
						arg_CB_0_cp_0[arg_CB_0_cp_1].Unity_NamedSerialize(depth + 1);
						SerializedNamedStateWriter.Instance.EndMetaGroup();
					}
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
			}
			if (depth <= 7)
			{
				if (this.sizes == null)
				{
					SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 5020, 0);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				else
				{
					SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 5020, this.sizes.Length);
					for (int i = 0; i < this.sizes.Length; i++)
					{
						SerializedNamedStateWriter.Instance.WriteSingle(this.sizes[i], (IntPtr)0);
					}
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
			}
			SerializedNamedStateWriter.Instance.WriteSingle(this.minVertexDistance, &var_0_cp_0[var_0_cp_1] + 5026);
			SerializedNamedStateWriter.Instance.WriteSingle(this.maxVertexDistance, &var_0_cp_0[var_0_cp_1] + 5044);
			SerializedNamedStateWriter.Instance.WriteSingle(this.maxAngle, &var_0_cp_0[var_0_cp_1] + 5062);
			SerializedNamedStateWriter.Instance.WriteInt32(this.subdivisions, &var_0_cp_0[var_0_cp_1] + 5071);
			if (depth <= 7)
			{
				SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.weaponBase, &var_0_cp_0[var_0_cp_1] + 5084);
			}
			if (depth <= 7)
			{
				SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.weaponTip, &var_0_cp_0[var_0_cp_1] + 5095);
			}
		}

		public unsafe override void Unity_NamedDeserialize(int depth)
		{
			ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
			byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			int var_0_cp_1 = 0;
			this.emit = arg_1A_0.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4999);
			SerializedNamedStateReader.Instance.Align();
			if (depth <= 7)
			{
				this.material = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3028) as Material);
			}
			this.lifeTime = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5004);
			if (depth <= 7)
			{
				this.colors = new Color[SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 5013)];
				for (int i = 0; i < this.colors.Length; i++)
				{
					Color color = default(Color);
					SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
					color.Unity_NamedDeserialize(depth + 1);
					SerializedNamedStateReader.Instance.EndMetaGroup();
					this.colors[i] = color;
				}
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			if (depth <= 7)
			{
				this.sizes = new float[SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 5020)];
				for (int i = 0; i < this.sizes.Length; i++)
				{
					this.sizes[i] = SerializedNamedStateReader.Instance.ReadSingle((IntPtr)0);
				}
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			this.minVertexDistance = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5026);
			this.maxVertexDistance = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5044);
			this.maxAngle = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 5062);
			this.subdivisions = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 5071);
			if (depth <= 7)
			{
				this.weaponBase = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 5084) as Transform);
			}
			if (depth <= 7)
			{
				this.weaponTip = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 5095) as Transform);
			}
		}

		protected internal WeaponTrail(UIntPtr dummy) : base(dummy)
		{
		}

		public static bool $Get0(object instance)
		{
			return ((WeaponTrail)instance).emit;
		}

		public static void $Set0(object instance, bool value)
		{
			((WeaponTrail)instance).emit = value;
		}

		public static long $Get1(object instance)
		{
			return GCHandledObjects.ObjectToGCHandle(((WeaponTrail)instance).material);
		}

		public static void $Set1(object instance, long value)
		{
			((WeaponTrail)instance).material = (Material)GCHandledObjects.GCHandleToObject(value);
		}

		public static float $Get2(object instance)
		{
			return ((WeaponTrail)instance).lifeTime;
		}

		public static void $Set2(object instance, float value)
		{
			((WeaponTrail)instance).lifeTime = value;
		}

		public static float $Get3(object instance)
		{
			return ((WeaponTrail)instance).minVertexDistance;
		}

		public static void $Set3(object instance, float value)
		{
			((WeaponTrail)instance).minVertexDistance = value;
		}

		public static float $Get4(object instance)
		{
			return ((WeaponTrail)instance).maxVertexDistance;
		}

		public static void $Set4(object instance, float value)
		{
			((WeaponTrail)instance).maxVertexDistance = value;
		}

		public static float $Get5(object instance)
		{
			return ((WeaponTrail)instance).maxAngle;
		}

		public static void $Set5(object instance, float value)
		{
			((WeaponTrail)instance).maxAngle = value;
		}

		public static long $Get6(object instance)
		{
			return GCHandledObjects.ObjectToGCHandle(((WeaponTrail)instance).weaponBase);
		}

		public static void $Set6(object instance, long value)
		{
			((WeaponTrail)instance).weaponBase = (Transform)GCHandledObjects.GCHandleToObject(value);
		}

		public static long $Get7(object instance)
		{
			return GCHandledObjects.ObjectToGCHandle(((WeaponTrail)instance).weaponTip);
		}

		public static void $Set7(object instance, long value)
		{
			((WeaponTrail)instance).weaponTip = (Transform)GCHandledObjects.GCHandleToObject(value);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WeaponTrail.CastVector3Func(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WeaponTrail.CatmullRom(*(*(IntPtr*)args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2)), *(*(IntPtr*)(args + 3)), *(float*)(args + 4), *(float*)(args + 5)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).ChangeLifeTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WeaponTrail.NewCatmullRom((Transform[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), *(int*)(args + 1), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WeaponTrail.NewCatmullRom((Vector3[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), *(int*)(args + 1), *(sbyte*)(args + 2) != 0));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).Restart();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).Emit = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WeaponTrail.TransformDotPosition((Transform)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((WeaponTrail)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
