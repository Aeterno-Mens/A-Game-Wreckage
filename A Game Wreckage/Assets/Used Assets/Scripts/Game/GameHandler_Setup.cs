using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;
using UnityEngine.SceneManagement;
using TMPro;


public class GameHandler_Setup : MonoBehaviour {

    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] public GameObject Ipoint1;
    [SerializeField] public GameObject Ipoint2;
    [SerializeField] public GameObject Ipoint3;
    private List<GameObject> Ipoints = new List<GameObject>();
    private Vector3 cameraPosition = new Vector3(97,60);
    private float orthoSize = 60f;
    public static GameHandler_Setup Instance;
    public GameState GameState;
    public int map = 0;
    public bool bot = false;
    public GameObject unit;
    private bool space = true;
    public bool action = false;
    public TextMeshProUGUI Turn;
    [SerializeField] public Faction playerTurn;
    void Awake()
    {
        bot = DataHolder.bot;
        map = DataHolder.map;
        Instance = this;
        Ipoints.Add(Ipoint1);
        Ipoints.Add(Ipoint2);
        Ipoints.Add(Ipoint3);
    }
    private void Start() {
        cameraFollow.Setup(() => cameraPosition, () => orthoSize, true, true);
        ChangeState(GameState.GenerateGrid);
    }
    public void Create(GameObject prefab)
    {
        unit = prefab;
        if (GameState == GameState.Player1Turn)
            ChangeState(GameState.SpawnPlayer1);
        else if (GameState == GameState.Player2Turn)
            ChangeState(GameState.SpawnPlayer2);

    }
    private void Update() {
        float cameraSpeed = 100f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            //ограничения камеры, чтобы мы за поле не убегали
            //if(cameraPosition.x > 67)
                cameraPosition += new Vector3(-1, 0) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            cameraPosition += new Vector3(+1, 0) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            cameraPosition += new Vector3(0, +1) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            //if(cameraPosition.y > 60)
                cameraPosition += new Vector3(0, -1) * cameraSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.mouseScrollDelta.y > 0) {
            if(orthoSize>10f)
                orthoSize -= 10f;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.mouseScrollDelta.y < 0)  {
            //if(orthoSize<100f)
            orthoSize += 10f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseMenu.SetActive(!PauseMenu.activeSelf);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        //Debug.Log("yay");
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void EndTurn()
    {
        if (!action)
        {
            space = true;
            if (playerTurn == Faction.Player1)
            {
                //playerTurn = Faction.Player2;
                ChangeState(GameState.Player2Turn);
            }
            else if (playerTurn == Faction.Player2)
            {
                //playerTurn = Faction.Player1;
                ChangeState(GameState.Player1Turn);
            }
        }
    }
    public void NewTurn(bool a, bool b)
    {
        int a1 = 0;
        int a2 = 0;
        GridHandler.Instance.base1.UI.SetActive(false);
        GridHandler.Instance.base2.UI.SetActive(false);
        GridHandler.Instance.base1.GetComponent<SpriteRenderer>().color = GridHandler.Instance.base1.startcolor;
        GridHandler.Instance.base2.GetComponent<SpriteRenderer>().color = GridHandler.Instance.base2.startcolor;
        GridHandler.Instance.base1.check = a;
        GridHandler.Instance.base2.check = b;
        UnitHandler.Instance.SelectedUnit = null;
        foreach (GameObject g in UnitHandler.Instance.spawnedUnits)
        {
            g.GetComponent<BaseUnit>().currentstamina = g.GetComponent<BaseUnit>().stamina;
            g.GetComponent<BaseUnit>().attacked = false;
        }
        foreach (GameObject g in Ipoints)
        {
            g.GetComponent<Influence_point>().AtEndTurn();
            if (g.GetComponent<Influence_point>().Faction == Faction.Player1)
                a1++;
            if (g.GetComponent<Influence_point>().Faction == Faction.Player2)
                a2++;
        }

    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridHandler.Instance.GenerateGrid();
                break;
            case GameState.SpawnPlayer1:
                UnitHandler.Instance.SpawnPlayer1();
                break;
            case GameState.SpawnPlayer2:
                UnitHandler.Instance.SpawnPlayer2();
                break;
            case GameState.Player1Turn:
                if (space)
                {
                    NewTurn(true, false);
                    playerTurn = Faction.Player1;
                    Turn.text = "Ход Игрока1";
                }
                space = false;
                break;
            case GameState.Player2Turn:
                if (space)
                {
                    NewTurn(false, true);
                    playerTurn = Faction.Player2;
                    Turn.text = "Ход Игрока2";
                }
                space = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
}
public enum GameState
{
    GenerateGrid = 0,
    SpawnPlayer1 = 1,
    SpawnPlayer2 = 2,
    Player1Turn = 3,
    Player2Turn = 4
}
