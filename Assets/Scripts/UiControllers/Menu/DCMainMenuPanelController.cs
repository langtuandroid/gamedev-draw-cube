using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UiControllers.Menu
{
    public class DcMainMenuPanelController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _backToMainMenuButtons;
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _levelsPanel;
        [SerializeField] private GameObject _settingPanel;
        [SerializeField] private GameObject _skinsPanel;
        [SerializeField] private GameObject _settingCrossedImage;

        private void Start()
        {
            Time.timeScale = 1;
            foreach (var btn in _backToMainMenuButtons) btn.SetActive(false);
            _levelsPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
            _skinsPanel.SetActive(false);
            _settingCrossedImage.SetActive(PlayerPrefs.GetInt("Audio", 0) != 0);
            DCAudioManager.Instance.SoundIsOn = PlayerPrefs.GetInt("Audio", 0) == 0;
        }

        public void SettingsButton()
        {
            foreach (var btn in _backToMainMenuButtons) btn.SetActive(true);
            _mainMenuPanel.SetActive(false);
            _settingPanel.SetActive(true);
            DCAudioManager.Instance.ClickSound();
        }

        public void SkinsButton()
        {
            foreach (var btn in _backToMainMenuButtons) btn.SetActive(true);
            _mainMenuPanel.SetActive(false);
            _settingPanel.SetActive(false);
            _skinsPanel.SetActive(true);
            DCAudioManager.Instance.ClickSound();
        }

        public void AudioButton()
        {
            _settingCrossedImage.SetActive(!DCAudioManager.Instance.SoundIsOn);
            PlayerPrefs.SetInt("Audio", PlayerPrefs.GetInt("Audio", 0) == 0 ? 1 : 0);

            _settingCrossedImage.SetActive(PlayerPrefs.GetInt("Audio", 0) != 0);
            DCAudioManager.Instance.SoundIsOn = PlayerPrefs.GetInt("Audio", 0) == 0;
            DCAudioManager.Instance.ClickSound();
        }

        public void PlayButton()
        {
            foreach (var btn in _backToMainMenuButtons) btn.SetActive(true);
            _levelsPanel.SetActive(true);
            _mainMenuPanel.SetActive(false);
            DCAudioManager.Instance.ClickSound();
        }

        public void BackToMainMenuButton()
        {
            _settingPanel.SetActive(false);
            foreach (var btn in _backToMainMenuButtons) btn.SetActive(false);
            _levelsPanel.SetActive(false);
            _skinsPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
            DCAudioManager.Instance.ClickSound();
        }

        public void ExitButton()
        {
            Application.Quit();
        }
    }
}
