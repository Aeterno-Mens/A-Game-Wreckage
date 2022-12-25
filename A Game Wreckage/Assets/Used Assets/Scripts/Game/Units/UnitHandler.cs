using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class UnitHandler : MonoBehaviour
{
    public static UnitHandler Instance;
    public List<GameObject> spawnedUnits = new List<GameObject>();
    private int P1max = 0, P2max = 0;
    public GameObject SelectedUnit = null;
    int x, y;
    private void Awake()
    {
        Instance = this;
    }
    // TODO: spawn units in 1-wide zone around base
    //      ***
    //      *B*
    //      *** 
    public void SpawnPlayer1()
    {
        if (P1max < 15)
        {
            var baseX = GridHandler.Instance.base1.transform.position.x;
            var baseY = GridHandler.Instance.base1.transform.position.y;
            Debug.Log((baseX) + ", " + (baseY));
            foreach (var unitNewPos in GetSurroundingCellsP1(baseX, baseY)) 
            {
                GridHandler.Instance.pathfinding.GetGrid().GetXY(unitNewPos, out x, out y);
                var node = GridHandler.Instance.pathfinding.GetNode(x, y);
                if (node.occupied is Faction.None)
                {
                    var spawnUnitP1 = Instantiate(GameHandler_Setup.Instance.unit);
                    P1max++;
                    spawnedUnits.Add(spawnUnitP1);
                    spawnUnitP1.transform.position = unitNewPos;
                    node.SetIsOccupied(Faction.Player1);
                    spawnUnitP1.GetComponent<BaseUnit>().unitx = x;
                    spawnUnitP1.GetComponent<BaseUnit>().unity = y;
                    Debug.Log("Successfuly created unit: " + spawnUnitP1);
                    break;
                }
            }
            GameHandler_Setup.Instance.ChangeState(GameState.Player1Turn);
        }
    }

    public void SpawnPlayer2()
    {
        if (P2max < 15)
        {
            var baseX = GridHandler.Instance.base2.transform.position.x;
            var baseY = GridHandler.Instance.base2.transform.position.y;
            Debug.Log((baseX) + ", " + (baseY));
            foreach (var unitNewPos in GetSurroundingCellsP2(baseX, baseY)) 
            {
                GridHandler.Instance.pathfinding.GetGrid().GetXY(unitNewPos, out x, out y);
                var node = GridHandler.Instance.pathfinding.GetNode(x, y);
                if (node.occupied is Faction.None)
                {
                    var spawnUnitP2 = Instantiate(GameHandler_Setup.Instance.unit);
                    P2max++;
                    spawnedUnits.Add(spawnUnitP2);
                    spawnUnitP2.transform.position = unitNewPos;
                    node.SetIsOccupied(Faction.Player2);
                    spawnUnitP2.GetComponent<BaseUnit>().unitx = x;
                    spawnUnitP2.GetComponent<BaseUnit>().unity = y;
                    Debug.Log("Successfuly created unit: " + spawnUnitP2);
                    break;
                }
            }
            GameHandler_Setup.Instance.ChangeState(GameState.Player2Turn);
        }
    }

    public void SetSelectedUnit(GameObject u)
    {
        SelectedUnit = u;
    }
/*
    public IEnumerable<Vector3> GetSurroundingCells(float x, float y) 
    {
        yield return new Vector3(x + 10, y);
        for (int idx = 10; idx > -20; idx -= 10) 
        {                                               
            yield return new Vector3(x + idx, y + 10);  
        }                                               
        yield return new Vector3(x - 10, y);
        for (int idx = -10; idx < 20; idx += 10) 
        {                                               
            yield return new Vector3(x + idx, y - 10);  
        }                                               
        yield break;
    }

*/

    // < < <
    // v B ^  <- Start here
    // > > >
    public IEnumerable<Vector3> GetSurroundingCellsP1(float x, float y) 
    {
        var offsets = new int[,] {{10,0},{10,10},{0,10},{-10,10},{-10,0},{-10,-10},{0,-10},{10,-10}};
        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            yield return new Vector3(x + offsets[i,0], y + offsets[i,1]);
        }
        yield break;
    }
    
    //               > > >
    // Start here -> ^ B v
    //               < < <
    public IEnumerable<Vector3> GetSurroundingCellsP2(float x, float y) 
    {
        var offsets = new int[,] {{-10,0},{-10,10},{0,10},{10,10},{10,0},{10,-10},{0,-10},{-10,-10}};
        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            yield return new Vector3(x + offsets[i,0], y + offsets[i,1]);
        }
        yield break;
    }
}
