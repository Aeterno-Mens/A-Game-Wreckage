using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;
    public BasePlayer SelectedUnit;

    void Awake() {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnPlayer1() {
        var unitCount = 1;

        for (int i = 0; i < unitCount; i++) {
            var randomPrefab = GetRandomUnit<BasePlayer>(Faction.Player1);
            var spawnedUnit = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetUnitSpawnTile();

            randomSpawnTile.SetUnit(spawnedUnit);
        }

        GameManager.Instance.ChangeState(GameState.SpawnPlayer2);
    }

    public void SpawnPlayer2() {
        var unitCount = 1;

        for (int i = 0; i < unitCount; i++) {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Player2);
            var spawnedUnit = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetUnitSpawnTile();

            randomSpawnTile.SetUnit(spawnedUnit);
        }

        GameManager.Instance.ChangeState(GameState.Player1Turn);
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedPlayer1Unit(BasePlayer unit) {
        SelectedUnit = unit;
        MenuManager.Instance.ShowSelectedUnit(unit);
    }

}
