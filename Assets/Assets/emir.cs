using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emir : MonoBehaviour
{
    [SerializeField] private GameObject controlUI;
    void Start()
    {
        StartCoroutine(aX());

    }
    private IEnumerator aX()
    {
        
        yield return new WaitForSeconds(12);
        controlUI.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
