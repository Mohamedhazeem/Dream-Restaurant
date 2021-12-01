using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public float speed;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        InputManager.Instance.OnMovePlayer += Move;
    }

    private void Move(float horizontal,float vertical)
    {
      //  playerRigidbody.velocity = transform.forward * vertical * speed;
       // playerRigidbody.velocity = transform.right * vertical * speed;

        playerRigidbody.velocity = new Vector3(horizontal * speed, 0, vertical * speed);

        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.AngleAxis(horizontal * speed, Vector3.up);
    }
}
