using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using RimWorld;
using Harmony;

namespace Prophesy.PreGame
{
    public static class ProSM
    {
		/****************
		* Senario Maker *
		****************/
        private static Scenario scen;

        public static Scenario ScenWIP
        {
            get
            {
                return scen;
            }
        }

        public static Scenario GenScenSkeleton(string _name)
        {
            // ***************** //
            // Set Scenario Head //
            // ***************** //
            scen = new Scenario();
            scen.name = _name;
            scen.summary = "Test summary";
            scen.description = "Test description";
            scen.Category = ScenarioCategory.CustomLocal;

            //Instantiate and inject player faction
            ScenPart_PlayerFaction f = new ScenPart_PlayerFaction();
            Traverse.Create(f).Field("factionDef").SetValue(FactionDefOf.PlayerColony);
            Traverse.Create(scen).Field("playerFaction").SetValue(f);

            // ***************** //
            // Set Scenario Body //
            // ***************** //

            //List for Scenario.parts
            List<ScenPart> p = new List<ScenPart>(); 

            //Pawn ConfigPage. Pawncount should be 3 already.
            ScenPart_ConfigPage_ConfigureStartingPawns SPCPCSP = new ScenPart_ConfigPage_ConfigureStartingPawns();
            SPCPCSP.def = ScenPartDefOf.ConfigPage_ConfigureStartingPawns;

            //Pawn Arrival method
            ScenPart_PlayerPawnsArriveMethod SPPPAM = new ScenPart_PlayerPawnsArriveMethod();
            SPPPAM.def = ScenPartDefOf.PlayerPawnsArriveMethod;
            Traverse.Create(SPPPAM).Field("method").SetValue(PlayerPawnsArriveMethod.Standing);
            SPPPAM.visible = false;

            //Add ScenParts into List
            p.Add(SPCPCSP);
            p.Add(SPPPAM);

            //Inject List into Scenario.parts
            Traverse.Create(scen).Field("parts").SetValue(p);

            return scen;
        }

    }
}
