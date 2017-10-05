using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Prophecy.PreGame
{
	public class ESDrugs
	{
		BiomeDef biome = new BiomeDef();
		public ESItem[] aDrugs = new ESItem[0];
		ThingDef[] aDrugTDs = (from k in DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories != null && (x.thingCategories.Any(y => y.defName == "Drugs") ||
			x.thingCategories.Any(y => y.defName == "Medicine")))select k).ToArray();

		public ESDrugs(BiomeDef _biome)
		{
			biome = _biome;
			aDrugTDs = aDrugTDs.Where(x => x != ThingDef.Named("GlitterworldMedicine") && x != ThingDef.Named("Medicine") && x != ThingDef.Named("WakeUp") &&
						x != ThingDef.Named("GoJuice") && x != ThingDef.Named("Luciferium") && x != ThingDef.Named("Penoxycyline")).ToArray();

			foreach (ThingDef td in aDrugTDs)
			{
				aDrugs = aDrugs.Concat(GetItems(td, biome)).ToArray();
			}
		}

		private ESItem[] GetItems(ThingDef _ItemType, BiomeDef _biome)
		{

			// Variables	
			ESItem[] aESI = new ESItem[0];
			string strThingDef = _ItemType.defName;
			float floBasePrice = _ItemType.BaseMarketValue;
			int intAmount = 0;

			switch (strThingDef)
			{
				case "HerbalMedicine":
					intAmount = 3;
					floBasePrice = floBasePrice * GetPlantPriceInflate(_biome);
					break;
				case "Beer":
					intAmount = 3;
					floBasePrice = floBasePrice * GetPlantPriceInflate(_biome);
					break;
				case "Ambrosia":
					intAmount = 1;
					floBasePrice = floBasePrice * GetPlantPriceInflate(_biome);
					break;
				default:
					intAmount = 1;
					floBasePrice = floBasePrice * GetPlantPriceInflate(_biome);
					break;
			}

			aESI = aESI.Concat(new ESItem[] { new ESItem(_ItemType.defName, intAmount, floBasePrice * intAmount) }).ToArray();

			return aESI;
		}

		private float GetPlantPriceInflate(BiomeDef _biome)
		{
			switch (_biome.defName)
			{
				case "IceSheet":
					return 1.9f;
				case "ExtremeDesert":
					return 1.9f;
				case "SeaIce":
					return 1.9f;
				case "Desert":
					return 1.7f;
				case "Tundra":
					return 1.8f;
				case "AridShrubland":
					return 1.4f;
				case "BorealForest":
					return 1.3f;
				case "TemperateForest":
					return 1.1f;
				case "TropicalRainforest":
					return 1f;
				default:
					return 1.9f;
			}
		}
	}
}
