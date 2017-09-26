using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;

namespace Prophecy.PreGame
{
	public class ProDialog_EditAge : Window
	{
		private const float TopAreaHeight = 40f;
		private const float TopButtonHeight = 35f;
		private const float TopButtonWidth = 150f;
		private Vector2 vecInitialSize = new Vector2(0f,0f);
		private float floAdjustButtonSize = 0f;
		private Pawn pawn;
		string strName;
		string strAge;
		string strLifeStage;
		string[] astr;

		public override Vector2 InitialSize
		{
			get
			{
				return vecInitialSize;
			}
		}

		public ProDialog_EditAge(Pawn _pawn)
		{
			pawn = _pawn;
			forcePause = true;
			doCloseX = false;
			closeOnEscapeKey = true;
			doCloseButton = true;
			closeOnClickedOutside = true;
			absorbInputAroundWindow = true;
			vecInitialSize.x = CloseButSize.x + (StandardMargin * 2f);
			vecInitialSize.y = CloseButSize.y + (StandardMargin * 2f);

			strName = pawn.Name.ToStringFull;
			strAge = string.Concat("Age: ", pawn.ageTracker.AgeNumberString);
			strLifeStage = pawn.ageTracker.CurLifeStage.label;
			astr = new string[] {strAge, strLifeStage };

			Text.Font = GameFont.Medium;
			if (Text.CalcSize(strName).x + (StandardMargin * 2f) > vecInitialSize.x)
			{
				vecInitialSize.x = Text.CalcSize(strName).x + (StandardMargin * 2f);
			}
			vecInitialSize.y += Text.CalcSize(strName).y;
			Text.Font = GameFont.Small;

			foreach (string s in astr)
			{
				vecInitialSize.y += Text.CalcSize(s).y;
				if (Text.CalcSize(s).x + (StandardMargin * 2f) > vecInitialSize.x)
				{
					vecInitialSize.x = Text.CalcSize(s).x + (StandardMargin * 2f);
				}
			}

			if (vecInitialSize.y >= vecInitialSize.x)
			{
				floAdjustButtonSize = vecInitialSize.x * .2f;
			}
			else
			{
				floAdjustButtonSize = vecInitialSize.y * .2f;
			}

			vecInitialSize.y += floAdjustButtonSize;
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

			if (Widgets.ButtonText(new Rect(x + ((vecInitialSize.x - (StandardMargin * 2f)) * .3f) - (floAdjustButtonSize * .5f), y, floAdjustButtonSize, floAdjustButtonSize), "-", true, true))
			{
				if (pawn.ageTracker.AgeBiologicalTicks - 3600000L < 3600000L * 16)
				{
					Messages.Message(string.Concat(pawn.Name.ToStringShort, " cannot be any younger."), MessageSound.RejectInput);
				}
				else
				{
					pawn.ageTracker.AgeBiologicalTicks -= 3600000L;
					astr[0] = string.Concat("Age: ", pawn.ageTracker.AgeNumberString);
					astr[1] = pawn.ageTracker.CurLifeStage.label;
				}
			}
			if (Widgets.ButtonText(new Rect(x + ((vecInitialSize.x - (StandardMargin * 2f)) * .7f) - (floAdjustButtonSize * .5f), y, floAdjustButtonSize, floAdjustButtonSize), "+", true, true))
			{
				pawn.ageTracker.AgeBiologicalTicks += 3600000L;
				astr[0] = string.Concat("Age: ", pawn.ageTracker.AgeNumberString);
				astr[1] = pawn.ageTracker.CurLifeStage.label;
			}

			y += floAdjustButtonSize;

			foreach (string s in astr)
			{
				w = Text.CalcSize(s).x;
				h = Text.CalcSize(s).y;
				Widgets.Label(new Rect(x, y, w, h), s);
				y += h;
			}
		}
	}
}
