using UnityEngine;

namespace BadgerHTN
{
    [CreateAssetMenu(menuName = "BadgerHTN/Tree")]
    public class BadgerTree : GraphAsset
    {
        public void OnValidate()
        {
            GenerateGuid();
        }

        public override void OnDataChange()
        {
            // Save every time that the data was changed.
            base.OnDataChange();
            Save();
        }

        private void Save()
        {
#if UNITY_EDITOR
            // Save editor data.
            bool didChange = SaveService.Save(this, Data);
            if (didChange)
            {
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
            }
#endif
        }

        private void GenerateGuid()
        {
#if UNITY_EDITOR
            // Set the guid by checking the metafile data.
            var path = UnityEditor.AssetDatabase.GetAssetPath(this);
            var guid = UnityEditor.AssetDatabase.GUIDFromAssetPath(path);
            
            Guid = guid.ToString();
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}