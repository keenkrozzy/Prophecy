using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using Prophecy.Meta;
using Prophecy.Stock;
using Prophecy.ProGame.Elements;

namespace Prophecy.PreGame
{
	class ProDialog_EditBackground : Window
	{
		private const float TopAreaHeight = 40f;
		private const float TopButtonHeight = 35f;
		private const float TopButtonWidth = 150f;
		private float floSpacing = Text.CalcSize("00000").y;
		private ProBackstories PBS = new ProBackstories();
		private Vector2 vecInitialSize = new Vector2(0f, 0f);
		private Pawn pawn;
		private string strName;
		private string strChildhoodLabel;
		private string strAdulthoodLabel;
		private Vector2 PawnPortraitSize = new Vector2(100f, 140f);
		private Rect rectNameLabel = new Rect();
		private Rect rectPortrait = new Rect();
		private Rect rectChildStoriesOut = new Rect();
		private Rect rectAdultStoriesOut = new Rect();
		private Rect rectChildStoriesIn = new Rect();
		private Rect rectAdultStoriesIn = new Rect();
		private Rect rectStoryViewerOut = new Rect();
		private Rect rectStoryViewerIn = new Rect();
		private Rect rectChildEquip = new Rect();
		private Rect rectAdultEquip = new Rect();
		private Rect rectChildLabel = new Rect();
		private Rect rectAdultLabel = new Rect();
		private Rect rectEquippedChildLabel = new Rect();
		private Rect rectEquippedAdultLabel = new Rect();
		private Backstory tempChildStory = new Backstory();
		private Backstory tempAdultStory = new Backstory();
		private Backstory tempStoryViewerBS = new Backstory();
		private Vector2 vecChildScrollPosition = Vector2.zero;
		private Vector2 vecAdultScrollPosition = Vector2.zero;
		private Vector2 vecViewerScrollPosition = Vector2.zero;
		private int intCurSelectedChildStory = 0;
		private int intCurSelectedAdultStory = 0;

		public override Vector2 InitialSize
		{
			get
			{
				return vecInitialSize;
			}
		}

		public ProDialog_EditBackground(Pawn _pawn)
		{
			pawn = _pawn;
			forcePause = true;
			doCloseX = false;
			closeOnEscapeKey = true;
			doCloseButton = true;
			closeOnClickedOutside = true;
			absorbInputAroundWindow = true;
		}

