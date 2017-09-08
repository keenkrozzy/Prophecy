using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Prophesy.PreGame
{
    public class ESApparel
    {
        public ESItem[] aApparel = new ESItem[0];
        public int length;

        public ESApparel()
        {
            aApparel = aApparel.Concat(new ESItem[] { new ESItem("Pemmican",10,10f), new ESItem("MealSimple",1,9f), new ESItem("MealFine",1,11f), new ESItem("Kibble",10,5f), new ESItem("Hay",10,4f),
            new ESItem("RawBerries",1,5f), new ESItem("RawPotatoes",1,5f), new ESItem("RawCorn",1,5f), new ESItem("RawRice",1,5f), new ESItem("RawAgave",1,5f), new ESItem("InsectJelly",1,5f),
            new ESItem("Milk",10,5f), new ESItem("EggChickenUnfertilized",1,3f)}).ToArray();

            length = aApparel.Length;

            Log.Message("ctor fired for ESFoods");
        }
    }
}
