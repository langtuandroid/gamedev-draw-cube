using System;
using TMPro;
using UnityEngine;

namespace UiControllers
{
    public class ScoreController : MonoBehaviour
    {
        public static ScoreController Instance;
        [SerializeField] private TextMeshProUGUI _tokenText;
        private Animation _tokenTextAnimation;

        private void Awake() => Instance = this;

        private void Start()
        {
            _tokenTextAnimation = _tokenText.gameObject.GetComponent<Animation>();
        }

        public void SetTokens(int tokens) => _tokenText.text = tokens.ToString();
        
        public void PlayAnimation() => _tokenTextAnimation.Play();
    }
}