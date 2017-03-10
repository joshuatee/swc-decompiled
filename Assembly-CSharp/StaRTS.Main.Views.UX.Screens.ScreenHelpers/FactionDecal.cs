using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX.Elements;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers
{
	public class FactionDecal
	{
		private const string SPRITE_ELEMENT = "SpriteHeroDecal";

		private const string SPRITE_NAME_EMPIRE = "HeroDecalEmpire";

		private const string SPRITE_NAME_REBEL = "HeroDecalRebel";

		private const string SPRITE_NAME_NEUTRAL = "HeroDecalNeutral";

		public static void UpdateDeployableDecal(string itemUid, UXGrid grid, IDeployableVO deployableVO)
		{
			string deployableDecalSpriteName = FactionDecal.GetDeployableDecalSpriteName(deployableVO);
			FactionDecal.SetSpriteAndVisibility(grid.GetSubElement<UXSprite>(itemUid, "SpriteHeroDecal"), deployableDecalSpriteName);
		}

		public static void UpdateDeployableDecal(string itemUid, UXGrid grid, IDeployableVO deployableVO, string elementName)
		{
			string deployableDecalSpriteName = FactionDecal.GetDeployableDecalSpriteName(deployableVO);
			FactionDecal.SetSpriteAndVisibility(grid.GetSubElement<UXSprite>(itemUid, elementName), deployableDecalSpriteName);
		}

		public static void UpdateDeployableDecal(string itemUid, string name, UXGrid grid, IDeployableVO deployableVO)
		{
			string deployableDecalSpriteName = FactionDecal.GetDeployableDecalSpriteName(deployableVO);
			string name2 = UXUtils.FormatAppendedName("SpriteHeroDecal", name);
			FactionDecal.SetSpriteAndVisibility(grid.GetSubElement<UXSprite>(itemUid, name2), deployableDecalSpriteName);
		}

		public static void SetDeployableDecalVisibiliy(string itemUid, UXGrid grid, bool visible)
		{
			grid.GetSubElement<UXSprite>(itemUid, "SpriteHeroDecal").Visible = visible;
		}

		public static void SetDeployableDecalVisibiliy(UXFactory uxFactory, bool visible)
		{
			uxFactory.GetElement<UXSprite>("SpriteHeroDecal").Visible = visible;
		}

		private static string GetDeployableDecalSpriteName(IDeployableVO deployableVO)
		{
			string result = null;
			if (deployableVO is TroopTypeVO)
			{
				TroopTypeVO troopTypeVO = (TroopTypeVO)deployableVO;
				if (troopTypeVO.Type == TroopType.Hero)
				{
					if (!string.IsNullOrEmpty(troopTypeVO.UIDecalAssetName))
					{
						result = troopTypeVO.UIDecalAssetName;
					}
					else
					{
						FactionType faction = troopTypeVO.Faction;
						if (faction != FactionType.Empire)
						{
							if (faction != FactionType.Rebel)
							{
								result = "HeroDecalNeutral";
							}
							else
							{
								result = "HeroDecalRebel";
							}
						}
						else
						{
							result = "HeroDecalEmpire";
						}
					}
				}
			}
			return result;
		}

		private static void SetSpriteAndVisibility(UXSprite sprite, string spriteName)
		{
			if (spriteName != null)
			{
				sprite.SpriteName = spriteName;
				sprite.Visible = true;
				return;
			}
			sprite.Visible = false;
		}

		public FactionDecal()
		{
		}

		protected internal FactionDecal(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FactionDecal.GetDeployableDecalSpriteName((IDeployableVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			FactionDecal.SetDeployableDecalVisibiliy((UXFactory)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			FactionDecal.SetDeployableDecalVisibiliy(Marshal.PtrToStringUni(*(IntPtr*)args), (UXGrid)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			FactionDecal.SetSpriteAndVisibility((UXSprite)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			FactionDecal.UpdateDeployableDecal(Marshal.PtrToStringUni(*(IntPtr*)args), (UXGrid)GCHandledObjects.GCHandleToObject(args[1]), (IDeployableVO)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			FactionDecal.UpdateDeployableDecal(Marshal.PtrToStringUni(*(IntPtr*)args), (UXGrid)GCHandledObjects.GCHandleToObject(args[1]), (IDeployableVO)GCHandledObjects.GCHandleToObject(args[2]), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			FactionDecal.UpdateDeployableDecal(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (UXGrid)GCHandledObjects.GCHandleToObject(args[2]), (IDeployableVO)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}
	}
}
