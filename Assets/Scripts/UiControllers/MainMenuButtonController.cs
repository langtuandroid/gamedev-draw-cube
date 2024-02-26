using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonController : MonoBehaviour
{
    public static MainMenuButtonController Instance;
    [SerializeField] private Button _thisButton;

    private void Awake() => Instance = this;

    private void Start() => _thisButton.onClick.AddListener(() => SceneManager.LoadScene(0));
}