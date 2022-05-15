using UnityEditor;


namespace Library.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EcsComponentProvider<>), true)]
    public class EcsComponentProviderInspector : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var isAnyVisible = false;
            var property = serializedObject.GetIterator();
            while (property.NextVisible(true))
            {
                if (property.name.ToLower() != "m_script" && 
                    property.name.ToLower() != "component")
                {
                    isAnyVisible = true;
                    break;
                }
            }

            if (isAnyVisible)
            {
                base.OnInspectorGUI();    
            }
            else
            {
                DrawScriptFieldFromMonoBehaviour();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}