using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Profile;
using Verse.Sound;
using RimWorld;
using Harmony;

namespace Prophecy.PreGame
{
	public class ProCWP : Page
    {
		/********************
		* Create world page *
		********************/
        private bool initialized;

        private string seedString;

        private float planetCoverage;

        private OverallRainfall rainfall;

        private OverallTemperature temperature;

        private readonly static float[] PlanetCoverages;

        private readonly static float[] PlanetCoveragesDev;

        public override string PageTitle
        {
            get
            {
                return "CreateWorld".Translate();
            }
        }

        static ProCWP()
        {
            ProCWP.PlanetCoverages = new float[] { 0.3f, 0.5f, 1f };
            ProCWP.PlanetCoveragesDev = new float[] { 0.3f, 0.5f, 1f, 0.05f };
        }

        public ProCWP()
        {
        }

        protected override bool CanDoNext()
        {
            if (!base.CanDoNext())
            {
                return false;
            }
            LongEventHandler.QueueLongEvent(() => {
                Find.GameInitData.ResetWorldRelatedMapInitData();
                Current.Game.World = WorldGenerator.GenerateWorld(this.planetCoverage, this.seedString, this.rainfall, this.temperature);
                LongEventHandler.ExecuteWhenFinished(() => {
                    if (this.next != null)
                    {
                        Find.WindowStack.Add(this.next);
                    }
                    MemoryUtility.UnloadUnusedUnityAssets();
                    Find.World.renderer.RegenerateAllLayersNow();
                    this.Close(true);
                });
            }, "GeneratingWorld", true, null);
            return false;
        }

        public override void DoWindowContents(Rect rect)
        {
            base.DrawPageTitle(rect);
            GUI.BeginGroup(base.GetMainRect(rect, 0f, false));
            Text.Font = GameFont.Small;
            float single = 0f;
            Widgets.Label(new Rect(0f, single, 200f, 30f), "WorldSeed".Translate());
            Rect rect1 = new Rect(200f, single, 200f, 30f);
            this.seedString = Widgets.TextField(rect1, this.seedString);
            single = single + 40f;
            Rect rect2 = new Rect(200f, single, 200f, 30f);
            if (Widgets.ButtonText(rect2, "RandomizeSeed".Translate(), true, false, true))
            {
                SoundDefOf.TickTiny.PlayOneShotOnCamera(null);
                this.seedString = GenText.RandomSeedString();
            }
            single = single + 40f;
            Widgets.Label(new Rect(0f, single, 200f, 30f), "PlanetCoverage".Translate());
            Rect rect3 = new Rect(200f, single, 200f, 30f);
            if (Widgets.ButtonText(rect3, this.planetCoverage.ToStringPercent(), true, false, true))
            {
                List<FloatMenuOption> floatMenuOptions = new List<FloatMenuOption>();
                float[] singleArray = (!Prefs.DevMode ? ProCWP.PlanetCoverages : ProCWP.PlanetCoveragesDev);
                for (int i = 0; i < (int)singleArray.Length; i++)
                {
                    float single1 = singleArray[i];
                    string stringPercent = single1.ToStringPercent();
                    if (single1 <= 0.1f)
                    {
                        stringPercent = string.Concat(stringPercent, " (dev)");
                    }
                    FloatMenuOption floatMenuOption = new FloatMenuOption(stringPercent, () => {
                        if (this.planetCoverage != single1)
                        {
                            this.planetCoverage = single1;
                            if (this.planetCoverage == 1f)
                            {
                                Messages.Message("MessageMaxPlanetCoveragePerformanceWarning".Translate(), MessageSound.Standard);
                            }
                        }
                    }, MenuOptionPriority.Default, null, null, 0f, null, null);
                    floatMenuOptions.Add(floatMenuOption);
                }
                Find.WindowStack.Add(new FloatMenu(floatMenuOptions));
            }
            TooltipHandler.TipRegion(new Rect(0f, single, rect3.xMax, rect3.height), "PlanetCoverageTip".Translate());
            single = single + 40f;
            Widgets.Label(new Rect(0f, single, 200f, 30f), "PlanetRainfall".Translate());
            Rect rect4 = new Rect(200f, single, 200f, 30f);
            this.rainfall = (OverallRainfall)Mathf.RoundToInt(Widgets.HorizontalSlider(rect4, (float)this.rainfall, 0f, (float)(OverallRainfallUtility.EnumValuesCount - 1), true, "PlanetRainfall_Normal".Translate(), "PlanetRainfall_Low".Translate(), "PlanetRainfall_High".Translate(), 1f));
            single = single + 40f;
            Widgets.Label(new Rect(0f, single, 200f, 30f), "PlanetTemperature".Translate());
            Rect rect5 = new Rect(200f, single, 200f, 30f);
            this.temperature = (OverallTemperature)Mathf.RoundToInt(Widgets.HorizontalSlider(rect5, (float)this.temperature, 0f, (float)(OverallTemperatureUtility.EnumValuesCount - 1), true, "PlanetTemperature_Normal".Translate(), "PlanetTemperature_Low".Translate(), "PlanetTemperature_High".Translate(), 1f));
            GUI.EndGroup();
            base.DoBottomButtons(rect, "WorldGenerate".Translate(), "Reset".Translate(), new Action(this.Reset), true);
        }

        public override void PostOpen()
        {
            base.PostOpen();
            //TutorSystem.Notify_Event("PageStart-CreateWorldParams");
        }

        public override void PreOpen()
        {
            Current.ProgramState = ProgramState.Entry;
            Current.Game = new Game();
            Current.Game.InitData = new GameInitData();
            Current.Game.Scenario = ProSM.GenScenSkeleton("Test Scenario");
            next = new ProSLS();
            base.PreOpen();
            if (!this.initialized)
            {
                this.Reset();
                this.initialized = true;
            }
        }

        public void Reset()
        {
            this.seedString = GenText.RandomSeedString();
            this.planetCoverage = (!Prefs.DevMode || !UnityData.isEditor ? 0.3f : 0.05f);
            this.rainfall = OverallRainfall.Normal;
            this.temperature = OverallTemperature.Normal;
        }
    }
}
