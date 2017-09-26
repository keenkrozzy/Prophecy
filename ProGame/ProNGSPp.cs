using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Harmony;
using Verse;
using RimWorld;

namespace Prophecy.ProGame
{
	[StaticConstructorOnStartup]
	internal static	class ProNGSPp
	{
		/*********************************************************
		* Patch for StartingPawnUtility.NewGeneratedStartingPawn *
		* line 68 to use ProPawnGenerator instead of vanilla.    *
		*********************************************************/

		static ProNGSPp()
		{
			HarmonyInstance NewGeneratedStartingPawnPatch = HarmonyInstance.Create("com.Prophesy.StartingPawnUtility.NewGeneratedStartingPawnPatch");
			MethodInfo methInfNewGeneratedStartingPawn = AccessTools.Method(typeof(StartingPawnUtility), "NewGeneratedStartingPawn", null, null);
			HarmonyMethod harmonyMethodPreFNewGeneratedStartingPawn = new HarmonyMethod(typeof(ProNGSPp).GetMethod("PreFNewGeneratedStartingPawn"));
			NewGeneratedStartingPawnPatch.Patch(methInfNewGeneratedStartingPawn, harmonyMethodPreFNewGeneratedStartingPawn, null, null);
			Log.Message("NewGeneratedStartingPawnPatch initialized");
		}

		public static bool PreFNewGeneratedStartingPawn(ref Pawn __result)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer, PawnGenerationContext.PlayerStarter, -1, true, false, false, false, true, false, 26f, false, true, true, false, false, null, null, null, null, null, null);
			Pawn pawn = null;
			try
			{
				pawn = ProPawnGenerator.GeneratePawn(request);
			}
			catch (Exception arg)
			{
				Log.Error("There was an exception thrown by the PawnGenerator during generating a starting pawn. Trying one more time...\nException: " + arg);
				pawn = PawnGenerator.GeneratePawn(request);
			}
			pawn.relations.everSeenByPlayer = true;
			PawnComponentsUtility.AddComponentsForSpawn(pawn);
			__result = pawn;
			return false;
		}
	}
}
