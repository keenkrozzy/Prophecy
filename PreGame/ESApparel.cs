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
		BiomeDef biome = new BiomeDef();

        public ESApparel(BiomeDef _biome)
        {
            aApparel = aApparel.Concat(new ESItem[] { new ESItem("Apparel_TribalA", 1, 135f, "WoolMuffalo"), new ESItem("Apparel_Parka", 1, 405f, "WoolMuffalo"),
                new ESItem("Apparel_Tuque", 1, 70f, "WoolMuffalo")}).ToArray();

			biome = _biome;
			switch (biome.defName)
			{
				case "AridShrubland":
					aApparel = aApparel.Concat(new ESItem[] { new ESItem("Apparel_TribalA", 1, 258f, "Squirrel_Leather"), new ESItem("Apparel_TribalA", 1, 87f, "Muffalo_Leather"),
					new ESItem("Apparel_TribalA", 1, 195f, "Gazelle_Leather"), new ESItem("Apparel_TribalA", 1, 321f, "Ibex_Leather"), new ESItem("Apparel_TribalA", 1, 319f, "Boomrat_Leather"),
					new ESItem("Apparel_TribalA", 1, 212f, "Rat_Leather"), new ESItem("Apparel_TribalA", 1, 125f, "Alpaca_Leather"), new ESItem("Apparel_TribalA", 1, 59f, "Elephant_Leather"),
					new ESItem("Apparel_TribalA", 1, 1138f, "Iguana_Leather"), new ESItem("Apparel_TribalA", 1, 404f, "Rhinoceros_Leather"), new ESItem("Apparel_TribalA", 1, 523f, "Pig_Leather"),
					new ESItem("Apparel_TribalA", 1, 164f, "Boomalope_Leather"), new ESItem("Apparel_TribalA", 1, 190f, "Emu_Leather"), new ESItem("Apparel_TribalA", 1, 137f, "Ostrich_Leather"),
					new ESItem("Apparel_TribalA", 1, 731f, "Cougar_Leather"), new ESItem("Apparel_Parka", 1, 884f, "Squirrel_Leather"), new ESItem("Apparel_Parka", 1, 297f, "Muffalo_Leather"),
					new ESItem("Apparel_Parka", 1, 669f, "Gazelle_Leather"), new ESItem("Apparel_Parka", 1, 1101f, "Ibex_Leather"), new ESItem("Apparel_Parka", 1, 1092f, "Boomrat_Leather"),
					new ESItem("Apparel_Parka", 1, 728f, "Rat_Leather"), new ESItem("Apparel_Parka", 1, 429f, "Alpaca_Leather"), new ESItem("Apparel_Parka", 1, 203f, "Elephant_Leather"),
					new ESItem("Apparel_Parka", 1, 1387f, "Rhinoceros_Leather"), new ESItem("Apparel_Parka", 1, 1794f, "Pig_Leather"), new ESItem("Apparel_Parka", 1, 562f, "Boomalope_Leather"),
					new ESItem("Apparel_Parka", 1, 650f, "Emu_Leather"), new ESItem("Apparel_Parka", 1, 468f, "Ostrich_Leather"), new ESItem("Apparel_Tuque", 1, 184f, "Squirrel_Leather"),
					new ESItem("Apparel_Tuque", 1, 62f, "Muffalo_Leather"), new ESItem("Apparel_Tuque", 1, 139f, "Gazelle_Leather"), new ESItem("Apparel_Tuque", 1, 229f, "Ibex_Leather"),
					new ESItem("Apparel_Tuque", 1, 228f, "Boomrat_Leather"), new ESItem("Apparel_Tuque", 1, 152f, "Rat_Leather"), new ESItem("Apparel_Tuque", 1, 89f, "Alpaca_Leather"),
					new ESItem("Apparel_Tuque", 1, 42f, "Elephant_Leather"), new ESItem("Apparel_Tuque", 1, 813f, "Iguana_Leather"), new ESItem("Apparel_Tuque", 1, 289f, "Rhinoceros_Leather"),
					new ESItem("Apparel_Tuque", 1, 374f, "Pig_Leather"), new ESItem("Apparel_Tuque", 1, 117f, "Boomalope_Leather"), new ESItem("Apparel_Tuque", 1, 135f, "Emu_Leather"),
					new ESItem("Apparel_Tuque", 1, 98f, "Ostrich_Leather"), new ESItem("Apparel_Tuque", 1, 522f, "Cougar_Leather"), new ESItem("Apparel_Tuque", 1, 1970f, "FoxFennec_Leather"), }).ToArray();
					break;
				case "Desert":
					aApparel = aApparel.Concat(new ESItem[] { }).ToArray();
					break;
				case "ExtremeDesert":
					aApparel = aApparel.Concat(new ESItem[] { }).ToArray();
					break;
				case "TemperateForest":
					aApparel = aApparel.Concat(new ESItem[] { }).ToArray();
					break;
				case "TropicalRainforest":
					aApparel = aApparel.Concat(new ESItem[] { }).ToArray();
					break;
				case "BorealForest":
					aApparel = aApparel.Concat(new ESItem[] { }).ToArray();
					break;
				case "Tundra":
					aApparel = aApparel.Concat(new ESItem[] { }).ToArray();
					break;
				case "IceSheet":
					aApparel = aApparel.Concat(new ESItem[] { }).ToArray();
					break;
				case "SeaIce":
					aApparel = aApparel.Concat(new ESItem[] { }).ToArray();
					break;
			}
		}
    }
}
