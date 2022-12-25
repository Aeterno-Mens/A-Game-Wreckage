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
    //статы юнита
    public int stamina;
    public float range;
    public int hp;
    public int attack;
    public int atribute;
    public Faction Faction;
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
            Destroy(this.gameObject);
        }
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        moving = true;
    //    }
    //    if (!moving)
    //        return;
    //    if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
    //    {
    //        //grid.SetValue(UtilsClass.GetMouseWorldPosition(), true);
    //        Vector3 position = UtilsClass.GetMouseWorldPosition();
    //        GridHandler.Instance.pathfinding.GetGrid().GetXY(position, out int x, out int y);
    //        List<PathNode> path = GridHandler.Instance.pathfinding.FindPath(this.unitx, this.unity, x, y);
    //        if (path != null)
    //        {
    //            for (int i = 0; i < path.Count; ++i)
    //            {
    //                this.unitx = path[i].x;
    //                this.unity = path[i].y;
    //                //DoDelayAction(1.0f);
    //            }
    //        }
    //    }
    //}
}
