using UnityEngine;

public class Slime : CellObject
{
    public int Health = 1;
    int _CurrentHealth;

    void Awake()
    {
        GameManager.Instance.TurnManager.OnTicke += TurnHappened;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.TurnManager != null)
        {
            GameManager.Instance.TurnManager.OnTicke -= TurnHappened;
        }
    }

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        _CurrentHealth = Health;
    }

    public override bool PlayerWantsToEnter()
    {
        _CurrentHealth -= 1;

        if (_CurrentHealth <= 0)
        {
            var board = GameManager.Instance.BoardManager;
            var cellData = board.GetCellData(_Cell);

            if (cellData != null && cellData.ContainedObject == this)
            {
                cellData.ContainedObject = null;
            }

            Destroy(gameObject);
        }

        return false;
    }

    bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if (targetCell == null || !targetCell.Passable || targetCell.ContainedObject != null)
        {
            return false;
        }

        var currentCell = board.GetCellData(_Cell);
        currentCell.ContainedObject = null;

        targetCell.ContainedObject = this;
        _Cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }

    void Die()
    {
        var board = GameManager.Instance.BoardManager;
        var cellData = board.GetCellData(_Cell);

        if (cellData != null && cellData.ContainedObject == this)
        {
            cellData.ContainedObject = null;
        }

        Destroy(gameObject);
    }

    void TurnHappened()
    {
        var playerCell = GameManager.Instance.PlayerController._CellPosition;

        int xDist = playerCell.x - _Cell.x;
        int yDist = playerCell.y - _Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1) || (yDist == 0 && absXDist == 1))
        {
            GameManager.Instance.ChangeFood(-2);
            Die();
            return;
        }

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.down
        };

        for (int i = 0; i < directions.Length; i++)
        {
            int randomIndex = Random.Range(i, directions.Length);
            Vector2Int temp = directions[i];
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }

        for (int i = 0; i < directions.Length; i++)
        {
            if (MoveTo(_Cell + directions[i]))
            {
                return;
            }
        }
    }
}