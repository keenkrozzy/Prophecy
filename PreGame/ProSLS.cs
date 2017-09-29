using RimWorld.Planet;
using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;
using Harmony;

namespace Prophecy.PreGame
{
	public class ProSLS : Page
    {
		/***************************
		* Select landing site page *
		***************************/
        private const float GapBetweenBottomButtons = 10f;

        private const float UseTwoRowsIfScreenWidthBelow = 1340f;

        public override Vector2 InitialSize
        {
            get
            {
                return Vector2.zero;
            }
        }

        protected override float Margin
        {
            get
            {
                return 0f;
            }
        }

        public override string PageTitle
        {
            get
            {
                return "SelectLandingSite".Translate();
            }
        }

        public ProSLS()
        {
            this.absorbInputAroundWindow = false;
            this.shadowAlpha = 0f;
            this.preventCameraMotion = false;
        }

        protected override bool CanDoNext()
        {
            if (!base.CanDoNext())
            {
                return false;
            }
            int selectedTile = Find.WorldInterface.SelectedTile;
            if (selectedTile < 0)
            {
                Messages.Message("MustSelectLandingSite".Translate(), MessageSound.RejectInput);
                return false;
            }
            StringBuilder stringBuilder = new StringBuilder();
            if (!TileFinder.IsValidTileForNewSettlement(selectedTile, stringBuilder))
            {
                Messages.Message(stringBuilder.ToString(), MessageSound.RejectInput);
                return false;
            }
            Tile item = Find.WorldGrid[selectedTile];
            if (!TutorSystem.AllowAction(string.Concat("ChooseBiome-", item.biome.defName, "-", item.hilliness.ToString())))
            {
                return false;
            }
            return true;
        }

        private void DoCustomBottomButtons()
        {
            int num;
            int num1 = (!TutorSystem.TutorialMode ? 5 : 4);
            num = (num1 < 4 || (float)UI.screenWidth >= 1340f ? 1 : 2);
            int num2 = Mathf.CeilToInt((float)num1 / (float)num);
            Vector2 bottomButSize = Page.BottomButSize;
            float single = bottomButSize.x * (float)num2 + 10f * (float)(num2 + 1);
            float single1 = (float)num;
            Vector2 vector2 = Page.BottomButSize;
            float single2 = single1 * vector2.y + 10f * (float)(num + 1);
            Rect rect = new Rect(((float)UI.screenWidth - single) / 2f, (float)UI.screenHeight - single2 - 4f, single, single2);
            if (Find.WindowStack.IsOpen<WorldInspectPane>() && rect.x < InspectPaneUtility.PaneSize.x + 4f)
            {
                rect.x = InspectPaneUtility.PaneSize.x + 4f;
            }
            Widgets.DrawWindowBackground(rect);
            float bottomButSize1 = rect.xMin + 10f;
            float bottomButSize2 = rect.yMin + 10f;
            Text.Font = GameFont.Small;
            if (Widgets.ButtonText(new Rect(bottomButSize1, bottomButSize2, Page.BottomButSize.x, Page.BottomButSize.y), "Back".Translate(), true, false, true) && this.CanDoBack())
            {
                this.DoBack();
            }
            bottomButSize1 = bottomButSize1 + (Page.BottomButSize.x + 10f);
            if (!TutorSystem.TutorialMode)
            {
                if (Widgets.ButtonText(new Rect(bottomButSize1, bottomButSize2, Page.BottomButSize.x, Page.BottomButSize.y), "Advanced".Translate(), true, false, true))
                {
                    Find.WindowStack.Add(new Dialog_AdvancedGameConfig(Find.WorldInterface.SelectedTile));
                }
                bottomButSize1 = bottomButSize1 + (Page.BottomButSize.x + 10f);
            }
            if (Widgets.ButtonText(new Rect(bottomButSize1, bottomButSize2, Page.BottomButSize.x, Page.BottomButSize.y), "SelectRandomSite".Translate(), true, false, true))
            {
                SoundDefOf.Click.PlayOneShotOnCamera(null);
                Find.WorldInterface.SelectedTile = TileFinder.RandomStartingTile();
                Find.WorldCameraDriver.JumpTo(Find.WorldGrid.GetTileCenter(Find.WorldInterface.SelectedTile));
            }
            bottomButSize1 = bottomButSize1 + (Page.BottomButSize.x + 10f);
            if (num == 2)
            {
                bottomButSize1 = rect.xMin + 10f;
                bottomButSize2 = bottomButSize2 + (Page.BottomButSize.y + 10f);
            }
            if (Widgets.ButtonText(new Rect(bottomButSize1, bottomButSize2, Page.BottomButSize.x, Page.BottomButSize.y), "WorldFactionsTab".Translate(), true, false, true))
            {
                Find.WindowStack.Add(new Dialog_FactionDuringLanding());
            }
            bottomButSize1 = bottomButSize1 + (Page.BottomButSize.x + 10f);
            if (Widgets.ButtonText(new Rect(bottomButSize1, bottomButSize2, Page.BottomButSize.x, Page.BottomButSize.y), "Next".Translate(), true, false, true) && this.CanDoNext())
            {
                this.DoNext();
            }
            bottomButSize1 = bottomButSize1 + (Page.BottomButSize.x + 10f);
            GenUI.AbsorbClicksInRect(rect);
        }

        protected override void DoNext()
        {
            Find.GameInitData.startingTile = Find.WorldInterface.SelectedTile;
            base.DoNext();
        }

        public override void DoWindowContents(Rect rect)
        {
            if (Find.WorldInterface.SelectedTile >= 0)
            {
                Find.GameInitData.startingTile = Find.WorldInterface.SelectedTile;
            }
            else if (Find.WorldSelector.FirstSelectedObject != null)
            {
                Find.GameInitData.startingTile = Find.WorldSelector.FirstSelectedObject.Tile;
            }
            this.closeOnEscapeKey = !Find.WorldRoutePlanner.Active;
        }

        public override void ExtraOnGUI()
        {
            base.ExtraOnGUI();
            Text.Anchor = TextAnchor.UpperCenter;
            base.DrawPageTitle(new Rect(0f, 5f, (float)UI.screenWidth, 300f));
            Text.Anchor = TextAnchor.UpperLeft;
            this.DoCustomBottomButtons();
        }

        public override void PostClose()
        {
            base.PostClose();
            Find.World.renderer.wantedMode = WorldRenderMode.None;
        }

        public override void PostOpen()
        {
            base.PostOpen();
            Find.GameInitData.ChooseRandomStartingTile();
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.WorldCameraMovement, OpportunityType.Important);
            TutorSystem.Notify_Event("PageStart-SelectLandingSite");
        }

        public override void PreOpen()
        {
            next = new ProPNC();
            base.PreOpen();
            Find.World.renderer.wantedMode = WorldRenderMode.Planet;
            Find.WorldInterface.Reset();
            ((MainButtonWorker_ToggleWorld)MainButtonDefOf.World.Worker).resetViewNextTime = true;
        }
    }
}
