using System;
using System.Collections.Generic;
using Array2DEditor;
using UnityEngine;

namespace Utils
{
    public static class Maths
    {
        
        /// <summary>
        /// Convert Array2D<T> to List<List<R>>
        /// </summary>
        /// <param name="array">The default array</param>
        /// <param name="transformer">Function to transform R to T</param>
        /// <typeparam name="R">Array2D type</typeparam>
        /// <typeparam name="T">Nested list type</typeparam>
        /// <returns></returns>
        public static List<List<R>> Get2DList<R, T>(this Array2D<T> array, Func<T, R> transformer = null)
        {
            if (transformer == null) transformer = (x) => (R)Convert.ChangeType(x, typeof(R));
            
            List<List<R>> result = new List<List<R>>();
            for (int x = 0; x < array.GridSize.x; x++)
            {
                List<R> row = new List<R>();
                for (int y = 0; y < array.GridSize.y; y++)
                {
                    row.Add(transformer(array.GetCell(x, (array.GridSize.y-1)-y))); // Invert y axis to match Array2DEditor convention
                }
                result.Add(row);
            }
            return result;
        }
    }
}