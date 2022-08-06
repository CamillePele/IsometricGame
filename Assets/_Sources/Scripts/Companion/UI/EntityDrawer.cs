using System.Collections;
using System.Collections.Generic;
using Companion.Cell;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Companion.UI
{
    public class EntityDrawer : Drawer
    {
        public Entity entity;
        
        [Header("Entity Info")]
        [SerializeField] private Image _entityThumbnail;
        [SerializeField] private TextMeshProUGUI _entityName;
        [SerializeField] private TextMeshProUGUI _entityLevel;
        [SerializeField] private TextMeshProUGUI _hpText; // Health points
        [SerializeField] private TextMeshProUGUI _mpText; // Movement points
        [SerializeField] private TextMeshProUGUI _apText; // Action points
        
        [Header("Buttons")]
        [SerializeField] private List<Toggle> _spellToggles = new List<Toggle>();
        [SerializeField] private Toggle _movementToggle;
        [SerializeField] private Button _backpackButton; // TODO: to be determined
         
        public void SetEntity(Entity entity)
        {
            this.entity = entity;
            
            //Infos setup
            _entityThumbnail.sprite = entity.entityData.entityThumbnail;
            _entityName.text = entity.entityName;
            _entityLevel.text = "Level " + entity.entityLevel;
            _hpText.text = "HP: " + entity.gameStats.health;
            _mpText.text = "MP: " + entity.gameStats.movementPoint;
            _apText.text = "AP: " + entity.gameStats.actionPoint;
            
            // Buttons setup
            int currentEntityIndex = Manager.Game.Instance.currentEntityPlaying;

            for (int i = 0; i < _spellToggles.Count; i++)
            {
                int index = i;
                _spellToggles[index].onValueChanged.AddListener((b) =>
                {
                    if (!b) return;
                    
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