using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    public BaseUnit OccupiedUnit;


    public virtual void Init(int x, int y)
    {

    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    void OnMouseDown() {
        if (GameManager.Instance.GameState != GameState.PlayerTurn) return;

        if (OccupiedUnit != null) {
            if (OccupiedUnit.Faction == Faction.Player) UnitManager.Instance.SetSelectedPlayerUnit((BasePlayer)OccupiedUnit);
            else {
                if (UnitManager.Instance.SelectedUnit != null) {
                    var enemy = (BaseEnemy) OccupiedUnit;
                    Destroy(enemy.gameObject);
                    UnitManager.Instance.SetSelectedPlayerUnit(null);
                }
            }
        }
        else {
            if (UnitManager.Instance.SelectedUnit != null) {
                SetUnit(UnitManager.Instance.SelectedUnit);
                UnitManager.Instance.SetSelectedPlayerUnit(null);
            }
        }

    }

    public void SetUnit(BaseUnit unit) {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
}
