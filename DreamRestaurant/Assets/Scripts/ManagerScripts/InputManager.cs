using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public delegate void MovePlayerCallback(float horizontal, float vertical);
    public event MovePlayerCallback OnMovePlayer;

    public FloatingJoystick floatingJoystick;
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
    void Update()
    {
        
        Joystick();
    }
    private void Joystick()
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.Menu && PlayerManager.instance.currentPlayerStates == PlayerStates.Idle && Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.SwitchGameStates();
        }

        if (GameManager.Instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.instance.currentPlayerStates == PlayerStates.Idle)
        {
            if(floatingJoystick.Horizontal >= 0.3f || floatingJoystick.Vertical >= 0.3f || floatingJoystick.Horizontal <= 0.3f || floatingJoystick.Vertical <= 0.3f)
            {
                PlayerManager.instance.SwitchPlayerStates();
            }
            
        }
        else if (Input.GetMouseButton(0) && GameManager.Instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.instance.currentPlayerStates == PlayerStates.Running)
        {
            OnMovePlayer?.Invoke(floatingJoystick.Horizontal, floatingJoystick.Vertical);

        }
        else if (Input.GetMouseButtonUp(0) && GameManager.Instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.instance.currentPlayerStates == PlayerStates.Running)
        {
            
            PlayerManager.instance.SwitchPlayerStates();

        }
    }
}

