using TMPro;
using UnityEngine;


namespace Library.Editor
{
    [CreateAssetMenu(fileName = "FontAssetLookup", menuName = "Text Data/Font Asset Lookup")]
    public class FontAssetLookup : ScriptableObject
    {
        [SerializeField] private FontAssetPair[] fontAssets;


        public TMP_FontAsset GetFontAsset(string fontName)
        {
            foreach (var fontAssetPair in fontAssets)
            {
                if (fontAssetPair.FontName == fontName)
                {
                    return fontAssetPair.FontAsset;
                }
            }

            Debug.LogError($"Font with name {fontName} not found");
            return null;
        }
    }
}