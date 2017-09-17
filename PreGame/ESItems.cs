using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Prophesy.PreGame
{
	public class ESItems
	{
		public ESItem[] aItems = new ESItem[0];
		ThingDef[] aItemTDs = (from k in DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories != null && (x.thingCategories.Any(y => y.defName == "Items") ||
							 x.thingCategories.Any(y => y.defName == "Manufactured"))) select k).ToArray();

		public ESItems()
		{
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
			
			aESI = aESI.Concat(new ESItem[] { new ESItem(_ItemType.defName,1, floBasePrice) }).ToArray();

			return aESI;
		}
	}
}
