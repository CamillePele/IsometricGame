using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Manager
{
    [CustomEditor(typeof(SOSkeleton.EntityData))]
    public class EntityEditor : DefaultEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();

            return root;
        }
    }
}