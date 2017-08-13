using Verse;
using UnityEngine;
using System;

namespace Prophesy.Meta
{	
	[StaticConstructorOnStartup]
	public class ProphecySettings : ModSettings
	{
		//public bool enableFirestarterAbility = true;
		//public bool onlyPyro = false;
		public bool enableResearch = true;

		public DietCategory diet = DietCategory.Omnivorous;

		public override void ExposeData()
		{
			base.ExposeData();
			//Scribe_Values.Look(ref this.enableFirestarterAbility, "enableFirestarterAbility", true);
			//Scribe_Values.Look(ref this.onlyPyro, "onlyPyro", false);
			//Scribe_Values.Look(ref this.enableResearch, "enableResearch", true);
		}
	}

	class ProphecyMod : Mod
	{
		public static ProphecySettings settings;

		public ProphecyMod(ModContentPack content) : base(content)
		{
			settings = GetSettings<ProphecySettings>();
		}

		public override string SettingsCategory() => "Prophecy";

		public override void DoSettingsWindowContents(Rect inRect)
		{
			//	settings.Write();
			ModWindowHelper.ConfigureModWindow(inRect, inRect.height * .05f, 0f);
			ModWindowHelper.MakeLabel("This is a Test");
			ModWindowHelper.MakeLabeledCheckbox("This is a Test labeled checkbox", ref settings.enableResearch);
			var dietTemp = (object)settings.diet;
			ModWindowHelper.MakeRadioButtonList("Diet", ref dietTemp);
			settings.diet = (DietCategory)dietTemp;
		}
	}
}
