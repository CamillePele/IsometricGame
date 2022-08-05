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
        
        [SerializeField] private List<Button> _spellButtons = new List<Button>();
        [SerializeField] private Button _movementButton;
        [SerializeField] private Button _backpackButton; // TODO: to be determined
         
        public void SetEntity(Entity entity)
        {
            this.entity = entity;
            
            int currentEntityIndex = Manager.Game.Instance.currentEntityPlaying;

            for (int i = 0; i < _spellButtons.Count; i++)
            {
                int index = i;
                _spellButtons[index].onClick.AddListener(() =>
                {
                    print("Spell " + index + " clicked");
                    Manager.Game.Instance.entities[currentEntityIndex].AttackIndex = index + 1; // Because index 0 is the movement button
                });
            }
            
            _movementButton.onClick.AddListener(() =>
            {
                Manager.Game.Instance.entities[currentEntityIndex].AttackIndex = 0;
            });
        }
    }
}