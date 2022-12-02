using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    //[SerializeField] private HeatMapVisual heatMapVisual;
    [SerializeField] private HeatMapGenericVisual heatMapGenericVisual;
    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingGenericVisual pathfindingGenericVisual;
    private Grid<HeatMapGridObject> grid;
    private Pathfinding pathfinding;
    void Start()
    {
        grid = new Grid<HeatMapGridObject>(2, 4, 10f, new Vector3(-80,0), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        //heatMapVisual.SetGrid(grid);
        pathfinding = new Pathfinding(10, 10, new Vector3(-50, -50));
        heatMapGenericVisual.SetGrid(grid);
        pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        pathfindingGenericVisual.SetGrid(pathfinding.GetGrid());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //grid.SetValue(UtilsClass.GetMouseWorldPosition(), true);
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            HeatMapGridObject heatMapGridObject = grid.GetValue(position);
            if (heatMapGridObject != null)
                heatMapGridObject.AddValue(5);
            pathfinding.GetGrid().GetXY(position, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            //Debug.Log(path.Count);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; ++i)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
                }
                //grid.SetValue(position, true);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(position, out int x, out int y);
            pathfinding.GetNode(x,y).SetIsWalkable(!pathfinding.GetNode(x,y).isWalkable);
        }
    }
}
public class HeatMapGridObject{
    private const int MIN = 0;
    private const int MAX = 100;
    public int value;

    private Grid<HeatMapGridObject> grid;
    private int x;
    private int y;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    public void AddValue(int addValue) {
        value += addValue;
        Mathf.Clamp(value, MIN, MAX);
        grid.TriggeredGridObjectChanged(x, y);
    }
    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
