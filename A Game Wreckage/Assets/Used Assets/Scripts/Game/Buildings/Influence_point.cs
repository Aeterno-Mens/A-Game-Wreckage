using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Influence_point : MonoBehaviour
{
    public static Influence_point Instance;
    public bool check;
    public int cp;
    public GameObject UI;
    public Color startcolor;
    public Faction Faction;
    // Start is called before the first frame update

    private void Awake()
    {
        startcolor = GetComponent<SpriteRenderer>().color;
        Instance = this;
        this.cp = 2;
    }

    private void Update()
    {
        if (check)
        {
            AtEndTurn();
        }
    }
    public void AtEndTurn()
    {
        if (GridHandler.Instance.pathfinding.GetNode((int)(this.transform.position.x / 10), (int)(this.transform.position.y / 10)).occupied != Faction.None)
        {
            if ((GameHandler_Setup.Instance.GameState == GameState.Player2Turn && GridHandler.Instance.pathfinding.GetNode((int)(this.transform.position.x / 10), (int)(this.transform.position.y / 10)).occupied == Faction.Player1) && this.cp < 4){
                this.cp++;
            }

            if ((GameHandler_Setup.Instance.GameState == GameState.Player1Turn && GridHandler.Instance.pathfinding.GetNode((int)(this.transform.position.x / 10), (int)(this.transform.position.y / 10)).occupied == Faction.Player2) && this.cp > 0){
                this.cp--;
            }
        }
        if(this.cp == 2)
        {
            Faction = Faction.None;
        }
        else if (this.cp == 0)
        {
            Faction = Faction.Player1;
        }
        else if (this.cp == 4)
        {
            Faction = Faction.Player2;
        }
    }

}
