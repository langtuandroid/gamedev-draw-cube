using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UiControllers.Menu
{
    public class DcLevelSelectButtonController : MonoBehaviour
    {
        [SerializeField] private Button _thisButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _lockImage;
        [SerializeField] private Image _lockedIcon;
        private int _levelIndex;
    
        public void Initialize(int levelIndex)
        {
            _levelIndex = levelIndex;
            var isUnlocked = _levelIndex <= PlayerPrefs.GetInt("Level", 1);
            _lockImage.enabled = !isUnlocked;
            _lockedIcon.enabled = !isUnlocked;
            _thisButton.interactable = isUnlocked;
            _levelText.text = _levelIndex.ToString();
            _thisButton.onClick.AddListener(SelectLevel);
        }

        private void SelectLevel()
        {
            SceneManager.LoadScene(_levelIndex);
        }
    }
}