using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //----------------------------------------------
    //Thank you for purchasing the asset! If you have any questions/suggestions, don't hesitate to contact me!
    //E-mail: ragendom@gmail.com
    //Please let me know your impressions about the asset by leaving a review, I will appreciate it.
    //----------------------------------------------

    public GameObject startPanel, clearedPanel, skinsPanel, pausedPanel, pauseButton, muteImage, progressBar;
    public TextMeshProUGUI[] levelClearedTexts;

    [HideInInspector]
    public bool gameIsOver = false;

    public static GameManager Instance;
    private GameObject confetti;

    private void Awake()
    {
        Instance = this;
        confetti = GameObject.FindGameObjectWithTag("Confetti");
        confetti.SetActive(false);
    }

    void Start()
    {
        if ((Time.time == Time.timeSinceLevelLoad))
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level", 0));

        StartPanelActivation();
        AudioCheck();
    }

    public void Initialize()
    {
        pauseButton.SetActive(false);
    }

    public void StartPanelActivation()
    {
        Initialize();
        startPanel.SetActive(true);
        skinsPanel.SetActive(false);
        pausedPanel.SetActive(false);
        clearedPanel.SetActive(false);
        DrawingBoardController.Instance.gameObject.SetActive(false);
        progressBar.SetActive(false);
        Line.Instance.enabled = false;
    }

    private bool cleared = false;

    public void ClearedPanelActivation()
    {
        if (!cleared)
        {
            cleared = true;
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
                levelClearedTexts[i].text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex + 1).ToString() + "\nCLEARED";

            confetti.SetActive(true);
        }
    }

    public void SkinsPanelActivation()
    {
        startPanel.SetActive(false);
        skinsPanel.SetActive(true);
        pausedPanel.SetActive(false);
    }

    public void PausedPanelActivation()
    {
        startPanel.SetActive(false);
        skinsPanel.SetActive(false);
        pausedPanel.SetActive(true);
        DrawingBoardController.Instance.gameObject.SetActive(false);
        progressBar.SetActive(false);
        Line.Instance.enabled = false;
    }

    public void AudioCheck()
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
        Line.Instance.mainCameraTransform.GetChild(0).gameObject.SetActive(true);
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
