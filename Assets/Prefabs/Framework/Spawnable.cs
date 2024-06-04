using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    [SerializeField] private float _SpawnInterval = 2f;
    [SerializeField] private MovementComp movementComp;

    public float SpawnInterval
    {
        get { return _SpawnInterval; }
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public  MovementComp GetMovementComp()
    {
        return movementComp;
    }
}
