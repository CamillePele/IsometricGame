using System.Collections.Generic;
using UnityEngine;

namespace SOSkeleton
{
    [CreateAssetMenu(fileName = "MapData", order = 1)]
    public class MapData : ScriptableObject
    {
        public int Size { get => Manager.Grid.GridSize; }
        public int Maximum { get => Manager.Grid.Maximum; }

        public class MapCell
        {
            public float height;
            
            public MapCell(float height)
            {
                this.height = height;
            }
        }

        public string mapName;

        public List<List<MapCell>> mapLayout = new List<List<MapCell>>();
        
    }
}