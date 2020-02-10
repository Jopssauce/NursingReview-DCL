using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardFace : MonoBehaviour, IPointerClickHandler
{
    public GameObject CardBackFace;
    public void OnPointerClick(PointerEventData eventData)
    {
        CardBackFace.SetActive(!CardBackFace.activeSelf);
    }
}
