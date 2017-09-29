using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Diagnostics;

namespace Prophecy.ProGame.Elements
{
	public class ProBackstories
	{
		private Backstory tempBS;
		public int intChildStoriesAmount = 12;
		public int intAdultStoriesAmount = 12;

		public ProBackstories()
		{
			try
			{
				tempBS = new Backstory();
			}
			catch(Exception e)
			{
				Log.Message("ProBackstories.ctor failed. /n " + e.Message);
			}
		}

		public void GetNeoChildStory(ref Backstory _bs, int _index = 0)
		{
			tempBS = new Backstory();
			int i = 0;

			if (_index == 0)
			{
				i = Rand.Range(1, intChildStoriesAmount + 1);
			}
			else
			{
				i = _index;
			}

			try
			{
				switch (i)
				{
					case 1:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Amateur astronomer");
						tempBS.SetTitleShort("Astronomer");
						tempBS.baseDesc = "NAME was fascinated with the stars. HECAP would spend hours gazing at night. HECAP has even made discoveries unkown to most scholars.";
						tempBS.skillGains = new Dictionary<string, int> { { "Artistic", 1 }, { "Intellectual", 3 }, { "Social", 1 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 2:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Amateur botanist");
						tempBS.SetTitleShort("Botanist");
						tempBS.baseDesc = "Spending many summers crawling through dirt, NAME found he had quite the green thumb. Instead of learning to cook the food HE grew, HE just grew more and more.";
						tempBS.skillGains = new Dictionary<string, int> { { "Growing", 4 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 3:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Cave child");
						tempBS.SetTitleShort("Cave child");
						tempBS.baseDesc = "NAME grew up in a large and intricate cave complex that extended deep into a mountainside. HECAP helped the adults maintain and improve the deep caves.";
						tempBS.skillGains = new Dictionary<string, int> { { "Construction", 2 }, { "Mining", 3 }, { "Shooting", -1 }, { "Social", 1 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 4:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Caveworld tender");
						tempBS.SetTitleShort("Cave kid");
						tempBS.baseDesc = "NAME grew up in cave complex deep beneath the surface of an inhospitable world. HECAP worked with the other children tending the tribe’s fungus crops.";
						tempBS.skillGains = new Dictionary<string, int> { { "Construction", 2 }, { "Mining", 3 }, { "Shooting", -1 }, { "Social", 1 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 5:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Disaster survivor");
						tempBS.SetTitleShort("Survivor");
						tempBS.baseDesc = "NAME was uprooted when marauders attacked HIS family farm, destroying machinery and killing farmhands and beasts alike. After the death of all HE knew, HE was left in the ruins to fend for HIMself.";
						tempBS.skillGains = new Dictionary<string, int> { { "Crafting", 2 }, { "Growing", 2 }, { "Melee", 2 }, { "Shooting", 1 }, { "Social", -1 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 6:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Feral child");
						tempBS.SetTitleShort("Feral");
						tempBS.baseDesc = "Abandoned in the wilderness as a small child with nought but a blanket with a name embroidered on it, NAME made HIMself one with the wild. When HE was 13, HE encountered a group of hunters, who \"offered\" HIM a home.";
						tempBS.skillGains = new Dictionary<string, int> { { "Animals", 3 }, { "Melee", 3 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 7:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Fire keeper");
						tempBS.SetTitleShort("Firekeep");
						tempBS.baseDesc = "NAME was responsible for keeping the tribe’s fire going. HECAP took this responsibility very seriously.";
						tempBS.skillGains = new Dictionary<string, int> { { "Construction", 1 }, { "Cooking", 2 }, { "Crafting", 2 }, { "Growing", -1 }, { "Intellectual", 2 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 8:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Fire-scarred child");
						tempBS.SetTitleShort("Scarred");
						tempBS.baseDesc = "NAMED was an active child who lived an uneventful childhood until HE fell into a fire and suffered horrific burns to HIS hands and arms. Although the scars have faded, HE can't bear to be in close proximity to fire.";
						tempBS.workDisables = WorkTags.Firefighting;
						tempBS.skillGains = new Dictionary<string, int> { { "Artistic", 3 }, { "Growing", 2 }, { "Melee", 1 }, { "Mining", 2 }, { "Social", -1 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 9:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Frightened child");
						tempBS.SetTitleShort("Scared");
						tempBS.baseDesc = "NAME grew up with a laundry list of phobias and neuroses. HECAP feared, among other things, doctors and foodborne pathogens. As a result, HE learned to cook and care for HIMself, but many of HIS fears dog HIM in adulthood.";
						tempBS.workDisables = WorkTags.Violent;
						tempBS.skillGains = new Dictionary<string, int> { { "Animals", -1 }, { "Construction", 1 }, { "Cooking", 3 }, { "Crafting", 1 }, { "Growing", 1 }, { "Medicine", 3 }, { "Intellectual", -1 }, { "Social", -2 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 10:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Herder");
						tempBS.SetTitleShort("Herder");
						tempBS.baseDesc = "As a child, NAME tended the tribe’s muffalo herds, keeping them safe from predators and treating the sick. It was quiet work, but HE enjoyed being away from people.";
						tempBS.skillGains = new Dictionary<string, int> { { "Animals", 3 }, { "Medicine", 2 }, { "Melee", 2 }, { "Social", -2 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 11:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Tundra child");
						tempBS.SetTitleShort("Ice child");
						tempBS.baseDesc = "Growing up on the frozen wastes of the far North, NAME only had animals and a few hard-bitten sailors as companions. The lack of social interaction made HIM develop a interest in engineering - but HE never developed any great fondness for humans.";
						tempBS.skillGains = new Dictionary<string, int> { { "Animals", 2 }, { "Construction", 2 }, { "Crafting", 2 }, { "Growing", -4 }, { "Melee", 2 }, { "Intellectual", 1 }, { "Social", -4 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					case 12:
						tempBS.slot = BackstorySlot.Childhood;
						tempBS.SetTitle("Hunting child");
						tempBS.SetTitleShort("Hunter");
						tempBS.baseDesc = "NAME had an great understanding of the sling and bow. HECAP spent hours almost everyday perfecting HIS shot. HECAP wanted to be the best hunter that ever lived.";
						tempBS.skillGains = new Dictionary<string, int> { { "Animals", 1 }, { "Crafting", 2 }, { "Shooting", 3 } };
						tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
						tempBS.bodyTypeMale = BodyType.Male;
						tempBS.bodyTypeFemale = BodyType.Female;
						break;

					default:
						Log.Message("ProBackstories.GetNeoChildStory failed switch.");
						break;
				}
			}
			catch(Exception e)
			{
				Log.Message(e.TargetSite.Name + " \n" +e.Message);
			}

			
			tempBS.PostLoad();
			tempBS.ResolveReferences();

			_bs = tempBS;
		}

		public void GetNeoAdultStory(ref Backstory _bs, int _index = 0)
		{
			tempBS = new Backstory();
			int i = 0;

			if (_index == 0)
			{
				i = Rand.Range(1, intAdultStoriesAmount + 1);
			}
			else
			{
				i = _index;
			}

			switch (i)
			{
				case 1:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Foreign herd owner");
					tempBS.SetTitleShort("Herder");
					tempBS.baseDesc = "NAME had wealthy life as a owner of a large herd in a remote and foreign land. That is, until disaster struck. Now HE strives to carve a new life among people HE can't understand. HISCAP culture is so different that HE has chosen to remain silent.";
					tempBS.workDisables = WorkTags.Social;
					tempBS.requiredWorkTags = WorkTags.ManualSkilled | WorkTags.Animals;
					tempBS.skillGains = new Dictionary<string, int> { { "Animals", 3 }, { "Artistic", 1 }, { "Cooking", 2 }, { "Crafting", 2 } };
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 2:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Storyteller");
					tempBS.SetTitleShort("Storyteller");
					tempBS.baseDesc = "NAME has traveled the world gathering and recounting many legends and histories. After many years, HE now looks for place to belong.";
					tempBS.requiredWorkTags = WorkTags.Social | WorkTags.Artistic;
					tempBS.skillGains = new Dictionary<string, int> { { "Artistic", 4 }, { "Construction", -4 }, { "Crafting", 1 }, { "Growing", -4 }, { "Medicine", -2 }, { "Mining", -4 }, { "Social", 3 } };
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 3:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Maker");
					tempBS.SetTitleShort("Maker");
					tempBS.baseDesc = "NAME struggled with hunting, but when it came to farming, HE had no patients. HECAP found his calling in building and creating things. HECAP has a natural talent for making plans become a reality.";
					tempBS.workDisables = WorkTags.PlantWork;
					tempBS.requiredWorkTags = WorkTags.ManualSkilled;
					tempBS.skillGains = new Dictionary<string, int> { { "Construction", 4 }, { "Crafting", 2 } };
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 4:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Warrior cook");
					tempBS.SetTitleShort("Cook");
					tempBS.baseDesc = "NAME joined a raiding tribe and spent most of HIS time cooking and repairing. During HIS travels, HE managed to get a basic knoweldge of combat.";
					tempBS.requiredWorkTags = WorkTags.ManualSkilled | WorkTags.Violent | WorkTags.Cooking;
					tempBS.skillGains = new Dictionary<string, int> { { "Animals", -1 }, { "Construction", 1 }, { "Cooking", 3 }, { "Growing", -2 }, { "Medicine", -1 }, { "Melee", 3 } };
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 5:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Adornment crafter");
					tempBS.SetTitleShort("Crafter");
					tempBS.baseDesc = "Name was quite good at making tinkets and pendants. As people traded HIM goods for these, NAME soon found that HE could spend all of HIS time and energy crafting more complicated and intrquet things.";
					tempBS.requiredWorkTags = WorkTags.ManualSkilled | WorkTags.Artistic | WorkTags.Crafting;
					tempBS.skillGains = new Dictionary<string, int> { { "Animals", -4 }, { "Artistic", 3 }, { "Crafting", 4 }, { "Growing", -4 }, { "Melee", -2 } };
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 6:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Stalwart farmer");
					tempBS.SetTitleShort("Farmer");
					tempBS.baseDesc = "NAME had deadly shot with the bow and arrow, but that couldn't save HIS farm from the brigands. HECAP had done such a good job growing wheat and vegetables, people thought it was the soil. ";
					tempBS.workDisables = WorkTags.None;
					tempBS.requiredWorkTags = WorkTags.PlantWork | WorkTags.Violent | WorkTags.ManualDumb | WorkTags.Hauling;
					tempBS.skillGains = new Dictionary<string, int> { { "Animals", -2 }, { "Artistic", -2 }, { "Construction", 0 }, { "Cooking", 0 }, { "Crafting", 0 }, { "Growing", 4 }
												, { "Medicine", 0 }, { "Melee", 0 }, { "Mining", -2 }, { "Intellectual", 0 }, { "Shooting", 3 }, { "Social", -4 }};
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 7:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Forest shaman");
					tempBS.SetTitleShort("Shaman");
					tempBS.baseDesc = "NAME stuidied the shamanistic ways, but was dissatisified with HIS tribe elders. HECAP chose to wander the forests and practice HIS learnings until HS could find a place that needed HIM most.";
					tempBS.workDisables = WorkTags.None;
					tempBS.requiredWorkTags = WorkTags.Caring | WorkTags.Animals | WorkTags.PlantWork;
					tempBS.skillGains = new Dictionary<string, int> { { "Animals", 2 }, { "Artistic", 0 }, { "Construction", -4 }, { "Cooking", 0 }, { "Crafting", -4 }, { "Growing", 2 }
												, { "Medicine", 4 }, { "Melee", 0 }, { "Mining", -4 }, { "Intellectual", 0 }, { "Shooting", 0 }, { "Social", 0 }};
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 8:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Runaway mine slave");
					tempBS.SetTitleShort("Ex slave");
					tempBS.baseDesc = "NAME was captured by a hostile clan and forced to mine stone and flint as a slave. Secretly the slaves practiced combat until the day came where they incited a massive revolt and fought their way to freedom. NAME can never pick at stone again without remembering the horrors HE endured as a cruely treated slave.";
					tempBS.workDisables = WorkTags.Mining;
					tempBS.requiredWorkTags = WorkTags.ManualDumb | WorkTags.Violent | WorkTags.Hauling;
					tempBS.skillGains = new Dictionary<string, int> { { "Animals", 0 }, { "Artistic", 0 }, { "Construction", 2 }, { "Cooking", 0 }, { "Crafting", 0 }, { "Growing", 0 }
												, { "Medicine", 1 }, { "Melee", 3 }, { "Mining", 0 }, { "Intellectual", 0 }, { "Shooting", 2 }, { "Social", 0 }};
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 9:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Gold miner");
					tempBS.SetTitleShort("Miner");
					tempBS.baseDesc = "NAME made HIS mark in the world when HE joined a lucrative gold mining community deep in the jungle mountains. HECAP learned a lot about life, but most of all, HE learned how to mine and HE learned how to grow food in the harshest conditions.";
					tempBS.workDisables = WorkTags.None;
					tempBS.requiredWorkTags = WorkTags.ManualSkilled | WorkTags.ManualDumb | WorkTags.Hauling | WorkTags.PlantWork | WorkTags.Mining;
					tempBS.skillGains = new Dictionary<string, int> { { "Animals", -3 }, { "Artistic", -3 }, { "Construction", 0 }, { "Cooking", -3 }, { "Crafting", -3 }, { "Growing", 3 }
												, { "Medicine", -2 }, { "Melee", 1 }, { "Mining", 4 }, { "Intellectual", 0 }, { "Shooting", 0 }, { "Social", 0 }};
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 10:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Lore keeper");
					tempBS.SetTitleShort("Keeper");
					tempBS.baseDesc = "NAME learned to read and write from a long line of lore keepers in HIS tribe. HECAP would stay up by fire every night and work on scrolls to pass on knowledge and helpful wisdom. It is through this mass of information that NAME now finds HIS purpose.";
					tempBS.workDisables = WorkTags.None;
					tempBS.requiredWorkTags = WorkTags.Social | WorkTags.Intellectual;
					tempBS.skillGains = new Dictionary<string, int> { { "Animals", 0 }, { "Artistic", 0 }, { "Construction", 0 }, { "Cooking", 0 }, { "Crafting", 0 }, { "Growing", 0 }
												, { "Medicine", 0 }, { "Melee", 0 }, { "Mining", 0 }, { "Intellectual", 4 }, { "Shooting", 0 }, { "Social", 0 }};
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 11:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Cave tribe hunter");
					tempBS.SetTitleShort("Hunter");
					tempBS.baseDesc = "NAME would leave HIS tribe's cave system and only return home once HE had a fresh kill. NAME was usually home by sunset. HISCAP tribe would have loved HIM, if HE only learned to clean up after HIMSELF. And this is coming from cave people.";
					tempBS.workDisables = WorkTags.Cleaning;
					tempBS.requiredWorkTags = WorkTags.Violent | WorkTags.Animals | WorkTags.Hauling | WorkTags.Mining;
					tempBS.skillGains = new Dictionary<string, int> { { "Animals", 0 }, { "Artistic", -2 }, { "Construction", 1 }, { "Cooking", 0 }, { "Crafting", 0 }, { "Growing", 0 }
												, { "Medicine", 0 }, { "Melee", 2 }, { "Mining", 2 }, { "Intellectual", -4 }, { "Shooting", 4 }, { "Social", -4 }};
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				case 12:
					tempBS.slot = BackstorySlot.Adulthood;
					tempBS.SetTitle("Clan missionary");
					tempBS.SetTitleShort("Missionary");
					tempBS.baseDesc = "NAME decided to devote HIS life to religious service. HECAP would do farm work by day in exchange for religious teachings in the evening. HECAP was an important member of HIS clan's society until a new warcheif took over, and kicked HIM out.";
					tempBS.workDisables = WorkTags.None;
					tempBS.requiredWorkTags = WorkTags.ManualDumb | WorkTags.Caring | WorkTags.Social | WorkTags.Intellectual | WorkTags.Animals | WorkTags.Cleaning | WorkTags.Hauling | WorkTags.PlantWork;
					tempBS.skillGains = new Dictionary<string, int> { { "Animals", 3 }, { "Artistic", 0 }, { "Construction", 0 }, { "Cooking", -3 }, { "Crafting", -3 }, { "Growing", 2 }
												, { "Medicine", 0 }, { "Melee", -4 }, { "Mining", 0 }, { "Intellectual", 2 }, { "Shooting", -4 }, { "Social", 3 }};
					tempBS.spawnCategories = new List<string>(new string[] { "New_Arrival", "Tribal" });
					tempBS.bodyTypeMale = BodyType.Male;
					tempBS.bodyTypeFemale = BodyType.Female;
					break;

				default:
					Log.Message("ProBackstories.GetNeoAdultStory failed switch.");
					break;
			}

			tempBS.PostLoad();
			tempBS.ResolveReferences();

			_bs = tempBS;
		}
	}
}
