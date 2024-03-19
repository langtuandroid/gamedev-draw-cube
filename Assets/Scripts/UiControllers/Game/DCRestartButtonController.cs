using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UiControllers.Game
{
    public class DCRestartButtonController : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
    
        private void Start()
        {
            _restartButton.onClick.AddListener(DCGameManager.Instance.OnRestartButton);
        }
    }
}