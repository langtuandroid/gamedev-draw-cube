using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UiControllers.Game
{
    public class DCGoToMainMenuButtonController : MonoBehaviour
    {
        public static DCGoToMainMenuButtonController Instance;
        [SerializeField] private Button _thisButton;

        private void Awake() => Instance = this;

        private void Start() => _thisButton.onClick.AddListener(() => SceneManager.LoadScene(0));
    }
}