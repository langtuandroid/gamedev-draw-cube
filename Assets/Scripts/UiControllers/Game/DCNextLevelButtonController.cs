using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UiControllers.Game
{
    public class DCNextLevelButtonController : MonoBehaviour
    {
        private Button _thisButton;
        private void Start()
        {
            _thisButton = GetComponent<Button>();
            _thisButton.onClick.AddListener(DCGameManager.Instance.OnNextLevelButton);
        }
    }
}
