using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SOSkeleton;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Manager
{
    public class ItemDatabase : EditorWindow
    {
        private Sprite m_DefaultItemIcon;
        private static List<Item> m_ItemDatabase = new List<Item>();
        
        private VisualElement m_ItemsTab;
        private static VisualTreeAsset m_ItemRowTemplate;
        private ListView m_ItemListView;
        private float m_ItemHeight = 40;
        
        private ScrollView m_DetailSection;
        private VisualElement m_LargeDisplayIcon;
        private Item m_activeItem;

        private VisualElement m_DeletePopup;

        [MenuItem("WUG/Item Database")]
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
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
                ("Assets/_Sources/Scripts/Editor/Manager/ItemDatabase.uxml"); // TODO: Change to relative path
            VisualElement rootFromUXML = visualTree.Instantiate();
            rootVisualElement.Add(rootFromUXML);
            
            //Import the ListView Item Template
            m_ItemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Sources/Scripts/Editor/Manager/ItemRowTemplate.uxml");
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>
                ("Assets/_Sources/Scripts/Editor/Manager/ItemDatabase.uss"); // TODO: Change to relative path
            rootVisualElement.styleSheets.Add(styleSheet);
            
            m_DefaultItemIcon = (Sprite) AssetDatabase.LoadAssetAtPath(
                "Assets/_Sources/UI/UnknownIcon.png", typeof(Sprite)); // TODO: Change to relative path
            
            LoadAllItems();
            
            m_ItemsTab = rootVisualElement.Q<VisualElement>("ItemsTab");
            GenerateListView();
            
            m_DetailSection = rootVisualElement.Q<ScrollView>("ScrollView_Details");
            m_DetailSection.style.visibility = Visibility.Hidden;
            m_LargeDisplayIcon = m_DetailSection.Q<VisualElement>("Icon");
            
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
                    m_activeItem.Icon = newSprite == null ? m_DefaultItemIcon : newSprite;
                    
                    m_LargeDisplayIcon.style.backgroundImage = newSprite == null ? 
                            m_DefaultItemIcon.texture : newSprite.texture;
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
            string[] allGuid = AssetDatabase.FindAssets( "t:Item" );

            foreach (string guid in allGuid)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                
                m_ItemDatabase.Add(AssetDatabase.LoadAssetAtPath<Item>(path));
            }
            Debug.Log(m_ItemDatabase.Count + " items loaded");
        }
        
        private void GenerateListView()
        {
            Func<VisualElement> makeItem = () => m_ItemRowTemplate.CloneTree();
    
            Action<VisualElement, int> bindItem = (e, i) =>
            {
                e.Q<VisualElement>("Icon").style.backgroundImage = 
                    m_ItemDatabase[i] == null ?
                    m_DefaultItemIcon.texture : m_ItemDatabase[i].Icon.texture;
                
                e.Q<Label>("Name").text = m_ItemDatabase[i].FriendlyName;
            };
            
            m_ItemListView = new ListView(m_ItemDatabase, 35, makeItem, bindItem);
            m_ItemListView.selectionType = SelectionType.Single;
            m_ItemListView.style.height = m_ItemDatabase.Count * m_ItemHeight;
            m_ItemsTab.Add(m_ItemListView);
            
            m_ItemListView.onSelectionChange += ListView_onSelectionChange;
        }
        
        private void ListView_onSelectionChange(IEnumerable<object> selectedItems)
        {
            m_activeItem = (Item)selectedItems.First();
            SerializedObject so = new SerializedObject(m_activeItem);
            m_DetailSection.Bind(so);
            if (m_activeItem.Icon != null)
            {
                m_LargeDisplayIcon.style.backgroundImage = m_activeItem.Icon.texture;
            }
            m_DetailSection.style.visibility = Visibility.Visible;
        }
        
        
        private void AddItem_OnClick()
        {
            //Create an instance of the scriptable object and set the default parameters
            Item newItem = CreateInstance<Item>();
            newItem.FriendlyName = $"New Item";
            newItem.Icon = m_DefaultItemIcon;
            //Create the asset, using the unique ID for the name
            AssetDatabase.CreateAsset(newItem, $"Assets/_Sources/{newItem.ID}.asset");
            //Add it to the item list
            m_ItemDatabase.Add(newItem);
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
            m_ItemDatabase.Remove(m_activeItem);
            m_ItemListView.Refresh();
            //Nothing is selected, so hide the details section
            m_DetailSection.style.visibility = Visibility.Hidden;
            m_DeletePopup.style.visibility = Visibility.Hidden;
        }
    }
}