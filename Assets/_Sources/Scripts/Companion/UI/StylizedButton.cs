using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private Color _mainColor;
        [SerializeField] private Color _shadowColor;
        [SerializeField] private RectTransform _childRectTransform;
        [SerializeField] private int _stepNumber = 10;
        [SerializeField] private TextMeshProUGUI _text;
        private List<Shadow> _shadows;
        
        [Header("Parameters")]
        [SerializeField] private float _hoverHeight = 5;
        [SerializeField] private float _pressedHeight = 0;
        [SerializeField] private float _defaultHeight = 3.2f;
        [SerializeField] private float _animationDuration = 0.1f;
        [SerializeField] private Vector2 _direction = new Vector2(0, -1);

        private float _height;
        private Sequence _sequence1;

        private void Awake()
        {
            _sequence1 = DOTween.Sequence();
            
            _shadows = new List<Shadow>();
            for (int i = 0; i < _stepNumber; i++)
            {
                // Add shadow to this and to the list
                var shadow = _childRectTransform.gameObject.AddComponent<Shadow>();
                _shadows.Add(shadow);
            }
            
            
        }

        private void Start()
        {
            SetHeight(_defaultHeight, false);

            _direction = _direction.normalized;
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

                    UpdateShadow();

                    Vector2 position = _direction.normalized * _height;
                    
                    Vector3 targetLocalPosition = _childRectTransform.localPosition;
                    targetLocalPosition.y = position.y;
                    targetLocalPosition.x = position.x;
                    _childRectTransform.localPosition = targetLocalPosition;
                    
                    targetLocalPosition = _text.transform.localPosition;
                    targetLocalPosition.y = position.y;
                    targetLocalPosition.x = position.x;
                    _text.transform.localPosition = targetLocalPosition;
                    
                }, height, _animationDuration));
                
                _sequence1.SetEase(Ease.OutSine);
                _sequence1.Play();
            }
            else
            {
                _height = height;
                
                UpdateShadow();

                Vector2 position = _direction.normalized * _height;
                    
                Vector3 targetLocalPosition = _childRectTransform.localPosition;
                targetLocalPosition.y = position.y;
                targetLocalPosition.x = position.x;
                _childRectTransform.localPosition = targetLocalPosition;
                    
                targetLocalPosition = _text.transform.localPosition;
                targetLocalPosition.y = position.y;
                targetLocalPosition.x = position.x;
                _text.transform.localPosition = targetLocalPosition;
            }
        }

        public void UpdateShadow()
        {
            float lastHeight = 0;
            
            for (int i = 0; i < _stepNumber; i++)
            {
                var shadow = _shadows[i];
                shadow.effectDistance = new Vector2(
                    -_height * ((i + 1f) / _stepNumber - lastHeight) * _direction.x,
                    -_height * ((i + 1f) / _stepNumber - lastHeight) * _direction.y);
                
                lastHeight = (i + 1f) / _stepNumber;
            }
            
            _shadows.ForEach(shadow => shadow.effectColor = _shadowColor);
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