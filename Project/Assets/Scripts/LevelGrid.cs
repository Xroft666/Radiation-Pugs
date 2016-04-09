using UnityEngine;
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

    public static Dictionary<PlayerEnum, int> cellCounter = new Dictionary<PlayerEnum, int>();

    public event Action<PlayerEnum, int> OnCounterChanged = (PlayerEnum id, int count) => {};

    public GameObject peePrefab;

    public bool debugMode;
    public int gridResolution = 750;
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
               
        cellCounter[PlayerEnum.Player1] = 0;
        cellCounter[PlayerEnum.Player2] = 0;
        cellCounter[PlayerEnum.Player3] = 0;
        cellCounter[PlayerEnum.Player4] = 0;
    }

    public void SetGridOwner(float x, float y, PlayerEnum id)
    {
        Point point = new Point((int) (x/scale), (int) (y/scale));

        PlayerEnum owner = m_grid[point].owner;

        if(owner == id)
            return;

        if(owner != PlayerEnum.None)
        {
            cellCounter[owner] --;

            m_grid[point].renderer = null;
            Destroy(m_grid[point].instantiatedObj);


            OnCounterChanged(owner, cellCounter[owner]);
        }

        takenGridCells.Add(m_grid[point]);

        m_grid[point].owner = id;
        cellCounter[id]++;

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

        OnCounterChanged(id, cellCounter[id]);
    }
}
