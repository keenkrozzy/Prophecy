using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Prophesy.PreGame
{
    public class ESFoods
    {
        public ESItem[] aFoods = new ESItem[0];
		BiomeDef biome = new BiomeDef();

		public ESFoods(BiomeDef _biome)
		{
			aFoods = aFoods.Concat(new ESItem[] { new ESItem("MealSimple",1,9f), new ESItem("MealFine",1,11f)}).ToArray();

			biome = _biome;
			switch (biome.defName)
			{
				case "AridShrubland":
					aFoods = aFoods.Concat(new ESItem[] { new ESItem("Pemmican", 10, 10f), new ESItem("Kibble", 10, 5f), new ESItem("Hay", 10, 4f),
					new ESItem("RawBerries",1,5f), new ESItem("RawPotatoes",1,5f), new ESItem("RawCorn",1,5f), new ESItem("RawRice",1,5f),
					new ESItem("Milk",10,5f), new ESItem("EggChickenUnfertilized",1,3f), new ESItem("InsectJelly",1,5f)}).ToArray();
					break;
				case "Desert":
					aFoods = aFoods.Concat(new ESItem[] { new ESItem("Pemmican", 10, 12f), new ESItem("Kibble", 10, 6f), new ESItem("Hay", 10, 5f),
					new ESItem("RawBerries",1,6f), new ESItem("RawPotatoes",1,6f), new ESItem("RawCorn",1,6f), new ESItem("RawRice",1,6f), new ESItem("RawAgave",1,5f),
					new ESItem("Milk",10,6f), new ESItem("EggChickenUnfertilized",1,4f), new ESItem("InsectJelly",1,5f)}).ToArray();
					break;
				case "ExtremeDesert":
					aFoods = aFoods.Concat(new ESItem[] { new ESItem("Pemmican", 10, 12f), new ESItem("Kibble", 12, 5f), new ESItem("RawAgave", 1, 5f), new ESItem("InsectJelly", 1, 5f) }).ToArray();
					break;
				case "TemperateForest":
					aFoods = aFoods.Concat(new ESItem[] { new ESItem("Pemmican", 10, 10f), new ESItem("Kibble", 10, 5f), new ESItem("Hay", 10, 4f),
					new ESItem("RawBerries",1,5f), new ESItem("RawPotatoes",1,5f), new ESItem("RawCorn",1,5f), new ESItem("RawRice",1,5f),
					new ESItem("Milk",10,5f), new ESItem("EggChickenUnfertilized",1,3f), new ESItem("InsectJelly",1,5f)}).ToArray();
					break;
				case "TropicalRainforest":
					aFoods = aFoods.Concat(new ESItem[] { new ESItem("Pemmican", 10, 10f), new ESItem("Kibble", 10, 5f), new ESItem("Hay", 10, 4f),
					new ESItem("RawBerries",1,5f), new ESItem("RawPotatoes",1,5f), new ESItem("RawCorn",1,5f), new ESItem("RawRice",1,5f),
					new ESItem("Milk",10,5f), new ESItem("EggChickenUnfertilized",1,3f), new ESItem("InsectJelly",1,5f)}).ToArray();
					break;
				case "BorealForest":
					aFoods = aFoods.Concat(new ESItem[] { new ESItem("Pemmican", 10, 10f), new ESItem("Kibble", 10, 5f), new ESItem("Hay", 10, 4f),
					new ESItem("RawBerries",1,5f), new ESItem("RawPotatoes",1,5f), new ESItem("RawCorn",1,5f), new ESItem("RawRice",1,5f),
					new ESItem("Milk",10,5f), new ESItem("EggChickenUnfertilized",1,3f), new ESItem("InsectJelly",1,5f)}).ToArray();
					break;
				case "Tundra":
					aFoods = aFoods.Concat(new ESItem[] { new ESItem("Pemmican", 12, 10f), new ESItem("Kibble", 12, 5f), new ESItem("Hay", 10, 5f),
					new ESItem("RawBerries",1,6f), new ESItem("RawPotatoes",1,6f), new ESItem("RawCorn",1,6f),
					new ESItem("Milk",10,6f), new ESItem("EggChickenUnfertilized",1,4f), new ESItem("InsectJelly",1,5f)}).ToArray();
					break;
				case "IceSheet":
					aFoods = aFoods.Concat(new ESItem[] { }).ToArray();
					break;
				case "SeaIce":
					aFoods = aFoods.Concat(new ESItem[] { }).ToArray();
					break;
			}
		}
    }

    public class ESItem
    {
        public ThingDef thingDef;
        public ThingDef stuff = null;
		public Thing thing = null;
		//public Texture2D icon = null;
		//public Color iconColor = Color.white;
        public string strNameLabel;
        public int thingAmount;
        public int thingAmountTotal;
        public int itemAmount;
        public float price;
        public float basePrice;
        public float subtotal;

        public ESItem(string _thingDef, int _thingAmount, float _basePrice, string _stuff = null)
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
					//mini = MinifyUtility.MakeMinified(thing);
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

            itemAmount = 1;
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
