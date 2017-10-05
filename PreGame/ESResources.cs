using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Prophecy.PreGame
{
	public class ESResources
	{
		BiomeDef biome = new BiomeDef();
		public ESItem[] aResources = new ESItem[0];
		ThingDef[] aResourceTDs = (from k in DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories != null && (x.thingCategories.Any(y => y.defName == "ResourcesRaw") ||
			x.thingCategories.Any(y => y.defName == "PlantMatter") || x.thingCategories.Any(y => y.defName == "StoneBlocks"))) select k).ToArray();

		public ESResources(BiomeDef _biome)
		{
			biome = _biome;
			aResourceTDs = aResourceTDs.Where(x => x != ThingDef.Named("Steel") && x != ThingDef.Named("Plasteel") && x != ThingDef.Named("Uranium")).ToArray();

			foreach (ThingDef td in aResourceTDs)
			{
				aResources = aResources.Concat(GetItems(td, biome)).ToArray();
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
				case "WoodLog":
					intAmount = 10;
					floBasePrice = floBasePrice * GetPlantPriceInflate(_biome);
					break;
				case "RawHops":
					intAmount = 10;
					floBasePrice = floBasePrice * GetPlantPriceInflate(_biome);
					break;
				case "PsychoidLeaves":
					intAmount = 10;
					floBasePrice = floBasePrice * GetPlantPriceInflate(_biome);
					break;
				case "SmokeleafLeaves":
					intAmount = 10;
					floBasePrice = floBasePrice * GetPlantPriceInflate(_biome);
					break;
				default:
					intAmount = 10;
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
