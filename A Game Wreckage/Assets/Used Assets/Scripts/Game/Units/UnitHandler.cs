using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    public static UnitHandler Instance;
    public List<GameObject> spawnedUnits = new List<GameObject>();
    public GameObject SelectedUnit;
    int x, y;
    private void Awake()
    {
        Instance = this;
    }
    public void SpawnPlayer1()
    {
        int x, y;
        var spawnUnitP1 = Instantiate(GameHandler_Setup.Instance.unit);
        spawnedUnits.Add(spawnUnitP1);
        spawnUnitP1.transform.position = new Vector3(25, 15.5f);
        GridHandler.Instance.pathfinding.GetGrid().GetXY(new Vector3(25, 15.5f), out x, out y);
        GridHandler.Instance.pathfinding.GetNode(x, y).SetIsOccupied(Faction.Player1);
        spawnUnitP1.GetComponent<BaseUnit>().unitx = x;
        spawnUnitP1.GetComponent<BaseUnit>().unity = y;
        GameHandler_Setup.Instance.ChangeState(GameState.Player1Turn);
    }

    public void SpawnPlayer2()
    {
        var spawnUnitP1 = Instantiate(GameHandler_Setup.Instance.unit);
        spawnedUnits.Add(spawnUnitP1);
        spawnUnitP1.transform.position = new Vector3(225, 235.5f);
        GridHandler.Instance.pathfinding.GetGrid().GetXY(new Vector3(225, 235.5f), out x, out y);
        GridHandler.Instance.pathfinding.GetNode((int)(spawnUnitP1.transform.position.x / 10), (int)(spawnUnitP1.transform.position.y / 10)).SetIsOccupied(Faction.Player2);
        spawnUnitP1.GetComponent<BaseUnit>().unitx = x;
        spawnUnitP1.GetComponent<BaseUnit>().unity = y;
        GameHandler_Setup.Instance.ChangeState(GameState.Player2Turn);
    }

    public void SetSelectedUnit(GameObject u)
    {
        SelectedUnit = u;
    }

}
