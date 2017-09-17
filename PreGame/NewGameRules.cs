using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Prophesy.PreGame
{
	[StaticConstructorOnStartup]
	public static class NewGameRules
	{
		public static float floStartingPoints = 1000f;
		public static float floCurItemPoints = 0f;

		static NewGameRules()
		{
			DefDatabase<ThingDef>.GetNamed("Silver").BaseMarketValue = 2f;
			DefDatabase<ThingDef>.GetNamed("Gold").BaseMarketValue = 20f;
			DefDatabase<ThingDef>.GetNamed("Bow_Short").BaseMarketValue = 300f;
			DefDatabase<ThingDef>.GetNamed("WoodLog").BaseMarketValue = 1f;
			DefDatabase<ThingDef>.GetNamed("Pila").BaseMarketValue = 200f;
			DefDatabase<ThingDef>.GetNamed("Bow_Great").BaseMarketValue = 500f;
		}
	}
}
