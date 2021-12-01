using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;

    public int currentLevelCount = 0;

    private void Awake()
    {
        AssignInstance();
        currentLevelCount = PlayerPrefs.GetInt("UserCurrentLevel", 0);
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
    public void SaveCurrentLevelCount()
    {
        currentLevelCount++;
        PlayerPrefs.SetInt("UserCurrentLevel", currentLevelCount);
    }
    public void ResetCurrentLevelCount()
    {
        currentLevelCount = 0;
        PlayerPrefs.DeleteKey("UserCurrentLevel");
    }

}
