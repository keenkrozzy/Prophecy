using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Prophecy.ProGame.Elements;

namespace Prophecy.ProGame
{
	[StaticConstructorOnStartup]
	public static class ProPawnBioAndNameGenerator
	{
		private static ProBackstories ProBS;

		private const float MinAgeForAdulthood = 20f;

		private const float SolidBioChance = 0.25f;

		private const float SolidNameChance = 0.5f;

		private const float TryPreferredNameChance_Bio = 0.5f;

		private const float TryPreferredNameChance_Name = 0.5f;

		private const float ShuffledNicknameChance = 0.15f;

		static ProPawnBioAndNameGenerator()
		{
			ProBS = new ProBackstories();
			
		}

		public static void GiveAppropriateBioAndNameTo(Pawn pawn, string requiredLastName)
		{
			/*************************************
			*DO NOT DELETE! MAY NEED THIS LATER!!*
			*************************************/
			//if ((Rand.Value < 0.25f || pawn.kindDef.factionLeader) && TryGiveSolidBioTo(pawn, requiredLastName))
			//{
			//	/*****Logging*****/
			//	Log.Message("((Rand.Value < 0.25f || pawn.kindDef.factionLeader) && TryGiveSolidBioTo(pawn, requiredLastName)) true. line 28");
			//	/*****Logging*****/
			//	return;
			//}
			GiveShuffledBioTo(pawn, pawn.Faction.def, requiredLastName);
		}

		private static void GiveShuffledBioTo(Pawn pawn, FactionDef factionType, string requiredLastName)
		{
			pawn.Name = GeneratePawnName(pawn, NameStyle.Full, requiredLastName);
			SetBackstoryInSlot(pawn, BackstorySlot.Childhood, ref pawn.story.childhood, factionType);
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 20f)
			{
				SetBackstoryInSlot(pawn, BackstorySlot.Adulthood, ref pawn.story.adulthood, factionType);
			}
		}

		private static void SetBackstoryInSlot(Pawn pawn, BackstorySlot slot, ref Backstory backstory, FactionDef factionType)
		{
			try
			{
				if (slot == BackstorySlot.Childhood && factionType == FactionDefOf.PlayerTribe)
				{
					//if (!(from bs in ProBackstories.aBSChildNeo where bs.shuffleable && bs.spawnCategories.Contains(factionType.backstoryCategory) &&
					//	  bs.slot == slot select bs).TryRandomElement(out backstory))
					//{
					//}
					try
					{
						ProBS.GetNeoChildStory(ref backstory);
					}
					catch (Exception e)
					{
						Log.Message(string.Format("EXCEPTION! {0}.{1} \n\tMESSAGE: {2} \n\tException occurred calling {3} method", e.TargetSite.ReflectedType.Name,
							e.TargetSite.Name, e.Message, Prophecy.Meta.KrozzyUtilities.GetCallForExceptionThisMethod(System.Reflection.MethodBase.GetCurrentMethod(), e)));
					}
				}
				else if (slot == BackstorySlot.Adulthood && factionType == FactionDefOf.PlayerTribe)
				{
					//if (!(from bs in ProBackstories.aBSAdultNeo where bs.shuffleable && bs.spawnCategories.Contains(factionType.backstoryCategory) &&
					//	  bs.slot == slot && !bs.requiredWorkTags.OverlapsWithOnAnyWorkType(pawn.story.childhood.workDisables) select bs).TryRandomElement(out backstory))
					//{
					//}
					try
					{
						ProBS.GetNeoAdultStory(ref backstory);
					}
					catch (Exception e)
					{
						Log.Message(string.Format("EXCEPTION! {0}.{1} \n\tMESSAGE: {2} \n\tException occurred calling {3} method", e.TargetSite.ReflectedType.Name,
							e.TargetSite.Name, e.Message, Prophecy.Meta.KrozzyUtilities.GetCallForExceptionThisMethod(System.Reflection.MethodBase.GetCurrentMethod(), e)));

					}
				}
			}
			catch(Exception e)
			{
				Log.Message("ProPawnBioAndNameGenerator.SetBackstoryInSlot failed. Using Vanilla. \n " + e.Message + " \n" + e.InnerException.Message + " \n" + e.TargetSite.Name);

				if (!(from kvp in BackstoryDatabase.allBackstories
					  where kvp.Value.shuffleable && kvp.Value.spawnCategories.Contains(factionType.backstoryCategory) && kvp.Value.slot == slot && (slot != BackstorySlot.Adulthood || !kvp.Value.requiredWorkTags.OverlapsWithOnAnyWorkType(pawn.story.childhood.workDisables))
					  select kvp.Value).TryRandomElement(out backstory))
				{
					Log.Error(string.Concat(new object[]
					{
					"No shuffled ",
					slot,
					" found for ",
					pawn,
					" of ",
					factionType,
					". Defaulting."
					}));
					backstory = (from kvp in BackstoryDatabase.allBackstories
								 where kvp.Value.slot == slot
								 select kvp).RandomElement<KeyValuePair<string, Backstory>>().Value;
				}
			}
		}

