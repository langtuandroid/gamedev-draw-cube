using UnityEngine;
using UnityEngine.EventSystems;

public class DCDrawingBoardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static DCDrawingBoardController Instance;
    [HideInInspector] public bool IsHovering;

    void Awake() => Instance = this;

    public void OnPointerEnter(PointerEventData eventData) => IsHovering = true;

    public void OnPointerExit(PointerEventData eventData) => IsHovering = false;
}
