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
		
		[SerializeField] private GameObject _pathPrefab;
		
		[SerializeField] public Sprite straightLine;
		[SerializeField] public Sprite cornerLine;
        [SerializeField] public Sprite startEndLine;

        private List<PathRender> _usingPaths = new List<PathRender>();
        private List<PathRender> _unusedPaths = new List<PathRender>();

        public void DrawPath(List<Vector2Int> path)
        {

	        for (int i = 0; i < path.Count; i++) {
		        Vector2Int currentPos = path[i];
		        Vector2Int? lastPos = null;
		        if (i > 0) {
			        lastPos = path[i - 1];
		        }
		        
		        
	        }
        }
	}
}