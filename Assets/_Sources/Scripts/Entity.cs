using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Entity : MonoBehaviour
{
    public SOEntityData entityData;
    
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
    
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (Position.x < -_gameManager.Maximum || Position.x > _gameManager.Maximum || Position.y < -_gameManager.Maximum || Position.y > _gameManager.Maximum)
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