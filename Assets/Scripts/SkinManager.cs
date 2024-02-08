using UnityEngine;

public static class SkinManager
{
    public const string key_Skin = "Skin";
    public const string key_Unlocked = "Unlocked";

    public static bool SkinSelect(int index)
    {
        if (PlayerPrefs.GetInt(key_Skin + index + key_Unlocked, 0) == 0)
        {
            if (PlayerPrefs.GetInt("Token", 0) < StaticVariables.RequiredTokensToUnlockSkin[index])
            {
                AudioManager.Instance.NotEnoughTokenSound();
                return false;
            }
            else
            {
                PlayerPrefs.SetInt(key_Skin + index + key_Unlocked, 1);
                ScoreManager.RemoveToken(StaticVariables.RequiredTokensToUnlockSkin[index]);
                PlayerPrefs.SetInt(key_Skin, index);
                AudioManager.Instance.SkinSwitchSound();
            }
        }
        else
        {
            PlayerPrefs.SetInt(key_Skin, index);
            AudioManager.Instance.SkinSwitchSound();
        }
        return true;
    }
}
