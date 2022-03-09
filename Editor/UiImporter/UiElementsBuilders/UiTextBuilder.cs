using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Ntreev.Library.Psd;
using UnityEngine;
using TMPro;


namespace Library.Editor
{
    public class UiTextBuilder : UiElementBuilder
    {
        public override void Build(FontAssetLookup fontAssetLookup, GameObject elementGameObject, PsdLayer psdLayer, string elementName)
        {
            var textMeshPro = elementGameObject.AddComponent<TextMeshProUGUI>();
            
            var textInfo = psdLayer.Records.TextInfo;
            var color = new Color(textInfo.color[0], textInfo.color[1], textInfo.color[2], textInfo.color[3]);
            var fontSize = textInfo.fontSize;
            var characterSpacing = textInfo.characterSpacing;
            var lineSpacing = 0.0f;
            
            if (TryGetIntNumberFromLayerName(psdLayer, PsdUiBuilder.KeyWords.Size, 0, out var customFontSize))
            {
                fontSize = customFontSize;
            }
            
            if (TryGetFloatNumberFromLayerName(psdLayer, PsdUiBuilder.KeyWords.CharacterSpacing, 0, out var customCharacterSpacing))
            {
                characterSpacing = customCharacterSpacing;
            }
            
            if (TryGetFloatNumberFromLayerName(psdLayer, PsdUiBuilder.KeyWords.LineSpacing, 0, out var customLineSpacing))
            {
                lineSpacing = customLineSpacing;
            }

            if (IsLayerNameContainsPattern(psdLayer, PsdUiBuilder.KeyWords.Bold))
            {
                textMeshPro.fontStyle = textMeshPro.fontStyle | FontStyles.Bold;
            }
            
            if (IsLayerNameContainsPattern(psdLayer, PsdUiBuilder.KeyWords.Italic))
            {
                textMeshPro.fontStyle = textMeshPro.fontStyle | FontStyles.Italic;
            }
            
            if (IsLayerNameContainsPattern(psdLayer, PsdUiBuilder.KeyWords.Underline))
            {
                textMeshPro.fontStyle = textMeshPro.fontStyle | FontStyles.Underline;
            }

            textMeshPro.text = textInfo.text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.characterSpacing = characterSpacing;
            textMeshPro.lineSpacing = lineSpacing;
            textMeshPro.color = color;
            textMeshPro.font = fontAssetLookup.GetFontAsset(textInfo.fontName);

            textMeshPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
            textMeshPro.verticalAlignment = VerticalAlignmentOptions.Middle;

            textMeshPro.enableWordWrapping = false;
        }
    }
}