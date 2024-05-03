using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private List<Button> _gemShopButtons;
        [SerializeField] private List<Button> _gemShopBackButtons;
        [SerializeField] private List<GameObject> _gemShops;

        private void Start()
        {
            foreach (var gemShopButton in _gemShopButtons)
            {
                gemShopButton.onClick.AddListener(() =>
                {
                    DCAudioManager.Instance.ClickSound();
                    SetMenuButtonsActivity(false);
                    SetGemShopsActivity(true);
                });
            }
            foreach (var gemShopBackButton in _gemShopBackButtons)
            {
                gemShopBackButton.onClick.AddListener(() =>
                {
                    DCAudioManager.Instance.ClickSound();
                    SetMenuButtonsActivity(true);
                    SetGemShopsActivity(false);
                });
            }
            SetMenuButtonsActivity(false);
            Time.timeScale = 1;
            _levelsPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
            _skinsPanel.SetActive(false);
            _settingCrossedImage.SetActive(PlayerPrefs.GetInt("Audio", 0) != 0);
            DCAudioManager.Instance.SoundIsOn = PlayerPrefs.GetInt("Audio", 0) == 0;
        }

        private void SetGemShopsActivity(bool isActive)
        {
            foreach (var gemShop in _gemShops) gemShop.SetActive(isActive);
        }
        
        private void OnDestroy()
        {
            foreach (var gemShopButton in _gemShopButtons) gemShopButton.onClick.RemoveAllListeners();
            foreach (var gemShopBackButton in _gemShopBackButtons) gemShopBackButton.onClick.RemoveAllListeners();
        }

        private void SetMenuButtonsActivity(bool state)
        {
            foreach (var button in _backToMainMenuButtons) button.SetActive(state);
        }

        public void SettingsButton()
        {
            SetMenuButtonsActivity(true);
            _mainMenuPanel.SetActive(false);
            _settingPanel.SetActive(true);
            DCAudioManager.Instance.ClickSound();
        }

        public void SkinsButton()
        {
            SetMenuButtonsActivity(true);
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
            SetMenuButtonsActivity(true);
            _levelsPanel.SetActive(true);
            _mainMenuPanel.SetActive(false);
            DCAudioManager.Instance.ClickSound();
        }

        public void BackToMainMenuButton()
        {
            _settingPanel.SetActive(false);
            SetMenuButtonsActivity(false);
            _levelsPanel.SetActive(false);
            _skinsPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
            DCAudioManager.Instance.ClickSound();
        }
    }
}
