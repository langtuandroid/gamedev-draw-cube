using UnityEngine;
using UnityEngine.UI;

namespace UiControllers
{
    public class NextLevelButtonController : MonoBehaviour
    {
        private Button _thisButton;
        private void Start()
        {
            _thisButton = GetComponent<Button>();
            _thisButton.onClick.AddListener(DCGameManager.Instance.NextLevelButton);
        }
    }
}
