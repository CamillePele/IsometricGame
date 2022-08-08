using System;
using System.Collections.Generic;
using Array2DEditor;
using Scripts.Misc;
using UnityEngine;

namespace SOSkeleton
{
    public abstract class AttackData : EditableObject
    {
        public override string VisualTreeAsset { get => "Assets/_Sources/UI/Editor/ItemDatabase/AttackContent.uxml"; }
        
        public int Damage;
        public int AttackCost;
        public bool IsSpecial;
        public EntityType Type;
        public float RandomRange;
        
        [SerializeField] public Array2DInt _attackPatternArray;
        [SerializeField] public Vector2Int attackPivot;
        [SerializeField] public Array2DBool _attackablePatternArray;

        public List<List<int>> AttackPatternValue
        {
            get
            {
                return Utils.Maths.Get2DList<int, int>(_attackPatternArray);
            }
        }
        
        public List<List<bool>> AttackPattern
        {
            get
            {
                return Utils.Maths.Get2DList<bool, int>(_attackPatternArray, f => f > 0);
            }
        }
        
        public List<List<bool>> AttackablePattern
        {
            get
            {
                return Utils.Maths.Get2DList<bool, bool>(_attackablePatternArray, f => f);
            }
        }

        public abstract void ApplyAttack(Entity owner, List<Tuple<Vector2Int, int>> cellToAttack);
    }
}