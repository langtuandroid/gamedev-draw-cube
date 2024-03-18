using UiControllers.Game;
using UnityEngine;

namespace Managers
{
    public static class DCScoreManager
    {
        public static void TokenAdd(int countOfToken = 1)
        {
            if (!DCGameManager.Instance.IsGameOver)
            {
                PlayerPrefs.SetInt("Token", PlayerPrefs.GetInt("Token", 0) + countOfToken);
                SetTokensToSC();
                DCScoreController.Instance.PlayAnimation();
                DCAudioManager.Instance.TokenCollectSound();
            }
        }

        public static void TokenRemove(int decreaseValue)
        {
            PlayerPrefs.SetInt("Token", PlayerPrefs.GetInt("Token", 0) - decreaseValue);
            SetTokensToSC();
        }

        public static void SetTokensToSC() => DCScoreController.Instance.SetTokens(PlayerPrefs.GetInt("Token", 0));
    }
}