		protected override void SetInitialSizeAndPosition()
		{
			strName = pawn.Name.ToStringFull;
			strChildhoodLabel = "Childhood";
			strAdulthoodLabel = "Adulthood";

			Text.Font = GameFont.Medium;
			rectNameLabel.width = Text.CalcSize(strName).x;
			rectNameLabel.height = Text.CalcSize(strName).y;
			Text.Font = GameFont.Small;

			for (int i = 1; i < PBS.intChildStoriesAmount; i++)
			{
				PBS.GetNeoChildStory(ref tempChildStory, i);
				if (Text.CalcSize(tempChildStory.Title).x >= rectChildStoriesOut.width + 16f)
				{
					rectChildStoriesOut.width = Text.CalcSize(tempChildStory.Title).x + 16f;
				}
				rectChildStoriesIn.height += Text.CalcSize("00000").y;
			}

			for (int i = 1; i < PBS.intAdultStoriesAmount; i++)
			{
				PBS.GetNeoAdultStory(ref tempAdultStory, i);
				if (Text.CalcSize(tempAdultStory.Title).x >= rectAdultStoriesOut.width + 16f)
				{
					rectAdultStoriesOut.width = Text.CalcSize(tempAdultStory.Title).x + 16f;
				}
				rectAdultStoriesIn.height += Text.CalcSize("00000").y;
			}

			if (rectChildStoriesOut.width > rectAdultStoriesOut.width)
			{
				rectAdultStoriesOut.width = rectChildStoriesOut.width;
			}
			else if (rectAdultStoriesOut.width > rectChildStoriesOut.width)
			{
				rectChildStoriesOut.width = rectAdultStoriesOut.width;
			}

			rectChildStoriesIn.width = rectChildStoriesOut.width - 16f;
			rectAdultStoriesIn.width = rectAdultStoriesOut.width - 16f;
			rectChildStoriesOut.height = Text.CalcSize("00000").y * 5f;
			rectAdultStoriesOut.height = Text.CalcSize("00000").y * 5f;
			rectChildEquip.width = rectChildStoriesOut.width;
			rectAdultEquip.width = rectAdultStoriesOut.width;
			rectChildEquip.height = Text.CalcHeight("Equip Childhood", rectChildEquip.width);
			rectAdultEquip.height = Text.CalcHeight("Equip Adulthood", rectAdultEquip.width);
			rectStoryViewerOut.width = rectChildStoriesOut.width * 1.5f;
			rectStoryViewerOut.height = rectChildStoriesOut.height + rectAdultStoriesOut.height + rectChildEquip.height + rectAdultEquip.height + floSpacing;
			rectStoryViewerIn.width = rectStoryViewerOut.width - 16f;
			rectAdultLabel.size = Text.CalcSize(strAdulthoodLabel);
			rectChildLabel.size = Text.CalcSize(strChildhoodLabel);
			rectEquippedChildLabel.width = rectChildStoriesIn.width;
			rectEquippedAdultLabel.width = rectAdultStoriesIn.width;
			rectEquippedChildLabel.height = Text.CalcSize("00000").y;
			rectEquippedAdultLabel.height = Text.CalcSize("00000").y;
			rectPortrait.size = PawnPortraitSize;

			float floRightWidth = 0f;
			if (rectNameLabel.width > rectEquippedChildLabel.width)
			{
				floRightWidth = rectNameLabel.width;
			}
			else if (rectEquippedChildLabel.width > rectNameLabel.width)
			{
				floRightWidth = rectEquippedChildLabel.width;
			}

			vecInitialSize.x += (StandardMargin * 2f) + rectChildStoriesOut.width + rectStoryViewerOut.width + floRightWidth + (floSpacing * 2f);
			vecInitialSize.y += CloseButSize.y + (StandardMargin * 2f) + rectChildStoriesOut.height + rectAdultStoriesOut.height + rectChildEquip.height + rectAdultEquip.height + (floSpacing * 2f);

			windowRect = new Rect(((float)UI.screenWidth - InitialSize.x) / 2f, ((float)UI.screenHeight - InitialSize.y) / 2f, InitialSize.x, InitialSize.y);

			rectChildEquip.y = rectChildStoriesOut.height;
			rectAdultStoriesOut.y = rectChildEquip.y + rectChildEquip.height + floSpacing;
			rectAdultEquip.y = rectAdultStoriesOut.y + rectAdultStoriesOut.height;
			rectStoryViewerOut.x = rectChildStoriesOut.x + rectChildStoriesOut.width + floSpacing;
			rectNameLabel.x = rectStoryViewerOut.x + rectStoryViewerOut.width + floSpacing;
			rectPortrait.x = rectNameLabel.x + (rectNameLabel.width * .5f) - (rectPortrait.width * .5f);
			rectPortrait.y = rectNameLabel.y + rectNameLabel.height;
			rectChildLabel.x = rectNameLabel.x;
			rectChildLabel.y = rectPortrait.y + rectPortrait.height;
			rectEquippedChildLabel.x = rectNameLabel.x;
			rectEquippedChildLabel.y = rectChildLabel.y + rectChildLabel.height;
			rectAdultLabel.x = rectNameLabel.x;
			rectAdultLabel.y = rectEquippedChildLabel.y + rectEquippedChildLabel.height + floSpacing;
			rectEquippedAdultLabel.x = rectNameLabel.x;
			rectEquippedAdultLabel.y = rectAdultLabel.y + rectAdultLabel.height;
		}

