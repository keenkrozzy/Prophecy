using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

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

			SettleFoodValues();
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

		private static void SettleFoodValues()
		{
			ThingDef[] aTDs = (from k in DefDatabase<ThingDef>.AllDefs.Where(x => x.category == ThingCategory.Item) select k).ToArray();
			List<CompProperties> lCPs;
			CompProperties_Rottable CPR;
			float floNutri;
			FoodPreferability eFoodPref;
			float floFoodPref;
			float floDaysTillRot;
			float floNewPrice;
			string strLog = PadString("NAME", 30) + PadString("NUTRITION", 12) + PadString("PREFERENCE", 20) + PadString("DAYSTOROT", 12) + PadString("OLD", 8) + PadString("NEW", 8) + "\n";

			foreach (ThingDef td in aTDs)
			{
				floDaysTillRot = 100f;
				floNutri = 0f;
				eFoodPref = FoodPreferability.Undefined;
				floFoodPref = 1f;
				floNewPrice = 999f;

				if (td.IsNutritionGivingIngestible)
				{
					floNutri = td.ingestible.nutrition;
					eFoodPref = td.ingestible.preferability;
					strLog = string.Format("{0}{1}{2}{3}", strLog, PadString(td.defName, 30), PadString(floNutri.ToString(), 12), PadString(eFoodPref.ToString(), 20));
					try
					{
						if ((td.comps != null) && (td.comps.Where(x => x.compClass == typeof(CompRottable)).Count() > 0))
						{
							lCPs = td.comps.Where(x => x.compClass == typeof(CompRottable)).ToList();
							CPR = (CompProperties_Rottable)lCPs.ElementAt(0);
							floDaysTillRot = CPR.daysToRotStart;
							strLog = string.Format("{0}{1}", strLog, PadString(floDaysTillRot.ToString(), 12));
						}
						else
						{
							strLog = string.Format("{0}{1}", strLog, PadString("n/a", 12));
						}
					}
					catch (Exception e)
					{
						Log.Message(string.Format("EXCEPTION! {0}.{1} \n\tMESSAGE: {2} \n\tException occurred calling {3} method", e.TargetSite.ReflectedType.Name,
							e.TargetSite.Name, e.Message, Prophecy.Meta.KrozzyUtilities.GetCallForExceptionThisMethod(System.Reflection.MethodBase.GetCurrentMethod(), e)));

						strLog = string.Format("{0}{1}", strLog, PadString("error", 12));
					}

					strLog = string.Format("{0}{1}", strLog, PadString(String.Format("{0:0.00}", td.BaseMarketValue), 8));

					switch (eFoodPref)
					{
						case FoodPreferability.Undefined:
							floFoodPref = 1f;
							break;
						case FoodPreferability.NeverForNutrition:
							floFoodPref = .5f;
							break;
						case FoodPreferability.DesperateOnly:
							floFoodPref = .25f;
							break;
						case FoodPreferability.RawBad:
							floFoodPref = 1f;
							break;
						case FoodPreferability.RawTasty:
							floFoodPref = 1.5f;
							break;
						case FoodPreferability.MealAwful:
							floFoodPref = .5f;
							break;
						case FoodPreferability.MealSimple:
							floFoodPref = 1f;
							break;
						case FoodPreferability.MealFine:
							floFoodPref = 2f;
							break;
						case FoodPreferability.MealLavish:
							floFoodPref = 3f;
							break;
						default:
							floFoodPref = 1f;
							break;
					}

					switch (td.defName)
					{
						case "RawHops":
							floNutri = floNutri * 2f;
							break;
						case "PsychoidLeaves":
							floNutri = floNutri * 5f;
							break;
						case "SmokeleafLeaves":
							floNutri = floNutri * 3f;
							break;
					}

					if ((td.comps != null) && (td.comps.Where(x => x.compClass == typeof(CompHatcher)).Count() > 0))
					{
						// set price to animal base value
					}
					else
					{
						if (td.ingestible.joy > 0)
						{
							floNutri = floNutri + (td.ingestible.joy * 3f);
						}

						floDaysTillRot = floDaysTillRot * .25f;
						if (floDaysTillRot < 1f)
						{
							floDaysTillRot = 1f;
						}

						floNutri *= 1.5f;

						floNewPrice = 1f + (floNutri * floDaysTillRot * floFoodPref);
					}

					td.BaseMarketValue = floNewPrice;

					strLog = string.Format("{0}{1}\n", strLog, PadString(String.Format("{0:0.00}", floNewPrice), 8));
				}
			}

			//
			// FOR DEVELOPMENT DON'T DELETE!
			//
			//Log.Message(strLog);
			//
		}

		static string PadString(string _s, int l)
		{
			string strPadded = "";
			if (_s.Length < l)
			{
				strPadded = _s.PadRight(l, ' ');
			}
			else
			{
				strPadded = _s;
			}

			return strPadded;
		}
	}
}
