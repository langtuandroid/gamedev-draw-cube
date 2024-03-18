using Gameplay;
using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UiControllers.Game
{
    public class SkinsPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject _panelItself;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _skin1Button;
        [SerializeField] private Button _skin2Button;
        [SerializeField] private Button _skin3Button;
        [SerializeField] private TextMeshProUGUI[] _requiredTokenTexts;
        [SerializeField] private GameObject[] _lockedSkinImages;
        [SerializeField] private GameObject _notEnoughTokensAnimationText;
        [SerializeField] private PlayerSkinsScriptableObject _playerSkinsScriptableObject;
        
        private DCVisiblePlayerController _currentVisiblePlayer;
        private MeshRenderer _playerRenderer;
        private MeshRenderer _cylinderRenderer;
        private MeshFilter _playerMeshFilter;
        
        private void Start()
        {
            PlayerPrefs.SetInt(DCSkinManager.key_Skin + "0" + DCSkinManager.key_Unlocked, 1);

            _currentVisiblePlayer = DCVisiblePlayerController.Instance;
            _playerMeshFilter = _currentVisiblePlayer.GetComponent<MeshFilter>();
            _playerRenderer = _currentVisiblePlayer.GetComponent<MeshRenderer>();
            _cylinderRenderer = _currentVisiblePlayer.transform.GetChild(0).GetComponent<MeshRenderer>();
            
            _closeButton.onClick.AddListener(DCGameManager.Instance.OnSkinsBackButton);
            _skin1Button.onClick.AddListener(() => OnSelectSkinClick(0));
            _skin2Button.onClick.AddListener(() => OnSelectSkinClick(1));
            _skin3Button.onClick.AddListener(() => OnSelectSkinClick(2));
            
            SetSkinPrice();
            EnableSkin();
        }

        private void OnSelectSkinClick(int index)
        {
            if (!DCSkinManager.SelectSkin(index)) _notEnoughTokensAnimationText.GetComponent<Animation>().Play();
            
            for (int i = 0; i < _lockedSkinImages.Length; i++)
            {
                if (PlayerPrefs.GetInt(DCSkinManager.key_Skin + i + DCSkinManager.key_Unlocked, 0) == 1) _lockedSkinImages[i].SetActive(false);
            }

            SetSkinPrice();
            EnableSkin();
        }

        private void EnableSkin()
        {
            _playerRenderer.material = _cylinderRenderer.material = _playerSkinsScriptableObject.GetMaterialByIndex(PlayerPrefs.GetInt(DCSkinManager.key_Skin, 0));
            _playerMeshFilter.mesh = _playerSkinsScriptableObject.GetMeshByIndex(PlayerPrefs.GetInt(DCSkinManager.key_Skin, 0));
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

        public void HideThisPanel() => _panelItself.SetActive(false);

        public void ShowThisPanel() => _panelItself.SetActive(true);
    }
}
