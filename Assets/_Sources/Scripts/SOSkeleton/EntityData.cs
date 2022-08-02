using System;
using System.Collections.Generic;
using Classes;
using UnityEngine;

namespace SOSkeleton
{
    [CreateAssetMenu(fileName = "Entity", order = 1)]
    public class EntityData : ScriptableObject
    {
        public EntityStats constStats; // type stats
        
        public int PM;
        public List<AttackData> attacks = new List<AttackData>();
    }
}