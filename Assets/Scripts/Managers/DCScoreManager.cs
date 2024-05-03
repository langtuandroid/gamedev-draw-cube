using Scripts.Gameplay.Managers;
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
                PlayerPrefsManager.AddInGameCurrency(countOfToken);
                SetTokensToSC();
                DCScoreController.Instance.PlayAnimation();
                DCAudioManager.Instance.TokenCollectSound();
            }
        }

        public static void TokenRemove(int decreaseValue)
        {
            PlayerPrefsManager.AddInGameCurrency(-decreaseValue);
            SetTokensToSC();
        }

        public static void SetTokensToSC() => DCScoreController.Instance.SetTokens(PlayerPrefsManager.GetInGameCurrency());
    }
}
