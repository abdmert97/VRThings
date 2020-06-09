using TMPro;
using UnityEditor;
using UnityEngine;

namespace Editors
{
#if UNITY_EDITOR
    public class CardEditor : EditorWindow
    {
        public GameObject cards;

        public GameObject cardPrefab;

        public int toolbarIndex = 0;
        public string[] toolbarOptions = new string[] { "Card Creation", "Deck Creation" };

        // For card creation
        public string cardTitle = "Title";
        public int cardCost = 0;
        public string cardText = "Text";
        public Sprite cardCover;
        public Sprite cardFace;
        public Sprite cardImage;
        public Sprite cardSlot;
        public Vector2 cardImageSize = new Vector2(0.38f, 0.4f);
        public string prefabName = "CardPrefab";

        // For deck creation
        public Sprite deckCover;
        public Sprite deckFace;
        public Sprite deckSlot;
        public float deckSpacing = 0.105f;
        public int deckSize = 10;


        [MenuItem("Window/Custom Editors/Card Editor")]
        static void Init()
        {
            CardEditor window = (CardEditor)EditorWindow.GetWindow(typeof(CardEditor));
            window.Show();
        }

        void OnGUI()
        {
        
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            cardPrefab = (GameObject)EditorGUILayout.ObjectField(new GUIContent("CardPrefab", "Prefab for the cards to be created " +
                                                                                              "- Prefabs should be in the provided format"), cardPrefab, typeof(GameObject), false);
            // Check if provided object is a prefab
            if (cardPrefab == null || PrefabUtility.GetPrefabAssetType(cardPrefab) != PrefabAssetType.Regular)
            {
                cardPrefab = null;
            }

            toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarOptions);
        
            if (toolbarIndex == 0)
            {
                GUILayout.Label("Card Settings", EditorStyles.boldLabel);

                cardTitle = EditorGUILayout.TextField("Title", cardTitle);
                cardCost = EditorGUILayout.IntField("Cost", cardCost);
                cardText = EditorGUILayout.TextField("Text", cardText);
                cardCover = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Cover", "The backside sprite of card"), cardCover, typeof(Sprite), true);
                cardFace = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Face", "The template of card, frontside sprite of card"), cardFace, typeof(Sprite), true);
                cardSlot = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Slot", "The script for the slot to display cost of card"), cardSlot, typeof(Sprite), true);
                cardImage = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Image", "The artwork of the card"), cardImage, typeof(Sprite), true);
                cardImageSize = EditorGUILayout.Vector2Field(new GUIContent("ImageSize", "Sizing for the card artwork"), cardImageSize);
            
                if (GUILayout.Button("Create Card"))
                {
                    GenerateCard(new Vector3(0f, 0f, 0f));
                }


                prefabName = EditorGUILayout.TextField("PrefabName", prefabName);
                if (GUILayout.Button("Create Card and Prefab"))
                {
                    GenerateCard(new Vector3(0f, 0f, 0f), null, true);
                }
            }
            else if (toolbarIndex == 1)
            {
                GUILayout.Label("Card Settings", EditorStyles.boldLabel);

                deckCover = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Cover", "The backside sprite of card - Applied to all cards in deck"), deckCover, typeof(Sprite), true);
                deckFace = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Face", "The template of card, frontside sprite of card - Applied to all cards in deck"), deckFace, typeof(Sprite), true);
                deckSlot = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Slot", "The script for the slot to display cost of card - Applied to all cards in deck"), deckSlot, typeof(Sprite), true);

                GUILayout.Label("Deck Settings", EditorStyles.boldLabel);

                deckSpacing = EditorGUILayout.FloatField(new GUIContent("Spacing", "The distance between each card of the deck"), deckSpacing);
                deckSize = EditorGUILayout.IntField(new GUIContent("Size", "Number of cards in the deck"), deckSize);

                if (GUILayout.Button("CreateDeck"))
                {
                    GameObject deck = new GameObject("Deck");
                    Vector3 location = new Vector3(0f, 0f, 0f);
                    for (int i = 0; i < deckSize; i++)
                    {
                        GenerateCard(location, deck);
                        location.z += deckSpacing;
                    }
                }
            }
        
        }

        private void GenerateCardPrefab(GameObject card)
        {
            string localPath = "Assets/Prefabs/Cards/" + prefabName + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(card, localPath, InteractionMode.UserAction);
        }

        private void GenerateCard(Vector3 location, GameObject deck = null, bool createPrefab = false)
        {
            GameObject card;
            card = Instantiate((GameObject)cardPrefab, location, Quaternion.identity);

            if(!cards)
            {
                cards = new GameObject("Cards");
            }

            if (deck)
            {
                deck.transform.SetParent(cards.transform);
                card.transform.SetParent(deck.transform);
            }
            else
            {
                card.transform.SetParent(cards.transform);
            }


            TextMeshPro[] textMeshPros = card.GetComponentsInChildren<TextMeshPro>();
            foreach (TextMeshPro textMeshPro in textMeshPros)
            {
                if (deck)
                {
                    if (string.Equals(textMeshPro.name, "Title"))
                    {
                        textMeshPro.text = "Title";
                    }
                    else if (string.Equals(textMeshPro.name, "Text"))
                    {
                        textMeshPro.text = "Text";
                    }
                    else if (string.Equals(textMeshPro.name, "SlotText"))
                    {
                        textMeshPro.text = "0";
                    }
                }
                else
                {
                    if (string.Equals(textMeshPro.name, "Title"))
                    {
                        textMeshPro.text = cardTitle;
                    }
                    else if (string.Equals(textMeshPro.name, "Text"))
                    {
                        textMeshPro.text = cardText;
                    }
                    else if (string.Equals(textMeshPro.name, "SlotText"))
                    {
                        textMeshPro.text = cardCost.ToString();
                    }
                }
                
            }

            SpriteRenderer[] sprites = card.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < sprites.Length; i++)
            {
                if (deck)
                {
                    if (string.Equals(sprites[i].name, "Backside") && deckCover)
                    {
                        sprites[i].sprite = deckCover;
                    }
                    else if (string.Equals(sprites[i].name, "Frontside") && deckFace)
                    {
                        sprites[i].sprite = deckFace;
                    }
                    else if (string.Equals(sprites[i].name, "Slot") && deckSlot)
                    {
                        sprites[i].sprite = deckSlot;
                    }
                }
                else
                {
                    if (string.Equals(sprites[i].name, "Backside") && cardCover)
                    {
                        sprites[i].sprite = cardCover;
                    }
                    else if (string.Equals(sprites[i].name, "Frontside") && cardFace)
                    {
                        sprites[i].sprite = cardFace;
                    }
                    else if (string.Equals(sprites[i].name, "Image") && cardImage)
                    {
                        sprites[i].sprite = cardImage;
                        sprites[i].drawMode = SpriteDrawMode.Sliced;
                        sprites[i].size = cardImageSize;
                    }
                    else if (string.Equals(sprites[i].name, "Slot") && cardSlot)
                    {
                        sprites[i].sprite = cardSlot;
                    }
                }
            
            }
            if(createPrefab)
            {
                GenerateCardPrefab(card);
            }
        }
    }
    #endif
}
