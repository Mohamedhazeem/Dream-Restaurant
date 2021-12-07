using UnityEngine;
using Lean.Pool;
public class SpendPlaceForUnlockObject : MonoBehaviour
{
    [Header("Count Text")]
    public TextMesh TextMesh;
    public int moneyAmount;
    public RestaurantObjects restaurantObjects;
    public int index = 0;

    private void Start()
    {
        TextMesh = GetComponentInChildren<TextMesh>();
        TextMesh.text =  moneyAmount.ToString();
    }
    public void ReduceAmount()
    {
        if (moneyAmount >= 0)
        {
            TextMesh.text = moneyAmount--.ToString();
        }
        if(moneyAmount == 0)
        {
            ChooseRestaurantObjects();
            gameObject.SetActive(false);
        }       
    }
    private void ChooseRestaurantObjects()
    {
        switch (restaurantObjects)
        {
            case RestaurantObjects.DinningTable:
                GameObject gameObject = LeanPool.Spawn(Resources.Load("Levels/" + RestaurantObjects.DinningTable.ToString()) as GameObject);
                var count = LevelManager.Instance.data[LevelManager.Instance.CURRENTLEVEL].gamePlayUis.Count;
                for (int i = 0; i < count; i++)
                {
                    Debug.Log("W");
                    gameObject.transform.position = LevelManager.Instance.data[LevelManager.Instance.CURRENTLEVEL].gamePlayUis[i].unlockableObjectPrefabTransformPosition[index];
                    gameObject.transform.rotation = LevelManager.Instance.data[LevelManager.Instance.CURRENTLEVEL].gamePlayUis[i].unlockableObjectPrefabTransformRotation[index];
                    gameObject.transform.localScale = LevelManager.Instance.data[LevelManager.Instance.CURRENTLEVEL].gamePlayUis[i].unlockableObjectPrefabTransformScale[index];
                }
                break;
            
            default:
                break;
        }
    }
}
