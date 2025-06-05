using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonController : MonoBehaviour
{
    [SerializeField] private Button _Bstart;
    [SerializeField] private Button _Bsetting;
    [SerializeField] private Button _BsettingBack;
    [SerializeField] private Button _BsettingQuit;
    [SerializeField] private Button _BsettingRePlay;
    [SerializeField] private Button _Bpause;

    [SerializeField] private Canvas _Csetting;

    [SerializeField] Sprite pause;
    [SerializeField] Sprite resume;

    void Awake()
    {
        _Csetting.gameObject.SetActive(false);

    }
    public void Start()
    {

        _Bstart.onClick.AddListener(PlayGame);
        _Bsetting.onClick.AddListener(EnableSettingUI);
        _BsettingBack.onClick.AddListener(DisableSettingUI);
        _BsettingQuit.onClick.AddListener(QuitGame);
        _BsettingRePlay.onClick.AddListener(RePlay);
        _Bpause.onClick.AddListener(Pause);


    }
    public void PlayGame()
    {
        GameManager.Instance.Play();
    }

    public void Pause()
    {
        Image image = _Bpause.GetComponent<Image>();

        if (Time.timeScale == 0)
        {
            GameManager.Instance.Resume();
            GameManager.Instance.SetFlagPause(false);
            image.sprite = pause;

        }
        else
        {
            GameManager.Instance.Pause();
            GameManager.Instance.SetFlagPause(true);
            image.sprite = resume;
        }
    }
    public void RePlay()
    {
        GameManager.Instance.RePlay();

        DisableSettingUI();
    }



    public void EnableSettingUI()
    {
        Debug.Log("Tao ddax duoc nhan");
        _Csetting.gameObject.SetActive(true);

        if (!GameManager.Instance.IsFlagPause())
        {
            GameManager.Instance.Pause();
        }

    }

    public void DisableSettingUI()
    {
        _Csetting.gameObject.SetActive(false);
        if (!GameManager.Instance.IsFlagPause())
        {
            GameManager.Instance.Resume();
        }
    }

    public void QuitGame()
    {
        GameManager.Instance.Resume();

        StartCoroutine(LoadAsyncMainMeunuScene());
    }

    IEnumerator LoadAsyncMainMeunuScene()
    {
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/SceneGames/Menu2");

        while (!asyncLoad.isDone)
        {
            yield return null; // Đợi đến khi load xong
        }
    }

}
