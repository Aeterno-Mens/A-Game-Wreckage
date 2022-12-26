using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class Base : MonoBehaviour
{
    // TODO: Health
    public static Base Instance;
    public bool check;
    [SerializeField] public int hp;
    public void OccupieNode()
    {
        GridHandler.Instance.pathfinding.GetNode((int)(this.transform.position.x / 10), (int)(this.transform.position.y / 10)).SetIsOccupied(this.Faction);
        Debug.Log("x = " + (int)(this.transform.position.x / 10) + ",  y = " + (int)(this.transform.position.y / 10));
    }
    private void Awake()
    {
        startcolor = GetComponent<SpriteRenderer>().color;
        Instance = this;
    }

    public GameObject UI;
    public Color startcolor;
    public Faction Faction;
    
    //public 
    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    void OnMouseEnter()
    {
        if (check)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
    private void Update()
    {
        if (check)
        {
            Vector3 bar = transform.position;
            if (Input.GetMouseButtonDown(0) && UI.activeSelf == false && !IsMouseOverUI())
            {
                Vector3 position = UtilsClass.GetMouseWorldPosition();

                //Debug.Log((int)bar.x+" , "+(int)position.x+" , "+(int)bar.y+" , "+(int)position.y);
                if ((int)bar.x - 4 <= (int)position.x && (int)bar.x + 4 >= (int)position.x && (int)bar.y - 4 <= (int)position.y && (int)bar.y + 4 >= (int)position.y)
                    UI.SetActive(true);
            }
            else if (Input.GetMouseButtonDown(0) && UI.activeSelf == true && !IsMouseOverUI())
            {
                Vector3 position = UtilsClass.GetMouseWorldPosition();
                Debug.Log((int)bar.x + " , " + (int)position.x + " , " + (int)bar.y + " , " + (int)position.y);
                if ((int)bar.x - 4 >= (int)position.x || (int)bar.x + 4 <= (int)position.x || (int)bar.y - 4 >= (int)position.y || (int)bar.y + 4 <= (int)position.y)
                    UI.SetActive(false);
            }
        }
    }

    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = startcolor;
    }

    public void AtEndTurn() 
    {
        int x, y;
        var BaseX = this.transform.position.x;
        var BaseY = this.transform.position.y;
        if (check) 
        {
            foreach (var unitPos in UnitHandler.Instance.GetSurroundingCellsP1(BaseX, BaseY)) 
            {
                GridHandler.Instance.pathfinding.GetGrid().GetXY(unitPos, out x, out y);
                var node = GridHandler.Instance.pathfinding.GetNode(x, y);
                if (node.occupied != this.Faction && node.occupied != Faction.None) 
                {
                    hp -= 1;
                    Debug.Log("Decrease hp to: " + hp);
                    break;
                }
            }
        }
    }
}
public enum Faction
{
    None = -1,
    Player1 = 0,
    Player2 = 1
}
