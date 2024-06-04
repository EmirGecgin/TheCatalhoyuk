using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    [Serializable]
    public struct RoadSpawnDefination
    {
        public GameObject roadBlock;
        public float weight;
    }

    [Header("Road Blocks")]
    [SerializeField] private Transform StartingPoint;
    [SerializeField] private Transform EndPoint;
    [SerializeField] RoadSpawnDefination[] roadBlocks;
    private float roadWeightTotalWeight;
    [Header("Buildings")]

    [SerializeField] GameObject[] buildings;
    [SerializeField] Transform[] buildingSpawnPoints;
    [SerializeField] Vector2 buildingSpawnScaleRange = new Vector2(0.6f, 0.8f);
    [SerializeField] Vector3 StreetSpawnScaleRange = new Vector3(0.6f, 1.4f, 1.5f);

    [Header("Street Lights")]
    [SerializeField] private GameObject StreetLight;
    [SerializeField] private Transform[] StreetSpawnPoints;

    [Header("Threats")]
    [SerializeField] private Threat[] Threats;
    [SerializeField] private Transform[] Lanes;
    [SerializeField] private Vector3 OccupationDetectionHalfExtend;
    Vector3 MoveDirection;
    [Header("Pickups")]
    [SerializeField] private Pickup[] _pickups;

    bool GetRandomSpawnPoint(out Vector3 spawnPoint, string occupationCheckTag)
    {
        Vector3[] spawnPoints = GetAvaliableSpawnPoints(occupationCheckTag);
        if (spawnPoints.Length == 0)
        {
            spawnPoint = new Vector3(0, 0, 0);
            return false;
        }
        int pick = Random.Range(0, spawnPoints.Length);
        spawnPoint = spawnPoints[pick];
        return true;
    }
    Vector3[] GetAvaliableSpawnPoints(string occupationCheckTag)
    {
        List<Vector3> AvaliableSpawnPoints = new List<Vector3>();
        foreach (Transform SpawnTrans in Lanes)
        {
            Vector3 spawnPoint = SpawnTrans.position + new Vector3(0, 0, StartingPoint.position.z);
            if (!GameplayStatics.IsPositionOccupied(spawnPoint, OccupationDetectionHalfExtend, occupationCheckTag))
            {
                AvaliableSpawnPoints.Add(spawnPoint);
            }
        }
        return AvaliableSpawnPoints.ToArray();
    }
    void Start()
    {
        roadWeightTotalWeight = 0;
        for (int i = 0; i < roadBlocks.Length; i++)
        {
            roadWeightTotalWeight += roadBlocks[i].weight;
        }
        StartCoroutine(IncreaseSecondRoadBlockWeight());

        Vector3 nextBlockPosition = StartingPoint.position;
        float EndPointDistance = Vector3.Distance(StartingPoint.position, EndPoint.position);
        MoveDirection = (EndPoint.position - StartingPoint.position).normalized;
        while (Vector3.Distance(StartingPoint.position, nextBlockPosition) < EndPointDistance)
        {
            GameObject pickedBlock = roadBlocks[0].roadBlock;
            GameObject newBlock = Instantiate(pickedBlock);
            newBlock.transform.position = nextBlockPosition;
            MovementComp moveComp = newBlock.GetComponent<MovementComp>();
            if (moveComp != null)
            {
                moveComp.SetDestination(EndPoint.position);
                moveComp.SetMoveDir(MoveDirection);
            }

            SpawnBuildings(newBlock);
            SpawnStreetLight(newBlock);

            float blockLength = newBlock.GetComponent<MeshRenderer>().bounds.size.z;
            nextBlockPosition += MoveDirection * blockLength;
        }
        StartSpawnElements();
        Pickup newPickup = Instantiate(_pickups[0], StartingPoint.position, Quaternion.identity);
        newPickup.GetComponent<MovementComp>().SetDestination(EndPoint.position);
        newPickup.GetComponent<MovementComp>().SetMoveDir(MoveDirection);
    }

    private void StartSpawnElements()
    {
        foreach (Threat threat in Threats)
        {
            StartCoroutine(SpawnElement(threat));
        }

        foreach (Pickup pickup in _pickups)
        {
            StartCoroutine(SpawnElement(pickup));
        }
    }
    IEnumerator SpawnElement(Spawnable elementToSpawn)
    {
        while (true)
        {
            if (GetRandomSpawnPoint(out Vector3 spawnPoint, elementToSpawn.gameObject.tag))
            {
                Spawnable newThreat = Instantiate(elementToSpawn, spawnPoint, Quaternion.identity);

                newThreat.GetMovementComp().SetDestination(EndPoint.position);
                newThreat.GetMovementComp().SetMoveDir(MoveDirection);
            }

            yield return new WaitForSeconds(elementToSpawn.SpawnInterval);
        }
    }
    GameObject SpawnNewBlock(Vector3 SpawnPosition, Vector3 MoveDir)
    {
        GameObject pickedBlock = GetRandomBlockToSpawn();
        GameObject newBlock = Instantiate(pickedBlock);
        newBlock.transform.position = SpawnPosition;
        MovementComp moveComp = newBlock.GetComponent<MovementComp>();
        if (moveComp != null)
        {
            moveComp.SetDestination(EndPoint.position);
            moveComp.SetMoveDir(MoveDir);
        }

        SpawnBuildings(newBlock);
        SpawnStreetLight(newBlock);

        return newBlock;
    }

    private GameObject GetRandomBlockToSpawn()
    {
        float pickWeight = Random.Range(0, roadWeightTotalWeight);
        float totalWeightSoFar = 0;
        int pick = 0;
        for (int i = 0; i < roadBlocks.Length; i++)
        {
            totalWeightSoFar += roadBlocks[i].weight;
            if (pickWeight < totalWeightSoFar)
            {
                pick = i;
                break;
            }
        }
        return roadBlocks[pick].roadBlock;
    }

    private void SpawnStreetLight(GameObject ParentBlock)
    {
        foreach (Transform StreetLightsSpawnPoint in StreetSpawnPoints)
        {
            Vector3 BuildingSpawnSize = Vector3.one;

            Vector3 spawnLoc = ParentBlock.transform.position + (StreetLightsSpawnPoint.position - StartingPoint.position);
            spawnLoc.y = 0.4f; // Set the Y-coordinate to the ground level

            Quaternion spawnRot = Quaternion.LookRotation((StartingPoint.position - StreetLightsSpawnPoint.position).normalized, Vector3.up);
            Quaternion spawnRotOffset = Quaternion.Euler(-90, 0, 0);

            GameObject newStreetLight = Instantiate(StreetLight, spawnLoc, spawnRot * spawnRotOffset, ParentBlock.transform);
            newStreetLight.transform.localScale = new Vector3(4f, Random.Range(4.20f, 4.40f), Random.Range(2.20f, 2.40f));
        }
    }

    private void SpawnBuildings(GameObject ParentBlock)
    {
        foreach (Transform BuildingSpawnPoint in buildingSpawnPoints)
        {
            Vector3 BuildingSpawnLoc = ParentBlock.transform.position + (BuildingSpawnPoint.position - StartingPoint.position);
            int RotationOffsetBy90 = Random.Range(0, 3);
            Quaternion BuildingSpawnRotation = Quaternion.Euler(0, RotationOffsetBy90 * 90, 0);
            Vector3 BuildingSpawnSize = Vector3.one * Random.Range(buildingSpawnScaleRange.x, buildingSpawnScaleRange.y);
            int BuildingPick = Random.Range(0, buildings.Length);

            GameObject newBuilding = Instantiate(buildings[BuildingPick], BuildingSpawnLoc, BuildingSpawnRotation,
                ParentBlock.transform);
            newBuilding.transform.localScale = BuildingSpawnSize / 90f;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("output");
        if (other.gameObject != null && other.gameObject.tag == "RoadBlock")
        {
            GameObject newBlock = SpawnNewBlock(other.transform.position, MoveDirection);
            float newBlockHalfWidth = newBlock.GetComponent<Renderer>().bounds.size.z / 2f;
            float previousBlockHalfWidth = other.GetComponent<Renderer>().bounds.size.z / 2f;
            Vector3 newBlockSpawnOffset = -(newBlockHalfWidth + previousBlockHalfWidth) * MoveDirection;
            newBlock.transform.position += newBlockSpawnOffset;
        }
    }

    private IEnumerator IncreaseSecondRoadBlockWeight()
    {
        while (true)
        {
            yield return new WaitForSeconds(11f);
            roadBlocks[1].weight += 0.5f;
            roadBlocks[4].weight += 0.5f;
            roadBlocks[3].weight += 0.5f;

            roadBlocks[2].weight += 0.25f;
            roadWeightTotalWeight += 0.5f;
        }
    }
}

