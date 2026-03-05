using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;
    [SerializeField]
    private GameObject gridVisualization;

    // which object occupies a given cell
    private readonly Dictionary<Vector3Int, PlacedObjectInfo> occupied = new();
    private enum ToolMode { None, Place, Remove }
    private ToolMode mode = ToolMode.None;

    private void Start()
    {
        StopAllTools();
        
    }

    public void StartPlacement (int ID)
    {
        StopAllTools();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        mode = ToolMode.Place;
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += OnLeftClick;
        inputManager.OnExit += StopAllTools;
    }

    private void PlaceStructure()
    {
        if (selectedObjectIndex < 0) return;
        // if (inputManager.IsPointerOverUI())
        // {
        //     return;
        // }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        ObjectData data = database.objectsData[selectedObjectIndex];

        // Compute footprint cells
        List<Vector3Int> footprint = GetFootprintCells(gridPosition, data.Size);

        // Enforce placement rules
        if (!CanPlace(footprint))
        {
            // optional: small feedback
            Debug.Log("Can't place here: cells occupied.");
            return;
        }
        
        if (!MoneyManager.Instance.Spend(data.Cost))
        {
            Debug.Log("Not enough money.");
            return;
        }

        // Place object
        GameObject newObject = Instantiate(data.Prefab);

        // Use center of the origin cell for clean alignment
        newObject.transform.position = grid.CellToWorld(gridPosition);

        // Track occupancy
        var info = newObject.GetComponent<PlacedObjectInfo>();
        if (info == null) info = newObject.AddComponent<PlacedObjectInfo>();
        info.Init(data.ID, gridPosition, footprint);

        foreach (var cell in footprint)
            occupied[cell] = info;    
    }

    private void Update()
    {
        if (mode == ToolMode.None)
        {
            return;            
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    private List<Vector3Int> GetFootprintCells(Vector3Int originCell, Vector2Int size)
    {
        // Assumes your game is on XZ plane, so Vector2Int(x,z)
        var result = new List<Vector3Int>(size.x * size.y);

        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                result.Add(new Vector3Int(originCell.x + x, originCell.y, originCell.z + z));
            }
        }
        return result;
    }

    private bool CanPlace(List<Vector3Int> cells)
    {
        foreach (var c in cells)
        {
            if (occupied.ContainsKey(c))
                return false;
        }
        return true;
    }

    private void RemoveStructure()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (!occupied.TryGetValue(gridPosition, out var info) || info == null)
        {
            Debug.Log("Nothing to remove here.");
            return;
        }

        // Remove all cells occupied by this object
        foreach (var cell in info.OccupiedCells)
        {
            // Safety: only remove if it points to the same object
            if (occupied.TryGetValue(cell, out var who) && who == info)
                occupied.Remove(cell);
        }

        Destroy(info.gameObject);
    }

    private void OnLeftClick()
    {
        if (inputManager.IsPointerOverUI()) return;

        if (mode == ToolMode.Place) PlaceStructure();
        else if (mode == ToolMode.Remove) RemoveStructure();
    }
    public void StopAllTools()
    {
        mode = ToolMode.None;
        selectedObjectIndex = -1;

        if (gridVisualization != null) gridVisualization.SetActive(false);
        if (cellIndicator != null) cellIndicator.SetActive(false);

        if (inputManager != null)
        {
            inputManager.OnClicked -= OnLeftClick;
            inputManager.OnExit -= StopAllTools;
        }
    }
    public void StartRemoveMode()
    {
        StopAllTools();

        mode = ToolMode.Remove;
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);

        inputManager.OnClicked += OnLeftClick;
        inputManager.OnExit += StopAllTools;
    }
}
