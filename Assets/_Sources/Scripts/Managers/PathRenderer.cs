using System.Collections.Generic;
using Companion.PathRenderer;
using UnityEngine;
using UnityEngine.UI;

namespace Manager {
	public class PathRenderer : MonoBehaviour {

		#region Singleton
		
		public static PathRenderer Instance;
		
		void Awake() {
			if (Instance != null) {
				Destroy(gameObject);
			} else {
				Instance = this;
			}
		}

		#endregion

		[SerializeField] private Transform _container;

		[SerializeField] private GameObject _pathPrefab;
		
		[SerializeField] public Sprite straightLine;
		[SerializeField] public Sprite cornerLine;
        [SerializeField] public Sprite startEndLine;

        private List<PathRender> _usingPaths = new List<PathRender>();
        private List<PathRender> _unusedPaths = new List<PathRender>();

        public void DrawPath(List<Vector2Int> path)
        {
	        // Move all pathrenders from using to unused
	        foreach (var pathRender in _usingPaths)
	        {
		        _unusedPaths.Add(pathRender);
		        pathRender.gameObject.SetActive(false);
	        }
	        _usingPaths.Clear();
	        
	        Grid.Direction direction = Grid.Direction.North;
	        for (int i = 0; i < path.Count; i++) {
		        Vector2Int currentPos = path[i];
		        Vector2Int? lastPos = null;
		        PathRender parent = null;
		        
		        Grid.Direction currentDirection = Grid.Direction.North;
		        if (i > 0) {
			        lastPos = path[i - 1];
			        parent = _usingPaths[i - 1];
			        
			        // Set the direction of the path by the last position
			        Grid.Direction? nullableDirection = Grid.GetDirection(lastPos.Value, currentPos);
			        if (nullableDirection == null) { // If the nullable direction is null, then the path is a straight line (shouldn't happen)
				        currentDirection = direction;
			        }
			        else { // If the nullable direction is not null, then the path is a corner line
				        currentDirection = nullableDirection.Value;
			        }
		        }

		        PathRender pathRender = null;
		        if (_unusedPaths.Count > 0) {
			        pathRender = _unusedPaths[0];
			        _unusedPaths.RemoveAt(0);
		        } else {
			        pathRender = Instantiate(_pathPrefab, _container).GetComponent<PathRender>();
		        }

		        pathRender.gameObject.SetActive(true);
				pathRender.transform.localPosition = new Vector3(currentPos.x, currentPos.y, 0);
		        pathRender.Setup(direction, currentDirection, parent);
		        _usingPaths.Add(pathRender);
		        
		        direction = currentDirection;
	        }
        }
	}
}