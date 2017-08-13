using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using UnityEngine;
using Prophesy.Meta;
using System.Text.RegularExpressions;
using Prophesy.Stock;

namespace Prophesy.PreGame
{
	public class PNC_Card
	{
		private Pawn pawn;
		private Vector2 PawnPortraitSize = new Vector2(100f, 140f);
		public Rect rectCard;
		private int intNumCards;
		//public int intCardPos;
		public ePNC_Card_Type eCardType;
		private Regex validNameRegex = new Regex("^[a-zA-Z0-9 '\\-]*$");
		private Vector2 v2BackgroundsScrollPosition = Vector2.zero;
		private ColorInt cintBorder = new ColorInt(135, 135, 135);
		private int intCurTab = 0;
		private Texture2D[] atexTabs = new Texture2D[6] { ProTBin.texFoodsTab, ProTBin.texApparelTab, ProTBin.texWeaponsTab, ProTBin.texDrugsTab, ProTBin.texResourcesTab, ProTBin.texItemsTab };
		private bool[] aboolCurTab = { true, false, false, false, false, false };
		private string[] astrTabTooltip = { "Foods", "Apparel", "Weapons", "Drugs", "Resources", "Items" };

		public PNC_Card(Pawn _pawn = null, int _intNumCards = 0)
		{
			if (_pawn != null)
			{
				pawn = _pawn;
				eCardType = ePNC_Card_Type.Pawn;
			}
			else
			{
				eCardType = ePNC_Card_Type.Items;
			}

			intNumCards = _intNumCards; 
		}

		/**************
		* Root Method *
		**************/
		public void DrawCard(Rect _rect)
		{
			Widgets.DrawMenuSection(_rect, true);
			rectCard = _rect;
			//Log.Message("PNC_Card " + GetLabel() + _rect.ToString());

			// For design guiding
			//DoGrid();

			if (eCardType == ePNC_Card_Type.Pawn)
			{
				DrawLabel(_rect);
				DrawPawnPortrait();
				DoNameInput();
				DoMainDesc();
				DoBackstories();
				DoSkills();
				DoTraits();
			}

			if (eCardType == ePNC_Card_Type.Items)
			{
				DrawLabel(_rect);
				DoTabs();
			}




		}

		public void DrawCardTopOnly(Rect _rect)
		{
			Widgets.DrawMenuSection(_rect, true);
			rectCard = _rect;
			DrawLabel(_rect);
		}

		private void DrawLabel(Rect _rect)
		{
			Rect rect = new Rect(_rect.x + _rect.width * .05f, _rect.y, 0f, 0f);
			Text.Font = GameFont.Medium;

			if (eCardType == ePNC_Card_Type.Pawn)
			{
				Vector2 vect2 = new Vector2();
				vect2 = Text.CalcSize(pawn.Label);

				rect.width = vect2.x;
				rect.height = vect2.y;

				Widgets.Label(rect, pawn.Label);
			}
			else if (eCardType == ePNC_Card_Type.Items)
			{
				Vector2 vect2 = new Vector2();
				vect2 = Text.CalcSize("Items");

				rect.width = vect2.x;
				rect.height = vect2.y;

				Widgets.Label(rect, "Items");
			}

			Text.Font = GameFont.Small;
		}

		private void DrawPawnPortrait()
		{

			
				Rect rect = new Rect(rectCard.x + (rectCard.width * .85f), rectCard.y + (rectCard.height * .05f), PawnPortraitSize.x, PawnPortraitSize.y);

				//Vector2 vector21 = PawnPortraitSize;
				Vector3 vector3 = new Vector3();
				GUI.DrawTexture(rect, PortraitsCache.Get(pawn, PawnPortraitSize, vector3, 1f));
			
		}

