using BadgerEditor.Graphs;
using BadgerEditor.Planning;
using UnityEditor;
using UnityEngine;

namespace BadgerHTN.Editor.Agents
{
    [CustomEditor(typeof(AgentComponent<>), true), CanEditMultipleObjects]
    public class AgentComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var instance = (IAgentComponent)target;

            if (GUILayout.Button("Tick"))
            {
                instance.Tick();
            }

            if (GUILayout.Button("Open Tree"))
            {
                var asset = instance.Asset;
                if (asset != null)
                {
                    GraphEditorWindow.OpenWindow(asset);
                }
            }

            if (GUILayout.Button("Open Plan"))
            {
                PlanWindow.OpenWindow();
            }
        }
    }
}