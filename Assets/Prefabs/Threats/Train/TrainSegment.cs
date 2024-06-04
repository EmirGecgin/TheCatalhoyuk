using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSegment : MonoBehaviour
{

    [SerializeField] private Mesh headMesh;
    [SerializeField] private Mesh[] segmentMeshes;
    [SerializeField] private MeshFilter trainMesh;
    [SerializeField] private BoxCollider trainCollider;
    [SerializeField] private MovementComp movementComp;
    private bool bIsHead = false;
    void Start()
    {
        RandomTrainMesh();
    }

    private void RandomTrainMesh()
    {
        if (bIsHead)
        {
            return;
        }
        int pick = UnityEngine.Random.Range(0, segmentMeshes.Length);
        trainMesh.mesh = segmentMeshes[pick];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetSegmentLength()
    {
        return trainCollider.size.z;
    }

    public void SetHead()
    {
        trainMesh.mesh = headMesh;
        bIsHead = true;
    }

    public MovementComp GetMovementComp()
    {
        return movementComp;
    }
}
