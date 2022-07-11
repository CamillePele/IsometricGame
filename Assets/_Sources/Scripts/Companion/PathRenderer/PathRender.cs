using UnityEngine;
using UnityEngine.UI;

namespace Companion.PathRenderer {
	public class PathRender : MonoBehaviour {
		
		private Image image;
		
		public void Setup(Manager.Grid.Direction from, Manager.Grid.Direction to, PathRender parent) {
			if (from == to) {
				return;
			}

			if (Mathf.Abs((int) from - (int) to) % 2 == 0) { // If go straight
				image.sprite = Manager.PathRenderer.Instance.straightLine;
				transform.rotation = parent.transform.rotation;
			} else { // If turn
				image.sprite = Manager.PathRenderer.Instance.cornerLine;
				
				// Get if turn right or left, handle out of range
				bool turnLeft = ((int) from - (int) to) % 3 == 0;
				
				if (turnLeft) transform.rotation = parent.transform.rotation * Quaternion.Euler(0, 0, -90);
				// Dont turn right it's the default rotation
				
			}
		}
	}
}