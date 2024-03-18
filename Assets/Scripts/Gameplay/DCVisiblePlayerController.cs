using UnityEngine;

namespace Gameplay
{
    public class DCVisiblePlayerController : MonoBehaviour
    {
        public static DCVisiblePlayerController Instance;
    
        private void Awake() => Instance = this;
    }
}