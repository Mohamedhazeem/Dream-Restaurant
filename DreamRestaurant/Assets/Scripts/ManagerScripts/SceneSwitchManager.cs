using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitchManager : MonoBehaviour
{
    public static SceneSwitchManager Instance;
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
    public void NextScene()
    {
        if(LevelManager.Instance.CURRENTLEVEL < LevelManager.TOTALLEVELS)
        {
            LevelManager.Instance.ChangeLevel();
            SceneManager.LoadScene("Level");
        }
        else
        {
            LevelManager.Instance.ChangeLevel();
            SceneManager.LoadScene("Level");
        }
       
    }
}
