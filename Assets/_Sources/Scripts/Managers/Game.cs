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
        
        [SerializeField] public List<Entity> entities = new List<Entity>();
        public int currentEntityPlaying = -1; // Index of the entity currently playing

        public void Start()
        {
            NextEntity(0);
        }

        public void NextEntity(int index = -1)
        {
            Grid.Instance.Clear();
            if (index == -1)
            { 
                currentEntityPlaying++;
                currentEntityPlaying %= entities.Count;
            }
            else currentEntityPlaying = index;
            
            entities[currentEntityPlaying].StartTurn();
        }
        
        
    }
}