using UnityEngine;

public static class DCSkinManager
{
    public const string key_Skin = "Skin";
    public const string key_Unlocked = "Unlocked";

    public static bool SkinSelect(int index)
    {
        if (PlayerPrefs.GetInt(key_Skin + index + key_Unlocked, 0) == 0)
        {
            if (PlayerPrefs.GetInt("Token", 0) < DCStaticVariables.RequiredTokensToUnlockSkin[index])
            {
                DCAudioManager.Instance.NotEnoughTokenSound();
                return false;
            }
            else
            {
                PlayerPrefs.SetInt(key_Skin + index + key_Unlocked, 1);
                DCScoreManager.RemoveToken(DCStaticVariables.RequiredTokensToUnlockSkin[index]);
                PlayerPrefs.SetInt(key_Skin, index);
                DCAudioManager.Instance.SkinSwitchSound();
            }
        }
        else
        {
            PlayerPrefs.SetInt(key_Skin, index);
            DCAudioManager.Instance.SkinSwitchSound();
        }
        return true;
    }
}
