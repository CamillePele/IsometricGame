using System;
using System.Collections.Generic;
using Classes;
using UnityEngine;

namespace SOSkeleton
{
    [CreateAssetMenu(fileName = "Entity", order = 1)]
    public class EntityData : ScriptableObject
    {
        [SerializeField] public string entityName;
        public string entityDescription;
        
        public Sprite entityThumbnail;
        public EntityStats constStats; // type stats
        
        public int PM;
        public List<AttackData> availableAttacks = new List<AttackData>();
        public Dictionary<int, AttackData> attackDictionary = new Dictionary<int, AttackData>(); // <level, attackLearned>
        
        
        public string ID = Guid.NewGuid().ToString().ToUpper();
        public string FriendlyName;
        public string Description;
        public Categories Category;
        public bool Stackable;
        public int BuyPrice;
        [Range(0,1)]
        public float SellPercentage;
        public Sprite Icon;
        public enum Categories
        {
            Food,
            Weapon,
            Junk
        }
    }
}