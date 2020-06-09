using System;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexGame.Cards
{
    public class Migration : Card
    {
        // TODO: player will do the first action. It is a must. The village building is optional.
        public Colonist colonist;
        public int tileToMove;
        public bool buildVillage;
        public TileType tileTypeToBuild;
        public Hex coordinateToBuild;
        public List<ParticleSystem> explorerParticles;
        private Player _player;
        private void Awake()
        {
            explorerParticles = new List<ParticleSystem>();
        }

        public override bool Play(Player player, out string message)//, int startVertex, int endVertex)
        {
            _player = player;
            player.EndTurn += StopParticleEffects;
            explorerParticles.Clear();
            foreach (var explorer in player.activeExplorers)
            {
                Vector3 position = explorer.transform.position+Vector3.up*0.2f;
                
                ParticleSystem explorerParticle  = explorer.GetComponent<ParticleSystem>();
                if (explorerParticle == null)
                {
                    explorerParticle = player.CreateParticle(GameManager.Instance.gameData.ExplorerInteractPrefab);
     
                    explorerParticle.transform.SetParent(player.transform);
              
                }
                explorerParticles.Add(explorerParticle);
                explorerParticle.gameObject.transform.position = position;
                explorerParticle.Play();
            }
            player.MoveCamera(true);
            player.playerInteract.SetInteractState(PlayerInteract.InteractState.HexAndExplorer);
            InGameUIManager.Instance.OpenInfoPanel(null,"MoveCapacity","Move remained: "+player.playerInteract.moveCapacity.ToString());
        /*   int vertexID;
            colonist.GetVertexCoordinate(player.playerCamera, out vertexID);
            if(colonist.Move(vertexID, tileToMove, GameManager.Instance.GetCurrentPlayerNumber()))
            {
                if(buildVillage)
                {
                    if(!GameManager.Instance.hexMap.BuildHouse(tileTypeToBuild, GameManager.Instance.GetCurrentPlayerNumber(), coordinateToBuild))
                    {
                        message = "Building village failed";
                        return false;
                    }
                }
                
                message = "Card play successful";
                player.discardPile.AddCard(this);
                player.deck.RemoveCard(this);
                return true;
            }
            else
            {
                message = "Colonist movement failed";
                return false;
            }*/
        message = " ";
        return true;
        }
        void StopParticleEffects()
        {
            _player.EndTurn -= StopParticleEffects;
            foreach (var particle in explorerParticles)
            {
                if (particle.isPlaying)
                {
                    particle.Stop();
                }
            }
        }
    }
    
}
