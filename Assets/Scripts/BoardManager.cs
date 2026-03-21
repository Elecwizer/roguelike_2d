using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [SerializeField] FoodObject[] _FoodPrefab;
    int _FoodLowerLimit = 1;
    int _FoodUpperLimit = 6;

    void GenerateFood()
    {
        int foodCount = Random.Range(_FoodLowerLimit, _FoodUpperLimit);
        for(int i = 0; i < foodCount; i++)
        {
            int randomIndex = Random.Range(0, _EmptyCellsList.Count);
            Vector2Int coord = _EmptyCellsList[randomIndex];
            _EmptyCellsList.RemoveAt(randomIndex);

            CellData data = _BoardData[coord.x, coord.y];
            FoodObject newFood = Instantiate(_FoodPrefab[Random.Range(0, _FoodPrefab.Length)]);
            newFood.transform.position = CellToWorld(coord);
            data.ContainedObject = newFood;
        }
    }
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= _width
            || cellIndex.y < 0 || cellIndex.y >= _height)
        {
            return null;
        }

        return _BoardData[cellIndex.x, cellIndex.y];
    }

    CellData[,] _BoardData;
    Tilemap _Tilemap;
    Grid _Grid;

    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] Tile[] _groundTiles;
    [SerializeField] Tile[] _wallTiles;

    List<Vector2Int> _EmptyCellsList;

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return _Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public void Init()
    {
        _BoardData = new CellData[_width, _height];

        _EmptyCellsList = new List<Vector2Int>();

        _Tilemap = GetComponentInChildren<Tilemap>();
        _Grid = GetComponentInChildren<Grid>();

        for (int y = 0; y < _height; ++y)
        {
            for (int x = 0; x < _width; ++x)
            {
                Tile tile;
                _BoardData[x, y] = new CellData();

                if(x == 0 || y ==0 || x == _width - 1 || y == _height - 1)
                {
                    tile = _wallTiles[Random.Range(0, _wallTiles.Length)];
                    _BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = _groundTiles[Random.Range(0, _groundTiles.Length)];
                    _BoardData[x, y].Passable = true;

                    _EmptyCellsList.Add(new Vector2Int(x, y));
                }
                
                _Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        _EmptyCellsList.Remove(new Vector2Int(1,1));
        GenerateFood();
    }

    void Update()
    {
        
    }
}
