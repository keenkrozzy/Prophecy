using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;
using Prophesy.Meta;

namespace Prophesy.PreGame
{
    class ProPNC : Page
    {
		/************************************
		* Configure new player colony pawns *
		************************************/
        private const float TabAreaHeight = 30f;

        private const float RectAreaWidth = 100f;

        private const float RightRectLeftPadding = 5f;

        private Pawn curPawn;

        //private readonly static Vector2 PawnPortraitSize;

		//List<Pawn> lstPawns = new List<Pawn>();

		private List<Rect> lstCardRects = new List<Rect>();

		private List<PNC_Card> lstPNC_Cards = new List<PNC_Card>();
		private List<PNC_Card> lstPNC_CardsInOrder = new List<PNC_Card>();

		private List<PNC_Card> templstPNC_CardsInOrder = new List<PNC_Card>();

		private bool boolSwitchingOut = false;
		private bool boolSwitchingIn = false;

		private int intSwitchAnimSteps = 30;
		private int intCurSwitchAnimStep = 0;

		private int intCardNum = 0;

		private int intPoints = 1000;

		private Rect rectCardLabelAdjust = new Rect();

		private Rect rectCard = new Rect();

		private Rect rectButtonInvisibleAdjust = new Rect();

		// Create New Game

		//public static void BeginScenarioConfiguration(Scenario scen, Page originPage)
		//{
		//    Current.Game = new Game();
		//    Current.Game.InitData = new GameInitData();
		//    Current.Game.Scenario = scen;
		//    Current.Game.Scenario.PreConfigure();
		//    Page firstConfigPage = Current.Game.Scenario.GetFirstConfigPage();
		//    if (firstConfigPage == null)
		//    {
		//        PageUtility.InitGameStart();
		//        return;
		//    }
		//    originPage.next = firstConfigPage;
		//    firstConfigPage.prev = originPage;
		//}

		public void BeginScenarioConfiguration()
        {
            //Current.Game = new Game();
            //Current.Game.InitData = new GameInitData();

            //ProSM.GenScenSkeleton("testing");

            //Current.Game.Scenario = ProSM.ScenWIP;

            //Current.Game.Scenario.PreConfigure();

            //Page firstConfigPage = Current.Game.Scenario.GetFirstConfigPage();

            //if (firstConfigPage == null)
            //{
            //    PageUtility.InitGameStart();
            //    return;
            //}
            //originPage.next = firstConfigPage;
            //firstConfigPage.prev = originPage;

            //Find.GameInitData.startingPawns.Add(StartingPawnUtility.NewGeneratedStartingPawn());

            //Current.ProgramState = ProgramState.Entry;
            //Current.Game = new Game();
            //Current.Game.InitData = new GameInitData();
            //Current.Game.Scenario = ProSM.GenScenSkeleton("Test Scenario");
            Find.Scenario.PreConfigure();
            Current.Game.storyteller = new Storyteller(StorytellerDefOf.Cassandra, DifficultyDefOf.Hard);
            //Current.Game.World = WorldGenerator.GenerateWorld(0.05f, GenText.RandomSeedString(), OverallRainfall.Normal, OverallTemperature.Normal);
            //Rand.RandomizeStateFromTime();
            //Find.GameInitData.ChooseRandomStartingTile();
            //Find.GameInitData.mapSize = 150;
            //Find.GameInitData.PrepForMapGen();
            //Find.Scenario.PreMapGenerate();


            //ProDebug
            Log.Message(Find.Scenario.ToString());
        }

        public override string PageTitle
        {
            get
            {
                return "CreateCharacters".Translate();
            }
        }

        static ProPNC()
        {
           // ProPNC.PawnPortraitSize = new Vector2(100f, 140f);
        }

        public ProPNC()
        {
        }

        



        public override void DoWindowContents(Rect rect)
        {
			// Draw title.
			base.DrawPageTitle(rect);

			// Shape Cards
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			float floCardX = mainRect.x + mainRect.width * .15f;
			float floCardY = mainRect.y;
			float floCardWidth = mainRect.width - mainRect.width * .2f;
			float floCardHeight = mainRect.height - mainRect.height * .18f;			
			rectCardLabelAdjust.Set(mainRect.width * .01f , 0f, 0f, 0f);
			rectCard.Set(floCardX, floCardY, floCardWidth, floCardHeight);
			rectButtonInvisibleAdjust.Set(0f, 0f, 0f, - floCardHeight + (mainRect.height * .05f));		

			// Draw Cards
			DoCards(lstPNC_CardsInOrder, mainRect);

			//Draw Points
			DoPoints(mainRect);

			//try
			//{
			//    Vector2 pawnPortraitSize = ProPNC.PawnPortraitSize;
			//    float pawnPortraitSize1 = ProPNC.PawnPortraitSize.x;
			//    Vector2 vector2 = ProPNC.PawnPortraitSize;


			//    Rect rect3 = new Rect(single + (single1 - pawnPortraitSize.x) / 2f - 10f, rect2.yMin + 20f, pawnPortraitSize1, vector2.y);
			//    Pawn pawn = this.curPawn;

			//    Vector2 vector21 = ProPNC.PawnPortraitSize;
			//    Vector3 vector3 = new Vector3();
			//    GUI.DrawTexture(rect3, PortraitsCache.Get(pawn, vector21, vector3, 1f));
			//}
			//catch
			//{

			//}

			base.DoBottomButtons(rect, "Start".Translate(), null, null, true);
        }

