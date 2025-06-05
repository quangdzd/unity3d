using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using System;
using System.Text;

public class PlacementManager :SingletonDestroy<PlacementManager>
{

    private bool isPlacementManagerActive = false;


    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask placement_LayerMask;

    private GameObject demo;
    private Vector3 positionOfMouseOnMap = new Vector3();



    [SerializeField] private Vector2Int cellSize;
    [SerializeField] private Vector2Int posStart;
                     private Vector2Int posEnd;
    [SerializeField] private int enemyRow;
    [SerializeField] private int allyRow;
    [SerializeField] private int Col;
    private GridMap gridMap;
    private DrawGid drawGrid;
    private DrawCell drawCell;
    private GameObject objOfGrid;
    [SerializeField] private Material lineMaterial;




    private Convert convert;
    private EnemyLocationManager enemyLocationManager;
    private JSONReader jSONReader;
    private String enemyDataPath = "Enemy.txt";
    private String enemyLocationPath = "GameLevels/1/map1.txt";

    private WaveDatas waveDatas;
    private int[,] locations;
    public EnemyData enemyData;

    


    protected override void Awake()
    {
        base.Awake();
        posEnd = posStart + new Vector2Int(Col * cellSize.x , (enemyRow + allyRow)* cellSize.y);
        gridMap = new GridMap(posStart , posEnd , cellSize);
        drawGrid = new DrawGid(enemyRow + allyRow , Col , GetLocationFromPosition(new Vector3(posStart.x,0,posStart.y)) , cellSize);
        drawCell = new DrawCell(cellSize,0.2f);
        
        drawCell.Deactivate();

        enemyLocationManager = new EnemyLocationManager();
        jSONReader = new JSONReader();
        convert = new Convert();

    }

    public void Start()
    {
        drawGrid.Draw(lineMaterial);
        drawCell.Draw(lineMaterial);


        enemyLocationPath = GameStateTransfer.Instance.EnemyLocationPath;
        waveDatas = jSONReader.WavedatasReader(enemyLocationPath);
        int seedmoney = waveDatas.seed_money;
        GameManager.Instance.SetSeedMoney(seedmoney);
        SetMapdata("1");
        // Debug.Log("Seed Money : " + seedmoney);

    }

    private void Update() {
        positionOfMouseOnMap = MousePosOnMap();


        if(isPlacementManagerActive)
        {
            
            demo.transform.position = GetLocationFromPosition(positionOfMouseOnMap);
            drawCell.SetPos(demo.transform.position);

        }


    }
    public void LoadMap()
    {
        List<StatsForEnemy> statsForEnemys = enemyData.StatsForEnemies;
        for(int i = 0; i < locations.GetLength(0) ; i++)
        {
            for(int j = 0; j < locations.GetLength(1) ; j++)
            {
                int id = locations[i, j];
                if(id != 0)
                {
                    Vector2Int index = new Vector2Int(i+10 , j);
                    AddEnemy(statsForEnemys[id].prefab , GetLocationFromIndex(index) );
                }
            }  
        }
    }

    public void LoadAllies()
    {
        GameObject[,] entities = gridMap.GetEntities();
        int[,] entitiesId = gridMap.GetEntitiesId();
        for (int x = 0; x < entities.GetLength(0); x++)
            {
                for (int y = 0; y < entities.GetLength(1); y++)
                {
                    GameObject entity = entities[x, y];
                if (entity != null)
                {
                        Vector3 worldPos = GetLocationFromIndex(new Vector2Int (x, y));
                        entity.transform.position = worldPos;
                    }
                }
            }
    }
    public void SetMapdata(string map)
    {
        WaveEntry wave = waveDatas.waves.Find(w => w.key == map);
        locations = convert.Convert1Dto2D(wave.values);
        LoadMap();
    }

    public void RegisterLocation(Vector2Int location, GameObject gameObject)
    {
        gridMap.RegisterLocation(location, 1, gameObject);
        // Print2DArray(gridMap.entitiesId);
    }
    public void UnregisterLocation(Vector2Int location)
    {
        gridMap.UnregisterLocation(location);
    }



    
    public void ChangeObjectDemo(GameObject gameObject)
    {
        Destroy(demo);
        demo =  Instantiate(gameObject);
        BoxCollider boxCollider = demo.GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }
    public bool IsActive()
    {
        return isPlacementManagerActive;
    }
    public void DestroyDemoObject()
    {
        Destroy(demo);
    }
    public void PS_Activate()
    {
        isPlacementManagerActive = true;
        drawCell.Activate();
        PS_RegisterListener();
        NONPS_UnregisterListener();
    }
    public void PS_Deactivate()
    {
        isPlacementManagerActive = false;
        drawCell.Deactivate();
        DestroyDemoObject();
        PS_UnregisterListener();
        NONPS_RegisterListener();
    }




