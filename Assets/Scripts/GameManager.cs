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

    int _FoodAmount = 20;

    int _CurrentLevel = 0;

    VisualElement _GameOverScreen;
    Label _GameOverMessage;

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        _FoodAmount += amount;
        _FoodLabel.text = "Food: " + _FoodAmount;

        if(_FoodAmount <= 0)
        {
            PlayerController.GameOver();
            _GameOverScreen.style.visibility = Visibility.Visible;
            _GameOverMessage.text = "Game Over!\n\nYou survived through " + _CurrentLevel + " days\n\nPress enter key to restart";
        }
    }

    public void NewLevel()
    {
        BoardManager.Clean();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));

        _CurrentLevel++;
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
        _GameOverScreen = _UIDoc.rootVisualElement.Q<VisualElement>("GameOverScreen");
        _GameOverMessage = _GameOverScreen.Q<Label>("GameOverMessage");

        _FoodLabel = _UIDoc.rootVisualElement.Q<Label>("FoodLabel");

        TurnManager = new TurnManager();
        TurnManager.OnTicke += OnTurnHappen;

        StartNewGame();
    }

    public void StartNewGame()
    {
        _GameOverScreen.style.visibility = Visibility.Hidden;

        _CurrentLevel = 0;
        _FoodAmount = 20;
        _FoodLabel.text = "Food: " + _FoodAmount;

        BoardManager.Clean();
        BoardManager.Init();

        PlayerController.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
    }
}
