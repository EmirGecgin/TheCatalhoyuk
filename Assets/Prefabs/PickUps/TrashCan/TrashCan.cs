using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TrashCan : Pickup
{

    [SerializeField] private float collisionPushSpeed = 20f;
    [SerializeField] private Vector3 collisionTorque = new Vector3(2f,2f,2f);
    [SerializeField] private float destroyDelay = 3f;


    protected override void PickedUpBy(GameObject picker)
    {
        GetMovementComp().enabled = false;
        GetComponent<Collider>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce((transform.position - picker.transform.position).normalized * collisionPushSpeed,ForceMode.VelocityChange);
        rb.AddTorque(collisionTorque,ForceMode.VelocityChange);
        
        Invoke("DestroySelf",destroyDelay);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
