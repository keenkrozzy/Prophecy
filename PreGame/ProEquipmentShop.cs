using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using System.Reflection;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Prophecy.ProGame;

namespace Prophecy.PreGame
{
	public class ProEquipmentShop
    {
        /********************************/
        private float floMultiTax = 1.2f;
		/********************************/

		private BiomeDef startingBiome = null;
		
        public float floTotalItemsPrice = 0f;

		public ESFoods Foods;
		public ESApparel Apparel;
		public ESWeapons Weapons;
		public ESDrugs Drugs;
		public ESResources Resources;
		public ESItems Items;

		public ProEquipmentShop()
		{

			startingBiome = Find.WorldGrid[Find.GameInitData.startingTile].biome;

			Foods = new ESFoods(startingBiome);
			Apparel = new ESApparel(startingBiome);
			Weapons = new ESWeapons();
			Drugs = new ESDrugs(startingBiome);
			Resources = new ESResources(startingBiome);
			Items = new ESItems();

		}

        public void EquipESItem(ESItem _item)
        {
            floTotalItemsPrice += GetPrice(_item);
			
			ESItem esi;

			if (_item.stuff == null)
			{
				esi = new ESItem(_item.thingDef.ToString(), _item.thingAmount, _item.basePrice);

			}
			else
			{
				esi = new ESItem(_item.thingDef.ToString(), _item.thingAmount, _item.basePrice, 1, _item.stuff.ToString());
			}

			if (ESStartingItems.aStartingItems.Any(x => x.thing.Label == esi.thing.Label))
            {
                ESStartingItems.aStartingItems.FirstOrDefault(x => x.thingDef == esi.thingDef).IncrementItem(esi.thingAmount, esi.itemAmount, GetPrice(esi), GetPrice(esi));
            }
            else
            {
                ESItem[] aesi = { esi };
                ESStartingItems.aStartingItems = ESStartingItems.aStartingItems.Concat(aesi).ToArray();
            }

        }

        public void UnequipESItem(ref ESItem _item)
        {
            floTotalItemsPrice -= SubtractPrice(_item);
			ESItem esi;

			if (_item.stuff == null)
			{
				esi = new ESItem(_item.thingDef.ToString(), _item.thingAmount, _item.basePrice)
				{
					price = _item.price
				};

			}
			else
			{
				esi = new ESItem(_item.thingDef.ToString(), _item.thingAmount, _item.basePrice, 1, _item.stuff.ToString())
				{
					price = _item.price
				};
			}

			ESItem esiCur = ESStartingItems.aStartingItems.FirstOrDefault(x => x.thing.Label == esi.thing.Label);

            esiCur.DecrementItem(esi);

            _item.price = SubtractPrice(_item);

            if (esiCur.thingAmountTotal <= 0)
            {
                List<ESItem> lstESItem = ESStartingItems.aStartingItems.ToList();
                lstESItem.RemoveAt(ESStartingItems.aStartingItems.FirstIndexOf(x => x.thing.Label == esi.thing.Label));
                ESStartingItems.aStartingItems = lstESItem.ToArray();
            }
        }

        public float GetPrice(ESItem _item)
        {
            float floPrice = _item.price;
            int intItemCount = 0;
			int intThingCount = 0;

            if (ESStartingItems.aStartingItems.Any(x => x.thing.Label == _item.thing.Label))
            {
                intItemCount = ESStartingItems.aStartingItems.FirstOrDefault(x => x.thing.Label == _item.thing.Label).itemAmount;
				intThingCount = ESStartingItems.aStartingItems.FirstOrDefault(x => x.thing.Label == _item.thing.Label).thingAmount;
			}

            for (int i = 0; i < intItemCount; i++)
            {
                floPrice = floPrice * floMultiTax;
            }

            return floPrice;
        }

        public float SubtractPrice(ESItem _item)
        {
            float floPrice = _item.basePrice;
            int intItemCount = 0;
			int intThingCount = 0;

			if (ESStartingItems.aStartingItems.Any(x => x.thing.Label == _item.thing.Label))
            {
                intItemCount = ESStartingItems.aStartingItems.FirstOrDefault(x => x.thing.Label == _item.thing.Label).itemAmount -1;
				intThingCount = ESStartingItems.aStartingItems.FirstOrDefault(x => x.thing.Label == _item.thing.Label).thingAmount;
			}

            for (int i = 0; i < intItemCount; i++)
            {
                floPrice = floPrice * floMultiTax;
            }

            return floPrice;
        }

        private int GetTotalItemsPrice()
        {
            int i = 0;

            return i;
        }

		public void SortItemsTool(ESSortList _list, ESSortType _type)
		{
			if (_list == ESSortList.Items)
			{
				switch (_type)
				{
					case ESSortType.DefName:
						Foods.aFoods = Foods.aFoods.OrderBy(esi => esi.thing.Label).ToArray();
						Apparel.aApparel = Apparel.aApparel.OrderBy(esi => esi.thing.Label).ToArray();
						//Weapons. = Weapons.OrderBy(esi => esi.thing.Label).ToArray();
						//Drugs. = Drugs.OrderBy(esi => esi.thing.Label).ToArray();
						//Resources. = Resources.OrderBy(esi => esi.thing.Label).ToArray();
						//Items. = Items.OrderBy(esi => esi.thing.Label).ToArray();
						break;

					case ESSortType.ThingAmount:
						Foods.aFoods = Foods.aFoods.OrderBy(esi => esi.thingAmount).ToArray();
						Apparel.aApparel = Apparel.aApparel.OrderBy(esi => esi.thingAmount).ToArray();
						//Weapons = _aItems.OrderBy(esi => esi.thingAmount).ToArray();
						//Drugs = _aItems.OrderBy(esi => esi.thingAmount).ToArray();
						//Resources = _aItems.OrderBy(esi => esi.thingAmount).ToArray();
						//Items = _aItems.OrderBy(esi => esi.thingAmount).ToArray();
						break;

					case ESSortType.Price:
						Foods.aFoods = Foods.aFoods.OrderBy(esi => GetPrice(esi)).ToArray();
						Apparel.aApparel = Apparel.aApparel.OrderBy(esi => GetPrice(esi)).ToArray();
						//Weapons = _aItems.OrderBy(esi => esi.price).ToArray();
						//Drugs = _aItems.OrderBy(esi => esi.price).ToArray();
						//Resources = _aItems.OrderBy(esi => esi.price).ToArray();
						//Items = _aItems.OrderBy(esi => esi.price).ToArray();
						break;
				}
			}

			if (_list == ESSortList.StartingItems)
			{
				switch (_type)
				{
					case ESSortType.DefName:
						ESStartingItems.aStartingItems = ESStartingItems.aStartingItems.OrderBy(esi => esi.thing.Label).ToArray();
						break;

					case ESSortType.ThingAmount:
						ESStartingItems.aStartingItems = ESStartingItems.aStartingItems.OrderBy(esi => esi.thingAmount).ToArray();
						break;

					case ESSortType.Price:
						ESStartingItems.aStartingItems = ESStartingItems.aStartingItems.OrderBy(esi => esi.subtotal).ToArray();
						break;
				}
			}
		}

		public enum ESSortType
		{
			DefName,
			ThingAmount,
			Price
		}

		public enum ESSortList
		{
			Items,
			StartingItems
		}
        
	}

}
