using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    public Image progressBar;
    public TextMeshProUGUI tempLevelText, nextLevelText;

    public static ProgressBar Instance;
    public static bool canFill = false;
    private Transform player, levelEnd;

    private float playerStartPosX, levelEndStartPosX;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //Initializations
        levelEnd = GameObject.FindGameObjectWithTag("LevelEnd").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tempLevelText.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        nextLevelText.text = (SceneManager.GetActiveScene().buildIndex + 2).ToString();
        progressBar.fillAmount = 0f;

        CanFill();
    }

    public void CanFill()
    {
        canFill = true;
        levelEndStartPosX = levelEnd.transform.position.x;
        playerStartPosX = player.transform.position.x;
    }

    void Update()
    {
        //Tracks the distance between the player and the end of the level
        if (canFill)
            progressBar.fillAmount = ((player.transform.position.x - playerStartPosX) / (levelEnd.transform.position.x - playerStartPosX));
    }
}
