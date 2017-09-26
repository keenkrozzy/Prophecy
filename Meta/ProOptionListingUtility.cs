using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Prophecy.Meta
{
    public static class ProOptionListingUtility
    {
        public static void ProDrawOptionListing(Rect _rect, List<ProListableOption> _optList, float _yPadding)
        {
            int count = _optList.Count;
            float floAvailableHeight = _rect.height - ((count - 1) * _yPadding);
            float floButtonHeight = floAvailableHeight / count;

            GUI.BeginGroup(_rect);

            if (_rect.width > 100f)
            {
                Text.Font = GameFont.Small;
            }
            else
            {
                Text.Font = GameFont.Tiny;
            }

            float floYPos = 0f;

            foreach (ProListableOption listableOption in _optList)
            {
                listableOption.DrawOption(new Vector2(0f, 0f + floYPos ), _rect.width, floButtonHeight);
                floYPos += floButtonHeight + _yPadding;
            }

            GUI.EndGroup();
        }

        public static void ProDrawOptionListing(Rect _rect, List<ProListableOption_WebLink> _optList, float _yPadding)
        {
            int count = _optList.Count;
            float floAvailableHeight = _rect.height - ((count - 1) * _yPadding);
            float floButtonHeight = floAvailableHeight / count;

            GUI.BeginGroup(_rect);

            if (_rect.width > 100f)
            {
                Text.Font = GameFont.Small;
            }
            else
            {
                Text.Font = GameFont.Tiny;
            }

            float floYPos = 0f;

            foreach (ProListableOption_WebLink listableOption in _optList)
            {
                listableOption.DrawOption(new Vector2(0f, 0f + floYPos), _rect.width, floButtonHeight);
                floYPos += floButtonHeight + _yPadding;
            }

            GUI.EndGroup();
        }
    }
}
