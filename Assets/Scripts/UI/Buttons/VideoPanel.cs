using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VideoPanel : MonoBehaviour, IPointerClickHandler
{
    public string VideoUI;
    public GameObject LoadingScreen;

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
        LoadingScreen.SetActive(true);
    }

    public void UnZoom()
    {
        animator.SetTrigger("UnZoom");
        LoadingScreen.SetActive(false);
    }
    //Makes use of animation event in video panel
    public void LoadVideoPlayer()
    {
        uiMainTopic.LoadVideoPlayer();
    }
}
