using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingBoardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static DrawingBoardController Instance;
    [HideInInspector] public bool IsHovering;

    void Awake() => Instance = this;

    public void OnPointerEnter(PointerEventData eventData) => IsHovering = true;

    public void OnPointerExit(PointerEventData eventData) => IsHovering = false;
}
