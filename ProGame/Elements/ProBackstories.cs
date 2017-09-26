using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Diagnostics;

namespace Prophecy.ProGame.Elements
{
	[StaticConstructorOnStartup]
	public static class ProBackstories
	{

		public static Backstory[] aBSChildNeo = new Backstory[0];

		public static Backstory[] aBSAdultNeo = new Backstory[0];

		static ProBackstories()
		{
			Backstory BSChildNeo01 = new Backstory();
			BSChildNeo01.slot = BackstorySlot.Childhood;
			BSChildNeo01.SetTitle("Amateur Astronomer");
			BSChildNeo01.SetTitleShort("Astronomer");
			BSChildNeo01.baseDesc = "NAME was fascinated with the stars. HECAP would spend hours gazing at night. HECAP has even made discoveries unkown to most scholars.";
			BSChildNeo01.skillGains = new Dictionary<string, int>{ {"Artistic", 1}, {"Intellectual", 3}, {"Social", 1} };
			BSChildNeo01.spawnCategories = new List<string>(new string[]{"Civil" });
			BSChildNeo01.bodyTypeMale = BodyType.Male;
			BSChildNeo01.bodyTypeFemale = BodyType.Female;

			Backstory BSChildNeo02 = new Backstory();
			BSChildNeo02.slot = BackstorySlot.Childhood;
			BSChildNeo02.SetTitle("Amateur Botanist");
			BSChildNeo02.SetTitleShort("Botanist");
			BSChildNeo02.baseDesc = "Spending many summers crawling through dirt, NAME found he had quite the green thumb. Instead of learning to cook the food HECAP grew, HECAP just grew more and more.";
			BSChildNeo02.skillGains = new Dictionary<string, int> { { "Growing", 4 } };
			BSChildNeo02.spawnCategories = new List<string>(new string[] { "Civil" });
			BSChildNeo02.bodyTypeMale = BodyType.Male;
			BSChildNeo02.bodyTypeFemale = BodyType.Female;

			

			Backstory BSAdultNeo01 = new Backstory();
			BSAdultNeo01.slot = BackstorySlot.Adulthood;
			BSAdultNeo01.SetTitle("Foreign Herd Owner");
			BSAdultNeo01.SetTitleShort("Herder");
			BSAdultNeo01.baseDesc = "NAME had wealthy life as a owner of a large herd in a remote and foreign land. That is, until disaster struck. Now HE strives to carve a new life among people he can't understand. HISCAP culture is so different that he has chosen to remain silent.";
			BSAdultNeo01.workDisables = WorkTags.Social;
			BSAdultNeo01.requiredWorkTags = WorkTags.ManualSkilled | WorkTags.Animals;
			BSAdultNeo01.skillGains = new Dictionary<string, int> { { "Animals", 3 }, { "Artistic", 1 }, { "Cooking", 2 }, { "Crafting", 2 } };
			BSAdultNeo01.spawnCategories = new List<string>(new string[] { "Civil" });
			BSAdultNeo01.bodyTypeMale = BodyType.Male;
			BSAdultNeo01.bodyTypeFemale = BodyType.Female;

			Backstory BSAdultNeo02 = new Backstory();
			BSAdultNeo02.slot = BackstorySlot.Adulthood;
			BSAdultNeo02.SetTitle("Storyteller");
			BSAdultNeo02.SetTitleShort("Storyteller");
			BSAdultNeo02.baseDesc = "NAME has traveled the world gathering and recounting many legends and histories. After many years, HECAP now looks for place to belong.";
			BSAdultNeo02.requiredWorkTags = WorkTags.Social | WorkTags.Artistic;
			BSAdultNeo02.skillGains = new Dictionary<string, int> { { "Artistic", 4 }, { "Construction", -4 }, { "Crafting", 1 }, { "Growing", -4 }, { "Medicine", -2 }, { "Mining", -4 }, { "Social", 3 } };
			BSAdultNeo02.spawnCategories = new List<string>(new string[] { "Civil" });
			BSAdultNeo02.bodyTypeMale = BodyType.Male;
			BSAdultNeo02.bodyTypeFemale = BodyType.Female;

			aBSChildNeo = aBSChildNeo.Concat(new Backstory[] { BSChildNeo01, BSChildNeo02 }).ToArray();
			aBSAdultNeo = aBSAdultNeo.Concat(new Backstory[] { BSAdultNeo01, BSAdultNeo02 }).ToArray();

			foreach (Backstory bs in aBSChildNeo)
			{
				bs.PostLoad();
				bs.ResolveReferences();
			}
			foreach (Backstory bs in aBSAdultNeo)
			{
				bs.PostLoad();
				bs.ResolveReferences();
			}
		}

		//public static Backstory GetRandomAdultMaleBackstory()
		//{
		//	Backstory BS = new Backstory();
		//	int intRand = 1;

		//	switch (intRand)
		//	{
		//		case 1:
		//		{
		//			BS.SetTitle("Architect");
		//			BS.SetTitleShort("Architect");
		//			BS.baseDesc = "NAME designed and constructed buildings. On HIS glitterworld home, most of the technical aspects of architecture were handled by an AI. This enabled HIM to push the artistic limits of the craft, but also meant HE never had to get HIS hands dirty at building sites.";
		//			BS.slot = BackstorySlot.Adulthood;
		//			BS.workDisables = WorkTags.ManualDumb;
		//			BS.requiredWorkTags = WorkTags.Intellectual;
		//			BS.skillGains = new Dictionary<string, int>
		//			{
		//				{ "Construction", 5}, { "Artistic", 7}, { "Crafting", -3}
		//			};
		//			BS.spawnCategories = new List<string>(new string[] { "Civil", "Traveler" });
		//			BS.bodyTypeMale = BodyType.Male;
		//			BS.bodyTypeFemale = BodyType.Female;
		//			break;
		//		}
		//	}

		//	return BS;
		//}

	}
}
