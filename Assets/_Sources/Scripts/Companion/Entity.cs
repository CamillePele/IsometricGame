using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Entity : MonoBehaviour
{
    public SOSkeleton.EntityData entityData;
    
    private Vector2Int _position;
    public Vector2Int Position
    {
        get => _position;
        set
        {
            UpdatePosition(value);
            _position = value;
        }
    }

    // If the current entity is a player, this will be set to true.
    public bool CanPlay
    {
        get => _gameManager.currentEntityPlaying == _gameManager.entities.IndexOf(this);
    }
    
    private Manager.Game _gameManager;
    private Manager.Grid _gridManager;

    private void Start()
    {
        _gameManager = Manager.Game.Instance;
        _gridManager = Manager.Grid.Instance;
        _gridManager.OnCellClicked.AddListener(OnCellClicked);
        Position = new Vector2Int(4, 4);
    }

    public void UpdatePosition(Vector2Int newPos = default)
    {
        if (newPos.x < 0 || newPos.x > Manager.Grid.GridSize || newPos.y < 0 || newPos.y > Manager.Grid.GridSize)
        {
            Debug.LogWarning("Trying to move outside of map", this);
            return;
        }
        
        // TODO : Animate movement
        transform.position = new Vector3(newPos.x + 0.5f, transform.position.y, newPos.y + 0.5f);
    }

    public void StartTurn()
    {
        var reach = Manager.Grid.GetReachableCells(entityData.PM);
        Vector2Int pivot = new Vector2Int(reach.Count / 2, reach.Count / 2); // Set the pivot to the center of the layout
        _gridManager.ShowLayout(reach, Position, pivot, Manager.Grid.Direction.North, Color.green);
    }
    
    private void OnCellClicked(Vector2Int position)
    {
        if (!CanPlay) return;
        
        int distance = _gridManager.GetDistance(Position, position);
        distance--; // Remove the distance between the current position and the clicked cell, to not count self as a move
        if (distance > entityData.PM)
        {
            Debug.LogWarning("Trying to move too far", this);
            return;
        }
        if (distance == 0)
        {
            Debug.LogWarning("Trying to move to the same cell", this);
            return;
        }
        if (distance < 0) // If the distance is negative, it means that the cell is not reachable
        {
            Debug.LogWarning("Trying to move to an unreachable cell", this);
            return;
        }
        
        Position = position;
        
        //TODO : dont do that here
        _gameManager.NextEntity();
    }
}