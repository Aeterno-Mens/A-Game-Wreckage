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


public class GameHandler : MonoBehaviour {

    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] public GameObject PauseMenu;
    [SerializeField] public GameObject Ipoint1;
    [SerializeField] public GameObject Ipoint2;
    [SerializeField] public GameObject Ipoint3;
    private Vector3[] CameraOriginalPosition = new Vector3[2];
    private List<GameObject> Ipoints = new List<GameObject>();
    [SerializeField] private GameObject VictoryScreen;
    public TextMeshProUGUI Winner;
    private Vector3 cameraPosition;
    private float orthoSize = 60f;
    public static GameHandler Instance;
    public GameState GameState;
    public int map = 0;
    public bool bot = false;
    public GameObject unit;
    public bool space = true;
    public bool action = false;
    public TextMeshProUGUI Turn;
    public TextMeshProUGUI ResourceAmount;
    //Ресурсы игрока1/2 и количество точек влияния под контролем
    public int ResourceP1 = 0;
    public int ResourceP2 = 0;
    public int a1 = 0;
    public int a2 = 0;
    public int UpgradeP1 = 1;
    public int UpgradeP2 = 1;
    public AudioSource Bgm1, Bgm2;
    [SerializeField] public Faction playerTurn;
    public Image BotScreen;
    void Awake()
    {
        bot = DataHolder.bot;
        map = DataHolder.map;
        if (map == 1)
        {
            CameraOriginalPosition[0] = new Vector3(20, 20);
            CameraOriginalPosition[1] = new Vector3(230, 240);
        }
        else if (map == 2 || map == 3)
        {
            CameraOriginalPosition[0] = new Vector3(125, 20);
            CameraOriginalPosition[1] = new Vector3(125, 240);
        }
        else
        {
            map = 1;
            CameraOriginalPosition[0] = new Vector3(20, 20);
            CameraOriginalPosition[1] = new Vector3(230, 240);
        }
        cameraPosition = CameraOriginalPosition[0];
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
        if (GameState == GameState.Player1Turn && prefab.GetComponent<BaseUnit>().tier <= UpgradeP1)
            ChangeState(GameState.SpawnPlayer1);
        else if (GameState == GameState.Player2Turn && prefab.GetComponent<BaseUnit>().tier <= UpgradeP2)
            ChangeState(GameState.SpawnPlayer2);
    }
    private void Update() {
        float cameraSpeed = 100f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            //ограничения камеры, чтобы мы за поле не убегали
            if(cameraPosition.x > 20)
                cameraPosition += new Vector3(-1, 0) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            if(cameraPosition.x<240)
                cameraPosition += new Vector3(+1, 0) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            if (cameraPosition.y < 240)
                cameraPosition += new Vector3(0, +1) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            if(cameraPosition.y > 20)
                cameraPosition += new Vector3(0, -1) * cameraSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.mouseScrollDelta.y > 0) {
            if(orthoSize>30f)
                orthoSize -= 10f;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.mouseScrollDelta.y < 0)  {
            if(orthoSize<100f)
            orthoSize += 10f;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && VictoryScreen.activeSelf != true)
            PauseMenu.SetActive(!PauseMenu.activeSelf);

        if (Input.GetKeyDown(KeyCode.Space) && VictoryScreen.activeSelf != true)
        {
            EndTurn();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GameState == GameState.Player1Turn)
                cameraPosition = CameraOriginalPosition[0];
            else if (GameState == GameState.Player2Turn)
                cameraPosition = CameraOriginalPosition[1];
        }
    }

    public void Pause()
    {
        if (VictoryScreen.activeSelf != true)
        {
            PauseMenu.SetActive(true);
            //Debug.Log("yay");
            Time.timeScale = 0f;
        }
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
        if (!bot || GameState != GameState.Player2Turn)
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
    }

    public void Win() 
    {
        if (GridHandler.Instance.base1.hp < 0) 
        {
            PauseMenu.SetActive(false);
            playerTurn = Faction.None;
            Winner.text = "Игрок2 Победил";
            VictoryScreen.SetActive(true);
            GridHandler.Instance.base1.transform.Find("Canvas").transform.Find("BigBoom").GetComponent<Image>().gameObject.SetActive(true);
            Bgm1.Pause();
            Bgm2.Play();
            //Home();
        }
        if (GridHandler.Instance.base2.hp < 0) 
        {
            if(bot)
                BotScreen.transform.gameObject.SetActive(false);
            bot = false;
            PauseMenu.SetActive(false);
            playerTurn = Faction.None;
            Winner.text = "Игрок1 Победил";
            VictoryScreen.SetActive(true);
            GridHandler.Instance.base2.transform.Find("Canvas").transform.Find("BigBoom").GetComponent<Image>().gameObject.SetActive(true);
            Bgm1.Pause();
            Bgm2.Play();    
            //Home();
        }
    }
    public void NewTurn(bool a, bool b)
    {
        a1 = 0;
        a2 = 0;
        GridHandler.Instance.base1.UI.SetActive(false);
        GridHandler.Instance.base2.UI.SetActive(false);
        GridHandler.Instance.base1.GetComponent<SpriteRenderer>().color = GridHandler.Instance.base1.startcolor;
        GridHandler.Instance.base2.GetComponent<SpriteRenderer>().color = GridHandler.Instance.base2.startcolor;
        GridHandler.Instance.base1.check = a;
        GridHandler.Instance.base2.check = b;
        // TODO: handle cells around bases for different faction units (damage)
        if(UnitHandler.Instance.SelectedUnit != null)
            UnitHandler.Instance.SelectedUnit.GetComponent<BaseUnit>().gameObject.transform.Find("Selected").gameObject.SetActive(false);

        GridHandler.Instance.base1.AtEndTurn();
        GridHandler.Instance.base2.AtEndTurn();
        Win();
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
                    ResourceP1 += 500 + a1 * 300;
                    ResourceAmount.text = ResourceP1.ToString();
                    playerTurn = Faction.Player1;
                    Turn.text = "Ход Игрока1";
                    if (bot)
                    {
                        BotScreen.transform.gameObject.SetActive(false);
                    }
                }
                space = false;
                break;
            case GameState.Player2Turn:
                if (space)
                {
                    NewTurn(false, true);
                    ResourceP2 += 500 + a2 * 300;
                    ResourceAmount.text = ResourceP2.ToString();
                    playerTurn = Faction.Player2;
                    Turn.text = "Ход Игрока2";
                    space = false;
                    if (bot)
                    {
                        BotScreen.transform.gameObject.SetActive(true);
                        Bot.Instance.BotBeheviour();
                    }
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

    public void Upg()
    {
        if (GameState == GameState.Player1Turn)
        {
            if (ResourceP1 > 250 * UpgradeP1 && UpgradeP1 < 4)
            {
                ResourceP1 -= 250 * UpgradeP1;
                UpgradeP1++;
                ResourceAmount.text = ResourceP1.ToString();
            }
        }

        if (GameState == GameState.Player2Turn)
        {
            if (ResourceP2 > 250 * UpgradeP2 && UpgradeP2 < 4)
            {
                ResourceP2 -= 250 * UpgradeP2;
                UpgradeP2++;
                ResourceAmount.text = ResourceP2.ToString();
            }
        }
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
