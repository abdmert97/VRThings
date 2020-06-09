using UnityEngine;

namespace ComplexGame
{
    public class Colonist : MonoBehaviour
    {
        //Get VertexID
        // Places the colonist to a map tile
        // Colonists can only be placed on starting tile or a tile with player's village
        // TODO: Implement
        // 
        public bool Place(int vertexID,int activePlayerID )
        {
            string result;
            GameObject vertex = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bool returnVal = GameManager.Instance.hexMap.PlaceExplorer(vertexID, activePlayerID , vertex,out result);
            Destroy(vertex);
            return returnVal;
        }

        // Moves the colonist to a map tile
        // Colonists cannot pass other players' explorers
        // TODO: Implement
        public bool Move(int startVertexID,int endVertexID,int activePlayerID )
        {
            string result;
            return false;
            // return GameManager.Instance.hexMap.MoveExplorer(null,startVertexID, endVertexID, activePlayerID,out result );
        }

        public bool Place(int vertexID,int activePlayerID , out string result)
        {
            GameObject vertex = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bool returnVal = GameManager.Instance.hexMap.PlaceExplorer(vertexID, activePlayerID , vertex,out result);
            Destroy(vertex);
            return returnVal;
        }

        // Moves the colonist to a map tile
        // Colonists cannot pass other players' explorers
        // TODO: Implement
        public bool Move(int startVertexID,int endVertexID,int activePlayerID,out string result )
        {
            result = " ";
            return false;
            //  return GameManager.Instance.hexMap.MoveExplorer(null,startVertexID, endVertexID, activePlayerID,out result );
        }
    }
}
