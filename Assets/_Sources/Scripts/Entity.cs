using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Entity : MonoBehaviour
{
    public SOSkeleton.EntityData entityData;
    
    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            UpdatePosition();
            _position = value;
        }
    }

    public bool CanPlay
    {
        get => _gameManager.currentEntityPlaying == _gameManager.entities.IndexOf(this);
    }
    
    private Manager.Game _gameManager;
    private Manager.Grid _gridManager;

    private void Awake()
    {
        _gameManager = Manager.Game.Instance;
        _gridManager = Manager.Grid.Instance;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (Position.x < -Manager.Grid.Maximum || Position.x > Manager.Grid.Maximum || Position.y < -Manager.Grid.Maximum || Position.y > Manager.Grid.Maximum)
        {
            Debug.LogWarning("Trying to move outside of map", this);
            return;
        }
        
        // TODO : Animate movement
        transform.position = new Vector3(Position.x, Position.y, 0);
    }

    public void StartTurn()
    {
        
    }
}