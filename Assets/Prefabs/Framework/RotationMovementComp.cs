using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMovementComp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up,rotationSpeed * Time.deltaTime);
    }
}
