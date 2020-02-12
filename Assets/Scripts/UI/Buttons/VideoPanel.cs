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

    private void Start()
    {
        uiMainTopic = GetComponentInParent<UIMainTopic>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        uiMainTopic.LoadVideoPlayer();
    }
}
