using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UiControllers.Game
{
    public class DCProgressBarController : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private TextMeshProUGUI _nextLevelText;

        private static bool _isFillable;
        private Transform _player;
        private Transform _levelEnd;

        private float _playerStartPosX;

        void Start()
        {
            _levelEnd = GameObject.FindGameObjectWithTag("LevelEnd").transform;
            _player = DCVisiblePlayerController.Instance.transform;
            _currentLevelText.text = (SceneManager.GetActiveScene().buildIndex).ToString();
            _nextLevelText.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
            _progressBar.fillAmount = 0f;
            
            _isFillable = true;
            _playerStartPosX = _player.transform.position.x;
        }

        void Update()
        {
            if (_isFillable) _progressBar.fillAmount = (_player.transform.position.x - _playerStartPosX) / (_levelEnd.transform.position.x - _playerStartPosX);
        }
    }
}
