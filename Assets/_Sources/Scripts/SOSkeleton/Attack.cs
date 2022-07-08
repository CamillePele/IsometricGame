using System.Collections.Generic;
using Array2DEditor;
using UnityEngine;

namespace SOSkeleton
{
    [CreateAssetMenu(fileName = "Attack", order = 1)]
    public class Attack : ScriptableObject
    {
        public string attackName;
        public int attackDamage;
        public int attackCost;

        [SerializeField] public Array2DFloat _attackPatternArray;

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
    }
}