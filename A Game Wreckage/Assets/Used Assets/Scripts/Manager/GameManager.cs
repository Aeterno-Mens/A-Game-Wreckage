using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState) {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnPlayer:
                break;
            case GameState.SpawnEnemy:
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnPlayer = 1,
    SpawnEnemy = 2,
    PlayerTurn = 3,
    EnemyTurn = 4
}
