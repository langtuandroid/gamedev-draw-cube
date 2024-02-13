using UnityEngine;
using UnityEngine.UI;

public class PauseButtonController : MonoBehaviour
{
    private Button _thisButton;
    private void Start()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(DCGameManager.Instance.PauseButton);
    }
}
