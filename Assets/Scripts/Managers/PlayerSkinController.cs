using Gameplay;
using ScriptableObjects;
using Scripts.Gameplay.Managers;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class PlayerSkinController : MonoBehaviour
    {
        [SerializeField] private PlayerSkinsScriptableObject _playerSkinsScriptableObject;
        [Inject] private LevelEnterAdsManager _levelEnterAdsManager;
        private DCVisiblePlayerController _currentVisiblePlayer;
        private MeshRenderer _playerRenderer;
        private MeshRenderer _cylinderRenderer;
        private MeshFilter _playerMeshFilter;

        private void Start()
        {
            _levelEnterAdsManager.LevelLoaded();
            _currentVisiblePlayer = DCVisiblePlayerController.Instance;
            _playerMeshFilter = _currentVisiblePlayer.GetComponent<MeshFilter>();
            _playerRenderer = _currentVisiblePlayer.GetComponent<MeshRenderer>();
            _cylinderRenderer = _currentVisiblePlayer.transform.GetChild(0).GetComponent<MeshRenderer>();
            
            _playerRenderer.material = _cylinderRenderer.material = _playerSkinsScriptableObject.GetMaterialByIndex(PlayerPrefsManager.GetCurrentEquippedSkin());
            _playerMeshFilter.mesh = _playerSkinsScriptableObject.GetMeshByIndex(PlayerPrefsManager.GetCurrentEquippedSkin());
        }
    }
}