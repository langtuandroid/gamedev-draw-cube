using Scripts.Gameplay.Managers;
using UnityEngine;
using UnityEngine.UI;

public class AddMoney : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    void Start()
    {
        _button.onClick.AddListener(() =>
        {
            PlayerPrefsManager.AddInGameCurrency(10);
        });
    }
}
