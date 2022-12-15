using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;

public class GridHandler : MonoBehaviour
{
    //[SerializeField] private HeatMapVisual heatMapVisual;
    [SerializeField] private HeatMapGenericVisual heatMapGenericVisual;
    //[SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingGenericVisual pathfindingGenericVisual;
    [SerializeField] private TilemapGenericVisual tilemapGenericVisual;
    [SerializeField] private string[] saves;
    
    private Grid<HeatMapGridObject> grid;
    private Pathfinding pathfinding;
    private Tilemap tilemap;
    private Tilemap.TilemapObject.TilemapSprite tilemapSprite = Tilemap.TilemapObject.TilemapSprite.None;
    public static GridHandler Instance;
    public GameObject GH;
    public int map;
    void Awake()
    {
        Instance = this;
    }
    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    public void GenerateGrid()
    {
        map = GH.GetComponent<GameHandler_Setup>().map;
        grid = new Grid<HeatMapGridObject>(2, 4, 10f, new Vector3(-30,0), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        //heatMapVisual.SetGrid(grid);
        pathfinding = new Pathfinding(25, 25, Vector3.zero);
        heatMapGenericVisual.SetGrid(grid);
        //pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        pathfindingGenericVisual.SetGrid(pathfinding.GetGrid());
        tilemap = new Tilemap(25, 25, 10f, Vector3.zero);//new Vector3(0, -110));
        tilemap.SetTilemapVisual(tilemapGenericVisual);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
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
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x , path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
                    Debug.Log("x1 = " + path[i].x + " y1 = " + path[i].y + " x2 = " + path[i + 1].x + " y2 = " + path[i + 1].y);
                }
                //grid.SetValue(position, true);
            }
            tilemap.SetTilemapSprite(position, tilemapSprite);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(tilemap.GetTilemapSprite(UtilsClass.GetMouseWorldPosition()));
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(position, out int x, out int y);
            pathfinding.GetNode(x,y).SetIsWalkable(2);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None)
                tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Grass;
            else if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Grass)
                tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Water;
            else if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Water)
                tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Mountain;
            else
                tilemapSprite = Tilemap.TilemapObject.TilemapSprite.None;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            tilemap.Save();
            Debug.Log("saved...it seems so at least");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            tilemap.Load("save_"+map);
            Debug.Log("Loaded");
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
