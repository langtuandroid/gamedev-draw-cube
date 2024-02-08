using UnityEngine;
using UiControllers;

public static class ScoreManager
{
    public static void AddToken(int countOfToken = 1)
    {
        if (!GameManager.Instance.IsGameOver)
        {
            PlayerPrefs.SetInt("Token", PlayerPrefs.GetInt("Token", 0) + countOfToken);
            SetTokens();
            ScoreController.Instance.PlayAnimation();
            AudioManager.Instance.TokenSound();
        }
    }

    public static void RemoveToken(int decreaseValue)
    {
        PlayerPrefs.SetInt("Token", PlayerPrefs.GetInt("Token", 0) - decreaseValue);
        SetTokens();
    }

    private static void SetTokens() => ScoreController.Instance.SetTokens(PlayerPrefs.GetInt("Token", 0));
}
