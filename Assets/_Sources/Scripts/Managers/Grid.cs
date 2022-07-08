using System;
using System.Collections.Generic;
using System.Linq;
using Array2DEditor;
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
        
        
        [SerializeField] private SOSkeleton.MapData _mapData;
        [SerializeField] private GridLayoutGroup _gridLayout;
        [SerializeField] private List<Cell> _cells;

        [CanBeNull] public Cell HoverCell;

        public UnityEvent<Vector2> OnCellClicked = new UnityEvent<Vector2>();

        private void Start()
        {
            _cells = _gridLayout.GetComponentsInChildren<Cell>().ToList();
            
            int index = 0;
            _cells.ForEach(c =>
            {
                int i = index;
                
                c.coordinates = GetCellPosition(i);
            
                c.OnHover.AddListener(() => HoverCell = c);
                c.OnUnhover.AddListener(() => HoverCell = null);

                c.OnClick.AddListener(() => OnCellClicked.Invoke(GetCellPosition(i)));
                
                index++;
            });

            LoadMap();
        }

        public void LoadMap()
        {
            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    Cell cell = GetCell(x - Maximum, y - Maximum);
                    cell.IsSelectable = _mapData.layout.GetCell(x, (GridSize-1)-y); // Invert y axis 
                }
            }
            Utils.GeneralUtils.AfterXFrames(this, 1,() => {
                _cells.ForEach(c =>
                {
                    if (c.IsSelectable) c.SetHeight();
                });
            });

            ShowLayout(GetReachableCells(3), Vector2Int.zero, new Vector2Int(3, 3));
        }

        /// <summary>
        /// Function do displaye layout over the grid
        /// </summary>
        /// <param name="layout">Cell to show</param>
        /// <param name="position">Position to show on the grid</param>
        /// <param name="pivot">Center of the layout</param>
        public void ShowLayout(List<List<bool>> layout, Vector2Int position, Vector2Int pivot)
        {
            for (int x = 0; x < layout.Count; x++)
            {
                for (int y = 0; y < layout[x].Count; y++)
                {
                    if (x - pivot.x == 0 && y - pivot.y == 0) continue;
                    if (layout[x][y]) GetCell(position.x + x - pivot.x,position.y + y - pivot.y).SetColor(Color.red);
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
            if (x < -Maximum || x > Maximum || y < -Maximum || y > Maximum)
            {
                return null;
            }
            x += Maximum; y += Maximum; // Convert to grid coordinates because the grid is offset by Maximum (eg: -4;-4)
            
            
            return _cells[x * GridSize + y];
        }

        public Vector2 GetCellPosition(int index)
        {
            if (index < 0 || index >= GridSize * GridSize)
            {
                Debug.LogWarning("Cell "+index+" is not in the grid");
                return Vector2.zero;
            }
            
            int x = index / GridSize - Maximum;
            int y = index % GridSize - Maximum;
            
            return new Vector2(x, y);
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
    }
}