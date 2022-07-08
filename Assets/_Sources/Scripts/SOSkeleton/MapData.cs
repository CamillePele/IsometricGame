using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;

namespace SOSkeleton
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "MapData", order = 1)]
    public class MapData : ScriptableObject
    {
        public int Size { get => Manager.Grid.GridSize; }
        public int Maximum { get => Manager.Grid.Maximum; }

        [System.Serializable] [DefaultValue(null)]
        public class MapCell
        {
            public float height;
            public bool enabled;
            
            public MapCell(float height)
            {
                this.height = height;
                enabled = false;
            }
        }

        public string displayName;

        public List<List<MapCell>> layoutEditor/* = new List<List<MapCell>>()*/
        /*{
            new List<MapCell>() {null, null, null, null, null, null, null, null, null,},
            new List<MapCell>() {null, null, null, null, null, null, null, null, null,},
            new List<MapCell>() {null, null, null, null, null, null, null, null, null,},
            new List<MapCell>() {null, null, null, null, null, null, null, null, null,},
            new List<MapCell>() {null, null, null, null, null, null, null, null, null,},
            new List<MapCell>() {null, null, null, null, null, null, null, null, null,},
            new List<MapCell>() {null, null, null, null, null, null, null, null, null,},
            new List<MapCell>() {null, null, null, null, null, null, null, null, null,},
            new List<MapCell>() {null, null, null, null, null, null, null, null, null,},
        }*/;

        public List<Column> layout;
        
        [System.Serializable]
        public class Column
        {
            public List<MapCell> cells = new List<MapCell>();
        }
    }
}