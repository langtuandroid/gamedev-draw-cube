using Gameplay;
using TMPro;
using UiControllers.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Managers
{
    public class DCGameManager : MonoBehaviour
    {
        public static DCGameManager Instance;
        [FormerlySerializedAs("clearedPanel")] [SerializeField] private GameObject _levelClearedPanel;
        [FormerlySerializedAs("pausedPanel")] [SerializeField] private GameObject _pausedLevelPanel;
        
        private GameObject _pauseButton;
        private GameObject _startGamePanel;
        private GameObject _progressBar;
        // private SkinsPanelController _skinsPanelController;
        private bool _isGameOver = false;
        private bool _levelCleared;
        private GameObject _confettiParticle;

        public bool IsGameOver => _isGameOver;

        private void Awake()
        {
            Instance = this;
            _confettiParticle = GameObject.FindGameObjectWithTag("Confetti");
            _confettiParticle.SetActive(false);
        }

        private void Start()
        {
            _startGamePanel = StartButtonController.Instance.gameObject.transform.parent.gameObject;
            _pauseButton = FindObjectOfType<DCPauseButtonController>().gameObject;
            _progressBar = FindObjectOfType<DCProgressBarController>().gameObject;
            Time.timeScale = 1;
            EnableStartPanel();
            AudioCheck();
            OnStartButton();
        }

        private void EnableStartPanel()
        {
            _pauseButton.SetActive(false);
            DCGoToMainMenuButtonController.Instance.gameObject.SetActive(true);
            _startGamePanel.SetActive(true);
            _pausedLevelPanel.SetActive(false);
            _levelClearedPanel.SetActive(false);
            DCDrawingBoardController.Instance.gameObject.SetActive(false);
            _progressBar.SetActive(false);
            DCLineWorker.Instance.enabled = false;
        }

        public void ShowWinPanel()
        {
            if (!_levelCleared)
            {
                int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
                if (SceneManager.sceneCountInBuildSettings > nextLevelIndex) PlayerPrefs.SetInt("Level", nextLevelIndex);
            
                _levelCleared = true;
                Time.timeScale = 1f;
                DCAudioManager.Instance.LevelWinSound();
                _pauseButton.SetActive(false);
                _levelClearedPanel.SetActive(true);
                _startGamePanel.SetActive(false);
                _pausedLevelPanel.SetActive(false);
                DCDrawingBoardController.Instance.gameObject.SetActive(false);
                _progressBar.SetActive(false);
                DCLineWorker.Instance.enabled = false;

                _confettiParticle.SetActive(true);
            }
        }

        private void PausedPanelActivation()
        {
            _startGamePanel.SetActive(false);
            _pausedLevelPanel.SetActive(true);
            DCDrawingBoardController.Instance.gameObject.SetActive(false);
            _progressBar.SetActive(false);
            DCLineWorker.Instance.enabled = false;
        }

        private void AudioCheck()
        {
            if (PlayerPrefs.GetInt("Audio", 0) == 0)
            {
                DCAudioManager.Instance.SoundIsOn = true;
                DCAudioManager.Instance.PlayBackgroundMusic();
            }
            else
            {
                DCAudioManager.Instance.SoundIsOn = false;
                DCAudioManager.Instance.StopBackgroundMusic();
            }
        }

        public void OnStartButton()
        {
            _pauseButton.SetActive(true);
            DCGoToMainMenuButtonController.Instance.gameObject.SetActive(false);
            _startGamePanel.SetActive(false);
            DCAudioManager.Instance.ClickSound();
            DCDrawingBoardController.Instance.gameObject.SetActive(true);
            _progressBar.SetActive(true);
            DCLineWorker.Instance.enabled = true;
            if (DCLineWorker.Instance.GetMainCameraTransform() && DCLineWorker.Instance.GetMainCameraTransform().childCount > 0)
            {
                DCLineWorker.Instance.GetMainCameraTransform().GetChild(0).gameObject.SetActive(true);
            }
        }

        public void OnRestartButton()
        {
            DCAudioManager.Instance.ClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnAudioButton()
        {
            PlayerPrefs.SetInt("Audio", PlayerPrefs.GetInt("Audio", 0) == 0 ? 1 : 0);
            DCAudioManager.Instance.ClickSound();
            AudioCheck();
        }

        public void OnPauseButton()
        {
            _pauseButton.SetActive(false);
            PausedPanelActivation();
            DCAudioManager.Instance.StopBackgroundMusic();
            Time.timeScale = 0f;
        }

        public void OnResumeButton()
        {
            Time.timeScale = 1f;
            DCAudioManager.Instance.PlayBackgroundMusic();
            _pauseButton.SetActive(true);
            _pausedLevelPanel.SetActive(false);
            DCDrawingBoardController.Instance.gameObject.SetActive(true);
            _progressBar.SetActive(true);
            DCLineWorker.Instance.enabled = true;
        }

        public void OnNextLevelButton()
        {
            DCAudioManager.Instance.ClickSound();
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings <= nextLevelIndex ? 0 : nextLevelIndex);
        }
    }
}
