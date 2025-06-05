using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour 
{
    public GameObject scrollBar;
    
    public GameObject content;

    public GameObject mainMenu;

    private Action loop;



    


    private float distance;
    private float[] values;
    private float currentValue;
    private Scrollbar scrollBarComponent;

    private float newpos;
    private float oldpos;
    private int index;
    private int childCount;

    


    private bool isChecking = false;
    private bool isScrolling = false;

    private string mapData;



    void Awake()
    {
        scrollBarComponent = scrollBar.GetComponent<Scrollbar>();
        
    }
    void Start()
    {
        childCount = content.transform.childCount;
        InitPositions();

        for (int i = 0; i < childCount; i++)
        {
            content.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(Disable);
            content.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(mainMenu.GetComponent<MainMenu>().Enable);

        }
        
    }

    void Update()
    {
        if (!isScrolling)
        {
            GetScrollbarValue();

        }

        UpdateScale();

        if(Input.GetMouseButtonDown(0))
        {
            isChecking = false;
        }
        if(!isChecking)
        {
            StartCoroutine(CheckIfStopped());
        }



        loop?.Invoke();
        
    }


    public void Enable()
    {
        gameObject.SetActive(true);
    }
    public void Disable()   
    {
        gameObject.SetActive(false);
        SetmapData(index +1);


        MainMenu gameController = mainMenu.GetComponent<MainMenu>();
        gameController.SetenemyLocationPath(mapData);
    }


    public void SetmapData(int index)
    {
        this.mapData = "map" + (index).ToString() + ".txt";
    }
    public void GetScrollbarValue()
    {
        newpos = scrollBarComponent.value;
        index = (int)(newpos / distance) + (int)((newpos % distance) * 2 / distance);
    }
    public void SetScrollbarValue(int i)
    {
        scrollBarComponent.value = values[i];
        isScrolling =false;

    }

    void InitPositions()
    {
       distance = 1f / (childCount -1f);

        values = new float[childCount];
        for (int i = 0; i < childCount ; i++)
        {
            values[i] = i * distance;
        }

    }

    void UpdateScale()
    {
        
        Scale(index);
    }

    public void Scale(int i)
    {
        for (int j = 0; j < childCount ; j++)
        {
            if(j != i)
            {
                content.transform.GetChild(j).localScale = Vector2.Lerp(content.transform.GetChild(j).localScale, new Vector2(0.8f , 0.8f) , 0.1f);
            }
            else
            {
                content.transform.GetChild(j).localScale = Vector2.Lerp(content.transform.GetChild(j).localScale, new Vector2(1f , 1f) , 0.1f);
     
            }
        }
    }
    public void LefpToValue(float value)
    {
        scrollBarComponent.value = Mathf.Lerp(scrollBarComponent.value, value, 0.01f);
        isScrolling = true;
        Debug.Log("wao");
    }
    

    public void NextIndex()
    {
        if(index < content.transform.childCount -1)
        {
            index += 1;
        }
        Debug.Log(index);
        StartCoroutine(LeftToIndex());
    }
    public void PrevIndex()
    {
        if(index > 0)
        {
            index -= 1;
        }
        Debug.Log(index);
        StartCoroutine(LeftToIndex());

    }


    IEnumerator CheckIfStopped()
    {
        oldpos = newpos;
        isChecking = true;
        
        yield return new WaitForSeconds(0.3f);

        if(Mathf.Abs(newpos - oldpos) < distance/5)
        {
            StartCoroutine(LeftToIndex());
        }
        else
        {
            isChecking = false;

        }


    }
    IEnumerator LeftToIndex()
    {
        System.Action action = () => LefpToValue(values[index]);
                    isScrolling =true;


            loop += action;
            yield return new WaitForSeconds(0.2f);
            loop -= action;
            SetScrollbarValue(index);
    }


}
