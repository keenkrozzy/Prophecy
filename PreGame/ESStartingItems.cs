using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Harmony;
using Verse;
using RimWorld;
using Prophecy.ProGame;

namespace Prophecy.PreGame
{
    public static class ESStartingItems
    {
		public static ESItem[] aStartingItems;

		static ESStartingItems()
		{
			aStartingItems = new ESItem[0];
		}

		public static void LoadESItemsToScenario()
		{
			ProScenPart_ScatterThingsNearPlayerStart[] aproScatThings = new ProScenPart_ScatterThingsNearPlayerStart[aStartingItems.Length];
			Log.Message(string.Concat("aStartingItems.Length: ", aStartingItems.Length.ToString()));

			for (int i = 0; i < aStartingItems.Length; i++)
			{
				aproScatThings[i] = new ProScenPart_ScatterThingsNearPlayerStart();
				if (aStartingItems[i].thing?.def != null)
				{
					aproScatThings[i].ThingDef = aStartingItems[i].thing.def;
				}
				else
				{
					Log.Message(string.Concat("ESStartingItem.LoadESItemsToScenario could not assign to aproScaThings.ThingDef, there is a null value"));
				}
					
				if (aStartingItems[i].thing?.Stuff != null)
				{
					aproScatThings[i].StuffDef = aStartingItems[i].thing.Stuff;
				}
				else
				{
					Log.Message(string.Concat("ESStartingItem.LoadESItemsToScenario could not assign to aproScaThings.StuffDef, there is a null value"));
				}

				if (aStartingItems[i].thingAmountTotal != 0)
				{
					aproScatThings[i].Count = aStartingItems[i].thingAmountTotal;
				}
				else
				{
					Log.Message(string.Concat("ESStartingItem.LoadESItemsToScenario could not assign to aproScaThings.Count, there is 0"));
				}

				aproScatThings[i].Radius = 0;

				try
				{
					Traverse.Create(Find.Scenario).Field("parts").GetValue<List<ScenPart>>().Add(aproScatThings[i]);
				}
				catch
				{
					Log.Message("Could not Traverse Find.Scenario.parts.Add(aproScatThings[i])");
				}
			}

			
		}
	}
}
