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

    [SerializeField] private GameObject player;

    private Vector3 mouseStartPos;
    private Vector3 mouseCurrentPos;
    private Vector3 dragDirection;
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
            if (Input.GetMouseButtonDown(0))
            {
                mouseStartPos = orthographicCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseStartPos.y = player.transform.position.y;
            }

            else if (Input.GetMouseButton(0))
            {
                mouseCurrentPos = orthographicCamera.ScreenToWorldPoint(Input.mousePosition);

                mouseCurrentPos.y = player.transform.position.y;
                dragDirection = mouseCurrentPos - mouseStartPos;

                if (dragDirection.magnitude > 0.01f)
                {
                    angle = Mathf.Atan2(dragDirection.x, dragDirection.z) * Mathf.Rad2Deg;
                    if (floatingJoystick.Horizontal > 0.1f || floatingJoystick.Horizontal < -0.1f || floatingJoystick.Vertical > 0.1 || floatingJoystick.Vertical < -0.1)
                    {
                        OnMovePlayer?.Invoke(floatingJoystick.Horizontal, floatingJoystick.Vertical);
                    }
                                 
                }
            }           
        }
        if (Input.GetMouseButtonUp(0) && GameManager.Instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.Instance.currentPlayerAnimationStates == PlayerAnimationStates.Running)
        {
            OnStoplayer?.Invoke();
            PlayerManager.Instance.SwitchPlayerStates();
        }
    }
}

