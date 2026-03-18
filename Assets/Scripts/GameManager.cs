using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField] BoardManager BoardManager;
    [SerializeField] PlayerController PlayerController;

    public TurnManager TurnManager;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        TurnManager = new TurnManager();

        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
    }

    void Update()
    {
        
    }
}
