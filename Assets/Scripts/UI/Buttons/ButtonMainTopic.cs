using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ButtonMainTopic : UIButton, IPointerEnterHandler
{
    public DataTopic TopicData;
    public TextMeshProUGUI TextMeshProUGUI;
    UIMainTopic uiMainTopic;

    public override void Start()
    {
        base.Start();
        uiMainTopic = GetComponentInParent<UIMainTopic>();
        TextMeshProUGUI.text = TopicData.TopicName;

        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        uiMainTopic.SetSelectedTopicText(TopicData.TopicName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //uiMainTopic.HoverImage.sprite = ReferenceImage;
    }

    public void ReplaceActiveScene(string scene)
    {
        //PersistentSceneManager.ReplaceActiveScene(scene);
    }
}
