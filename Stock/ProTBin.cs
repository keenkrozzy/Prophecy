using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Harmony;

namespace Prophesy.Stock
{
    [StaticConstructorOnStartup]
    public static class ProTBin
    {
       // public static Texture2D BGMain =  ContentFinder<Texture2D>.Get("Prophecy/Meta/BGMain", true);
        //public static Texture2D ProTitle = ContentFinder<Texture2D>.Get("Prophecy/Meta/ProTitle", true);

        public static Texture2D BGMain;
        public static Texture2D ProTitle;

        public readonly static Texture2D CloseXBig;
        public readonly static Texture2D CloseXSmall;
        public readonly static Texture2D NextBig;
        public readonly static Texture2D DeleteX;
        public readonly static Texture2D ReorderUp;
        public readonly static Texture2D ReorderDown;
        public readonly static Texture2D Plus;
        public readonly static Texture2D Minus;
        public readonly static Texture2D Suspend;
        public readonly static Texture2D SelectOverlappingNext;
        public readonly static Texture2D Info;
        public readonly static Texture2D Rename;
        public readonly static Texture2D OpenStatsReport;
        public readonly static Texture2D Copy;
        public readonly static Texture2D Paste;
        public readonly static Texture2D Drop;
        public readonly static Texture2D Ingest;
        public readonly static Texture2D DragHash;
        public readonly static Texture2D ToggleLog;
        public readonly static Texture2D OpenDebugActionsMenu;
        public readonly static Texture2D OpenInspector;
        public readonly static Texture2D OpenInspectSettings;
        public readonly static Texture2D ToggleGodMode;
        public readonly static Texture2D OpenPackageEditor;
        public readonly static Texture2D TogglePauseOnError;
        public readonly static Texture2D Add;
        public readonly static Texture2D NewItem;
        public readonly static Texture2D Reveal;
        public readonly static Texture2D Collapse;
        public readonly static Texture2D Empty;
        public readonly static Texture2D Save;
        public readonly static Texture2D NewFile;
        public readonly static Texture2D RenameDev;
        public readonly static Texture2D Reload;
        public readonly static Texture2D Play;
        public readonly static Texture2D Stop;
        public readonly static Texture2D RangeMatch;
        public readonly static Texture2D InspectModeToggle;
        public readonly static Texture2D CenterOnPointsTex;
        public readonly static Texture2D CurveResetTex;
        public readonly static Texture2D QuickZoomHor1Tex;
        public readonly static Texture2D QuickZoomHor100Tex;
        public readonly static Texture2D QuickZoomHor20kTex;
        public readonly static Texture2D QuickZoomVer1Tex;
        public readonly static Texture2D QuickZoomVer100Tex;
        public readonly static Texture2D QuickZoomVer20kTex;
        public readonly static Texture2D IconBlog;
        public readonly static Texture2D IconForums;
        public readonly static Texture2D IconTwitter;
        public readonly static Texture2D IconBook;
        public readonly static Texture2D IconSoundtrack;
        public readonly static Texture2D ShowLearningHelper;
        public readonly static Texture2D ShowZones;
        public readonly static Texture2D ShowEnvironment;
        public readonly static Texture2D ShowColonistBar;
        public readonly static Texture2D ShowRoofOverlay;
        public readonly static Texture2D AutoHomeArea;
        public readonly static Texture2D CategorizedResourceReadout;
        public readonly static Texture2D LockNorthUp;
        public readonly static Texture2D UsePlanetDayNightSystem;
        public readonly static Texture2D ExpandingIcons;
        public readonly static Texture2D[] SpeedButtonTextures;

		// Textures for PNC_Cards
		public static Texture2D texPassionMinorIcon;
		public static Texture2D texPassionMajorIcon;
		public static Texture2D texSkillBarFill;
		public static Texture2D texSkillBarFillMinorPassion;
		public static Texture2D texSkillBarFillMajorPassion;
		public static Texture2D texWeaponsTab;
		public static Texture2D texFoodsTab;
		public static Texture2D texApparelTab;
		public static Texture2D texDrugsTab;
		public static Texture2D texResourcesTab;
		public static Texture2D texItemsTab;
		public static Texture2D texWeaponsTab_Hover;
		public static Texture2D texVellum;


