using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;
public class BaseUnit : MonoBehaviour
{
    //координаты с которыми мы будем работать
    public int unitx;
    public int unity;
    public int tier;
    //статы юнита
    public int cost;
    public int stamina;
    public float range;
    public int hp;
    public int attack;
    public int atribute;
    public Faction Faction;
    //public HealthBar health;
    //различные булевые переменные для контроля
    public int currentstamina;
    public bool moving = false;
    public bool attacked = false;

    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void IsDead()
    {
        if (this.hp <= 0)
        {
            if(this.Faction == Faction.Player1)
            {
                UnitHandler.Instance.P1max--;
            }
            else
            {
                UnitHandler.Instance.P2max--;
            }
            UnitHandler.Instance.spawnedUnits.Remove(gameObject);
            if (GameHandler.Instance.bot)
            {
                UnitHandler.Instance.botspawnedUnits.Remove(gameObject);
            }
            GridHandler.Instance.pathfinding.GetNode(unitx, unity).occupied = Faction.None;
            Destroy(gameObject);
        }
    }
}