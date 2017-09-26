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
    [StaticConstructorOnStartup]
    internal static class ProUIBGMp
    {
        static ProUIBGMp()
        {

            HarmonyInstance UI_BackgroundMainPatch = HarmonyInstance.Create("com.Prophesy.MainMenu.UI_BackgroundMainPatch");
            MethodInfo methInfBackgroundOnGUI = AccessTools.Method(typeof(UI_BackgroundMain), "BackgroundOnGUI", null, null);
            HarmonyMethod harmonyMethodPreFBackgroundOnGUI = new HarmonyMethod(typeof(ProUIBGMp).GetMethod("PreFBackgroundOnGUI"));
            UI_BackgroundMainPatch.Patch(methInfBackgroundOnGUI, harmonyMethodPreFBackgroundOnGUI, null, null);
            Log.Message("UI_BackgroundMainPatch initialized");
        }

        /**************************************************************************************************************************************************
        * In addition to shaping the BG, this patch is making the static constructor fire again, since you can't stop a static constructor from firing. *
        * ProTBin changes the fields, then this Prefix makes the cctor fire again with the new values.                                                    *
        **************************************************************************************************************************************************/

        public static bool PreFBackgroundOnGUI()
        {

            // Shape the BG
            float floRatio = UI.screenWidth / 2048f;
            float floHeight = 1280f * floRatio;
            float floYPos = (UI.screenHeight - floHeight) / 2f;
            Rect rectBG = new Rect(0f, floYPos, UI.screenWidth, floHeight);

            // Draw the BG
            GUI.DrawTexture(rectBG, Traverse.Create(typeof(UI_BackgroundMain)).Field("BGPlanet").GetValue<Texture2D>(), ScaleMode.ScaleToFit);

            return false;
        }
    }
}
