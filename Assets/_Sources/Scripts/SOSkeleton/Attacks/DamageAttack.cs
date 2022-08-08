using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SOSkeleton
{
    [CreateAssetMenu(fileName = "AttackData", order = 1)]
    public class DamageAttack : AttackData
    {
        public override void ApplyAttack(Entity owner, List<Tuple<Vector2Int, int>> cellToAttack)
        {
            Debug.Log("DamageAttack");
            Manager.Game gameManager = Manager.Game.Instance;
            
            float ownerDamage = IsSpecial ? owner.gameStats.attackSpe : owner.gameStats.attack;
            float ownerModifierDamage = IsSpecial ? owner.modifiersStats.attackSpe : owner.modifiersStats.attack;
            
            float tempDamage = Damage;
            tempDamage += ownerDamage + ownerModifierDamage;

            int finalDamage = (int)tempDamage;
            
            foreach (Tuple<Vector2Int, int> cellData in cellToAttack)
            {
                Entity entity = gameManager.GetEntityAt(cellData.Item1);
                if (entity != null)
                {
                    //                         Multiply damage by cell value in percent
                    Debug.Log(finalDamage +" * "+ cellData.Item2);
                    entity.TakeDamage(finalDamage * (cellData.Item2 / 100));
                }
            }
        }
    }
}