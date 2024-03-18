using Managers;
using TMPro;
using UnityEngine;

namespace UiControllers.Game
{
    public class DCScoreController : MonoBehaviour
    {
        public static DCScoreController Instance;
        
        [SerializeField] private TextMeshProUGUI _tokenText;
        private Animation _tokenTextAnimation;

        private void Awake() => Instance = this;

        private void Start()
        {
            _tokenTextAnimation = _tokenText.gameObject.GetComponent<Animation>();
            DCScoreManager.SetTokensToSC();
        }

        public void SetTokens(int tokens) => _tokenText.text = tokens.ToString();
        
        public void PlayAnimation() => _tokenTextAnimation.Play();
    }
}