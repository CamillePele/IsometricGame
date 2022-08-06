using System.Collections;
using System.Collections.Generic;
using Companion.Cell;
using UnityEngine;
using UnityEngine.UI;

namespace Companion.UI
{
    public class EntityDrawer : Drawer
    {
        public Entity entity;
        
        [SerializeField] private List<Toggle> _spellToggles = new List<Toggle>();
        [SerializeField] private Toggle _movementToggle;
        [SerializeField] private Button _backpackButton; // TODO: to be determined
         
        public void SetEntity(Entity entity)
        {
            this.entity = entity;
            
            int currentEntityIndex = Manager.Game.Instance.currentEntityPlaying;

            for (int i = 0; i < _spellToggles.Count; i++)
            {
                int index = i;
                _spellToggles[index].onValueChanged.AddListener((b) =>
                {
                    if (!b) return;
                    
                    print("Spell " + index + " clicked");
                    Manager.Game.Instance.entities[currentEntityIndex].AttackIndex = index + 1; // Because index 0 is the movement button
                });
            }
            
            _movementToggle.onValueChanged.AddListener((b) =>
            {
                if (!b) return;
                
                Manager.Game.Instance.entities[currentEntityIndex].AttackIndex = 0;
            });
        }
    }
}