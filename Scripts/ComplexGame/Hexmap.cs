using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;


namespace ComplexGame
{
    public class Hexmap : MonoBehaviour
    {
        public int depth;
        public int regionCount = 4;
        public Color borderColor;
        public Material defaultVertexMaterial;
        public GameObject tileParticleEffect; // Show non-desert Tiles 
        public GameObject vertexParticleEffect;
        public ParticleSystem constructionParticle;
        private HexTile[,] hexTiles;

        private List<Vertex> vertices;
        private List<int>[] indices;
        private const int INF = 999;

        private GameObject hexMap;
        private GameObject buildings;
        private GameObject explorers;


        private GameObject house;
        private List<GameObject> Desert;
        private List<GameObject> Lake;
        private List<GameObject> Field;
        private List<GameObject> Forest;
        private List<GameObject> Mountain;
        private List<GameObject> Hill;

        public Hex startHex;
        public HexTile startHexTile;
        public List<int> startVertices;
        private int numberofPlayers;
        private float scale = 1f;

        private bool[] regionActivationStatus;
        private GameObject[] allRegions;
        public List<Material> playerMaterials;
        private List<Interact> interactList;

        private Animator _peasantAnimator;
        // Hex-Map generation
        // Tüm tilelar şuanlık desert şeklide ve regionlar halkalar şeklinde
        // Regionların durumu ve tiletypeların durumu randomize edilecek