		public override void DoWindowContents(Rect inRect)
		{

			// Draw the childhood stories
			Widgets.BeginScrollView(rectChildStoriesOut, ref vecChildScrollPosition, rectChildStoriesIn);
			GUI.BeginGroup(rectChildStoriesIn);
			ChildStoriesIn();
			GUI.EndGroup();
			Widgets.EndScrollView();

			// Draw the childhood equip button
			if (Widgets.ButtonText(rectChildEquip, "Apply Childhood", true, true) && intCurSelectedChildStory != 0)
			{
				pawn.story.childhood = tempStoryViewerBS;
			}

			// Draw the adulthood stories
			Widgets.BeginScrollView(rectAdultStoriesOut, ref vecAdultScrollPosition, rectAdultStoriesIn);
			GUI.BeginGroup(rectAdultStoriesIn);
			AdultStoriesIn();
			GUI.EndGroup();
			Widgets.EndScrollView();

			// Draw the adulthood equip button
			if (Widgets.ButtonText(rectAdultEquip, "Apply Adulthood", true, true) && intCurSelectedAdultStory != 0 && pawn.ageTracker.AgeBiologicalYearsFloat >= 20f)
			{
				pawn.story.adulthood = tempStoryViewerBS;
			}

			// Draw the story viewer
			Widgets.BeginScrollView(rectStoryViewerOut, ref vecViewerScrollPosition, rectStoryViewerIn);
			GUI.BeginGroup(rectStoryViewerIn);
			StoryViewerIn();
			GUI.EndGroup();
			Widgets.EndScrollView();

			// Draw pawn name
			Text.Font = GameFont.Medium;
			Widgets.Label(rectNameLabel, strName);
			Text.Font = GameFont.Small;

			// Draw the portrait
			Vector3 vector3 = new Vector3();
			GUI.DrawTexture(rectPortrait, PortraitsCache.Get(pawn, PawnPortraitSize, vector3, 1f));

			// Draw the equipped childhood label
			Widgets.Label(rectChildLabel, strChildhoodLabel);

			// Draw the equipped childhood
			Widgets.Label(rectEquippedChildLabel, pawn.story.childhood.Title);

			if (pawn.story.adulthood != null)
			{
				// Draw the equipped adulthood label
				Widgets.Label(rectAdultLabel, strAdulthoodLabel);

				// Draw the equipped adulthood
				Widgets.Label(rectEquippedAdultLabel, pawn.story.adulthood.Title);
			}
		}

		private void ChildStoriesIn()
		{
			string strStoryLabel = "";
			Rect rectStoryLabel = new Rect(0, 0, rectChildStoriesIn.width, Text.CalcSize("00000").y);
			float floYAdjust = rectStoryLabel.height;

			GUI.DrawTexture(rectChildStoriesIn, ProTBin.texVellum, ScaleMode.ScaleAndCrop);

			for (int i = 1; i < PBS.intChildStoriesAmount; i++)
			{
				PBS.GetNeoChildStory(ref tempChildStory, i);
				strStoryLabel = tempChildStory.Title;

				// Draw label based upon if it's currently selected
				if (i == intCurSelectedChildStory)
				{
					GUI.Label(rectStoryLabel, strStoryLabel, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.Yellow));

					// Prime variable for equip button
					//esItemCurSelectedToEquip = _aItems[i];
				}
				else
				{
					Widgets.Label(rectStoryLabel, strStoryLabel);
				}

				// Highlight is mouse is hovering over
				if (Mouse.IsOver(rectStoryLabel))
				{
					Widgets.DrawHighlight(rectStoryLabel);
				}

				// Draw invisible button for item selecting
				if (Widgets.ButtonInvisible(rectStoryLabel, true))
				{
					intCurSelectedAdultStory = 0;
					intCurSelectedChildStory = i;
				}

				rectStoryLabel.y += floYAdjust;
			}
		}

