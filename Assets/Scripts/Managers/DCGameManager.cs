using Gameplay;
using TMPro;
using UiControllers;
using UiControllers.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Managers
{
    public class DCGameManager : MonoBehaviour
    {
        public static DCGameManager Instance;
        [FormerlySerializedAs("startPanel")] [SerializeField] private GameObject _startGamePanel;
        [FormerlySerializedAs("clearedPanel")] [SerializeField] private GameObject _levelClearedPanel;
        [FormerlySerializedAs("pausedPanel")] [SerializeField] private GameObject _pausedLevelPanel;
        [FormerlySerializedAs("pauseButton")] [SerializeField] private GameObject _pauseButton;
        [FormerlySerializedAs("muteImage")] [SerializeField] private GameObject _audioMuteImage;
        [FormerlySerializedAs("progressBar")] [SerializeField] private GameObject _progressBarReference;
        [FormerlySerializedAs("levelClearedTexts")] [SerializeField] private TextMeshProUGUI[] _levelClearedTexts;
        
        private SkinsPanelController _skinsPanelController;
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
            _skinsPanelController = FindObjectOfType<SkinsPanelController>();
            Time.timeScale = 1;
            EnableStartPanel();
            AudioCheck();
        }

        private void EnableStartPanel()
        {
            _pauseButton.SetActive(false);
            DCGoToMainMenuButtonController.Instance.gameObject.SetActive(true);
            _startGamePanel.SetActive(true);
            _skinsPanelController.HideThisPanel();
            _pausedLevelPanel.SetActive(false);
            _levelClearedPanel.SetActive(false);
            DCDrawingBoardController.Instance.gameObject.SetActive(false);
            _progressBarReference.SetActive(false);
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
                _skinsPanelController.HideThisPanel();
                _pausedLevelPanel.SetActive(false);
                DCDrawingBoardController.Instance.gameObject.SetActive(false);
                _progressBarReference.SetActive(false);
                DCLineWorker.Instance.enabled = false;

                foreach (var levelText in _levelClearedTexts) levelText.text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex) + "\nCLEARED";

                _confettiParticle.SetActive(true);
            }
        }

        private void SkinsPanelActivation()
        {
            _startGamePanel.SetActive(false);
            _skinsPanelController.ShowThisPanel();
            _pausedLevelPanel.SetActive(false);
        }

        private void PausedPanelActivation()
        {
            _startGamePanel.SetActive(false);
            _skinsPanelController.HideThisPanel();
            _pausedLevelPanel.SetActive(true);
            DCDrawingBoardController.Instance.gameObject.SetActive(false);
            _progressBarReference.SetActive(false);
            DCLineWorker.Instance.enabled = false;
        }

        private void AudioCheck()
        {
            if (PlayerPrefs.GetInt("Audio", 0) == 0)
            {
                _audioMuteImage.SetActive(false);
                DCAudioManager.Instance.SoundIsOn = true;
                DCAudioManager.Instance.PlayBackgroundMusic();
            }
            else
            {
                _audioMuteImage.SetActive(true);
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
            _progressBarReference.SetActive(true);
            DCLineWorker.Instance.enabled = true;
            DCLineWorker.Instance.GetMainCameraTransform().GetChild(0).gameObject.SetActive(true);
        }

        public void OnRestartButton()
        {
            DCAudioManager.Instance.ClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnSkinsBackButton()
        {
            EnableStartPanel();
            DCAudioManager.Instance.ClickSound();
        }

        public void OnAudioButton()
        {
            DCAudioManager.Instance.ClickSound();
            if (PlayerPrefs.GetInt("Audio", 0) == 0)
                PlayerPrefs.SetInt("Audio", 1);
            else
                PlayerPrefs.SetInt("Audio", 0);
            AudioCheck();
        }

        public void OnSkinsButton()
        {
            SkinsPanelActivation();
            DCAudioManager.Instance.ClickSound();
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
            _progressBarReference.SetActive(true);
            DCLineWorker.Instance.enabled = true;
        }

        public void OnHomeButton()
        {
            OnResumeButton();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnNextLevelButton()
        {
            DCAudioManager.Instance.ClickSound();
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (SceneManager.sceneCountInBuildSettings <= nextLevelIndex) SceneManager.LoadScene(0);
            else SceneManager.LoadScene(nextLevelIndex);
        }
    }
}
