using UnityEngine;
using UiControllers;

public static class DCScoreManager
{
    public static void AddToken(int countOfToken = 1)
    {
        if (!DCGameManager.Instance.IsGameOver)
        {
            PlayerPrefs.SetInt("Token", PlayerPrefs.GetInt("Token", 0) + countOfToken);
            SetTokens();
            ScoreController.Instance.PlayAnimation();
            DCAudioManager.Instance.TokenSound();
        }
    }

    public static void RemoveToken(int decreaseValue)
    {
        PlayerPrefs.SetInt("Token", PlayerPrefs.GetInt("Token", 0) - decreaseValue);
        SetTokens();
    }

    public static void SetTokens() => ScoreController.Instance.SetTokens(PlayerPrefs.GetInt("Token", 0));
}
