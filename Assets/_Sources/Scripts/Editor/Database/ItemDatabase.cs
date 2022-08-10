using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scripts.Misc;
using SOSkeleton;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Manager
{
    public class ItemDatabase : EditorWindow
    {

        [SerializeField] private VisualTreeAsset _rootVisualTreeAsset;
        [SerializeField] private StyleSheet _mainStyleSheet;
        
        [SerializeField] private VisualTreeAsset _itemRowTemplate;
        [SerializeField] private VisualTreeAsset _typeColumnTemplate;
        
        [SerializeField] private Sprite _defaultItemIcon;
        
        
        private static Dictionary<Type, List<EditableObject>> m_ItemDatabase = new Dictionary<Type, List<EditableObject>>();
        private List<Type> m_ItemTypes = new List<Type>();
        
        private VisualElement m_ItemsTab;
        private ListView m_ItemListView;
        private float m_ItemHeight = 40;
        
        private ScrollView m_DetailSection;
        private VisualElement m_LargeDisplayIcon;
        private EditableObject m_activeItem;
        
        private VisualElement m_TypesTab;
        private ScrollView m_TypeScrollView;
        private Type m_activeItemType;

        private VisualElement m_DeletePopup;

        [MenuItem("Iso Game/Prefab Manager")]
        public static void Init()
        {
            ItemDatabase wnd = GetWindow<ItemDatabase>();
            wnd.titleContent = new GUIContent("Item Database");
            Vector2 size = new Vector2(800, 475);
            wnd.minSize = size;
            wnd.maxSize = size;

        }

        public void CreateGUI()
        {
            VisualElement rootFromUXML = _rootVisualTreeAsset.Instantiate();
            rootVisualElement.Add(rootFromUXML);
            
            rootVisualElement.styleSheets.Add(_mainStyleSheet);
            
            _defaultItemIcon = (Sprite) AssetDatabase.LoadAssetAtPath(
                "Assets/_Sources/UI/UnknownIcon.png", typeof(Sprite)); // TODO: Change to relative path
            
            LoadAllItems();
            
            m_ItemsTab = rootVisualElement.Q<VisualElement>("ItemsTab");
            m_ItemsTab.style.visibility = Visibility.Hidden;
            
            m_TypesTab = rootVisualElement.Q<VisualElement>("TypesTab");
            GenerateTypeView();
            
            m_DetailSection = rootVisualElement.Q<ScrollView>("ScrollView_Details");
            m_DetailSection.style.visibility = Visibility.Hidden;

            rootVisualElement.Q<Button>("Btn_AddItem").clicked += AddItem_OnClick;

            m_DetailSection.Q<TextField>("ItemName")
                .RegisterValueChangedCallback(evt =>
                {
                    m_activeItem.FriendlyName = evt.newValue;
                    m_ItemListView.Refresh();
                });
            
            m_DetailSection.Q<ObjectField>("IconPicker")
                .RegisterValueChangedCallback(evt =>
                {
                    Sprite newSprite = evt.newValue as Sprite;
                    m_activeItem.Icon = newSprite == null ? _defaultItemIcon : newSprite;
                    
                    m_LargeDisplayIcon.style.backgroundImage = newSprite == null ? 
                            _defaultItemIcon.texture : newSprite.texture;
                    m_ItemListView.Refresh();
                });


            m_DeletePopup = rootVisualElement.Q<VisualElement>("DeletePopup");
            m_DeletePopup.style.visibility = Visibility.Hidden;

            rootVisualElement.Q<Button>("Btn_DeleteItem").clicked += () =>
            {
                m_DeletePopup.style.visibility = Visibility.Visible;
                
                rootVisualElement.Q<Button>("NoButton").clicked += () =>
                {
                    m_DeletePopup.style.visibility = Visibility.Hidden;
                };

                rootVisualElement.Q<Button>("YesButton").clicked += DeleteItem_OnClick;
            };
        }

        private void LoadAllItems()
        {
            m_ItemDatabase.Clear();
            string[] allGuid = AssetDatabase.FindAssets( "t:EditableObject" );

            foreach (string guid in allGuid)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                EditableObject item = AssetDatabase.LoadAssetAtPath<EditableObject>(path);
                Type type = item.GetType();

                if (!m_ItemDatabase.ContainsKey(type))
                {
                    m_ItemDatabase.Add(type, new List<EditableObject>());
                    m_ItemTypes.Add(type);
                }

                m_ItemDatabase[type].Add(item);
            }
        }
        
        private void GenerateTypeView()
        {
            m_TypeScrollView = m_TypesTab.Q<ScrollView>("TypeListView");
            
            for (int i = 0; i < m_ItemTypes.Count; i++)
            {
                VisualElement typeColumn = _typeColumnTemplate.CloneTree();
                
                Button typeButton = typeColumn.Q<Button>("Button");
                typeButton.text = m_ItemTypes[i].Name;
                
                int index = i;
                typeButton.clicked += () =>
                {
                    TypeView_onSelectionChange(index);
                };
                
                m_TypeScrollView.Add(typeColumn);
            }
        }
        
        private void TypeView_onSelectionChange(int selectedIndex)
        {
            if (selectedIndex == m_ItemTypes.IndexOf(m_activeItemType)) return;

            // Destroy the old list view if it exists
            if (m_ItemListView != null)
            {
                m_ItemListView.RemoveFromHierarchy();
                m_ItemListView = null;
            }
            
            m_activeItemType = m_ItemTypes[selectedIndex];
            
            // Remove Content from ScrollView
            VisualElement content = m_DetailSection.Q<VisualElement>("Content");
            if (content != null) content.hierarchy.parent.RemoveFromHierarchy();
            
            var editor = UnityEditor.Editor.CreateEditor(m_ItemDatabase[m_activeItemType][0]);
            var inspector = editor.CreateInspectorGUI();
            m_DetailSection.Add(inspector);
            
            m_LargeDisplayIcon = m_DetailSection.Q<VisualElement>("Icon");

            GenerateListView();
            m_ItemsTab.style.visibility = Visibility.Visible;
            m_DetailSection.style.visibility = Visibility.Hidden;
        }
        
        private void GenerateListView()
        {
            Func<VisualElement> makeItem = () => _itemRowTemplate.CloneTree();
    
            Action<VisualElement, int> bindItem = (e, i) =>
            {
                e.Q<VisualElement>("Icon").style.backgroundImage = 
                    m_ItemDatabase[m_activeItemType][i].Icon == null ?
                    _defaultItemIcon.texture : m_ItemDatabase[m_activeItemType][i].Icon.texture;
                
                e.Q<Label>("Name").text = m_ItemDatabase[m_activeItemType][i].FriendlyName;
            };
            
            m_ItemListView = new ListView(m_ItemDatabase[m_activeItemType], 35, makeItem, bindItem);
            m_ItemListView.selectionType = SelectionType.Single;
            m_ItemListView.style.height = m_ItemDatabase[m_activeItemType].Count * m_ItemHeight;
            m_ItemsTab.Add(m_ItemListView);
            
            m_ItemListView.onSelectionChange += ListView_onSelectionChange;
        }
        
        private void ListView_onSelectionChange(IEnumerable<object> selectedItems)
        {
            m_activeItem = (EditableObject)selectedItems.First();
            SerializedObject so = new SerializedObject(m_activeItem);
            m_DetailSection.Bind(so);
            if (m_activeItem.Icon != null)
            {
                m_DetailSection.Q<ObjectField>("IconPicker")
                    .RegisterValueChangedCallback(evt =>
                    {
                        Sprite newSprite = evt.newValue as Sprite;
                        m_activeItem.Icon = newSprite == null ? _defaultItemIcon : newSprite;
                        m_LargeDisplayIcon.style.backgroundImage = newSprite == null ? _defaultItemIcon.texture : newSprite.texture;
                        m_ItemListView.Refresh();
                    });
                
                m_DetailSection.Q<TextField>("ItemName")
                    .RegisterValueChangedCallback(evt => 
                    {
                        m_activeItem.FriendlyName = evt.newValue;
                        m_ItemListView.Refresh();
                    });
            }
            
            m_DetailSection.style.visibility = Visibility.Visible;
        }
        
        
        private void AddItem_OnClick()
        {
            //Create an instance of the scriptable object and set the default parameters
            EditableObject newItem = CreateInstance<EditableObject>();
            newItem.FriendlyName = $"New Item";
            newItem.Icon = _defaultItemIcon;
            //Create the asset, using the unique ID for the name
            AssetDatabase.CreateAsset(newItem, $"Assets/_Sources/{newItem.ID}.asset");
            //Add it to the item list
            m_ItemDatabase[m_activeItemType].Add(newItem);
            //Refresh the ListView so everything is redrawn again
            m_ItemListView.Refresh();
            m_ItemListView.style.height = m_ItemDatabase.Count * m_ItemHeight;
        }
        
        private void DeleteItem_OnClick()
        {
            //Get the path of the file and delete it through AssetDatabase
            string path = AssetDatabase.GetAssetPath(m_activeItem);
            AssetDatabase.DeleteAsset(path);
            //Purge the reference from the list and refresh the ListView
            m_ItemDatabase[m_activeItemType].Remove(m_activeItem);
            m_ItemListView.Refresh();
            //Nothing is selected, so hide the details section
            m_DetailSection.style.visibility = Visibility.Hidden;
            m_DeletePopup.style.visibility = Visibility.Hidden;
        }
    }
}