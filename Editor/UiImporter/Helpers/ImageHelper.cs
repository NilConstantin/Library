using System;
using Ntreev.Library.Psd;
using UnityEditor;
using UnityEngine;
using System.IO;


namespace Library.Editor
{
    public static class ImageHelper
    {
        public static Color GetLayerColor(PsdLayer layer)
        {
            var result = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            if (layer.Width > 0 && layer.Height > 0)
            {
                Channel red = Array.Find(layer.Channels, i => i.Type == ChannelType.Red);
                Channel green = Array.Find(layer.Channels, i => i.Type == ChannelType.Green);
                Channel blue = Array.Find(layer.Channels, i => i.Type == ChannelType.Blue);
                Channel alpha = Array.Find(layer.Channels, i => i.Type == ChannelType.Alpha);

                result = new Color32(red.Data[0], green.Data[0], blue.Data[0], alpha.Data[0]);
            }

            return result;
        }
        
        
        public static Sprite CreateSpriteAsset(Texture2D texture, string filePath, float pixelsPerUnit, Vector4 spriteBorder)
        {
            // Write out the texture contents into the file
            // AssetDatabase.CreateAsset(texture, filePath);
            byte[] buf = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, buf);

            AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceUpdate);
            Texture2D textureObj = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);

            // Get the texture importer for the asset
            TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(filePath);
            // Read out the texture import settings so settings can be changed
            TextureImporterSettings texSetting = new TextureImporterSettings();

            textureImporter.ReadTextureSettings(texSetting);

            // Change settings
            //texSetting.spriteAlignment = 
            texSetting.spritePivot = Vector2.one * 0.5f;
            texSetting.spritePixelsPerUnit = pixelsPerUnit;
            texSetting.filterMode = FilterMode.Bilinear;
            texSetting.wrapMode = TextureWrapMode.Clamp;
            texSetting.textureType = TextureImporterType.Sprite;
            texSetting.spriteMode = (int)SpriteImportMode.Single;
            texSetting.mipmapEnabled = false;
            //Because of Premultiplied Alpha
            texSetting.alphaIsTransparency = false;
            texSetting.spriteBorder = spriteBorder;
            texSetting.npotScale = TextureImporterNPOTScale.None;
            // Set the rest of the texture settings
            //textureImporter.spritePackingTag = 
            // Write in the texture import settings
            textureImporter.SetTextureSettings(texSetting);

            EditorUtility.SetDirty(textureObj);
            AssetDatabase.WriteImportSettingsIfDirty(filePath);
            AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceUpdate);

            return (Sprite)AssetDatabase.LoadAssetAtPath(filePath, typeof(Sprite));
        }


        public static Texture2D CreateSlicedTexture(PsdLayer layer, int width, int height, int border)
        {
            var sourceTexture = CreateTexture(layer);

            if (sourceTexture.width == 0 || sourceTexture.height == 0)
            {
                return sourceTexture;
            }

            var texture = new Texture2D(width, height);

            //Left Bottom
            Graphics.CopyTexture(
                sourceTexture, 0, 0,
                0, 0, border, border,
                texture, 0, 0,
                0, 0);

            //Center Bottom
            Graphics.CopyTexture(
                sourceTexture, 0, 0,
                border, 0, width - border * 2, border,
                texture, 0, 0,
                border, 0);

            //Right Bottom
            Graphics.CopyTexture(
                sourceTexture, 0, 0,
                sourceTexture.width - border, 0, border, border,
                texture, 0, 0,
                width - border, 0);

            //Left Middle
            Graphics.CopyTexture(
                sourceTexture, 0, 0,
                0, border, border, height - border * 2,
                texture, 0, 0,
                0, border);

            //Center Middle
            Graphics.CopyTexture(
                sourceTexture, 0, 0,
                border, border, width - border * 2, height - border * 2,
                texture, 0, 0,
                border, border);

            //Right Middle
            Graphics.CopyTexture(
                sourceTexture, 0, 0,
                sourceTexture.width - border, border, border, height - border * 2,
                texture, 0, 0,
                width - border, border);

            //Left Top
            Graphics.CopyTexture(
                sourceTexture, 0, 0,
                0, sourceTexture.height - border, border, border,
                texture, 0, 0,
                0, height - border);
            
            //Center Top
            Graphics.CopyTexture(
                sourceTexture, 0, 0,
                border, sourceTexture.height - border, width - border * 2, border,
                texture, 0, 0,
                border, height - border);      
            
            //Right Top
            Graphics.CopyTexture(
                sourceTexture, 0, 0,
                sourceTexture.width - border, sourceTexture.height - border, border, border,
                texture, 0, 0,
                width - border, height - border);

            texture.Apply();
            return texture;
        }


        public static Texture2D CreateTexture(PsdLayer layer)
        {
            Debug.Assert(layer.Width != 0 && layer.Height != 0, layer.Name + ": width = height = 0");
            if (layer.Width == 0 || layer.Height == 0)
            {
                return new Texture2D(layer.Width, layer.Height);
            }

            var texture = new Texture2D(layer.Width, layer.Height);
            var pixels = new Color32[layer.Width * layer.Height];

            var red = Array.Find(layer.Channels, i => i.Type == ChannelType.Red);
            var green = Array.Find(layer.Channels, i => i.Type == ChannelType.Green);
            var blue = Array.Find(layer.Channels, i => i.Type == ChannelType.Blue);
            var alpha = Array.Find(layer.Channels, i => i.Type == ChannelType.Alpha);

            for (int i = 0; i < pixels.Length; i++)
            {
                var redErr = red == null || red.Data == null || red.Data.Length <= i;
                var greenErr = green == null || green.Data == null || green.Data.Length <= i;
                var blueErr = blue == null || blue.Data == null || blue.Data.Length <= i;
                var alphaErr = alpha == null || alpha.Data == null || alpha.Data.Length <= i;

                byte r = redErr ? (byte)0 : red.Data[i];
                byte g = greenErr ? (byte)0 : green.Data[i];
                byte b = blueErr ? (byte)0 : blue.Data[i];
                byte a = alphaErr ? (byte)255 : alpha.Data[i];

                int mod = i % texture.width;
                int n = ((texture.width - mod - 1) + i) - mod;
                pixels[pixels.Length - n - 1] = new Color32(r, g, b, a);
            }

            texture.SetPixels32(pixels);
            texture.Apply();
            return texture;
        }
    }
}