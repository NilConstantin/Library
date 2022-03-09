using Ntreev.Library.Psd;
using UnityEngine;
using UnityEngine.UI;


namespace Library.Editor
{
    public class UiButtonBuilder : UiElementBuilder
    {
        public override void Build(FontAssetLookup fontAssetLookup, GameObject elementGameObject, PsdLayer psdLayer, string elementName)
        {
            var button = elementGameObject.AddComponent<Button>();

            if (IsLayerNameContainsPattern(psdLayer, PsdUiBuilder.KeyWords.Image))
            {
                button.targetGraphic = elementGameObject.GetComponent<Image>();
            }
        }

        
        public override void Setup(GameObject gameObject)
        {
            base.Setup(gameObject);

            if (gameObject.TryGetComponent<Button>(out var button))
            {
                if (button.targetGraphic == null)
                {
                    button.targetGraphic = gameObject.GetComponentInChildren<Image>();    
                }
            }
        }
    }
}