using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _playButton;
    private void Start()
    {
        _restartButton.onClick.AddListener(DCGameManager.Instance.RestartButton);
        _playButton.onClick.AddListener(DCGameManager.Instance.ResumeButton);
    }
}
