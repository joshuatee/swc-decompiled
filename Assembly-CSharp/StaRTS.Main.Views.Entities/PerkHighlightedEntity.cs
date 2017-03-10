using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class PerkHighlightedEntity : ShaderSwappedEntity
	{
		private const float RATE = 1f;

		private const string PARAM_COLOR = "_Color";

		private const string PARAM_RATE = "_VerticalRate";

		public PerkHighlightedEntity(Entity entity, string shaderName) : base(entity, shaderName)
		{
		}

		protected override void SetMaterialCustomProperties(Material material)
		{
			PlanetVO planet = Service.Get<CurrentPlayer>().Planet;
			material.SetColor("_Color", planet.PlanetPerkShaderColor);
			material.SetFloat("_VerticalRate", 1f);
		}

		protected internal PerkHighlightedEntity(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PerkHighlightedEntity)GCHandledObjects.GCHandleToObject(instance)).SetMaterialCustomProperties((Material)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
