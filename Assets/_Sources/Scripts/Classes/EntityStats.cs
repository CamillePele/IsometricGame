using System;
using UnityEngine;

// TODO: To refactor this class.
namespace Classes
{
    [Serializable]
    public class EntityStats
    {
        [SerializeField] private int statMax = 100;
        [SerializeField] private int statMin = 0;

        [SerializeField] private int _health;
        public int health
        {
            get => Mathf.Clamp(_health, statMin, statMax);
            set
            {
                _health = Mathf.Clamp(value, statMin, statMax);
            }
        }

        [SerializeField] private int _movementPoint;
        public int movementPoint
        {
            get => _movementPoint;
            set
            {
                _movementPoint = Mathf.Clamp(value, statMin, statMax);
            }
        }
        
        [SerializeField] private int _actionPoint;
        public int actionPoint
        {
            get => _actionPoint;
            set
            {
                _actionPoint = Mathf.Clamp(value, statMin, statMax);
            }
        }

        [SerializeField] private int _attack;
        public int attack
        {
            get => Mathf.Clamp(_attack, statMin, statMax);
            set
            {
                _attack = Mathf.Clamp(value, statMin, statMax);
            }
        }

        [SerializeField] private int _speed;
        public int speed
        {
            get => Mathf.Clamp(_speed, statMin, statMax);
            set
            {
                _speed = Mathf.Clamp(value, statMin, statMax);
            }
        }
        
        [SerializeField] private int _defense;
        public int defense
        {
            get => Mathf.Clamp(_defense, statMin, statMax);
            set
            {
                _defense = Mathf.Clamp(value, statMin, statMax);
            }
        }

        [SerializeField] private int _attackSpe;
        public int attackSpe
        {
            get => Mathf.Clamp(_attackSpe, statMin, statMax);
            set
            {
                _attackSpe = Mathf.Clamp(value, statMin, statMax);
            }
        }
        
        [SerializeField] private int _defenseSpe;
        public int defenseSpe
        {
            get => Mathf.Clamp(_defenseSpe, statMin, statMax);
            set
            {
                _defenseSpe = Mathf.Clamp(value, statMin, statMax);
            }
        }
        
        public EntityStats(int health, int movementPoint, int actionPoint, int attack, int speed, int defense, int attackSpe, int defenseSpe)
        {
            // Ensure that all stats are in range
            this.health = health;
            this.movementPoint = movementPoint;
            this.actionPoint = actionPoint;
            this.attack = attack;
            this.speed = speed;
            this.defense = defense;
            this.attackSpe = attackSpe;
            this.defenseSpe = defenseSpe;
        }
        
        public EntityStats()
        {
            this.health = 0;
            this.movementPoint = 0;
            this.actionPoint = 0;
            this.attack = 0;
            this.speed = 0;
            this.defense = 0;
            this.attackSpe = 0;
            this.defenseSpe = 0;
        }
        
        public EntityStats Clone()
        {
            return new EntityStats(health, movementPoint, actionPoint, attack, speed, defense, attackSpe, defenseSpe);
        }
    }
}