		private void DoNameInput()
		{
				/************************************************************
				* Draw Name input text boxes with text set to current name. *
				************************************************************/
				NameTriple nameTriple = pawn.Name as NameTriple;
				string first = nameTriple.First;
				string nick = nameTriple.Nick;
				string last = nameTriple.Last;

				Rect rect = new Rect(rectCard.x + (rectCard.width * .85f), rectCard.y + (rectCard.height * .26f), PawnPortraitSize.x, PawnPortraitSize.y * .2f);
				Rect rectOffset = new Rect(0f,PawnPortraitSize.y * .22f,0f,0f);

				string strFirst = Widgets.TextField(rect, first);
				if (strFirst.Length <= 12 && validNameRegex.IsMatch(strFirst))
				{
					first = strFirst;
				}

				string strNick = Widgets.TextField(KrozzyUtilities.RectAddition(rect, rectOffset), nick);
				if (strNick.Length <= 9 && validNameRegex.IsMatch(strNick))
				{
					nick = strNick;
				}

				string strLast = Widgets.TextField(KrozzyUtilities.RectAddition(KrozzyUtilities.RectAddition(rect, rectOffset),rectOffset), last);
				if (strLast.Length <= 12 && validNameRegex.IsMatch(strLast))
				{
					last = strLast;
				}

				if (nameTriple.First != first || nameTriple.Nick != nick || nameTriple.Last != last)
				{
					pawn.Name = new NameTriple(first, nick, last);
				}

				TooltipHandler.TipRegion(rect, "FirstNameDesc".Translate());
				TooltipHandler.TipRegion(KrozzyUtilities.RectAddition(rect, rectOffset), "ShortIdentifierDesc".Translate());
				TooltipHandler.TipRegion(KrozzyUtilities.RectAddition(KrozzyUtilities.RectAddition(rect, rectOffset), rectOffset), "LastNameDesc".Translate());

			

		}

		private void DoMainDesc()
		{
			string strMainDesc = pawn.MainDesc(true);
			Rect rect = new Rect(rectCard.width * .05f, rectCard.height * .05f, Text.CalcSize(strMainDesc).x, Text.CalcSize(strMainDesc).y);
			Widgets.Label(KrozzyUtilities.RectAddition(rect, rectCard),strMainDesc);
			TooltipHandler.TipRegion(KrozzyUtilities.RectAddition(rect, rectCard), () => pawn.ageTracker.AgeTooltipString, 6873641);

		}

		private void DoBackstories()
		{
			// Shape backtories area
			float floBSwidth = rectCard.width * .525f;
			float floBSheight = rectCard.height * .59f;

			// Shape and draw backstory Label
			Text.Font = GameFont.Medium;
			string strBSLabel = "Backstory".Translate();
			Rect rect = new Rect(rectCard.width * .025f, rectCard.height * .09f, Text.CalcSize(strBSLabel).x, Text.CalcSize(strBSLabel).y);
			Widgets.Label(KrozzyUtilities.RectAddition(rect, rectCard), strBSLabel);
			Text.Font = GameFont.Small;

			
			// Get texts and text sizes for scroll view
			string strBSChildhood = "Childhood".Translate() + ": ";
			Vector2 v2BSChildhood = Text.CalcSize(strBSChildhood);
			string strBSChildhoodTitle = pawn.story.GetBackstory(BackstorySlot.Childhood).Title;
			Vector2 v2BSChildhoodTitle = Text.CalcSize(strBSChildhoodTitle);
			string strBSChildhoodDesc = pawn.story.GetBackstory(BackstorySlot.Childhood).FullDescriptionFor(pawn);		
			float floBSChildhoodDesc = Text.CalcHeight(strBSChildhoodDesc, floBSwidth -16f); // Scrollbar has 16f hard number

			string strBSAdulthood = "";
			Vector2 v2BSAdulthood = new Vector2();
			string strBSAdulthoodTitle = "";
			Vector2 v2BSAdulthoodTitle = new Vector2();
			string strBSAdulthoodDesc = "";
			float floBSAdulthoodDesc = 0f;
			if (pawn.story.GetBackstory(BackstorySlot.Adulthood) != null)
			{
				strBSAdulthood = "Adulthood".Translate() + ": ";
				v2BSAdulthood = Text.CalcSize(strBSAdulthood);
				strBSAdulthoodTitle = pawn.story.GetBackstory(BackstorySlot.Adulthood).Title;
				v2BSAdulthoodTitle = Text.CalcSize(strBSAdulthoodTitle);
				strBSAdulthoodDesc = pawn.story.GetBackstory(BackstorySlot.Adulthood).FullDescriptionFor(pawn);
				floBSAdulthoodDesc = Text.CalcHeight(strBSAdulthoodDesc, floBSwidth -16f); // Scrollbar has 16f hard number
			}

			// Shape and draw scroll view
			Rect rectOut = new Rect(rectCard.width * .025f, rectCard.height * .155f, floBSwidth, floBSheight);
			Rect rectView = new Rect(0f, 0f, rectOut.width -16f, v2BSChildhood.y + v2BSChildhoodTitle.y + floBSChildhoodDesc + v2BSAdulthood.y + v2BSAdulthoodTitle.y + floBSAdulthoodDesc);
			GUI.DrawTexture(KrozzyUtilities.RectAddition(rectOut, rectCard), SolidColorMaterials.NewSolidColorTexture(new Color(.125f, .125f, .125f, .5f)));
			Widgets.BeginScrollView(KrozzyUtilities.RectAddition(rectOut, rectCard), ref v2BackgroundsScrollPosition, rectView);

			// Shape and draw childhood label
			Rect rect1 = new Rect(0f, 0f, v2BSChildhood.x, v2BSChildhood.y);
			Widgets.Label(rect1, strBSChildhood);

			// Shape and draw childhood title
			Rect rect2 = new Rect(rect1.width, 0f, v2BSChildhoodTitle.x, v2BSChildhoodTitle.y);
			Widgets.Label(rect2, strBSChildhoodTitle);

			// Shape and draw childhood description
			Rect rect3 = new Rect(0f, rect1.height, floBSwidth -16f, floBSChildhoodDesc);
			Widgets.Label(rect3, strBSChildhoodDesc);

			if (pawn.story.GetBackstory(BackstorySlot.Adulthood) != null)
			{
				// Shape and draw adult label
				Rect rect4 = new Rect(0f, rect3.y + rect3.height + (rectCard.height * .05f), v2BSAdulthood.x, v2BSAdulthood.y);
				Widgets.Label(rect4, strBSAdulthood);

				// Shape and draw adult title
				Rect rect5 = new Rect(rect4.width, rect3.y + rect3.height + (rectCard.height * .05f), v2BSAdulthoodTitle.x, v2BSAdulthoodTitle.y);
				Widgets.Label(rect5, strBSAdulthoodTitle);

				// Shape and draw adult description
				Rect rect6 = new Rect(0f, rect4.y + rect4.height, floBSwidth -16f, floBSAdulthoodDesc);
				Widgets.Label(rect6, strBSAdulthoodDesc);
			}

			Widgets.EndScrollView();
		}

