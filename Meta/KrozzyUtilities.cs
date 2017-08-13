using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Prophesy.Meta
{
	public static class KrozzyUtilities
	{
		public static Rect RectAddition(Rect _rect1, Rect _rect2, bool _bAddPosistion = true, bool _bAddSize = false)
		{
			Rect rect = _rect1;

			if (_bAddPosistion == true)
			{
				rect.x += _rect2.x;
				rect.y += _rect2.y;
			}
			if (_bAddSize == true)
			{
				rect.width +=  _rect2.width;
				rect.height += _rect2.height;
			}

			return rect;

			// Spdskatr - Today at 10:56 PM ...
			// Spdskatr - Today at 10:56 PM That doesnt make sense
		}

		/// <summary>
		/// Tool tip to lower-right of mouse cursor.
		/// </summary>
		/// <param name="_rectCurrentWindow">Current window being drawn in. Most likely Find.WindowStack.currentlyDrawnWindow.windowRect</param>
		/// <param name="_strTooltip">Text for the tool tip.</param>
		public static void Tooltip(Rect _rectCurrentWindow, string _strTooltip)
		{
			// Prepare variables for tooltip
			Rect rectWindow = _rectCurrentWindow;
			float floWindowMargin = 18f; // Margin put in by base class Window with optionalTitle = null
			float floCursorOffsetX = 22f;
			float floCursorOffsetY = 24f;
			Vector2 vectMouse = Event.current.mousePosition;
			Vector2 vectTooltipSize = Text.CalcSize(_strTooltip);
			string strTemp = _strTooltip;

			// Shape the tooltip
			vectMouse.x += floWindowMargin + floCursorOffsetX;
			vectMouse.y += floWindowMargin + floCursorOffsetY;
			Rect rectTooltipContainer = new Rect(vectMouse, vectTooltipSize);
			Rect rectTooltip = new Rect(0f, 0f, vectTooltipSize.x, vectTooltipSize.y);

			// Draw the tooltip
			Find.WindowStack.ImmediateWindow(140 * _strTooltip.GetHashCode() + 85708, KrozzyUtilities.RectAddition(rectTooltipContainer, rectWindow), WindowLayer.Super, delegate
			{
				Widgets.DrawBoxSolid(rectTooltip, Color.black);
				try
				{
					Widgets.Label(rectTooltip, strTemp);
				}
				catch (Exception e)
				{
					Log.Message(e.ToString());
				}
			}, false, false, 1f);
		}

	}
}
