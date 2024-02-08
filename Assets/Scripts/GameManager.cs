using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel, clearedPanel, skinsPanel, pausedPanel, pauseButton, muteImage, progressBar;
    [SerializeField] private TextMeshProUGUI[] levelClearedTexts;

    private bool _isGameOver = false;

    public static GameManager Instance;
    private bool _cleared;
    private GameObject _confetti;

    public bool IsGameOver => _isGameOver;

    private void Awake()
    {
        Instance = this;
        _confetti = GameObject.FindGameObjectWithTag("Confetti");
        _confetti.SetActive(false);
    }

    private void Start()
    {
        if (Time.time == Time.timeSinceLevelLoad) SceneManager.LoadScene(PlayerPrefs.GetInt("Level", 0));

        Time.timeScale = 1;
        StartPanelActivation();
        AudioCheck();
    }

    private void StartPanelActivation()
    {
        pauseButton.SetActive(false);
        startPanel.SetActive(true);
        skinsPanel.SetActive(false);
        pausedPanel.SetActive(false);
        clearedPanel.SetActive(false);
        DrawingBoardController.Instance.gameObject.SetActive(false);
        progressBar.SetActive(false);
        Line.Instance.enabled = false;
    }

    public void ClearedPanelActivation()
    {
        if (!_cleared)
        {
            _cleared = true;
            Time.timeScale = 1f;
            AudioManager.Instance.LevelClearedSound();
            pauseButton.SetActive(false);
            clearedPanel.SetActive(true);
            startPanel.SetActive(false);
            skinsPanel.SetActive(false);
            pausedPanel.SetActive(false);
            DrawingBoardController.Instance.gameObject.SetActive(false);
            progressBar.SetActive(false);
            Line.Instance.enabled = false;

            for (int i = 0; i < levelClearedTexts.Length; i++)
                levelClearedTexts[i].text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex + 1) + "\nCLEARED";

            _confetti.SetActive(true);
        }
    }

    private void SkinsPanelActivation()
    {
        startPanel.SetActive(false);
        skinsPanel.SetActive(true);
        pausedPanel.SetActive(false);
    }

    private void PausedPanelActivation()
    {
        startPanel.SetActive(false);
        skinsPanel.SetActive(false);
        pausedPanel.SetActive(true);
        DrawingBoardController.Instance.gameObject.SetActive(false);
        progressBar.SetActive(false);
        Line.Instance.enabled = false;
    }

    private void AudioCheck()
    {
        if (PlayerPrefs.GetInt("Audio", 0) == 0)
        {
            muteImage.SetActive(false);
            AudioManager.Instance.soundIsOn = true;
            AudioManager.Instance.PlayBackgroundMusic();
        }
        else
        {
            muteImage.SetActive(true);
            AudioManager.Instance.soundIsOn = false;
            AudioManager.Instance.StopBackgroundMusic();
        }
    }

    public void StartButton()
    {
        pauseButton.SetActive(true);
        startPanel.SetActive(false);
        AudioManager.Instance.ButtonClickSound();
        DrawingBoardController.Instance.gameObject.SetActive(true);
        progressBar.SetActive(true);
        Line.Instance.enabled = true;
        Line.Instance.GetMainCameraTransform().GetChild(0).gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        AudioManager.Instance.ButtonClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SkinsBackButton()
    {
        StartPanelActivation();
        AudioManager.Instance.ButtonClickSound();
    }

    public void AudioButton()
    {
        AudioManager.Instance.ButtonClickSound();
        if (PlayerPrefs.GetInt("Audio", 0) == 0)
            PlayerPrefs.SetInt("Audio", 1);
        else
            PlayerPrefs.SetInt("Audio", 0);
        AudioCheck();
    }

    public void SkinsButton()
    {
        SkinsPanelActivation();
        AudioManager.Instance.ButtonClickSound();
    }

    public void PauseButton()
    {
        pauseButton.SetActive(false);
        PausedPanelActivation();
        AudioManager.Instance.StopBackgroundMusic();
        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlayBackgroundMusic();
        pauseButton.SetActive(true);
        pausedPanel.SetActive(false);
        DrawingBoardController.Instance.gameObject.SetActive(true);
        progressBar.SetActive(true);
        Line.Instance.enabled = true;
    }

    public void HomeButton()
    {
        ResumeButton();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevelButton()
    {
        AudioManager.Instance.ButtonClickSound();
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.sceneCountInBuildSettings <= nextLevelIndex)
            Debug.LogWarning("THERE ARE NO MORE SCENES!");
        else
        {
            PlayerPrefs.SetInt("Level", nextLevelIndex);
            SceneManager.LoadScene(nextLevelIndex);
        }
    }
}
