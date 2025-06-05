using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityPickerUIManager : MonoBehaviour
{

    public CanvasItem canvasItem;

    public Button close;
    public Button display;
    public GameObject allChild;


    private EntityInfoDisplay entityInfoDisplay;
    private Dictionary<string, EnemyData> enemyDataDictionary;

    private Dictionary<string  , GameObject> frames ;

    private string activeFrameId  = "";



    void Awake()
    {
        Button button = canvasItem.button;
        GameObject frame = canvasItem.frame;
        GameObject frameParent = canvasItem.frameParent;

        entityInfoDisplay = gameObject.GetComponent<EntityInfoDisplay>();

        frames = new Dictionary<string , GameObject>();

        enemyDataDictionary = new Dictionary<string, EnemyData>();

        
    }
    public void Start()
    {
        foreach (var data in AllEnemyData.Instance.GetAllEnemies())
        {
            if (!enemyDataDictionary.ContainsKey(data.id))
            {
                enemyDataDictionary.Add(data.id, data);
                CreatFrame(data.id);

            }
            else if(data == null)
            {
                Debug.Log("khoong co data quai vat");
            }
        }

    }

    public void Close()
    {
        allChild.gameObject.SetActive(false);
        close.gameObject.SetActive(false);
        display.gameObject.SetActive(true);
    }
    public void Display()
    {
        allChild.gameObject.SetActive(true);
        close.gameObject.SetActive(true);
        display.gameObject.SetActive(false);

    }
    public void OnDisplayFrame(String id)
    {
        if(frames.ContainsKey(id))
        {
            try{
                OnDeactiveFramebyID(activeFrameId);
            }
            catch(Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            OnActiveFramebyID(id);
            activeFrameId = id;
        }
        else
        {
            Debug.LogError("Dell có frame");
        }
    }




    private EnemyData GetEnemyDataByID(string id)
    {
        if (enemyDataDictionary.TryGetValue(id, out EnemyData data))
        {
            return data;
        }

        Debug.LogWarning($"EnemyData with ID '{id}' not found!");
        return null;
    }
    private void OnActiveFramebyID(string id)
    {
        if(frames.TryGetValue(id, out GameObject frame))
        {
            frame.SetActive(true);
        }
    }

    private void OnDeactiveFramebyID(string id)
    {
        if(frames.TryGetValue(id, out GameObject frame))
        {
            frame.SetActive(false);
        }
    }
    private void CreatFrame(string id) 
    {
        GameObject frame = Instantiate(canvasItem.frame);
        EnemyData enemyData = GetEnemyDataByID(id);

        foreach (var item in enemyData.StatsForEnemies)
        {
            Button button = CreateButton(canvasItem.button);
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            text.text = item._name;


            button.onClick.AddListener(() => ButtonOnClick(item.prefab));
            button.gameObject.AddComponent<PointerHandlerButton>();
            HoverButtonToDisplatInfo(button , item);

            AddParentforGameobject( button.gameObject, frame);
        }
        frame.transform.SetParent(canvasItem.frameParent.transform , false) ;

        frames.Add(id, frame);

        OnDeactiveFramebyID(id);

    }

    public void HoverButtonToDisplatInfo(Button button , StatsForEnemy stats)
    {
        PointerHandlerButton pointerHandlerButton = button.gameObject.GetComponent<PointerHandlerButton>();
        pointerHandlerButton.OnPointerEnterEvent += () => OnPoiterEnterButton(stats) ;
        pointerHandlerButton.OnPointerExitEvent += () => OnPoiterExitButton(stats);
    }
     public void OnPoiterExitButton(StatsForEnemy  stats)
     {
        
     }
     public void OnPoiterEnterButton(StatsForEnemy  stats)
     {
        entityInfoDisplay.ChangeInfo(stats);

     }

    private Button CreateButton( Button button) 
    {
        Button gameObject = Instantiate(button);

        return gameObject;
    }
    private void AddParentforGameobject(GameObject child , GameObject parent)
    {
        child.transform.SetParent(parent.transform, false);
    }
    private void ButtonOnClick(GameObject gameObject)
    {
        Debug.Log("Tao đã được nhấn và tao là " + gameObject.name);
        PlacementManager.Instance.PS_Activate();
        PlacementManager.Instance.ChangeObjectDemo(gameObject);
    }
    

    
}