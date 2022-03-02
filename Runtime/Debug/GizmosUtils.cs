#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


namespace Library
{
    public static class GizmosUtils
    {
        public static void DrawText(GUISkin guiSkin, string text, Vector3 position, Color? color = null, int fontSize = 0, float yOffset = 0)
        {
            var prevSkin = GUI.skin;

            if (guiSkin == null)
            {
                Debug.LogWarning("editor warning: guiSkin parameter is null");
            }
            else
            {
                GUI.skin = guiSkin;
            }

            GUIContent textContent = new GUIContent(text);

            GUIStyle style = (guiSkin != null) ? new GUIStyle(guiSkin.GetStyle("Label")) : new GUIStyle();
            if (color != null)
            {
                style.normal.textColor = (Color)color;
            }
                
            if (fontSize > 0)
            {
                style.fontSize = fontSize;
            }

            Vector2 textSize = style.CalcSize(textContent);
            Vector3 screenPoint = Camera.current.WorldToScreenPoint(position);

            if (screenPoint.z > 0) // checks necessary to the text is not visible when the camera is pointed in the opposite direction relative to the object
            {
                var worldPosition = Camera.current.ScreenToWorldPoint(new Vector3(screenPoint.x - textSize.x * 0.5f, screenPoint.y + textSize.y * 0.5f + yOffset, screenPoint.z));
                Handles.Label(worldPosition, textContent, style);
            }

            GUI.skin = prevSkin;
        }
        

        public static void DrawSphere(Vector3 position, float radius, Color? color = null)
        {
            var previousColor = Handles.color;
            if (color.HasValue)
            {
                Handles.color = color.Value;
            }
            
            Handles.SphereHandleCap(0, position, Quaternion.identity, radius * 2.0f, EventType.Repaint);

            Handles.color = previousColor;
        }
        
        
        public static void DrawWireDisc(Vector3 position, float radius, Color? color = null)
        {
            var previousColor = Handles.color;
            if (color.HasValue)
            {
                Handles.color = color.Value;
            }
            
            Handles.DrawWireDisc(position, Vector3.up, radius);

            Handles.color = previousColor;
        }
        
        
        public static void DrawWireCube(Vector3 center, Vector3 size, Color? color = null)
        {
            var previousColor = Handles.color;
            if (color.HasValue)
            {
                Handles.color = color.Value;
            }

            Handles.DrawWireCube(center, size);

            Handles.color = previousColor;
        }
        
        
        public static void DrawLine(Vector3 startPosition, Vector3 endPosition, Color? color = null, float thickness = 1.5f)
        {
            var previousColor = Handles.color;
            if (color.HasValue)
            {
                Handles.color = color.Value;
            }

            Handles.DrawLine(startPosition, endPosition, thickness);

            Handles.color = previousColor;
        }
        
        
        public static void DrawArrow(Vector3 startPosition, Quaternion rotation, Color? color = null, float size = 1.0f)
        {
            var previousColor = Handles.color;
            if (color.HasValue)
            {
                Handles.color = color.Value;
            }

            Handles.ArrowHandleCap(0, startPosition, rotation, size, EventType.Repaint);

            Handles.color = previousColor;
        }
    }
}
#endif