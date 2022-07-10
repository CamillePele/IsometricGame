using System.Collections.Generic;
using Array2DEditor;
using UnityEngine;

namespace SOSkeleton
{
    [CreateAssetMenu(fileName = "AttackData", order = 1)]
    public class AttackData : ScriptableObject
    {
        public string attackName;
        public int attackDamage;
        public int attackCost;

        [SerializeField] public Array2DFloat _attackPatternArray;
        [SerializeField] public Vector2Int attackPivot;
        [SerializeField] public Array2DBool _attackablePatternArray;

        public List<List<float>> AttackPatternValue
        {
            get
            {
                return Utils.Maths.Get2DList<float, float>(_attackPatternArray);
            }
        }
        
        public List<List<bool>> AttackPattern
        {
            get
            {
                return Utils.Maths.Get2DList<bool, float>(_attackPatternArray, f => f > 0);
            }
        }
        
        public List<List<bool>> AttackablePattern
        {
            get
            {
                return Utils.Maths.Get2DList<bool, bool>(_attackablePatternArray, f => f);
            }
        }
    }
}