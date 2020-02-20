using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaybackMask : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animator;


    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetTrigger("Up");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger("Down");
    }
}
