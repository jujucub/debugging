using UnityEngine;
using UnityEngine.EventSystems;

public class PointerDown : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
}