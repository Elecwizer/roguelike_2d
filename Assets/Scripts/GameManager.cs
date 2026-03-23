using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public BoardManager BoardManager;
    [SerializeField] PlayerController PlayerController;

    public TurnManager TurnManager;

    [SerializeField] UIDocument _UIDoc;
    Label _FoodLabel;

    int _FoodAmount = 100;

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        _FoodAmount += amount;
        _FoodLabel.text = "Food: " + _FoodAmount;
    }

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
        _FoodLabel = _UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        _FoodLabel.text = "Food: " + _FoodAmount;

        TurnManager = new TurnManager();
        TurnManager.OnTicke += OnTurnHappen;

        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
    }

    void Update()
    {
        
    }
}
