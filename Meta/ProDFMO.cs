using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace Prophesy.Meta
{
    // Attempting to make a dialogue box that can display FloatMenuOptions using code from FloatMenu
    class ProDFMO : Window
    {
        /***************************
        * Dialog_MessageBox Fields * 
        ***************************/
        private const float TitleHeight = 42f;
        private const float ButtonHeight = 35f;
        public string text = null;
        public string title;
        public string buttonAText;
        public Action buttonAAction;
        public bool buttonADestructive;
        public string buttonBText;
        public Action buttonBAction;
        public float interactionDelay = 0f;
        private Vector2 scrollPosition = Vector2.zero;
        private float creationRealTime = -1f;

        /*******************
        * FloatMenu Fields * 
        *******************/
        private const float OptionSpacing = -1f;
        private const float MaxScreenHeightPercent = 0.9f;
        private const float MinimumColumnWidth = 70f;
        private const float FadeStartMouseDist = 5f;
        private const float FadeFinishMouseDist = 100f;
        public bool givesColonistOrders = false;
        public bool vanishIfMouseDistant = true;
        protected List<FloatMenuOption> options;
        //private string title;
        private Color baseColor = Color.white;
        //private Vector2 scrollPosition;
        //private readonly static Vector2 TitleOffset;
        //private readonly static Vector2 InitialPositionShift;


        //Dialog_MessageBox Properties
        //public override Vector2 InitialSize
        //{
        //    get
        //    {
        //        return new Vector2(640f, 460f);
        //    }
        //}
        private bool InteractionDelayExpired
        {
            get
            {
                return this.TimeUntilInteractive <= 0f;
            }
        }
        private float TimeUntilInteractive
        {
            get
            {
                return this.interactionDelay - (Time.realtimeSinceStartup - this.creationRealTime);
            }
        }


        //FloatMenu Properties
        private int ColumnCount
        {
            get
            {
                return Mathf.Min(this.ColumnCountIfNoScrollbar, this.MaxColumns);
            }
        }
        private int ColumnCountIfNoScrollbar
        {
            get
            {
                if (this.options == null)
                {
                    return 1;
                }
                Text.Font = GameFont.Small;
                int num = 1;
                float single = 0f;
                float maxWindowHeight = this.MaxWindowHeight;
                for (int i = 0; i < this.options.Count; i++)
                {
                    float requiredHeight = this.options[i].RequiredHeight;
                    if (single + requiredHeight + -1f <= maxWindowHeight)
                    {
                        single = single + (requiredHeight + -1f);
                    }
                    else
                    {
                        single = requiredHeight;
                        num++;
                    }
                }
                return num;
            }
        }
        private float ColumnWidth
        {
            get
            {
                float single = 70f;
                for (int i = 0; i < this.options.Count; i++)
                {
                    float requiredWidth = this.options[i].RequiredWidth;
                    if (requiredWidth >= 300f)
                    {
                        return 300f;
                    }
                    if (requiredWidth > single)
                    {
                        single = requiredWidth;
                    }
                }
                return Mathf.Round(single);
            }
        }
        //public override Vector2 InitialSize
        //{
        //    get
        //    {
        //        return new Vector2(this.TotalWidth, this.TotalWindowHeight);
        //    }
        //}
        //protected override float Margin
        //{
        //    get
        //    {
        //        return 0f;
        //    }
        //}
        private int MaxColumns
        {
            get
            {
                return Mathf.FloorToInt(((float)UI.screenWidth - 16f) / this.ColumnWidth);
            }
        }
        private float MaxViewHeight
        {
            get
            {
                //if (!this.UsingScrollbar)
                //{
                   // return this.MaxWindowHeight;
                //}
                float single = 0f;
                float single1 = 0f;
                for (int i = 0; i < this.options.Count; i++)
                {
                    float requiredHeight = this.options[i].RequiredHeight;
                    if (requiredHeight > single)
                    {
                        single = requiredHeight;
                    }
                    single1 = single1 + (requiredHeight + -1f);
                }
                int columnCount = this.ColumnCount;
                single1 = single1 + (float)columnCount * single;
                return single1 / (float)columnCount;
            }
        }
        private float MaxWindowHeight
        {
            get
            {
                //return (float)UI.screenHeight * 0.9f;
                return 464f;
            }
        }
        public FloatMenuSizeMode SizeMode
        {
            get
            {
                if (this.options.Count > 60)
                {
                    return FloatMenuSizeMode.Tiny;
                }
                return FloatMenuSizeMode.Normal;
            }
        }
        //private float TotalViewHeight
        //{
        //    get
        //    {
        //        float single = 0f;
        //        float single1 = 0f;
        //        float maxViewHeight = this.MaxViewHeight;
        //        for (int i = 0; i < this.options.Count; i++)
        //        {
        //            float requiredHeight = this.options[i].RequiredHeight;
        //            if (single1 + requiredHeight + -1f <= maxViewHeight)
        //            {
        //                single1 = single1 + (requiredHeight + -1f);
        //            }
        //            else
        //            {
        //                if (single1 > single)
        //                {
        //                    single = single1;
        //                }
        //                single1 = requiredHeight;
        //            }
        //        }
        //        return Mathf.Max(single, single1);
        //    }
        //}
        //private float TotalWidth
        //{
        //    get
        //    {
        //        float columnCount = (float)this.ColumnCount * this.ColumnWidth;
        //        if (this.UsingScrollbar)
        //        {
        //            columnCount = columnCount + 16f;
        //        }
        //        return columnCount;
        //    }
        //}
        //private float TotalWindowHeight
        //{
        //    get
        //    {
        //        return Mathf.Min(this.TotalViewHeight, this.MaxWindowHeight) + 1f;
        //    }
        //}
        //private bool UsingScrollbar
        //{
        //    get
        //    {
        //        return this.ColumnCountIfNoScrollbar > this.MaxColumns;
        //    }
        //}

        public ProDFMO(List<FloatMenuOption> options, string buttonAText = null, Action buttonAAction = null, string buttonBText = null, Action buttonBAction = null, string _title = null, bool buttonADestructive = false)
        {
           // this.text = text;
            this.buttonAText = buttonAText;
            this.buttonAAction = buttonAAction;
            this.buttonADestructive = buttonADestructive;
            this.buttonBText = buttonBText;
            this.buttonBAction = buttonBAction;
            title = _title;
            if (buttonAText.NullOrEmpty())
            {
                this.buttonAText = "OK".Translate();
            }
            if (buttonAAction == null)
            {
                this.closeOnEscapeKey = true;
            }
            this.forcePause = true;
            this.absorbInputAroundWindow = true;
            this.closeOnEscapeKey = false;
            this.creationRealTime = RealTime.LastRealTime;
            this.onlyOneOfTypeAllowed = false;

            if (options.NullOrEmpty<FloatMenuOption>())
            {
                Log.Error("Created FloatMenu with no options. Closing.");
                this.Close(true);
            }
            this.options = options;
            for (int i = 0; i < options.Count; i++)
            {
                options[i].SetSizeMode(this.SizeMode);
            }
            //this.layer = WindowLayer.Super;
            //this.closeOnClickedOutside = true;
            //this.doWindowBackground = false;
            //this.drawShadow = false;
            SoundDefOf.FloatMenuOpen.PlayOneShotOnCamera(null);
        }

        public static Dialog_MessageBox CreateConfirmation(string text, Action confirmedAct, bool destructive = false, string title = null)
        {
            string str = title;
            return new Dialog_MessageBox(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), null, str, destructive);
        }

        public void Cancel()
        {
            SoundDefOf.FloatMenuCancel.PlayOneShotOnCamera(null);
            Find.WindowStack.TryRemove(this, true);
        }

        public override void DoWindowContents(Rect _rect)
        {
            string str;
            float floTopY = _rect.y;
            float floInnerTopY;

            if (!title.NullOrEmpty())
            {
                Text.Font = GameFont.Medium;
                Widgets.Label(new Rect(0f, floTopY, _rect.width, 42f), title);
                floInnerTopY = floTopY + 42f;
            }
            else
            {
                floInnerTopY = floTopY;
            }

            Text.Font = GameFont.Small;

            Rect innerRect = new Rect(_rect.x, floInnerTopY, _rect.width, _rect.height);

            float floInnerContentWidth = innerRect.width - 16f;
            float floInnerContentHeight = innerRect.height -35f;

            GUI.color = baseColor;

            Vector2 vector2 = Vector2.zero;
            float maxViewHeight = MaxViewHeight;
            float columnWidth = ColumnWidth;

            Widgets.BeginScrollView(innerRect, ref scrollPosition, new Rect(0f, 0f, floInnerContentWidth, floInnerContentHeight), true);

            IEnumerator<FloatMenuOption> enumerator = (
                from op in options
                orderby op.Priority descending
                select op).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    FloatMenuOption current = enumerator.Current;
                    float requiredHeight = current.RequiredHeight;
                    if (vector2.y + requiredHeight + -1f > maxViewHeight)
                    {
                        vector2.y = 0f;
                        vector2.x = vector2.x + (columnWidth + -1f);
                    }
                    Rect rect1 = new Rect(vector2.x, vector2.y, columnWidth, requiredHeight);
                    vector2.y = vector2.y + (requiredHeight + -1f);
                    if (!current.DoGUI(rect1, givesColonistOrders))
                    {
                        continue;
                    }
                    Find.WindowStack.TryRemove(this, true);
                    break;
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }

            Widgets.EndScrollView();

            if (Event.current.type == EventType.MouseDown)
            {
                Event.current.Use();
            }
            GUI.color = Color.white;

            if (buttonADestructive)
            {
                GUI.color = new Color(1f, 0.3f, 0.35f);
            }
            if (!InteractionDelayExpired)
            {
                string str1 = buttonAText;
                float single2 = Mathf.Ceil(TimeUntilInteractive);
                str = string.Concat(str1, "(", single2.ToString("F0"), ")");
            }
            else
            {
                str = buttonAText;
            }
            string str2 = str;
            float single3 = _rect.width / 2f - 20f;
            if (Widgets.ButtonText(new Rect(_rect.width / 2f + 20f, _rect.height - 35f, single3, 35f), str2, true, false, true) /*&& InteractionDelayExpired*/)
            {
                if (buttonAAction != null)
                {
                    buttonAAction();
                }
                Close(true);
            }
            GUI.color = Color.white;
            if (this.buttonBText != null && Widgets.ButtonText(new Rect(0f, _rect.height - 35f, single3, 35f), this.buttonBText, true, false, true))
            {
                if (buttonBAction != null)
                {
                    buttonBAction();
                }
                Close(true);
            }

            
        }
        //public override void ExtraOnGUI()
        //{
        //    base.ExtraOnGUI();
        //    if (!this.title.NullOrEmpty())
        //    {
        //        Vector2 vector2 = new Vector2(this.windowRect.x, this.windowRect.y);
        //        Text.Font = GameFont.Small;
        //        Vector2 vector21 = Text.CalcSize(this.title);
        //        float single = Mathf.Max(150f, 15f + vector21.x);
        //        float titleOffset = vector2.x + TitleOffset.x;
        //        float single1 = vector2.y;
        //        Vector2 titleOffset1 = TitleOffset;
        //        Rect rect2 = new Rect(titleOffset, single1 + titleOffset1.y, single, 23f);
        //        Find.WindowStack.ImmediateWindow(6830963, rect2, WindowLayer.Super, () => {
        //            GUI.color = this.baseColor;
        //            Text.Font = GameFont.Small;
        //            Rect rect = rect2.AtZero();
        //            rect.width = 150f;
        //            GUI.DrawTexture(rect, TexUI.TextBGBlack);
        //            Rect rect1 = rect2.AtZero();
        //            rect1.x = rect1.x + 15f;
        //            Text.Anchor = TextAnchor.MiddleLeft;
        //            Widgets.Label(rect1, this.title);
        //            Text.Anchor = TextAnchor.UpperLeft;
        //        }, false, false, 0f);
        //    }
        //}

        // from FloatMenu
        //protected override void SetInitialSizeAndPosition()
        //{
        //    Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted + InitialPositionShift;
        //    if (mousePositionOnUIInverted.x + this.InitialSize.x > (float)UI.screenWidth)
        //    {
        //        mousePositionOnUIInverted.x = (float)UI.screenWidth - this.InitialSize.x;
        //    }
        //    if (mousePositionOnUIInverted.y + this.InitialSize.y > (float)UI.screenHeight)
        //    {
        //        mousePositionOnUIInverted.y = (float)UI.screenHeight - this.InitialSize.y;
        //    }
        //    float single = mousePositionOnUIInverted.x;
        //    float single1 = mousePositionOnUIInverted.y;
        //    float initialSize = this.InitialSize.x;
        //    Vector2 vector2 = this.InitialSize;
        //    this.windowRect = new Rect(single, single1, initialSize, vector2.y);
        //}

        private void UpdateBaseColor()
        {
            this.baseColor = Color.white;
            //if (this.vanishIfMouseDistant)
            //{
            //    Rect rect = (new Rect(0f, 0f, this.TotalWidth, this.TotalWindowHeight)).ContractedBy(-5f);
            //    if (!rect.Contains(Event.current.mousePosition))
            //    {
            //        float single = GenUI.DistFromRect(rect, Event.current.mousePosition);
            //        this.baseColor = new Color(1f, 1f, 1f, 1f - single / 95f);
            //        if (single > 95f)
            //        {
            //            this.Close(false);
            //            this.Cancel();
            //            return;
            //        }
            //    }
            //}
        }

    }
}