        private void Awake()
        {
            interactList = new List<Interact>();
            GameManager.PlayerJoin += AddPlayer;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        public void Init()
        {
            Desert = GameManager.Instance.tileData.DesertPrefab;
            Lake = GameManager.Instance.tileData.LakePrefab;
            Field = GameManager.Instance.tileData.FieldPrefab;
            Forest = GameManager.Instance.tileData.ForestPrefab;
            Mountain = GameManager.Instance.tileData.MountainPrefab;
            Hill = GameManager.Instance.tileData.HillPrefab;
            house = GameManager.Instance.tileData.HousePrefab;

            numberofPlayers = GameManager.Instance.gameData.PlayerCount;

            regionActivationStatus = new bool[regionCount];
            for (int i = 0; i < regionCount; i++) regionActivationStatus[i] = false;


            GenerateHexMap();
            GameObject vertex = GameObject.CreatePrimitive(PrimitiveType.Cube);

            GenerateVertex(vertex);
            Destroy(vertex);
        }

        private void GenerateHexMap()
        {
            int layer = LayerMask.NameToLayer("Hex");

            hexTiles = new HexTile[depth * 2 + 1, depth * 2 + 1];

            hexMap = gameObject;
            Transform hexMapTransform = hexMap.transform;

            buildings = new GameObject("Buildings");
            buildings.transform.parent = hexMapTransform;
            explorers = new GameObject("Explorers");
            explorers.transform.parent = hexMapTransform;

            int hexPrefabSize = regionCount;
            GameObject[] regions = new GameObject[hexPrefabSize];
            Transform[] regionsTransform = new Transform[hexPrefabSize];
            allRegions = new GameObject[hexPrefabSize];
            for (int i = 0; i < hexPrefabSize; i++)
            {
                regions[i] = new GameObject("Region/" + i.ToString());
                allRegions[i] = regions[i];
                regions[i].transform.parent = hexMapTransform;
                regionsTransform[i] = regions[i].transform;
            }

            Randomizer randomizer = new Randomizer(depth, hexPrefabSize);

            Hex currentHex = new Hex(-1, 0);
            Vector3 currentPosition = new Vector3(-1, 0, 0);

            float cos60 = Mathf.Cos(60 * Mathf.Deg2Rad);
            float sin60 = Mathf.Sin(60 * Mathf.Deg2Rad);

            float size = 1f;

            Vector3[] degrees = new Vector3[6];
            degrees[0] = new Vector3(1, 0, 0) * size;
            degrees[1] = new Vector3(cos60, 0, sin60) * size;
            degrees[2] = new Vector3(-cos60, 0, sin60) * size;
            degrees[3] = new Vector3(-1, 0, 0) * size;
            degrees[4] = new Vector3(-cos60, 0, -sin60) * size;
            degrees[5] = new Vector3(cos60, 0, -sin60) * size;

            GameObject temp;
            Hex lastHex;
            for (int i = 0; i <= depth; i++)
            {
                currentHex = currentHex.GetNeighbor(0);
                currentPosition += degrees[0];
                lastHex = currentHex;
                
                int index = randomizer.GetRandomRegion(currentHex);
                TileType tileType = randomizer.GetRandomTileType(currentHex);

                //temp = Object.Instantiate(hexPrefab[index],currentPosition,Quaternion.identity);
                temp = Object.Instantiate(GetTile(tileType), currentPosition, Quaternion.identity);
                temp.transform.localScale = BoundCalculator.SetUnitScale(temp, 1, -1, Mathf.Sqrt(3) / 2);
                temp.transform.Rotate(Vector3.up * 30);
                temp.name = currentHex.ToAxial();
                if (currentHex.q == 0 && currentHex.r == 0 && currentHex.r == 0)
                    startHex = currentHex;
                temp.transform.parent = regionsTransform[index];
                this[currentHex] = new HexTile(TileType.Desert, currentPosition, numberofPlayers);
                this[currentHex].regionID = index;
                this[currentHex].tileType = tileType;
                SetLayerRecursively(temp, layer);
                temp.layer = layer;
                if (currentHex.q == 0 && currentHex.r == 0 && currentHex.r == 0)
                {
                    startHex = currentHex;
                    startHexTile = this[currentHex];
                    startVertices = startHexTile.vertices;
                }
                if (tileType != TileType.Desert)
                {
                    temp.tag = "Hex";
                    AddInteraction(temp);
                    CreateTileParticleEffect(temp);
                }

                for (int j = 2; j <= 7; j++)
                {
                    for (int k = 0; k < i; k++)
                    {
                        currentHex = currentHex.GetNeighbor(j % 6);
                        currentPosition += degrees[j % 6];

                        if ( currentHex == lastHex) continue;
                        index = randomizer.GetRandomRegion(currentHex);
                        tileType = randomizer.GetRandomTileType(currentHex);

                        //temp = Object.Instantiate(hexPrefab[index],currentPosition,Quaternion.identity);
                        temp = Object.Instantiate(GetTile(tileType), currentPosition, Quaternion.identity);
                        temp.transform.localScale = BoundCalculator.SetUnitScale(temp, 1, -1, Mathf.Sqrt(3) / 2);
                        temp.transform.Rotate(Vector3.up * 30);
                        temp.name = currentHex.ToAxial();

                        temp.transform.parent = regionsTransform[index];
                        this[currentHex] = new HexTile(TileType.Desert, currentPosition, numberofPlayers);
                        this[currentHex].regionID = index;
                        this[currentHex].tileType = tileType;
                        temp.layer = layer;
                        SetLayerRecursively(temp, layer);
                        if (tileType != TileType.Desert)
                        {
                            temp.tag = "Hex";
                            AddInteraction(temp);
                            CreateTileParticleEffect(temp);
                        }
                    }
                }
            }
        }

        private void CreateTileParticleEffect(GameObject temp)
        {
            GameObject particle = Instantiate(tileParticleEffect, temp.transform);
            ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
            GameManager.Instance.tileParticleSystems.Add(particleSystem);
        }
        private void CreateVertexParticleEffect(GameObject temp)
        {
            GameObject particle = Instantiate(vertexParticleEffect, temp.transform);
            ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
            GameManager.Instance.vertexParticleSystem.Add(particleSystem);
        }


        void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        private void AddInteraction(GameObject hex)
        {
            Interact interact = hex.AddComponent<Interact>();
            interact.interactType = Interact.InteractType.Click;
            interactList.Add(interact);
        }

        private void AddPlayer(GameObject player)
        {
            foreach (var interact in interactList)
            {
                interact.InteractableWith.Add(player.gameObject);
            }
        }

        // Mouse un o anki konumundaki tile ın positionunu hex position olarak vermekte
        // Hex position reference olarak alınıp içeride değiştirilmektedir
        // Hex değil bool olarak dönmesinin nedeni mouse un o anki pozisyonunda, kullanıcı board un üzerinde olmayabilir o yüzden dönen hex anlamsız olabilir
        // Gameplayde kullanılırken fonksiyon true döniyorsa pozisyon kullanılmalıdır.
        // Dönülen hex pozisyonu "BuildHouse" gibi hex position parametresi isteyen functionlarda kullanılabilir.
        public Hex GetHexCoordinate(GameObject hit)
        {
            Hex hex = Hex.AxialToHex(hit.name);
            return hex;
        }

        // Mouse un o anki konumundaki region ın positionunu vermekte
        // Region position int reference olarak alınıp içeride değiştirilmektedir
        // Int değil bool olarak dönmesinin nedeni mouse un o anki pozisyonunda, kullanıcı board un üzerinde olmayabilir o yüzden dönen region anlamsız olabilir
        // Gameplayde kullanılırken fonksiyon true döniyorsa region kullanılmalıdır.
        // Dönülen region pozisyonu "GetResource" gibi region posizyonu paremetresi isteyen functionanlarda kulanılaiblri. 
        public int GetRegionCoordinate(GameObject hit)
        {
                string regionName = (hit.transform.parent.gameObject.name);

                string[] split = regionName.Split('/');

               return  int.Parse(split[1]);
        }

        // Mapte bir yere bina kurulurken kullanıcak fonksiyon.
        // playerType oynanyan oyuncuları ayırmak için. Örneğin 4 oyuncu oynuyorsa playerType 0,1,2,3 olabilir.
        // result stringi reference olarak alınıp içeride değiştirilmekte
        // bina yapma başarısız olursa, result stringi actionun başarız olma nedeni kullanıcıya UI yoluyla bildirilecekse kullanılabilir.
        //
        //
        // Peasant ları tutan structure map e eklenince bina kurarken, tile'ın yanında peasent ın olup olmaması durumu eklenecek.
        public bool BuildHouse(int playerType, Hex coordinate, out string result)
        {
            HexTile hexTile = this[coordinate];

            if (hexTile.tileType == TileType.Desert)
            {
                result = "House cannot built in Dessert";
                Debug.Log(result);
                return false;
            }

            if (hexTile.isPlayerBuiltVillage[playerType])
            {
                result = "You have already built a house";
                Debug.Log(result);
                return false;
            }

            // Check Enough reserve 
            bool enoughResource = CheckResource(playerType, hexTile);


            List<int> v = hexTile.vertices;
            bool isHaveExplorer = false;

            foreach (int i in v)
            {
                if (vertices[i].IsTherePeasant() && vertices[i].playerType == playerType)
                {
                    isHaveExplorer = true;
                }
            }

            if (!isHaveExplorer)
            {
                result = "You have not a explorer in that vertex";
                Debug.Log(result);
                return false;
            }

            GameObject playerHouse = Instantiate(house);
            Debug.Log("temp inst");
            playerHouse.transform.localScale = BoundCalculator.SetUnitScale(playerHouse, 0.2f, -1f, 0.2f) *
                                               hexMap.transform.localScale.y * 2;
            playerHouse.transform.parent = buildings.transform;

            //int playerCount = GameManager.NumberofPlayers;
            int playerCount = numberofPlayers;
            float angle = ((float) playerType / playerCount) * 360;
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);

            float l = 0.35f * hexMap.transform.localScale.y;
            playerHouse.transform.position = hexTile.centerPosition + new Vector3(cos * l, 0, sin * l);
            Renderer renderer = playerHouse.GetComponent<Renderer>();
            renderer.sharedMaterial = playerMaterials[playerType];
            ParticleSystem particleSystem = playerHouse.transform.GetChild(0).GetComponent<ParticleSystem>();
            var col = particleSystem.colorOverLifetime;
            col.enabled = true;

            Gradient grad = new Gradient();
            grad.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(playerMaterials[playerType].color, 0.0f),
                    new GradientColorKey(Color.black, 1.0f)
                }, new GradientAlphaKey[] {new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f)});

            col.color = grad;
            hexTile.isPlayerBuiltVillage[playerType] = true;
            StartCoroutine(ConstructBuilding(playerHouse, 1.5f));
            result = "";
            return true;
        }

        private bool CheckResource(int playerType, HexTile hexTile)
        {
            Player player = GameManager.Instance.playerList[playerType];
            Dictionary<ResourceType, int> resourceNeeded = new Dictionary<ResourceType, int>();

            switch (hexTile.tileType)
            {
                case TileType.Lake:
                    resourceNeeded.Add(ResourceType.Food, 1);
                    resourceNeeded.Add(ResourceType.Gold, 1);
                    break;
                case TileType.Field:
                    resourceNeeded.Add(ResourceType.Water, 1);
                    resourceNeeded.Add(ResourceType.Food, 1);
                    resourceNeeded.Add(ResourceType.Gold, 2);
                    break;
                case TileType.Forest:
                    resourceNeeded.Add(ResourceType.Water, 1);
                    resourceNeeded.Add(ResourceType.Food, 1);
                    resourceNeeded.Add(ResourceType.Gold, 3);
                    break;
                case TileType.Hill:
                    resourceNeeded.Add(ResourceType.Water, 1);
                    resourceNeeded.Add(ResourceType.Stone, 1);
                    resourceNeeded.Add(ResourceType.Gold, 4);
                    break;
                case TileType.Mountain:
                    resourceNeeded.Add(ResourceType.Water, 1);
                    resourceNeeded.Add(ResourceType.Iron, 1);
                    resourceNeeded.Add(ResourceType.Gold, 5);
                    break;
            }

            string result;
            return player.storage.RemoveResource(resourceNeeded, out result);
        }

        IEnumerator ConstructBuilding(GameObject building, float time)
        {
            constructionParticle.gameObject.transform.position = building.transform.position + Vector3.up * 0.1f;
            constructionParticle.Play();
            building.transform.position -= Vector3.up * 2;

            int iteration = (int) (time / Time.fixedDeltaTime);
            float distance = 2f / iteration;
            for (int i = 0; i < iteration; i++)
            {
                building.transform.position += Vector3.up * distance;
                yield return new WaitForFixedUpdate();
            }
        }

        // Function overloading 
        // Aynı bina kurma fonksiyonun result stringsiz hali
        //
        public bool BuildHouse(TileType houseType, int playerType, Hex coordinate)
        {
            HexTile hexTile = this[coordinate];

            if (hexTile.tileType == TileType.Desert)
            {
                return false;
            }

            if (hexTile.tileType != houseType)
            {
                return false;
            }

            if (hexTile.isPlayerBuiltVillage[playerType])
            {
                return false;
            }

            /* 
            // Peasant var mı yok mu bakılacak
            if()
            {
    
            }
            */
            // Instantiate proper house
            hexTile.isPlayerBuiltVillage[playerType] = true;
            return true;
        }

        // Spesifik bir hex pozisyonundaki şuandaki bina sayısını dönemkte
        // Gameplay kitapçığına göre bina kurarken o alandaki bina sayısı arttıkça bina yapmanın costuda artıyor
        // Bina sayısı burdan çekilerek cost hesaplanabilir.
        public int BuildingCount(Hex coordinate)
        {
            HexTile hexTile = this[coordinate];
            int count = 0;

            foreach (bool isBuilt in hexTile.isPlayerBuiltVillage)
            {
                if (isBuilt) count++;
            }

            return count;
        }


        // private operator overload
        // hex pozisyonundan depo edilen hex tile i bulamk için kullanılıyor
        // Dışarıdan kullanılmazi sadece class içinde işleri kolaylaştırmak için.
        public HexTile this[Hex hex]
        {
            get
            {
                int q = hex.q + depth;
                int r = hex.r + depth;

                return hexTiles[q, r];
            }
            set
            {
                int q = hex.q + depth;
                int r = hex.r + depth;

                hexTiles[q, r] = value;
            }
        }


        private void GenerateVertex(GameObject prefab)
        {
            int layer = LayerMask.NameToLayer("Vertex");

            Vector3 epsilon = new Vector3(0, 0.1f, 0);

            GameObject vertexObjects = new GameObject("Vertices");
            Transform verticesTransform = vertexObjects.transform;

            verticesTransform.parent = hexMap.transform;

            List<int>[] verticesList = new List<int>[depth * 2];
            for (int i = 0; i < depth * 2; i++)
            {
                verticesList[i] = new List<int>();
            }

            List<Vertex> vertices = new List<Vertex>();

            Hex startHex = new Hex(-depth, depth);

            Hex downHex;
            Hex upHex;
            Hex rightHex;

            int currentVertexIndex = 0;


            for (int i = 0; i < 2 * depth; i++)
            {
                downHex = startHex;
                upHex = i < depth ? downHex.GetNeighbor(2) : downHex.GetNeighbor(1);
                startHex = upHex;

                bool isUp = i < depth ? true : false;
                while (true)
                {
                    rightHex = isUp ? upHex.GetNeighbor(0) : downHex.GetNeighbor(0);

                    if (rightHex.IsOutOfRange(depth)) break;

                    Vertex newVertex = new Vertex();

                    newVertex.position =
                        (this[downHex].centerPosition + this[upHex].centerPosition + this[rightHex].centerPosition) /
                        3 - epsilon;

                    vertices.Add(newVertex);
                    this[downHex].vertices.Add(currentVertexIndex);
                    this[upHex].vertices.Add(currentVertexIndex);
                    this[rightHex].vertices.Add(currentVertexIndex);

                    GameObject temp = Object.Instantiate(prefab, newVertex.position, Quaternion.identity) as GameObject;
                    temp.transform.localScale = BoundCalculator.SetUnitScale(temp, 0.5f, 0.1f, 0.58f);
                    temp.name = currentVertexIndex.ToString();
                    temp.transform.parent = verticesTransform;
                    temp.layer = layer;
                    temp.tag = "Vertice";
                    CreateVertexParticleEffect(temp);
                    AddInteraction(temp);
                    temp.GetComponent<Renderer>().sharedMaterial = defaultVertexMaterial;

                    if (!(this[upHex].regionID == this[rightHex].regionID &&
                          this[upHex].regionID == this[downHex].regionID))
                    {
                        if (isUp)
                        {
                            var texture = new Texture2D(2, 2);
                            if (this[upHex].regionID == this[rightHex].regionID)
                            {
                                texture.SetPixel(0, 0, Color.black);
                                
                                texture.SetPixel(1, 0, Color.black);
                            }
                            else
                            {
                                texture.SetPixel(0, 0, borderColor);
                                
                                texture.SetPixel(1, 0, borderColor);
                            }

                            if (this[upHex].regionID == this[downHex].regionID)
                            {
                                texture.SetPixel(1, 1, Color.black);
                            }
                            else
                            {
                                texture.SetPixel(1, 1, borderColor);
                            }

                            if (this[rightHex].regionID == this[downHex].regionID)
                            {
                                texture.SetPixel(0, 1, Color.black);
                            }
                            else
                            {
                                texture.SetPixel(0, 1, borderColor);
                            }

                            texture.filterMode = FilterMode.Point;
                            texture.Apply();
                            temp.GetComponent<Renderer>().material.color = Color.white;
                            temp.GetComponent<Renderer>().material.mainTexture = texture;
                        }
                        else
                        {
                            var texture = new Texture2D(2, 2);
                            if (this[upHex].regionID == this[rightHex].regionID)
                            {
                                texture.SetPixel(0, 0, Color.black);
                            }
                            else
                            {
                                texture.SetPixel(0, 0, borderColor);
                            }

                            if (this[upHex].regionID == this[downHex].regionID)
                            {
                                texture.SetPixel(1, 0, Color.black);
                            }
                            else
                            {
                                texture.SetPixel(1, 0, borderColor);
                            }

                            if (this[rightHex].regionID == this[downHex].regionID)
                            {
                                texture.SetPixel(1, 1, Color.black);
                                texture.SetPixel(0, 1, Color.black);
                            }
                            else
                            {
                                texture.SetPixel(1, 1, borderColor);
                                texture.SetPixel(0, 1, borderColor);
                            }

                            texture.filterMode = FilterMode.Point;
                            texture.Apply();
                            temp.GetComponent<Renderer>().material.color = Color.white;
                            temp.GetComponent<Renderer>().material.mainTexture = texture;
                        }
                    }


                    verticesList[i].Add(currentVertexIndex);
                    currentVertexIndex++;

                    if (isUp) upHex = rightHex;
                    else downHex = rightHex;

                    isUp = !isUp;
                }
            }


            List<int>[] indices = new List<int>[currentVertexIndex];

            for (int i = 0; i < currentVertexIndex; i++)
            {
                indices[i] = new List<int>();
            }


            int basisVertexCount = depth * 2 + 1;
            int j = 0;
            bool isEven = true;
            int k = currentVertexIndex - 1;
            for (int i = 0; i < depth; i++)
            {
                int currentTarget = (i + 1) * (basisVertexCount + i);
                int padding = basisVertexCount + i * 2 + 1;
                if (i + 1 == depth) padding--;
                //Debug.Log(j);
                for (; j < currentTarget; j++, k--)
                {
                    if ((isEven && j % 2 == 0) || (!isEven && j % 2 == 1))
                    {
                        int adjacent = j + padding;
                        indices[j].Add(adjacent);
                        indices[adjacent].Add(j);

                        if (i + 1 != depth)
                        {
                            int adjacent2 = k - padding;
                            indices[k].Add(adjacent2);
                            indices[adjacent2].Add(k);
                        }
                    }

                    if (j + 1 != currentTarget)
                    {
                        indices[j + 1].Add(j);
                        indices[j].Add(j + 1);

                        indices[k].Add(k - 1);
                        indices[k - 1].Add(k);
                    }
                }

                isEven = !isEven;
            }

            /* 
            for(int i=0 ; i<currentVertexIndex ; i++)
            {
                string string1 = i.ToString() + " -> ";
    
                foreach (int a in indices[i])
                {
                    string1+= a.ToString() + " - ";
                }
                Debug.Log(string1);
            }
            */

            this.vertices = vertices;
            this.indices = indices;

            Resize();
        }

        // Shortest path 
        private Stack A_Star(int startIndex, int targetIndex)
        {
            List<VDH> visited = new List<VDH>();
            List<VDH> inProgress = new List<VDH>();

            VDH v1 = new VDH();
            v1.vertexId = startIndex;
            v1.preVertexId = startIndex;
            v1.distance = 0;
            v1.heuristics = Vector3.Distance(vertices[startIndex].position, vertices[targetIndex].position);

            visited.Add(v1);

            int lastVisitedIndex = startIndex;
            VDH lastVisitedVDH = v1;


            while (true)
            {
                if (lastVisitedIndex == targetIndex) break;

                foreach (int i in indices[lastVisitedIndex])
                {
                    VDH newVDH = new VDH();
                    if (lastVisitedIndex != startIndex)
                        newVDH.distance = vertices[lastVisitedIndex].IsTherePeasant() || vertices[i].IsTherePeasant()
                            ? INF + lastVisitedVDH.distance
                            : 1 + lastVisitedVDH.distance;
                    else newVDH.distance = 1 + lastVisitedVDH.distance;
                    newVDH.vertexId = i;
                    newVDH.preVertexId = lastVisitedIndex;
                    newVDH.heuristics = newVDH.distance +
                                        Vector3.Distance(vertices[i].position, vertices[targetIndex].position);

                    int index = inProgress.FindIndex(x => x.vertexId == i);

                    if (index.Equals(-1)) inProgress.Add(newVDH);
                    else
                    {
                        VDH temp = inProgress[index];

                        if (newVDH.CompareTo(temp) == -1)
                        {
                            inProgress[index] = newVDH;
                        }
                    }
                }

                inProgress.Sort();

                lastVisitedVDH = inProgress[0];
                lastVisitedIndex = lastVisitedVDH.vertexId;

                inProgress.RemoveAt(0);

                visited.Add(lastVisitedVDH);
            }

            if (lastVisitedVDH.distance >= INF) return null;
            Debug.Log(lastVisitedVDH.distance);

            Stack shortestPath = new Stack();

            string w = " : ";
            int target = targetIndex;

            while (true)
            {
                shortestPath.Push(target);
                w += target.ToString() + " -> ";
                if (target == startIndex) break;
                VDH temp = visited.Find(x => x.vertexId == target);
                target = temp.preVertexId;
            }

            Debug.Log(w);

            return shortestPath;
        }

        // Test amaçlı
        public void SetVertex(int vertex1, GameObject gameObject1)
        {
            GameObject temp = Instantiate(gameObject1) as GameObject;
            temp.transform.localScale = new Vector3(0.4f, 0.8f, 0.4f);
            temp.transform.position = vertices[vertex1].position;
            vertices[vertex1].peasant = temp;
        }

        private void Move(Stack shortestPath, int start, int end)
        {
            if (shortestPath != null && shortestPath.Count >= 2)
            {
                from = (int) shortestPath.Pop();
                to = (int) shortestPath.Pop();
                peasent = vertices[from].peasant.transform;
                vertices[from].peasant = null;
                movePath = shortestPath;

                vertices[end].peasant = peasent.gameObject;
                peasent.name = end.ToString();
                vertices[end].playerType = vertices[start].playerType;
            }
        }

        public bool MoveExplorer(Player player, Animator animator, int startVertex, int endVertex, int playerType,
            out string result)
        {
            Vertex start = vertices[startVertex];
            Vertex end = vertices[endVertex];

            if (!start.IsTherePeasant())
            {
                result = "There is not a Colonist in start vertex";
                return false;
            }

            if (start.playerType != playerType)
            {
                result = "Colonist in start index is not yours";
                return false;
            }

            if (end.IsTherePeasant())
            {
                result = "There is a Colonist in end vertex";
                return false;
            }


            Stack stack = A_Star(startVertex, endVertex);
            if (stack == null || stack.Count < 2)
            {
                result = "Colonist can not move through this path";
                return false;
            }

            if (stack.Count - 1 > player.playerInteract.moveCapacity)
            {
                result = "You can not move more than " + player.playerInteract.moveCapacity;
                return false;
            }

            player.playerInteract.moveCapacity -= (stack.Count - 1);
            InGameUIManager.Instance.OpenInfoPanel(null, "MoveCapacity",
                "Move remained: " + player.playerInteract.moveCapacity.ToString());
            _peasantAnimator = animator;
            animator.SetBool("Run", true);
            Move(stack, startVertex, endVertex);
            result = "successful";
            return true;
        }

        private Stack movePath = null;
        private int from;
        private int to;
        private Transform peasent;
        private float speed = 1.5f;


        // Test amaçlı
        // Daha sonra FixedUpdate e eklenecek
        private void Updating(float deltaTime)
        {
            if (movePath != null)
            {
                float step = speed * deltaTime * scale;
                peasent.position = Vector3.MoveTowards(peasent.position, vertices[to].position, step);
                peasent.transform.LookAt(vertices[to].position);
                if (Vector3.Distance(peasent.position, vertices[to].position) < 0.001f)
                {
                    if (movePath.Count == 0)
                    {
                        _peasantAnimator.SetBool("Run", false);
                        _peasantAnimator = null;
                        movePath = null;
                    }
                    else to = (int) movePath.Pop();
                }
            }
        }


        void FixedUpdate()
        {
            Updating(Time.deltaTime);
        }

        public int GetVertexCoordinate(GameObject hit)
        {
            string name = (hit.name);
            int index = int.Parse(name);

            return index;
        }


        public bool PlaceExplorer(int vertexID, int playerType, GameObject explorerPrefab, out string result,bool needResource = true )
        {
            HexTile hexTile = this[startHex];


            List<int> v = hexTile.vertices;
            Vertex vertex = vertices[vertexID];
            if (!v.Contains(vertexID))
            {
                result = "You can not place explorer except start tile.";

                return false;
            }

            if (vertex.IsTherePeasant())
            {
                result = "In that vertex, there is already a colonist.";
                return false;
            }

            GameObject temp = explorerPrefab;
            //    temp.transform.localScale = BoundCalculator.SetUnitScale(temp,0.3f,-1f,0.3f) * hexMap.transform.localScale.y;
            temp.transform.position = vertices[vertexID].position + Vector3.up * 0.2f;
            vertex.peasant = temp;
            vertex.playerType = playerType;
            temp.transform.parent = explorers.transform;
            result = "You successfully placed your colonist.";
            explorerPrefab.name = vertexID.ToString();
            if (needResource)
            {
                GameManager.Instance.playerList[playerType].storage.RemoveResource(ResourceType.Colonist, 1);    
            }
            
            return true;
        }

        public void Scale(float value)
        {
            scale = value;
            hexMap.transform.localScale = new Vector3(value, value, value);
            Resize();
        }

        public void Translate(Vector3 pos)
        {
            hexMap.transform.position = pos;
            Resize();
        }

        private void Resize()
        {
            Transform transform = hexMap.transform;

            foreach (Transform child in transform)
            {
                string name = child.name;

                if (name[0] == 'R')
                {
                    foreach (Transform hexChild in child)
                    {
                        Hex hex = Hex.AxialToHex(hexChild.name);
                        this[hex].centerPosition = hexChild.position;
                    }
                }
                else if (name[0] == 'V')
                {
                    foreach (Transform vertexChild in child)
                    {
                        int id = int.Parse(vertexChild.name);

                        Vertex temp = vertices[id];
                        temp.position = vertexChild.position;
                        if (temp.IsTherePeasant())
                        {
                            //temp.peasant.transform.position = temp.position;
                            //temp.peasant.transform.localScale = BoundCalculator.SetUnitScale(temp.peasant,0.3f,-1f,0.3f) * hexMap.transform.localScale.y;
                        }
                    }
                }
            }
        }

        private int desertType = 0;
        private int lakeType = 0;
        private int fieldType = 0;
        private int forestType = 0;
        private int mountainType = 0;
        private int hillType = 0;

        private GameObject GetTile(TileType tileType)
        {
            switch (tileType)
            {
                case TileType.Desert:
                    desertType++;
                    return Desert[desertType % Desert.Count];

                case TileType.Lake:
                    lakeType++;
                    return Lake[lakeType % Lake.Count];

                case TileType.Field:
                    fieldType++;
                    return Field[fieldType % Field.Count];

                case TileType.Forest:
                    forestType++;
                    return Forest[forestType % Forest.Count];

                case TileType.Mountain:
                    mountainType++;
                    return Mountain[mountainType % Mountain.Count];

                case TileType.Hill:
                    hillType++;
                    return Hill[hillType % Hill.Count];

                default:
                    desertType++;
                    return Desert[desertType % Desert.Count];
            }
        }


        public int DeactivateRegions()
        {
            int deactivatedCount = 0;

            for (int i = 0; i < regionCount; i++)
            {
                if (regionActivationStatus[i] == true)
                {
                    regionActivationStatus[i] = false;
                    deactivatedCount++;
                }
            }

            return (int) Mathf.Floor(deactivatedCount * 1.5f);
        }

        public bool ActivateRegion(int regionID, int playerType, out List<int> resource, out string result)
        {
            if (regionActivationStatus[regionID] == true)
            {
                result = "This region has already actiavated";
                resource = new List<int>();
                return false;
            }

            int water = 0;
            int food = 0;
            int wood = 0;
            int stone = 0;
            int iron = 0;

            foreach (Transform temp in allRegions[regionID].transform)
            {
                Hex hex = Hex.AxialToHex(temp.name);
                HexTile hexTile = this[hex];

                if (hexTile.isPlayerBuiltVillage[playerType] == true)
                {
                    if (hexTile.tileType == TileType.Field) food++;
                    else if (hexTile.tileType == TileType.Forest) wood++;
                    else if (hexTile.tileType == TileType.Hill) stone++;
                    else if (hexTile.tileType == TileType.Lake) water++;
                    else if (hexTile.tileType == TileType.Mountain) iron++;
                }
            }

            resource = new List<int>();
            resource.Add(water);
            resource.Add(food);
            resource.Add(wood);
            resource.Add(stone);
            resource.Add(iron);
            result = "Successful";
            return true;
        }

        public int GetCountOfVillagesWithType(TileType tileType, int playerNumber)
        {
            return this.hexTiles.Cast<HexTile>().Where(tile => tile.isPlayerBuiltVillage[playerNumber])
                .Count(tile => tile.tileType == tileType);
        }
    }
}