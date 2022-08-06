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
    public class GenericButton : MonoBehaviour
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
        [SerializeField] private Color _mainColor; public Color MainColor
        {
            get { return _mainColor; }
            set
            {
                if (value == _mainColor) return;
                _mainColor = value;
                UpdateColor();
            }
        }
        [SerializeField] private Color _shadowColor; public Color ShadowColor
        {
            get { return _shadowColor; }
            set
            {
                if (value == _shadowColor) return;
                _shadowColor = value;
                UpdateColor();
            }
        }
        [SerializeField] private Image _background;
        [SerializeField] private Outline _outline;
        [Range(1, 10)] [SerializeField] private int _stepNumber = 10;
        [SerializeField] private TextMeshProUGUI _text;
        private List<Shadow> _shadows = new List<Shadow>();
        
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

            _outline = _background.GetComponent<Outline>();
            
            _shadows = new List<Shadow>();
            for (int i = 0; i < _stepNumber; i++)
            {
                // Add shadow to this and to the list
                var shadow = _background.gameObject.AddComponent<Shadow>();
                _shadows.Add(shadow);
            }
        }

        private void Start()
        {
            SetHeight(_defaultHeight, false);

            _direction = _direction.normalized;
        }

        public void OnValidate()
        {
            if (_stepNumber < 1) _stepNumber = 1;
            if (_stepNumber > 10) _stepNumber = 10;
            
            UpdateColor();
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
                    
                    Vector3 targetLocalPosition = _background.rectTransform.localPosition;
                    targetLocalPosition.y = position.y;
                    targetLocalPosition.x = position.x;
                    _background.rectTransform.localPosition = targetLocalPosition;
                    
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
                    
                Vector3 targetLocalPosition = _background.rectTransform.localPosition;
                targetLocalPosition.y = position.y;
                targetLocalPosition.x = position.x;
                _background.rectTransform.localPosition = targetLocalPosition;
                    
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

        public void UpdateColor()
        {
            if (_outline != null) _outline.effectColor = _shadowColor;
            else _background.GetComponent<Outline>().effectColor = _shadowColor;
            
            _shadows.ForEach(shadow => shadow.effectColor = _shadowColor);
            _background.color = _mainColor;
        }
    }
}