using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class PathfindingDebugStepVisual : MonoBehaviour {

    public static PathfindingDebugStepVisual Instance { get; private set; }

    [SerializeField] private Transform pfPathfindingDebugStepVisualNode;
    private List<Transform> visualNodeList;
    //private List<GridSnapshotAction> gridSnapshotActionList;
    private bool autoShowSnapshots;
    private float autoShowSnapshotsTimer;
    private Transform[,] visualNodeArray; 

    private void Awake() {
        Instance = this;
        visualNodeList = new List<Transform>();
        //gridSnapshotActionList = new List<GridSnapshotAction>();
    }

    public void Setup(Grid<PathNode> grid) {
        visualNodeArray = new Transform[grid.GetWidth(), grid.GetHeight()];
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                Vector3 gridPosition = new Vector3(x, y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f;
                Transform visualNode = CreateVisualNode(gridPosition);
                visualNodeArray[x, y] = visualNode;
                visualNodeList.Add(visualNode);
            }
        }
    }
    private Transform CreateVisualNode(Vector3 position) {
        Transform visualNodeTransform = Instantiate(pfPathfindingDebugStepVisualNode, position, Quaternion.identity);
        return visualNodeTransform;
    }
}

