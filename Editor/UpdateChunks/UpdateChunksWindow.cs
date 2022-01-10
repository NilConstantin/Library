using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;


namespace Library.Editor
{
    public class UpdateChunksWindow : EditorWindow
    {
        public GameObject[] BrokenRoots;
        public Material ChunkMaterial = default;


        [MenuItem("Window/Update Chunks Window")]
        public static void OpenWindow()
        {
            var window = GetWindow<UpdateChunksWindow>();
            window.titleContent = new GUIContent("Update Chunks");
        }


        private void OnGUI()
        {
            ScriptableObject target = this;
            SerializedObject so = new SerializedObject(target);

            SerializedProperty chunkMaterialProperty = so.FindProperty("ChunkMaterial");
            EditorGUILayout.PropertyField(chunkMaterialProperty); // True means show children            

            SerializedProperty brokenRootsProperty = so.FindProperty("BrokenRoots");
            EditorGUILayout.PropertyField(brokenRootsProperty, true); // True means show children

            so.ApplyModifiedProperties(); // Remember to apply modified properties

            if (GUILayout.Button("Update"))
            {
                foreach (var brokenRoot in BrokenRoots)
                {
                    UpdateChunksMeshesAndColliders(brokenRoot, ChunkMaterial);
                }
            }
            
            if (GUILayout.Button("Update Colliders"))
            {
                foreach (var brokenRoot in BrokenRoots)
                {
                    UpdateChunksColliders(brokenRoot);
                }
            }
            
            if (GUILayout.Button("Disable shadows"))
            {
                foreach (var brokenRoot in BrokenRoots)
                {
                    DisableShadows(brokenRoot);
                }
            }
        }


        private void UpdateChunksMeshesAndColliders(GameObject brokenRoot, Material chunkMaterial)
        {
            if (brokenRoot)
            {
                var meshFilters = brokenRoot.GetComponentsInChildren<MeshFilter>();
                foreach (var meshFilter in meshFilters)
                {
                    meshFilter.sharedMesh.SetTriangles(meshFilter.sharedMesh.triangles, 0);
                    meshFilter.sharedMesh.subMeshCount = 1;
                }

                var meshRenderers = brokenRoot.GetComponentsInChildren<MeshRenderer>();
                foreach (var meshRenderer in meshRenderers)
                {
                    meshRenderer.sharedMaterials = new[] { chunkMaterial };

                    meshRenderer.lightProbeUsage = LightProbeUsage.Off;
                    meshRenderer.allowOcclusionWhenDynamic = false;
                }

                UpdateChunksColliders(brokenRoot);
            }
            else
            {
                Debug.Log("Not selected!");
            }
        }
        
        
        private void UpdateChunksColliders(GameObject brokenRoot)
        {
            if (brokenRoot)
            {
                var meshColliders = brokenRoot.GetComponentsInChildren<MeshCollider>();
                foreach (var meshCollider in meshColliders)
                {
                    meshCollider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation | 
                                                  MeshColliderCookingOptions.EnableMeshCleaning | 
                                                  MeshColliderCookingOptions.WeldColocatedVertices | 
                                                  MeshColliderCookingOptions.UseFastMidphase;
                }

                EditorUtility.SetDirty(brokenRoot);
            }
            else
            {
                Debug.Log("Not selected!");
            }
        }
        
        
        private void DisableShadows(GameObject brokenRoot)
        {
            if (brokenRoot)
            {
                var meshRenderers = brokenRoot.GetComponentsInChildren<MeshRenderer>();
                foreach (var meshRenderer in meshRenderers)
                {
                    meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                }

                EditorUtility.SetDirty(brokenRoot);
            }
            else
            {
                Debug.Log("Not selected!");
            }
        }
    }
}