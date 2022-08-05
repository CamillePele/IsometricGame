using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Companion.Cell
{
    public class Drawer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool IsOpen
        {
            get
            {
                // If _rectTransform.y is greater than the mid of _closedHeight and _openedHeight, then the drawer is open.
                return _rectTransform.anchoredPosition.y > ClosedHeight + (OpenedHeight - ClosedHeight) / 2;
            }
        }
        
        [SerializeField] private DrawerKnob _drawerKnob;
        
        [SerializeField] private Transform _closedPosition;
        [SerializeField] private Transform _openedPosition;
        
        public float ClosedHeight { get => _closedPosition.position.y; }
        public float OpenedHeight { get => _openedPosition.position.y; }
        
        public UnityEvent<bool> StateChanged = new UnityEvent<bool>();

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            
        }

        private void Start()
        {
            SetState(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsOpen)
            {
                return;
            }
            
            Open();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsOpen)
            {
                return;
            }
            
            Close();
        }
        
        public void Open()
        {
            SetState(true);
        }
        
        public void Close()
        {
            SetState(false);
        }
        
        public void Toggle()
        {
            SetState(!IsOpen);
        }

        public void SetState(bool state, bool animate = true)
        {
            if (state == IsOpen) return;

            float targetHeight = state ? OpenedHeight : ClosedHeight;

            if (animate)
            {
                _rectTransform.DOMoveY(targetHeight, 0.2f).SetEase(Ease.OutQuad);
            } else {
                _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, targetHeight);
            }
            
            _drawerKnob.AnimationState(state);
        }
    }
}