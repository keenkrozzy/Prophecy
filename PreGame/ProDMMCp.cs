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
using Prophecy.Meta;

namespace Prophecy.PreGame
{
    [StaticConstructorOnStartup]
    internal static class ProDMMCp
    {
        static ProDMMCp()
        {
            
            HarmonyInstance DoMainMenuControlsPatch = HarmonyInstance.Create("com.Prophesy.MainMenu.DoMainMenuControlsPatch");
            MethodInfo methInfDoMainMenuControls = AccessTools.Method(typeof(MainMenuDrawer), "DoMainMenuControls", null, null);
            HarmonyMethod harmonyMethodPreFDoMainMenuControls = new HarmonyMethod(typeof(ProDMMCp).GetMethod("PreFDoMainMenuControls"));
            DoMainMenuControlsPatch.Patch(methInfDoMainMenuControls, harmonyMethodPreFDoMainMenuControls, null, null);
            Log.Message("DoMainMenuControlsPatch initialized");

        }


        public static bool PreFDoMainMenuControls(Rect rect, bool anyMapFiles)
        {


            // Shape buttons
            float floButtonHeight = rect.height / 8f;
            float floButtonPadding = floButtonHeight / 6f;
            float floButtonWidth = rect.width / 2f;

            Rect rectButtonsFrame = new Rect(rect.x, rect.y, (rect.width / 2f) - rect.width * .05f, rect.height);
            Rect rectButtons = new Rect(0f, 0f, rectButtonsFrame.width, rectButtonsFrame.height);

            GUI.BeginGroup(rectButtonsFrame);

            // make sure font is set to small
            Text.Font = GameFont.Small;

            // Menu Buttons are contained in this List
            List<ProListableOption> listableOptions = new List<ProListableOption>();

            // Tutorial Button and New Colony Button
            string str; // string needed because Tutorial doesn't exist in every language..
            if (Current.ProgramState == ProgramState.Entry)
            {
                str = ("Tutorial".CanTranslate() ? "Tutorial".Translate() : "LearnToPlay".Translate());
                listableOptions.Add(new ProListableOption(str, () => Traverse.Create(typeof(MainMenuDrawer)).Method("InitLearnToPlay"), null));
                listableOptions.Add(new ProListableOption("NewColony".Translate(), () => Find.WindowStack.Add(new ProCWP()), null));
            }

            // In-Game Save Button
            if (Current.ProgramState == ProgramState.Playing && !Current.Game.Info.permadeathMode)
            {
                listableOptions.Add(new ProListableOption("Save".Translate(), () => {
                    Traverse.Create(typeof(MainMenuDrawer)).Method("CloseMainTab");
                    Find.WindowStack.Add(new Dialog_SaveFileList_Save());
                }, null));
            }

            // Load Game Button
            if (anyMapFiles && (Current.ProgramState != ProgramState.Playing || !Current.Game.Info.permadeathMode))
            {
                listableOptions.Add(new ProListableOption("LoadGame".Translate(), () => {
                    Traverse.Create(typeof(MainMenuDrawer)).Method("CloseMainTab");
                    Find.WindowStack.Add(new Dialog_SaveFileList_Load());
                }, null));
            }

            // Review Scenario Button
            if (Current.ProgramState == ProgramState.Playing)
            {
                listableOptions.Add(new ProListableOption("ReviewScenario".Translate(), () => {
                    WindowStack windowStack = Find.WindowStack;
                    string scenario = Find.Scenario.name;
                    windowStack.Add(new Dialog_MessageBox(Find.Scenario.GetFullInformationText(), null, null, null, null, scenario, false));
                }, null));
            }

            // Options Button
            listableOptions.Add(new ProListableOption("Options".Translate(), () => {
                Traverse.Create(typeof(MainMenuDrawer)).Method("CloseMainTab");
                Find.WindowStack.Add(new Dialog_Options());
            }, "MenuButton-Options"));

            // Mods Button and Credit Button
            if (Current.ProgramState == ProgramState.Entry)
            {
                listableOptions.Add(new ProListableOption("Mods".Translate(), () => Find.WindowStack.Add(new Page_ModsConfig()), null));
                listableOptions.Add(new ProListableOption("Credits".Translate(), () => Find.WindowStack.Add(new Screen_Credits()), null));
            }

            // Quit To OS Button
            if (Current.ProgramState != ProgramState.Playing)
            {
                listableOptions.Add(new ProListableOption("QuitToOS".Translate(), () => Root.Shutdown(), null));
            }

            // Quit Buttons depending on Permadeath setting
            else if (!Current.Game.Info.permadeathMode)
            {
                Action currentGameStateIsValuable = () => {
                    if (!GameDataSaveLoader.CurrentGameStateIsValuable)
                    {
                        GenScene.GoToMainMenu();
                    }
                    else
                    {
                        Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmQuit".Translate(), () => GenScene.GoToMainMenu(), true, null));
                    }
                };
                ProListableOption listableOption = new ProListableOption("QuitToMainMenu".Translate(), currentGameStateIsValuable, null);
                listableOptions.Add(listableOption);
                Action action = () => {
                    if (!GameDataSaveLoader.CurrentGameStateIsValuable)
                    {
                        Root.Shutdown();
                    }
                    else
                    {
                        Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmQuit".Translate(), () => Root.Shutdown(), true, null));
                    }
                };
                listableOption = new ProListableOption("QuitToOS".Translate(), action, null);
                listableOptions.Add(listableOption);
            }
            else
            {
                listableOptions.Add(new ProListableOption("SaveAndQuitToMainMenu".Translate(), () => LongEventHandler.QueueLongEvent(() => {
                    GameDataSaveLoader.SaveGame(Current.Game.Info.permadeathModeUniqueName);
                    MemoryUtility.ClearAllMapsAndWorld();
                }, "Entry", "SavingLongEvent", false, null), null));
                listableOptions.Add(new ProListableOption("SaveAndQuitToOS".Translate(), () => LongEventHandler.QueueLongEvent(() => {
                    GameDataSaveLoader.SaveGame(Current.Game.Info.permadeathModeUniqueName);
                    LongEventHandler.ExecuteWhenFinished(() => Root.Shutdown());
                }, "SavingLongEvent", false, null), null));
            }

            // Draw Menu Buttons
            ProOptionListingUtility.ProDrawOptionListing(rectButtons, listableOptions, UI.screenHeight / 100f);
            GUI.EndGroup();

            
            // Configure Links
            Text.Font = GameFont.Small;
            List<ProListableOption_WebLink> listableOptions1 = new List<ProListableOption_WebLink>();
            ProListableOption_WebLink listableOptionWebLink = new ProListableOption_WebLink("FictionPrimer".Translate(), null, "https://docs.google.com/document/d/1pIZyKif0bFbBWten4drrm7kfSSfvBoJPgG9-ywfN8j8/pub", ProTBin.IconBlog);
            listableOptions1.Add(listableOptionWebLink);
            listableOptionWebLink = new ProListableOption_WebLink("LudeonBlog".Translate(), null, "http://ludeon.com/blog", ProTBin.IconBlog);
            listableOptions1.Add(listableOptionWebLink);
            listableOptionWebLink = new ProListableOption_WebLink("Forums".Translate(), null, "http://ludeon.com/forums", ProTBin.IconForums);
            listableOptions1.Add(listableOptionWebLink);
            listableOptionWebLink = new ProListableOption_WebLink("OfficialWiki".Translate(), null, "http://rimworldwiki.com", ProTBin.IconBlog);
            listableOptions1.Add(listableOptionWebLink);
            listableOptionWebLink = new ProListableOption_WebLink("TynansTwitter".Translate(), null, "https://twitter.com/TynanSylvester", ProTBin.IconTwitter);
            listableOptions1.Add(listableOptionWebLink);
            listableOptionWebLink = new ProListableOption_WebLink("TynansDesignBook".Translate(), null, "http://tynansylvester.com/book", ProTBin.IconBook);
            listableOptions1.Add(listableOptionWebLink);
            listableOptionWebLink = new ProListableOption_WebLink("HelpTranslate".Translate(), null, "http://ludeon.com/forums/index.php?topic=2933.0", ProTBin.IconForums);
            listableOptions1.Add(listableOptionWebLink);
            listableOptionWebLink = new ProListableOption_WebLink("BuySoundtrack".Translate(), null, "http://www.lasgameaudio.co.uk/#!store/t04fw", ProTBin.IconSoundtrack);
            listableOptions1.Add(listableOptionWebLink);

            // Shape Links 
            Rect rectLinksFrame = new Rect(rect.x + (rect.width / 2f), rect.y, (rect.width / 2f), (rect.height / (listableOptions1.Count + 1)) * (listableOptions1.Count - 1));
            GUI.BeginGroup(rectLinksFrame);
            Rect rectLinks = new Rect(0f, 0f, rectLinksFrame.width, rectLinksFrame.height);

            // Draw Links
            ProOptionListingUtility.ProDrawOptionListing(rectLinks, listableOptions1, (rectLinks.height / 9f) / 7f);
            GUI.EndGroup();

            // Handle language button
            if (Current.ProgramState == ProgramState.Entry)
            {
                // Configure Language button.
                List<FloatMenuOption> floatMenuOptions = new List<FloatMenuOption>();
                IEnumerator<LoadedLanguage> enumerator = LanguageDatabase.AllLoadedLanguages.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        LoadedLanguage current = enumerator.Current;
                        floatMenuOptions.Add(new FloatMenuOption(current.FriendlyNameNative, () =>
                        {
                            LanguageDatabase.SelectLanguage(current);
                            Prefs.Save();
                        }, MenuOptionPriority.Default, null, null, 0f, null, null));
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }

                // Shape language button
                Rect rectLanguageButtonFrame = new Rect(rect.x + (rect.width / 2f), rect.y + rectLinksFrame.height, (rect.width / 2f), (rect.height / (listableOptions1.Count + 1)) * 2f);
                GUI.BeginGroup(rectLanguageButtonFrame);
                float floLanguageButtonPadding = rectLanguageButtonFrame.height * .1f;
                Rect rectLanguageButton = new Rect(0f + floLanguageButtonPadding, 0f + floLanguageButtonPadding, rectLanguageButtonFrame.width - (floLanguageButtonPadding * 2f), rectLanguageButtonFrame.height - (floLanguageButtonPadding * 2f));

                // Draw language button
                if (Widgets.ButtonImage(rectLanguageButton, LanguageDatabase.activeLanguage.icon))
                {
                    Find.WindowStack.Add(new FloatMenu(floatMenuOptions));
                }
                GUI.EndGroup();
            }

            return false;
        }
    }
}
