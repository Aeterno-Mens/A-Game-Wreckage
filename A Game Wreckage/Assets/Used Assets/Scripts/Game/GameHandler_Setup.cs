using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;
using UnityEngine.SceneManagement;


public class GameHandler_Setup : MonoBehaviour {

    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private GameObject PauseMenu;
    private Vector3 cameraPosition = new Vector3(97,60);
    private float orthoSize = 60f;
    public static GameHandler_Setup Instance;
    public GameState GameState;
    public int map = 0;
    public bool bot = false;
    public GameObject unit;
    [SerializeField] public Faction playerTurn;
    void Awake()
    {
        bot = DataHolder.bot;
        map = DataHolder.map;
        Instance = this;
    }
    private void Start() {
        cameraFollow.Setup(() => cameraPosition, () => orthoSize, true, true);
        ChangeState(GameState.GenerateGrid);
    }
    public void Create(GameObject prefab)
    {
        unit = prefab;
        if (GameHandler_Setup.Instance.GameState == GameState.Player1Turn)
            GameHandler_Setup.Instance.ChangeState(GameState.SpawnPlayer1);
        else if (GameHandler_Setup.Instance.GameState == GameState.Player2Turn)
            GameHandler_Setup.Instance.ChangeState(GameState.SpawnPlayer2);

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

    public void NewTurn(bool a, bool b)
    {
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
                playerTurn = Faction.Player1;
                NewTurn(true, false);
                break;
            case GameState.Player2Turn:
                playerTurn = Faction.Player2;
                NewTurn(false, true);
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
