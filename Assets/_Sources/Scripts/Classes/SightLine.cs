using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Classes
{
    public class SightLine
    {
        
        public static List<Vector2Int> Bresenham(Vector2Int posA,Vector2Int posB) {
            Vector2Int size = posB - posA;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0 ;
            List<Vector2Int> cells = new List<Vector2Int>();
            if (size.x<0) dx1 = -1 ; else if (size.x>0) dx1 = 1 ;
            if (size.y<0) dy1 = -1 ; else if (size.y>0) dy1 = 1 ;
            if (size.x<0) dx2 = -1 ; else if (size.x>0) dx2 = 1 ;
            int longest = Math.Abs(size.x) ;
            int shortest = Math.Abs(size.y) ;
            if (!(longest>shortest)) {
                longest = Math.Abs(size.y) ;
                shortest = Math.Abs(size.x) ;
                if (size.y<0) dy2 = -1 ; else if (size.y>0) dy2 = 1 ;
                dx2 = 0 ;            
            }
            int numerator = longest >> 1 ;
            for (int i=0;i<=longest;i++) {
                cells.Add(posA);
                numerator += shortest ;
                if (!(numerator<longest)) {
                    numerator -= longest ;
                    posA.x += dx1 ;
                    posA.y += dy1 ;
                } else {
                    posA.x += dx2 ;
                    posA.y += dy2 ;
                }
            }
            return cells;
        }

        // Fonction LDV basée sur le pseudo code de la fonction LDV au dessus
        public static bool HasSightLine(Vector2Int pointA, Vector2Int pointB, Func<Vector2Int, bool> isObstacle)
        {
            Debug.Log("From " + pointA + " to " + pointB);
            foreach (Vector2Int cell in Bresenham(pointA, pointB))
            {
                if (isObstacle(cell))
                {
                    return false;
                }
            }
            return true;
        }
    }
}