using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBackground : MonoBehaviour, IPointerClickHandler
{
    public GameObject CardPanel;
    public void OnPointerClick(PointerEventData eventData)
    {
        CardPanel.SetActive(false);
    }
}
