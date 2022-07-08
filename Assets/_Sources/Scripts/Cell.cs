using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private EventTrigger _eventTrigger;
    public Vector2 coordinates;
    
    public UnityEvent OnClick;
    public UnityEvent OnHover;
    public UnityEvent OnUnhover;

    public bool IsSelectable
    {
        get => _button.interactable;
        set => gameObject.SetActive(value);
    }
    
    private void Awake()
    {
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
}