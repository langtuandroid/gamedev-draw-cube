using UnityEngine;

namespace Managers
{
    public class DCAudioManager : MonoBehaviour {
    
        public static DCAudioManager Instance;
        [SerializeField] private AudioSource backgroundMusic, tokenSound, levelClearedSound, buttonClickSound, skinSwitchSound, notEnoughTokenSound;

        private bool _soundIsOn = true;

        public bool SoundIsOn
        {
            get => _soundIsOn;
            set => _soundIsOn = value;
        }
        
        private void Awake() => Instance = this;

        public void StopBackgroundMusic() => backgroundMusic.Stop();
        
        public void PlayBackgroundMusic()
        {
            if (_soundIsOn) backgroundMusic.Play();
        }

        public void TokenCollectSound()
        {
            if(_soundIsOn) tokenSound.Play();
        }

        public void LevelWinSound()
        {
            if (_soundIsOn) levelClearedSound.Play();
        }

        public void ClickSound()
        {
            if (_soundIsOn) buttonClickSound.Play();
        }

        public void InsufficientTokenSound()
        {
            if (_soundIsOn) notEnoughTokenSound.Play();
        }

        public void SwitchSkinSound()
        {
            if (_soundIsOn) skinSwitchSound.Play();
        }
    }
}
