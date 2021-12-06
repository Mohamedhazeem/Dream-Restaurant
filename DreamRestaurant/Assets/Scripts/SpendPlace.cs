using UnityEngine;
using Lean.Pool;
public class SpendPlace : MonoBehaviour
{
    [Header("Count Text")]
    public TextMesh TextMesh;
    public int count;
    public RestaurantObjects restaurantObjects;
    
    private void OnEnable()
    {
        TextMesh = GetComponentInChildren<TextMesh>();
        TextMesh.text =  count.ToString();
    }
    public void ReduceAmount()
    {
        if (count >= 0)
        {
            TextMesh.text = count--.ToString();
        }
        if(count == 0)
        {
            ChooseRestaurantObjects();
            gameObject.SetActive(false);
        }       
    }
    private void ChooseRestaurantObjects()
    {
        switch (restaurantObjects)
        {
            case RestaurantObjects.UnlockableArea:

                break;

            case RestaurantObjects.DinningTable:
                GameObject gameObject = LeanPool.Spawn(Resources.Load("Levels/" + RestaurantObjects.DinningTable.ToString()) as GameObject);
                break;
            
            default:
                break;
        }
    }
}
