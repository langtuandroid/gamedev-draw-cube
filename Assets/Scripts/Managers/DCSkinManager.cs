using Gameplay;
using UnityEngine;

namespace Managers
{
    public static class DCSkinManager
    {
        public const string key_Skin = "Skin";
        public const string key_Unlocked = "Unlocked";

        public static bool SelectSkin(int index)
        {
            if (PlayerPrefs.GetInt(key_Skin + index + key_Unlocked, 0) == 0)
            {
                if (PlayerPrefs.GetInt("Token", 0) < DCStaticVariables.TokensToUnlockSkin[index])
                {
                    DCAudioManager.Instance.InsufficientTokenSound();
                    return false;
                }

                PlayerPrefs.SetInt(key_Skin + index + key_Unlocked, 1);
                DCScoreManager.TokenRemove(DCStaticVariables.TokensToUnlockSkin[index]);
                PlayerPrefs.SetInt(key_Skin, index);
                DCAudioManager.Instance.SwitchSkinSound();
            }
            else
            {
                PlayerPrefs.SetInt(key_Skin, index);
                DCAudioManager.Instance.SwitchSkinSound();
            }
            return true;
        }
    }
}
