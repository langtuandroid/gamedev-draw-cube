using UnityEngine;
using UnityEngine.SceneManagement;

namespace UiControllers.Menu
{
    public class DcLevelButtonsGeneratorController : MonoBehaviour
    {
        [SerializeField] private DcLevelSelectButtonController _buttonPrefab;
        [SerializeField] private GameObject _emptyPrefab;
        [SerializeField] private RectTransform _levelsPanel;
    
        void Start()
        {
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var levelButton = Instantiate(_buttonPrefab, _levelsPanel);
                levelButton.Initialize(i);
            }
            Instantiate(_emptyPrefab, _levelsPanel);
            Instantiate(_emptyPrefab, _levelsPanel);
            Instantiate(_emptyPrefab, _levelsPanel);
            Instantiate(_emptyPrefab, _levelsPanel);
        }
    }
}
