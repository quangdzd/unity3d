using UnityEngine;
using UnityEngine.UI;

public class DeactivatePS : MonoBehaviour
{
    public Button button;
    public GameObject UI;
    private PointerHandlerButton pointerHandlerButton;
    private CanvasGroup _buttonCanvasGroup;
    private void Awake() {
        pointerHandlerButton = button.GetComponent<PointerHandlerButton>();
        _buttonCanvasGroup = button.GetComponent<CanvasGroup>();

    }
    private void Start() {
        button.onClick.AddListener(() => DeactivatePSAndUI());
        pointerHandlerButton.OnPointerEnterEvent += OnPointerEnterButton;
        pointerHandlerButton.OnPointerExitEvent += OnPointerExitButton;

    }

    public bool isPsActive()
    {
        return PlacementManager.Instance.IsActive();
    }
    public void DeactivatePSAndUI()
    {
        PlacementManager.Instance.PS_Deactivate();
        UI.SetActive(false);
        _buttonCanvasGroup.alpha = 0;
        
    }
    public void OnPointerEnterButton()
    {
        if(isPsActive())
        {
            UI.SetActive(true);
            _buttonCanvasGroup.alpha= 1;

        }
    }
    public void OnPointerExitButton()
    {
        if(isPsActive())
        {
            UI.SetActive(false);
            _buttonCanvasGroup.alpha = 0;
        }


    }

}
