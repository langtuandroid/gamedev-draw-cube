using UnityEngine;
using TMPro;
using UiControllers;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel, clearedPanel, pausedPanel, pauseButton, muteImage, progressBar;
    [SerializeField] private SkinsPanelController skinsPanel;
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
        if (Time.time == Time.timeSinceLevelLoad)
        {
            var visibleLevel = PlayerPrefs.GetInt("CurrentVisibleLevel", 0);
            var levelIndex = PlayerPrefs.GetInt("Level", 0);
            if (SceneManager.sceneCountInBuildSettings <= visibleLevel)
            {
                levelIndex = Random.Range(0, 29);
                visibleLevel++;
            }
            else
            {
                visibleLevel = levelIndex;
            }
            PlayerPrefs.SetInt("Level", levelIndex);
            PlayerPrefs.SetInt("CurrentVisibleLevel", visibleLevel);
            SceneManager.LoadScene(levelIndex);
        }

        Time.timeScale = 1;
        StartPanelActivation();
        AudioCheck();
    }

    private void StartPanelActivation()
    {
        pauseButton.SetActive(false);
        startPanel.SetActive(true);
        skinsPanel.Hide();
        pausedPanel.SetActive(false);
        clearedPanel.SetActive(false);
        DrawingBoardController.Instance.gameObject.SetActive(false);
        progressBar.SetActive(false);
        Line.Instance.enabled = false;
    }

    public void ShowWinPanel()
    {
        if (!_cleared)
        {
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            var visibleLevel = PlayerPrefs.GetInt("CurrentVisibleLevel", 0);
            
            if (SceneManager.sceneCountInBuildSettings <= visibleLevel) nextLevelIndex = Random.Range(0, 29);
        
            PlayerPrefs.SetInt("Level", nextLevelIndex);
            PlayerPrefs.SetInt("CurrentVisibleLevel", visibleLevel + 1);
            
            _cleared = true;
            Time.timeScale = 1f;
            AudioManager.Instance.LevelClearedSound();
            pauseButton.SetActive(false);
            clearedPanel.SetActive(true);
            startPanel.SetActive(false);
            skinsPanel.Hide();
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
        skinsPanel.Show();
        pausedPanel.SetActive(false);
    }

    private void PausedPanelActivation()
    {
        startPanel.SetActive(false);
        skinsPanel.Hide();
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

        if (SceneManager.sceneCountInBuildSettings <= PlayerPrefs.GetInt("CurrentVisibleLevel", 0)) 
            nextLevelIndex = Random.Range(0, 29);
        
        PlayerPrefs.SetInt("Level", nextLevelIndex);
        SceneManager.LoadScene(nextLevelIndex);
    }
}
