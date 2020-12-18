﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Variables
    private static GameController insnatce;
    [HideInInspector]
    public static GameController Insnatce { get => insnatce; set => insnatce = value; }

    [SerializeField]
    public List<BuildingInfo> buildingsInfo;

    [SerializeField]
    public List<UnitInfo> unitsInfo;

    [SerializeField]
    public int startCoins, startPeoples;

    private int
        mapSize,
        botsCount,
        playersCount = 1,
        botsMaxCount = 5,
        botsMinCount = 3;

    [SerializeField]
    private float cellSize = 1f;

    [SerializeField]
    private GameObject
        goFieldCell,
        goMapHolder,
        goCastlePrefab,
        goPlayerCastlePrefab,
        goArmyPrefab,
        goMoveCellPrefab,
        goFieldHolder;
    
    [HideInInspector]
    public bool isArmySelected = false;

    private List<(int x, int y)> moveList;

    private List<Vector3> listSpawnpoints;

    [SerializeField]
    public GameObject mainCamera; 

    [HideInInspector]
    public Grid 
        grid,
        gridMoveZone;

    private GameObject selectedArmy = null;

    private GraphicRaycaster m_Raycaster;
    private EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;
    #endregion
    private void Awake()
    {
        if (GameController.insnatce == null)
        {
            insnatce = this;
            //DontDestroyOnLoad(this);
        }

        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = FindObjectOfType<EventSystem>();
        moveList = new List<(int x, int y)>();
    }

    void Start()
    {
        MapGenerator();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            
            if (results.Count == 0)
            {
                ClickDetector();
            }
        }
        CameraMove();
    }

    private void ClickDetector()
    {
        GameObject 
            point = grid.GetValue(ExtensionClass.GetMouseWorldPosition()),
            pointMove = gridMoveZone.GetValue(ExtensionClass.GetMouseWorldPosition());
        
        if (pointMove != null && pointMove.GetComponent<MoveCell>() != null && isArmySelected)
        {
            Debug.Log("MoveAction");
            if(selectedArmy != null && (point == null || point.GetComponent<Castle>() != null))
            {
                if(point != null && point.GetComponent<ArmyController>() != null)
                {
                    Debug.Log("Attack");
                }
                else
                {
                    grid.MoveTo(pointMove.transform.position, selectedArmy);

                }
            }
        }

        if (point != null && point.GetComponent<ArmyController>() != null && point.GetComponent<ArmyController>().owner == FindObjectOfType<PlayerController>().gameObject && !isArmySelected)
        {
            selectedArmy = point;
            moveList = ArmyMoveZone(point);
            ShowArmyMoveZone();
            Debug.Log("You just click at your army");
            isArmySelected = true;
        }
        else if (isArmySelected)
        {
            HideArmyMoveZone();
        }
        //Debug.Log(grid.GetValue(ExtensionClass.GetMouseWorldPosition()));
    }

    /// <summary>
    /// Метод высчитывания зоны движения армии
    /// </summary>
    /// <param name="army"></param>
    /// <returns></returns>
    public List<(int x, int y)> ArmyMoveZone(GameObject army)
    {
        List<(int x, int y)> moveList = new List<(int x, int y)>();

        int x = (int)army.transform.position.x,
            y = (int)army.transform.position.y,
            speed = army.GetComponent<ArmyController>().amrySpeed;

        Debug.Log("Selected Army speed: " + speed);
        
        for(int jx = x - speed; jx <= x + speed; jx++)
        {
            if (jx < 0)
                continue;
            else if (jx == x)
                continue;
            else if (jx >= mapSize)
                continue;
            moveList.Add((jx, y));
        }
        for (int jy = y - speed; jy <= y + speed; jy++)
        {
            if (jy < 0)
                continue;
            else if (jy == y)
                continue;
            else if (jy >= mapSize)
                continue;
            moveList.Add((x, jy));
        }
        return moveList;
    }

    private void ShowArmyMoveZone()
    {
        for (int i = 0; i < moveList.Count; i++)
        {
            gridMoveZone.SetValue(moveList[i].x, moveList[i].y,
                Instantiate(goMoveCellPrefab, new Vector3(moveList[i].x, moveList[i].y, goMapHolder.transform.position.z), new Quaternion(0, 0, 0, 0), goMapHolder.transform));
        }
    }

    private void HideArmyMoveZone()
    {
        for (int i = 0; i < moveList.Count; i++)
        {
            gridMoveZone.RemoveValue(moveList[i].x, moveList[i].y);
        }
        selectedArmy = null;
        isArmySelected = false;
    }

    private void CameraMove()
    {
        if (Input.GetAxis("Vertical") > 0 && mainCamera.transform.position.y <= mapSize)
            mainCamera.transform.Translate(Vector3.up * Time.deltaTime * 10f);
        if (Input.GetAxis("Vertical") < 0 && mainCamera.transform.position.y >= 0)
            mainCamera.transform.Translate(Vector3.down * Time.deltaTime * 10f);

        if (Input.GetAxis("Horizontal") > 0 && mainCamera.transform.position.x <= mapSize)
            mainCamera.transform.Translate(Vector3.right * Time.deltaTime * 10f);
        if (Input.GetAxis("Horizontal") < 0 && mainCamera.transform.position.x >= 0)
            mainCamera.transform.Translate(Vector3.left * Time.deltaTime * 10f);
    }

    public void MapGenerator()
    {
        listSpawnpoints = new List<Vector3>();

        if(mapSize == 0)
            mapSize = Random.Range(15, 51);

        int width = mapSize, height = mapSize;

        grid = new Grid(mapSize, mapSize);
        gridMoveZone = new Grid(mapSize, mapSize);


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Instantiate(goFieldCell,new Vector3(x,y,0), new Quaternion(0,0,0,0), goFieldHolder.transform);

                if ((x == width - 1 || x == 0) && (y == height - 1 || y == 0))
                {
                    listSpawnpoints.Add(new Vector3(x, y, 0));
                }

                if (x == (width - 1) / 2 && (y == 0 || y == height - 1))
                {
                    listSpawnpoints.Add(new Vector3(x, y, 0));
                }

                if (y == (height - 1) / 2 && (x == 0 || x == width - 1))
                {
                    listSpawnpoints.Add(new Vector3(x, y, 0));
                }
            }
        }
        Debug.Log("Grid size is: " + mapSize);

        ExtensionClass.Shuffle(listSpawnpoints);
        SpawnSelector();
    }

    private void SpawnSelector()
    {
        if (botsCount == 0 && botsMaxCount > botsMinCount)
            botsCount = Random.Range(3, botsMaxCount + 1);

        Debug.Log("Total players count: " + (botsCount + playersCount));
        
        mainCamera.transform.position = new Vector3(listSpawnpoints[0].x, listSpawnpoints[0].y, mainCamera.transform.position.z);

        grid.GetXY(listSpawnpoints[0], out int x, out int y);
        grid.SetValue(x, y, 
            Instantiate(goPlayerCastlePrefab, listSpawnpoints[0], new Quaternion(0, 0, 0, 0), goMapHolder.transform));

        listSpawnpoints.RemoveAt(0);

        for (int i = 0; i< botsCount; i++)
        {
            grid.GetXY(listSpawnpoints[0], out int x2, out int y2);
            grid.SetValue(x2, y2, Instantiate(goCastlePrefab, listSpawnpoints[0], new Quaternion(0, 0, 0, 0), goMapHolder.transform));

            listSpawnpoints.RemoveAt(0);
        }

        GameObject gameObject = Instantiate(goArmyPrefab, new Vector3(6, 6, 0), new Quaternion(0, 0, 0, 0), goMapHolder.transform);
        gameObject.GetComponent<ArmyController>().owner = FindObjectOfType<PlayerController>().gameObject;
        //gameObject.GetComponent<ArmyController>().amrySpeed = 2;
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        grid.SetValue(6, 6, gameObject);
    }
}

public static class ExtensionClass
{
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vector = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vector.z = 0f;
        return vector;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
