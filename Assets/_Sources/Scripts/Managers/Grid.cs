using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SimpleJSON;
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
        [SerializeField] private List<Cell> _cells = new List<Cell>();
        
        [CanBeNull] public Cell HoverCell;

        public UnityEvent<Vector2> OnCellClicked = new UnityEvent<Vector2>();

        private void Start()
        {
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
            for (int y = Maximum+1; y > -Maximum; y--)
            {
                string line = jsonArray[y + Maximum].Value;
                int x = -Maximum;
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
            if (x < -Maximum || x > Maximum || y < -Maximum || y > Maximum)
            {
                return null;
            }
            x += Maximum; y += Maximum;
            
            return _cells[x * GridSize + y];
        }
        
        public Vector2 GetCellPosition(Cell cell)
        {
            if (_cells.Contains(cell) == false)
            {
                Debug.LogWarning("Cell is not in the grid", cell);
                return Vector2.zero;
            }

            int cellIndex = _cells.IndexOf(cell);
            
            int x = cellIndex / GridSize - Maximum;
            int y = cellIndex % GridSize - Maximum;
            
            return new Vector2(x, y);
        }
    }
}