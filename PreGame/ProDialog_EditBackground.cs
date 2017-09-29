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
		private Vector2 vecChildScrollPosition = Vector2.zero;
		private Vector2 vecAdultScrollPosition = Vector2.zero;
		private Vector2 vecViewerScrollPosition = Vector2.zero;

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
				if (Text.CalcSize(tempChildStory.Title).x >= rectChildStoriesOut.width)
				{
					rectChildStoriesOut.width = Text.CalcSize(tempChildStory.Title).x;
				}
				rectChildStoriesIn.height += Text.CalcSize("00000").y;
			}

			for (int i = 1; i < PBS.intAdultStoriesAmount; i++)
			{
				PBS.GetNeoAdultStory(ref tempAdultStory, i);
				if (Text.CalcSize(tempAdultStory.Title).x >= rectAdultStoriesOut.width)
				{
					rectAdultStoriesOut.width = Text.CalcSize(tempAdultStory.Title).x;
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
			rectAdultLabel.size = Text.CalcSize(strChildhoodLabel);
			rectChildLabel.size = Text.CalcSize(strAdulthoodLabel);
			rectEquippedChildLabel.width = rectChildStoriesIn.width;
			rectEquippedAdultLabel.width = rectAdultStoriesIn.width;
			rectEquippedChildLabel.height = Text.CalcSize("00000").y;
			rectEquippedAdultLabel.height = Text.CalcSize("00000").y;


			rectPortrait.size = PawnPortraitSize;

			vecInitialSize.x += (StandardMargin * 2f) + rectChildStoriesOut.width + rectStoryViewerOut.width + rectNameLabel.width + (floSpacing * 2f);
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
			rectAdultLabel.y = rectEquippedChildLabel.y + rectEquippedChildLabel.height;
			rectEquippedAdultLabel.x = rectNameLabel.x;
			rectEquippedAdultLabel.y = rectAdultLabel.y + rectAdultLabel.height;
		}


		public override void DoWindowContents(Rect inRect)
		{

			// Draw the childhood stories
			Widgets.BeginScrollView(rectChildStoriesOut, ref vecChildScrollPosition, rectChildStoriesIn);

			Widgets.EndScrollView();

			// Draw the childhood equip button
			Widgets.ButtonText(rectChildEquip, "Equip Childhood", true, true);

			// Draw the adulthood stories
			Widgets.BeginScrollView(rectAdultStoriesOut, ref vecAdultScrollPosition, rectAdultStoriesIn);

			Widgets.EndScrollView();

			// Draw the adulthood equip button
			Widgets.ButtonText(rectAdultEquip, "Equip Adulthood", true, true);

			// Draw the story viewer
			Widgets.BeginScrollView(rectStoryViewerOut, ref vecViewerScrollPosition, rectStoryViewerIn);

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

			// Draw the equipped adulthood label
			Widgets.Label(rectAdultLabel, strAdulthoodLabel);

			// Draw the equipped adulthood
			Widgets.Label(rectEquippedAdultLabel, pawn.story.adulthood.Title);
		}
	}
}