		private static bool TryGiveSolidBioTo(Pawn pawn, string requiredLastName)
		{
			PawnBio pawnBio = TryGetRandomUnusedSolidBioFor(pawn.Faction.def.backstoryCategory, pawn.kindDef, pawn.gender, requiredLastName);
			if (pawnBio == null)
			{
				return false;
			}
			if (pawnBio.name.First == "Tynan" && pawnBio.name.Last == "Sylvester" && Rand.Value < 0.5f)
			{
				pawnBio = TryGetRandomUnusedSolidBioFor(pawn.Faction.def.backstoryCategory, pawn.kindDef, pawn.gender, requiredLastName);
			}
			if (pawnBio == null)
			{
				return false;
			}
			pawn.Name = pawnBio.name;
			pawn.story.childhood = pawnBio.childhood;
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 20f)
			{
				pawn.story.adulthood = pawnBio.adulthood;
			}
			return true;
		}

		private static PawnBio TryGetRandomUnusedSolidBioFor(string backstoryCategory, PawnKindDef kind, Gender gender, string requiredLastName)
		{
			NameTriple prefName = null;
			SolidBioDatabase.allBios.Shuffle<PawnBio>();
			PawnBio pawnBio;
			while (true)
			{
				pawnBio = SolidBioDatabase.allBios.FirstOrDefault(delegate (PawnBio bio)
				{
					if (bio.gender != GenderPossibility.Either)
					{
						if (gender == Gender.Male && bio.gender != GenderPossibility.Male)
						{
							return false;
						}
						if (gender == Gender.Female && bio.gender != GenderPossibility.Female)
						{
							return false;
						}
					}
					return (requiredLastName.NullOrEmpty() || !(bio.name.Last != requiredLastName)) && (prefName == null || bio.name.Equals(prefName)) && (!kind.factionLeader || bio.pirateKing) && bio.adulthood.spawnCategories.Contains(backstoryCategory) && !bio.name.UsedThisGame;
				});
				if (pawnBio != null || prefName == null)
				{
					break;
				}
				prefName = null;
			}
			return pawnBio;
		}

		public static NameTriple TryGetRandomUnusedSolidName(Gender gender, string requiredLastName = null)
		{
			NameTriple nameTriple = null;
			if (Rand.Value < 0.5f)
			{
				nameTriple = Prefs.RandomPreferredName();
				if (nameTriple != null && (nameTriple.UsedThisGame || (requiredLastName != null && nameTriple.Last != requiredLastName)))
				{
					nameTriple = null;
				}
			}
			List<NameTriple> listForGender = PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Either);
			List<NameTriple> list = (gender != Gender.Male) ? PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Female) : PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Male);
			float num = ((float)listForGender.Count + 0.1f) / ((float)(listForGender.Count + list.Count) + 0.1f);
			List<NameTriple> list2;
			if (Rand.Value < num)
			{
				list2 = listForGender;
			}
			else
			{
				list2 = list;
			}
			if (list2.Count == 0)
			{
				Log.Error("Empty solid pawn name list for gender: " + gender + ".");
				return null;
			}
			if (nameTriple != null && list2.Contains(nameTriple))
			{
				return nameTriple;
			}
			list2.Shuffle<NameTriple>();
			return (from name in list2
					where (requiredLastName == null || !(name.Last != requiredLastName)) && !name.UsedThisGame
					select name).FirstOrDefault<NameTriple>();
		}

		public static Name GeneratePawnName(Pawn pawn, NameStyle style = NameStyle.Full, string forcedLastName = null)
		{
			if (style == NameStyle.Full)
			{
				RulePackDef nameGenerator = pawn.RaceProps.GetNameGenerator(pawn.gender);
				if (nameGenerator != null)
				{
					string name = NameGenerator.GenerateName(nameGenerator, (string x) => !new NameSingle(x, false).UsedThisGame, false);
					return new NameSingle(name, false);
				}
				if (pawn.Faction != null && pawn.Faction.def.pawnNameMaker != null)
				{
					string rawName = NameGenerator.GenerateName(pawn.Faction.def.pawnNameMaker, delegate (string x)
					{
						NameTriple nameTriple4 = NameTriple.FromString(x);
						nameTriple4.ResolveMissingPieces(forcedLastName);
						return !nameTriple4.UsedThisGame;
					}, false);
					NameTriple nameTriple = NameTriple.FromString(rawName);
					nameTriple.CapitalizeNick();
					nameTriple.ResolveMissingPieces(forcedLastName);
					return nameTriple;
				}
				if (pawn.RaceProps.nameCategory != PawnNameCategory.NoName)
				{
					if (Rand.Value < 0.5f)
					{
						NameTriple nameTriple2 = TryGetRandomUnusedSolidName(pawn.gender, forcedLastName);
						if (nameTriple2 != null)
						{
							return nameTriple2;
						}
					}
					return GeneratePawnName_Shuffled(pawn, forcedLastName);
				}
				Log.Error("No name making method for " + pawn);
				NameTriple nameTriple3 = NameTriple.FromString(pawn.def.label);
				nameTriple3.ResolveMissingPieces(null);
				return nameTriple3;
			}
			else
			{
				if (style == NameStyle.Numeric)
				{
					int num = 1;
					string text;
					while (true)
					{
						text = pawn.KindLabel + " " + num.ToString();
						if (!NameUseChecker.NameSingleIsUsed(text))
						{
							break;
						}
						num++;
					}
					return new NameSingle(text, true);
				}
				throw new InvalidOperationException();
			}
		}

		private static NameTriple GeneratePawnName_Shuffled(Pawn pawn, string forcedLastName = null)
		{
			PawnNameCategory pawnNameCategory = pawn.RaceProps.nameCategory;
			if (pawnNameCategory == PawnNameCategory.NoName)
			{
				Log.Message("ProPawnBioAndNameGenerator.GeneratePawnName_Shuddled: Can't create a name of type NoName. Defaulting to HumanStandard.");
				pawnNameCategory = PawnNameCategory.HumanStandard;
			}
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(pawnNameCategory);
			string name = nameBank.GetName(PawnNameSlot.First, pawn.gender);
			string text;
			if (forcedLastName != null)
			{
				text = forcedLastName;
			}
			else
			{
				text = nameBank.GetName(PawnNameSlot.Last, Gender.None);
			}
			int num = 0;
			string nick;
			do
			{
				num++;
				if (Rand.Value < 0.15f)
				{
					Gender gender = pawn.gender;
					if (Rand.Value < 0.5f)
					{
						gender = Gender.None;
					}
					nick = nameBank.GetName(PawnNameSlot.Nick, gender);
				}
				else if (Rand.Value < 0.5f)
				{
					nick = name;
				}
				else
				{
					nick = text;
				}
			}
			while (num < 50 && NameUseChecker.AllPawnsNamesEverUsed.Any(delegate (Name x)
			{
				NameTriple nameTriple = x as NameTriple;
				return nameTriple != null && nameTriple.Nick == nick;
			}));
			return new NameTriple(name, nick, text);
		}
	}
}
