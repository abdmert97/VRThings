using System.Collections.Generic;
using UnityEngine;

namespace UIManager
{
    public class ListGames : MonoBehaviour
    {

        [SerializeField] List<RectTransform> GamesList = new List<RectTransform>();
        [SerializeField] RectTransform panel;
        [SerializeField] Vector2 iconSize;
        [SerializeField] Vector2 anchor;
        [SerializeField] float iconDistance;
    
        private Camera cam;
        private Rect panelRect;
        private void Start()
        {
            cam = Camera.main;
            panelRect = panel.rect;
            GetGameList();
        }
        public void GetGameList()
        {
            int gameCount = GamesList.Count;
            int iconCountInRow = (int)((panelRect.width) / (iconSize.x+iconDistance));
          
            int columnCount = gameCount / iconCountInRow ;
            if (gameCount % iconCountInRow!=0) columnCount += 1;
            if (iconCountInRow > gameCount) iconCountInRow = gameCount;
      
            for(int column  = 0; column<columnCount ; column++)
            {
                for(int row = 0; row < iconCountInRow; row++)
                {
                    if (row + column * iconCountInRow >= gameCount) continue;
                    RectTransform selectedGame = GamesList[row + column * iconCountInRow];
                    selectedGame.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,iconSize.x );
                    selectedGame.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, iconSize.y);

                    selectedGame.localPosition = (panelRect.height/2) * Vector2.up + (panelRect.width / 2) * Vector2.left
                                                                                   + anchor + Vector2.right * (row + 0.5f) * iconSize.x+ Vector2.down * (column+0.5f) * iconSize.y
                                                                                   +Vector2.right*iconDistance*(row)+Vector2.down* iconDistance*(column);
                }
            }


     

        }
    }
}
