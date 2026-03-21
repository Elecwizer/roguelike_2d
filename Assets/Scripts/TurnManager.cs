using UnityEngine;

public class TurnManager
{
    public event System.Action OnTicke;

    int _turnCount;

    public TurnManager()
    {
        _turnCount = 1;
    }

    public void Tick()
    {
        _turnCount += 1;
        OnTicke?.Invoke();
        Debug.Log("Current turn count: " + _turnCount);
    }
}
