using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;
public class BaseUnit : MonoBehaviour
{
    public string UnitName;
    public int x;
    public int y;
    public int stamina;
    public int range;
    public int hp;
    public int attack;
    public Faction Faction;
    public bool moving = false;

    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            moving = true;
        }
        if (!moving)
            return;
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            //grid.SetValue(UtilsClass.GetMouseWorldPosition(), true);
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            GridHandler.Instance.pathfinding.GetGrid().GetXY(position, out int x, out int y);
            List<PathNode> path = GridHandler.Instance.pathfinding.FindPath(this.x, this.y, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count; ++i)
                {
                    this.x = path[i].x;
                    this.y = path[i].y;
                    DoDelayAction(1.0f);
                }
            }
        }
    }
    void DoDelayAction(float delayTime)
    {
        StartCoroutine(DelayAction(delayTime));
    }

    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.
    }
}
