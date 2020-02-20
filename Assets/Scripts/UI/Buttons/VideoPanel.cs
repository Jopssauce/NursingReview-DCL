using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VideoPanel : MonoBehaviour, IPointerClickHandler
{
    public string VideoUI;

    UIMainTopic uiMainTopic;
    Animator animator;

    private void Start()
    {
        uiMainTopic = GetComponentInParent<UIMainTopic>();
        animator = GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        animator.SetTrigger("Zoom");
    }

    public void UnZoom()
    {
        animator.SetTrigger("UnZoom");
    }
    //Makes use of animation event in video panel
    public void LoadVideoPlayer()
    {
        uiMainTopic.LoadVideoPlayer();
    }
}
