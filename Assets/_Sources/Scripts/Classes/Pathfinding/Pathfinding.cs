using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Classes.Pathfinding
{
    public class Pathfinding
    {
        private const float STRAIGHT_DISTANCE = 1;
        
        // public Pathfinding(List<List<bool>> grid, Vector2Int start, Vector2Int end, Manager.Grid.Direction direction, Action<List<Vector2Int>> callback)
        // {
        //     this._grid = grid;
        //     this._debug = false;
        //     return FindPath(start, end, direction, callback);
        // }

        public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, Manager.Grid.Direction direction, Func<Vector2Int, bool> isAvailable, bool debug)
        {
            PathNode startNode = new PathNode(0, HeuristicCost(start, end), start, null);
            if (debug) Manager.Grid.Instance.GetCell(start, true).PathNode = startNode;
            
            List<PathNode> openList = new List<PathNode>() {startNode};
            List<PathNode> closedList = new List<PathNode>();

            while (openList.Count > 0)
            {
                // Get the node with the lowest F cost and the lowest Hcost, and direction
                var orderedEnumerable = openList.OrderBy(node => node.FCost)
                    .ThenBy(node => node.hCost);
                
                // TODO : Fix clock wise direction, for the moment it's not clockwise
                int directionsCount = Enum.GetNames(typeof(Manager.Grid.Direction)).Length;
                for (int i = 0; i < directionsCount; i++)
                {
                    Manager.Grid.Direction tempDirection = (Manager.Grid.Direction) (((int) direction + i) % directionsCount);
                    // Debug.Log(((Manager.Grid.Direction) tempDirection).ToString());
                    
                    if (tempDirection == Manager.Grid.Direction.North)
                    {
                        orderedEnumerable = orderedEnumerable.ThenBy(n => n.position.x);
                    }
                    else if (tempDirection == Manager.Grid.Direction.South)
                    {
                        orderedEnumerable = orderedEnumerable.ThenByDescending(n => n.position.y);
                    }
                    else if (tempDirection == Manager.Grid.Direction.East)
                    {
                        orderedEnumerable = orderedEnumerable.ThenBy(n => n.position.y);
                    }
                    else if (tempDirection == Manager.Grid.Direction.West)
                    {
                        orderedEnumerable = orderedEnumerable.ThenByDescending(n => n.position.x);
                    }
                }
                openList = orderedEnumerable.ToList();

                PathNode currentNode = openList[0];

                if (debug)
                {
                    Manager.Grid.Instance.GetCell(currentNode.position, true).SetColor(Color.green);
                }
                
                // If the current node is the end node, return the path
                if (currentNode.position.Equals(end))
                {
                    return ReconstructPath(currentNode).Select(n => n.position).ToList();
                }
                
                // Remove the current node from the open list
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // Get the neighbours of the current node
                List<PathNode> neighbours = GetNeighbours(currentNode, isAvailable);
                foreach (PathNode neighbour in neighbours)
                {
                    // If the neighbour is in the closed list, skip it
                    if (closedList.Any(n => n.position == neighbour.position) || openList.Any(n => n.position == neighbour.position))
                    {
                        continue;
                    }
                    
                    // Calculate the neighbour's costs
                    neighbour.gCost = currentNode.gCost + STRAIGHT_DISTANCE;
                    neighbour.hCost = HeuristicCost(neighbour.position, end);
                    
                    // Set the neighbour's parent to the current node
                    neighbour.parent = currentNode;
                    
                    // Add the neighbour to the open list
                    openList.Add(neighbour);

                    if (debug)
                    {
                        Manager.Grid.Instance.GetCell(neighbour.position, true).PathNode = neighbour;
                        Manager.Grid.Instance.GetCell(neighbour.position, true).SetColor(Color.blue);
                    }
                }
                Manager.Grid.Instance.GetCell(currentNode.position, true).SetColor(Color.red);
            }
            return null;
        }
        
        public static List<PathNode> GetNeighbours(PathNode currentNode, Func<Vector2Int, bool> isAvailable)
        {
            List<PathNode> neighbours = new List<PathNode>();
            
            // Get the neighbours of the current node
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // If the neighbour is the current node, skip it, or corners that's not allowed
                    if (Mathf.Abs(x) == Mathf.Abs(y))
                    {
                        continue;
                    }
                    // Get the neighbour position
                    Vector2Int neighbourPosition = new Vector2Int(currentNode.position.x + x, currentNode.position.y + y);
                    
                    // If the neighbour is not walkable, skip it
                    if (!isAvailable(neighbourPosition))
                    {
                        continue;
                    }
                    
                    // Get the neighbour node
                    PathNode neighbourNode = new PathNode(neighbourPosition);
                    
                    // Add the neighbour node to the neighbours list
                    neighbours.Add(neighbourNode);
                }
            }
            
            return neighbours;
        }
        
        public static List<PathNode> ReconstructPath(PathNode currentNode)
        {
            List<PathNode> path = new List<PathNode>();
            while (currentNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }
        
        public static float HeuristicCost(Vector2Int a, Vector2Int b)
        { 
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) * STRAIGHT_DISTANCE;
        }
    }
}