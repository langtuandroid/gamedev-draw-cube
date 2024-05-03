using System;
using UnityEngine;

namespace Scripts.Gameplay.Managers
{
    public static class PlayerPrefsManager
    {
        public static Action ON_COINS_CHANGED;

        private const string PAID_CURRENCY_KEY = "PAID_CURRENCY";
        private const string IN_GAME_CURRENCY_KEY = "IN_GAME_CURRENCY";
        private const string SKIN_UNLOCKED_KEY_ = "SKIN_UNLOCKED_";
        private const string EQUIPPED_SKIN = "EQUIPPED_SKIN_";
        private const string SOUND_KEY = "SOUND_KEY";
        private const string LEVEL_KEY = "LEVEL";
        private const string AMOUNT_OF_TIMES_SKIN_WATCHED_FOR_ADS = "SKIN_WATCHED_FOR_ADS_";
        
        public static int GetSoundState()
        { return PlayerPrefs.GetInt(SOUND_KEY, 1); }

        public static void SetSoundState(int state)
        { PlayerPrefs.SetInt(SOUND_KEY, state); }
        
        public static int GetPaidCurrency() => PlayerPrefs.GetInt(PAID_CURRENCY_KEY, 0);

        public static void AddPaidCurrency(int currencyAmount)
        {
            PlayerPrefs.SetInt(PAID_CURRENCY_KEY, GetPaidCurrency() + currencyAmount);
            ON_COINS_CHANGED?.Invoke();
        }
        
        public static int GetInGameCurrency() => PlayerPrefs.GetInt(IN_GAME_CURRENCY_KEY, 0);

        public static void AddInGameCurrency(int currencyAmount)
        {
            PlayerPrefs.SetInt(IN_GAME_CURRENCY_KEY, GetInGameCurrency() + currencyAmount);
            ON_COINS_CHANGED?.Invoke();
        }

        public static void AddSkinForAdsWatched(int skinIndex) => PlayerPrefs.SetInt(AMOUNT_OF_TIMES_SKIN_WATCHED_FOR_ADS + skinIndex, GetSkinForAdsWatched(skinIndex) + 1);

        public static int GetSkinForAdsWatched(int skinIndex) => PlayerPrefs.GetInt(AMOUNT_OF_TIMES_SKIN_WATCHED_FOR_ADS + skinIndex, 0);

        public static int GetIsSkinUnlocked(int index)
        { return PlayerPrefs.GetInt(SKIN_UNLOCKED_KEY_ + index, 0); }

        public static void SetSkinUnlocked(int index)
        { PlayerPrefs.SetInt(SKIN_UNLOCKED_KEY_ + index, 1); }

        public static int GetCurrentEquippedSkin()
        { return PlayerPrefs.GetInt(EQUIPPED_SKIN, 0); }

        public static void SetCurrentEquippedSkin(int index)
        { PlayerPrefs.SetInt(EQUIPPED_SKIN, index); }
    }
}
