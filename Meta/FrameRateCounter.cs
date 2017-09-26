using UnityEngine;
using Verse;
using System.Reflection;
using Harmony;

namespace Prophecy.Meta
{
    [StaticConstructorOnStartup]
    public static class FrameRateCounter
    {
        static float updateInterval = 1f;

        private static float accum = 0;
        private static int frames = 0;
        private static float timeleft;
        private static float fps = 0;
        static FrameRateCounter()
        {
            HarmonyInstance UIRootOnGUIPatch = HarmonyInstance.Create("com.KeenKrozzy.UIRootOnGUI.FrameRateCounterPatch");
            MethodInfo methInfUIRootOnGUI = AccessTools.Method(typeof(UIRoot), "UIRootOnGUI", null, null);
            HarmonyMethod harmonyMethodPostFUIRootOnGUI = new HarmonyMethod(typeof(FrameRateCounter).GetMethod("PostFUIRootOnGUI"));
            UIRootOnGUIPatch.Patch(methInfUIRootOnGUI, null, harmonyMethodPostFUIRootOnGUI, null);



            timeleft = updateInterval;
            Log.Message("FrameRateCounter has been initialized.");
        }

        public static void PostFUIRootOnGUI()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            if (timeleft <= 0.0)
            {
                fps = accum / frames;               
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }

            Display(fps.ToString("N0") + " FPS");
        }

        private static void Display(string _str)
        {

            Text.Font = GameFont.Small;
            GUI.color = new Color(1f, 1f, 1f, 0.5f);

			//Rect rectWindow = new Rect((UI.screenWidth / 2) - 50, UI.screenHeight / 2, Text.CalcSize(_str).x + 10f, Text.CalcHeight(_str, 100f));
			Rect rectWindow = new Rect(0f, 0f, Text.CalcSize(_str).x + 10f, Text.CalcHeight(_str, 100f));
			Rect rectFPS = new Rect(5f, 0f, Text.CalcSize(_str).x, Text.CalcHeight(_str, 100f));

            Find.WindowStack.ImmediateWindow(1987, rectWindow, WindowLayer.GameUI, () => Widgets.Label(rectFPS, _str));

            GUI.color = Color.white;
        }

    }
}