    public Vector3 MousePosOnMap()
    {
        Vector3 lastPos = new Vector3();
        Vector3 mouse = Input.mousePosition;
        mouse.z = cam.nearClipPlane;
        Ray ray = cam.ScreenPointToRay(mouse);
        RaycastHit hit ;
        if(Physics.Raycast(ray , out hit , Mathf.Infinity ,placement_LayerMask))
        {
            lastPos = hit.point;

        }

    
        return lastPos;
    }
    
    // Chuyển vị trí của chuột sang vị trí được chuẩn hoá trên lưới
    public Vector3 GetLocationFromPosition( Vector3 position)
    {
        float x= Mathf.FloorToInt(position.x);
        x = x - x%cellSize.x + cellSize.x/2.0f;
        x = Mathf.Min(Mathf.Max(posStart.x - posStart.x%cellSize.x+ cellSize.x/2.0f, x), posEnd.x - posEnd.x%cellSize.x - cellSize.x/2.0f);




        float z= Mathf.FloorToInt(position.z);
        z = z - z%cellSize.y + cellSize.y/2.0f;

        z = Mathf.Min(Mathf.Max(posStart.y - posStart.y%cellSize.y + cellSize.y/2.0f, z), posEnd.y - posEnd.y%cellSize.y -cellSize.y/2.0f - cellSize.y*10);


        Ray ray = new Ray(new Vector3(x, 1000f, z), Vector3.down);  // Tia sẽ đi xuống từ (x, 1000, z)
        RaycastHit hit;
        float y = position.y;
        if (Physics.Raycast(ray, out hit , Mathf.Infinity , placement_LayerMask))
        {

            y = hit.point.y;

        }
        return new Vector3(x,y,z);

    }

    // Chuyển vị trí sang hàng và cột mảng 2 chiều 
    public Vector2Int GetIndexFromLocation(Vector3 pos)
    {
        Vector3 posS = new Vector3(posStart.x , 0 , posStart.y);


        int rol = (int)(GetLocationFromPosition(pos).z - GetLocationFromPosition(posS).z )/cellSize.y;
        int col = (int)(GetLocationFromPosition(pos).x - GetLocationFromPosition(posS).x )/cellSize.x;
        return new Vector2Int(rol,col);
    }
    
