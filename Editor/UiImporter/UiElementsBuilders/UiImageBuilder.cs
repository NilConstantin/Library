using Ntreev.Library.Psd;
using UnityEngine;
using UnityEngine.UI;


namespace Library.Editor
{
    public class UiImageBuilder : UiElementBuilder
    {
        public override void Build(FontAssetLookup fontAssetLookup, GameObject elementGameObject, PsdLayer psdLayer, string elementName)
        {
            var image = elementGameObject.AddComponent<Image>();
            
            if (IsLayerNameContainsPattern(psdLayer, PsdUiBuilder.KeyWords.Color))
            {
                image.color = ImageHelper.GetLayerColor(psdLayer);
            }
            else if (IsLayerNameContainsPattern(psdLayer, PsdUiBuilder.KeyWords.Sliced))
            {
                var isWidthOk = TryGetIntNumberFromLayerName(psdLayer, PsdUiBuilder.KeyWords.Sliced, 0, out var width); 
                var isHeightOk = TryGetIntNumberFromLayerName(psdLayer, PsdUiBuilder.KeyWords.Sliced, 1, out var height);
                if (isWidthOk && isHeightOk)
                {
                    var border = Mathf.FloorToInt(Mathf.Min(width, height) * 0.34f);
                    var sprite = CreateSlicedSpriteAsset(psdLayer, elementName, width, height, border);
                    image.sprite = sprite;
                    image.type = Image.Type.Sliced;
                }
            }
            else
            {
                var sprite = CreateSpriteAsset(psdLayer, elementName);
                image.sprite = sprite;   
            }
        }
    }
}