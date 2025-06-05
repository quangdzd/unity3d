using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EntityInfoDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _hp;
    [SerializeField] private TMP_Text _dmg;
    [SerializeField] private TMP_Text _spd;
    [SerializeField] private TMP_Text _ar;

    [SerializeField] private TMP_Text _as;

    [SerializeField] private UnityEngine.UI.Image image;

    [SerializeField] private Button close;
    [SerializeField] private Button display;
    [SerializeField] private GameObject StatsPanel;
    public void Start()
    {
        close.onClick.AddListener(Close);
        display.onClick.AddListener(Display);
    }

    public void Close()
    {
        StatsPanel.SetActive(false);
        display.gameObject.SetActive(true);
    }
    public void Display()
    {
        StatsPanel.SetActive(true);
        display.gameObject.SetActive(false);
    }
    public void ChangeInfo (StatsForEnemy stats)
    {
        _name.text = stats._name.ToString();
        _hp.text = stats._hp.ToString();
        _dmg.text = stats._dmg.ToString();
        _spd.text = stats._speed.ToString();
        _as.text = stats._attackSpeed.ToString();
        _ar.text = stats._attackThreshold.ToString();

        image.sprite = stats._image;


    }



}
