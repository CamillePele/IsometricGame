﻿using System;
using System.Collections.Generic;
using System.Linq;
using Classes.Pathfinding;
using Companion.Cell;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Manager
{
    public class Grid : MonoBehaviour
    {
        #region Singleton
        
        public static Grid Instance;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            Init();
        }
        
        #endregion

        [SerializeField] public string mapGroundLayer = "MapGround";
        
        public static int GridSize = 9;
        public static int Maximum
        {
            get
            {
                if (GridSize % 2 == 0)
                {
                    return GridSize / 2;
                }
                else
                {
                    return (GridSize - 1) / 2;
                }
            }
        }

        public List<List<bool>> GridData
        {
            get { return Utils.Maths.Get2DList(_mapData.layoutArray, i => i == 0); }
        }
        
        [SerializeField] private SOSkeleton.MapData _mapData;
        [SerializeField] private GridLayoutGroup _gridLayout;
        [SerializeField] private List<Cell> _cells;

        public enum Direction
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }

        [CanBeNull] public Cell HoverCell;

        public UnityEvent<Vector2Int> OnCellClicked = new UnityEvent<Vector2Int>();

        private void Init()
        {
            _cells = _gridLayout.GetComponentsInChildren<Cell>().ToList();
            
            _cells.ForEach(c =>
            {
                c.OnHover.AddListener(() => HoverCell = c);
                c.OnUnhover.AddListener(() => HoverCell = null);

                c.OnClick.AddListener(() =>
                {
                    if (!c.IsSelectable) return;
                    OnCellClicked.Invoke(GetCellPosition(c));
                });
            });

            LoadMap();
        }

        public void LoadMap()
        {
            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    Cell cell = GetCell(x, y);
                    cell.IsSelectable = _mapData.Layout[x][y] == 0; 
                }
            }
            Utils.GeneralUtils.AfterXFrames(this, 1,() => {
                _cells.ForEach(c =>
                {
                    if (c.IsSelectable) c.SetHeight();
                });
            });

            // ShowLayout(GetReachableCells(3), Vector2Int.zero, new Vector2Int(3, 3), Direction.North, Color.red);
        }

        /// <summary>
        /// Function do displaye layout over the grid
        /// </summary>
        /// <param name="layout">Cell to show</param>
        /// <param name="position">Position to show on the grid</param>
        /// <param name="pivot">Center of the layout</param>
        public void ShowLayout(List<List<bool>> layout, Vector2Int position, Vector2Int pivot, Direction direction, Color color)
        {
            for (int x = 0; x < layout.Count; x++)
            {
                for (int y = 0; y < layout[x].Count; y++)
                {
                    // Create copy of the parameters
                    List<List<bool>> newLayout = new List<List<bool>>(layout);
                    Vector2Int newPivot = pivot;
                    (int xPos, int yPos) = (x, y);
                    
                    
                    if (direction == Direction.South)
                    {
                        // Reverse the y axis layout
                        newLayout = newLayout.Select(l => l.AsEnumerable().Reverse().ToList()).ToList();
                        // Reverse the x axis layout
                        newLayout = newLayout.AsEnumerable().Reverse().ToList();

                        newPivot = new Vector2Int(newLayout.Count - 1 - pivot.x, newLayout[0].Count - 1 - pivot.y);
                    }
                    if (direction == Direction.East || direction == Direction.West)
                    {
                        //Exchange row and columns of newLayout
                        newLayout = new List<List<bool>>(); // Clear newLayout
                        for (int i = 0; i < layout[0].Count; i++)
                        {
                            newLayout.Add(new List<bool>());
                            for (int j = 0; j < layout.Count; j++)
                            {
                                newLayout[i].Add(layout[j][i]); // Invert row and columns of newLayout
                            }
                        }
                        newLayout = newLayout.Select(l => l.AsEnumerable().Reverse().ToList()).ToList(); // Reverse the x axis layout

                        newPivot = new Vector2Int(pivot.y, pivot.x); // Exchange x and y axis of pivot
                        
                        (xPos, yPos) = (y, x); // Exchange x and y

                        if (direction == Direction.West)
                        {
                            // Reverse the y axis layout
                            newLayout = newLayout.Select(l => l.AsEnumerable().Reverse().ToList()).ToList();
                            // Reverse the x axis layout
                            newLayout = newLayout.AsEnumerable().Reverse().ToList();
                            
                            newPivot = new Vector2Int(newLayout.Count - 1 - newPivot.x, newLayout[0].Count - 1 - newPivot.y); // Invert x and y
                        }
                    }
                    //          Offset by the position of the grid    Substract the pivot to get the position of the layout in the grid
                    Cell cell = GetCell(position.x + xPos - newPivot.x, position.y + yPos - newPivot.y);
                    if (cell == null || !cell.IsSelectable) continue;
                    if (newLayout[xPos][yPos]) cell.SetColor(color);
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
            if (x < 0 || x > GridSize || y < 0 || y > GridSize)
            {
                return null;
            }
            print($"GetCell at {x}, {y}: {x + y * GridSize}");
            
            return _cells[x + y * GridSize];
        }
        /// <summary>
        /// Surcharge of GetCell to get the cell at the given position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convert">True to convert from 0 -> max to -max/2 -> max/2</param>
        /// <returns></returns>
        public Cell GetCell(Vector2Int position)
        {
            return GetCell(position.x, position.y);
        }


        public Vector2Int GetCellPosition(int index)
        {
            if (index < 0 || index >= GridSize * GridSize)
            {
                Debug.LogWarning("Cell "+index+" is not in the grid");
                return Vector2Int.zero;
            }
            
            int x = index % GridSize;
            int y = index / GridSize;
            
            return new Vector2Int(x, y);
        }
        public Vector2Int GetCellPosition(Cell cell)
        {
            return GetCellPosition(cell.transform.GetSiblingIndex());
        }

        /// <summary>
        /// Fiunction to clear the grid
        /// </summary>
        public void Clear()
        {
            _cells.ForEach(c =>
            {
                if (c.IsSelectable) c.SetColor(Color.white);
            });
        }
        
        public static List<List<bool>> GetReachableCells(int range)
        {
            List<List<bool>> result = new List<List<bool>>();
            for (int y = -range; y < range+1; y++)
            {
                string line = "";
                line += new String(' ', Math.Abs(y));
                line += new String('X', (2 * range + 1) - Math.Abs(y)*2);
                line += new String(' ', Math.Abs(y));
                
                result.Add(line.Select(c => c == 'X').ToList());
            }

            return result;
        }
        
        public int GetDistance(Vector2Int a, Vector2Int b)
        {
            List<Vector2Int> path = Pathfinding.FindPath(a, b, Direction.North, 
                (v2) =>
                {
                    if (v2.x < 0 || v2.y < 0 || v2.x >= GridSize || v2.y >= GridSize)
                    {
                        return false;
                    }
                    return GridData[v2.x][v2.y];
                });
            if (path == null) return -1;
            return path.Count;
        }
    }
}