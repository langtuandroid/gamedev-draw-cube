using UnityEngine;
using UnityEngine.EventSystems;

namespace UiControllers.Game
{
    public class DCDrawingBoardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static DCDrawingBoardController Instance;
        private bool _isHovering;
        
        public bool IsHovering => _isHovering;

        void Awake() => Instance = this;

        public void OnPointerEnter(PointerEventData eventData) => _isHovering = true;

        public void OnPointerExit(PointerEventData eventData) => _isHovering = false;
    }
}
