using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using Prophecy.Meta;
using Prophecy.Stock;

namespace Prophecy.PreGame
{
	class ProDialog_EditPassions : Window
	{
		private const float TopAreaHeight = 40f;
		private const float TopButtonHeight = 35f;
		private const float TopButtonWidth = 150f;
		private Vector2 vecInitialSize = new Vector2(0f, 0f);
		//private new Vector2 CloseButSize = new Vector2(120f, 40f);
		private float floAdjustButtonSize = 0f;
		private float floSkillsColumnRight = 0f;
		private Pawn pawn;
		string strName;
		SkillRecord[] aSKs;

		public override Vector2 InitialSize
		{
			get
			{
				return vecInitialSize;
			}
		}

		public ProDialog_EditPassions(Pawn _pawn)
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
			floAdjustButtonSize = Text.CalcSize("00").y;
			strName = pawn.Name.ToStringFull;
			aSKs = pawn.skills.skills.ToArray();

			foreach (SkillRecord sk in aSKs)
			{
				vecInitialSize.y += Text.CalcSize(sk.def.skillLabel).y;

				// Calculate window size
				if (Text.CalcSize(sk.def.skillLabel).x + (StandardMargin * 2f) > vecInitialSize.x)
				{
					vecInitialSize.x = Text.CalcSize(sk.def.skillLabel).x + (StandardMargin * 2f);

				}

				// Calculate floSkillsColumnRight
				if (Text.CalcSize(sk.def.skillLabel).x + floAdjustButtonSize > floSkillsColumnRight)
				{
					floSkillsColumnRight = Text.CalcSize(sk.def.skillLabel).x + floAdjustButtonSize;

				}
			}

			// Calculate window size more...spacing
			vecInitialSize.x += (floAdjustButtonSize * 6f);

			Text.Font = GameFont.Medium;
			vecInitialSize.y += Text.CalcSize(strName).y;

			// Calculate window size more... If name is too long
			if (Text.CalcSize(strName).x + (StandardMargin * 2f) > vecInitialSize.x)
			{
				vecInitialSize.x = Text.CalcSize(strName).x + (StandardMargin * 2f);

			}
			Text.Font = GameFont.Small;

			vecInitialSize.y += CloseButSize.y + (StandardMargin * 2f) + (floAdjustButtonSize * 2f);

			windowRect = new Rect(((float)UI.screenWidth - InitialSize.x) / 2f, ((float)UI.screenHeight - InitialSize.y) / 2f, InitialSize.x, InitialSize.y);
		}

		public override void PreClose()
		{
			base.PreClose();
		}

		public override void DoWindowContents(Rect inRect)
		{
			float x = 0f;
			float y = 0f;
			float w = 0f;
			float h = 0f;

			Text.Font = GameFont.Medium;
			w = Text.CalcSize(strName).x;
			h = Text.CalcSize(strName).y;
			Widgets.Label(new Rect(x, y, w, h), strName);
			y += h;
			Text.Font = GameFont.Small;

			string strPassionMinor = string.Concat(" = ", NewGameRules.GetPassionCost(pawn, Passion.Minor).ToString(), " Points");
			string strPassionMajor = string.Concat(" = ", NewGameRules.GetPassionCost(pawn, Passion.Major).ToString(), " Points");
			Vector2 vecPassionMinor = Text.CalcSize(strPassionMinor);
			Vector2 vecPassionMajor = Text.CalcSize(strPassionMajor);

			GUI.DrawTexture(new Rect(x, y, vecPassionMinor.y, vecPassionMinor.y), ProTBin.texPassionMinorIcon);
			Widgets.Label(new Rect(vecPassionMinor.y, y, vecPassionMinor.x, vecPassionMinor.y), strPassionMinor);

			y += vecPassionMinor.y;

			GUI.DrawTexture(new Rect(x, y, vecPassionMajor.y, vecPassionMajor.y), ProTBin.texPassionMajorIcon);
			Widgets.Label(new Rect(vecPassionMajor.y, y, vecPassionMajor.x, vecPassionMajor.y), strPassionMajor);

			y += vecPassionMajor.y;

			foreach (SkillRecord sk in aSKs)
			{
				w = Text.CalcSize(sk.def.skillLabel).x;
				h = Text.CalcSize(sk.def.skillLabel).y;
				//string strPointsLabel = "Points: ";
				//string strPoints = String.Format("{0:0}", NewGameRules.GetPassionCost(pawn, sk.passion));

				Rect rectSkillLabel = new Rect(x, y, w, h);
				Rect rectDownButton = new Rect(floSkillsColumnRight, y, floAdjustButtonSize, floAdjustButtonSize);
				Rect rectSkillLevel = new Rect(rectDownButton.x + rectDownButton.width + floAdjustButtonSize, y, floAdjustButtonSize, floAdjustButtonSize);
				Rect rectUpButton = new Rect(rectSkillLevel.x + rectSkillLevel.width + floAdjustButtonSize, y, floAdjustButtonSize, floAdjustButtonSize);
				//Rect rectPointsLabel = new Rect(rectUpButton.x + rectUpButton.width + floAdjustButtonSize, y, Text.CalcSize(strPointsLabel).x, floAdjustButtonSize);
				//Rect rectPoints = new Rect(rectPointsLabel.x + rectPointsLabel.width, y, Text.CalcSize("0000").x, floAdjustButtonSize);

				GUI.Label(rectSkillLabel, sk.def.skillLabel, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.White, FontStyle.Normal, TextAnchor.MiddleLeft));

				if (sk.TotallyDisabled != true)
				{
					if (GUI.Button(rectDownButton, "-", "button"))
					{
						if (sk.passion > Passion.None && sk.TotallyDisabled != true)
						{
							sk.passion--;
						}
					}

					if (sk.passion == Passion.Minor)
					{
						GUI.DrawTexture(rectSkillLevel, ProTBin.texPassionMinorIcon);
					}
					else if (sk.passion == Passion.Major)
					{
						GUI.DrawTexture(rectSkillLevel, ProTBin.texPassionMajorIcon);
					}

					if (GUI.Button(rectUpButton, "+", "button"))
					{
						if (sk.passion < Passion.Major && sk.TotallyDisabled != true)
						{
							sk.passion++;
						}
					}

					//GUI.Label(rectPointsLabel, strPointsLabel, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.White, FontStyle.Normal, TextAnchor.MiddleLeft));
					//GUI.Label(rectPoints, strPoints, KrozzyUtilities.BuildStyle(Fonts.Arial_small, Colors.White, FontStyle.Normal, TextAnchor.MiddleRight));
				}

				y += h;
			}
		}
	}
}