		private void DoSkills()
		{
			// Shape Skills area
			float floSkillsX = rectCard.width * .575f;
			float floSkillsY = rectCard.height * .09f;
			float floSkillsWidth = rectCard.width * .25f;
			float floSkillsHeight = rectCard.height * .65f;
			float floTextHeight = floSkillsHeight / 13f;
			float floAdjustY = 0f;

			// Shape and draw skills label
			Text.Font = GameFont.Medium;
			string strSkillsLabel = "Skills".Translate();
			Rect rectSkillsLabel = new Rect(floSkillsX, floSkillsY, Text.CalcSize(strSkillsLabel).x, floTextHeight);
			floAdjustY = floTextHeight + (rectCard.height * .01f);
			Widgets.Label(KrozzyUtilities.RectAddition(rectSkillsLabel, rectCard), strSkillsLabel);
			Text.Font = GameFont.Small;

			// Prepare and shape skill list
			float floMaxSkillWidth = 0f;
			float floSkillLevelBarX = 0f;
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				float x = Text.CalcSize(allDefsListForReading[i].skillLabel).x;
				if (x > floMaxSkillWidth)
				{
					floMaxSkillWidth = x;
				}
			}
			floSkillLevelBarX = floMaxSkillWidth;

			// Shape and draw skills
			Text.Anchor = TextAnchor.MiddleLeft;
			for (int j = 0; j < pawn.skills.skills.Count; j++)
			{
				
				string strSkillLabel = pawn.skills.skills[j].def.skillLabel;
				Rect rectSkillLabel = new Rect(floSkillsX, floSkillsY + floAdjustY, Text.CalcSize(strSkillLabel).x, floTextHeight);
				Widgets.Label(KrozzyUtilities.RectAddition(rectSkillLabel, rectCard), strSkillLabel);

				// Shape and draw Passion Icon
				if (pawn.skills.skills[j].passion > Passion.None)
				{
					Rect rectPassion = new Rect(floSkillsX + floSkillLevelBarX, floSkillsY + floAdjustY, floTextHeight, floTextHeight);
					Texture2D texPassion = (pawn.skills.skills[j].passion != Passion.Major) ? ProTBin.texPassionMinorIcon : ProTBin.texPassionMajorIcon;
					GUI.DrawTexture(KrozzyUtilities.RectAddition(rectPassion, rectCard), texPassion);
				}

				// Shape and draw skill bar
				if (!pawn.skills.skills[j].TotallyDisabled)
				{
					Rect rectSkillBar = new Rect(floSkillsX + floSkillLevelBarX + floTextHeight, floSkillsY + floAdjustY, floSkillsWidth - floSkillLevelBarX, floTextHeight);
					float fillPercent = Mathf.Max(0.01f, (float)pawn.skills.skills[j].Level / 20f);
					if (pawn.skills.skills[j].passion == Passion.None)
					{
						Widgets.FillableBar(KrozzyUtilities.RectAddition(rectSkillBar, rectCard), fillPercent, ProTBin.texSkillBarFill, null, false);
					}
					if (pawn.skills.skills[j].passion > Passion.None)
					{
						Texture2D texPassionOverlay = (pawn.skills.skills[j].passion != Passion.Major) ? ProTBin.texSkillBarFillMinorPassion : ProTBin.texSkillBarFillMajorPassion;
						Widgets.FillableBar(KrozzyUtilities.RectAddition(rectSkillBar, rectCard), fillPercent, texPassionOverlay, null, false);
					}
					Widgets.Label(KrozzyUtilities.RectAddition(rectSkillBar, rectCard), pawn.skills.skills[j].Level.ToString());
				}


				floAdjustY += floTextHeight;

			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		private void DoTraits()
		{
			// Shape Traits area
			float floSkillsX = rectCard.width * .85f;
			float floSkillsY = rectCard.height * .5f;
			float floSkillsWidth = rectCard.width * .125f;
			float floSkillsHeight = rectCard.height * .25f;
			float floTextHeight = floSkillsHeight / 4f;
			float floAdjustY = 0f;

			Text.Font = GameFont.Medium;
			Rect rectTraits = new Rect(floSkillsX, floSkillsY, Text.CalcSize("Traits").x, floTextHeight);
			Widgets.Label(KrozzyUtilities.RectAddition(rectTraits, rectCard), "Traits".Translate());
			Text.Font = GameFont.Small;
			floAdjustY += floTextHeight;

			Text.Anchor = TextAnchor.MiddleLeft;
			for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
			{
				Trait trait = pawn.story.traits.allTraits[i];
				Rect rectTrait = new Rect(floSkillsX, floSkillsY + floAdjustY, Text.CalcSize(trait.LabelCap).x, floTextHeight);
				if (Mouse.IsOver(KrozzyUtilities.RectAddition(rectTrait, rectCard)))
				{
					Widgets.DrawHighlight(KrozzyUtilities.RectAddition(rectTrait, rectCard));
				}
				Widgets.Label(KrozzyUtilities.RectAddition(rectTrait, rectCard), trait.LabelCap);
				Trait trLocal = trait;
				TipSignal tip = new TipSignal(() => trLocal.TipString(pawn), (int)floAdjustY * 17);
				TooltipHandler.TipRegion(KrozzyUtilities.RectAddition(rectTrait, rectCard), tip);

				floAdjustY += floTextHeight;
			}
			Text.Anchor = TextAnchor.UpperLeft;

		}

		private void DoTabs()
		{
			// Calculate variables		
			float floMenuHeight = rectCard.height * .8f;
			float floYAdjust = floMenuHeight * .1f;

			// Do unfocused tabs
			for (int x = 5; x > -1; x--)
			{
				if (aboolCurTab[x] == false)
				{
					// Shape the tab
					Rect rectTabUnfocused = new Rect(rectCard.width * .025f + 1f, (rectCard.height * .1f)+ (floMenuHeight * .1f) + (floYAdjust * (float)x), rectCard.width * .05f, rectCard.width * .075f);

					// Draw the tab
					Widgets.DrawTextureFitted(KrozzyUtilities.RectAddition(rectTabUnfocused, rectCard), atexTabs[x], 1f);

					// Shape the tab button
					Rect rectTabUnfocusedButton = new Rect(rectTabUnfocused.x, rectTabUnfocused.y + (rectTabUnfocused.height * .34f), rectTabUnfocused.width, rectTabUnfocused.height - (rectTabUnfocused.height * .34f));

					// Draw the tab button


					// Do tooltip
					if (Mouse.IsOver(KrozzyUtilities.RectAddition(rectTabUnfocusedButton, rectCard)))
					{
						KrozzyUtilities.Tooltip(Find.WindowStack.currentlyDrawnWindow.windowRect, astrTabTooltip[x]);
					}
				}
			}

			// Shape the menu section
			Rect rectMenuSection = new Rect(rectCard.width * .075f, rectCard.height * .1f, rectCard.width * .3f, floMenuHeight);

			// Draw the menu section
			GUI.DrawTexture(KrozzyUtilities.RectAddition(rectMenuSection, rectCard), ProTBin.texVellum, ScaleMode.ScaleAndCrop);
			GUI.color = cintBorder.ToColor;
			Widgets.DrawBox(KrozzyUtilities.RectAddition(rectMenuSection, rectCard), 1);
			GUI.color = Color.white;

			// Do focused tab
			for (int x = 0; x < 6; x++)
			{
				if (aboolCurTab[x] == true)
				{
					// Shape the tab
					Rect rectTabFocused = new Rect(rectCard.width * .025f + 1f, (rectCard.height * .1f) + (floMenuHeight * .1f) + (floYAdjust * (float)x), rectCard.width * .05f, rectCard.width * .075f);

					// Draw the tab
					Widgets.DrawTextureFitted(KrozzyUtilities.RectAddition(rectTabFocused, rectCard), atexTabs[x], 1f);

					// Shape the tab button
					Rect rectTabfocusedButton = new Rect(rectTabFocused.x, rectTabFocused.y + (rectTabFocused.height * .34f), rectTabFocused.width, rectTabFocused.height - (rectTabFocused.height * .34f));

					// Draw the tab button


					// Do tooltip
					if (Mouse.IsOver(KrozzyUtilities.RectAddition(rectTabfocusedButton, rectCard)))
					{
						KrozzyUtilities.Tooltip(Find.WindowStack.currentlyDrawnWindow.windowRect, astrTabTooltip[x]);
					}

					// Do the sheet
					itemSheet();
				}
			}
		}

		private void itemSheet()
		{

			Log.Message(GenDefDatabase.AllDefTypesWithDatabases().ToString());
		}











		private void DoGrid()
		{
			/*******************************************
			* Grid for reference while designing cards *
			*******************************************/
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .1f), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .2f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .2f), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .3f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .3f), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .4f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .4f), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .5f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .5f), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .6f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .6f), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .7f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .7f), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .8f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .8f), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .9f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .9f), Color.cyan, 2f);

			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .05f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .05f), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .15f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .15f), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .25f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .25f), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .35f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .35f), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .45f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .45f), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .55f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .55f), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .65f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .65f), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .75f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .75f), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .85f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .85f), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x, rectCard.y + rectCard.height * .95f), new Vector2(rectCard.x + rectCard.width, rectCard.y + rectCard.height * .95f), Color.blue, 1f);

			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .05f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .05f, rectCard.y + rectCard.height), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .15f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .15f, rectCard.y + rectCard.height), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .25f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .25f, rectCard.y + rectCard.height), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .35f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .35f, rectCard.y + rectCard.height), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .45f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .45f, rectCard.y + rectCard.height), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .55f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .55f, rectCard.y + rectCard.height), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .65f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .65f, rectCard.y + rectCard.height), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .75f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .75f, rectCard.y + rectCard.height), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .85f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .85f, rectCard.y + rectCard.height), Color.blue, 1f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .95f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .95f, rectCard.y + rectCard.height), Color.blue, 1f);

			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .1f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .1f, rectCard.y + rectCard.height), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .2f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .2f, rectCard.y + rectCard.height), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .3f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .3f, rectCard.y + rectCard.height), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .4f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .4f, rectCard.y + rectCard.height), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .5f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .5f, rectCard.y + rectCard.height), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .6f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .6f, rectCard.y + rectCard.height), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .7f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .7f, rectCard.y + rectCard.height), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .8f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .8f, rectCard.y + rectCard.height), Color.cyan, 2f);
			Widgets.DrawLine(new Vector2(rectCard.x + rectCard.width * .9f, rectCard.y + rectCard.height * .1f), new Vector2(rectCard.x + rectCard.width * .9f, rectCard.y + rectCard.height), Color.cyan, 2f);

		}

		public string GetLabel()
		{
			if (eCardType == ePNC_Card_Type.Pawn)
			{
				return pawn.Label;
			}
			else if (eCardType == ePNC_Card_Type.Items)
			{
				return "Items";
			}

			return "GetLabel error.";
		}


	}
}
