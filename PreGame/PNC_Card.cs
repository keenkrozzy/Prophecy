using Prophecy.Meta;
using Prophecy.Stock;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace Prophecy.PreGame
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
		//private int intCurTab = 0;
		private Texture2D[] atexTabs = new Texture2D[6] { ProTBin.texFoodsTab, ProTBin.texApparelTab, ProTBin.texWeaponsTab, ProTBin.texDrugsTab, ProTBin.texResourcesTab, ProTBin.texItemsTab };
		private bool[] aboolCurTab = { true, false, false, false, false, false };
		private string[] astrTabTooltip = { "Foods", "Apparel", "Weapons", "Drugs", "Resources", "Items" };
		private Vector2 v2FoodsScrollPosition1 = Vector2.zero;
		private Vector2 v2FoodsScrollPosition2 = Vector2.zero;
		private int intCurSelectedToEquip = 0;
        private int intCurSelectedToUnequip = 0;
        private ESItem esItemCurSelectedToEquip = null;
        //private int intCurSelectedAmountToEquip = 0;
        //private float floCurSelectedPriceToEquip = 0f;
        private ESItem esItemCurSelectedToUnequip = null;
        //private int intCurSelectedAmountToUnequip = 0;
        //private int intCurSelectedPriceToUnequip = 0;
        private ProEquipmentShop ES;
		private PNC_Card_PawnCustomStats PCS;


		public PNC_Card(Pawn _pawn = null, int _intNumCards = 0)
		{
			if (_pawn != null)
			{
				pawn = _pawn;
				eCardType = ePNC_Card_Type.Pawn;
				PCS = new PNC_Card_PawnCustomStats();
				NewGameRules.AddPawnToCurPoints(pawn);
			}
			else
			{
				eCardType = ePNC_Card_Type.Items;
				ES = new ProEquipmentShop();
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

			if (eCardType == ePNC_Card_Type.Pawn)
			{
				DrawLabel(_rect);
				DrawPawnPortrait();
				DoNameInput();
				DoMainDesc();
				DoBackstories();
				DoSkills();
				DoTraits();
				PCS.DoCustomStats(rectCard, pawn);
			}

			if (eCardType == ePNC_Card_Type.Items)
			{
				DrawLabel(_rect);
				DoTabs();
                DoStartingItems();
                DoEquipButton();
                DoUnequipButton();
            }

			DoTotalCostLabel();

		}

        /***********************************************************************************
		* Method to only draw the top of the card if it is not the first card in the stack *
		***********************************************************************************/
        public void DrawCardTopOnly(Rect _rect)
		{
			Widgets.DrawMenuSection(_rect, true);
			rectCard = _rect;
			DrawLabel(_rect);
			DoTotalCostLabel();
            

        }

		private void DrawLabel(Rect _rect)
		{
			Rect rectHead = new Rect(_rect.x + (_rect.width * .025f), _rect.y, 0f, 0f);
			Rect rectLabel = new Rect(0f, _rect.y, 0f, 0f);
			Text.Font = GameFont.Medium;

			if (eCardType == ePNC_Card_Type.Pawn)
			{
				Vector2 vect2 = new Vector2();
				vect2 = Text.CalcSize(pawn.Label);

				rectLabel.width = vect2.x;
				rectLabel.height = vect2.y;
				rectLabel.x = rectHead.x + vect2.y;

				float floHeadAdjust = vect2.y * .1f;
				rectHead.y += floHeadAdjust;
				rectHead.width = vect2.y - floHeadAdjust;
				rectHead.height = vect2.y - floHeadAdjust;

				Widgets.Label(rectLabel, pawn.Label);
				GUI.DrawTexture(rectHead, PortraitsCache.Get(pawn, rectHead.size, new Vector3(0f, 0f, .3f), 4f), ScaleMode.ScaleToFit);
			}
			else if (eCardType == ePNC_Card_Type.Items)
			{
				Vector2 vect2 = new Vector2();
				vect2 = Text.CalcSize("Items");

				rectLabel.width = vect2.x;
				rectLabel.height = vect2.y;
				rectLabel.x = rectHead.x + vect2.y;

				Widgets.Label(rectLabel, "Items");
			}

			Text.Font = GameFont.Small;
		}

        /***************************************
		* Method to handle the pawn's portrait *
		***************************************/
        private void DrawPawnPortrait()
		{
            // Shape the portrait
			Rect rect = new Rect(rectCard.x + (rectCard.width * .85f), rectCard.y + (rectCard.height * .05f), PawnPortraitSize.x, PawnPortraitSize.y);
			Vector3 vector3 = new Vector3();

            // Draw the portrait
			GUI.DrawTexture(rect, PortraitsCache.Get(pawn, PawnPortraitSize, vector3, 1f));
		}

        /************************************************************
		* Draw Name input text boxes with text set to current name. *
		************************************************************/
        private void DoNameInput()
		{
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
			Rect rect = new Rect(rectCard.width * .025f, rectCard.height * .05f, Text.CalcSize(strMainDesc).x, Text.CalcSize(strMainDesc).y);
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
                    if (Widgets.ButtonInvisible(KrozzyUtilities.RectAddition(rectTabUnfocusedButton, rectCard), true))
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            aboolCurTab[i] = false;
                        }
                        aboolCurTab[x] = true;
                    }

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

					// Do tooltip
					if (Mouse.IsOver(KrozzyUtilities.RectAddition(rectTabfocusedButton, rectCard)))
					{
						KrozzyUtilities.Tooltip(Find.WindowStack.currentlyDrawnWindow.windowRect, astrTabTooltip[x]);
					}

					// Do the sheet
					switch (astrTabTooltip[x])
					{
						case "Foods":
						ItemSheet(rectMenuSection, ES.Foods.aFoods, GameFont.Small);
						break;

                        case "Apparel":
                        ItemSheet(rectMenuSection, ES.Apparel.aApparel, GameFont.Small);
                        break;

                        case "Weapons":
                        ItemSheet(rectMenuSection, ES.Weapons.aWeapons, GameFont.Small);
                        break;

                        case "Drugs":
                        ItemSheet(rectMenuSection, ES.Drugs.aDrugs, GameFont.Small);
                        break;

                        case "Resources":
                        ItemSheet(rectMenuSection, ES.Resources.aResources, GameFont.Small);
                        break;

                        case "Items":
                        ItemSheet(rectMenuSection, ES.Items.aItems, GameFont.Small);
                        break;
                    }
				}
			}
		}

		private void ItemSheet(Rect _rectOut, ESItem[] _aItems, GameFont _fontSize)
		{
			// Shape item sheet
			float floViewWidth = _rectOut.width - 16f; // Scrollbar has 16f hard number
			float floColumnTexWidth = floViewWidth * .1f;
			float floColumnNameWidth = floViewWidth * .6f;
            float floColumnQuantityWidth = floViewWidth * .12f;
            float floColumnCostWidth = floViewWidth * .18f;
            Text.Font = _fontSize;
			float TotalHeight = 0f;

			// Create strings for headers
			string strItemNameHeader = "Item";
			string strItemAmountHeader = "#";
			string strItemPriceHeader = "Cost";

			// Calculate header heights
			float floHeaderHeight = Text.CalcHeight(strItemNameHeader, floColumnNameWidth);

			// Shape the headers
			Rect rectItemNameHeader = new Rect(_rectOut.x + floColumnTexWidth, _rectOut.y, floColumnNameWidth, floHeaderHeight);
			Rect rectItemAmountHeader = new Rect(_rectOut.x + floColumnTexWidth + floColumnNameWidth, _rectOut.y, floColumnQuantityWidth, floHeaderHeight);
			Rect rectItemPriceHeader = new Rect(_rectOut.x + floColumnTexWidth + floColumnNameWidth + floColumnQuantityWidth, _rectOut.y, floColumnCostWidth, floHeaderHeight);

			// Draw the headers
			GUI.Label(KrozzyUtilities.RectAddition(rectItemNameHeader, rectCard), strItemNameHeader, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.White, FontStyle.Bold));
			GUI.Label(KrozzyUtilities.RectAddition(rectItemAmountHeader, rectCard), strItemAmountHeader, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.White, FontStyle.Bold));
			GUI.Label(KrozzyUtilities.RectAddition(rectItemPriceHeader, rectCard), strItemPriceHeader, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.White, FontStyle.Bold));

			// Draw the buttons
			if (Widgets.ButtonInvisible(KrozzyUtilities.RectAddition(rectItemNameHeader, rectCard)))
			{
				ES.SortItemsTool(ProEquipmentShop.ESSortList.Items, ProEquipmentShop.ESSortType.DefName);
			}
			if (Widgets.ButtonInvisible(KrozzyUtilities.RectAddition(rectItemAmountHeader, rectCard)))
			{
				ES.SortItemsTool(ProEquipmentShop.ESSortList.Items, ProEquipmentShop.ESSortType.ThingAmount);
			}
			if (Widgets.ButtonInvisible(KrozzyUtilities.RectAddition(rectItemPriceHeader, rectCard)))
			{
				ES.SortItemsTool(ProEquipmentShop.ESSortList.Items, ProEquipmentShop.ESSortType.Price);
			}

			// Reshape the _rectOut
			_rectOut = new Rect(_rectOut.x, _rectOut.y + floHeaderHeight, _rectOut.width, _rectOut.height - floHeaderHeight);

			// Calculate total item list height
			foreach (ESItem item in _aItems)
            {
                TotalHeight += Text.CalcHeight(item.strNameLabel, floColumnNameWidth);
            }

			// Calculate rectView based on if the scroll bar will need to be shown
            Rect rectView = new Rect();
            if (TotalHeight > _rectOut.height) {rectView = new Rect(_rectOut.x, _rectOut.y, floViewWidth, TotalHeight);}
            else {rectView = new Rect(_rectOut.x, _rectOut.y, _rectOut.width, TotalHeight);}
            
			// Calculate rects for GUI group and tooltip
			Rect rectGroup = KrozzyUtilities.RectAddition(rectView, rectCard);
			Rect rectScrollingTooltip = KrozzyUtilities.RectAddition(_rectOut, rectCard);
            
            // Begin scroll view and group
            Widgets.BeginScrollView(KrozzyUtilities.RectAddition(_rectOut, rectCard), ref v2FoodsScrollPosition1, KrozzyUtilities.RectAddition(rectView, rectCard));
			GUI.BeginGroup(rectGroup);

            // Create list of items from array
            float floYPos = 0f;
			for (int i = 0; i < _aItems.Length; i++)
			{
                string strNameLabel = _aItems[i].strNameLabel;
                string strQauntityLabel = _aItems[i].thingAmount.ToString();
                string strCostLabel = String.Format( "{0:0}", ES.GetPrice(_aItems[i]));
                string strDescToolTip = _aItems[i].thingDef.description;

                // Shape idividual item
                Rect rectWholeItem = new Rect(0f, floYPos, rectView.width, Text.CalcHeight(strNameLabel, floColumnNameWidth));
				Rect rectMiniTex = new Rect(0f, floYPos, floColumnTexWidth, Text.CalcHeight(strNameLabel, floColumnNameWidth));
				Rect rectNameLabel = new Rect(floColumnTexWidth, floYPos, floColumnNameWidth, Text.CalcHeight(strNameLabel, floColumnNameWidth));
                Rect rectQauntityLabel = new Rect(floColumnTexWidth + floColumnNameWidth, floYPos, floColumnQuantityWidth, Text.CalcHeight(strNameLabel, floColumnNameWidth));
                Rect rectCostLabel = new Rect(floColumnTexWidth + floColumnNameWidth + floColumnQuantityWidth, floYPos, floColumnCostWidth, Text.CalcHeight(strNameLabel, floColumnNameWidth));

				// update y position for next label
				floYPos += Text.CalcHeight(strNameLabel, floColumnNameWidth);

                // Draw label based upon if it's currently selected
                if (i == intCurSelectedToEquip)
				{
					GUI.Label(rectNameLabel, strNameLabel, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.Yellow));
                    GUI.Label(rectQauntityLabel, strQauntityLabel, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.Yellow));
                    GUI.Label(rectCostLabel, strCostLabel, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.Yellow));
					_aItems[i].DrawIcon(rectMiniTex);

					// Prime variable for equip button
					esItemCurSelectedToEquip = _aItems[i];
                }
				else
				{
					Widgets.Label(rectNameLabel, strNameLabel);
                    Widgets.Label(rectQauntityLabel, strQauntityLabel);
                    Widgets.Label(rectCostLabel, strCostLabel);
					_aItems[i].DrawIcon(rectMiniTex);
				}

                // Highlight is mouse is hovering over
				if (Mouse.IsOver(rectWholeItem))
				{
					KrozzyUtilities.Tooltip(KrozzyUtilities.RectAddition(rectScrollingTooltip, Find.WindowStack.currentlyDrawnWindow.windowRect), strDescToolTip);
                    Widgets.DrawHighlight(rectWholeItem);                    
				}

                // Draw invisible button for item selecting
                if (Widgets.ButtonInvisible(rectWholeItem, true))
                {
                    intCurSelectedToEquip = i;
                }
            }

			// End group and scroll view
			GUI.EndGroup();
			Widgets.EndScrollView();
            Text.Font = GameFont.Small; // Ensure font size is back to default
        }

        private void DoStartingItems()
        {
            // Calculate variables		
            float floMenuHeight = rectCard.height * .8f;
            ESItem[] esiStartingItems = ESStartingItems.aStartingItems;

            // Shape the menu section
            Rect rectMenuSection = new Rect(rectCard.width * .625f, rectCard.height * .1f, rectCard.width * .3f, floMenuHeight);
			Rect rectOut = KrozzyUtilities.RectAddition(rectMenuSection, rectCard);

			// Draw the menu section
			GUI.DrawTexture(rectOut, ProTBin.texVellum, ScaleMode.ScaleAndCrop);
            GUI.color = cintBorder.ToColor;
            Widgets.DrawBox(rectOut, 1);
            GUI.color = Color.white; // set GUI color back to default

            // Shape item sheet
            float floViewWidth = rectMenuSection.width - 16f; // Scrollbar has 16f hard number
			float floColumnTexWidth = floViewWidth * .1f;
			float floColumnNameWidth = floViewWidth * .6f;
            float floColumnQuantityWidth = floViewWidth * .12f;
            float floColumnCostWidth = floViewWidth * .18f;
            Text.Font = GameFont.Small; // Make sure the font is default for size calculation
            float TotalHeight = 0f;

			// Create strings for headers
			string strItemNameHeader = "Item";
			string strItemAmountHeader = "#";
			string strItemPriceHeader = "Cost";

			// Calculate header heights
			float floHeaderHeight = Text.CalcHeight(strItemNameHeader, floColumnNameWidth);

			// Shape the headers
			Rect rectItemNameHeader = new Rect(rectOut.x + floColumnTexWidth, rectOut.y, floColumnNameWidth, floHeaderHeight);
			Rect rectItemAmountHeader = new Rect(rectOut.x + floColumnTexWidth + floColumnNameWidth, rectOut.y, floColumnQuantityWidth, floHeaderHeight);
			Rect rectItemPriceHeader = new Rect(rectOut.x + floColumnTexWidth + floColumnNameWidth + floColumnQuantityWidth, rectOut.y, floColumnCostWidth, floHeaderHeight);

			// Draw the headers
			GUI.Label(rectItemNameHeader, strItemNameHeader, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.White, FontStyle.Bold));
			GUI.Label(rectItemAmountHeader, strItemAmountHeader, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.White, FontStyle.Bold));
			GUI.Label(rectItemPriceHeader, strItemPriceHeader, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.White, FontStyle.Bold));

			// Draw the buttons
			if (Widgets.ButtonInvisible(rectItemNameHeader))
			{
				ES.SortItemsTool(ProEquipmentShop.ESSortList.StartingItems, ProEquipmentShop.ESSortType.DefName);
			}
			if (Widgets.ButtonInvisible(rectItemAmountHeader))
			{
				ES.SortItemsTool(ProEquipmentShop.ESSortList.StartingItems, ProEquipmentShop.ESSortType.ThingAmount);
			}
			if (Widgets.ButtonInvisible(rectItemPriceHeader))
			{
				ES.SortItemsTool(ProEquipmentShop.ESSortList.StartingItems, ProEquipmentShop.ESSortType.Price);
			}

			// Reshape the rectOut
			rectOut = new Rect(rectOut.x, rectOut.y + floHeaderHeight, rectOut.width, rectOut.height - floHeaderHeight);

			// Calculate total height of items
			foreach (ESItem item in esiStartingItems)
            {
                TotalHeight += Text.CalcHeight(item.strNameLabel, floColumnNameWidth);
            }

            // If the total height of items in more than the menu section height, make scroll view fit inside the menu
            // Else make the scroll view wide enough so the highlighted item is not clipped
            Rect rectView = new Rect();
            if (TotalHeight > rectOut.height)
            {
                rectView = new Rect(rectOut.x, rectOut.y, floViewWidth, TotalHeight);
            }
            else
            {
                rectView = new Rect(rectOut.x, rectOut.y, rectOut.width, TotalHeight);
            }

            // Shape the group and shape the tooltip so that it does not scroll inside the scroll view
            Rect rectGroup = KrozzyUtilities.RectAddition(rectView, rectCard);
            Rect rectScrollingTooltip = KrozzyUtilities.RectAddition(rectOut, rectCard);


            // Begin scroll view and group
            Widgets.BeginScrollView(rectOut, ref v2FoodsScrollPosition2, KrozzyUtilities.RectAddition(rectView, rectCard));
            GUI.BeginGroup(rectGroup);

            // Create list of items from array
            float floYPos = 0f;
            for (int i = 0; i < esiStartingItems.Length; i++)
            {
                // Build strings
                string strNameLabel = esiStartingItems[i].strNameLabel;
                string strQauntityLabel = esiStartingItems[i].thingAmountTotal.ToString();
                string strCostLabel = String.Format("{0:0}", esiStartingItems[i].subtotal);
                string strDescToolTip = string.Concat(esiStartingItems[i].thingDef.description, " price: ", esiStartingItems[i].price.ToString(), 
                    ", itemAmount: ", esiStartingItems[i].itemAmount.ToString());

                // Shape idividual item
				Rect rectWholeItem = new Rect(0f, floYPos, rectView.width, Text.CalcHeight(strNameLabel, floColumnNameWidth));
				Rect rectMiniTex = new Rect(0f, floYPos, floColumnTexWidth, Text.CalcHeight(strNameLabel, floColumnNameWidth));
				Rect rectNameLabel = new Rect(floColumnTexWidth, floYPos, floColumnNameWidth, Text.CalcHeight(strNameLabel, floColumnNameWidth));
				Rect rectQauntityLabel = new Rect(floColumnTexWidth + floColumnNameWidth, floYPos, floColumnQuantityWidth, Text.CalcHeight(strNameLabel, floColumnNameWidth));
				Rect rectCostLabel = new Rect(floColumnTexWidth + floColumnNameWidth + floColumnQuantityWidth, floYPos, floColumnCostWidth, Text.CalcHeight(strNameLabel, floColumnNameWidth));

				// update y position for next label
				floYPos += Text.CalcHeight(strNameLabel, floColumnNameWidth);

                // Draw label based upon if it's currently selected
                if (i == intCurSelectedToUnequip)
                {
					// Using custom font if selected
					esiStartingItems[i].DrawIcon(rectMiniTex);
					GUI.Label(rectNameLabel, strNameLabel, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.Yellow));
                    GUI.Label(rectQauntityLabel, strQauntityLabel, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.Yellow));
                    GUI.Label(rectCostLabel, strCostLabel, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.Yellow));

                    // Prime variable for unequip button
                    esItemCurSelectedToUnequip = esiStartingItems[i];
                    //intCurSelectedAmountToUnequip = esiStartingItems[i].amount;
                    //intCurSelectedPriceToUnequip = ES.GetPrice(esiStartingItems[i]);
                }
                else
                {
					// Using default font if not selected
					esiStartingItems[i].DrawIcon(rectMiniTex);
					Widgets.Label(rectNameLabel, strNameLabel);
                    Widgets.Label(rectQauntityLabel, strQauntityLabel);
                    Widgets.Label(rectCostLabel, strCostLabel);
                }

                // Highlight is mouse is hovering over
                if (Mouse.IsOver(rectWholeItem))
                {
                    KrozzyUtilities.Tooltip(KrozzyUtilities.RectAddition(rectScrollingTooltip, Find.WindowStack.currentlyDrawnWindow.windowRect), strDescToolTip);
                    Widgets.DrawHighlight(rectWholeItem);
                }

                // Draw invisible button for item selecting
                if (Widgets.ButtonInvisible(rectWholeItem, true))
                {
                    intCurSelectedToUnequip = i;
                }
            }

            // End group and scroll view
            GUI.EndGroup();
            Widgets.EndScrollView();
            Text.Font = GameFont.Small; // Ensure font size is back to default
        }

        private void DoEquipButton()
        {
            // Shape the button
            Rect rectEqiupButton = new Rect(rectCard.width * .4f, rectCard.height * .1f, rectCard.width * .2f, rectCard.height * .1f);

            // Draw the button
            if (Widgets.ButtonText(KrozzyUtilities.RectAddition(rectEqiupButton, rectCard), "Equip", true, true, true))
            {
				// Method to add the item to the starting items list
				ES.EquipESItem(esItemCurSelectedToEquip);

				if(esItemCurSelectedToUnequip == null)
				{
					esItemCurSelectedToUnequip = ESStartingItems.aStartingItems[intCurSelectedToUnequip];
				}
            }
        }

        private void DoUnequipButton()
        {
            ESItem[] esiStartingItems = ESStartingItems.aStartingItems;

            // Shape the button
            Rect rectEqiupButton = new Rect(rectCard.width * .4f, rectCard.height * .3f, rectCard.width * .2f, rectCard.height * .1f);

            if (esiStartingItems.Any(x => x.strNameLabel == esItemCurSelectedToUnequip.strNameLabel))
            {
                // Draw the button
                if (Widgets.ButtonText(KrozzyUtilities.RectAddition(rectEqiupButton, rectCard), "Unequip", true, true, true))
                {
                    // Method to remove the item to the starting items list
                    ES.UnequipESItem(ref esItemCurSelectedToUnequip);
                }
            }
            else
            {
                // Draw the button
                Widgets.ButtonText(KrozzyUtilities.RectAddition(rectEqiupButton, rectCard), "Unequip", true, false, false);
            }
        }

        private void DoTotalCostLabel()
        {
			if (eCardType == ePNC_Card_Type.Pawn)
			{
				// Calculate variables
				Text.Font = GameFont.Medium;
				float floTotalPrice = PCS.GetTotalCost(pawn);
				string strTotalPrice = String.Concat("Cost: ", String.Format("{0:0}", floTotalPrice));
				Vector2 vecSize = Text.CalcSize(strTotalPrice);
				float floPosX = (rectCard.width - vecSize.x) - (rectCard.width * .05f);

				// Shape the label
				Rect rectTotalCostLabel = new Rect(floPosX, rectCard.height * 0f, vecSize.x, vecSize.y);

				// Draw the label
				Widgets.Label(KrozzyUtilities.RectAddition(rectTotalCostLabel, rectCard), strTotalPrice);

				Text.Font = GameFont.Small;

				NewGameRules.UpdateCurPawnPoints(pawn, floTotalPrice);
			}
			else if (eCardType == ePNC_Card_Type.Items)
			{
				// Calculate variables
				Text.Font = GameFont.Medium;
				string strTotalPrice = String.Concat("Cost: ", String.Format("{0:0}", ES.floTotalItemsPrice));
				Vector2 vecSize = Text.CalcSize(strTotalPrice);
				float floPosX = (rectCard.width - vecSize.x) - (rectCard.width * .05f);

				// Shape the label
				Rect rectTotalCostLabel = new Rect(floPosX, rectCard.height * 0f, vecSize.x, vecSize.y);

				// Draw the label
				Widgets.Label(KrozzyUtilities.RectAddition(rectTotalCostLabel, rectCard), strTotalPrice);

				Text.Font = GameFont.Small;

				NewGameRules.floCurItemPoints = NewGameRules.floStartingItemPoints - ES.floTotalItemsPrice;
			}
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
