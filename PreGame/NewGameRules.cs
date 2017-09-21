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
		public static float floStartingItemPoints = 2000f;
		public static float floCurItemPoints = 0f;
		public static float floStartingPawnPoints = 1000f;
		public static float floCurPawnPoints = 0;
		public static float[] afloCurPawnPoints = new float[0];
		public static string[] astrCurPawnPoints = new string[0];

		static NewGameRules()
		{
			DefDatabase<ThingDef>.GetNamed("Silver").BaseMarketValue = 2f;
			DefDatabase<ThingDef>.GetNamed("Gold").BaseMarketValue = 20f;
			DefDatabase<ThingDef>.GetNamed("Bow_Short").BaseMarketValue = 300f;
			DefDatabase<ThingDef>.GetNamed("WoodLog").BaseMarketValue = 1f;
			DefDatabase<ThingDef>.GetNamed("Pila").BaseMarketValue = 200f;
			DefDatabase<ThingDef>.GetNamed("Bow_Great").BaseMarketValue = 500f;
		}

		public static void AddPawnToCurPoints(Pawn _pawn)
		{
			astrCurPawnPoints = astrCurPawnPoints.Concat(new string[] { _pawn.GetUniqueLoadID() }).ToArray();
			afloCurPawnPoints = afloCurPawnPoints.Concat(new float[1]).ToArray();
		}

		public static void UpdateCurPawnPoints(Pawn _pawn, float _floPoints)
		{ 
			int index = astrCurPawnPoints.FirstIndexOf(x => x == _pawn.GetUniqueLoadID());
			afloCurPawnPoints[index] = _floPoints;

			float floSubTotal = 0;

			foreach (float f in afloCurPawnPoints)
			{
				floSubTotal += f;
			}

			floCurPawnPoints = floStartingPawnPoints - floSubTotal;
		}

		public static void ClearCurPawns()
		{
			afloCurPawnPoints = new float[0];
			astrCurPawnPoints = new string[0];
		}

		public static float GetSkillCost(int _intAge,int _intSkillLevel)
		{
			float floAgeFactor = 32f / (float)_intAge;

			switch (_intSkillLevel)
			{
				case 0:
					return 0f;
				case 1:
					return 2.5f * floAgeFactor;
				case 2:
					return 7f * floAgeFactor;
				case 3:
					return 14.5f * floAgeFactor;
				case 4:
					return 25f * floAgeFactor;
				case 5:
					return 38.6f * floAgeFactor;
				case 6:
					return 55.2f * floAgeFactor;
				case 7:
					return 74.8f * floAgeFactor;
				case 8:
					return 97.7f * floAgeFactor;
				case 9:
					return 123.7f * floAgeFactor;
				case 10:
					return 153f * floAgeFactor;
				case 11:
					return 185.7f * floAgeFactor;
				case 12:
					return 222f * floAgeFactor;
				case 13:
					return 261.9f * floAgeFactor;
				case 14:
					return 305.8f * floAgeFactor;
				case 15:
					return 353.7f * floAgeFactor;
				case 16:
					return 406f * floAgeFactor;
				case 17:
					return 462.9f * floAgeFactor;
				case 18:
					return 524.8f * floAgeFactor;
				case 19:
					return 592f * floAgeFactor;
				case 20:
					return 665f * floAgeFactor;
				default:
					return 0f;
			}
		}
	}
}
