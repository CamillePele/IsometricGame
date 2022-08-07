using System;
using System.Collections.Generic;
using Classes;
using Scripts.Misc;
using UnityEngine;

namespace SOSkeleton
{
    [CreateAssetMenu(fileName = "Entity", order = 1)]
    public class EntityData : EditableObject
    {
        public override string VisualTreeAsset { get => "Assets/_Sources/UI/Editor/ItemDatabase/EntityContent.uxml"; }

        // [SerializeField] public string entityName;
        // public string entityDescription;
        
        public EntityStats constStats; // type stats
        
        public EntityType TypeOne;
        public EntityType TypeTwo;
        
        public int PM;
        public List<AttackData> AvailableAttacks = new List<AttackData>();
        // public Dictionary<int, AttackData> attackDictionary = new Dictionary<int, AttackData>(); // <level, attackLearned>
    }
}

public enum EntityType // TODO: temporary
{
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
}