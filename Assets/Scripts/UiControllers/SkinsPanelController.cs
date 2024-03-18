using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UiControllers
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
        
        private DCVisiblePlayerController currentVisiblePlayer;
        private MeshFilter _playerMesh;
        private MeshRenderer _playerRenderer, _cylinderRenderer;
        
        private void Start()
        {
            PlayerPrefs.SetInt(DCSkinManager.key_Skin + "0" + DCSkinManager.key_Unlocked, 1);

            currentVisiblePlayer = DCVisiblePlayerController.Instance;
            _playerMesh = currentVisiblePlayer.GetComponent<MeshFilter>();
            _playerRenderer = currentVisiblePlayer.GetComponent<MeshRenderer>();
            _cylinderRenderer = currentVisiblePlayer.transform.GetChild(0).GetComponent<MeshRenderer>();
            
            _closeButton.onClick.AddListener(DCGameManager.Instance.SkinsBackButton);
            _skin1Button.onClick.AddListener(() => OnSelectSkinButtonClick(0));
            _skin2Button.onClick.AddListener(() => OnSelectSkinButtonClick(1));
            _skin3Button.onClick.AddListener(() => OnSelectSkinButtonClick(2));
            
            SetSkinPriceTexts();
            EnableSelectedSkin();
        }

        private void OnSelectSkinButtonClick(int index)
        {
            if (!DCSkinManager.SkinSelect(index)) _notEnoughTokensAnimationText.GetComponent<Animation>().Play();
            
            for (int i = 0; i < _lockedSkinImages.Length; i++)
            {
                if (PlayerPrefs.GetInt(DCSkinManager.key_Skin + i + DCSkinManager.key_Unlocked, 0) == 1) _lockedSkinImages[i].SetActive(false);
            }

            SetSkinPriceTexts();
            EnableSelectedSkin();
        }

        private void EnableSelectedSkin()
        {
            _playerRenderer.material = _cylinderRenderer.material = _playerSkinsScriptableObject.GetMaterialByIndex(PlayerPrefs.GetInt(DCSkinManager.key_Skin, 0));
            _playerMesh.mesh = _playerSkinsScriptableObject.GetMeshByIndex(PlayerPrefs.GetInt(DCSkinManager.key_Skin, 0));
        }
        
        private void SetSkinPriceTexts()
        {
            for (int i = 0; i < _requiredTokenTexts.Length; i++)
                _requiredTokenTexts[i].text = DCStaticVariables.RequiredTokensToUnlockSkin[i].ToString();
            
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

        public void Hide() => _panelItself.SetActive(false);

        public void Show() => _panelItself.SetActive(true);
    }
}
