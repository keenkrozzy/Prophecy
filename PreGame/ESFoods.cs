using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Prophesy.PreGame
{
    public class ESFoods
    {
        public ESItem[] aFoods = new ESItem[0];
        public int length;

        public ESFoods()
        {
            aFoods = aFoods.Concat(new ESItem[] { new ESItem("Pemmican",10,10f), new ESItem("MealSimple",1,9f), new ESItem("MealFine",1,11f), new ESItem("Kibble",10,5f), new ESItem("Hay",10,4f),
            new ESItem("RawBerries",1,5f), new ESItem("RawPotatoes",1,5f), new ESItem("RawCorn",1,5f), new ESItem("RawRice",1,5f), new ESItem("RawAgave",1,5f), new ESItem("InsectJelly",1,5f),
            new ESItem("Milk",10,5f), new ESItem("EggChickenUnfertilized",1,3f)}).ToArray();

            length = aFoods.Length;

            Log.Message("ctor fired for ESFoods");
        }
    }

    public class ESItem
    {
        public ThingDef thingDef;
        public int thingAmount;
        public int thingAmountTotal;
        public int itemAmount;
        public float price;
        public float basePrice;
        public float subtotal;

        public ESItem(string _thingDef, int _thingAmount, float _basePrice)
        {
            try
            {
                thingDef = ThingDef.Named(_thingDef);
            }
            catch
            {
                Log.Message(string.Concat("ESItem was NOT able to assign ThingDef named: ", _thingDef, "."));
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
    }
}
