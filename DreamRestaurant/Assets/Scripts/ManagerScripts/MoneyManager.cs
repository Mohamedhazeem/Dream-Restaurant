using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public GameObject money;
    private void Awake()
    {
        AssignInstance();
    }
    private void AssignInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public GameObject GenerateMoney(Transform parent)
    {
        var gameObject = LeanPool.Spawn(Resources.Load("Money/" + money.name) as GameObject,parent);
        return gameObject;
    }
    public void RemoveMoney(GameObject gameObject)
    {
        LeanPool.Despawn(gameObject);
    }

}
// enum name must be same name as resources folder prefab name
public enum RestaurantObjects
{
    DinningTable
}
public enum UnloackableAreas
{
    Kitchen
}

