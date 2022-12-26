using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;

public class GridHandler : MonoBehaviour
{
    //[SerializeField] private HeatMapVisual heatMapVisual;
    //[SerializeField] private HeatMapGenericVisual heatMapGenericVisual;
    //[SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingGenericVisual pathfindingGenericVisual;
    [SerializeField] private TilemapGenericVisual tilemapGenericVisual;
    [SerializeField] private string[] saves;
    [SerializeField] public Base base1;
    [SerializeField] public Base base2;
    //private Grid<HeatMapGridObject> grid;

    public Pathfinding pathfinding;
    private Tilemap tilemap;
    private Tilemap.TilemapObject.TilemapSprite tilemapSprite = Tilemap.TilemapObject.TilemapSprite.None;
    public static GridHandler Instance;
    public GameObject GH;
    //private bool space = false;

    void Awake()
    {
        Instance = this;
    }
    public bool IsMouseOverUIWtihIgnores()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        for (int i = 0; i < raycastResults.Count; ++i)
        {
            if (raycastResults[i].gameObject.GetComponent<MouseClickThrough>() != null)
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }
        return raycastResults.Count > 0;
    }
    public void GenerateGrid()
    {
        if(GameHandler_Setup.Instance.map == 1)
        {
            base1.transform.position = new Vector3(15, 15);
            base2.transform.position = new Vector3(235, 235);
            GameHandler_Setup.Instance.Ipoint1.transform.position = new Vector3(45, 65);
            GameHandler_Setup.Instance.Ipoint2.transform.position = new Vector3(105, 105);
            GameHandler_Setup.Instance.Ipoint3.transform.position = new Vector3(165, 185);
        }
        else if(GameHandler_Setup.Instance.map == 2)
        {
            base1.transform.position = new Vector3(125, 15);
            base2.transform.position = new Vector3(125, 235);
            GameHandler_Setup.Instance.Ipoint1.transform.position = new Vector3(125, 65);
            GameHandler_Setup.Instance.Ipoint2.transform.position = new Vector3(125, 125);
            GameHandler_Setup.Instance.Ipoint3.transform.position = new Vector3(125, 185);
        }
        else if (GameHandler_Setup.Instance.map == 3)
        {
            base1.transform.position = new Vector3(125, 15);
            base2.transform.position = new Vector3(125, 235);
            GameHandler_Setup.Instance.Ipoint1.transform.position = new Vector3(125, 75);
            GameHandler_Setup.Instance.Ipoint2.transform.position = new Vector3(125, 135);
            GameHandler_Setup.Instance.Ipoint3.transform.position = new Vector3(125, 195);
        }
        //grid = new Grid<HeatMapGridObject>(2, 4, 10f, new Vector3(-30, 0), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        //heatMapVisual.SetGrid(grid);
        pathfinding = new Pathfinding(25, 25, Vector3.zero);
        //heatMapGenericVisual.SetGrid(grid);
        //pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        map = GH.GetComponent<GameHandler_Setup>().map;
        pathfinding = new Pathfinding(25, 25, Vector3.zero);
        pathfindingGenericVisual.SetGrid(pathfinding.GetGrid());
        tilemap = new Tilemap(25, 25, 10f, Vector3.zero);
        tilemap.SetTilemapVisual(tilemapGenericVisual);
        tilemap.Load("save_" + GameHandler_Setup.Instance.map);
        for (int i = 0; i < pathfinding.GetGrid().GetWidth(); i++)
        {
            for (int j = 0; j < pathfinding.GetGrid().GetHeight(); ++j)
            {

                Vector3 a = new Vector3(i * 10 + 0.1f, j * 10 + 0.1f, 0);
                Debug.Log("x:" + (i * 10 + 0.1f) + " y:" + (j * 10 + 0.1f) + " = " + tilemap.GetTilemapSprite(a));
                pathfinding.GetNode(i, j).SetIsWalkable(tilemap.GetTilemapSprite(a));
            }
        }
        Debug.Log("Loaded");
        base1.OccupieNode();
        base2.OccupieNode();
        GameHandler_Setup.Instance.ChangeState(GameState.Player1Turn);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUIWtihIgnores())
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            //HeatMapGridObject heatMapGridObject = grid.GetValue(position);
            //if (heatMapGridObject != null)
            //    heatMapGridObject.AddValue(5);
            //Debug.Log(path.Count);
            //if (path != null)
            //{

            //    //grid.SetValue(position, true);
            //}
            //tilemap.SetTilemapSprite(position, tilemapSprite);
            //if (pathfinding.GetNode(x, y) != null)
            //{
            //    pathfinding.GetNode(x, y).SetIsWalkable(tilemapSprite);
            //}
            pathfinding.GetGrid().GetXY(position, out int x, out int y);
            //List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            
            if (GameHandler_Setup.Instance.GameState == GameState.Player1Turn)
            {
                if (GetUnitAtCoordinate(x, y) != null && pathfinding.GetNode(x, y).occupied == Faction.Player1) 
                {
                    UnitHandler.Instance.SetSelectedUnit(GetUnitAtCoordinate(x, y));
                    Debug.Log("worked i think " + UnitHandler.Instance.SelectedUnit);
                }
                else if(pathfinding.GetNode(x, y).occupied == Faction.None)
                {
                    Movement(position, x, y);
                }
                else if (pathfinding.GetNode(x, y).occupied == Faction.Player2)
                {
                    Attack(x, y);
                }
            }
            if (GameHandler_Setup.Instance.GameState == GameState.Player2Turn)
            {
                if (GetUnitAtCoordinate(x, y) != null && pathfinding.GetNode(x, y).occupied == Faction.Player2)
                {
                    UnitHandler.Instance.SetSelectedUnit(GetUnitAtCoordinate(x, y));
                    Debug.Log("worked i think " + UnitHandler.Instance.SelectedUnit);
                }
                else if (pathfinding.GetNode(x, y).occupied == Faction.None)
                {
                    Movement(position, x, y);
                }
                else if (pathfinding.GetNode(x, y).occupied == Faction.Player1)
                {
                    Attack(x, y);
                }
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(position, out int x, out int y);

            Debug.Log("position: " + position + "tile: " + tilemap.GetTilemapSprite(position) + " x:" + x + " y:" + y);
            if (pathfinding.GetNode(x, y) != null)
            {
                Debug.Log("type: " + pathfinding.GetNode(x, y).type + " occupation = " + pathfinding.GetNode(x, y).occupied);
                //pathfinding.GetNode(x, y).SetIsWalkable(Tilemap.TilemapObject.TilemapSprite.Water);
            }
        }
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None)
        //        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Grass;
        //    else if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Grass)
        //        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Water;
        //    else if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Water)
        //        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Mountain;
        //    else
        //        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.None;
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    tilemap.Save();
        //    Debug.Log("saved...it seems so at least");
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    tilemap.Load("save_" + map);
        //    for (int i = 0; i < pathfinding.GetGrid().GetWidth(); i++)
        //    {
        //        for (int j = 0; j < pathfinding.GetGrid().GetHeight(); ++j)
        //        {

        //            Vector3 a = new Vector3(i * 10 + 0.1f, j * 10 + 0.1f, 0);
        //            Debug.Log("x:" + (i * 10 + 0.1f) + " y:" + (j * 10 + 0.1f) + " = " + tilemap.GetTilemapSprite(a));
        //            pathfinding.GetNode(i, j).SetIsWalkable(tilemap.GetTilemapSprite(a));
        //        }
        //    }
        //    Debug.Log("Loaded");
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //    space = true;
    }
    //Так как Update обновляется каждый фрейм, то для задержки чего-то требуется Coroutine что ниже
    IEnumerator DelayedMovement(float delayTime, List<PathNode> path)
    {
        GameHandler_Setup.Instance.action = true;
        for (int i = 0; i < path.Count; ++i)
        {
            UnitHandler.Instance.SelectedUnit.transform.position = new Vector3(path[i].x * 10 + 5, path[i].y * 10 + 5);
            if (i > 0)
            {
                Debug.DrawLine(new Vector3(path[i - 1].x, path[i - 1].y) * 10f + Vector3.one * 5f, new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, Color.yellow, 5f, false);
                Debug.Log("x1 = " + path[i - 1].x + " y1 = " + path[i - 1].y + " x2 = " + path[i].x + " y2 = " + path[i].y);
            }
            yield return new WaitForSeconds(delayTime);
        }
        GameHandler_Setup.Instance.action = false;
        //Do the action after the delay time has finished.
    }

    IEnumerator DelayedAttack(float delayTime, BaseUnit t, BaseUnit u)
    {
        GameHandler_Setup.Instance.action = true;
        bool notdead = true;
        var damaged = t.gameObject.transform.Find("Damaged").gameObject;
        var attacking = u.gameObject.transform.Find("Attacking").gameObject;
        if (t.hp <= 0)
            notdead = false;
        //Wait for the specified delay time before continuing.
        damaged.SetActive(true);
        attacking.SetActive(true);
        yield return new WaitForSeconds(delayTime);
        //Do the action after the delay time has finished.
        if(notdead)
            damaged.SetActive(false);
        attacking.SetActive(false);
        GameHandler_Setup.Instance.action = false;
    }

    public void Movement(Vector3 position, int x, int y)
    {
        if (UnitHandler.Instance.SelectedUnit != null)
        {
            var u = UnitHandler.Instance.SelectedUnit.GetComponent<BaseUnit>();


            List<PathNode> path = pathfinding.FindPath(u.unitx, u.unity, x, y, u.atribute);
            if (path != null)
            {
                int cost = pathfinding.CalculateDistanceCost(pathfinding.GetGrid().GetValue(u.unitx, u.unity), pathfinding.GetGrid().GetValue(x, y));
                if (u.currentstamina > cost)
                {
                    pathfinding.GetNode(UnitHandler.Instance.SelectedUnit.GetComponent<BaseUnit>().unitx, UnitHandler.Instance.SelectedUnit.GetComponent<BaseUnit>().unity).SetIsOccupied(Faction.None);

                    //for (int i = 0; i < path.Count; ++i)
                    //{
                    //    UnitHandler.Instance.SelectedUnit.transform.position = new Vector3(path[i].x * 10 + 5, path[i].y * 10 + 5);
                    //    if (i > 0)
                    //    {
                    //        Debug.DrawLine(new Vector3(path[i - 1].x, path[i - 1].y) * 10f + Vector3.one * 5f, new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, Color.yellow, 5f, false);
                    //        Debug.Log("x1 = " + path[i - 1].x + " y1 = " + path[i - 1].y + " x2 = " + path[i].x + " y2 = " + path[i].y);
                    //    }
                    //Переместил все в coroutine, чтобы отрисовывалось постепенно и можно было просмотреть как он собственно идет
                    StartCoroutine(DelayedMovement(0.25f, path));
                    //}
                    UnitHandler.Instance.SelectedUnit.GetComponent<BaseUnit>().unitx = x;
                    UnitHandler.Instance.SelectedUnit.GetComponent<BaseUnit>().unity = y;
                    pathfinding.GetNode(x, y).SetIsOccupied(u.Faction);
                    u.currentstamina -= cost;
                }
            }
        }
    }
    public void Attack(int x, int y)
    {
        if (UnitHandler.Instance.SelectedUnit != null)
        {
            var u = UnitHandler.Instance.SelectedUnit.GetComponent<BaseUnit>();
            var t = GetUnitAtCoordinate(x, y).GetComponent<BaseUnit>();
            if (u.attacked == false && Mathf.Sqrt((t.unitx - u.unitx) ^ 2 + (t.unity - u.unity) ^ 2) <= u.range)
            {
                Debug.Log("Расстояние между ними собственно = " + Mathf.Sqrt((t.unitx - u.unitx) ^ 2 + (t.unity - u.unity) ^ 2));
                
                
                //sprite[0].gameObject.SetActive(true);

                //.Find(x => x.name == "Damaged");
                t.hp -= u.attack;
                u.attacked = true;
                //sprite.SetActive(true);
                StartCoroutine(DelayedAttack(0.5f, t, u));
                t.IsDead();
            }
        }
    }
    public GameObject GetUnitAtCoordinate(int x, int y)
    {
        foreach (GameObject g in UnitHandler.Instance.spawnedUnits)
        {
            if (g.GetComponent<BaseUnit>().unitx == x && g.GetComponent<BaseUnit>().unity == y)
            {
                return g; // match found
            }
            
        }
        return null; // no match found
    }
}
//public class HeatMapGridObject{
//    private const int MIN = 0;
//    private const int MAX = 100;
//    public int value;

//    private Grid<HeatMapGridObject> grid;
//    private int x;
//    private int y;

//    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
//    {
//        this.grid = grid;
//        this.x = x;
//        this.y = y;
//    }
//    public void AddValue(int addValue) {
//        value += addValue;
//        Mathf.Clamp(value, MIN, MAX);
//        grid.TriggeredGridObjectChanged(x, y);
//    }
//    public float GetValueNormalized()
//    {
//        return (float)value / MAX;
//    }

//    public override string ToString()
//    {
//        return value.ToString();
//    }
//}
