using UnityEngine;

public class VisiblePlayerController : MonoBehaviour
{
    public static VisiblePlayerController Instance;
    
    private void Awake() => Instance = this;
}