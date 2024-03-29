﻿using Classes.Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Companion.Cell
{
    
    // TODO: Rewrite this class
     public class Cell : MonoBehaviour
     {
         private RectTransform _rectTransform;
         [SerializeField] private Button _button;
         [SerializeField] private EventTrigger _eventTrigger;
         
         
         // Pathnode variables
         private PathNode _pathNode;
         public PathNode PathNode
         {
             get => _pathNode;
             set
             {
                 _fCost.text = value.FCost.ToString();
                 _gCost.text = value.gCost.ToString();
                 _hCost.text = value.hCost.ToString();
             }
         }

         [SerializeField] private TextMeshProUGUI _fCost;
         [SerializeField] private TextMeshProUGUI _gCost;
         [SerializeField] private TextMeshProUGUI _hCost;

         public Vector2Int Position
         {
             get => Manager.Grid.Instance.GetCellPosition(this);
         }
         
         public UnityEvent OnClick;
         public UnityEvent OnHover;
         public UnityEvent OnUnhover;
     
         public bool IsSelectable
         {
             get => GetComponentInChildren<Image>().enabled;
             set => GetComponentInChildren<Image>().enabled = value;
         }
         
         private void Awake()
         {
             _rectTransform = GetComponent<RectTransform>();
             OnClick = _button.onClick;
     
             // Add listener for hover event
             EventTrigger.Entry entry = new EventTrigger.Entry();
             entry.eventID = EventTriggerType.PointerEnter;
             entry.callback.AddListener((data) =>
             {
                 if (IsSelectable) OnHover.Invoke();
             });
             _eventTrigger.triggers.Add(entry);  
     
             // Add listener for unhover event
             entry = new EventTrigger.Entry();
             entry.eventID = EventTriggerType.PointerExit;
             entry.callback.AddListener((data) =>
             {
                 if (IsSelectable) OnUnhover.Invoke();
             });
             _eventTrigger.triggers.Add(entry);
         }
         
         public void SetColor(Color color)
         {
             GetComponentInChildren<Image>().color = color;
         }
         
         public void Clear()
         {
             _fCost.text = "";
             _gCost.text = "";
             _hCost.text = "";
             SetColor(Color.white); // TODO : made a color variable
         }
         
         public void SetHeight()
         {
             int layerMask = LayerMask.NameToLayer(Manager.Grid.Instance.mapGroundLayer);
             
             RaycastHit hit;
             if (Physics.Raycast(transform.position + (Vector3.up*10), Vector3.down*100, out hit, Mathf.Infinity)) // TODO : use layer
             {
                 float yPos = hit.point.y + 0.01f;
                 transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
             }
         }
     }   
}