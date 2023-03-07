using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;
    // private Button _button;

    private void Awake()
    {
        // this._button = GetComponentInChildren<Button>();
        this._textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        this._textMeshPro.text = baseAction.ToString();
    }
}
