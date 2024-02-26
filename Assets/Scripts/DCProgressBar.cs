using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DCProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI tempLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;

    private static bool _isFillabe;
    private Transform _player;
    private Transform _levelEnd;

    private float playerStartPosX;

    void Start()
    {
        _levelEnd = GameObject.FindGameObjectWithTag("LevelEnd").transform;
        _player = DCVisiblePlayerController.Instance.transform;
        tempLevelText.text = (SceneManager.GetActiveScene().buildIndex).ToString();
        nextLevelText.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        progressBar.fillAmount = 0f;

        IsFillable();
    }

    private void IsFillable()
    {
        _isFillabe = true;
        playerStartPosX = _player.transform.position.x;
    }

    void Update()
    {
        if (_isFillabe) progressBar.fillAmount = (_player.transform.position.x - playerStartPosX) / (_levelEnd.transform.position.x - playerStartPosX);
    }
}
