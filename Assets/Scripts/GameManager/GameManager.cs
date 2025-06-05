using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class GameManager : SingletonDestroy<GameManager>
{
    private bool isPlay;
    private bool isPause = false;
    private int map = 1;

    private bool isUpdatingMap;


    private Money money;
    private JSONReader jsonReader;
    [SerializeField] private Camera main_camera;
    public TMP_Text money_text;
    public GameObject money_panel;


    public event Action OnPlayStarted;

    protected override void Awake()
    {
        base.Awake();

        jsonReader = new JSONReader();
        isPlay = false;

    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic("fightingBgm");

        String enemyLocationPath = GameStateTransfer.Instance.EnemyLocationPath;
        WaveDatas waveDatas = jsonReader.WavedatasReader(enemyLocationPath);
        int seedmoney = waveDatas.seed_money;
        SetSeedMoney(seedmoney);
        SetLevelMap();
    }
    public void SetSeedMoney(int seedMoney)
    {
        money = new Money();
        money.MoneyChanged += UpdateMoneyUI;
        GetReward(seedMoney);
    }
    public void UpdateMoneyUI()
    {
        money_text.text = money.GetAmount().ToString();
    }

    public void SpendMoney(int money)
    {
        this.money.Subtract(money);
        StartCoroutine(ChangeMoney(money*-1));
        Debug.Log("Spend :" + money);
    }

    public void GetReward(int reward)
    {
        this.money.Add(reward);
        StartCoroutine(ChangeMoney(reward));
        Debug.Log("Get :" + money);
    }


    

    public int GetMoney()
    {
        return this.money.GetAmount();
    }
    public bool isGamePlay()
    {
        return isPlay;
    }
    public void Play()
    {
        if (!isGamePlay())
        {
            isPlay = true;
            OnPlayStarted?.Invoke();
            PlacementManager.Instance.PS_Deactivate();
        }
        

    }
    public void Stop()
    {
        isPlay = false;
        EntityManager.Instance.Clear();
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }
    public void RePlay()
    {
        EntityManager.Instance.Clear();
        ResetLevelMap();
        SetLevelMap();
        ResetCamera();


    }

    public void SetLevelMap()
    {
        PlacementManager.Instance.SetMapdata(map.ToString());
        PlacementManager.Instance.LoadAllies();
    }

    public void ResetLevelMap()
    {
        map = 1;
    }

    public void UpdateLevelMap()
    {
        map += 1;
    }


    public void SetFlagPause(bool flag)
    {
        isPause = flag;
    }
    public bool IsFlagPause()
    {
        return isPause;
    }
    public void ResetCamera()
    {
        main_camera.GetComponent<CameraController>().ResetCamera();
    }

    public void CheckUpdate()
    {
        if (EntityManager.Instance.GetRedTeamMenbersCount() == 0)
        {
            StartCoroutine(UpdateMap());
        }
    }
    public IEnumerator UpdateMap()
    {
        isUpdatingMap = true;
        yield return new WaitForSeconds(2f);

        UpdateLevelMap();
        SetLevelMap();
        ResetCamera();

        isUpdatingMap = false;
        isPlay = false;

    }

    public IEnumerator ChangeMoney(int money, float duration = 1f)
    {

        GameObject money_change = PoolManager.Instance.GetFromPool("money_change", new Vector3(), new Quaternion(), false);
        money_change.transform.SetParent(money_panel.transform, false);
        RectTransform rectTransform = money_change.GetComponent<RectTransform>();


        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 targetPos = startPos + new Vector2(0, 50);

        TMP_Text tmp_Text = money_change.transform.Find("Text (TMP)").GetComponent<TMP_Text>();
        tmp_Text.text = money.ToString("+#;-#");

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }
        rectTransform.anchoredPosition = targetPos;

        yield return new WaitForSeconds(0.1f);

        rectTransform.anchoredPosition = startPos;

        PoolManager.Instance.AddToPool(money_change, "money_change");


    }

}
