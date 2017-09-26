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
	class ProDialog_EditBackground : Window
	{
		private Pawn pawn;

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
		}


		public override void DoWindowContents(Rect inRect)
		{
		}
	}
}
