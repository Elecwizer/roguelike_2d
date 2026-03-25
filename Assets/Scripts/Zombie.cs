using UnityEngine;

public class NewMonoBehaviourScript : CellObject
{
    public int Health = 3;
    int _CurrentHealth;

    void Awake()
    {
        GameManager.Instance.TurnManager.OnTicke += TurnHappened;
    }

    void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnTicke -= TurnHappened;
    }

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        _CurrentHealth = Health;
    }

    public override bool PlayerWantsToEnter()
    {
        _CurrentHealth -= 1;

        if(_CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }

        return false;
    }

    bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if(targetCell == null || !targetCell.Passable || targetCell.ContainedObject != null)
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

    void TurnHappened()
    {
        var playerCell = GameManager.Instance.PlayerController._CellPosition;

        int xDist = playerCell.x - _Cell.x;
        int yDist = playerCell.y - _Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if((xDist == 0 && absYDist == 1) || (yDist == 0 && absXDist == 1))
        {
            GameManager.Instance.ChangeFood(-3);
        }
        else
        {
            if(absXDist > absYDist)
            {
                if (!TryMoveInX(xDist))
                {
                    TryMoveInY(yDist);
                }
            }
            else
            {
                if (!TryMoveInY(yDist))
                {
                    TryMoveInX(xDist);
                }
            }
        }
    }

    bool TryMoveInX(int xDist)
    {
        if (xDist > 0)
        {
            return MoveTo(_Cell + Vector2Int.right);
        }

        return MoveTo(_Cell + Vector2Int.left);
    }

   bool TryMoveInY(int yDist)
    {

        if (yDist > 0)
        {
            return MoveTo(_Cell + Vector2Int.up);
        }

        return MoveTo(_Cell + Vector2Int.down);
    }
}
