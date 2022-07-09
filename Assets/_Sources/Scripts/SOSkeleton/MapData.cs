using System.Collections.Generic;
using Array2DEditor;
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

        // -1 = void; 0 = cell; 1 = wall
        public Array2DInt layoutArray = new Array2DInt();

        public List<List<int>> Layout
        {
            get
            {
                return Utils.Maths.Get2DList<int, int>(layoutArray);
            }
        }
    }
}