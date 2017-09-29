using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection;
using Harmony;

namespace Prophecy.ProGame
{
	/***************************
	* Patching Class Backstory *
	***************************/
	[StaticConstructorOnStartup]
	internal static class ProBackstoryp
	{
		static ProBackstoryp()
		{
			/***************************************************************************************************************************************
			* ResolveReferences patch: Don't add to skillGains if the value is 0. In Prophecy.ProGame.Elements.ProBackstories, every skill gain is *
			* copy-pasted with default values of 0 to make entering backstories esaier.
			***************************************************************************************************************************************/
			HarmonyInstance ResolveReferencesPatch = HarmonyInstance.Create("com.Prophesy.Backstory.ResolveReferences");
			MethodInfo methInfResolveReferences = AccessTools.Method(typeof(Backstory), "ResolveReferences", null, null);
			HarmonyMethod harmonyMethodPreFResolveReferences = new HarmonyMethod(typeof(ProBackstoryp).GetMethod("PrefResolveReferences"));
			ResolveReferencesPatch.Patch(methInfResolveReferences, harmonyMethodPreFResolveReferences, null, null);
			Log.Message("Backstory ResolveReferencesPatch initialized");
		}

		public static bool PrefResolveReferences(Backstory __instance)
		{
			int num = Mathf.Abs(GenText.StableStringHash(__instance.baseDesc) % 100);
			string s = __instance.Title.Replace('-', ' ');
			s = GenText.CapitalizedNoSpaces(s);
			__instance.identifier = GenText.RemoveNonAlphanumeric(s) + num.ToString();
			foreach (KeyValuePair<string, int> keyValuePair in __instance.skillGains)
			{
				if (keyValuePair.Value != 0) // Changed line.
				{
					__instance.skillGainsResolved.Add(DefDatabase<SkillDef>.GetNamed(keyValuePair.Key, true), keyValuePair.Value);
				}
			}
			__instance.skillGains = null;

			return false; 
		}
	}
}
