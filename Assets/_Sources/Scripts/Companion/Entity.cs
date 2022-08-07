using System;
using System.Collections.Generic;
using Classes;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Grid = Manager.Grid;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    [Header("Entity Infos")]
    public string entityName;
    public int entityLevel;

    [Space]
    public SOSkeleton.EntityData entityData;
    public EntityStats gameStats;
    public EntityStats modifiersStats;
    public OrderedList<SOSkeleton.AttackData> attacks = new OrderedList<SOSkeleton.AttackData>(4);
    
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
            _attackIndex = value % (entityData.availableAttacks.Count + 1);
            UpdateDisplay();
        }
    }
    private List<Vector2Int> _availableAttackCells = new List<Vector2Int>();
    private List<Tuple<Vector2Int, int>> _attackCells = new List<Tuple<Vector2Int, int>>();
    
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

        gameStats = entityData.constStats.Clone(); // Set default stats to data stats
        
        // To debug take 4 randomly attacks from the entity data.
        List<int> randomAttacks = new List<int>() {0, 1, 2, 3};
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, randomAttacks.Count);
            attacks.Set(randomIndex, entityData.availableAttacks[randomAttacks[randomIndex]]);
            
            randomAttacks.Remove(randomIndex);
        }
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
    
    public void TakeDamage(int damage)
    {
        gameStats.health -= damage;
        if (gameStats.health <= 0)
        {
            // TODO : Animate death
        }
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
            DisplayAttackableCells(entityData.availableAttacks[AttackIndex - 1]);
        }
    }
    
    private void OnHoverPositionChanged(Vector2Int? pos)
    {
        if (!CanPlay) return;
        
        if (pos != null)
        {
            Manager.Grid.Direction? dir = _gridManager.GetDirection(Position, pos.Value);
            if (dir != null) // TODO : handle diagonal movement
            {
                Direction = dir.Value;
            }
        }
        
        if (AttackIndex > 0) // If the player selected an attack
        {        
            foreach (Tuple<Vector2Int, int> cellData in _attackCells)
            {
                _gridManager.GetCell(cellData.Item1).Clear();
            }
            _attackCells.Clear();

            foreach (Vector2Int attackableCell in _availableAttackCells) // Re colorize the attackable cells, don't care about loop in empty list
            {
                _gridManager.GetCell(attackableCell).SetColor(Color.blue);
            }

            if (pos != null  && _availableAttackCells.Contains(pos.Value))
            {
                DisplayAttackCells(entityData.availableAttacks[AttackIndex - 1], pos.Value, Direction);
            }
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
                entityData.availableAttacks[AttackIndex - 1].ApplyAttack(this, _attackCells);
            }
        }
    }
    
    public void DisplayReachableCells()
    {
        var reach = Manager.Grid.GetReachableCells(entityData.PM);
        Vector2Int pivot = new Vector2Int(reach.Count / 2, reach.Count / 2); // Set the pivot to the center of the layout
        List<Tuple<Vector2Int, Vector2Int>> cells =_gridManager.GetCellsByLayout(reach, Position, pivot, Manager.Grid.Direction.North);

        foreach (Tuple<Vector2Int, Vector2Int> cellData in cells)
        {
            if (!CanMoveTo(cellData.Item1)) continue;
            _gridManager.GetCell(cellData.Item1).SetColor(Color.green);
        }
    }
    
    public void DisplayAttackableCells(SOSkeleton.AttackData attack)
    {
        List<List<bool>> attackableCells = attack.AttackablePattern;
        Vector2Int pivot = new Vector2Int(attackableCells.Count / 2, attackableCells.Count / 2); // Set the pivot to the center of the layout
        List<Tuple<Vector2Int, Vector2Int>> cells = _gridManager.GetCellsByLayout(attackableCells, Position, pivot, Manager.Grid.Direction.North);

        _availableAttackCells = new List<Vector2Int>();
        
        foreach (Tuple<Vector2Int, Vector2Int> cellData in cells)
        { 
            bool hasSightLine = SightLine.HasSightLine(Position, cellData.Item1, (v2) => _gridManager.mapData.Layout[v2.x][v2.y] > 0);
            if (hasSightLine)
            {
                _gridManager.GetCell(cellData.Item1).SetColor(Color.blue);
                _availableAttackCells.Add(cellData.Item1);
            }
        }
    }

    public void DisplayAttackCells(SOSkeleton.AttackData attack, Vector2Int pos, Grid.Direction dir)
    {
        Vector2Int pivot = attack.attackPivot;
        List<Tuple<Vector2Int, Vector2Int>> cells = _gridManager.GetCellsByLayout(attack.AttackPattern, pos, pivot, dir);
        
        foreach (Tuple<Vector2Int, Vector2Int> cellData in cells)
        {
            _gridManager.GetCell(cellData.Item1).SetColor(Color.red);
            
            int damageValue = attack.AttackPatternValue[cellData.Item2.x][cellData.Item2.y];
            
            _attackCells.Add(new Tuple<Vector2Int, int>(cellData.Item1, damageValue));
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