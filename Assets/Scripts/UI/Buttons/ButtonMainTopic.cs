using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ButtonMainTopic : UIButton, IPointerEnterHandler, IPointerExitHandler
{
    public DataTopic TopicData;
    public TextMeshProUGUI TextMeshProUGUI;

    public Color Default;
    public Color Highlighted;

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

    public void ReplaceActiveScene(string scene)
    {
        //PersistentSceneManager.ReplaceActiveScene(scene);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TextMeshProUGUI.color = Highlighted;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TextMeshProUGUI.color = Default;
    }
}
