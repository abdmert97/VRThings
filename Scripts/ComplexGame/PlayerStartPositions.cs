using UnityEngine;

namespace ComplexGame
{
    public class PlayerStartPositions 
    {
        public static void CalculateDeckPositions(GameManager gameManager)
        {
            /*   
             *    This map is used to place players deck to
             *    the map.
             *    e_i show edges and v_j show vertices.
             *    
             *     
             *            v1 e1 v2
             *            --------
             *       e0  /        \ e2 
             *       v0 /          \ 
             *          \          / v3
             *       e5  \        / e3
             *            --------
             *            v5  e4 v4
             */
            gameManager.hexMap.Init();
            gameManager.hexMap.Scale(3);
            Bounds bound =BoundCalculator.CalculateCumulativeBounds(gameManager.hexMap.gameObject);    
            
            var v0 = CalculateVertices(bound, out var v1, out var v2, out var v3, out var v4, out var v5);

            float y = bound.center.y;
            float offset =8;  // It will be fixed 
            
            var edges = CalculateEdges(v0, v1, v2, v3, v4, v5);
            
            float[] angles6Player = {-240,180,240,-60,0,60 };
            var positions6Player = new[] {edges[0], edges[1], edges[2], edges[3], edges[4], edges[5]};
            float[] angles4Player = {-270,-180,-90,0};
            var positions4Player = new[] {v0, edges[1], v3, edges[4]};
            float[] angles = new float[]{};
            Vector3[] positions = new Vector3[] {};
            switch (gameManager.gameData.PlayerCount)
            {
                case 4:
                    angles = angles4Player;
                    positions = positions4Player;
                    break;
                case 6:
                    angles = angles6Player;
                    positions = positions6Player;
                    break;
            }

            for (int i = 0; i <gameManager.gameData.PlayerCount ; i++)
            {
                gameManager.deckTransformObjects[i] = new GameObject("Edge "+i.ToString());
                gameManager.deckTransformObjects[i].transform.SetParent(gameManager.transform);
                gameManager.deckTransformObjects[i].transform.rotation =Quaternion.Euler(new Vector3(0,angles[i],0));
                gameManager.deckTransformObjects[i].transform.position = positions[i];
                gameManager.deckTransformObjects[i].transform.position -= gameManager.deckTransformObjects[i].transform.forward * offset;
            }
          

        
        }

        private static Vector3[] CalculateEdges(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 v5)
        {
            Vector3 e0 = (v0 + v1) / 2;
            Vector3 e1 = (v1 + v2) / 2;
            Vector3 e2 = (v2 + v3) / 2;
            Vector3 e3 = (v3 + v4) / 2;
            Vector3 e4 = (v4 + v5) / 2;
            Vector3 e5 = (v5 + v0) / 2;
            Vector3[] edges = {e0, e1, e2, e3, e4, e5};
            return edges;
        }

        private static Vector3 CalculateVertices(Bounds bound, out Vector3 v1, out Vector3 v2, out Vector3 v3, out Vector3 v4,
            out Vector3 v5)
        {
            /*            v1 e1 v2
                *            --------
                *       e0  /        \ e2 
                *       v0 /          \ 
                *          \          / v3
                *       e5  \        / e3
                *            --------
                *            v5  e4 v4
                */
            Vector3 v0 = new Vector3();
            v0.x = bound.min.x;
            v0.y = bound.center.y;
            v0.z = bound.min.z + bound.extents.z;

            v1 = new Vector3();
            v1.x = bound.min.x + bound.extents.x * 0.5f;
            v1.y = bound.center.y;
            v1.z = bound.max.z;

            v2 = new Vector3();
            v2.x = bound.max.x - bound.extents.x * 0.5f;
            v2.y = bound.center.y;
            v2.z = bound.max.z;

            v3 = new Vector3();
            v3.x = bound.max.x;
            v3.y = bound.center.y;
            v3.z = bound.min.z + bound.extents.z;

            v4 = new Vector3();
            v4.x = bound.max.x - bound.extents.x * 0.5f;
            v4.y = bound.center.y;
            v4.z = bound.min.z;

            v5 = new Vector3();
            v5.x = bound.min.x + bound.extents.x * 0.5f;
            v5.y = bound.center.y;
            v5.z = bound.min.z;
            return v0;
        }
    }
}
