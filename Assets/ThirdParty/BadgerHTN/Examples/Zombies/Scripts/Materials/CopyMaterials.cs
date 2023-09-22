using UnityEditor;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Materials
{
    [ExecuteInEditMode]
    public class CopyMaterials : MonoBehaviour
    {
        public Material material;

        private void OnValidate()
        {
#if UNITY_EDITOR
            var renderers = GetComponentsInChildren<MeshRenderer>(true);
            foreach (var r in renderers)
            {
                r.sharedMaterial = material;
            }

            EditorUtility.SetDirty(this);
#endif
        }
    }
}