using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace ComplexGame
{
    public class GameManager : MonoBehaviour
    {

        #region Events
        // This event will be called by Network system when Game started
        public static event Action GameStarted;
        // This event will be used by Netwrok when Game Ended. GameManager will called this event
     

        public static event Action GameEnded;
        public static event Action GamePaused;
        // This event will be called when player join the game by Network system
        public static Action<GameObject> PlayerJoin;
       
        #endregion

        public List<ParticleSystem> tileParticleSystems;
        public List<ParticleSystem> vertexParticleSystem;
        public static int NumberofPlayers;
        public static GameManager Instance { get; private set; }
        public Interactable interactable;
        public ItemBuilder itemBuilder;
        public GameObject[] deckTransformObjects; // Hexagonal map consist of 6 edges
        public DeckBuilder deckBuilder;
        public GameData gameData;
        public TileData tileData;
        public Hexmap hexMap;
        public Transform cameraTopPoint;
        public ShopPanel shopPanel;
        public List<Player> playerList = new System.Collections.Generic.List<Player>();
      
        
        private TurnManager _turnManager;
        private WinGame _winGame;
        
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            deckTransformObjects = new GameObject[6];
            NumberofPlayers = 0;
            _turnManager = FindObjectOfType<TurnManager>();
            
            vertexParticleSystem = new List<ParticleSystem>();
            tileParticleSystems = new List<ParticleSystem>();
            gameData = Instantiate(gameData);
            tileData = Instantiate(tileData);
            // Place players to suitable positions
            PlayerStartPositions.CalculateDeckPositions(this);
        }

        private void Start()
        {
            CloseTileEffect();
            CloseVertexEffect();
            
        }

        public int GetCurrentPlayerNumber()
        {
            return _turnManager.TurnFor;
        }
        public GameObject GetCurrentPlayer()
        {
            return playerList[_turnManager.TurnFor].gameObject;
        }

        private void AddPlayer(GameObject playerObject)
        {
            playerList.Add(playerObject.GetComponent<Player>());
        }
        public void OpenTileEffect()
        {
         
            foreach (var particle in tileParticleSystems)
            {
                particle.enableEmission = true;
            }
        }
        public void CloseTileEffect()
        {
            foreach (var particle in tileParticleSystems)
            {
                particle.enableEmission = false;
            }
        }
        public void OpenVertexEffect()
        {
         
            foreach (var particle in vertexParticleSystem)
            {   
                particle.Play();
            }
        }
        public void CloseVertexEffect()
        {
            foreach (var particle in vertexParticleSystem)
            {
                particle.Stop();
            }
        }
    
        public void EndTurn()
        {
            _turnManager.NextTurn();
        }
    }
}