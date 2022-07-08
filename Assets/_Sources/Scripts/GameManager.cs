using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of MapManager found!");
            return;
        }
        Instance = this;
    }
    #endregion
    
    public int GridSize = 9;
    public int Maximum
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
    
    [SerializeField] private Grid _grid;
    [SerializeField] public char disableChar = ' ';
    [SerializeField] public List<Entity> entities = new List<Entity>();
    public int currentEntityPlaying = 0;
    
    private void Start()
    {
        // Load json from StreamingAssets folder
        string json = Resources.Load<TextAsset>("00").text;
        // Parse json
        JSONNode node = JSON.Parse(json);
        LoadMap(node);
    }
    
    public void NextEntity()
    {
        currentEntityPlaying++;
        currentEntityPlaying %= entities.Count;
        
        entities[currentEntityPlaying].StartTurn();
    }
    
    public void LoadMap(JSONNode json)
    {
        //                 if char is not disableChar, tile is interactable
        _grid.LoadMap(json, c => c != disableChar);
        
        _grid.OnCellClicked.AddListener((v2) => print(v2));
    }

    public static List<List<char>> GetReachableCells(int range)
    {
        List<List<char>> result = new List<List<char>>();
        for (int y = -range; y < range+1; y++)
        {
            string line = "";
            print((2 * range + 1) - Math.Abs(y)*2);
            line += new String(' ', Math.Abs(y));
            line += new String('X', (2 * range + 1) - Math.Abs(y)*2);
            line += new String(' ', Math.Abs(y));
            
            result.Add(line.ToList());
        }

        return result;
    }
}
