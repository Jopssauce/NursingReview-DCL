using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIOnClickPanel : MonoBehaviour, IPointerClickHandler
{
    public Image Panel;

    public UnityEvent onClick;

    private void OnEnable()
    {
        Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
    }
}