		private void DoCards(List<PNC_Card> _lstCards, Rect mainRect)
		{
			if (boolSwitchingIn == false && boolSwitchingOut == false)
			{
				int intCurCard = 1;
				Rect rect = rectCard;
				float floCardY = 0f;

				// Draw Cards
				foreach (PNC_Card card in _lstCards)
				{
					Rect rectYadjusted = new Rect(rect.x, rect.y + floCardY, rect.width, rect.height);

					

					// Draw card tops only for cards not at the top of the stack
					if (intCurCard != _lstCards.Count)
					{
						card.DrawCardTopOnly(KrozzyUtilities.RectAddition(rectYadjusted, rectCardLabelAdjust));

						// Draw invisible button at top of cards, except top of the stack
						if (Widgets.ButtonInvisible(KrozzyUtilities.RectAddition(rectButtonInvisibleAdjust, rectYadjusted, true, true), true))
						{
							SwitchCardOut(intCurCard);
							//Log.Message(card.GetLabel() + " Clicked.");
							break;
						}
					}
					else // Draw card that is visible when cards are not being switched.
					{
						card.DrawCard(KrozzyUtilities.RectAddition(rectYadjusted, rectCardLabelAdjust));
					}

					intCurCard += 1;
					floCardY = floCardY + (mainRect.height * .05f);

				}
			}
			else
			{
				if (boolSwitchingOut == true)
				{
					Rect rect = rectCard;
					float floCardY = 0f;

					// Shape switching cards X
					float floCardXMax = mainRect.x + mainRect.width;
					float floCardXAdjust = floCardXMax - rect.x;
					float floOutX = rect.x + (((float)intCurSwitchAnimStep / (float)intSwitchAnimSteps) * floCardXAdjust);

					// Shape not switching cards Y
					float floCardYMax2 = (mainRect.height * .05f) * templstPNC_CardsInOrder.Count;
					//Log.Message("mainRect.height: " + mainRect.height.ToString());
					float floCardYAdjust2 = floCardYMax2 + rect.y;
					float floOutY2 = ((float)intCurSwitchAnimStep / (float)intSwitchAnimSteps) * floCardYAdjust2;

					// Shape switching cards Y
					float floCardYMax = -((mainRect.height * .05f) * lstPNC_CardsInOrder.Count);
					float floCardYAdjust = floCardYMax + rect.y;
					float floOutY = ((float)intCurSwitchAnimStep / (float)intSwitchAnimSteps) * floCardYAdjust;

					// Draw not switching cards
					foreach (PNC_Card notSwitchCard in lstPNC_CardsInOrder)
					{
						Rect rectYadjusted = new Rect(rect.x, floOutY2 + floCardY, rect.width, rect.height);
						//Log.Message("ProPNC.DoCards (Draw not switching cards)" + notSwitchCard.GetLabel() + " " + KrozzyUtilities.RectAddition(rectYadjusted, rectCardLabelAdjust).ToString() + " intCurSwitchAnimStep " + intCurSwitchAnimStep.ToString());
						notSwitchCard.DrawCard(KrozzyUtilities.RectAddition(rectYadjusted, rectCardLabelAdjust));
						floCardY = floCardY + (mainRect.height * .05f);		
					}

					// Draw switching cards
					foreach (PNC_Card SwitchCard in templstPNC_CardsInOrder)
					{
						Rect rectYadjusted = new Rect(floOutX, floOutY + floCardY, rect.width, rect.height);
						SwitchCard.DrawCard(KrozzyUtilities.RectAddition(rectYadjusted, rectCardLabelAdjust));
						floCardY = floCardY + (mainRect.height * .05f);
					}

					if (intCurSwitchAnimStep >= intSwitchAnimSteps)
					{
						boolSwitchingIn = true;
						boolSwitchingOut = false;
						//Log.Message("Switching out > Switching In");
					}
					else
					{
						intCurSwitchAnimStep++;
					}
				}
				else if (boolSwitchingIn == true)
				{
					Rect rect = rectCard;
					float floCardY = (mainRect.height * .05f) * templstPNC_CardsInOrder.Count;

					// Shape switching cards X
					float floCardXMax = mainRect.x + mainRect.width;
					float floCardXAdjust = floCardXMax - rect.x;
					float floOutX = rect.x + (((float)intCurSwitchAnimStep / (float)intSwitchAnimSteps) * floCardXAdjust);

					// Shape switching cards Y
					float floOutY = 0f;

					// Draw switching cards
					foreach (PNC_Card SwitchCard in templstPNC_CardsInOrder)
					{
						Rect rectYadjusted = new Rect(floOutX, rect.y + floOutY, rect.width, rect.height);
						SwitchCard.DrawCard(KrozzyUtilities.RectAddition(rectYadjusted, rectCardLabelAdjust));
						floOutY = floOutY + (mainRect.height * .05f);
					}

					// Draw not switching cards
					foreach (PNC_Card notSwitchCard in lstPNC_CardsInOrder)
					{
						Rect rectYadjusted = new Rect(rect.x, rect.y + floCardY, rect.width, rect.height);
						notSwitchCard.DrawCard(KrozzyUtilities.RectAddition(rectYadjusted, rectCardLabelAdjust));
						floCardY = floCardY + (mainRect.height * .05f);
					}

					

					if (intCurSwitchAnimStep <= 1)
					{
						boolSwitchingOut = false;
						boolSwitchingIn = false;
						SwitchCardIn();
					}
					else
					{
						intCurSwitchAnimStep--;
					}
				}
				else
				{
					Log.Message("Error in ProPNC.DoCards, if boolSwitchingIn or Out triggered true but neither handled as true");
				}
			}
		}

