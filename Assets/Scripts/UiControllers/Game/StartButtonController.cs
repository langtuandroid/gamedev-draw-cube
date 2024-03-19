using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UiControllers.Game
{
    public class StartButtonController : MonoBehaviour
    {
        public static StartButtonController Instance;
        private Button _thisButton;

        private void Awake() => Instance = this;

        private void Start()
        {
            _thisButton = GetComponent<Button>();
            _thisButton.onClick.AddListener(DCGameManager.Instance.OnStartButton);
        }
    }
}