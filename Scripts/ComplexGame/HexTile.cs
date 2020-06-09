using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



namespace ComplexGame
{
// HexTile (HexCell) structure
// Hex Tile ın bilgisini tutmak için. 
// Sadece HexMap class ı içinde kullanılıyor, dışarıdan doğrudan etkileşime geçilemez
// 
public class HexTile
{
    public TileType tileType;
    public Vector3 centerPosition;
    public bool[] isPlayerBuiltVillage;
    public int regionID;
    public List<int> vertices;

    public HexTile(TileType tileType1,Vector3 centerPosition1,int numberofPlayers)
    {
        tileType = tileType1;
        centerPosition = centerPosition1;
        
        isPlayerBuiltVillage = new bool[numberofPlayers];
        for(int i=0 ; i< numberofPlayers ; i++) 
        {
            isPlayerBuiltVillage[i]= false;
        }

        vertices = new List<int>();
    }


}


// HexMapta bulunan tile lar için pozisyon structure i 
public struct Hex
{

    public readonly int q;
    public readonly int r;
    public readonly int s;


    public Hex(int q1, int r1, int s1)
    {
        this.q = q1;
        this.r = r1;
        this.s = s1;

        if (q1 + r1 + s1 != 0) throw new IndexOutOfRangeException("Invalid Hex Index");
    }

    public Hex(int q1, int r1)
    {
        this.q = q1;
        this.r = r1;
        this.s = -(q1 + r1);
    }


    public static Hex operator +(Hex a, Hex b)
    {
        return new Hex(a.q + b.q, a.r + b.r, a.s + b.s);
    }

    public static Hex operator -(Hex a, Hex b)
    {
        return new Hex(a.q - b.q, a.r - b.r, a.s - b.s);
    }

    public static Hex operator *(Hex a, int scale)
    {
        return new Hex(a.q * scale, a.r * scale, a.s * scale);
    }

    public static Hex operator *(int scale, Hex a)
    {
        return new Hex(a.q * scale, a.r * scale, a.s * scale);
    }

    public int Length()
    {
        return (int)((Math.Abs(q) + Math.Abs(r) + Math.Abs(s)) / 2);
    }

    public static int Distance(Hex a, Hex b)
    {
        return (a - b).Length();
    }

    static public List<Hex> directions = new List<Hex> { new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1), new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1) };

    static public Hex Direction(int direction)
    {
        if (direction < 0 || direction > 5) throw new IndexOutOfRangeException("Invalid Direction Index");

        return Hex.directions[direction];
    }

    public Hex GetNeighbor(int direction)
    {
        return this + Hex.Direction(direction);
    }

    public override bool Equals(object other)
    {
        if (!(other is Hex)) return false;

        return Equals((Hex)other);
    }

    public bool Equals(Hex other)
    {
        return q == other.q && r == other.r && s == other.s;
    }

    public static bool operator ==(Hex a, Hex b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Hex a, Hex b)
    {
        return !a.Equals(b);
    }

    public override string ToString()
    {
        return "(" + q.ToString() + "," + r.ToString() + "," + s.ToString() + ")";
    }

    public string ToAxial()
    {
        return q.ToString() + "/" + r.ToString();
    }

    public static Hex AxialToHex(string a)
    {
        string[] split = a.Split('/');

        int q = int.Parse(split[0]);
        int r = int.Parse(split[1]);
        int s = -(q + r);


        return new Hex(q, r, s);
    }

    public bool IsOutOfRange(int depth)
    {
        return Mathf.Abs(q) > depth || Mathf.Abs(r) > depth || Mathf.Abs(s) > depth;
    }

}


// Vertexleri tutmak için helper class 
// Dışarıdan kullanılmayacak
public class Vertex
{
    public Vector3 position;
    public GameObject peasant = null;
    public int playerType;

    public bool IsTherePeasant()
    {
        return !(peasant == null);
    }

    
}

