using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator _Animator;

    bool _IsGameOver;
    public bool _isPaused;
    bool  _hasMoved;

    BoardManager _Board;
    public Vector2Int _CellPosition;

   public void Spawn(BoardManager boardManager, Vector2Int cell)
   {
       _Board = boardManager;
       MoveTo(cell);
   }
  
   public void MoveTo(Vector2Int cell)
   {
       _CellPosition = cell;
       transform.position = _Board.CellToWorld(_CellPosition);
   }

   public void GameOver()
    {
        _IsGameOver = true;
    }

  
   private void Update()
   {
        if (_IsGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                _Animator.SetBool("Moving", false);
                GameManager.Instance.StartNewGame();
            }
            return;
        }

        if (_isPaused)
        {
            return;
        }

       Vector2Int newCellTarget = _CellPosition;
        _hasMoved = false;

       if(Keyboard.current.upArrowKey.wasPressedThisFrame)
       {
           newCellTarget.y += 1;
           _hasMoved = true;
           _Animator.SetBool("Moving", true);
       }
       else if(Keyboard.current.downArrowKey.wasPressedThisFrame)
       {
           newCellTarget.y -= 1;
           _hasMoved = true;
           _Animator.SetBool("Moving", true);
       }
       else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
       {
           newCellTarget.x += 1;
           _hasMoved = true;
           _Animator.SetBool("Moving", true);
       }
       else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
       {
           newCellTarget.x -= 1;
           _hasMoved = true;
           _Animator.SetBool("Moving", true);
       }

       if(_hasMoved)
       {
           BoardManager.CellData cellData = _Board.GetCellData(newCellTarget);

           if(cellData != null && cellData.Passable)
           {
                GameManager.Instance.TurnManager.Tick();
                if(cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget);
                }else if (cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget);
                    cellData.ContainedObject.PlayerEntered();
                }
           }
       }
   }

   public void Init()
    {
        _IsGameOver = false;
    }

    void Awake()
    {
        _Animator = GetComponent<Animator>();
    }
}