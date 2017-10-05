using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Prophecy.PreGame
{
	public class ESItems
	{
		public ESItem[] aItems = new ESItem[0];
		ThingDef[] aItemTDs = (from k in DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories != null && (x.thingCategories.Any(y => y.defName == "Items") ||
							 x.thingCategories.Any(y => y.defName == "Manufactured"))) select k).ToArray();

		public ESItems()
		{
			aItemTDs = aItemTDs.Where(x => x != ThingDef.Named("MortarShell") && x != ThingDef.Named("AIPersonaCore") && x != ThingDef.Named("Neurotrainer") &&
						x != ThingDef.Named("Component") && x != ThingDef.Named("Neutroamine") && x != ThingDef.Named("Chemfuel")).ToArray();

			foreach (ThingDef td in aItemTDs)
			{
				aItems = aItems.Concat(GetItems(td)).ToArray();
			}
		}

		private ESItem[] GetItems(ThingDef _ItemType)
		{
			// Variables	
			ESItem[] aESI = new ESItem[0];
			string strThingDef = _ItemType.defName;
			float floBasePrice = _ItemType.BaseMarketValue;

			int intAmount = 0;

			switch (strThingDef)
			{
				case "Wort":
					intAmount = 5;
					break;
				default:
					intAmount = 1;
					break;
			}

			aESI = aESI.Concat(new ESItem[] { new ESItem(_ItemType.defName, intAmount, floBasePrice * intAmount) }).ToArray();

			return aESI;
		}
	}
}
