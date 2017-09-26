using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Harmony;
using RimWorld;
using Verse;
using Verse.Profile;
using UnityEngine;
using Prophecy.Stock;


namespace Prophecy.PreGame
{

    /******************************************************************************************************************
    * This Class is patching MainMenuDrawer.MainMenuOnGUI is order to add Prophecy title and change main menu layout. * 
    ******************************************************************************************************************/

    [StaticConstructorOnStartup]
    internal static class ProMMOGUI
    {
        private readonly static Vector2 PaneSize;
        private readonly static Vector2 TitleSize;
        private readonly static Vector2 ProTitleSize;
        private readonly static Texture2D TexTitle;
        private readonly static Vector2 LudeonLogoSize;
        private readonly static Texture2D TexLudeonLogo;

        static ProMMOGUI()
        {
            PaneSize = new Vector2(450f, 450f);
            TitleSize = new Vector2(1032f, 146f);
            ProTitleSize = new Vector2(450f, 146f);
            TexTitle = ContentFinder<Texture2D>.Get("UI/HeroArt/GameTitle", true);
            LudeonLogoSize = new Vector2(200f, 58f);
            TexLudeonLogo = ContentFinder<Texture2D>.Get("UI/HeroArt/LudeonLogoSmall", true);

            HarmonyInstance MainMenuOnGUIPatch = HarmonyInstance.Create("com.Prophesy.MainMenu.MainMenuOnGUIPatch");
            MethodInfo methInfMainMenuOnGUI = AccessTools.Method(typeof(MainMenuDrawer), "MainMenuOnGUI", null, null);
            HarmonyMethod harmonyMethodPreFMainMenuOnGUI = new HarmonyMethod(typeof(ProMMOGUI).GetMethod("PreFMainMenuOnGUI"));
            HarmonyMethod harmonyMethodPostFMainMenuOnGUI = new HarmonyMethod(typeof(ProMMOGUI).GetMethod("PostFMainMenuOnGUI"));
            MainMenuOnGUIPatch.Patch(methInfMainMenuOnGUI, harmonyMethodPreFMainMenuOnGUI, null, null);
            Log.Message("MainMenuOnGUIPatch initialized");

        }

        public static bool PreFMainMenuOnGUI()
        {
            VersionControl.DrawInfoInCorner();

            // Calculate main menu frame
            float floFramePadding = 25f;
            float floFrameWidth = (UI.screenWidth - (UI.screenWidth / 1.62f)) / 1.62f;
            float floFrameHeight = (UI.screenHeight / 1.62f);
            float floFrameX = UI.screenWidth - (floFrameWidth + floFramePadding);
            float floFrameY = (UI.screenHeight - floFrameHeight) / 2f;

            float floAspect = (float)UI.screenWidth / (float)UI.screenHeight;

            if (floAspect > 2f)
            {
                floFrameWidth = floFrameHeight * .765f;
                floFrameX = UI.screenWidth - (floFrameWidth + floFramePadding);
            }

            if (floAspect < 1.25)
            {
                floFrameWidth = floFrameHeight * .477f;
                floFrameX = UI.screenWidth - (floFrameWidth + floFramePadding);
            }


            // Shape RimWorld Title.
            float floTexTitleWidth = floFrameWidth - (floFrameWidth / 1.62f);
            float floTexTitleHeight = (floTexTitleWidth / TitleSize.x) * TitleSize.y;

            // Draw Rimworld Title.
            Rect rectTexTitle = new Rect(floFrameX + (floFrameWidth * .05f),floFrameY ,floTexTitleWidth ,floTexTitleHeight );
            GUI.DrawTexture(rectTexTitle, TexTitle, ScaleMode.StretchToFill, true);


            // Shape Prophecy Title.
            float floProTitleWidth = floFrameWidth;
            float floProTitleHeight = (floProTitleWidth / ProTitleSize.x) * ProTitleSize.y;

            // Draw Prophecy Title.
            Rect rectProTitle = new Rect(floFrameX, (floFrameY + floTexTitleHeight) - floProTitleHeight * .1f, floProTitleWidth, floProTitleHeight);
            GUI.DrawTexture(rectProTitle, ProTBin.ProTitle, ScaleMode.StretchToFill, true);


            // Shape main page credit.           
            if (UI.screenWidth > 1700f)
            {
                Text.Font = GameFont.Medium;
            }
            else
            {
                Text.Font = GameFont.Small;
            }
            Text.Anchor = TextAnchor.UpperLeft;
            string str = "MainPageCredit".Translate();
            float floMPCWidth = floFrameWidth;
            float floMPCHeight = Text.CalcHeight(str, floMPCWidth);

            // Draw main page credit.
            Rect rectMPC = new Rect(floFrameX, rectProTitle.y + floProTitleHeight, floMPCWidth, floMPCHeight);
            Widgets.Label(rectMPC, str);


            // Revert GUI text size.
            Text.Font = GameFont.Small;


            // Shape main menu controls.
            float floMMCHeight = floFrameHeight - floMPCHeight - floProTitleHeight - floTexTitleHeight;


            // Draw main menu controls.
            Rect rectMMC = new Rect(floFrameX, rectMPC.y + (floMPCHeight * 2f), floFrameWidth, floMMCHeight);
            MainMenuDrawer.DoMainMenuControls(rectMMC, Traverse.CreateWithType("MainMenuDrawer").Field("anyMapFiles").GetValue<bool>());


            // Shape Ludeon logo.
            GUI.color = new Color(1f, 1f, 1f, 0.5f);
            float ludeonLogoSize = (float)(UI.screenWidth - 8) - LudeonLogoSize.x;
            float ludeonLogoSize1 = LudeonLogoSize.x;
            Vector2 ludeonLogoSize2 = LudeonLogoSize;

            // Draw Ludeon logo.
            Rect rect4 = new Rect(ludeonLogoSize, 8f, ludeonLogoSize1, ludeonLogoSize2.y);
            GUI.DrawTexture(rect4, TexLudeonLogo, ScaleMode.StretchToFill, true);


            // Revert GUI text color
            GUI.color = Color.white;


            return false;
        }
    }
}
