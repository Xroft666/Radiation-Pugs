﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum PlayerEnum
{
    None = -1,
    Player1,
    Player2,
    Player3,
    Player4
}

public enum LevelObjectEnum
{
    None,
    Obstacle,
    WaterBowl
}

public class GridCell
{
    public PlayerEnum owner = PlayerEnum.None;
    public LevelObjectEnum levelObject;

    public GameObject instantiatedObj;
    public Renderer renderer;

    public GridHelper helper;
}

public class LevelGrid : MonoBehaviour 
{
    public static LevelGrid Instance;

    public static Dictionary<PlayerEnum, float> pointsCounter = new Dictionary<PlayerEnum, float>();

    public event Action<PlayerEnum, float> OnCounterChanged = (PlayerEnum id, float count) => {};

    public GameObject peePrefab;

    public bool debugMode;
    public static int gridResolution = 50;
    public float scale = 0.1f;

    private Dictionary<Point, GridCell> m_grid = new Dictionary<Point, GridCell>();

    public List<GridCell> takenGridCells = new List<GridCell>();
	
    public void Awake()
    {
        Instance = this;

        for(int i = 0; i < (int) (1.7f * gridResolution); i++)
            for(int j = 0; j < gridResolution; j++)
            {
                Point point = new Point((int) ((i - 1.7f * gridResolution / 2 ) ), 
                                        (int) ((j - gridResolution / 2) ));
                m_grid[point] = new GridCell();
               
                if(debugMode)
                {
                    GameObject helperGO = new GameObject(point.ToString());
                    helperGO.transform.position = new Vector3(point.x * scale, point.y * scale);
                    m_grid[point].helper = helperGO.AddComponent<GridHelper>();     
                }
            }
               
        pointsCounter[PlayerEnum.Player1] = 0;
        pointsCounter[PlayerEnum.Player2] = 0;
        pointsCounter[PlayerEnum.Player3] = 0;
        pointsCounter[PlayerEnum.Player4] = 0;
    }

    public GridCell GetCell(float x, float y)
    {
        Point point = new Point((int) (x/scale), (int) (y/scale));
        return m_grid[point];
    }

    public void SetGridOwner(float x, float y, PlayerEnum id)
    {
        Point point = new Point((int) (x/scale), (int) (y/scale));

        PlayerEnum owner = m_grid[point].owner;

        if(owner == id)
            return;

        if(owner != PlayerEnum.None)
        {
            pointsCounter[owner] -= 2f;

            m_grid[point].renderer = null;
            Destroy(m_grid[point].instantiatedObj);


            OnCounterChanged(owner, pointsCounter[owner]);
        }

        //takenGridCells.Add(m_grid[point]);

        m_grid[point].owner = id;
        pointsCounter[id] += 2f;

        Color changeColor = Color.white;
        switch(id)
        {
            case PlayerEnum.Player1:
                changeColor = Color.red;
                break;
            case PlayerEnum.Player2:
                changeColor = Color.blue;
                break;
            case PlayerEnum.Player3:
                changeColor = Color.yellow;
                break;
            case PlayerEnum.Player4:
                changeColor = Color.black;
                break;
        }

        m_grid[point].instantiatedObj = Instantiate(peePrefab, new Vector3(x,y), Quaternion.identity) as GameObject;
        m_grid[point].renderer = m_grid[point].instantiatedObj.GetComponent<Renderer>();

        if(debugMode)
        {
            m_grid[point].helper.color = changeColor;
        }

        OnCounterChanged(id, pointsCounter[id]);
    }

    public void UpdateScore(PlayerEnum id, float score)
    {
        OnCounterChanged(id, score);
    }
}
