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
        

        public string displayName;

        public List<List<bool>> layoutEditor/* = new List<List<MapCell>>()*/
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
            public List<bool> cells = new List<bool>();
        }
    }
}