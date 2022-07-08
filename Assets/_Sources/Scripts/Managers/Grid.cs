using System;
using System.Collections.Generic;
using System.Linq;
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
                    cell.IsSelectable = _mapData.layout[x].cells[y];
                }
            }
            Utils.GeneralUtils.AfterXFrames(this, 1,() => {
                _cells.ForEach(c =>
                {
                    if (c.IsSelectable) c.SetHeight();
                });
            });
            
            // JSONArray jsonArray = json["cells"].AsArray;
            // for (int y = Maximum+1; y > -Maximum; y--)
            // {
            //     string line = jsonArray[y + Maximum].Value;
            //     int x = -Maximum;
            //     foreach (char c in line)
            //     {
            //         GetCell(x, -y).IsSelectable = isEnable(c);
            //         
            //         x++;
            //     }
            // }
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
        
        /// <summary>
        /// Function to get the cell at the given position by gridLayoutHeight.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="gridlayoutHeight">The specific grid layout height</param>
        /// <returns></returns>
        // public Cell GetCellByGrid(int x, int y, GridlayoutHeight gridlayoutHeight)
        // {
        //     if (x < -Maximum || x > Maximum || y < -Maximum || y > Maximum)
        //     {
        //         return null;
        //     }
        //     x += Maximum; y += Maximum; // Convert to grid coordinates because the grid is offset by Maximum (eg: -4;-4)
        //     Debug.Log(gridlayoutHeight.cells.Count);
        //     return gridlayoutHeight.cells[x * GridSize + y];
        // }
        
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
    }
}