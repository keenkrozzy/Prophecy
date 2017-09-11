using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using System.Reflection;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Prophesy.ProGame;

namespace Prophesy.PreGame
{
    public class ProEquipmentShop
    {
        /********************************/
        private float floMultiTax = 1.2f;
		/********************************/

		private BiomeDef startingBiome = null;
		
        public float floTotalItemsPrice = 0f;

		public ESFoods Foods;
        public ESStartingItems StartingItems = new ESStartingItems();
		public ESApparel Apparel;
        
        public ThingDef[] aApparelDefs = ThingCategoryDefOf.Apparel.DescendantThingDefs.ToArray();
        public ThingDef[] aWeaponsDefs = ThingCategoryDefOf.Weapons.DescendantThingDefs.ToArray();
        public ThingDef[] aDrugsDefs = 
            ThingCategoryDefOf.Drugs.DescendantThingDefs.Concat(
                ThingCategoryDefOf.Medicine.DescendantThingDefs).ToArray();
        public ThingDef[] aResourcesRawDefs = ThingCategoryDefOf.ResourcesRaw.DescendantThingDefs.Concat(
            ThingCategoryDefOf.Leathers.DescendantThingDefs).ToArray();
        public ThingDef[] aItemsDefs = ThingCategoryDefOf.Art.DescendantThingDefs.Concat(
            ThingCategoryDefOf.Items.childThingDefs).ToArray();

	public ProEquipmentShop()
		{

			startingBiome = Find.WorldGrid[Find.GameInitData.startingTile].biome;

			Foods = new ESFoods(startingBiome);
			Apparel = new ESApparel(startingBiome);

			Log.Message("ctor fired for ProEquipmentShop");
        }

        public void LoadESItemsToScenario()
        {
            ESItem[] aesi = new ESItem[0];

            ProScenPart_ScatterThingsNearPlayerStart[] aproScatThings = new ProScenPart_ScatterThingsNearPlayerStart[aesi.Length];

            for (int i = 0; i <= aesi.Length; i++)
            {
                aproScatThings[i].ThingDef = aesi[i].thingDef;
                aproScatThings[i].Count = aesi[i].thingAmount;
                aproScatThings[i].Radius = 0;

                Traverse.Create(Current.Game.Scenario).Field("parts").GetValue<List<ScenPart>>().Add(aproScatThings[i]);
            }        
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
				esi = new ESItem(_item.thingDef.ToString(), _item.thingAmount, _item.basePrice, _item.stuff.ToString());
			}

			if (StartingItems.aStartingItems.Any(x => x.thing.Label == esi.thing.Label))
            {
                StartingItems.aStartingItems.FirstOrDefault(x => x.thingDef == esi.thingDef).IncrementItem(esi.thingAmount, esi.itemAmount, GetPrice(esi), GetPrice(esi));
            }
            else
            {
                ESItem[] aesi = { esi };
                StartingItems.aStartingItems = StartingItems.aStartingItems.Concat(aesi).ToArray();
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
				esi = new ESItem(_item.thingDef.ToString(), _item.thingAmount, _item.basePrice, _item.stuff.ToString())
				{
					price = _item.price
				};
			}

			ESItem esiCur = StartingItems.aStartingItems.FirstOrDefault(x => x.thing.Label == esi.thing.Label);

            esiCur.DecrementItem(esi);

            _item.price = SubtractPrice(_item);

            if (esiCur.thingAmountTotal <= 0)
            {
                List<ESItem> lstESItem = StartingItems.aStartingItems.ToList();
                lstESItem.RemoveAt(StartingItems.aStartingItems.FirstIndexOf(x => x.thing.Label == esi.thing.Label));
                StartingItems.aStartingItems = lstESItem.ToArray();
            }
        }

        public float GetPrice(ESItem _item)
        {
            float floPrice = _item.price;
            int intItemCount = 0;

            if (StartingItems.aStartingItems.Any(x => x.thing.Label == _item.thing.Label))
            {
                intItemCount = StartingItems.aStartingItems.FirstOrDefault(x => x.thing.Label == _item.thing.Label).itemAmount;
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

            if (StartingItems.aStartingItems.Any(x => x.thing.Label == _item.thing.Label))
            {
                intItemCount = StartingItems.aStartingItems.FirstOrDefault(x => x.thing.Label == _item.thing.Label).itemAmount -1;
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
        
	}

}
