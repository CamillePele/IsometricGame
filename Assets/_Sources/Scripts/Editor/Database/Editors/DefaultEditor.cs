using Scripts.Misc;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Manager
{
    [CustomEditor(typeof(EditableObject), true)]
    public class DefaultEditor : UnityEditor.Editor
    {
        [field: SerializeField] protected virtual VisualTreeAsset _editorAsset { get; set; }
        [SerializeField] Sprite _defaultIcon;

        public override VisualElement CreateInspectorGUI()
        {
            var root = _editorAsset.CloneTree();
            var obj = target as EditableObject;
            
            SerializedObject so = new SerializedObject(obj);
            root.Bind(so);

            root.Q<ObjectField>("IconPicker")
                .RegisterValueChangedCallback(evt =>
                {
                    Sprite newSprite = evt.newValue as Sprite;
                    Texture newTexture = _defaultIcon.texture;
                    if (newSprite != null)
                    {
                        newTexture = newSprite.texture;
                    }

                    root.Q<VisualElement>("Icon").style.backgroundImage = (StyleBackground) newTexture;
                });

            return root;
        }
    }
}