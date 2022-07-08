using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;


namespace Manager
{
    public class Game : MonoBehaviour
    {
        #region Singleton
        public static Game Instance;
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
            Grid.Instance.OnCellClicked.AddListener((v2) => print(v2));
        }
        
    }
}