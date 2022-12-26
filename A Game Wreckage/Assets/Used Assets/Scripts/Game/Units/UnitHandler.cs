using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class UnitHandler : MonoBehaviour
{
    public static UnitHandler Instance;
    public List<GameObject> spawnedUnits = new List<GameObject>();
    public int P1max = 0, P2max = 0;
    public GameObject SelectedUnit = null;
    int x, y;
    private void Awake()
    {
        Instance = this;
    }
    public void SpawnPlayer1()
    {
        if (P1max < 15)
        {
            var spawnUnitP1 = Instantiate(GameHandler_Setup.Instance.unit);
            P1max++;
            spawnedUnits.Add(spawnUnitP1);
            //spawnUnitP1.GetComponent<BaseUnit>().currentstamina = spawnUnitP1.GetComponent<BaseUnit>().stamina;
            spawnUnitP1.transform.position = new Vector3(25, 15.5f);
            GridHandler.Instance.pathfinding.GetGrid().GetXY(new Vector3(25, 15.5f), out x, out y);
            GridHandler.Instance.pathfinding.GetNode(x, y).SetIsOccupied(Faction.Player1);
            spawnUnitP1.GetComponent<BaseUnit>().unitx = x;
            spawnUnitP1.GetComponent<BaseUnit>().unity = y;
            GameHandler_Setup.Instance.ChangeState(GameState.Player1Turn);
        }
    }

    public void SpawnPlayer2()
    {
        if (P2max < 15)
        {
            var spawnUnitP1 = Instantiate(GameHandler_Setup.Instance.unit);
            P2max++;
            spawnedUnits.Add(spawnUnitP1);
            spawnUnitP1.transform.position = new Vector3(225, 235.5f);
            GridHandler.Instance.pathfinding.GetGrid().GetXY(new Vector3(225, 235.5f), out x, out y);
            GridHandler.Instance.pathfinding.GetNode(x, y).SetIsOccupied(Faction.Player2);
            spawnUnitP1.GetComponent<BaseUnit>().unitx = x;
            spawnUnitP1.GetComponent<BaseUnit>().unity = y;
            GameHandler_Setup.Instance.ChangeState(GameState.Player2Turn);
        }
    }

    public void SetSelectedUnit(GameObject u)
    {
        if (!GameHandler_Setup.Instance.action)
        {
            if (SelectedUnit != null)
            {
                SelectedUnit.GetComponent<BaseUnit>().gameObject.transform.Find("Selected").gameObject.SetActive(false);
            }
            SelectedUnit = u;
            SelectedUnit.GetComponent<BaseUnit>().gameObject.transform.Find("Aknowledged").gameObject.GetComponent<AudioSource>().Play();
            SelectedUnit.GetComponent<BaseUnit>().gameObject.transform.Find("Selected").gameObject.SetActive(true);
        }
    }

    public void UnselectUnit()
    {
        if (!GameHandler_Setup.Instance.action)
        {
            if (SelectedUnit != null)
            {
                SelectedUnit.GetComponent<BaseUnit>().gameObject.transform.Find("Selected").gameObject.SetActive(false);
            }
            SelectedUnit = null;
        }
    }
}
