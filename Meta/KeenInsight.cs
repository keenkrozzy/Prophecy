using Verse;
using UnityEngine;

namespace Prophesy.Meta
{
	public class KeenInsight : GameComponent
	{
		private Vector2 v2BackgroundsScrollPosition = Vector2.zero;
		private bool boolMinamized = true;

		public KeenInsight(Game _game)
		{

		}

		public void DoPanel()
		{
			//Log.Message("DoPanel has fired.");
			Rect rectWindow = new Rect(0f, 0f, UI.screenWidth * .4f, UI.screenHeight);
			Rect rectOut = new Rect(0f, 0f, UI.screenWidth * .4f, UI.screenHeight * .8f);
			Rect rectView = new Rect(0f, 0f, rectOut.width - 16f, UI.screenHeight); // Scrollbar has 16f hard number

			Find.WindowStack.ImmediateWindow(41900, rectWindow, WindowLayer.Super, delegate
			{
				Widgets.BeginScrollView(rectOut, ref v2BackgroundsScrollPosition, rectView);
				GUI.DrawTexture(rectOut, SolidColorMaterials.NewSolidColorTexture(new Color(.125f, .125f, .125f, .5f)));
				Widgets.EndScrollView();

				string strClose = "Close";
				Vector2 vectCloseButtonSize = Text.CalcSize(strClose);
				vectCloseButtonSize.x += vectCloseButtonSize.y * .5f;
				Rect rectCloseButtonSize = new Rect(rectWindow.width - vectCloseButtonSize.x, rectWindow.height - vectCloseButtonSize.y, vectCloseButtonSize.x, vectCloseButtonSize.y);

				if (Widgets.ButtonText(rectCloseButtonSize, strClose, true, true))
				{
					boolMinamized = true;
				}
			});
		}

		public void DoMainButton()
		{
			Text.Font = GameFont.Tiny;
			string strMainButtonText = "KeenInsight";
			Vector2 vectSize = Text.CalcSize(strMainButtonText);
			Text.Font = GameFont.Small;
			Rect rectWindow = new Rect(0f, 0f, vectSize.x + (vectSize.y * .5f), vectSize.y);
			Rect rectButton = rectWindow;
			Find.WindowStack.ImmediateWindow(41901, rectWindow, WindowLayer.Super, delegate
			{
				Text.Font = GameFont.Tiny;
				if (Widgets.ButtonText(rectButton, strMainButtonText, true, true))
				{
					boolMinamized = false;
				}
				Text.Font = GameFont.Small;
			});
			
		}

		public override void GameComponentOnGUI()
		{
			if (boolMinamized)
			{
				DoMainButton();
			}
			else
			{
				DoPanel();
			}
		}

		public override void GameComponentUpdate()
		{
		}

		public override void GameComponentTick()
		{
		}

		public override void ExposeData()
		{
		}

		public override void FinalizeInit()
		{
		}

		public override void StartedNewGame()
		{
		}

		public override void LoadedGame()
		{
		}
	}

}
