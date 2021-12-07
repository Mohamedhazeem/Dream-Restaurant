using UnityEngine;
using Lean.Pool;
public class SpendPlaceForUnlockArea : MonoBehaviour
{
    [Header("Count Text")]
    public TextMesh TextMesh;
    public int moneyAmount;
    public UnloackableAreas unloackableAreas;
    public int index = 0;
    private void Start()
    {
        TextMesh = GetComponentInChildren<TextMesh>();
        TextMesh.text = moneyAmount.ToString();
    }
    public void ReduceAmount()
    {
        if (moneyAmount >= 0)
        {
            TextMesh.text = moneyAmount--.ToString();
        }
        if (moneyAmount == 0)
        {
            ChooseUnloackableAreas();
            gameObject.SetActive(false);
        }
    }

    private void ChooseUnloackableAreas()
    {
        switch (unloackableAreas)
        {
            case UnloackableAreas.Kitchen:
                GameObject gameObject = LeanPool.Spawn(Resources.Load("Levels/" + UnloackableAreas.Kitchen.ToString()) as GameObject);
                Debug.Log(gameObject.name);
                var count = LevelManager.Instance.data[LevelManager.Instance.CURRENTLEVEL].gamePlayUis.Count;
                for (int i = 0; i < count; i++)
                {
                    gameObject.transform.position = LevelManager.Instance.data[LevelManager.Instance.CURRENTLEVEL].gamePlayUis[i].unlockableAreaPrefabTransformPosition[index];
                    gameObject.transform.rotation = LevelManager.Instance.data[LevelManager.Instance.CURRENTLEVEL].gamePlayUis[i].unlockableAreaPrefabTransformRotation[index];
                    gameObject.transform.localScale = LevelManager.Instance.data[LevelManager.Instance.CURRENTLEVEL].gamePlayUis[i].unlockableAreaPrefabTransformScale[index];
                }
                break;
            default:
                break;
        }
    }
}
