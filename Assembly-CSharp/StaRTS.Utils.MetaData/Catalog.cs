using StaRTS.Assets;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Utils.MetaData
{
	public class Catalog
	{
		public delegate void CatalogDelegate(bool success, string file);

		private Dictionary<string, Sheet> sheets;

		private Dictionary<string, AssetHandle> assetHandles;

		public Catalog()
		{
			this.sheets = new Dictionary<string, Sheet>();
			this.assetHandles = new Dictionary<string, AssetHandle>();
		}

		public Sheet GetSheet(string sheetName)
		{
			return (!this.sheets.ContainsKey(sheetName)) ? null : this.sheets[sheetName];
		}

		public void PatchData(string catalogFile, Catalog.CatalogDelegate completeCallback)
		{
			catalogFile = catalogFile.Replace(".json", ".json.joe");
			int num = catalogFile.LastIndexOf("/");
			string assetName = catalogFile.Substring(num + 1);
			string assetPath = catalogFile.Substring(0, num);
			AssetManager assetManager = Service.Get<AssetManager>();
			assetManager.AddJoeFileToManifest(assetName, assetPath);
			assetManager.RegisterPreloadableAsset(assetName);
			object cookie = new KeyValuePair<string, Catalog.CatalogDelegate>(catalogFile, completeCallback);
			AssetHandle value = AssetHandle.Invalid;
			assetManager.Load(ref value, assetName, new AssetSuccessDelegate(this.AssetSuccess), new AssetFailureDelegate(this.AssetFailure), cookie);
			this.assetHandles.Add(catalogFile, value);
		}

		private void AssetSuccess(object asset, object cookie)
		{
			byte[] binaryContents = Service.Get<AssetManager>().GetBinaryContents(asset);
			JoeFile joe = new JoeFile(binaryContents);
			this.ProcessJoe(joe, cookie);
		}

		private void AssetFailure(object cookie)
		{
			this.ProcessJoe(null, cookie);
		}

		private void ProcessJoe(JoeFile joe, object cookie)
		{
			KeyValuePair<string, Catalog.CatalogDelegate> keyValuePair = (KeyValuePair<string, Catalog.CatalogDelegate>)cookie;
			string text = keyValuePair.Key;
			Catalog.CatalogDelegate value = keyValuePair.Value;
			Service.Get<AssetManager>().Unload(this.assetHandles[text]);
			this.assetHandles.Remove(text);
			if (joe != null)
			{
				this.ParseCatalog(joe);
			}
			if (value != null)
			{
				text = text.Replace(".json.joe", ".json");
				value(joe != null, text);
			}
		}

		public void ParseCatalog(JoeFile joe)
		{
			Sheet[] allSheets = joe.GetAllSheets();
			if (allSheets == null)
			{
				return;
			}
			int i = 0;
			int num = allSheets.Length;
			while (i < num)
			{
				Sheet sheet = allSheets[i];
				string sheetName = sheet.SheetName;
				if (this.sheets.ContainsKey(sheetName))
				{
					this.sheets[sheetName].PatchRows(sheet);
				}
				else
				{
					this.sheets.Add(sheetName, sheet);
				}
				i++;
			}
		}
	}
}
