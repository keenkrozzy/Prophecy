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
			//string aString = "";
			//foreach (ThingDef td in aItemTDs)
			//{
			//	aString = String.Concat(aString, td.defName, " ");
			//	Thing thing = ThingMaker.MakeThing(td);
			//	if (!thing.def.uiIconPath.NullOrEmpty())
			//	{
			//		//icon = thing.def.uiIcon;
			//		aString = String.Concat(aString, thing.def.uiIcon.name, " \n");
			//	}
			//	else
			//	{
			//		//icon = thing.Graphic.ExtractInnerGraphicFor(thing).MatSingle.mainTexture;
			//		if (thing.Graphic != null)
			//		{
			//			aString = String.Concat(aString, thing.Graphic.ExtractInnerGraphicFor(thing).MatSingle.mainTexture.name, " \n");
			//		}
			//		else
			//		{
			//			aString = String.Concat(aString, "has no uiIconPath or Graphic", " \n");
			//		}
			//	}
			//}
			//Log.Message(aString);

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