// A* algoritması için helper structure
// Dışarıdan kullanılmayacak
public struct VDH : IComparable<VDH>
{
    public int vertexId;
    public int distance;
    public float heuristics;
    public int preVertexId; 

    public int CompareTo(VDH comparePart)
    { 
        return this.heuristics.CompareTo(comparePart.heuristics);
    }
}

public class Randomizer
{
    public int depth;
    public int regionCount;

    private RandomTile[,] map;
    private List<Hex> [] regions;

    private List<Hex> allNeighbors;
    

    public Randomizer(int _depth, int _regionCount)
    {
        depth = _depth;
        regionCount = _regionCount;
        Initialize();
    }

    private void Initialize()
    {
        int size = depth*2 + 1;
        
        map = new RandomTile[size,size];
        
        regions = new List<Hex>[regionCount];

        allNeighbors = new List<Hex>();

        for(int i=0 ; i < regionCount ; i++)
        {
            regions[i] = new List<Hex>();

            while(true)
            {
                int q = UnityEngine.Random.Range(-depth,depth+1);
                int r = UnityEngine.Random.Range(-depth,depth+1);
                
                if(Mathf.Abs(q+r) > depth) continue;

                Hex temp = new Hex(q,r);

                if(this[temp] != null) continue;

                regions[i].Add(temp);
                
                RandomTile randomTile = new RandomTile();
                randomTile.regionID = i;

                this[temp] = randomTile;

                break;
            }
        }


        while(true)
        {
            bool isAnyCaptured = false;

            for(int i=0 ; i<regionCount ; i++)
            {
                allNeighbors.Clear();

                foreach(Hex hex in regions[i])
                {
                    for(int j=0 ; j<6 ; j++)
                    {
                        Hex neighbor = hex.GetNeighbor(j);
                        if(neighbor.IsOutOfRange(depth) || this[neighbor] != null) continue;

                        allNeighbors.Add(neighbor);
                    }
                }

                if(allNeighbors.Count == 0) continue;
                
                int index = UnityEngine.Random.Range(0,allNeighbors.Count);

                Hex temp = allNeighbors[index];
                
                regions[i].Add(temp);

                RandomTile randomTile = new RandomTile();
                randomTile.regionID = i;
                randomTile.tileType = TileType.Desert;

                this[temp] = randomTile;

                isAnyCaptured = true;

            }

            if(!isAnyCaptured) break;
        }

        
        int tileType = 0;

        foreach(List<Hex> aRegion in regions)
        {
            int regionSize = aRegion.Count;
            int willBuilt = (int) (regionSize / 3);
            
            for(int i=0 ; i<willBuilt ; i++)
            {
                Hex randomHex = aRegion[UnityEngine.Random.Range(0,regionSize)];
                
                while(this[randomHex].tileType != TileType.Desert)
                {
                    randomHex = aRegion[UnityEngine.Random.Range(0,regionSize)];
                }

                this[randomHex].tileType = GetTileType(tileType);
                tileType++;
                

            }
        }
        
    }


    private TileType GetTileType(int i)
    {
        i = i%5;
        switch(i)
        {
            case 0:
                return TileType.Field;
            case 1:
                return TileType.Forest;
            case 2:
                return TileType.Hill;
            case 3:
                return TileType.Lake;
            default:
                return TileType.Mountain;
        }
    }

    public int GetRandomRegion(Hex hex)
    {
        return this[hex].regionID;
    }

    public TileType GetRandomTileType(Hex hex)
    {
        return this[hex].tileType;
    }

    private RandomTile this[Hex hex]
    {
        get
        {
            int q = hex.q + depth;
            int r = hex.r + depth;

            return map[q,r];
        }
        set
        {
            int q = hex.q + depth;
            int r = hex.r + depth;

            map[q,r] = value;
        }
    }

}


public class RandomTile
{
    public TileType tileType;
    public int regionID;
}  
}

