using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class Base : MonoBehaviour
{
    public GameObject UI;
    private Color startcolor;
    public Faction Faction;
    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    void OnMouseEnter()
    {
        startcolor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }
    private void Update()
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
    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = startcolor;
    }
    
}
public enum Faction
{
    Player1 = 0,
    Player2 = 1
}
