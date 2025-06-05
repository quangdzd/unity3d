using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject border; 
    public GameObject mapImage;
    private int level ; 
    private string _enemyLocationPath;
    private int _challengeRating;

    void Start()
    {
        SoundManager.Instance.PlayMusic("bmmenu");
    }
    public void SetBorder(GameObject panel)
    {
        border.transform.SetParent(panel.transform, false);
        RectTransform rect = border.GetComponent<RectTransform>();

        // Gán tọa độ anchoredPosition (vị trí tương đối so với anchor)
        rect.anchoredPosition = new Vector2(91f, rect.anchoredPosition.y);    
    }
    public void SetLevel(int level)
    {
        this.level = level;
    }
    public void SetImage(Sprite image)
    {
        Button mapI = mapImage.GetComponent<Button>();
        mapI.image.sprite = image;
        Debug.Log("yahoo");
    }
    public void Enable()
    {
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }


    public void LoadGame()
    {
        StartCoroutine(LoadAsyncPLayScene());
        // PlacementManager.Instance.LoadMap();
    }
    public void SetenemyLocationPath(string enemyLocationPath)
    {
        this._enemyLocationPath = "GameLevels/1/" + enemyLocationPath;
    }
    public void SetchallengeRating(int challengeRating)
    {
        this._challengeRating = challengeRating;
    }

    IEnumerator LoadAsyncPLayScene()
    {
        GameStateTransfer.Instance.EnemyLocationPath = this._enemyLocationPath;
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/GamePlayScene");

        while (!asyncLoad.isDone)
        {
            yield return null; // Đợi đến khi load xong
        }
    }
}
