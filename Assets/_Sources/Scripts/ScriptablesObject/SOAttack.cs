using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", order = 1)]
public class SOAttack : ScriptableObject
{
    public string attackName;
    public int attackDamage;
    public int attackCost;

    public char disabledChar;
    
    [TextArea]
    public string attackPattern;

    public List<List<char>> attackPatternList
    {
        get
        {
            // Loop over each line of the string
            List<List<char>> attackPatternList = new List<List<char>>();
            foreach (string line in attackPattern.Split('\n'))
            {
                // Loop over each character in the line
                List<char> lineList = new List<char>();
                foreach (char c in line)
                {
                    char value = c;
                    if (value == disabledChar)
                    {
                        value = GameManager.Instance.disableChar;
                    }
                    lineList.Add(value);
                }
                attackPatternList.Add(lineList);
            }
            return attackPatternList;
        }
    }
}