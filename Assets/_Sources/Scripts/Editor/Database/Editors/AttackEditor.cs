using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Manager
{
    [CustomEditor(typeof(SOSkeleton.AttackData), true)]
    public class AttackEditor : DefaultEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();

            return root;
        }
    }
}