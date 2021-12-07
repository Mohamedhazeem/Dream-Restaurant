using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("RigidBody")]
    public Rigidbody playerRigidbody;
    [Header("Animator")]
    public Animator animator;
    [Header("Spend Place For Unlockobject")]
    public SpendPlaceForUnlockObject spendPlaceToUnlockObject;
    [Header("Spend Place for Unlock Area")]
    public SpendPlaceForUnlockArea spendPlaceToUnlockArea;
    [Header("Move Speed")]
    public float speed;
    [Header("Rotate Speed")]
    public float rotateSpeed;

    [Header("Stack Position gameobject")]
    public Transform stackPosition;
    public Vector3 stackMoneyOffset;
    private Vector3 lastMoneyposition;
    private Vector3 move;

    public GameObject stackObject;


    bool isTrigger = false;
    bool isSpendtrigger = false;
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        InputManager.Instance.OnMovePlayer += Move;
        InputManager.Instance.OnStoplayer += StopMoveAnimation;
    }

    private void Move(float horizontal,float vertical)
    {
        move.x = horizontal * speed;
        move.y = 0;
        move.z = vertical * speed;
        playerRigidbody.velocity = move;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(InputManager.Instance.angle, Vector3.up), Time.deltaTime * rotateSpeed);
        PlayMoveAnimation();
    }

    private void PlayMoveAnimation()
    {
        animator.SetBool(Animator.StringToHash("Idle"), false);
        animator.SetBool(Animator.StringToHash("Run"), true);

    }

    private void StopMoveAnimation()
    {
        playerRigidbody.velocity = Vector3.zero;
        animator.SetBool(Animator.StringToHash("Idle"), true);
        animator.SetBool(Animator.StringToHash("Run"), false);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoneyPlace")&& !isTrigger)
        {
            
            StartCoroutine(GetMoney());
            isTrigger = true;
        }
        else if (other.CompareTag("MoneySpendToUnlockObject") && !isSpendtrigger)
        {
            spendPlaceToUnlockObject = other.GetComponent<SpendPlaceForUnlockObject>();
            StartCoroutine(GiveMoney());
            isSpendtrigger = true;
        }
        else if (other.CompareTag("MoneySpendToUnlockArea") && !isSpendtrigger)
        {
            spendPlaceToUnlockArea = other.GetComponent<SpendPlaceForUnlockArea>();
            StartCoroutine(GiveMoney());
            isSpendtrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MoneyPlace"))
        {
            isTrigger = false;
            StopAllCoroutines();
        }
        else if (other.CompareTag("MoneySpendToUnlockObject"))
        {
            StopAllCoroutines();
            spendPlaceToUnlockObject = null;
            isSpendtrigger = false;
        }
        else if (other.CompareTag("MoneySpendToUnlockArea"))
        {
            StopAllCoroutines();
            spendPlaceToUnlockArea = null;
            isSpendtrigger = false;
        }
    }
    IEnumerator GetMoney()
    {
        PlayerManager.Instance.currentPlayerMoneyStates = PlayerMoneyState.LoadWithMoney;
        while (true)
        {
            stackObject = MoneyManager.Instance.GenerateMoney(stackPosition);

            if(stackPosition.childCount <= 1)
            {
                stackObject.transform.position = stackPosition.position;
                lastMoneyposition = stackObject.transform.position;
            }
            else
            {
                lastMoneyposition.x = stackPosition.position.x;
                lastMoneyposition.z = stackPosition.position.z;
                stackObject.transform.position = lastMoneyposition + stackMoneyOffset;
                lastMoneyposition = stackObject.transform.position;
            }
            PlayerManager.Instance.moneyStack.Push(stackObject);
            PlayerManager.Instance.lastMoneyPosition.Push(lastMoneyposition);
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator GiveMoney()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (PlayerManager.Instance.moneyStack.Count > 0 && spendPlaceToUnlockObject!= null && spendPlaceToUnlockObject.moneyAmount >= 0)
            {
                stackObject = PlayerManager.Instance.moneyStack.Pop();
                MoneyManager.Instance.RemoveMoney(stackObject);
                lastMoneyposition = PlayerManager.Instance.lastMoneyPosition.Pop();
                lastMoneyposition -= stackMoneyOffset;
                spendPlaceToUnlockObject.ReduceAmount();
            }
            if (PlayerManager.Instance.moneyStack.Count > 0 && spendPlaceToUnlockArea != null && spendPlaceToUnlockArea.moneyAmount >= 0)
            {
                stackObject = PlayerManager.Instance.moneyStack.Pop();
                MoneyManager.Instance.RemoveMoney(stackObject);
                lastMoneyposition = PlayerManager.Instance.lastMoneyPosition.Pop();
                lastMoneyposition -= stackMoneyOffset;
                spendPlaceToUnlockArea.ReduceAmount();
            }
            else if(PlayerManager.Instance.moneyStack.Count <= 0)
            {
                PlayerManager.Instance.currentPlayerMoneyStates = PlayerMoneyState.BareHand;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
