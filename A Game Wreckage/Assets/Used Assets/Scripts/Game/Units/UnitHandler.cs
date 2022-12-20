using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    public static UnitHandler Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void SpawnPlayer1()
    {
        var spawnUnitP1 = Instantiate(GameHandler_Setup.Instance.unit);
        spawnUnitP1.transform.position = new Vector3(25, 15.5f);
        GridHandler.Instance.pathfinding.GetNode((int)(spawnUnitP1.transform.position.x / 10), (int)(spawnUnitP1.transform.position.y / 10)).SetIsOccupied(true);
        GameHandler_Setup.Instance.ChangeState(GameState.Player1Turn);
    }

    public void SpawnPlayer2()
    {
        var spawnUnitP1 = Instantiate(GameHandler_Setup.Instance.unit);
        spawnUnitP1.transform.position = new Vector3(225, 235.5f);
        GridHandler.Instance.pathfinding.GetNode((int)(spawnUnitP1.transform.position.x / 10), (int)(spawnUnitP1.transform.position.y / 10)).SetIsOccupied(true);
        GameHandler_Setup.Instance.ChangeState(GameState.Player2Turn);
    }

}
