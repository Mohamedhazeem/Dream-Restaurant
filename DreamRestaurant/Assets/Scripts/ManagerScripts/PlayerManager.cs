using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Spawn Point")]
    public Transform playerSpawnPoint;

    [Header("Players")]
    [SerializeField]
    private GameObject playerPrefab;
    public GameObject currentPlayer;

    public PlayerAnimationStates currentPlayerAnimationStates;
    public PlayerMoneyState currentPlayerMoneyStates;

    public Stack<GameObject> moneyStack = new Stack<GameObject>();
    public Stack<Vector3> lastMoneyPosition = new Stack<Vector3>();
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
        if (playerSpawnPoint != null)
        {
            currentPlayer = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        }

        currentPlayerAnimationStates = PlayerAnimationStates.Idle;
        currentPlayerMoneyStates = PlayerMoneyState.BareHand;
    }

    public void SwitchPlayerStates()
    {
        switch (currentPlayerAnimationStates)
        {
            case PlayerAnimationStates.Idle:
                currentPlayerAnimationStates = PlayerAnimationStates.Running;
                break;

            case PlayerAnimationStates.Running:
                currentPlayerAnimationStates = PlayerAnimationStates.Idle;
                break;

            case PlayerAnimationStates.Win:
                break;
            default:
                break;
        }
    }
}   
public enum PlayerAnimationStates
{
    Idle,
    Running,
    Win,
}
public enum PlayerMoneyState
{
    BareHand,
    LoadWithMoney
}
