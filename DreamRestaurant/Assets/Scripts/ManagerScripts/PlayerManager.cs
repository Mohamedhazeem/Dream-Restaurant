using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Player Spawn Point")]
    public Transform playerSpawnPoint;

    [Header("Players")]
    [SerializeField]
    private GameObject playerPrefab;
    public GameObject currentPlayer;
    public Material playerMaterial;

    public PlayerStates currentPlayerStates;

    public List<GameObject> npc;

    public float enemyrange;
    public LayerMask layerMask;

    public int Count;
    private int currentCount;
    private int temporaryCount = 0;
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
    private void Start()
    {
        if (playerSpawnPoint != null)
        {
            currentPlayer = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        }

        currentPlayerStates = PlayerStates.Idle;
    }

    public void SwitchPlayerStates()
    {
        switch (currentPlayerStates)
        {
            case PlayerStates.Idle:
                currentPlayerStates = PlayerStates.Running;
                break;

            case PlayerStates.Running:
                currentPlayerStates = PlayerStates.Idle;
                break;

            case PlayerStates.Win:
                break;
            default:
                break;
        }
    }
}   
public enum PlayerStates
{
    Idle,
    Running,
    Win,
}
