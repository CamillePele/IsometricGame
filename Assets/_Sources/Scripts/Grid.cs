using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayout;
    [SerializeField] private List<Cell> _cells = new List<Cell>();
    private GameManager _gameManager;    
    
    [CanBeNull] public Cell HoverCell;

    public UnityEvent<Vector2> OnCellClicked = new UnityEvent<Vector2>();

    private void Start()
    {
        _gameManager = GameManager.Instance;
        
        _cells = GetComponentsInChildren<Cell>().ToList();

        _cells.ForEach(c =>
        {
            c.coordinates = GetCellPosition(c);
            
            c.OnHover.AddListener(() => HoverCell = c);
            c.OnUnhover.AddListener(() => HoverCell = null);
            
            c.OnClick.AddListener(() => OnCellClicked.Invoke(GetCellPosition(c)));
        });
    }

    public void LoadMap(JSONNode json, Func<char, bool> isEnable)
    {
        JSONArray jsonArray = json["cells"].AsArray;
        for (int y = _gameManager.Maximum+1; y > -_gameManager.Maximum; y--)
        {
            string line = jsonArray[y + _gameManager.Maximum].Value;
            int x = -_gameManager.Maximum;
            foreach (char c in line)
            {
                GetCell(x, -y).IsSelectable = isEnable(c);
                
                x++;
            }
        }
    }
    
    /// <summary>
    /// Function to get the cell at the given position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Cell GetCell(int x, int y)
    {
        if (x < -_gameManager.Maximum || x > _gameManager.Maximum || y < -_gameManager.Maximum || y > _gameManager.Maximum)
        {
            return null;
        }
        x += _gameManager.Maximum; y += _gameManager.Maximum;
        
        return _cells[x * _gameManager.GridSize + y];
    }
    
    public Vector2 GetCellPosition(Cell cell)
    {
        if (_cells.Contains(cell) == false)
        {
            Debug.LogWarning("Cell is not in the grid", cell);
            return Vector2.zero;
        }

        int cellIndex = _cells.IndexOf(cell);
        
        int x = cellIndex / _gameManager.GridSize - _gameManager.Maximum;
        int y = cellIndex % _gameManager.GridSize - _gameManager.Maximum;
        
        return new Vector2(x, y);
    }
}