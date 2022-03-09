using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ntreev.Library.Psd;
using UnityEditor;
using UnityEngine;


namespace Library.Editor
{
    public class PsdUiBuilder
    {
        public static class KeyWords
        {
            public const string Image = "\\[image]";
            public const string Button = "\\[button]";
            public const string Text = "\\[text]";

            public const string Ignore = "\\[ignore]";
            public const string Color = "\\[color]";
            public const string Sliced = "\\[sliced:[+-]?([0-9]*[.])?[0-9]+:[+-]?([0-9]*[.])?[0-9]+\\]";
            public const string Size = "\\[size:[+-]?([0-9]*[.])?[0-9]+\\]";
            public const string Bold = "\\[b]";
            public const string Italic = "\\[i]";
            public const string Underline = "\\[u]";
            public const string CharacterSpacing = "\\[space:[+-]?([0-9]*[.])?[0-9]+\\]";
            public const string LineSpacing = "\\[line:[+-]?([0-9]*[.])?[0-9]+\\]";
        }


        private readonly Dictionary<string, UiElementBuilder> elementsBuilders = new Dictionary<string, UiElementBuilder>
        {
            { KeyWords.Image, new UiImageBuilder() },
            { KeyWords.Button, new UiButtonBuilder() },
            { KeyWords.Text, new UiTextBuilder() }
        };


        private readonly string[] allKeyWords =
        {
            KeyWords.Image,
            KeyWords.Button,
            KeyWords.Text,
            KeyWords.Ignore,
            KeyWords.Size,
            KeyWords.Color,
            KeyWords.Sliced,
            KeyWords.Bold,
            KeyWords.Italic,
            KeyWords.Underline,
            KeyWords.CharacterSpacing,
            KeyWords.LineSpacing
        };


        public void BuildUi(FontAssetLookup fontAssetLookup, GameObject rootGameObject, Texture texture)
        {
            CheckRootGameObject(rootGameObject);
            CheckTexture(texture);

            var filePath = AssetDatabase.GetAssetPath(texture);

            using var document = PsdDocument.Create(filePath);
            var rootSize = new Vector2Int(document.Width, document.Height);
            AddUiElements(fontAssetLookup, rootGameObject, rootSize, document.Children);
            SetupUiElements(rootGameObject);
        }


        private void AddUiElements(FontAssetLookup fontAssetLookup, GameObject rootGameObject, Vector2Int rootSize, IPsdLayer[] children)
        {
            foreach (var child in children)
            {
                var psdLayer = child as PsdLayer;
                if (psdLayer == null || !psdLayer.IsVisible)
                {
                    continue;
                }

                var currentRootGameObject = rootGameObject;

                if (!Regex.IsMatch(psdLayer.Name.ToLower(), KeyWords.Ignore))
                {
                    var elementName = GetUiElementName(psdLayer);

                    var elementGameObject = new GameObject(elementName, typeof(RectTransform));
                    elementGameObject.transform.SetParent(rootGameObject.transform, false);

                    var rect = GetRectFromLayer(psdLayer, rootSize);
                    SetRectTransform(rootGameObject, rect, elementGameObject.GetComponent<RectTransform>());

                    BuildElement(fontAssetLookup, elementGameObject, psdLayer, elementName);

                    currentRootGameObject = elementGameObject;
                }

                AddUiElements(fontAssetLookup, currentRootGameObject, rootSize, child.Children);
            }
        }


        private void SetupUiElements(GameObject rootGameObject)
        {
            for (var i = 0; i < rootGameObject.transform.childCount; i++)
            {
                var child = rootGameObject.transform.GetChild(i).gameObject;

                foreach (var elementImporterPair in elementsBuilders)
                {
                    elementImporterPair.Value.Setup(child);
                }

                SetupUiElements(child);
            }
        }


        private string GetUiElementName(IPsdLayer item)
        {
            var uiElementName = item.Name;

            foreach (var keyWord in allKeyWords)
            {
                uiElementName = Regex.Replace(uiElementName, keyWord, string.Empty);
            }

            uiElementName = uiElementName.Trim().ToPascalCase();

            return uiElementName;
        }


        private void BuildElement(FontAssetLookup fontAssetLookup, GameObject elementGameObject, PsdLayer psdLayer, string elementName)
        {
            UiElementBuilder uiElementBuilder = null;
            foreach (var elementImporterPair in elementsBuilders)
            {
                if (Regex.IsMatch(psdLayer.Name, elementImporterPair.Key))
                {
                    uiElementBuilder = elementImporterPair.Value;
                }
            }

            if (uiElementBuilder == null)
            {
                if (!psdLayer.IsGroup)
                {
                    Debug.LogWarning($"No suitable ui element builder found for {elementName}");
                }

                return;
            }

            uiElementBuilder.Build(fontAssetLookup, elementGameObject, psdLayer, elementName);
        }


        private static void SetRectTransform(GameObject rootGameObject, Rect rect, RectTransform rectTransform)
        {
            rectTransform.pivot = Vector2.one * 0.5f;
            rectTransform.anchorMin = rectTransform.anchorMax = Vector2.one * 0.5f;
            rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
            rectTransform.anchoredPosition = new Vector2(rect.x, rect.y);

            rectTransform.anchoredPosition -= GetAbsoluteAnchoredPosition(rectTransform.parent.GetComponent<RectTransform>(), Vector2.zero);
        }


        private static Vector2 GetAbsoluteAnchoredPosition(RectTransform rectTransform, Vector2 current)
        {
            var hasCanvas = rectTransform.TryGetComponent<Canvas>(out _);
            if (hasCanvas || rectTransform.parent == null)
            {
                return current;
            }

            current += rectTransform.anchoredPosition;
            
            return GetAbsoluteAnchoredPosition(rectTransform.parent.GetComponent<RectTransform>(), current);
        }


        private static Rect GetRectFromLayer(PsdLayer psdLayer, Vector2Int rootSize)
        {
            var left = psdLayer.Left;
            var bottom = psdLayer.Bottom;
            var top = psdLayer.Top;
            var right = psdLayer.Right;
            var xMin = (right + left - rootSize.x) * 0.5f;
            var yMin = -(top + bottom - rootSize.y) * 0.5f;

            var x = Mathf.RoundToInt(xMin / 5.0f) * 5;
            var y = Mathf.RoundToInt(yMin / 5.0f) * 5;

            var width = psdLayer.Width;
            var height = psdLayer.Height;

            if (psdLayer.LayerType == LayerType.Text)
            {
                width = Mathf.RoundToInt(width * 1.1f);
                height = Mathf.RoundToInt(height * 1.1f);
            }

            return new Rect(x, y, width, height);
        }


        private void CheckRootGameObject(GameObject rootGameObject)
        {
            if (rootGameObject == null)
            {
                throw new Exception("No root game object");
            }

            var hasRectTransform = rootGameObject.GetComponent<RectTransform>() != null;
            if (!hasRectTransform)
            {
                throw new Exception("Root game object has to rect transform");
            }
        }


        private void CheckTexture(Texture texture)
        {
            if (texture == null)
            {
                throw new Exception("No texture");
            }

            string path = AssetDatabase.GetAssetPath(texture);

            if (path.ToLower().EndsWith(".psd") == false)
            {
                throw new Exception("Texture is not a psd file");
            }
        }
    }
}