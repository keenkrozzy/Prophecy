using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Prophecy.ProGame
{
    public class ProGenStep_ScatterThings : GenStep_Scatterer
    {
        private const int ClusterRadius = 4;

        private static List<Rot4> tmpRotations = new List<Rot4>();

        public int radius = 4;

        [Unsaved]
        private int leftInCluster;

        [Unsaved]
        private IntVec3 clusterCenter;

        [NoTranslate]
        private List<string> terrainValidationDisallowed = null;

        private List<Rot4> possibleRotationsInt;

        public int clusterSize = 1;

        public int clearSpaceSize;

        public ThingDef stuff;

        public ThingDef thingDef;

        public float terrainValidationRadius;

        private List<Rot4> PossibleRotations
        {
            get
            {
                if (possibleRotationsInt == null)
                {
                    possibleRotationsInt = new List<Rot4>();
                    if (thingDef.rotatable)
                    {
                        possibleRotationsInt.Add(Rot4.North);
                        possibleRotationsInt.Add(Rot4.East);
                        possibleRotationsInt.Add(Rot4.South);
                        possibleRotationsInt.Add(Rot4.West);
                    }
                    else
                    {
                        possibleRotationsInt.Add(Rot4.North);
                    }
                }
                return possibleRotationsInt;
            }
        }

        public static List<int> CountDividedIntoStacks(int count, IntRange stackSizeRange)
        {
            List<int> list = new List<int>();
            while (count > 0)
            {
                int num = Mathf.Min(count, stackSizeRange.RandomInRange);
                count -= num;
                list.Add(num);
            }
            if (stackSizeRange.max > 2)
            {
                for (int i = 0; i < list.Count * 4; i++)
                {
                    int num2 = Rand.RangeInclusive(0, list.Count - 1);
                    int num3 = Rand.RangeInclusive(0, list.Count - 1);
                    if (num2 != num3 && list[num2] > list[num3])
                    {
                        int num4 = (int)((float)(list[num2] - list[num3]) * Rand.Value);
                        List<int> list2;
                        int index;
                        int num5 = (list2 = list)[index = num2];
                        list2[index] = num5 - num4;
                        List<int> list3;
                        int index2;
                        num5 = (list3 = list)[index2 = num3];
                        list3[index2] = num5 + num4;
                    }
                }
            }
            return list;
        }

        protected override bool CanScatterAt(IntVec3 loc, Map map)
        {
            if (!base.CanScatterAt(loc, map))
            {
                return false;
            }
			if (!TryGetRandomValidRotation(loc, map, out Rot4 rot))
			{
				return false;
			}
			if (terrainValidationRadius > 0f)
            {
                foreach (IntVec3 current in GenRadial.RadialCellsAround(loc, terrainValidationRadius, true))
                {
                    if (GenGrid.InBounds(current, map))
                    {
                        TerrainDef terrain = GridsUtility.GetTerrain(current, map);
                        for (int i = 0; i < terrainValidationDisallowed.Count; i++)
                        {
                            if (terrain.HasTag(terrainValidationDisallowed[i]))
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return true;
        }

        public override void Generate(Map map)
        {
            if (!allowOnWater && map.TileInfo.WaterCovered)
            {
                return;
            }
            int intFinalCount = CalculateFinalCount(map);
			//Log.Message(string.Concat(thingDef.label, " Count: ", count.ToString(), " intFinalCount: ", intFinalCount.ToString()));
            IntRange one;
            if (thingDef.ingestible != null && thingDef.ingestible.IsMeal && thingDef.stackLimit <= 10)
            {
                one = IntRange.one;
            }
            else if (thingDef.stackLimit > 5)
            {
                one = new IntRange(Mathf.RoundToInt((float)thingDef.stackLimit * 0.5f), thingDef.stackLimit);
            }
            else
            {
                one = new IntRange(thingDef.stackLimit, thingDef.stackLimit);
            }
            List<int> list = CountDividedIntoStacks(intFinalCount, one);
            for (int i = 0; i < list.Count; i++)
            {
                IntVec3 intVec = new IntVec3();
                if (!TryFindScatterCell(map, out intVec))
                {
                    return;
                }
                ScatterAt(intVec, map, list[i]);
                usedSpots.Add(intVec);
            }
            usedSpots.Clear();
            clusterCenter = IntVec3.Invalid;
            leftInCluster = 0;
        }

        private bool IsRotationValid(IntVec3 loc, Rot4 rot, Map map)
        {
            return GenAdj.OccupiedRect(loc, rot, thingDef.size).InBounds(map) && !GenSpawn.WouldWipeAnythingWith(loc, rot, thingDef, map, (Thing x) => x.def == thingDef || (x.def.category != ThingCategory.Plant && x.def.category != ThingCategory.Filth));
        }

        protected override void ScatterAt(IntVec3 loc, Map map, int stackCount = 1)
        {
			if (!TryGetRandomValidRotation(loc, map, out Rot4 rot))
			{
				Log.Warning("Could not find any valid rotation for " + thingDef);
				return;
			}
			if (clearSpaceSize > 0)
            {
                using (IEnumerator<IntVec3> enumerator = GridShapeMaker.IrregularLump(loc, map, clearSpaceSize).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Building edifice = GridsUtility.GetEdifice(enumerator.Current, map);
                        if (edifice != null)
                        {
                            edifice.Destroy(0);
                        }
                    }
                }
            }
            Thing thing = ThingMaker.MakeThing(thingDef, stuff);
            if (thingDef.Minifiable)
            {
                thing = MinifyUtility.MakeMinified(thing);
            }
            if (thing.def.category == ThingCategory.Item)
            {
                thing.stackCount = stackCount;
                ForbidUtility.SetForbidden(thing, true, false);
				GenPlace.TryPlaceThing(thing, loc, map, ThingPlaceMode.Near, out Thing thing2, null);
				if (nearPlayerStart && thing2 != null && thing2.def.category == ThingCategory.Item && TutorSystem.TutorialMode)
                {
                    Find.TutorialState.AddStartingItem(thing2);
                    return;
                }
            }
            else
            {
                GenSpawn.Spawn(thing, loc, map, rot, false);
            }
        }

        protected override bool TryFindScatterCell(Map map, out IntVec3 result)
        {
            if (clusterSize > 1)
            {
                if (leftInCluster <= 0)
                {
                    if (!base.TryFindScatterCell(map, out clusterCenter))
                    {
                        Log.Error("Could not find cluster center to scatter " + thingDef);
                    }
                    leftInCluster = clusterSize;
                }
                leftInCluster--;
                result = CellFinder.RandomClosewalkCellNear(clusterCenter, map, radius, delegate (IntVec3 x)
                {
					return TryGetRandomValidRotation(x, map, out Rot4 rot);
				});
                return result.IsValid;
            }
            return base.TryFindScatterCell(map, out result);
        }

        private bool TryGetRandomValidRotation(IntVec3 loc, Map map, out Rot4 rot)
        {
            List<Rot4> possibleRotations = PossibleRotations;
            for (int i = 0; i < possibleRotations.Count; i++)
            {
                if (IsRotationValid(loc, possibleRotations[i], map))
                {
                    tmpRotations.Add(possibleRotations[i]);
                }
            }
            if (GenCollection.TryRandomElement<Rot4>(tmpRotations, out rot))
            {
                tmpRotations.Clear();
                return true;
            }
            rot = Rot4.Invalid;
            return false;
        }
    }
}