		static ProTBin()
        {
            LoadTextures();

            CloseXBig = ContentFinder<Texture2D>.Get("UI/Widgets/CloseX", true);
            CloseXSmall = ContentFinder<Texture2D>.Get("UI/Widgets/CloseXSmall", true);
            NextBig = ContentFinder<Texture2D>.Get("UI/Widgets/NextArrow", true);
            DeleteX = ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true);
            ReorderUp = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderUp", true);
            ReorderDown = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderDown", true);
            Plus = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);
            Minus = ContentFinder<Texture2D>.Get("UI/Buttons/Minus", true);
            Suspend = ContentFinder<Texture2D>.Get("UI/Buttons/Suspend", true);
            SelectOverlappingNext = ContentFinder<Texture2D>.Get("UI/Buttons/SelectNextOverlapping", true);
            Info = ContentFinder<Texture2D>.Get("UI/Buttons/InfoButton", true);
            Rename = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);
            OpenStatsReport = ContentFinder<Texture2D>.Get("UI/Buttons/OpenStatsReport", true);
            Copy = ContentFinder<Texture2D>.Get("UI/Buttons/Copy", true);
            Paste = ContentFinder<Texture2D>.Get("UI/Buttons/Paste", true);
            Drop = ContentFinder<Texture2D>.Get("UI/Buttons/Drop", true);
            Ingest = ContentFinder<Texture2D>.Get("UI/Buttons/Ingest", true);
            DragHash = ContentFinder<Texture2D>.Get("UI/Buttons/DragHash", true);
            ToggleLog = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleLog", true);
            OpenDebugActionsMenu = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenDebugActionsMenu", true);
            OpenInspector = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspector", true);
            OpenInspectSettings = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspectSettings", true);
            ToggleGodMode = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleGodMode", true);
            OpenPackageEditor = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenPackageEditor", true);
            TogglePauseOnError = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/TogglePauseOnError", true);
            Add = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Add", true);
            NewItem = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/NewItem", true);
            Reveal = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Reveal", true);
            Collapse = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Collapse", true);
            Empty = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Empty", true);
            Save = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Save", true);
            NewFile = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/NewFile", true);
            RenameDev = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Rename", true);
            Reload = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Reload", true);
            Play = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Play", true);
            Stop = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Stop", true);
            RangeMatch = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/RangeMatch", true);
            InspectModeToggle = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/InspectModeToggle", true);
            CenterOnPointsTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/CenterOnPoints", true);
            CurveResetTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/CurveReset", true);
            QuickZoomHor1Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor1", true);
            QuickZoomHor100Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor100", true);
            QuickZoomHor20kTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor20k", true);
            QuickZoomVer1Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer1", true);
            QuickZoomVer100Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer100", true);
            QuickZoomVer20kTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer20k", true);
            IconBlog = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Blog", true);
            IconForums = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Forums", true);
            IconTwitter = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Twitter", true);
            IconBook = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Book", true);
            IconSoundtrack = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Soundtrack", true);
            ShowLearningHelper = ContentFinder<Texture2D>.Get("UI/Buttons/ShowLearningHelper", true);
            ShowZones = ContentFinder<Texture2D>.Get("UI/Buttons/ShowZones", true);
            ShowEnvironment = ContentFinder<Texture2D>.Get("UI/Buttons/ShowEnvironment", true);
            ShowColonistBar = ContentFinder<Texture2D>.Get("UI/Buttons/ShowColonistBar", true);
            ShowRoofOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowRoofOverlay", true);
            AutoHomeArea = ContentFinder<Texture2D>.Get("UI/Buttons/AutoHomeArea", true);
            CategorizedResourceReadout = ContentFinder<Texture2D>.Get("UI/Buttons/ResourceReadoutCategorized", true);
            LockNorthUp = ContentFinder<Texture2D>.Get("UI/Buttons/LockNorthUp", true);
            UsePlanetDayNightSystem = ContentFinder<Texture2D>.Get("UI/Buttons/UsePlanetDayNightSystem", true);
            ExpandingIcons = ContentFinder<Texture2D>.Get("UI/Buttons/ExpandingIcons", true);
            SpeedButtonTextures = new Texture2D[] { ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Pause", true), ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Normal", true), ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Fast", true), ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast", true), ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast", true) };

			
	}

        private static void LoadTextures()
        {
            BGMain = ContentFinder<Texture2D>.Get("Prophecy/Meta/BGMain", true);
            ProTitle = ContentFinder<Texture2D>.Get("Prophecy/Meta/ProTitle", true);

            try
            {
                Traverse.CreateWithType("UI_BackgroundMain").Field("BGPlanet").SetValue(BGMain);
            }
            catch
            {
                Log.Message("ProTBin.LoadTextures: Failed to Traverse BGPlanet");
            }

			// Textures for PNC_Cards
			texPassionMinorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMinor", true);
			texPassionMajorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMajor", true);
			texSkillBarFill = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));
			texSkillBarFillMinorPassion = BumpGradient(new Color(1f, 1f, 0f, .4f), new Color(1f, 1f, 1f, 0.1f));
			texSkillBarFillMajorPassion = BumpGradient(new Color(1f, 0f, 0f, .4f), new Color(1f, 1f, 1f, 0.1f));
			
			texWeaponsTab = ContentFinder<Texture2D>.Get("Prophecy/PreGame/WeaponsTab", true);
			texWeaponsTab.wrapMode = TextureWrapMode.Clamp;

			texFoodsTab = ContentFinder<Texture2D>.Get("Prophecy/PreGame/Tab_Foods", true);
			texFoodsTab.wrapMode = TextureWrapMode.Clamp;

			texApparelTab = ContentFinder<Texture2D>.Get("Prophecy/PreGame/Tab_Apparel", true);
			texApparelTab.wrapMode = TextureWrapMode.Clamp;

			texDrugsTab = ContentFinder<Texture2D>.Get("Prophecy/PreGame/Tab_Drugs", true);
			texDrugsTab.wrapMode = TextureWrapMode.Clamp;

			texResourcesTab = ContentFinder<Texture2D>.Get("Prophecy/PreGame/Tab_Resources", true);
			texResourcesTab.wrapMode = TextureWrapMode.Clamp;

			texItemsTab = ContentFinder<Texture2D>.Get("Prophecy/PreGame/Tab_Items", true);
			texItemsTab.wrapMode = TextureWrapMode.Clamp;

			texWeaponsTab_Hover = ContentFinder<Texture2D>.Get("Prophecy/PreGame/WeaponsTab_Hover", true);
			texWeaponsTab_Hover.wrapMode = TextureWrapMode.Clamp;

			texVellum = ContentFinder<Texture2D>.Get("Prophecy/PreGame/Vellum", true);
			texVellum.wrapMode = TextureWrapMode.Repeat;

		}

		private static Texture2D BumpGradient(Color _lowColor, Color _highColor)
		{
			Texture2D tex = new Texture2D(16, 16);

			for (int x = 0; x < 16; x++)
			{
				for (int y = 0; y < 16; y++)
				{
					float x1 = 0;
					float y1 = 0;

					switch (x)
					{
						case 0: case 1:	case 2:	case 3:	case 4:	case 5:
						{
							x1 = (float)x / 6f;
							break;
						}
						case 6:	case 7:	case 8:	case 9:
						{
							x1 = 1f;
							break;
						}
						case 10: case 11: case 12: case 13: case 14: case 15:
						{
							x1 = ((float)x - 15f) / -6f;
							break;
						}
					}

					switch (y)
					{
						case 0:	case 1:	case 2:	case 3:	case 4:	case 5:
						{
							y1 = (float)y / 6f;
							break;
						}
						case 6:	case 7:	case 8:	case 9:
						{
							y1 = 1f;
							break;
						}
						case 10: case 11: case 12: case 13: case 14: case 15:
						{
							y1 = ((float)y - 15f) / -6f;
							break;
						}
					}
					x1 = Math.Abs(x1);
					y1 = Math.Abs(y1);
					float final = 0f;
					if (x1 <= y1)
					{
						final = x1;
					}
					else
					{
						final = y1;
					}
					tex.SetPixel(x, y, Color.Lerp(_lowColor, _highColor, final));
				}
			}
			tex.Apply();
			return tex;
		}
    }
}
