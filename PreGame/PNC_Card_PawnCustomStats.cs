using Prophesy.Meta;
using Prophesy.Stock;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace Prophesy.PreGame
{
	public class PNC_Card_PawnCustomStats
	{
		Rect rectCard = new Rect();
		Color cBackdrop = new Color(0f, 0f, 0f, .4f);
		float floPaddingX;
		float floPaddingY;
		float floNumOfButtons = 5;
		float floButtonWidth;
		float floButtonHeight;

		public PNC_Card_PawnCustomStats()
		{
			
		}

		public void DoCustomStats(Rect _rectCard, Pawn _pawn)
		{
			rectCard = _rectCard;
			floPaddingX = rectCard.width * .025f;
			floPaddingY = rectCard.height * .05f;
			Rect rectGroup = new Rect(floPaddingX, rectCard.height * .8f, rectCard.width - (floPaddingX * 2f), rectCard.height * .15f);
			rectGroup = KrozzyUtilities.RectAddition(rectGroup, rectCard);

			Widgets.DrawBoxSolid(rectGroup, cBackdrop);
			GUI.BeginGroup(rectGroup);

			floPaddingX = rectGroup.width * .025f;
			floPaddingY = rectGroup.height * .2f;
			floButtonWidth = (rectGroup.width - (floPaddingX * (float)(2 + (floNumOfButtons - 1)))) / floNumOfButtons;
			floButtonHeight = rectGroup.height - (floPaddingY * 2f);
			Rect rectAge = new Rect(floPaddingX, floPaddingY, floButtonWidth, floButtonHeight);
			Rect rectBackstory = new Rect(rectAge.x + rectAge.width + floPaddingX, floPaddingY, floButtonWidth, floButtonHeight);
			Rect rectInterests = new Rect(rectBackstory.x + rectBackstory.width + floPaddingX, floPaddingY, floButtonWidth, floButtonHeight);
			Rect rectSkills = new Rect(rectInterests.x + rectInterests.width + floPaddingX, floPaddingY, floButtonWidth, floButtonHeight);
			Rect rectTraits = new Rect(rectSkills.x + rectSkills.width + floPaddingX, floPaddingY, floButtonWidth, floButtonHeight);

			EditAge(rectAge, _pawn);
			EditBackstory(rectBackstory, _pawn);
			EditInterests(rectInterests, _pawn);
			EditSkills(rectSkills, _pawn);
			EditTraits(rectTraits, _pawn);

			GUI.EndGroup();
		}

		private void EditAge(Rect _rect, Pawn _pawn)
		{
			if (Widgets.ButtonText(_rect, "Edit Age", true, true))
			{
				Find.WindowStack.Add(new ProDialog_EditAge(_pawn));
			}
		}

		private void EditBackstory(Rect _rect, Pawn _pawn)
		{
			Widgets.ButtonText(_rect, "Edit Backstory", true, true);
		}

		private void EditInterests(Rect _rect, Pawn _pawn)
		{
			Widgets.ButtonText(_rect, "Edit Passions", true, true);
		}

		private void EditSkills(Rect _rect, Pawn _pawn)
		{
			if (Widgets.ButtonText(_rect, "Edit Skills", true, true))
			{
				Find.WindowStack.Add(new ProDialog_EditSkills(_pawn));
			}
		}

		private void EditTraits(Rect _rect, Pawn _pawn)
		{
			Widgets.ButtonText(_rect, "Edit Traits", true, true);
		}

		public float GetTotalCost(Pawn _pawn)
		{
			float floSkillsCost = 0f;

			foreach (SkillRecord sr in _pawn.skills.skills)
			{
				floSkillsCost += NewGameRules.GetSkillCost(_pawn.ageTracker.AgeBiologicalYears, sr.levelInt);
			}

			return floSkillsCost;
		}
		
	}
}
