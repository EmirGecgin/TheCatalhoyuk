using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private TrainSegment segmentPrefab;
    [SerializeField] private Vector2 segmentCountRange;
    [SerializeField] private Threat _threat;
    void Start()
    {
        GenerateTrainBody();
    }

    private void GenerateTrainBody()
    {
        int bodyCount = Random.Range((int)segmentCountRange.x,(int) segmentCountRange.y);
        for (int i = 0; i < bodyCount; i++)
        {
            Vector3 spawnPos = transform.position + transform.forward * segmentPrefab.GetSegmentLength() * i;
            TrainSegment newSegment = Instantiate(segmentPrefab, spawnPos, Quaternion.identity);
            if (i == 0)
            {
                newSegment.SetHead();
            }

            newSegment.GetMovementComp().CopyFrom(_threat.GetMovementComp());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
