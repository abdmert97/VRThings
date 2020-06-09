using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame
{
    public class Player : MonoBehaviour
    {

        #region Events
        public event Action EnableDeck;
        public event Action ActivateInteractables;
        public event Action InterActionEnd;
        public event Action<InputInfo> HandleInput;
        public event Action Play;
        public Action EndTurn;
        public Action ActivateDiscardPile;
        public Action DeActivateDiscardPile;
        
        #endregion

        #region publics
        public int ActionCount { get; private set; }
        public int PlayerNumber { get; private set; }
        public Deck deck;
        public int DeckPosition { get; set; }
        public int explorerCount { get; set; }
    
        public ResourceAmounts storage;
        public Storehouse storehouse;
        public Camera playerCamera;
        public Deck discardPile;
        public DiscardPile discardPileClass;
        public PlayerInteract playerInteract;
        public GameObject items;
        public int explorerInMap = 0;
        public int houseCount = 0;
        public List<GameObject> activeExplorers;
        #endregion

        #region privates
        private GameObject _playerDeck;
        private GameObject _discardPile;
       
        private Vector3 _cameraPosition;
        private Vector3 _cameraAngle;
        private Hexmap _hexmap;
        #endregion
       
        private void Awake()
        {
            GameManager.Instance.playerList.Add(this);
            PlayerNumber = GameManager.NumberofPlayers++;
            name = "Player " + PlayerNumber.ToString();
         
            _hexmap = GameManager.Instance.hexMap;
            //MAK: create storehouse
            storehouse = gameObject.AddComponent<Storehouse>() as Storehouse;
            storehouse.SetDefaultResources();
            playerInteract = new PlayerInteract();
            playerInteract.SetPlayer((this));
            playerInteract.hexmap = _hexmap;
            //    ShowCards();
            Play += Turn;
           
            EndTurn += DisableCamera;
            EndTurn += ResetDeckTransform;
            EndTurn += ResetCamera;
            EnableDeck += ActivateDeck;
            ActivateInteractables += EnableInteractables;
          
            HandleInput += GetInput;
            
            transform.position = GameManager.Instance.deckTransformObjects[PlayerNumber].transform.position+
                                    Vector3.up*GameManager.Instance.gameData.PlayerHeight;
            transform.LookAt(GameManager.Instance.hexMap.transform);
            playerCamera.transform.localPosition-= Vector3.forward*16;

            _cameraPosition = playerCamera.transform.position;
            if(VRManager.instance)
            {
                _cameraPosition = playerCamera.transform.position + playerCamera.transform.forward * VRManager.instance.camera_offset;
                playerCamera.transform.position = _cameraPosition;
                playerCamera.enabled = false;

                Vector3 camRotation = playerCamera.transform.rotation.eulerAngles;
                camRotation.x = 0;
                playerCamera.transform.eulerAngles = camRotation;
            }
            _cameraAngle = playerCamera.transform.rotation.eulerAngles;
                        
            CreateStarterDeck();
            CreatePileDeck();
            deck.ShowDeck(Vector3.down * 0.65f);
            CreateItems();
         
            Invoke(nameof(AddInteractEvent), 0.1f);
            Invoke(nameof(AddEndInteractEvent), 0.1f);


            // Create storage
            storage = new ResourceAmounts();
            storage.SetAmountsToPlayerDefault();
        }

        private void Start()
        {
            GameManager.PlayerJoin.Invoke(gameObject);
        }

        private void CreateItems()
        {
            Transform lastCard =  deck.cards[deck.cards.Count-1].gameObject.transform;
            items = new GameObject("items");
            Vector3 position = lastCard.transform.position;
            items.transform.position = position;
            items.transform.SetParent(transform);
            items.transform.localPosition+= Vector3.right*3;
            for (int i = 0; i < 5; i++)
            {
               GameObject explorer =  GameManager.Instance.itemBuilder.CreateExplorer(items.transform, Vector3.zero + transform.right * i,this);
               
            }

            foreach (var vertice in _hexmap.startVertices)
            {
                string result;
                var explorer = items.transform.GetChild(0).gameObject;
                if(_hexmap.PlaceExplorer(vertice,PlayerNumber,explorer,out result,false))
                {
                    explorerInMap++;
                    activeExplorers.Add(explorer);
                    
                    break;
                }
            }
            
        }

        private void ResetCamera()
        {
            playerCamera.transform.position = _cameraPosition;
            playerCamera.transform.rotation = Quaternion.Euler(_cameraAngle);
        }

        
        public void EnableInteractables()
        {
            // TO DO
        }
        public void ActivateDeck()
        {
            deck.ShowDeck(Vector3.down * 0.65f);
        }
        public void Turn()
        {
            Debug.Log("Turn For " + name);
            PlayerAction();
            DisplayResources();

            //LookCloseToDeck();
        }

        private void DisplayResources()
        {
            InGameUIManager.Instance.OpenResourcePanel(storage);
        }

        public ParticleSystem CreateParticle(GameObject particle)
        {
            return Instantiate(GameManager.Instance.gameData.ExplorerInteractPrefab).GetComponent<ParticleSystem>();
        }
        public void InvokePlay()
        {
            Play?.Invoke();
        }
        public void InvokeEndTurn()
        {
            EndTurn?.Invoke();
        }
        public void InvokeHandleInput(InputInfo obj)
        {
            HandleInput?.Invoke(obj);
        }
        public void InvokeEnableDeck()
        {
            EnableDeck?.Invoke();
        }
        public void InvokeActivateInteractables()
        {
            ActivateInteractables?.Invoke();
        }    
        /// <summary>
        /// <param name="direction"> True shows Looking down to the map,
        /// false shows looking horizontally </param>
        /// </summary>
        public void MoveCamera(bool direction)
        {
            StartCoroutine(CameraMovement(direction));
        }
        
     
        private void AddInteractEvent()
        {
            Interactable.InteractWithObject[PlayerNumber] += playerInteract.InteractWith;
        }
        private void AddEndInteractEvent()
        {
            Interactable.EndInteractWithObject[PlayerNumber] += playerInteract.EndInteractWith;
        }
        private void ResetDeckTransform()
        {
            // deck.ResetDeckTransform();
        }
        private void GetInput(InputInfo inputInfo)
        {
            if (inputInfo.hit!=null)
            {
                // SelectedObject
            }
        
        }
        private void PlayerAction()
        {
            playerCamera.enabled = true;
            GameManager.Instance.interactable.currentCamera = playerCamera;
        }
        private void DisableCamera()
        {
            playerCamera.enabled = false;
    
        }
        private void CreateStarterDeck()
        {
            List<GameObject> players = new List<GameObject>();
            players.Add(gameObject);
            Transform deckTransform = GameManager.Instance.deckTransformObjects[PlayerNumber].transform;
            _playerDeck = new GameObject("Deck"+ PlayerNumber.ToString());
            _playerDeck.transform.position = deckTransform.position ;
            _playerDeck.transform.rotation = deckTransform.transform.rotation;
           
            _playerDeck.transform.SetParent(transform);
            deck = GameManager.Instance.deckBuilder.GetStarterDeck(_playerDeck.transform,players);

        }
        private void CreatePileDeck()
        {
            Transform deckTransform = GameManager.Instance.deckTransformObjects[PlayerNumber].transform;
          
          
            _discardPile = GameManager.Instance.deckBuilder.GetDiscardPile();
            _discardPile.transform.position = deckTransform.position  + deckTransform.forward*5;
            _discardPile.transform.rotation = deckTransform.transform.rotation;
            _discardPile.transform.Rotate(Vector3.right*90);
           
            _discardPile.transform.SetParent(transform);
            _discardPile.name = "Discard Pile "+ PlayerNumber.ToString();
            discardPile = _discardPile.GetComponent<Deck>();
            discardPileClass = _discardPile.GetComponent<DiscardPile>();
            discardPileClass.player = this;
            discardPileClass.discardDeck = discardPile;
        }
        IEnumerator CameraMovement(bool forward)
        {
           
            Transform target = GameManager.Instance.cameraTopPoint;
            int iteration = 60;
            int direction = forward ? 1 : -1;
            Vector3 position = playerCamera.transform.position;
            Vector3 distance = direction * (target.position - position) / iteration;
             float timeCount = 1f/60f;
             var rotation = playerCamera.transform.rotation;
             target.rotation = Quaternion.Euler(new Vector3(90,rotation.eulerAngles.y,rotation.eulerAngles.z));
             yield return  new WaitForSeconds(.5f);

             for (int i = 0; i < iteration; i++)
            {
                playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, target.rotation, timeCount);
              
                timeCount += Time.fixedDeltaTime/5;
                playerCamera.transform.position += distance;
                yield return  new WaitForFixedUpdate();

            }
           
               

        }
    }

}