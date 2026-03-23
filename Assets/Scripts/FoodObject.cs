using UnityEngine;

public class FoodObject : CellObject
{
    [SerializeField] int AmountGranted;

    public override void PlayerEntered()
    {
        Destroy(gameObject);
        
        GameManager.Instance.ChangeFood(AmountGranted);
    }
}
