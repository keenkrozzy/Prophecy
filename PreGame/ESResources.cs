using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Prophesy.PreGame
{
	public class ESResources
	{
		public ESItem[] aResources = new ESItem[0];
		ThingDef[] aResourceTDs = (from k in DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories != null && (x.thingCategories.Any(y => y.defName == "ResourcesRaw") ||
			x.thingCategories.Any(y => y.defName == "PlantMatter") || x.thingCategories.Any(y => y.defName == "StoneBlocks"))) select k).ToArray();

		public ESResources()
		{
			aResourceTDs = aResourceTDs.Where(x => x != ThingDef.Named("Steel") && x != ThingDef.Named("Plasteel") && x != ThingDef.Named("Uranium")).ToArray();

			//string aString = "";
			//foreach (ThingDef td in aResourceTDs)
			//{
			//	aString = String.Concat(aString, td.defName, " \n");
			//}
			//Log.Message(aString);

			foreach (ThingDef td in aResourceTDs)
			{
				aResources = aResources.Concat(GetItems(td)).ToArray();
			}
		}

		private ESItem[] GetItems(ThingDef _ItemType)
		{

			// Variables	
			ESItem[] aESI = new ESItem[0];
			string strThingDef = _ItemType.defName;
			float floBasePrice = _ItemType.BaseMarketValue;


			aESI = aESI.Concat(new ESItem[] { new ESItem(_ItemType.defName, 1, floBasePrice) }).ToArray();




			return aESI;
		}
	}
}
