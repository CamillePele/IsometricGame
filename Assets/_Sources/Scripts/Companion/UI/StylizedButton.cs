using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace _Sources.Scripts.Companion.UI
{
    public class StylizedButton : MonoBehaviour, 
        IPointerDownHandler, IPointerEnterHandler, 
        IPointerExitHandler, IPointerUpHandler, ISelectHandler, 
        IDeselectHandler, ISubmitHandler
    {

        [SerializeField] private bool _isPressed; public bool IsPressed
        {
            get { return _isPressed; }
            set
            {
                if (value == _isPressed) return; 
                _isPressed = value;
                
                if (_isPressed) SetHeight(_pressedHeight);
                else
                {
                    if (_isHovered) SetHeight(_hoverHeight);
                    else SetHeight(_defaultHeight);
                }
            }
        }
        
        [SerializeField] private bool _isHovered; public bool IsHovered
        {
            get { return _isHovered; }
            set
            {
                if (value == _isHovered) return; 
                _isHovered = value;
                
                if (_isHovered) SetHeight(_hoverHeight);
                else SetHeight(_defaultHeight);
            }
        }
        
        [Header("Stylized Button")]
        [SerializeField] private RectTransform _childRectTransform;
        [SerializeField] private Shadow _shadow;
        [SerializeField] private TextMeshProUGUI _text;
        
        [Header("Parameters")]
        [SerializeField] private float _hoverHeight = 5;
        [SerializeField] private float _pressedHeight = 0;
        [SerializeField] private float _defaultHeight = 3.2f;

        private float _height;
        private Sequence _sequence1;

        private void Awake()
        {
            _sequence1 = DOTween.Sequence();
        }

        private void Start()
        {
            SetHeight(_defaultHeight, false);
        }

        public void SetHeight(float height, bool animate = true)
        {
            // StartCoroutine(UpdateHeight(height));
            
            if (animate)
            {
                _sequence1.Kill();
                _sequence1 = DOTween.Sequence();

                _sequence1.Insert(0, DOTween.To(() => _height, y =>
                {
                    _height = y;

                    Vector3 newShadowOffset = _shadow.effectDistance;
                    newShadowOffset.y = -y;
                    _shadow.effectDistance = newShadowOffset;

                    Vector3 targetLocalPosition = _childRectTransform.localPosition;
                    targetLocalPosition.y = y;
                    _childRectTransform.localPosition = targetLocalPosition;
                    
                    targetLocalPosition = _text.transform.localPosition;
                    targetLocalPosition.y = y;
                    _text.transform.localPosition = targetLocalPosition;
                }, height, 0.1f));
                
                _sequence1.SetEase(Ease.OutSine);
                _sequence1.Play();
            }
            else
            {
                _height = height;
                
                Vector3 targetLocalPosition = _childRectTransform.localPosition;
                targetLocalPosition.y = height;
                _childRectTransform.localPosition = targetLocalPosition;
                
                Vector3 newShadowOffset = _shadow.effectDistance;
                newShadowOffset.y = -height;
                _shadow.effectDistance = newShadowOffset;
                
                targetLocalPosition = _text.transform.localPosition;
                targetLocalPosition.y = height;
                _text.transform.localPosition = targetLocalPosition;
            }
        }

        #region Events
        
        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            IsHovered = true;
        }
        
        public void OnDeselect(BaseEventData eventData)
        {
            IsHovered = false;
        }
        
        public void OnSubmit(BaseEventData eventData)
        {
            IsHovered = false;
        }
        
        #endregion
        
    }
}