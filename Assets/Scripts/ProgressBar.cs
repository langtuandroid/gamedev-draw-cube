using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ProgressBar : MonoBehaviour
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
        _player = VisiblePlayerController.Instance.transform;
        tempLevelText.text = (PlayerPrefs.GetInt("CurrentVisibleLevel", 0) + 1).ToString();//SceneManager.GetActiveScene().buildIndex + 1)).ToString();
        nextLevelText.text = (PlayerPrefs.GetInt("CurrentVisibleLevel", 0) + 2).ToString();//(SceneManager.GetActiveScene().buildIndex + 2).ToString();
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
