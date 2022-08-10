using System;
using System.Collections.Generic;
using Classes;
using Scripts.Misc;
using SOSkeleton;
using UnityEngine;

namespace SOSkeleton
{
    [CreateAssetMenu(fileName = "Entity", order = 1)]
    public class EntityData : EditableObject
    {
        public EntityStats constStats; // type stats
        
        public EntityType TypeOne;
        public EntityType TypeTwo;
        
        public int PM;
        public List<AttackData> AvailableAttacks = new List<AttackData>();
        public List<ValueTest> AvailableAttacksNames = new List<ValueTest>();
        // public Dictionary<int, AttackData> attackDictionary = new Dictionary<int, AttackData>(); // <level, attackLearned>
    }
}

[Serializable]
public struct ValueTest
{
    public string name;
    public bool enabled;
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