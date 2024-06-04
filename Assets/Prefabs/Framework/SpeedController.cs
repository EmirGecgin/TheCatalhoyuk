using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public delegate void OnGlobalSpeedChanged(float newSpeed);
    [SerializeField] private float globalSpeed = 15f;
    [SerializeField] private InGameUI inGameUI;
    private int lastMeterCheck = 0; // Son kontrol edilen metre

    public event OnGlobalSpeedChanged onGlobalSpeedChanged;

    private void Update()
    {
        OnMeterChanged(inGameUI.currentMeter);
    }

    public void changeGlobalSpeed(float speedChange, float duration)
    {
        globalSpeed += speedChange;
        InformSpeedChange();
        StartCoroutine(RemoveSpeedChange(speedChange, duration));
    }

    public float GetGlobalSpeed()
    {
        return globalSpeed;
    }

    IEnumerator RemoveSpeedChange(float SpeedChangedAmt, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        globalSpeed -= SpeedChangedAmt;
        InformSpeedChange();
    }

    private void InformSpeedChange()
    {
        onGlobalSpeedChanged?.Invoke(globalSpeed);

    }
    public void OnMeterChanged(int currentMeter)
    {
        // 50 birimlik aral�klarla kontrol et
        if (currentMeter >= lastMeterCheck + 90)
        {
            lastMeterCheck = currentMeter; // Son kontrol edilen metre g�ncelle
            globalSpeed += 1.5f; // globalSpeed'i art�r
            InformSpeedChange(); // De�i�iklikleri bildir
        }
    }


}