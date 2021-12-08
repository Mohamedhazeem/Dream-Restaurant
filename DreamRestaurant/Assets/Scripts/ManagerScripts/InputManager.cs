using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private Camera orthographicCamera;

    public delegate void MovePlayerCallback(float horizontal, float vertical);
    public event MovePlayerCallback OnMovePlayer;
    public delegate void StopPlayerCallback();
    public event StopPlayerCallback OnStoplayer;

    public FloatingJoystick floatingJoystick;

    public float angle;
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
        if (GameManager.Instance.currentGameState == GameManager.GameState.Menu && PlayerManager.Instance.currentPlayerAnimationStates == PlayerAnimationStates.Idle && Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.SwitchGameStates();
            
        }
        if(GameManager.Instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.Instance.currentPlayerAnimationStates == PlayerAnimationStates.Idle && Input.GetMouseButtonDown(0))
        {
            PlayerManager.Instance.SwitchPlayerStates();
        }

        if (Input.GetMouseButton(0) && GameManager.Instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.Instance.currentPlayerAnimationStates == PlayerAnimationStates.Running)
        {
             angle = Mathf.Atan2(floatingJoystick.Horizontal, floatingJoystick.Vertical) * Mathf.Rad2Deg;
             if (floatingJoystick.Horizontal > 0.1f || floatingJoystick.Horizontal < -0.1f || floatingJoystick.Vertical > 0.1f || floatingJoystick.Vertical < -0.1f)
             {
                 OnMovePlayer?.Invoke(floatingJoystick.Horizontal, floatingJoystick.Vertical);
             }
        }
        if (Input.GetMouseButtonUp(0) && GameManager.Instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.Instance.currentPlayerAnimationStates == PlayerAnimationStates.Running)
        {
            OnStoplayer?.Invoke();
            PlayerManager.Instance.SwitchPlayerStates();
        }
    }
}

