using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [SerializeField] FoodObject[] _FoodPrefab;
    int _FoodLowerLimit = 2;
    int _FoodUpperLimit = 6;

    [SerializeField] Zombie _Zombie;
    int _ZombieLowerLimit = 1;
    int _ZombieUpperLimit = 2;

    [SerializeField] Slime _Slime;
    int _SlimeLowerLimit = 1;
    int _SlimeeUpperLimit = 3;

    [SerializeField] WallObject _WallPrefab;

    [SerializeField] ExitCellObject _ExitCellObject;

    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = _BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }

    void GenerateFood()
    {
        int foodCount = Random.Range(_FoodLowerLimit, _FoodUpperLimit);
        for(int i = 0; i < foodCount; i++)
        {
            int randomIndex = Random.Range(0, _EmptyCellsList.Count);
            Vector2Int coord = _EmptyCellsList[randomIndex];
            _EmptyCellsList.RemoveAt(randomIndex);
            FoodObject newFood = Instantiate(_FoodPrefab[Random.Range(0, _FoodPrefab.Length)]);

            AddObject(newFood, coord);
        }
    }

    void GenerateWall()
    {
        int wallCount = Random.Range(5, 11);
        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = Random.Range(0, _EmptyCellsList.Count);
            Vector2Int coord = _EmptyCellsList[randomIndex];
            _EmptyCellsList.RemoveAt(randomIndex);
            WallObject newWall = Instantiate(_WallPrefab);

            AddObject(newWall, coord);
        }
    }

    void GenerateEnemies()
    {
        int zombieCount = Random.Range(_ZombieLowerLimit, _ZombieUpperLimit);
        for (int i = 0; i < zombieCount; i++)
        {
            int randomIndex = Random.Range(0, _EmptyCellsList.Count);
            Vector2Int coord = _EmptyCellsList[randomIndex];
            _EmptyCellsList.RemoveAt(randomIndex);
            Zombie newZombie = Instantiate(_Zombie);

            AddObject(newZombie, coord);
        }

        int slimeCount = Random.Range(_SlimeLowerLimit, _SlimeeUpperLimit);
        for (int i = 0; i < slimeCount; i++)
        {
            int randomIndex = Random.Range(0, _EmptyCellsList.Count);
            Vector2Int coord = _EmptyCellsList[randomIndex];
            _EmptyCellsList.RemoveAt(randomIndex);
            Slime newSlime = Instantiate(_Slime);

            AddObject(newSlime, coord);
        }
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        _Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
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
        Vector2Int endCoord = new Vector2Int(_width - 2, _height - 2);
        AddObject(Instantiate(_ExitCellObject), endCoord);
        _EmptyCellsList.Remove(endCoord);

        GameManager.Instance.PlayerController._Animator.SetBool("Moving", false);

        GenerateWall();
        GenerateFood();
        GenerateEnemies();
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return _Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }

    public void Clean()
    {
        if(_BoardData == null)
            return;

        for(int y = 0; y < _height; y++)
        {
            for(int x = 0; x < _width; x++)
            {
                var cellData =_BoardData[x, y];
                if(cellData.ContainedObject != null)
                {
                    Destroy(cellData.ContainedObject.gameObject);
                }

                SetCellTile(new Vector2Int(x, y), null);
            }
        }
    }
}
