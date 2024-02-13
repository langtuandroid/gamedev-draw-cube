using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCAudioManager : MonoBehaviour {

    //------------------------CREDITS----------------------------
    //Background music by Eric Matyas: http://www.soundimage.org
    //Sound effects: https://www.noiseforfun.com
    //-----------------------------------------------------------

    [SerializeField]
    private AudioSource backgroundMusic, tokenSound, levelClearedSound, buttonClickSound, skinSwitchSound, notEnoughTokenSound;

    [HideInInspector]
    public bool soundIsOn = true;       //DCGameManager script might modify this value

    public static DCAudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    //Functions are called by other scripts when it is necessary

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }

    public void PlayBackgroundMusic()
    {
        if (soundIsOn)
            backgroundMusic.Play();
    }

    public void TokenSound()
    {
        if(soundIsOn)
            tokenSound.Play();
    }

    public void LevelClearedSound()
    {
        if (soundIsOn)
            levelClearedSound.Play();
    }

    public void ButtonClickSound()
    {
        if (soundIsOn)
            buttonClickSound.Play();
    }

    public void NotEnoughTokenSound()
    {
        if (soundIsOn)
            notEnoughTokenSound.Play();
    }

    public void SkinSwitchSound()
    {
        if (soundIsOn)
            skinSwitchSound.Play();
    }
}
