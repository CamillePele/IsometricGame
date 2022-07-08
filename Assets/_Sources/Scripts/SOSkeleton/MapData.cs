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

        [SerializeField] private Array2DBool _layoutArray = new Array2DBool();

        public List<List<bool>> Layout
        {
            get
            {
                return Utils.Maths.Get2DList<bool, bool>(_layoutArray);
            }
        }
    }
}