using System.Collections.Generic;
using UnityEngine;

namespace SOSkeleton
{
    [CreateAssetMenu(fileName = "Entity", order = 1)]
    public class EntityData : ScriptableObject
    {
        public int PM;
        public List<AttackData> attacks = new List<AttackData>();
    }
}