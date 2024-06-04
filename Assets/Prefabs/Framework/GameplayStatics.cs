using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplayStatics
{
    static private GameMode _gameMode;
    public static bool IsPositionOccupied(Vector3 position,Vector3 detectionHalfExtend, string occupationCheckTag)
    {
        Collider[] cols= Physics.OverlapBox(position, detectionHalfExtend);
        foreach(Collider col in cols)
        {
            if (col.gameObject.tag == occupationCheckTag || col.gameObject.tag=="NoSpawn")
            {
                return true;
            }
        }
        return false;
    }

    public static GameMode GetGameMode()
    {
        if (_gameMode == null)
        {
            _gameMode = GameObject.FindObjectOfType<GameMode>();
        }

        return _gameMode;
    }
}