		private void SwitchCardOut(int _cardNum)
		{
			templstPNC_CardsInOrder = lstPNC_CardsInOrder.GetRange(_cardNum, intCardNum - _cardNum);

			lstPNC_CardsInOrder.RemoveRange(_cardNum, intCardNum - _cardNum);

			boolSwitchingOut = true;
		}

		private void SwitchCardIn()
		{
			templstPNC_CardsInOrder.AddRange(lstPNC_CardsInOrder);

			lstPNC_CardsInOrder.Clear();
			lstPNC_CardsInOrder.AddRange(templstPNC_CardsInOrder);
			templstPNC_CardsInOrder.Clear();
		}

		private void DoPoints(Rect _rect)
		{
			// Make string
			string strLabel = "Points: " + intPoints.ToString();

			// Shape label
			Rect rectLabel = new Rect(_rect.x, _rect.y, Text.CalcSize(strLabel).x, Text.CalcSize(strLabel).y);

			// Draw label
			Widgets.Label(rectLabel, strLabel);
		}

		private void PopulateCards()
		{
			// Populate Cards
			if (lstPNC_Cards.Count + 1 != intCardNum)
			{
				List<Pawn> lstPawns = new List<Pawn>(Find.GameInitData.startingPawns);
				int intcardsneeded = lstPawns.Count + 1;

				// Make Pawn Cards
				for (int x = 0; x < intcardsneeded - 1; x++)
				{
					lstPNC_Cards.Add(new PNC_Card(lstPawns[x], intcardsneeded));
					Log.Message("Added pawn card");
				}

				// Make Items Card
				lstPNC_Cards.Add(new PNC_Card(null, intcardsneeded));
                Log.Message("Added item card");
                intCardNum = intcardsneeded;
			}

			// Set initial Card Order
			foreach (PNC_Card card in lstPNC_Cards)
			{
				lstPNC_CardsInOrder.Add(card);
			}
			
		}



		public override void PostOpen()
        {
            base.PostOpen();
            TutorSystem.Notify_Event("PageStart-ConfigureStartingPawns");

			// Get Pawns
			//lstPawns = Find.GameInitData.startingPawns;

			// Set initial Cards
			PopulateCards();
		}

        public override void PreOpen()
        {
            // Create the game and load the scenario from Static Scen_Test
            BeginScenarioConfiguration();

            base.PreOpen();
            if (Find.GameInitData.startingPawns.Count > 0)
            {
                //this.curPawn = Find.GameInitData.startingPawns[0];
                nextAct = () => PageUtility.InitGameStart();
            }
			
			


		}

		protected override bool CanDoNext()
		{
			bool flag;
			if (!base.CanDoNext())
			{
				return false;
			}
			List<Pawn>.Enumerator enumerator = Find.GameInitData.startingPawns.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Name.IsValid)
					{
						continue;
					}
					Messages.Message("EveryoneNeedsValidName".Translate(), MessageSound.RejectInput);
					flag = false;
					return flag;
				}
				PortraitsCache.Clear();
				return true;
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return flag;
		}

		//private void RandomizeCurPawn()
		//{
		//    //ProDebug
		//    Log.Message("RandomizeCurePawn fired.");

		//    if (!TutorSystem.AllowAction("RandomizePawn"))
		//    {
		//        return;
		//    }
		//    int num = 0;
		//    do
		//    {
		//        this.curPawn = StartingPawnUtility.RandomizeInPlace(this.curPawn);
		//        num++;
		//        if (num <= 15)
		//        {
		//            continue;
		//        }
		//        return;
		//    }
		//    while (!StartingPawnUtility.WorkTypeRequirementsSatisfied());
		//    TutorSystem.Notify_Event("RandomizePawn");
		//}

		//public void SelectPawn(Pawn c)
		//{
		//    if (c != this.curPawn)
		//    {
		//        this.curPawn = c;
		//    }
		//}
	}
}

