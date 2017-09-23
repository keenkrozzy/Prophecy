using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using RimWorld;
using System.IO;


namespace Prophesy.Meta
{
	[StaticConstructorOnStartup]
	public static class KrozzyUtilities
	{
		private static Font[] fontStyles = new Font[3];
		private static Color[] colors = new Color[4];
		

		static KrozzyUtilities()
		{
			fontStyles[0] = (Font)Resources.Load("Fonts/Arial_small");
			fontStyles[1] = (Font)Resources.Load("Fonts/Arial_medium");
			fontStyles[2] = (Font)Resources.Load("Fonts/Calibri_tiny");
			colors[0] = Color.black;
			colors[1] = Color.white;
			colors[2] = Color.yellow;
			colors[3] = Color.red;
		}

		/// <summary>
		/// Add Rects together. Useful for windows within windows or UI groups within windows.
		/// Spdskatr - Today at 10:56 PM ...
		/// Spdskatr - Today at 10:56 PM That doesnt make sense
		/// </summary>
		/// <param name="_bAddPosistion">Add x and y</param>
		/// <param name="_bAddSize">Add width and height</param>
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
			float floCursorOffsetX = 22f;
			float floCursorOffsetY = 24f;
			Vector2 vectMouse = MousePosition();
			Vector2 vectTooltipSize0 = new Vector2();
			Vector2 vectTooltipSize1 = Text.CalcSize(_strTooltip);
			Vector2 vectTooltipSize2 = new Vector2((float)UI.screenWidth * .3f, Text.CalcHeight(_strTooltip, (float)UI.screenWidth * .3f));
			if (vectTooltipSize2.y > vectTooltipSize1.y)
			{
				vectTooltipSize0 = vectTooltipSize2;
			}
			else
			{
				vectTooltipSize0 = vectTooltipSize1;
			}
			string strTemp = _strTooltip;

			// Shape the tooltip
			vectMouse.x += floCursorOffsetX;
			vectMouse.y += floCursorOffsetY;
			Rect rectTooltipContainer = new Rect(vectMouse, vectTooltipSize0);
			Rect rectTooltip = new Rect(0f, 0f, vectTooltipSize0.x, vectTooltipSize0.y);

			// Draw the tooltip
			Find.WindowStack.ImmediateWindow(140 * _strTooltip.GetHashCode() + 85708, rectTooltipContainer, WindowLayer.Super, delegate
			{
			Widgets.DrawBoxSolid(rectTooltip, new Color(0f, 0f, 0f, .7f));
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

		/// <summary>
		/// Returns the true mouse position, inverted to work with the UI.
		/// </summary>
		public static Vector2 MousePosition()
		{
			Vector2 mousePositionOnUI = UI.MousePositionOnUI;
			mousePositionOnUI.y = (float)UI.screenHeight - mousePositionOnUI.y;
			return mousePositionOnUI;
		}

		public static GUIStyle BuildStyle(Fonts _font, Colors _color, FontStyle _style = FontStyle.Normal, TextAnchor _textAnchor = TextAnchor.UpperLeft)
		{
			GUIStyle style = new GUIStyle() 
			{
				font = fontStyles[(int)_font]
			};
			
			style.normal.textColor = colors[(int)_color];
            style.wordWrap = true;
			style.fontStyle = _style;
			style.alignment = _textAnchor;

			return style;
		}

		public static GUIStyle BuildStyleButton(Fonts _font, Colors _color,  FontStyle _style = FontStyle.Normal, TextAnchor _textAnchor = TextAnchor.UpperLeft)
		{
			GUIStyle style = new GUIStyle() {
				font = fontStyles[(int)_font]
			};
			//style.onNormal = UnityEngine.UI.
			style.normal.textColor = colors[(int)_color];
			style.wordWrap = true;
			style.fontStyle = _style;
			style.alignment = _textAnchor;

			return style;
		}


	}

	public enum Colors
	{
		Black,
		White,
		Yellow,
		Red
	}

	public enum Fonts
	{
		Arial_small,
		Arial_medium,
		Calibri_tiny
	}	

}
