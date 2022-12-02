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

    public void SpawnPlayer() {
        var unitCount = 1;

        for (int i = 0; i < unitCount; i++) {
            var randomPrefab = GetRandomUnit<BasePlayer>(Faction.Player);
            var spawnedUnit = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetUnitSpawnTile();

            randomSpawnTile.SetUnit(spawnedUnit);
        }

        GameManager.Instance.ChangeState(GameState.SpawnEnemy);
    }

    public void SpawnEnemy() {
        var unitCount = 1;

        for (int i = 0; i < unitCount; i++) {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedUnit = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetUnitSpawnTile();

            randomSpawnTile.SetUnit(spawnedUnit);
        }

        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedPlayerUnit(BasePlayer unit) {
        SelectedUnit = unit;
        MenuManager.Instance.ShowSelectedUnit(unit);
    }

}
