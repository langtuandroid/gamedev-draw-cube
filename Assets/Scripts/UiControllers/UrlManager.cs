using System;
using System.Collections;
using System.Threading.Tasks;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class UrlManager : MonoBehaviour
    {
        [SerializeField] private string _urlForPrivacyPolicy;
        [SerializeField] private string _urlForTermsOfUse;
        [SerializeField] private Button _termsButton;
        [SerializeField] private Button _privacyButton;

        private bool _externalOpeningUrlDelayFlag = false;

        private void Awake()
        {
            if (_termsButton != null)
                _termsButton.onClick.AddListener(() => OpenUrl(_urlForTermsOfUse));

            if (_privacyButton != null)
                _privacyButton.onClick.AddListener(() => OpenUrl(_urlForPrivacyPolicy));
        }

        private void OnDestroy()
        {
            if (_termsButton != null)
                _termsButton.onClick.RemoveListener(() => OpenUrl(_urlForTermsOfUse));

            if (_privacyButton != null)
                _privacyButton.onClick.RemoveListener(() => OpenUrl(_urlForPrivacyPolicy));
        }

        private async void OpenUrl(string url)
        {
            DCAudioManager.Instance.ClickSound();
            if (_externalOpeningUrlDelayFlag) return;
            _externalOpeningUrlDelayFlag = true;
            await OpenURLAsync(url);
            StartCoroutine(WaitForSeconds(1, () => _externalOpeningUrlDelayFlag = false));
        }
    
        private async Task OpenURLAsync(string url)
        {
            await Task.Delay(1);
            try
            {
                Application.OpenURL(url);
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка при открытии ссылки {url}: {e.Message}");
            }
        }

        private IEnumerator WaitForSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        } 
    }
}