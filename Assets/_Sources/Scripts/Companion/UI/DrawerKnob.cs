using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Companion.Cell
{
    public class DrawerKnob : MonoBehaviour
    {
        [SerializeField] private Image _headArrow;
        
        public void AnimationState(bool state)
        {
            int targetScale = state ? -1 : 1;
            _headArrow.transform.DOScaleY(targetScale, 0.1f);
        }
    }
}
