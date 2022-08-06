using _Sources.Scripts.Companion.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    [CustomEditor(typeof(SpellToggle))]
    public class SpellToggleEditor : ToggleEditor
    {
        public override void OnInspectorGUI()
        {
            SpellToggle spellToggle = (SpellToggle)target;
            
            base.OnInspectorGUI();
            
            // Now display SpellToggle properties
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("SpellToggle Properties", EditorStyles.boldLabel);
            spellToggle.genericButton = (GenericButton)EditorGUILayout.ObjectField("Generic Button", spellToggle.genericButton, typeof(GenericButton), true);
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(spellToggle);
            }
        }
    }
}