		private void AdultStoriesIn()
		{
			string strStoryLabel = "";
			Rect rectStoryLabel = new Rect(0, 0, rectAdultStoriesIn.width, Text.CalcSize("00000").y);
			float floYAdjust = rectStoryLabel.height;

			GUI.DrawTexture(rectAdultStoriesIn, ProTBin.texVellum, ScaleMode.ScaleAndCrop);

			for (int i = 1; i < PBS.intAdultStoriesAmount; i++)
			{
				PBS.GetNeoAdultStory(ref tempAdultStory, i);
				strStoryLabel = tempAdultStory.Title;

				// Draw label based upon if it's currently selected
				if (i == intCurSelectedAdultStory)
				{
					GUI.Label(rectStoryLabel, strStoryLabel, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.Yellow));

					// Prime variable for equip button
					//esItemCurSelectedToEquip = _aItems[i];
				}
				else
				{
					Widgets.Label(rectStoryLabel, strStoryLabel);
				}

				// Highlight is mouse is hovering over
				if (Mouse.IsOver(rectStoryLabel))
				{
					Widgets.DrawHighlight(rectStoryLabel);
				}

				// Draw invisible button for item selecting
				if (Widgets.ButtonInvisible(rectStoryLabel, true))
				{
					intCurSelectedChildStory = 0;
					intCurSelectedAdultStory = i;
				}

				rectStoryLabel.y += floYAdjust;
			}
		}

		private void StoryViewerIn()
		{
			string strLabel = "";
			string strStoryLabel = "";
			string strStoryDesc = "";
			Vector2 v2Label = new Vector2();
			Vector2 v2StoryLabel = new Vector2();
			Vector2 v2StoryDesc = new Vector2();
			if (intCurSelectedChildStory != 0)
			{
				PBS.GetNeoChildStory(ref tempStoryViewerBS, intCurSelectedChildStory);
				strLabel = "Childhood".Translate() + ": ";
				strStoryLabel = tempStoryViewerBS.Title;
				strStoryDesc = tempStoryViewerBS.FullDescriptionFor(pawn);
				v2Label = Text.CalcSize(strLabel);
				v2StoryLabel = Text.CalcSize(strStoryLabel);
				v2StoryDesc.x = rectStoryViewerIn.width;
				v2StoryDesc.y = Text.CalcHeight(strStoryDesc, rectStoryViewerIn.width);
				rectStoryViewerIn.height = v2StoryLabel.y + v2StoryDesc.y;

				GUI.DrawTexture(rectStoryViewerIn, ProTBin.texVellum, ScaleMode.ScaleAndCrop);

				Widgets.Label(new Rect(0, 0, v2Label.x, v2Label.y), strLabel);
				Widgets.Label(new Rect(v2Label.x, 0, v2StoryLabel.x, v2StoryLabel.y), strStoryLabel);
				Widgets.Label(new Rect(0, v2Label.y, v2StoryDesc.x, v2StoryDesc.y), strStoryDesc);
			}
			else if (intCurSelectedAdultStory != 0)
			{
				PBS.GetNeoAdultStory(ref tempStoryViewerBS, intCurSelectedAdultStory);
				strLabel = "Adulthood".Translate() + ": ";
				strStoryLabel = tempStoryViewerBS.Title;
				strStoryDesc = tempStoryViewerBS.FullDescriptionFor(pawn);
				v2Label = Text.CalcSize(strLabel);
				v2StoryLabel = Text.CalcSize(strStoryLabel);
				v2StoryDesc.x = rectStoryViewerIn.width;
				v2StoryDesc.y = Text.CalcHeight(strStoryDesc, rectStoryViewerIn.width);
				rectStoryViewerIn.height = v2StoryLabel.y + v2StoryDesc.y;

				GUI.DrawTexture(rectStoryViewerIn, ProTBin.texVellum, ScaleMode.ScaleAndCrop);

				Widgets.Label(new Rect(0, 0, v2Label.x, v2Label.y), strLabel);
				Widgets.Label(new Rect(v2Label.x, 0, v2StoryLabel.x, v2StoryLabel.y), strStoryLabel);
				Widgets.Label(new Rect(0, v2Label.y, v2StoryDesc.x, v2StoryDesc.y), strStoryDesc);
			}
		}
	}
}
