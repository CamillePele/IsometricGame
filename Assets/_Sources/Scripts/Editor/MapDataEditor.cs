using System.Collections.Generic;
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
            SOSkeleton.MapData map = (SOSkeleton.MapData)target;

            EditorGUILayout.Space();

            showLayout = EditorGUILayout.Foldout(showLayout, "Map ("+map.Size+")");
            if (showLayout) {
                EditorGUI.indentLevel = 0;

                #region Styles

                GUIStyle tableStyle = new GUIStyle ("box"); // General table style
                tableStyle.padding = new RectOffset (10, 10, 10, 10);
                tableStyle.margin.left = 32;
                
                GUIStyle headerColumnStyle = new GUIStyle(); // Header column style
                headerColumnStyle.fixedWidth = 35;

                GUIStyle columnStyle = new GUIStyle(); // Style for columns
                columnStyle.fixedWidth = 40;

                GUIStyle rowStyle = new GUIStyle(); // Style for rows
                rowStyle.fixedHeight = 40;

                GUIStyle rowHeaderStyle = new GUIStyle(); // Style for row headers indecators
                rowHeaderStyle.fixedWidth = columnStyle.fixedWidth - 1;

                GUIStyle columnHeaderStyle = new GUIStyle(); // Style for column headers indecators
                columnHeaderStyle.fixedWidth = 40;
                columnHeaderStyle.fixedHeight = 40;

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
                
                EditorGUILayout.BeginHorizontal(tableStyle);
                for (int x = -1; x < map.Size; x++) {
                    EditorGUILayout.BeginVertical ((x == -1) ? headerColumnStyle : columnStyle);
                    
                    if (x >= 0 && map.mapLayout.Count <= x+1) {
                        map.mapLayout.Add(new List<SOSkeleton.MapData.MapCell>());
                    }
                    
                    for (int y = -1; y < map.Size; y++)
                    {
                        if (y == -1 && x == -1) {
                            EditorGUILayout.BeginHorizontal(rowHeaderStyle);
                            EditorGUILayout.LabelField("[X,Y]", cornerLabelStyle);
                            EditorGUILayout.EndHorizontal();
                        } else if (x == -1) {
                            EditorGUILayout.BeginVertical(columnHeaderStyle);
                            EditorGUILayout.LabelField((y - map.Maximum).ToString(), rowLabelStyle);
                            EditorGUILayout.EndHorizontal();
                        } else if (y == -1) {
                            EditorGUILayout.BeginVertical(rowHeaderStyle);
                            EditorGUILayout.LabelField((x - map.Maximum).ToString(), columnLabelStyle);
                            EditorGUILayout.EndHorizontal();
                        }

                        if (y >= 0 && x >= 0 && map.mapLayout[x].Count <= y+1) {
                            map.mapLayout[x].Add(null);
                        }
                        
                        if (x >= 0 && y >= 0) {
                            EditorGUILayout.BeginHorizontal(rowStyle);

                            if (map.mapLayout[x][y] != null) {
                                int indexOld = heights.IndexOf(map.mapLayout[x][y].height) + 1;
                                
                                int index = EditorGUILayout.Popup(
                                    indexOld,
                                    heightStrings.ToArray(),
                                    popupStyle);
                            
                                if (index == 0) {
                                    map.mapLayout[x][y] = null;
                                } else if (index-1 > 0) {
                                    map.mapLayout[x][y].height = heights[index-1];
                                }
                            }
                            else {
                                bool empty = EditorGUILayout.Toggle(false, toggleStyle);
                                if (empty) {
                                    map.mapLayout[x][y] = new SOSkeleton.MapData.MapCell(_heightDefault);
                                }
                            }
                            
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}