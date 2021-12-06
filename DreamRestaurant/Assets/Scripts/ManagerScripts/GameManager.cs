using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
     public enum GameState
     {
        Menu,
        GamePlay,
        Win
     }
    
    public GameState currentGameState;

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

    private void Start()
    {
        currentGameState = GameState.Menu;
    }
    public void SwitchGameStates()
    {
        switch (currentGameState)
        {
            case GameState.Menu:
                currentGameState = GameState.GamePlay;
                break;

            case GameState.GamePlay:
                break;

            case GameState.Win:
                UserDataManager.instance.SaveCurrentLevelCount();

                break;

            default:
                currentGameState = GameState.Menu;
                break;
        }
    }
}
