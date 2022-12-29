using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Bot : MonoBehaviour
{
    public static Bot Instance;
    [SerializeField] public List<GameObject> player2Units;
    public int increse = 1;
    private bool first = true;
    void Awake()
    {
        Instance = this;
    }

    public void BotBeheviour()
    {
        if(GridHandler.Instance.base2.hp > 0) {
        var rnd = new System.Random();
        // проверку даже больше думаю можно ставить, а то он свою экономику апгрейдами убьёт
        if (GameHandler.Instance.ResourceP2 >= 250 * increse && increse < 3)
        {
            increse++;
            GameHandler.Instance.Upg();
        }
        //int g = rnd.Next(1, 3);
        int g = 1;
        if (UnitHandler.Instance.spawnedUnits.Count < 14)
        {
            if (GameHandler.Instance.ResourceP2 > 1000)
                GameHandler.Instance.Create(player2Units[7]);
            if (GameHandler.Instance.ResourceP2 > 450)
                GameHandler.Instance.Create(player2Units[6]);
            if (GameHandler.Instance.ResourceP2 > 500)
                GameHandler.Instance.Create(player2Units[5]);
            if (GameHandler.Instance.ResourceP2 > 400)
                GameHandler.Instance.Create(player2Units[4]);
            if (GameHandler.Instance.ResourceP2 > 350)
                GameHandler.Instance.Create(player2Units[3]);
            if (GameHandler.Instance.ResourceP2 > 350)
                GameHandler.Instance.Create(player2Units[1]);
            if (GameHandler.Instance.ResourceP2 > 150)
                GameHandler.Instance.Create(player2Units[0]);
        }
        StartCoroutine(DelayedAction(g, rnd));
    }

    IEnumerator DelayedAction(int g, System.Random rnd)
        {
            foreach (var unit2 in UnitHandler.Instance.botspawnedUnits)
            {
                if (unit2.GetComponent<BaseUnit>().Faction == Faction.Player2)
                {
                    yield return new WaitUntil(() => GameHandler.Instance.action == false);
                    UnitHandler.Instance.SetSelectedUnit(unit2);
                    if (UnitHandler.Instance.SelectedUnit != null)
                    {
                        g = rnd.Next(1, 6);
                        GridHandler.Instance.pathfinding.GetGrid().GetXY(GridHandler.Instance.base1.transform.position, out int x, out int y);
                        var savex = x;
                        var savey = y;
                        PathNode destination = GridHandler.Instance.pathfinding.GetNode(x, y);
                        var un2x = unit2.GetComponent<BaseUnit>().unitx;
                        var un2y = unit2.GetComponent<BaseUnit>().unity;
                        var un2atr = unit2.GetComponent<BaseUnit>().atribute;
                        int x2 = 0;
                        int y2 = 0;
                        foreach (var unitNewPos in UnitHandler.Instance.GetSurroundingCellsP2(savex + 14, savey + 14))
                        {
                            GridHandler.Instance.pathfinding.GetGrid().GetXY(unitNewPos, out x2, out y2);
                            if (GridHandler.Instance.pathfinding.GetNode(x2, y2).occupied != Faction.Player1 && GridHandler.Instance.pathfinding.GetNode(x2, y2).occupied != Faction.Player2)
                            {
                                x = x2;
                                y = y2;

                            }
                        }
                        if (first == true)
                        {
                            GridHandler.Instance.pathfinding.GetGrid().GetXY(GameHandler.Instance.Ipoint3.transform.position, out x, out y);
                            first = false;
                        }

                        destination = GridHandler.Instance.pathfinding.FindBotPath(un2x, un2y, x, y, un2atr); // бот решает отправить юнита брать базу(чёт я не до конца понял что и как там с baseunit - ом надо потестить)

                        // destination = GridHandler.Instance.pathfinding.FindBotPath(un2x, un2y, x , y , un2atr); // бот хочет точку3, вообще лучше бы сделать чтобы он поочереди от самой ближней до самой дальней их захватывал

                        //if (GridHandler.Instance.GetUnitAtCoordinate(destination.x, destination.y) != null && GridHandler.Instance.pathfinding.GetNode(destination.x, destination.y).occupied == Faction.Player2)
                        if (destination != null && !((un2x <= savex + 1) && (un2x >= savex - 1) && (un2y <= savey + 1) && (un2y >= savey - 1)))
                        {
                            if (GridHandler.Instance.GetUnitAtCoordinate(destination.x, destination.y) == null && GridHandler.Instance.pathfinding.GetNode(destination.x, destination.y).occupied == Faction.Player2)
                            {
                                Debug.Log("Your bot is crap, try again " + UnitHandler.Instance.SelectedUnit);
                            }
                            else if (GridHandler.Instance.pathfinding.GetNode(destination.x, destination.y).occupied == Faction.None)
                            {
                                GridHandler.Instance.Movement(new Vector3(0, 0), destination.x, destination.y);
                            }
                        }
                        yield return new WaitUntil(() => GameHandler.Instance.action == false);
                        yield return new WaitForSeconds(0.5f);
                        BotSmash(unit2);
                        yield return new WaitUntil(() => GameHandler.Instance.action == false);
                    }
                }
            }
            first = true;
            GameHandler.Instance.space = true;
            GameHandler.Instance.ChangeState(GameState.Player1Turn);
        }
    }
    public void BotSmash(GameObject unit2)
    {
        bool ch = false;
        var unit2x = unit2.GetComponent<BaseUnit>().unitx;
        var unit2y = unit2.GetComponent<BaseUnit>().unity;
        var unit2range = unit2.GetComponent<BaseUnit>().range;
        var startPointX = (int)(unit2x - unit2range);
        var startPointY = (int)(unit2y - unit2range);
        var endPointX = (int)(unit2x + unit2range);
        var endPointY = (int)(unit2y + unit2range);
        if (startPointX < 0)
            startPointX = 0;
        if (startPointY < 0)
            startPointY = 0;
        if (endPointX > 25)
            endPointX = 25;
        if (endPointY > 25)
            endPointY = 25;
        for (int i = startPointX; i < endPointX; i++)
        {
            for (int j = startPointY; j < endPointY; j++)
            {
                if (GridHandler.Instance.pathfinding.GetNode(i, j).occupied == Faction.Player1 && Mathf.Sqrt((int)Mathf.Pow((unit2x - i), 2) + (int)Mathf.Pow((unit2y - j), 2)) <= unit2range)
                {
                    if (GridHandler.Instance.GetUnitAtCoordinate(i, j) != null)
                    {
                        GridHandler.Instance.Attack(i, j);
                        ch = true;
                        break;
                    }
                }

            }
            if (ch)
                break;
        }
    }

}

