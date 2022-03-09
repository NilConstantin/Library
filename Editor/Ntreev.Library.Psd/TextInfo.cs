using Ntreev.Library.Psd.Structures;
using System.Collections;
using System;
using System.Globalization;
using UnityEngine;

namespace Ntreev.Library.Psd
{
    public class TextInfo
    {
        public string text;
        public float[] color;
        public int fontSize;
        public float characterSpacing;
        public string fontName;
        public TextInfo(DescriptorStructure text)
        {
            this.text = text["Txt"].ToString().Replace("\r", "\n").Replace("\n\n", "\n");
            //颜色路径EngineData/EngineDict/StyleRun/RunArray/StyleSheet/StyleSheetData/FillColor
            //UnityEngine.Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(text));
            var engineData = text["EngineData"] as StructureEngineData;
            var engineDict = engineData["EngineDict"] as Properties;

            var documentResources = engineData["DocumentResources"] as Properties;
            var fontSet = documentResources["FontSet"] as ArrayList;

            var stylerun = engineDict["StyleRun"] as Properties;
            var runarray = stylerun["RunArray"] as ArrayList;
            var styleSheet = (runarray[0] as Properties)["StyleSheet"] as Properties;
            var styleSheetsData = (styleSheet as Properties)["StyleSheetData"] as Properties;
            if (styleSheetsData.Contains("FontSize"))
            {
                var sourceFontSize = Convert.ToSingle(styleSheetsData["FontSize"], CultureInfo.InvariantCulture);
                //sourceFontSize *= (1.0f - 0.17f);
                fontSize = Convert.ToInt32(sourceFontSize, CultureInfo.InvariantCulture);
                
                var sourceCharacterSpacing = Convert.ToSingle(styleSheetsData["Tracking"], CultureInfo.InvariantCulture) / 5.0f;
                characterSpacing = Convert.ToInt32(sourceCharacterSpacing, CultureInfo.InvariantCulture);
            }
            if (styleSheetsData.Contains("Font"))
            {
                var fontIndex = Convert.ToInt32(styleSheetsData["Font"], CultureInfo.InvariantCulture);
                var fontData = fontSet[fontIndex] as Properties;
                fontName = fontData["Name"] as string;
            }

            if (styleSheetsData.Contains("FillColor"))
            {
                var strokeColorProp = styleSheetsData["FillColor"] as Properties;
                var strokeColor = strokeColorProp["Values"] as ArrayList;
                if (strokeColor != null && strokeColor.Count >= 4)
                {
                    color = new [] 
                    {
                        Convert.ToSingle(strokeColor[1].ToString(), CultureInfo.InvariantCulture),
                        Convert.ToSingle(strokeColor[2].ToString(), CultureInfo.InvariantCulture),
                        Convert.ToSingle(strokeColor[3].ToString(), CultureInfo.InvariantCulture),
                        Convert.ToSingle(strokeColor[0].ToString(), CultureInfo.InvariantCulture)
                        
                    };
                }
                else
                {
                    color =new float[4] { 0,0,0,1};
                }
            }
            else
            {
                color = new float[4] { 0, 0, 0, 1 };
            }
        }
    }
}
