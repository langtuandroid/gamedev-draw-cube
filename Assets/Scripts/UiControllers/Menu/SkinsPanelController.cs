using Gameplay;
using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UiControllers.Menu
{
    public class SkinsPanelController : MonoBehaviour
    {
        [SerializeField] private Button _skin1Button;
        [SerializeField] private Button _skin2Button;
        [SerializeField] private Button _skin3Button;
        [SerializeField] private Image _mainSkinImage;
        [SerializeField] private TextMeshProUGUI[] _requiredTokenTexts;
        [SerializeField] private GameObject[] _lockedSkinImages;
        [SerializeField] private Image[] _skinIcons;
        
        private void Start()
        {
            PlayerPrefs.SetInt(DCSkinManager.key_Skin + "0" + DCSkinManager.key_Unlocked, 1);
            
            _skin1Button.onClick.AddListener(() => OnSelectSkinClick(0));
            _skin2Button.onClick.AddListener(() => OnSelectSkinClick(1));
            _skin3Button.onClick.AddListener(() => OnSelectSkinClick(2));
            _mainSkinImage.sprite = _skinIcons[PlayerPrefs.GetInt(DCSkinManager.key_Skin, 0)].sprite;
            _mainSkinImage.color = _skinIcons[PlayerPrefs.GetInt(DCSkinManager.key_Skin, 0)].color;
                
            SetSkinPrice();
        }

        private void OnSelectSkinClick(int index)
        {
            if (DCSkinManager.SelectSkin(index))
            {
                _mainSkinImage.sprite = _skinIcons[index].sprite;
                _mainSkinImage.color = _skinIcons[index].color;
            }
            
            for (int i = 0; i < _lockedSkinImages.Length; i++)
            {
                if (PlayerPrefs.GetInt(DCSkinManager.key_Skin + i + DCSkinManager.key_Unlocked, 0) == 1) _lockedSkinImages[i].SetActive(false);
            }

            SetSkinPrice();
        }
        
        private void SetSkinPrice()
        {
            for (int i = 0; i < _requiredTokenTexts.Length; i++)
                _requiredTokenTexts[i].text = DCStaticVariables.TokensToUnlockSkin[i].ToString();
            
            for (int i = 0; i < _lockedSkinImages.Length; i++)
            {
                var isOpened = PlayerPrefs.GetInt(DCSkinManager.key_Skin + i + DCSkinManager.key_Unlocked, 0);
                if (isOpened == 1)
                {
                    _lockedSkinImages[i].SetActive(false);
                    _requiredTokenTexts[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
