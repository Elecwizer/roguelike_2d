using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    [SerializeField] Tile[] _ObstacleTile;
    public int MaxHealth = 3;

    int _HealthPoint;
    Tile _OriginalTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        _HealthPoint = MaxHealth;
        _OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);

        int randomWall = Random.Range(0, _ObstacleTile.Length);
        GameManager.Instance.BoardManager.SetCellTile(cell, _ObstacleTile[randomWall]);
    }

    public override bool PlayerWantsToEnter()
    {
        _HealthPoint -= 1;

        if(_HealthPoint > 0)
        {
            return false;
        }
        
        GameManager.Instance.BoardManager.SetCellTile(_Cell, _OriginalTile);
        Destroy(gameObject);
        return true;
    }
}
