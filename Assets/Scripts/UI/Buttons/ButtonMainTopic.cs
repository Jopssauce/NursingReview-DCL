using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class ButtonMainTopic : UIButton, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public DataTopic TopicData;
    public TextMeshProUGUI TextMeshProUGUI;

    public Color Default;
    public Color Highlighted;

    UIMainTopic uiMainTopic;
    Image image;

    public override void Start()
    {
        base.Start();
        uiMainTopic = GetComponentInParent<UIMainTopic>();
        TextMeshProUGUI.text = TopicData.TopicName;

        button.onClick.AddListener(OnClick);
    }

    void OnEnable()
    {
        image = GetComponent<Image>();
    }

    void OnClick()
    {
        uiMainTopic.SetSelectedTopicText(TopicData);
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

    public void OnSelect(BaseEventData eventData)
    {
        image.fillAmount = 0;
        image.DOFillAmount(1, 1f);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        image.fillAmount = 1;
    }
}
