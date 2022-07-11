using System.Collections.Generic;
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

	}
}