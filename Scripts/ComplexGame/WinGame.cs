using UnityEngine;

namespace ComplexGame
{
    public class WinGame : MonoBehaviour
    {

        
        public bool CheckWinner(Player player)
        {
            if (player.houseCount == 15)
            {
                return true;
            }
            return false;
        }
    }
}
