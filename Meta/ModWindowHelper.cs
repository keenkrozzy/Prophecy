using Verse;
using UnityEngine;
using System.Collections.Generic;
using System;
using RimWorld;
using Verse.Sound;

namespace Prophesy.Meta
{
	// reference: https://github.com/erdelf/GodsOfRimworld/blob/master/Source/Ankh/ModControl.cs
	// reference: https://github.com/erdelf/PrisonerRansom/
	// Idea Credit: Why_is_that - https://github.com/AaronCRobinson

	[StaticConstructorOnStartup]
	public static class ModWindowHelper
	{
		private static Texture2D texRBOn = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOn", true);
		private static Texture2D texRBOff = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOff", true);
		static string strConfigWarning = "rectMain has zero position and size." + System.Environment.NewLine  + "Did you forget to use ConfigureModWindow?";
		static Texture2D texPanelBG = SolidColorMaterials.NewSolidColorTexture(new Color(.113f, .16f, .384f, .75f));
		static Texture2D texPanelBorder = SolidColorMaterials.NewSolidColorTexture(new Color(.894f, .901f, .925f, .9f));
		static float floPadding = 0f;
		static float floSpacingY = 0f;
		static Vector2 vectCurPos = new Vector2(0f, 0f);
		static Rect encapRectMain;
		static Rect rectMain	
		{
			get
			{
				// Do warning if rectMain is returning zeros
				Rect rect = new Rect();
				if (encapRectMain == rect)
				{
					// Shape the warning
					Vector2 vectConfigWarning = Text.CalcSize(strConfigWarning);
					Rect rectWindow = new Rect((UI.screenWidth * .5f) - (vectConfigWarning.x * .5f), (UI.screenHeight * .5f) - (vectConfigWarning.y * .5f), vectConfigWarning.x, vectConfigWarning.y);
					Rect rectSloppyYellowBorder = rectWindow.ContractedBy(-vectConfigWarning.y * .1f);

					// Draw the warning
					Find.WindowStack.ImmediateWindow(2006, rectSloppyYellowBorder, WindowLayer.Super, () => Widgets.DrawBoxSolid(rectSloppyYellowBorder.AtZero(), Color.yellow));
					Find.WindowStack.ImmediateWindow(2005, rectWindow, WindowLayer.Super, () => Widgets.Label(rectWindow.AtZero(), strConfigWarning));	
				}

				return encapRectMain;
			}

			set
			{
				encapRectMain = value;
			}
		}

		/// <summary>
		/// Configure shared values.
		/// </summary>
		/// <param name="_inRect">Normaly inRect from DoSettingsWindowContents</param>
		/// <param name="_padding">Edge of window padding</param>
		/// <param name="_spacing">Verticale space between lines</param>
		static public void ConfigureModWindow(Rect _inRect, float _padding, float _spacing)
		{
			rectMain = _inRect;
			floPadding = _padding;
			floSpacingY = _spacing;

			vectCurPos = new Vector2();
		}

		static public void MakeLabel(string _label)
		{
			// Shape Label
			Vector2 vectLabelSize = Text.CalcSize(_label);
			Vector2 vectLabelPos = vectCurPos;
			vectCurPos.y += vectLabelSize.y;
			Rect rectLabel = new Rect(vectLabelPos + rectMain.position, vectLabelSize);

			// Draw Label
			Widgets.Label(rectLabel, _label);
		}

		static public void MakeLabeledCheckbox(string _label, ref bool _val)
		{
			// Shape Label
			Vector2 vectLabelSize = Text.CalcSize(_label);
			Vector2 vectLabelPos = vectCurPos;
			vectCurPos.y += vectLabelSize.y;
			Rect rectLabel = new Rect(vectLabelPos + rectMain.position, vectLabelSize);

			// Draw Label
			Widgets.Label(rectLabel, _label);
			Widgets.Checkbox(vectLabelPos.x + vectLabelSize.x + rectMain.x, vectLabelPos.y + rectMain.y, ref _val);

		}

		/// <summary>
		/// Pass an enum by ref to create a RadioButtonList. Must be cast to object before, then cast back to the enum type afterwards.
		/// Example:
		/// DietCategory diet = DietCategory.Omnivorous;
		/// var dietTemp = (object)diet;
		/// ModWindowHelper.MakeRadioButtonList("Diet", ref dietTemp);
		/// diet = (DietCategory)dietTemp;
		/// </summary>
		/// <param name="_listLabel">Group Label</param>
		/// <param name="_enumInstance">enum instance (don't forget to ref)</param>
		static public void MakeRadioButtonList(string _listLabel, ref object _enumInstance)
		{
			// Shape the list label
			Vector2 vectLabelSize = Text.CalcSize(_listLabel);
			Vector2 vectLabelPos = vectCurPos;
			vectCurPos.y += vectLabelSize.y;
			Rect rectLabel = new Rect(vectLabelPos + rectMain.position, vectLabelSize);

			// Draw the list label
			Widgets.Label(rectLabel, _listLabel);

			//var enumValues = Enum.GetValues(_Enum.GetType());
			var enumValues = Enum.GetValues(_enumInstance.GetType());

			foreach (var e in enumValues)
			{
				// Shape the radiobutton
				Vector2 vectRBLabelSize = Text.CalcSize(e.ToString());
				Vector2 vectRBLabelPos = vectCurPos;
				vectCurPos.y += vectRBLabelSize.y;
				Rect rectRBLabel = new Rect(vectRBLabelPos + rectMain.position, vectRBLabelSize);
				Rect rectRBButton = new Rect(rectRBLabel.x + rectRBLabel.width, rectRBLabel.y, vectRBLabelSize.y, vectRBLabelSize.y);
				Rect rectRB = new Rect(rectRBLabel.x, rectRBLabel.y, rectRBLabel.width + rectRBButton.width, rectRBLabel.height);

				// Draw the radiobutton label
				Widgets.Label(rectRBLabel, e.ToString());

				// Draw the invisible button
				bool flag = Widgets.ButtonInvisible(rectRB, true);
				bool flag2 = e.Equals(_enumInstance);

				if (flag && !flag2)
				{
					SoundDefOf.RadioButtonClicked.PlayOneShotOnCamera(null);
					_enumInstance = (object)e;

				}
				RadioButtonDraw(rectRBButton, flag2);
			}
		}

		private static void RadioButtonDraw(Rect _rect, bool _chosen)
		{
			Texture2D image;
			if (_chosen)
			{
				image = texRBOn;
			}
			else
			{
				image = texRBOff;
			}

			GUI.DrawTexture(_rect, image);
		}

	}
}
