using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= _width
            || cellIndex.y < 0 || cellIndex.y >= _height)
        {
            return null;
        }

        return m_BoardData[cellIndex.x, cellIndex.y];
    }


    public PlayerController Player;

    CellData[,] m_BoardData;
    Tilemap m_Tilemap;
    Grid m_Grid;

    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] Tile[] _groundTiles;
    [SerializeField] Tile[] _wallTiles;

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    void Start()
    {
        m_BoardData = new CellData[_width, _height];

        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();

        for (int y = 0; y < _height; ++y)
        {
            for (int x = 0; x < _width; ++x)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData();

                if(x == 0 || y ==0 || x == _width - 1 || y == _height - 1)
                {
                    tile = _wallTiles[Random.Range(0, _wallTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = _groundTiles[Random.Range(0, _groundTiles.Length)];
                    m_BoardData[x, y].Passable = true;
                }
                
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        Player.Spawn(this, new Vector2Int(1,1));
    }

    void Update()
    {
        
    }
}
