using System;
using System.Collections.Generic;
using Companion.Cell;
using UnityEngine;

namespace Classes.Pathfinding 
{
    [Serializable]
    public class PathNode
    {

        public float gCost;

        public float hCost;

        public float FCost { get { return gCost + hCost; } }
        
        public Vector2Int position;
        public PathNode parent;
        
        public PathNode(float gCost, float hCost, Vector2Int position, PathNode parent)
        {
            this.gCost = gCost;
            this.hCost = hCost;
            this.position = position;
            this.parent = parent;
        }
        
        public PathNode(Vector2Int position)
        {
            this.position = position;
        }
    }
}