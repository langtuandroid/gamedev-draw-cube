using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinsButtonController : MonoBehaviour
{
    private Button _thisButton;
    private void Start()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(DCGameManager.Instance.SkinsButton);
    }
}
