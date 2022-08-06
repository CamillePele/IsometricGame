using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Sources.Scripts.Companion.UI
{
    public class SpellToggle : Toggle
    {

        public GenericButton genericButton;
        
        public bool IsHovered
        {
            get => genericButton.IsHovered;
            set
            {
                if (genericButton.IsPressed) return;
                genericButton.IsHovered = value;
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            onValueChanged.AddListener(UpdateButtonState);
        }
        
        public void UpdateButtonState()
        {
            UpdateButtonState(isOn);
        }
        
        public void UpdateButtonState(bool state)
        {
            if (state)
            {
                genericButton.IsHovered = false;
            }
            genericButton.IsPressed = state;
        }

        #region Interface Implementations

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            
            IsHovered = true;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            
            IsHovered = false;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            
            IsHovered = true;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            
            IsHovered = false;
        }

        #endregion
    }
}