    // Lấy vị trí được chuẩn hoá từ  hàng/cột 
    public Vector3 GetLocationFromIndex(Vector2Int index)
    {
        Vector3 posS = GetLocationFromPosition(new Vector3 (posStart.x , 0 , posStart.y));
        //index = 1 , 2 -> x + 10 , z + 5
        float x, y,z;

        x= posS.x + index.y*cellSize.x;
        z= posS.z + index.x*cellSize.y;
        Ray ray = new Ray(new Vector3(x, 1000f, z), Vector3.down);  // Tia sẽ đi xuống từ (x, 1000, z)
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit , Mathf.Infinity , placement_LayerMask))
        {
            y = hit.point.y;
        }
        else
        {
            y = 0;
        }

        return new Vector3(x,y,z);
    }



    
    public bool IsPointerOverUIObject()
    {
        // Kiểm tra nếu con trỏ chuột đang đè lên UI
        return EventSystem.current.IsPointerOverGameObject();
    }

    public GameObject CreatGameObject(GameObject gameObject)
    {
        GameObject gameObject1= Instantiate(gameObject);
        BoxCollider boxCollider = gameObject1.GetComponent<BoxCollider>();
        boxCollider.isTrigger = false;
        return gameObject1;

    }
    public void AddEnemy( GameObject gameObject , Vector3 pos)
    {
        GameObject gameObject1= Instantiate(gameObject);
        EntityManager.Instance.AddToRedTeam(gameObject1, pos + new Vector3(0,0.5f,0));

    }


    public void AddAlly()
    {
        if(isPlacementManagerActive &&!IsPointerOverUIObject())
        {

            Vector3 mapongrid = GetLocationFromPosition(positionOfMouseOnMap);
            Vector2Int location = GetIndexFromLocation(mapongrid);
            int cost = demo.GetComponent<MoneyManager>().GetCost();
            int gold = GameManager.Instance.GetMoney();

                if (gridMap.GetLocation(location) != 0 ||  gold < cost)
            {
                return;
            }


            GameObject gameObject = CreatGameObject(demo);


            EntityManager.Instance.AddToBlueTeam(gameObject, mapongrid);

            EnemyLocation enemyLocation = gameObject.AddComponent<EnemyLocation>();
            enemyLocation.Location = location;
            enemyLocation.RegisterLocationOnMap(gameObject);

            PS_Deactivate();
        }
    }

    public void AddAllies()
    {
        if(isPlacementManagerActive &&!IsPointerOverUIObject())
        {


            Vector3 mapongrid = GetLocationFromPosition(positionOfMouseOnMap);
            Vector2Int location = GetIndexFromLocation(mapongrid);
            int cost = demo.GetComponent<MoneyManager>().GetCost();
            int gold = GameManager.Instance.GetMoney();
                if(gridMap.GetLocation(location) != 0 || gold < cost)
                {
                    return;
                }
                
            GameObject gameObject = CreatGameObject(demo);
            EntityManager.Instance.AddToBlueTeam(gameObject, mapongrid);


            EnemyLocation enemyLocation = gameObject.AddComponent<EnemyLocation>();
            enemyLocation.Location = location;
            enemyLocation.RegisterLocationOnMap(gameObject);

        }
    }
    public void AddConsecutiveAllies()
    {
        if(isPlacementManagerActive &&!IsPointerOverUIObject())
        {
            Vector3 mapongrid = GetLocationFromPosition(positionOfMouseOnMap);

            Vector2Int location = GetIndexFromLocation(mapongrid);
            int cost = demo.GetComponent<MoneyManager>().GetCost();
            int gold = GameManager.Instance.GetMoney();
            
                if (gridMap.GetLocation(location) != 0 ||  gold < cost)
            {
                return;
            }


            GameObject gameObject = CreatGameObject(demo);

            EntityManager.Instance.AddToBlueTeam(gameObject, mapongrid);


            EnemyLocation enemyLocation = gameObject.AddComponent<EnemyLocation>();
            enemyLocation.Location = location;
            enemyLocation.RegisterLocationOnMap(gameObject);

        }
    }


    

    public void Choose()
    {

        Vector2Int index = GetIndexFromLocation(GetLocationFromPosition(positionOfMouseOnMap));
        // Debug.Log(index);
        if (gridMap.GetLocation(index) != 0)
        {
            objOfGrid = gridMap.GetObjLocation(index);
            drawCell.Activate();
            gridMap.UnregisterLocation(index);
        }
    }
    public void Move()
    {
        if(objOfGrid != null)
        {
        // Debug.Log("a huuuuuu");
            Vector3 mapongrid = GetLocationFromPosition(positionOfMouseOnMap);

            Vector2Int location = GetIndexFromLocation(mapongrid);

            // Debug.Log("dang di chuyen");
            drawCell.SetPos(mapongrid);
            objOfGrid.transform.position = mapongrid;
                
        }
    }
    public void Set()
    {
        if(objOfGrid != null)
        {
            Vector3 mapongrid = GetLocationFromPosition(positionOfMouseOnMap);
            drawCell.Deactivate();

            Vector2Int location = GetIndexFromLocation(mapongrid);

            EnemyLocation enemyLocation = objOfGrid.GetComponent<EnemyLocation>();
            if (enemyLocation == null)
            {
                enemyLocation = objOfGrid.AddComponent<EnemyLocation>();
            }
            enemyLocation.Location = location;
            enemyLocation.RegisterLocationOnMap(objOfGrid);
            objOfGrid.transform.position = mapongrid;

            objOfGrid = null;

        }

    }




    public void PS_RegisterListener()
    {
        InputManager.Instance.RegisterAction(OnMouse0ClickPS, AddAlly);
        InputManager.Instance.RegisterAction(OnShiftMouse0Click , AddAllies);
        InputManager.Instance.RegisterAction(OnShiftMouse0 , AddConsecutiveAllies);
    }

    public void PS_UnregisterListener()
    {
        InputManager.Instance.UnregisterAction(OnMouse0ClickPS); 
        InputManager.Instance.UnregisterAction(OnShiftMouse0Click); 
        InputManager.Instance.UnregisterAction(OnShiftMouse0); 

    }

    public void NONPS_RegisterListener()
    {        
        InputManager.Instance.RegisterAction(OnMouse0ClickNonPS, Choose);
        InputManager.Instance.RegisterAction(OnMouse0 , Move);
        InputManager.Instance.RegisterAction(OnMouse0Up , Set);

    }
    public void NONPS_UnregisterListener()
    {
        InputManager.Instance.UnregisterAction(OnMouse0ClickNonPS);
        InputManager.Instance.UnregisterAction(OnMouse0);
        InputManager.Instance.UnregisterAction(OnMouse0Up);

    }
    public bool OnMouse0ClickPS()
    {
        return Input.GetMouseButtonDown(0) && !OnKeyShift();
    }
        public bool OnMouse0ClickNonPS()
    {
        return Input.GetMouseButtonDown(0) && !OnKeyShift();
    }
    public bool OnMouse0()
    {
        return Input.GetMouseButton(0) && !OnKeyShift();
    }
    public bool OnMouse0Up()
    {
        return Input.GetMouseButtonUp(0) && !OnKeyShift();
    }
    public bool OnShiftMouse0Click()
    {
        return Input.GetMouseButtonDown(0) && OnKeyShift();
    }
    public bool OnMouse1Click()
    {
        return Input.GetMouseButtonDown(1) && !OnKeyShift();
    }
    public bool OnShiftMouse1Click()
    {
    return Input.GetMouseButtonDown(1) && OnKeyShift();
    }

    public bool OnShiftMouse0()
    {
        return Input.GetMouseButton(0) && OnKeyShift();
    }


    public bool OnKeyShift()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }



}
