using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComp : MonoBehaviour
{
     
     [SerializeField] private float MoveSpeed = 20f;
     [SerializeField] private Vector3 MoveDir = Vector3.forward;

     [SerializeField] Vector3 Destination;
    void Start()
    {
        SpeedController speedController = FindObjectOfType<SpeedController>();
        if (speedController != null)
        {
            speedController.onGlobalSpeedChanged += SetMoveSpeed;
            SetMoveSpeed(speedController.GetGlobalSpeed());
        }
    }

    public void SetMoveDir(Vector3 dir)
    {
        MoveDir = dir;
    }
    public void SetDestination(Vector3 newDestination)
    {
        Destination = newDestination;
    }
    void Update()
    {
        transform.position += MoveDir * MoveSpeed * Time.deltaTime;
        if(Vector3.Dot((Destination-transform.position).normalized,MoveDir) < 0)
        {
            Destroy(gameObject);
        }
    }

    internal void SetMoveSpeed(float newMoveSpeed)
    {
        MoveSpeed = newMoveSpeed;
    }

    public void CopyFrom(MovementComp other)
    {
        SetMoveSpeed(other.MoveSpeed);
        SetMoveDir(other.MoveDir);
        SetDestination(other.Destination);
    }
}
