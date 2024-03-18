using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UiControllers.Game
{
    public class DCPausePanelController : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _playButton;
    
        private void Start()
        {
            _restartButton.onClick.AddListener(DCGameManager.Instance.OnRestartButton);
            _playButton.onClick.AddListener(DCGameManager.Instance.OnResumeButton);
        }
    }
}
