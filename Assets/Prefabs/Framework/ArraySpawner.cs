using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArraySpawner : MonoBehaviour
{
    [SerializeField] private int amt = 10;
    [SerializeField] private float gap = 1f;
    void Start()
    {
        for(int i = 1;i<amt;i++)
        {
            Vector3 spawnPos = new Vector3(transform.position.x,0f,transform.position.z) + transform.forward * gap * i;
            GameObject nextCoin = Instantiate(gameObject,spawnPos,Quaternion.identity);
            nextCoin.GetComponent<ArraySpawner>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
