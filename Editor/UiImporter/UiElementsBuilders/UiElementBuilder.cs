using Ntreev.Library.Psd;
using UnityEngine;


namespace Library.Editor
{
    public abstract class UiElementBuilder
    {
        public abstract void Build(FontAssetLookup fontAssetLookup, GameObject elementGameObject, PsdLayer psdLayer, string elementName);

        
        public virtual void Setup(GameObject gameObject)
        {
        }


        protected Sprite CreateSpriteAsset(PsdLayer layer, string elementName)
        {
            var filePath = $"Assets/Game/RawUi/{elementName}.png";
            var texture = ImageHelper.CreateTexture(layer);
            return ImageHelper.CreateSpriteAsset(texture, filePath, 100.0f, Vector4.zero);
        }


        protected Sprite CreateSlicedSpriteAsset(PsdLayer layer, string elementName, int width, int height, int border)
        {
            var filePath = $"Assets/Game/RawUi/{elementName}.png";
            var texture = ImageHelper.CreateSlicedTexture(layer, width, height, border);
            return ImageHelper.CreateSpriteAsset(texture, filePath, 100.0f, new Vector4(border, border, border, border));
        }
        

        protected bool IsLayerNameContainsPattern(PsdLayer psdLayer, string pattern)
        {
            return ParametersHelper.IsStringContainsParameter(psdLayer.Name, pattern);
        }


        protected bool TryGetIntNumberFromLayerName(PsdLayer psdLayer, string pattern, int numberIndex, out int number)
        {
            return ParametersHelper.TryGetIntNumberFromParameters(psdLayer.Name, pattern, numberIndex, out number);
        }


        protected bool TryGetFloatNumberFromLayerName(PsdLayer psdLayer, string pattern, int numberIndex, out float number)
        {
            return ParametersHelper.TryGetFloatNumberFromParameters(psdLayer.Name, pattern, numberIndex, out number);
        }
    }
}