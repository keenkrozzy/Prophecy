using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Prophecy.PreGame
{
    public class ESApparel
    {
        public ESItem[] aApparel = new ESItem[0];
		BiomeDef biome = new BiomeDef();
		BiomeDef[] abiomes = DefDatabase<BiomeDef>.AllDefs.ToArray();

		public ESApparel(BiomeDef _biome)
        {

			biome = _biome;

			foreach (BiomeDef bd in abiomes)
			{
				if (bd == biome)
				{
					aApparel = aApparel.Concat(GetLeatherApparel(biome, "Apparel_TribalA", false)).Concat(GetLeatherApparel(biome, "Apparel_Parka", false)).Concat(
						GetLeatherApparel(biome, "Apparel_Tuque", false)).Concat(GetWoolApparel(biome, "Apparel_TribalA", false)).Concat(
						GetWoolApparel(biome, "Apparel_Parka", false)).Concat(GetWoolApparel(biome, "Apparel_Tuque", false)).ToArray();
				}
			}
		}

		private ESItem[] GetLeatherApparel(BiomeDef _biome, string _ApparelDefName, bool _LogCalculation)
		{
			
			// Variables
			bool boolLogCalculation = _LogCalculation;
			PawnKindDef[] aAnimals = new PawnKindDef[0];
			aAnimals = aAnimals.Concat(_biome.AllWildAnimals).ToArray();
			PawnKindDef[] aAllAnimals = (from k in DefDatabase<PawnKindDef>.AllDefs where (k.RaceProps.Animal) select k).ToArray();
			PawnKindDef[] aTameAnimals = (from a in aAllAnimals where (a.race.race.wildness < .1f) select a).ToArray();
			PawnKindDef[] aNoLeatherAnimals = (from b in aAllAnimals where (b.race.race.leatherDef == null) select b).ToArray();
			
			if (aAnimals.Contains(PawnKindDef.Named("WildBoar")))
			{
				aTameAnimals = aTameAnimals.Where(x => x != PawnKindDef.Named("Pig")).ToArray();
			}

			aTameAnimals = aTameAnimals.Where(x => x != PawnKindDef.Named("YorkshireTerrier") && x != PawnKindDef.Named("Husky") &&
				x != PawnKindDef.Named("LabradorRetriever")).ToArray();
			aAnimals = aAnimals.Union(aTameAnimals).ToArray();
			aAnimals = aAnimals.Where(x => !aNoLeatherAnimals.Contains(x)).ToArray();
			aAnimals = aAnimals.OrderBy(pkd => pkd.defName).ToArray();
			ESItem[] aESI = new ESItem[aAnimals.Length];
			string strThingDef = _ApparelDefName;

			// Variables for logging
			string strAnimalCommonality = "";
			string strAnimalLeather = "";
			string strAnimalBodySize = "";
			string strAnimalDanger = "";
			string strAnimalLeatherComonality = "";
			string strAnimalLMVF = "";
			string strCostPerLeather = "";
			string strFinal = "";
			string[] aStrings = new string[0];

			// Start master loop for each animal in biome
			for (int i = 0; i < aAnimals.Count(); i++)
			{
				try
				{
					// variables
					int intThingAmount = 1;
					float floBasePrice = 0f;
					string strStuff;
					string strAnimalName = aAnimals[i].defName;
					float floAnimalDPS = 0f;
					float floAnimalCommonality = 0f;

					// variables for tweeking
					// Scale opens price gaps
					float floHealthScale = 1f; // drives prices up
					float floDangerScale = 1f; // drives prices up
					float floCommonalityScale = 1f; // drives prices down
					float floBodySizeScale = 1f; // drives prices down
					float floInsulationScale = 6f; // drives prices up

					// Floor closes price gaps
					float floHealthFloor = .4f;
					float floDangerFloor = 20f;
					float floCommonalityFloor = 1f;
					float floBodySizeFloor = 1f;
					float floInsulationFloor = 0f;

					float floPredatorScale = 4f;
					float floFarmScale = 1.5f;
					float floTotalScale = .1f;

					// Calc animal DPS
					if (!aAnimals[i].race.Verbs.NullOrEmpty<VerbProperties>())
					{
						VerbProperties[] aVerbs = aAnimals[i].race.Verbs.ToArray();

						for (int x = 0; x < aVerbs.Length; x++)
						{
							if ((float)aVerbs[x].meleeDamageBaseAmount > .001f)
							{
								floAnimalDPS += (float)aVerbs[x].meleeDamageBaseAmount / (aVerbs[x].defaultCooldownTime + aVerbs[x].warmupTime);
							}
						}

						floAnimalDPS = floAnimalDPS / (float)aVerbs.Length;
					}

					// Calc animal danger with baseHealthScale scale and floor
					float floAnimalDanger = floAnimalDPS * aAnimals[i].race.GetStatValueAbstract(StatDefOf.MoveSpeed) * (aAnimals[i].race.race.baseHealthScale * floHealthScale + floHealthFloor);

					// if Boomrat or Boomalope, add more danger
					if (aAnimals[i].defName == "Boomrat" || aAnimals[i].defName == "Boomalope")
					{
						floAnimalDanger += 50f;
					}

					// Add danger scale and floor
					floAnimalDanger = floAnimalDanger * floDangerScale + floDangerFloor;

					// Calc animal commonality, add animal commonality scale and floor, multiply for predators and farm types
					if (aAnimals[i].race.race.predator == true)
					{
						floAnimalCommonality = _biome.CommonalityOfAnimal(aAnimals[i]) * floPredatorScale * floCommonalityScale * _biome.animalDensity;
					}
					else if (aAnimals[i].defName == "Chicken" || aAnimals[i].defName == "Pig" || aAnimals[i].defName == "WildBoar" || aAnimals[i].defName == "Cow" || aAnimals[i].defName == "Alpaca")
					{
						floAnimalCommonality = _biome.CommonalityOfAnimal(aAnimals[i]) * floFarmScale * floCommonalityScale  * _biome.animalDensity;
						//if (floAnimalCommonality < .1f)
						//{
						//	floAnimalCommonality = floCommonalityFloor * .5f;
						//}
					}
					else
					{
						floAnimalCommonality = _biome.CommonalityOfAnimal(aAnimals[i]) * floCommonalityScale * _biome.animalDensity;
					}
					floAnimalCommonality += floCommonalityFloor;

					// Calc animal leather commonality with baseBodySize scale and floor
					float floAnimalLeatherCommonality = (floAnimalCommonality * (aAnimals[i].race.race.baseBodySize * floBodySizeScale + floBodySizeFloor)) / floAnimalDanger;

					// Logging part 1
					if (boolLogCalculation)
					{
						strAnimalCommonality = string.Concat("Commonality of ", aAnimals[i].ToString(), " * Animal Density: ", floAnimalCommonality.ToString(), "\n");
						strAnimalLeather = string.Concat(aAnimals[i].race.race.leatherDef.ToString(), ", Insulation: ", aAnimals[i].race.race.leatherInsulation.ToString(), "\n");
						strAnimalBodySize = string.Concat("BodySize: ", aAnimals[i].race.race.baseBodySize.ToString(), "\n");
						strAnimalDanger = string.Concat("AnimalDPS * AnimalMoveSpeed * AnimalBaseHealthScale = Danger: ", floAnimalDanger.ToString(), "\n");
						strAnimalLeatherComonality = string.Concat("Animal Commonality * BodySize / Danger = LeatherCommonality: ", floAnimalLeatherCommonality.ToString(), "\n");
						strAnimalLMVF = string.Concat("leatherMarketValueFactor: ", aAnimals[i].race.race.leatherMarketValueFactor.ToString(), "\n");
						strCostPerLeather = string.Concat("leatherInsulation / LeatherCommonality * leatherMarketValueFactor = Cost per leather: ");
					}

					// Handle WildBoar because it uses Pig leather, add leatherInsulation scale and floor
					if (strAnimalName == "WildBoar")
					{
						strAnimalName = "Pig";
						floBasePrice = ((aAnimals[i].race.race.leatherInsulation * floInsulationScale + floInsulationFloor) / floAnimalLeatherCommonality) * PawnKindDef.Named("Pig").race.race.leatherMarketValueFactor;
					}
					else
					{
						floBasePrice = ((aAnimals[i].race.race.leatherInsulation * floInsulationScale + floInsulationFloor) / floAnimalLeatherCommonality) * aAnimals[i].race.race.leatherMarketValueFactor;
					}

					// Add total scale
					floBasePrice *= floTotalScale;

					// Calc ThingDef string for Stuff, calc leather needed for apparel
					strStuff = string.Concat(strAnimalName, "_Leather");
					float floFinalCost = (float)ThingDef.Named(strThingDef).costStuffCount * floBasePrice;

					// Populate ESItem list for return
					aESI[i] = new ESItem(strThingDef, intThingAmount, floFinalCost, strStuff);

					// Logging part 2
					if (boolLogCalculation)
					{
						strFinal = string.Concat(aAnimals[i].ToString(), " FINAL: ", floBasePrice.ToString(), "\n", strThingDef, ": ", floFinalCost.ToString());
						aStrings = new string[] { strAnimalCommonality, strAnimalLeather, strAnimalBodySize, strAnimalDanger, strAnimalLeatherComonality, strAnimalLMVF, strCostPerLeather, strFinal };
						Log.Message(string.Concat(aStrings));
					}
				}
				catch
				{
					aESI[i] = new ESItem("Apparel_Tuque", 1, 9001, "Cloth");
					Log.Message(string.Concat(aAnimals[i].defName, " FAILED ESApparel.GetLeatherApparel."));
				}
			}

			return aESI;
		}

		private ESItem[] GetWoolApparel(BiomeDef _biome, string _ApparelDefName, bool _LogCalculation)
		{
			// Variables
			bool boolLogCalculation = _LogCalculation;
			PawnKindDef[] aBiomeAnimals = new PawnKindDef[0];
			aBiomeAnimals = aBiomeAnimals.Concat(_biome.AllWildAnimals).ToArray();
			string strThingDef = _ApparelDefName;
			PawnKindDef[] aAllAnimals = (from k in DefDatabase<PawnKindDef>.AllDefs where (k.RaceProps.Animal) select k).ToArray();
			PawnKindDef[] aWoolAnimals = (from a in aAllAnimals where a.race.HasComp(typeof(CompShearable)) select a).ToArray();
			PawnKindDef[] aTameAnimals = (from a in aAllAnimals where (a.race.race.wildness < .1f) select a).ToArray();
			PawnKindDef[] aTameWoolAnimals = aWoolAnimals.Intersect(aTameAnimals).ToArray();
			PawnKindDef[] aBiomeWoolAnimals = aBiomeAnimals.Intersect(aWoolAnimals).ToArray();
			PawnKindDef[] aBiomeAndTameWoolAnimals = aTameWoolAnimals.Union(aBiomeWoolAnimals).ToArray();
			PawnKindDef[] aAnimals = aBiomeAndTameWoolAnimals;
			ESItem[] aESI = new ESItem[aAnimals.Length];

			// Logging for variables
			if (boolLogCalculation == true)
			{
				string[] aStr = new string[] { "All tame animals: \n" };
				foreach (PawnKindDef pkd in aTameAnimals)
				{
					aStr = aStr.Concat(new string[] { pkd.defName, "\n" }).ToArray();
				}
				Log.Message(string.Concat(aStr));

				string[] aStr2 = new string[] { "All wool producing animals: \n" };
				foreach (PawnKindDef pkd in aWoolAnimals)
				{
					aStr2 = aStr2.Concat(new string[] { pkd.defName, "\n" }).ToArray();
				}
				Log.Message(string.Concat(aStr2));

				string[] aStr3 = new string[] { "All tame animals that produce wool: \n" };
				foreach (PawnKindDef pkd in aTameWoolAnimals)
				{
					aStr3 = aStr3.Concat(new string[] { pkd.defName, "\n" }).ToArray();
				}
				Log.Message(string.Concat(aStr3));

				string[] aStr4 = new string[] { "All animals that produce wool in this biome: \n" };
				foreach (PawnKindDef pkd in aBiomeWoolAnimals)
				{
					aStr4 = aStr4.Concat(new string[] { pkd.defName, "\n" }).ToArray();
				}
				Log.Message(string.Concat(aStr4));

				string[] aStr5 = new string[] { "All Tame and Biome wool producing animals: \n" };
				foreach (PawnKindDef pkd in aAnimals)
				{
					aStr5 = aStr5.Concat(new string[] { pkd.defName, "\n" }).ToArray();
				}
				Log.Message(string.Concat(aStr5));
			}

			// Variables for logging
			string strAnimalCommonality = "";
			string strAnimalWool = "";
			string strAnimalBodySize = "";
			string strAnimalDanger = "";
			string strAnimalwoolComonality = "";
			string strAnimalWMVF = "";
			string strCostPerWool = "";
			string strFinal = "";
			string[] aStrings = new string[0];

			// Start master loop for each wool producing animal in biome
			for (int i = 0; i < aAnimals.Count(); i++)
			{
				// variables
				int intThingAmount = 1;
				float floBasePrice = 0f;
				string strStuff;
				string strAnimalName = aAnimals[i].defName;
				float floAnimalDPS = 0f;
				float floAnimalCommonality = 0f;
				string strAnimalWoolDef = aAnimals[i].race.GetCompProperties<CompProperties_Shearable>().woolDef.defName;
				StatModifier[] asmStatOffsets = ThingDef.Named(strAnimalWoolDef).stuffProps.statOffsets.ToArray();
				StatModifier[] asmStatFactors = ThingDef.Named(strAnimalWoolDef).stuffProps.statFactors.ToArray();
				(from a in aAllAnimals where a.race.HasComp(typeof(CompShearable)) select a).ToArray();

				// variables for tweeking
				// Scale opens price gaps
				float floHealthScale = 1f; // drives prices up
				float floDangerScale = 1f; // drives prices up
				float floCommonalityScale = 1f; // drives prices down
				float floBodySizeScale = 1f; // drives prices down
				float floInsulationScale = 6f; // drives prices up

				// Floor closes price gaps
				float floHealthFloor = .4f;
				float floDangerFloor = 20f;
				float floCommonalityFloor = 1f;
				float floBodySizeFloor = 1f;
				float floInsulationFloor = 0f;

				float floPredatorScale = 4f;
				float floFarmScale = 1.5f;
				float floTotalScale = .05f;

				// Calc animal DPS
				if (!aAnimals[i].race.Verbs.NullOrEmpty<VerbProperties>())
				{
					VerbProperties[] aVerbs = aAnimals[i].race.Verbs.ToArray();

					for (int x = 0; x < aVerbs.Length; x++)
					{
						if ((float)aVerbs[x].meleeDamageBaseAmount > .001f)
						{
							floAnimalDPS += (float)aVerbs[x].meleeDamageBaseAmount / (aVerbs[x].defaultCooldownTime + aVerbs[x].warmupTime);
						}
					}

					floAnimalDPS = floAnimalDPS / (float)aVerbs.Length;
				}

				// Calc animal danger with baseHealthScale scale and floor
				float floAnimalDanger = floAnimalDPS * aAnimals[i].race.GetStatValueAbstract(StatDefOf.MoveSpeed) * (aAnimals[i].race.race.baseHealthScale * floHealthScale + floHealthFloor);

				// Add danger scale and floor
				floAnimalDanger = floAnimalDanger * floDangerScale + floDangerFloor;

				// Calc animal commonality, multiply for predators and farm types
				if (aAnimals[i].race.race.predator == true)
				{
					floAnimalCommonality = _biome.CommonalityOfAnimal(aAnimals[i]) * floPredatorScale * floCommonalityScale * _biome.animalDensity;
				}
				else if (aTameAnimals.Contains( aAnimals[i]))
				{
					floAnimalCommonality = _biome.CommonalityOfAnimal(aAnimals[i]) * floFarmScale * floCommonalityScale * _biome.animalDensity;
					floAnimalDanger /= floFarmScale;
				}
				else
				{
					floAnimalCommonality = _biome.CommonalityOfAnimal(aAnimals[i]) * floCommonalityScale * _biome.animalDensity;
				}

				// Add animal commonality scale and floor
				floAnimalCommonality += floCommonalityFloor;

				// Calc animal Wool commonality with baseBodySize scale and floor
				float floAnimalWoolCommonality = (floAnimalCommonality  * ((aAnimals[i].race.race.baseBodySize * 3f) * floBodySizeScale + floBodySizeFloor)) / floAnimalDanger;

				// Calc total statOffsets
				float floStatOffsetsTotal = 0f;
				foreach (StatModifier sm in asmStatOffsets)
				{
					floStatOffsetsTotal += sm.value;
				}

				// Calc total statFactors
				float floStatFactorsTotal = 0f;
				foreach (StatModifier sf in asmStatFactors)
				{
					floStatFactorsTotal += (sf.value * .75f);
				}

				//Add woolInsulation scale and floor
				floBasePrice = ((floStatFactorsTotal * floInsulationScale + floInsulationFloor) + floStatOffsetsTotal) / floAnimalWoolCommonality;

				// Logging part 1
				if (boolLogCalculation)
				{
					strAnimalCommonality = string.Concat("Commonality of ", aAnimals[i].ToString(), " * Animal Density: ", floAnimalCommonality.ToString(), "\n");
					strAnimalWool = string.Concat(aAnimals[i].race.race.leatherDef.ToString(), ", statFactors: ", floStatFactorsTotal.ToString(), "\n");
					strAnimalBodySize = string.Concat("BodySize: ", aAnimals[i].race.race.baseBodySize.ToString(), "\n");
					strAnimalDanger = string.Concat("AnimalDPS * AnimalMoveSpeed * AnimalBaseHealthScale = Danger: ", floAnimalDanger.ToString(), "\n");
					strAnimalwoolComonality = string.Concat("Animal Commonality * BodySize / Danger = woolCommonality: ", floAnimalWoolCommonality.ToString(), "\n");
					strAnimalWMVF = string.Concat("woolMarketValueFactor: ", aAnimals[i].race.race.leatherMarketValueFactor.ToString(), "\n");
					strCostPerWool = string.Concat("statFactors / woolCommonality = Cost per leather: ");
				}

				// Add total scale
				floBasePrice *= floTotalScale;

				// Calc ThingDef string for Stuff, calc wool needed for apparel
				strStuff = strAnimalWoolDef;
				float floFinalCost = (float)ThingDef.Named(strThingDef).costStuffCount * floBasePrice;

				// Populate ESItem list for return
				aESI[i] = new ESItem(strThingDef, intThingAmount, floFinalCost, strStuff);

				// Logging part 2
				if (boolLogCalculation)
				{
					strFinal = string.Concat(aAnimals[i].ToString(), " FINAL: ", floBasePrice.ToString(), "\n", strThingDef, ": ", floFinalCost.ToString());
					aStrings = new string[] { strAnimalCommonality, strAnimalWool, strAnimalBodySize, strAnimalDanger, strAnimalwoolComonality, strAnimalWMVF, strCostPerWool, strFinal };
					Log.Message(string.Concat(aStrings));
				}
			}

			return aESI;
		}
	}
}
