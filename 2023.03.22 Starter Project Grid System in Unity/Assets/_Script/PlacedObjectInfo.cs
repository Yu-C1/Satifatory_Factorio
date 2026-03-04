using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObjectInfo : MonoBehaviour
{
    public int ObjectID { get; private set; }
    public Vector3Int OriginCell { get; private set; }
    public IReadOnlyList<Vector3Int> OccupiedCells => occupiedCells;

    [SerializeField] private List<Vector3Int> occupiedCells = new();

    public void Init(int objectID, Vector3Int originCell, List<Vector3Int> cells)
    {
        ObjectID = objectID;
        OriginCell = originCell;
        occupiedCells = cells;
    }
}
