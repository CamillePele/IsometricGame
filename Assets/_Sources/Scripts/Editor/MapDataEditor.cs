using System.Collections.Generic;
using SOSkeleton;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SOSkeleton.MapData))]
    public class MapDataEditor : UnityEditor.Editor
    {
        public bool showLayout = true;

        private float _heightGap = 0.25f;
        private float _heightRangeMin = 0f;
        private float _heightRangeMax = 1f;
        private float _heightDefault = 1f;
        
        
        public override void OnInspectorGUI() {
            // base.OnInspectorGUI();
            
            SOSkeleton.MapData map = (SOSkeleton.MapData)target;

            EditorGUILayout.Space();

            showLayout = EditorGUILayout.Foldout(showLayout, "Map ("+map.Size+")");
            if (showLayout) {
                EditorGUI.indentLevel = 0;
                
                if (map.layout == null) {
                    map.layout = new List<SOSkeleton.MapData.Column>();
                    for (int x = 0; x < map.Size; x++)
                    {
                        map.layout.Add(new SOSkeleton.MapData.Column());
                        for (int y = 0; y < map.Size; y++)
                        {
                            map.layout[x].cells.Add(true);
                        }
                    }
                }
                
                if (map.layoutEditor == null)
                {
                    map.layoutEditor = new List<List<bool>>();
                    for (int x = 0; x < map.layout.Count; x++)
                    {
                        map.layoutEditor.Add(new List<bool>());
                        for (int y = 0; y < map.layout[x].cells.Count; y++)
                        {
                            map.layoutEditor[x].Add(map.layout[x].cells[y]);
                        }
                    }
                }
                
                #region Styles

                GUIStyle tableStyle = new GUIStyle ("box"); // General table style
                tableStyle.padding = new RectOffset (10, 10, 10, 10);
                tableStyle.margin.left = 32;
                
                GUIStyle headerColumnStyle = new GUIStyle(); // Header column style
                headerColumnStyle.fixedWidth = 35;

                GUIStyle columnStyle = new GUIStyle(); // Style for columns
                columnStyle.fixedWidth = 20;

                GUIStyle rowStyle = new GUIStyle(); // Style for rows
                rowStyle.fixedHeight = 20;

                GUIStyle rowHeaderStyle = new GUIStyle(); // Style for row headers indecators
                rowHeaderStyle.fixedWidth = columnStyle.fixedWidth - 1;

                GUIStyle columnHeaderStyle = new GUIStyle(); // Style for column headers indecators
                columnHeaderStyle.fixedWidth = 20;
                columnHeaderStyle.fixedHeight = 20;

                GUIStyle columnLabelStyle = new GUIStyle(); // Style for column labels
                columnLabelStyle.fixedWidth = rowHeaderStyle.fixedWidth - 6;
                columnLabelStyle.alignment = TextAnchor.MiddleCenter;
                columnLabelStyle.fontStyle = FontStyle.Bold;
                columnLabelStyle.normal.textColor = Color.white;

                GUIStyle cornerLabelStyle = new GUIStyle(); // Style for corner labels
                cornerLabelStyle.fixedWidth = 42;
                cornerLabelStyle.alignment = TextAnchor.MiddleCenter;
                cornerLabelStyle.fontStyle = FontStyle.BoldAndItalic;
                cornerLabelStyle.fontSize = 9;
                cornerLabelStyle.normal.textColor = Color.white;
                cornerLabelStyle.padding.top = -5;

                GUIStyle rowLabelStyle = new GUIStyle(); // Style for row labels
                rowLabelStyle.fixedWidth = 25;
                rowLabelStyle.alignment = TextAnchor.MiddleRight;
                rowLabelStyle.fontStyle = FontStyle.Bold;
                rowLabelStyle.normal.textColor = Color.white;
                
                GUIStyle popupStyle = new GUIStyle("Popup"); // Popup style
                popupStyle.fontSize = 9;
                popupStyle.fixedWidth = columnStyle.fixedWidth - 5;
                popupStyle.alignment = TextAnchor.MiddleCenter;
                // TODO: center the popup in the column
                
                GUIStyle toggleStyle = new GUIStyle("Toggle"); // Toggle style
                toggleStyle.alignment = TextAnchor.MiddleCenter;
                // TODO: center toggle box
                
                #endregion

                List<float> heights = new List<float>();
                for (float i = _heightRangeMin; i < _heightRangeMax + _heightGap; i += _heightGap)
                {
                    heights.Add(i);
                }
                List<string> heightStrings = new List<string>();
                heightStrings.Add("Ø");
                foreach (float height in heights)
                {
                    heightStrings.Add(height.ToString());
                }
                
                if (map.layoutEditor == null) map.layoutEditor = new List<List<bool>>();

                EditorGUILayout.BeginHorizontal(tableStyle);
                for (int x = -1; x < map.Size; x++) {
                    EditorGUILayout.BeginVertical ((x == -1) ? headerColumnStyle : columnStyle);

                    for (int y = -1; y < map.Size; y++)
                    {
                        if (y == -1 && x == -1) {
                            EditorGUILayout.BeginHorizontal(rowHeaderStyle);
                            EditorGUILayout.LabelField("[X,Y]", cornerLabelStyle);
                            EditorGUILayout.EndHorizontal();
                        } else if (x == -1) {
                            EditorGUILayout.BeginVertical(columnHeaderStyle);
                            EditorGUILayout.LabelField((map.Maximum - y).ToString(), rowLabelStyle);
                            EditorGUILayout.EndHorizontal();
                        } else if (y == -1) {
                            EditorGUILayout.BeginVertical(rowHeaderStyle);
                            EditorGUILayout.LabelField((x - map.Maximum).ToString(), columnLabelStyle);
                            EditorGUILayout.EndHorizontal();
                        }

                        if (x >= 0 && y >= 0) {
                            EditorGUILayout.BeginHorizontal(rowStyle);

                            int yPos = (map.Size-1) - y; // Invert y axis
                            
                            bool oldValue = map.layoutEditor[x][yPos];
                            map.layoutEditor[x][yPos] = EditorGUILayout.Toggle(oldValue, toggleStyle);
                            if (oldValue != map.layoutEditor[x][yPos])
                            {
                                map.layout[x].cells[yPos] = map.layoutEditor[x][yPos];
                                EditorUtility.SetDirty(map);
                            }
                            
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Sync"))
            {
                Save(map);
            }
        }

        public void Save(MapData map)
        {
            map.layout = new List<MapData.Column>();
            foreach (List<bool> column in map.layoutEditor)
            {
                map.layout.Add(new MapData.Column());
                foreach (bool cell in column)
                {
                    map.layout[map.layout.Count - 1].cells.Add(cell);
                }
            }
        }
    }
}