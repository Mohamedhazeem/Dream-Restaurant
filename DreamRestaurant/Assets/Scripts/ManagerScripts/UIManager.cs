using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Next Button GameObject")]
    [SerializeField] private GameObject nextButton;

    private void Awake()
    {
        AssignInstance();
    }
    private void AssignInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void NextScene()
    {
        SceneSwitchManager.Instance.NextScene();
    }

}