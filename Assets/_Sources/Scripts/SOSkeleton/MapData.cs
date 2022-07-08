using System.Collections.Generic;
using System.ComponentModel;
using Array2DEditor;
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

        public Array2DBool layout = new Array2DBool();
    }
}