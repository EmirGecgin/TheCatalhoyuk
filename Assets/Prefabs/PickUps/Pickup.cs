using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Spawnable
{
    [SerializeField] private int scoreEffect;
    [SerializeField] private float speedEffect;
    [SerializeField] private float speedEffectDuration;
    private bool bAdusted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //add score
            SpeedController speedController = FindObjectOfType<SpeedController>();
            if (speedController != null&& speedEffect!=0)
            {
                speedController.changeGlobalSpeed(speedEffect,speedEffectDuration);
            }

            ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
            if (scoreKeeper != null && scoreEffect!=0)
            {
                scoreKeeper.ChangeScore(scoreEffect);
            }
            
            PickedUpBy(other.gameObject);
        }

        if (other.gameObject.tag == "Threat" && !bAdusted)
        {
            Collider col = other.GetComponent<Collider>();
            if (col != null)
            {
                transform.position = col.bounds.center + col.bounds.extents.y * Vector3.up;
                bAdusted = true;
            }
        }
        
        
    }

    protected virtual void PickedUpBy(GameObject picker)
    {
        Destroy(gameObject);
    }
}
