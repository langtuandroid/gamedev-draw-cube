using Gameplay;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class PlayerSkinController : MonoBehaviour
    {
        [SerializeField] private PlayerSkinsScriptableObject _playerSkinsScriptableObject;
        private DCVisiblePlayerController _currentVisiblePlayer;
        private MeshRenderer _playerRenderer;
        private MeshRenderer _cylinderRenderer;
        private MeshFilter _playerMeshFilter;

        private void Start()
        {
            _currentVisiblePlayer = DCVisiblePlayerController.Instance;
            _playerMeshFilter = _currentVisiblePlayer.GetComponent<MeshFilter>();
            _playerRenderer = _currentVisiblePlayer.GetComponent<MeshRenderer>();
            _cylinderRenderer = _currentVisiblePlayer.transform.GetChild(0).GetComponent<MeshRenderer>();
            
            _playerRenderer.material = _cylinderRenderer.material = _playerSkinsScriptableObject.GetMaterialByIndex(PlayerPrefs.GetInt(DCSkinManager.key_Skin, 0));
            _playerMeshFilter.mesh = _playerSkinsScriptableObject.GetMeshByIndex(PlayerPrefs.GetInt(DCSkinManager.key_Skin, 0));
        }
    }
}