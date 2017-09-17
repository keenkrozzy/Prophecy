using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Prophesy.PreGame
{
	public class ESDrugs
	{
		public ESItem[] aDrugs = new ESItem[0];
		ThingDef[] aDrugTDs = (from k in DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories != null && (x.thingCategories.Any(y => y.defName == "Drugs") ||
			x.thingCategories.Any(y => y.defName == "Medicine")))select k).ToArray();

		public ESDrugs()
		{
			aDrugTDs = aDrugTDs.Where(x => x != ThingDef.Named("GlitterworldMedicine") && x != ThingDef.Named("Plasteel") && x != ThingDef.Named("Uranium")).ToArray();

			//string aString = "";
			//foreach (ThingDef td in aDrugTDs)
			//{
			//	aString = String.Concat(aString, td.defName, " \n");
			//}
			//Log.Message(aString);

			foreach (ThingDef td in aDrugTDs)
			{
				aDrugs = aDrugs.Concat(GetItems(td)).ToArray();
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
