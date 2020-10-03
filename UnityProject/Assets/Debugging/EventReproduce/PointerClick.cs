using UnityEngine;
using UnityEngine.EventSystems;

public class PointerClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
    }
}