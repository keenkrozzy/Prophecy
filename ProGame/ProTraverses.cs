using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Harmony;
using Verse;

namespace Prophecy.ProGame
{
	public static class ProTraverses
	{
		public static Traverse travGenerateRandomOldAgeInjuries;

		static ProTraverses()
		{
			travGenerateRandomOldAgeInjuries = Traverse.CreateWithType("AgeInjuryUtility").Method("GenerateRandomOldAgeInjuries", new Type[] { typeof(Pawn), typeof(PawnGenerationRequest) }, null);
		}
	}
}
