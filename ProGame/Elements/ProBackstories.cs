using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace Prophesy.ProGame.Elements
{
	public static class ProBackstories
	{
		public static Backstory BSChildNeo01;

		public static Backstory BSAdultNeo01;

		static ProBackstories()
		{
			BSChildNeo01.slot = BackstorySlot.Childhood;
			BSChildNeo01.SetTitle("Amateur Astronomer");
			BSChildNeo01.SetTitleShort("Astronomer");
			BSChildNeo01.baseDesc = "NAME was fascinated with the stars. HECAP would spend hours gazing at night. HECAP has even made discoveries unkown to most scholars.";
			BSChildNeo01.skillGains = new Dictionary<string, int>{ {"Artistic", 1}, {"Intellectual", 3}, {"Social", 1} };
			BSChildNeo01.spawnCategories = new List<string>(new string[]{"Civil" });
			BSChildNeo01.bodyTypeMale = BodyType.Male;
			BSChildNeo01.bodyTypeFemale = BodyType.Female;


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
