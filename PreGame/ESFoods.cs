using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Prophecy.PreGame
{
	public class ESFoods
    {
        public ESItem[] aFoods = new ESItem[0];
		BiomeDef biome = new BiomeDef();

		public ESFoods(BiomeDef _biome)
		{
			ThingDef[] aTDs = (from k in DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories != null && (x.thingCategories.Any(y => y.defName == "FoodMeals") 
							   || x.thingCategories.Any(y => y.defName == "Foods") || x.thingCategories.Any(y => y.defName == "PlantFoodRaw") 
							   || x.thingCategories.Any(y => y.defName == "AnimalProductRaw") || x.thingCategories.Any(y => y.defName == "EggsUnfertilized"))) select k).ToArray();

			foreach (ThingDef td in aTDs)
			{
				if (td.defName != "MealNutrientPaste" && td.defName != "MealLavish" && td.defName != "MealSurvivalPack" && td.defName != "Chocolate")
				{
					aFoods = aFoods.Concat(GetItems(td, _biome)).ToArray();
				}
				
			}
		}

		private ESItem[] GetItems(ThingDef _ItemType, BiomeDef _biome)
		{
			// Variables	
			ESItem[] aESI = new ESItem[0];
			string strThingDef = _ItemType.defName;
			float floBasePrice = 0;
			int intAmount = 0;

			switch (strThingDef)
			{
				case "MealSimple":
					intAmount = 5;
					floBasePrice = _ItemType.BaseMarketValue * GetAnimalPriceInflate(_biome) * GetPlantPriceInflate(_biome);
					break;
				case "MealFine":
					intAmount = 1;
					floBasePrice = _ItemType.BaseMarketValue * GetAnimalPriceInflate(_biome) * GetPlantPriceInflate(_biome);
					break;
				case "EggChickenUnfertilized":
					intAmount = 5;
					floBasePrice = _ItemType.BaseMarketValue * GetAnimalPriceInflate(_biome);
					break;
				case "InsectJelly":
					intAmount = 1;
					floBasePrice = _ItemType.BaseMarketValue * GetAnimalPriceInflate(_biome);
					break;
				case "Pemmican":
					intAmount = 10;
					floBasePrice = _ItemType.BaseMarketValue * GetAnimalPriceInflate(_biome) * GetPlantPriceInflate(_biome);
					break;
				case "Milk":
					intAmount = 10;
					floBasePrice = _ItemType.BaseMarketValue * GetAnimalPriceInflate(_biome);
					break;
				case "RawAgave":
					if (_biome != BiomeDefOf.Desert && _biome != BiomeDefOf.AridShrubland && _biome.defName != "ExtremeDesert")
					{
						floBasePrice = _ItemType.BaseMarketValue * GetPlantPriceInflate(_biome) * 3f;
					}
					else
					{
						floBasePrice = _ItemType.BaseMarketValue * GetPlantPriceInflate(_biome);
					}
					intAmount = 10;
					break;
				case "RawBerries":
					if (_biome != BiomeDefOf.Desert && _biome.defName != "ExtremeDesert")
					{
						floBasePrice = _ItemType.BaseMarketValue * GetPlantPriceInflate(_biome);
					}
					else
					{
						floBasePrice = _ItemType.BaseMarketValue * GetPlantPriceInflate(_biome) *1.5f;
					}
					intAmount = 10;
					break;
				default:
					floBasePrice = _ItemType.BaseMarketValue * GetPlantPriceInflate(_biome);
					intAmount = 10;
					break;
			}

			aESI = aESI.Concat(new ESItem[] { new ESItem(_ItemType.defName, intAmount, floBasePrice * intAmount) }).ToArray();

			return aESI;
		}

		private float GetAnimalPriceInflate(BiomeDef _biome)
		{
			switch (_biome.defName)
			{
				case "IceSheet":
					return 1.9f;
				case "ExtremeDesert":
					return 1.9f;
				case "SeaIce":
					return 1.8f;
				case "Desert":
					return 1.7f;
				case "Tundra":
					return 1.6f;
				case "AridShrubland":
					return 1.3f;
				case "BorealForest":
					return 1.2f;
				case "TemperateForest":
					return 1.1f;
				case "TropicalRainforest":
					return 1f;
				default:
					return 1.9f;
			}
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

		private void LogFoods()
		{
			string log = "";

			foreach (ESItem esi in aFoods)
			{

				if (esi.thingDef.defName != null)
				{
					string strTd = esi.thingDef.defName;
					log = string.Format("{0}Name: {1}    ", log, strTd);
				}

				if (esi.thingDef.category != null)
				{
					ThingCategory tc = esi.thingDef.category;
					log = string.Format("{0}Category: {1}    ", log, tc.ToString());
				}

				if (esi.thingDef.thingCategories != null)
				{
					List<ThingCategoryDef> ltc = esi.thingDef.thingCategories;
					log = string.Format("{0}ThingCategoryDefs: ", log);
					foreach (ThingCategoryDef tcd in ltc)
					{
						log = string.Format("{0}{1}, ", log, tcd.defName);
					}
				}

				log = string.Format("{0}\n", log);
			}

			Log.Message(log);
		}
    }

    public class ESItem
    {
        public ThingDef thingDef;
        public ThingDef stuff = null;
		public Thing thing = null;
        public string strNameLabel;
        public int thingAmount;
        public int thingAmountTotal;
        public int itemAmount;
        public float price;
        public float basePrice;
        public float subtotal;

        public ESItem(string _thingDef, int _thingAmount, float _basePrice, int _itemAmount = 1, string _stuff = null)
        {
            try
            {
                thingDef = ThingDef.Named(_thingDef);
            }
            catch
            {
                Log.Message(string.Concat("ESItem was NOT able to assign ThingDef named: ", _thingDef, " to thingDef."));
            }

            if (_stuff != null)
            {
                try
                {
                    stuff = ThingDef.Named(_stuff);
                    strNameLabel = "ThingMadeOfStuffLabel".Translate(new object[]{ stuff.LabelAsStuff, thingDef.label });
                    thing = ThingMaker.MakeThing(thingDef, stuff);
                }
                catch
                {
                    Log.Message(string.Concat("ESItem was NOT able to assign ThingDef named: ", _thingDef, " to stuff."));
                }
            }
            else
            {
                try
                {
                    strNameLabel = thingDef.label;
					thing = ThingMaker.MakeThing(thingDef);
				}
				catch
                {

                }
            }

            itemAmount = _itemAmount;
            thingAmount = _thingAmount;
            thingAmountTotal = _thingAmount;
            basePrice = _basePrice;
            price = _basePrice;
            subtotal = _basePrice;
        }

        public void IncrementItem(int _intThingAmount, int _intItemAmount, float _floPrice, float _floSubtotal)
        {
            thingAmountTotal += _intThingAmount;
            itemAmount += _intItemAmount;
            price = _floPrice;
            subtotal += _floSubtotal;
        }

        public void DecrementItem(ESItem _esi)
        {
            thingAmountTotal -= _esi.thingAmount;
            itemAmount -= _esi.itemAmount;
            subtotal -= _esi.price;
        }

		public void DrawIcon(Rect _rect)
		{
			if (thing.DrawColor != null)
			{
				GUI.color = thing.DrawColor;
			}

			Texture icon;
			if (!thing.def.uiIconPath.NullOrEmpty())
			{
				icon = thing.def.uiIcon;
			}
			else
			{
				icon = thing.Graphic.ExtractInnerGraphicFor(thing).MatSingle.mainTexture;
			}
			GUI.DrawTexture(_rect, icon, ScaleMode.ScaleToFit);
			GUI.color = Color.white;
		}
    }
}
