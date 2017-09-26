using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using Prophecy.ProGame.Elements;

namespace Prophecy.PreGame
{
	[StaticConstructorOnStartup]
	public static class NewGameRules
	{
		public static float floStartingItemPoints = 2000f;
		public static float floCurItemPoints = 0f;
		public static float floStartingPawnPoints = 2000f;
		public static float floCurPawnPoints = 0;
		public static float[] afloCurPawnPoints = new float[0];
		public static string[] astrCurPawnPoints = new string[0];
		private static float floBasePassionCost = 1f;
		private static float floIncrementingReturn = .1f;


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

		public static float GetSkillCost(int _intAge, int _intSkillLevel)
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

		public static float GetPassionCost(Pawn _pawn, Passion _passion)
		{
			float floIncrements = 0f;
			float floPassionScore = 0f;
			float floMinorCost = 50f;
			float floMajorCost = 150f;

			//if (_passion == Passion.Minor && floPassionScore > 0f)
			//{
			//	return ((floPassionScore * floIncrementingReturn) * floMinorCost) + (floMinorCost * floBasePassionCost);
			//}
			//else if (_passion == Passion.Major && floPassionScore > 0f)
			//{
			//	return ((floPassionScore * floIncrementingReturn) * floMajorCost) + (floMajorCost * floBasePassionCost);
			//}
			//else if (_passion == Passion.Minor)
			//{
			//	return floMinorCost * floBasePassionCost;
			//}
			//else if (_passion == Passion.Major)
			//{
			//	return floMajorCost * floBasePassionCost;
			//}
			//else
			//{
			//	return 0f;
			//}

			foreach (SkillRecord sk in _pawn.skills.skills)
			{
				if (sk.passion == Passion.Minor)
				{
					floIncrements += 1f;
					floPassionScore += floMinorCost * floBasePassionCost;
				}
				else if (sk.passion == Passion.Major)
				{
					floIncrements += 2f;
					floPassionScore += floMajorCost * floBasePassionCost;
				}
			}

			//if (_passion == Passion.Minor && floPassionScore > 0f)
			//{
			//	return ((floPassionScore * floIncrementingReturn) * floMinorCost) + (floMinorCost * floBasePassionCost);
			//}
			//else if (_passion == Passion.Major && floPassionScore > 0f)
			//{
			//	return ((floPassionScore * floIncrementingReturn) * floMajorCost) + (floMajorCost * floBasePassionCost);
			//}


			//return (floIncrements * floIncrementingReturn * floPassionScore) + floPassionScore;


			if (_passion == Passion.Minor)
			{
				//return floMinorCost * floBasePassionCost;
				return (((floIncrements + 1) * floIncrementingReturn * (floPassionScore + floMinorCost)) + (floPassionScore + floMinorCost)) - GetPassionTotalCost(_pawn);
			}
			else if (_passion == Passion.Major)
			{
				//return floMajorCost * floBasePassionCost;
				return (((floIncrements + 1) * floIncrementingReturn * (floPassionScore + floMajorCost - floMinorCost)) + (floPassionScore + floMajorCost - floMinorCost)) - GetPassionTotalCost(_pawn);
			}
			else
			{
				return 0f;
			}
		}

		public static float GetPassionTotalCost(Pawn _pawn)
		{
			float floIncrements = 0f;
			float floPassionScore = 0;
			float floMinorCost = 50f;
			float floMajorCost = 150f;

			foreach (SkillRecord sk in _pawn.skills.skills)
			{
				//if (sk.passion == Passion.Minor && floPassionScore > 0f)
				////if (sk.passion == Passion.Minor)
				//{
				//floPassionScore += ((floPassionScore * floIncrementingReturn) * floMinorCost) + (floMinorCost * floBasePassionCost);
				//}
				//else if (sk.passion == Passion.Major && floPassionScore > 0f)
				//	//else if (sk.passion == Passion.Major)
				//{
				//floPassionScore += ((floPassionScore * floIncrementingReturn) * floMajorCost) + (floMajorCost * floBasePassionCost);
				//}
				//else if (sk.passion == Passion.Minor)
				//{
				//	floPassionScore += floMinorCost * floBasePassionCost;
				//}
				//else if (sk.passion == Passion.Major)
				//{
				//	floPassionScore += floMajorCost * floBasePassionCost;
				//}

				//if (sk.passion == Passion.Minor && floPassionScore > 0f)
				////if (sk.passion == Passion.Minor)
				//{
				//	floIncrements += 1f;
				//	floPassionScore += floMinorCost * floBasePassionCost;
				//}
				//else if (sk.passion == Passion.Major && floPassionScore > 0f)
				////else if (sk.passion == Passion.Major)
				//{
				//	floIncrements += 2f;
				//	floPassionScore += floMajorCost * floBasePassionCost;
				//}
				if (sk.passion == Passion.Minor)
				{
					floIncrements += 1f;
					floPassionScore += floMinorCost * floBasePassionCost;
				}
				else if (sk.passion == Passion.Major)
				{
					floIncrements += 2f;
					floPassionScore += floMajorCost * floBasePassionCost;
				}
			}

			//return floPassionScore;
			return (floIncrements * floIncrementingReturn * floPassionScore) + floPassionScore;
		}
	}
}
