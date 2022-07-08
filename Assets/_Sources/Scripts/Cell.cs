using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField] private Button _button;
    [SerializeField] private EventTrigger _eventTrigger;
    public Vector2 coordinates;
    
    public UnityEvent OnClick;
    public UnityEvent OnHover;
    public UnityEvent OnUnhover;

    public bool IsSelectable
    {
        get => GetComponent<Image>().enabled;
        set => GetComponent<Image>().enabled = value;
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