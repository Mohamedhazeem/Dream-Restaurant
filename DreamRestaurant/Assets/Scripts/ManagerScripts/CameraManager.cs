using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Camera primaryCamera;
    public Transform target;
    private Vector3 offset;
    public float distance;

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
       target = PlayerManager.instance.currentPlayer.transform;

       offset = primaryCamera.transform.position - target.transform.position;
    }
    void Update()
    {
   
        if(target != null)
        {
            Vector3 pos = target.position;
            pos.x = 0;
            pos.y += offset.y;
            pos.z += offset.z;
            primaryCamera.transform.position = pos;

        }
        
    }
}
