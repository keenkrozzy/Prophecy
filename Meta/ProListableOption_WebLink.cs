using System;
using UnityEngine;
using Verse;

namespace Prophecy.Meta
{
    public class ProListableOption_WebLink : ProListableOption
    {
        public string URL;
        public Texture2D texture;
        private string uiHighlightTag;

        public ProListableOption_WebLink(string _label, Action _action = null, string _URL = null, Texture2D _texture = null, string _uiHighlightTag = null) :base(_label, _action)
        {
            label = _label;
            action = _action;
            URL = _URL;
            texture = _texture;
            uiHighlightTag = _uiHighlightTag;

        }

        public override void DrawOption(Vector2 _pos, float _width, float _height)
        {
            Rect rectButton = new Rect(_pos.x, _pos.y, _width, _height);

            if (Mouse.IsOver(rectButton))
            {
                GUI.color = Widgets.MouseoverOptionColor;
            }

            if (texture != null)
            {
                float floHeightRatio = _height / texture.height;
                Rect rectTexture = new Rect(_pos.x, _pos.y, texture.width * floHeightRatio, _height);
                float floLabelHeight = Text.CalcHeight(label, _width);
                Rect rectLabel = new Rect(_pos.x + rectTexture.width, _pos.y + ((_height - floLabelHeight) / 2f), _width - rectTexture.width, floLabelHeight);

                GUI.DrawTexture(rectTexture, texture);
                Widgets.Label(rectLabel, label);
            }
            else
            {
                float floLabelHeight = Text.CalcHeight(label, _width);
                Rect rectLabel = new Rect(_pos.x, _pos.y + ((_height - floLabelHeight) / 2f), _width, floLabelHeight);

                Widgets.Label(rectLabel, label);
            }

            

            if (Widgets.ButtonInvisible(rectButton, true))
            {
                if (action != null)
                    action();

                if (URL != null)
                    Application.OpenURL(URL);
            }

            

            if (this.uiHighlightTag != null)
            {
                UIHighlighter.HighlightOpportunity(rectButton, this.uiHighlightTag);
            }

            GUI.color = Color.white;
        }
    }
}
