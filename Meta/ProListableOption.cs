using System;
using UnityEngine;
using Verse;

namespace Prophecy.Meta
{
    public class ProListableOption
    {
        public string label;
        public Action action;
        private string uiHighlightTag;

        public ProListableOption(string _label, Action _action, string _uiHighlightTag = null)
        {
            label = _label;
            action = _action;
            uiHighlightTag = _uiHighlightTag;
        }

        public virtual void DrawOption(Vector2 _pos, float _width, float _height)
        {

            Rect rectButton = new Rect(_pos.x, _pos.y, _width, _height);

            if (Widgets.ButtonText(rectButton, this.label, true, true, true))
            {
                this.action();
            }
            if (this.uiHighlightTag != null)
            {
                UIHighlighter.HighlightOpportunity(rectButton, this.uiHighlightTag);
            }
        }
    }
}
