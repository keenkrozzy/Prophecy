using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Prophecy.ProGame
{
	[StaticConstructorOnStartup]
	public class ProScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
    {
        protected int radius = 4;

        public ThingDef ThingDef
        {
            get
            {
                return thingDef;
            }
            set
            {
                thingDef = value;
            }
        }

        public ThingDef StuffDef
        {
            get
            {
                return stuff;
            }
            set
            {
                stuff = value;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }

        protected override bool NearPlayerStart
        {
            get
            {
                return true;
            }
        }

        public int Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        public ProScenPart_ScatterThingsNearPlayerStart()
        {
            def = ScenPartDefOf.ScatterThingsNearPlayerStart;
        }

        public override void GenerateIntoMap(Map map)
        {
            if (Find.GameInitData == null)
            {
                return;
            }
            new ProGenStep_ScatterThings
            {
                nearPlayerStart = NearPlayerStart,
                thingDef = thingDef,
                stuff = stuff,
                count = count,
                spotMustBeStandable = true,
                minSpacing = 5f,
                clusterSize = ((thingDef.category != ThingCategory.Building) ? 4 : 1),
                radius = 4  + radius
            }.Generate(map);
        }

        public override string Summary(Scenario scen)
        {
            return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
        }

        public override IEnumerable<string> GetSummaryListEntries(string tag)
        {
            if (tag == "PlayerStartsWith")
            {
                return new List<string>
                {
                    GenText.CapitalizeFirst(GenLabel.ThingLabel(thingDef, stuff, count))
                };
            }
            return Enumerable.Empty<string>();
        }
    }
}
