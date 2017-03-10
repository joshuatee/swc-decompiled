using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World
{
	public class Footprint
	{
		public const float TILE_Y_OFFSET = 0.075f;

		private static readonly Color32 invalidColor = new Color32(204, 0, 0, 255);

		private const string TILES_NAME = "Tiles";

		private const string SECONDARY_TILES_NAME = "Secondary Tiles";

		private FootprintMesh mesh;

		private FootprintMesh secondaryMesh;

		private Material validInnerMaterial;

		private Material validOuterMaterial;

		private Material invalidMaterial;

		private string name;

		private float tileSize;

		private bool generated;

		public Footprint(string name, float tileSize)
		{
			this.name = name;
			this.tileSize = tileSize;
			this.generated = false;
		}

		public void DestroyFootprint()
		{
			if (this.mesh != null)
			{
				this.mesh.DestroyFootprintMesh();
				this.mesh = null;
			}
			if (this.secondaryMesh != null)
			{
				this.secondaryMesh.DestroyFootprintMesh();
				this.secondaryMesh = null;
			}
			if (this.validInnerMaterial != null)
			{
				UnityUtils.DestroyMaterial(this.validInnerMaterial);
				this.validInnerMaterial = null;
			}
			if (this.validOuterMaterial != null)
			{
				UnityUtils.DestroyMaterial(this.validOuterMaterial);
				this.validOuterMaterial = null;
			}
			if (this.invalidMaterial != null)
			{
				UnityUtils.DestroyMaterial(this.invalidMaterial);
				this.invalidMaterial = null;
			}
		}

		public void AddTiles(float worldX, float worldZ, int width, int depth, bool forWall)
		{
			if (this.generated)
			{
				Service.Get<StaRTSLogger>().Error("Cannot add tiles after mesh has been generated");
				return;
			}
			if (this.mesh == null)
			{
				this.mesh = new FootprintMesh(this.MakeName("Tiles"));
			}
			float num = (float)width * this.tileSize;
			float num2 = (float)depth * this.tileSize;
			float num3 = this.tileSize * 0.5f;
			if (forWall)
			{
				if (this.secondaryMesh == null)
				{
					this.secondaryMesh = new FootprintMesh(this.MakeName("Secondary Tiles"));
				}
				this.secondaryMesh.AddQuad(worldX, worldZ, num, num2);
				this.mesh.AddQuad(worldX - num3, worldZ - num3, num * 2f, num2 * 2f);
				return;
			}
			if (width != 1 || depth != 1)
			{
				worldX -= num3;
				worldZ -= num3;
			}
			this.mesh.AddQuad(worldX, worldZ, num, num2);
		}

		private string MakeName(string meshName)
		{
			return string.Format("{0} {1}", new object[]
			{
				this.name,
				meshName
			});
		}

		public void GenerateMesh(bool valid, bool lifted)
		{
			this.GenerateMesh(valid, lifted, true);
		}

		public void GenerateMesh(bool valid, bool lifted, bool allowErrorThrown)
		{
			if (this.mesh == null)
			{
				if (allowErrorThrown)
				{
					Service.Get<StaRTSLogger>().Error("Must Addtiles() before generating mesh");
				}
				return;
			}
			this.mesh.GenerateMeshFromQuads();
			if (this.secondaryMesh != null)
			{
				this.secondaryMesh.GenerateMeshFromQuads();
			}
			this.generated = true;
			this.MoveTiles(0f, 0f, valid, lifted);
		}

		public bool MoveTiles(float worldX, float worldZ, bool valid, bool lifted)
		{
			bool result = false;
			if (!this.generated)
			{
				Service.Get<StaRTSLogger>().Error("Must GenerateMesh() before moving tiles");
				return result;
			}
			Vector3 newPosition = new Vector3(worldX, 0.075f, worldZ);
			if (lifted)
			{
				newPosition.y += 0.075f;
				if (!valid)
				{
					newPosition.y += 0.225000009f;
				}
			}
			if (this.mesh.ModifyTiles(newPosition, this.GetMaterial(valid, true)))
			{
				result = true;
			}
			if (this.secondaryMesh != null)
			{
				if (valid)
				{
					newPosition.y += 0.15f;
				}
				if (this.secondaryMesh.ModifyTiles(newPosition, this.GetMaterial(valid, false)))
				{
					result = true;
				}
			}
			return result;
		}

		private Material GetMaterial(bool valid, bool outer)
		{
			if (!valid)
			{
				return this.GetSimpleMaterialHelper(ref this.invalidMaterial, Footprint.invalidColor);
			}
			if (outer)
			{
				return this.GetMaterialHelper(ref this.validOuterMaterial);
			}
			return this.GetMaterialHelper(ref this.validInnerMaterial);
		}

		private Material GetMaterialHelper(ref Material material)
		{
			if (material == null)
			{
				material = UnityUtils.CreateBuildingColorMaterial();
			}
			return material;
		}

		private Material GetSimpleMaterialHelper(ref Material material, Color color)
		{
			if (material == null)
			{
				material = UnityUtils.CreateColorMaterial(color);
			}
			return material;
		}

		protected internal Footprint(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Footprint)GCHandledObjects.GCHandleToObject(instance)).AddTiles(*(float*)args, *(float*)(args + 1), *(int*)(args + 2), *(int*)(args + 3), *(sbyte*)(args + 4) != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((Footprint)GCHandledObjects.GCHandleToObject(instance)).DestroyFootprint();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Footprint)GCHandledObjects.GCHandleToObject(instance)).GenerateMesh(*(sbyte*)args != 0, *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Footprint)GCHandledObjects.GCHandleToObject(instance)).GenerateMesh(*(sbyte*)args != 0, *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Footprint)GCHandledObjects.GCHandleToObject(instance)).GetMaterial(*(sbyte*)args != 0, *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Footprint)GCHandledObjects.GCHandleToObject(instance)).MakeName(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Footprint)GCHandledObjects.GCHandleToObject(instance)).MoveTiles(*(float*)args, *(float*)(args + 1), *(sbyte*)(args + 2) != 0, *(sbyte*)(args + 3) != 0));
		}
	}
}
