using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Prophesy.PreGame
{
	public class ESWeapons
	{
		public ESItem[] aWeapons = new ESItem[0];
		ThingDef[] aNeolithicWeaponTypes = (from k in DefDatabase<ThingDef>.AllDefs where (k.techLevel == TechLevel.Neolithic && k.IsWeapon && k.recipeMaker != null) select k).ToArray();

		// ThingDef stuffProps can be null
		ThingDef[] aWeaponStuff = (from k in DefDatabase<ThingDef>.AllDefs where (k.stuffProps != null && (k.stuffProps.categories.Any(x => x.defName == "Metallic") ||
								  k.stuffProps.categories.Any(x => x.defName == "Stony") || k.stuffProps.categories.Any(x => x.defName == "Woody")) &&
								   (k.defName != "Steel" && k.defName != "Plasteel" && k.defName != "Uranium")) select k).ToArray();

		public ESWeapons()
		{
			foreach (ThingDef td in aNeolithicWeaponTypes)
			{
				aWeapons = aWeapons.Concat(GetWeapons(td)).ToArray();
			}

			//string aString = "";
			//foreach (ESItem esi in aWeapons)
			//{
			//	aString = String.Concat(aString, esi.strNameLabel, " price:", esi.price, " \n");
			//}
			//Log.Message(aString);
		}

		private ESItem[] GetWeapons(ThingDef _WeaponType)
		{

			// Variables	
			ESItem[] aESI = new ESItem[0];
			string strThingDef = _WeaponType.defName;

			foreach (ThingDef td in aWeaponStuff)
			{
				ESItem ESI = GetWeapon(_WeaponType, td);
				if (ESI != null)
				{
					aESI = aESI.Concat(new ESItem[] { ESI }).ToArray();
				}

			}
			

			return aESI;
		}

		private ESItem GetWeapon(ThingDef _WeaponType, ThingDef _Stuff)
		{
			ESItem ESI = null;
			float floStuffBasePrice = _Stuff.BaseMarketValue;

			if (_WeaponType.stuffCategories != null && _Stuff.stuffProps.CanMake(_WeaponType))
			{
				float price = _WeaponType.BaseMarketValue + ((float)_WeaponType.costStuffCount * floStuffBasePrice);
				//Log.Message(_Stuff.defName);
				ESI = new ESItem(_WeaponType.defName, 1, price, _Stuff.defName);
			}
			else if (_WeaponType.costList != null)
			{
				foreach (ThingCountClass tcc in _WeaponType.costList)
				{
					if (_Stuff.defName == tcc.thingDef.defName)
					{
						float price = _WeaponType.BaseMarketValue + ((float)tcc.count * floStuffBasePrice);
						ESI = new ESItem(_WeaponType.defName, 1, price);
					}
					else
					{
						//Log.Message(string.Concat("ESWeapons.GetWeapon: ", _Stuff.defName, " can NOT make ", _WeaponType.defName, " AND is not on it's costList."));
						ESI = null;
					}
				}

				
			}
			else
			{
				//Log.Message(string.Concat("ESWeapons.GetWeapon: ", _Stuff.defName, " can NOT make ", _WeaponType.defName));
				ESI = null;
			}

			return ESI;
		}
	}
}
