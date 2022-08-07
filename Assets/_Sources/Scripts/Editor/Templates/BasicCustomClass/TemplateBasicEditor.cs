using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Templates.BasicCustomClass
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TemplateClass))]
    public class TemplateBasicEditor : UnityEditor.Editor
    {
        private TemplateClass _castedTarget => (TemplateClass) target;
        private IEnumerable<TemplateClass> _targets => targets.Cast<TemplateClass>();

        private VisualElement _root;
        
        private TextField _nameField;
        private IntegerField _healthField;
        
        private SerializedProperty _propertyName;
        private SerializedProperty _propertyHealth;

        public override VisualElement CreateInspectorGUI()
        {
            FindProperties();
            InitializeProperties();
            Compose();
            
            return _root;
        }

        private void FindProperties()
        {
            _propertyName = serializedObject.FindProperty(nameof(TemplateClass.className)); // Can do this to be sure that the property is found
            _propertyHealth = serializedObject.FindProperty("_health"); // Unfortuantely, this is not possible to do with the provate field
        }
        
        private void InitializeProperties()
        {
            _root = new VisualElement();
            _root.style.flexDirection = FlexDirection.Column;
            
            _nameField = new TextField();
            _nameField.BindProperty(_propertyName); // Binds the property to the text field
            _nameField.style.flexGrow = 1;
            _nameField.tooltip = "Enter the name of the class";
            
            _healthField = new IntegerField();
            _healthField.BindProperty(_propertyHealth); // Binds the property to the text field
            _healthField.style.flexGrow = 1;
            _healthField.tooltip = "Enter the health of the class";
        }
        
        private void Compose()
        {
            _root.Add(_nameField);
            _root.Add(_healthField);
        }
    }
}