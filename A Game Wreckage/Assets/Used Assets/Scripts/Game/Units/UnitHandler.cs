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
        if (P1max < 15 && GameHandler.Instance.unit.GetComponent<BaseUnit>().cost <= GameHandler.Instance.ResourceP1)
        {
            GameHandler.Instance.ResourceP1 -= GameHandler.Instance.unit.GetComponent<BaseUnit>().cost;
            GameHandler.Instance.ResourceAmount.text = GameHandler.Instance.ResourceP1.ToString();
            var baseX = GridHandler.Instance.base1.transform.position.x;
            var baseY = GridHandler.Instance.base1.transform.position.y;
            Debug.Log((baseX) + ", " + (baseY));
            foreach (var unitNewPos in GetSurroundingCellsP1(baseX, baseY))
            {
                GridHandler.Instance.pathfinding.GetGrid().GetXY(unitNewPos, out x, out y);
                var node = GridHandler.Instance.pathfinding.GetNode(x, y);
                if (node.occupied is Faction.None)
                {
                    var spawnUnitP1 = Instantiate(GameHandler.Instance.unit);
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
            GameHandler.Instance.ChangeState(GameState.Player1Turn);
        }
    }

    public void SpawnPlayer2()
    {
        if (P2max < 15 && GameHandler.Instance.unit.GetComponent<BaseUnit>().cost <= GameHandler.Instance.ResourceP2)
        {
            GameHandler.Instance.ResourceP2 -= GameHandler.Instance.unit.GetComponent<BaseUnit>().cost;
            GameHandler.Instance.ResourceAmount.text = GameHandler.Instance.ResourceP2.ToString();
            var baseX = GridHandler.Instance.base2.transform.position.x;
            var baseY = GridHandler.Instance.base2.transform.position.y;
            Debug.Log((baseX) + ", " + (baseY));
            foreach (var unitNewPos in GetSurroundingCellsP2(baseX, baseY))
            {
                GridHandler.Instance.pathfinding.GetGrid().GetXY(unitNewPos, out x, out y);
                var node = GridHandler.Instance.pathfinding.GetNode(x, y);
                if (node.occupied is Faction.None)
                {
                    var spawnUnitP2 = Instantiate(GameHandler.Instance.unit);
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
            GameHandler.Instance.ChangeState(GameState.Player2Turn);
        }
    }

    public void SetSelectedUnit(GameObject u)
    {
        if (!GameHandler.Instance.action)
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
        if (!GameHandler.Instance.action)
        {
            if (SelectedUnit != null)
            {
                SelectedUnit.GetComponent<BaseUnit>().gameObject.transform.Find("Selected").gameObject.SetActive(false);
            }
            SelectedUnit = null;
        }
    }

    public IEnumerable<Vector3> GetSurroundingCellsP1(float x, float y)
    {
        var offsets = new int[,] { { 10, 0 }, { 10, 10 }, { 0, 10 }, { -10, 10 }, { -10, 0 }, { -10, -10 }, { 0, -10 }, { 10, -10 } };
        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            yield return new Vector3(x + offsets[i, 0], y + offsets[i, 1]);
        }
        yield break;
    }

    //               > > >
    // Start here -> ^ B v
    //               < < <
    public IEnumerable<Vector3> GetSurroundingCellsP2(float x, float y)
    {
        var offsets = new int[,] { { -10, 0 }, { -10, 10 }, { 0, 10 }, { 10, 10 }, { 10, 0 }, { 10, -10 }, { 0, -10 }, { -10, -10 } };
        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            yield return new Vector3(x + offsets[i, 0], y + offsets[i, 1]);
        }
        yield break;
    }
}
