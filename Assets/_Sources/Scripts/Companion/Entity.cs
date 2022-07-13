using System;
using System.Collections.Generic;
using Classes;
using Classes.Pathfinding;
using Companion.PathRenderer;
using Manager;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Grid = Manager.Grid;

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
            UpdateDisplay();
        }
    }
    
    [SerializeField] private Manager.Grid.Direction _direction;
    public Manager.Grid.Direction Direction
    {
        get => _direction;
        set
        {
            UpdateDirection(value);
            _direction = value;
        }
    }


    // If the current entity is a player, this will be set to true.
    public bool CanPlay
    {
        get => _gameManager.currentEntityPlaying == _gameManager.entities.IndexOf(this);
    }

    private int _attackIndex;
    public int AttackIndex
    {
        get => _attackIndex;
        set
        {
            _attackIndex = value % (entityData.attacks.Count + 1);
            UpdateDisplay();
        }
    }
    private List<Vector2Int> _availableAttackCells = new List<Vector2Int>();
    private List<Vector2Int> _colorizedAttackCells = new List<Vector2Int>();
    
    private Manager.Game _gameManager;
    private Manager.Grid _gridManager;

    private void Start()
    {
        _gameManager = Manager.Game.Instance;
        _gridManager = Manager.Grid.Instance;
        _gridManager.OnCellClicked.AddListener(OnCellClicked);
        _gridManager.OnHoverPositionChanged.AddListener(OnHoverPositionChanged);
        Position = new Vector2Int(4, 4);
        _attackIndex = 1;
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
    
    public void UpdateDirection(Manager.Grid.Direction newDir = default)
    {
        if (newDir == default)
            newDir = Manager.Grid.Direction.North;
        transform.eulerAngles = new Vector3(0, (int) newDir * 90, 0);
    }   

    public void StartTurn()
    {
        UpdateDisplay();
    }
    
    public void UpdateDisplay()
    {
        _gridManager.Clear();
        if (AttackIndex == 0) //Display moveable cells
        {
            DisplayReachableCells();
        }
        else //Display attackable cells
        {
            DisplayAttackableCells(entityData.attacks[AttackIndex - 1]);
        }
    }
    
    private void OnHoverPositionChanged(Vector2Int? pos)
    {
        if (!CanPlay) return;
        
        if (pos != null)
        {
            Manager.Grid.Direction? dir = Manager.Grid.GetDirection(Position, pos.Value);
            if (dir != null) // TODO : handle diagonal movement
            {
                Direction = dir.Value;
            }
        }
        
        if (AttackIndex > 0) // If the player selected an attack
        {        
            foreach (Vector2Int cell in _colorizedAttackCells)
            {
                _gridManager.GetCell(cell).Clear();
            }
            _colorizedAttackCells.Clear();

            foreach (Vector2Int attackableCell in _availableAttackCells) // Re colorize the attackable cells, don't care about loop in empty list
            {
                _gridManager.GetCell(attackableCell).SetColor(Color.blue);
            }

            if (pos != null  && _availableAttackCells.Contains(pos.Value))
            {
                DisplayAttackCells(entityData.attacks[AttackIndex - 1], pos.Value, Direction);
            }
        }
        else if (pos.HasValue) {
            PathRenderer.Instance.DrawPath(Pathfinding.FindPath(Position, pos.Value, Grid.Direction.North, i => true));
        }
    }
    
    private void OnCellClicked(Vector2Int pos)
    {
        if (!CanPlay) return;
        if (AttackIndex == 0) // Is entity selected move
        {
            MoveTo(pos);
        }
        else
        {
            if (_availableAttackCells.Contains(pos))
            {
                // TODO : move for the moment to have feedback befor attack implementation
                Position = pos;
            }
        }
    }
    
    public void DisplayReachableCells()
    {
        var reach = Manager.Grid.GetReachableCells(entityData.PM);
        Vector2Int pivot = new Vector2Int(reach.Count / 2, reach.Count / 2); // Set the pivot to the center of the layout
        List<Vector2Int> cells =_gridManager.GetCellsByLayout(reach, Position, pivot, Manager.Grid.Direction.North);

        foreach (Vector2Int cell in cells)
        {
            if (!CanMoveTo(cell)) continue;
            _gridManager.GetCell(cell).SetColor(Color.green);
        }
    }
    
    public void DisplayAttackableCells(SOSkeleton.AttackData attack)
    {
        List<List<bool>> attackableCells = attack.AttackablePattern;
        Vector2Int pivot = new Vector2Int(attackableCells.Count / 2, attackableCells.Count / 2); // Set the pivot to the center of the layout
        List<Vector2Int> cells = _gridManager.GetCellsByLayout(attackableCells, Position, pivot, Manager.Grid.Direction.North);

        _availableAttackCells = new List<Vector2Int>();
        
        foreach (Vector2Int cell in cells)
        { 
            bool hasSightLine = SightLine.HasSightLine(Position, cell, (v2) => _gridManager.mapData.Layout[v2.x][v2.y] > 0);
            if (hasSightLine)
            {
                _gridManager.GetCell(cell).SetColor(Color.blue);
                _availableAttackCells.Add(cell);
            }
        }
    }

    public void DisplayAttackCells(SOSkeleton.AttackData attack, Vector2Int pos, Grid.Direction dir)
    {
        List<List<bool>> attackCells = attack.AttackPattern;
        Vector2Int pivot = attack.attackPivot;
        List<Vector2Int> cells = _gridManager.GetCellsByLayout(attackCells, pos, pivot, dir);
        
        foreach (Vector2Int cell in cells)
        {
            _gridManager.GetCell(cell).SetColor(Color.red);
            _colorizedAttackCells.Add(cell);
        }
    }
    
    public void MoveTo(Vector2Int position)
    {
        if (!CanMoveTo(position)) return;
        Position = position;
    }
    
    public bool CanMoveTo(Vector2Int position)
    {
        int distance = _gridManager.GetDistance(Position, position);
        distance--; // Remove the distance between the current position and the clicked cell, to not count self as a move
        if (distance > entityData.PM)
        {
            // Debug.LogWarning("Trying to move too far", this);
            return false;
        }
        if (distance == 0)
        {
            // Debug.LogWarning("Trying to move to the same cell", this);
            return false;
        }
        if (distance < 0) // If the distance is negative, it means that the cell is not reachable
        {
            // Debug.LogWarning("Trying to move to an unreachable cell", this);
            return false;
        }
        return true;
    }
}