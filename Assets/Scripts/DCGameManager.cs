using UnityEngine;
using TMPro;
using UiControllers;
using UnityEngine.SceneManagement;

public class DCGameManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel, clearedPanel, pausedPanel, pauseButton, muteImage, progressBar;
    [SerializeField] private SkinsPanelController skinsPanel;
    [SerializeField] private TextMeshProUGUI[] levelClearedTexts;

    private bool _isGameOver = false;

    public static DCGameManager Instance;
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
            int levelIndex = PlayerPrefs.GetInt("Level", 0);
            
            if (SceneManager.sceneCountInBuildSettings > visibleLevel) visibleLevel = levelIndex;
            
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
        DCDrawingBoardController.Instance.gameObject.SetActive(false);
        progressBar.SetActive(false);
        DCLineWorker.Instance.enabled = false;
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
            DCAudioManager.Instance.LevelClearedSound();
            pauseButton.SetActive(false);
            clearedPanel.SetActive(true);
            startPanel.SetActive(false);
            skinsPanel.Hide();
            pausedPanel.SetActive(false);
            DCDrawingBoardController.Instance.gameObject.SetActive(false);
            progressBar.SetActive(false);
            DCLineWorker.Instance.enabled = false;

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
        DCDrawingBoardController.Instance.gameObject.SetActive(false);
        progressBar.SetActive(false);
        DCLineWorker.Instance.enabled = false;
    }

    private void AudioCheck()
    {
        if (PlayerPrefs.GetInt("Audio", 0) == 0)
        {
            muteImage.SetActive(false);
            DCAudioManager.Instance.soundIsOn = true;
            DCAudioManager.Instance.PlayBackgroundMusic();
        }
        else
        {
            muteImage.SetActive(true);
            DCAudioManager.Instance.soundIsOn = false;
            DCAudioManager.Instance.StopBackgroundMusic();
        }
    }

    public void StartButton()
    {
        pauseButton.SetActive(true);
        startPanel.SetActive(false);
        DCAudioManager.Instance.ButtonClickSound();
        DCDrawingBoardController.Instance.gameObject.SetActive(true);
        progressBar.SetActive(true);
        DCLineWorker.Instance.enabled = true;
        DCLineWorker.Instance.GetMainCameraTransform().GetChild(0).gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        DCAudioManager.Instance.ButtonClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SkinsBackButton()
    {
        StartPanelActivation();
        DCAudioManager.Instance.ButtonClickSound();
    }

    public void AudioButton()
    {
        DCAudioManager.Instance.ButtonClickSound();
        if (PlayerPrefs.GetInt("Audio", 0) == 0)
            PlayerPrefs.SetInt("Audio", 1);
        else
            PlayerPrefs.SetInt("Audio", 0);
        AudioCheck();
    }

    public void SkinsButton()
    {
        SkinsPanelActivation();
        DCAudioManager.Instance.ButtonClickSound();
    }

    public void PauseButton()
    {
        pauseButton.SetActive(false);
        PausedPanelActivation();
        DCAudioManager.Instance.StopBackgroundMusic();
        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        DCAudioManager.Instance.PlayBackgroundMusic();
        pauseButton.SetActive(true);
        pausedPanel.SetActive(false);
        DCDrawingBoardController.Instance.gameObject.SetActive(true);
        progressBar.SetActive(true);
        DCLineWorker.Instance.enabled = true;
    }

    public void HomeButton()
    {
        ResumeButton();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevelButton()
    {
        DCAudioManager.Instance.ButtonClickSound();
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.sceneCountInBuildSettings <= PlayerPrefs.GetInt("CurrentVisibleLevel", 0)) 
            nextLevelIndex = Random.Range(0, 29);
        
        PlayerPrefs.SetInt("Level", nextLevelIndex);
        SceneManager.LoadScene(nextLevelIndex);
    }
}
