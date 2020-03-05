using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class ButtonMainTopic : UIButton, IPointerEnterHandler, IPointerExitHandler
{
    public DataTopic TopicData;
    public TextMeshProUGUI TextMeshProUGUI;

    public Color Default;
    public Color Highlighted;

    public Sprite SpriteDefault;
    public Sprite SpriteHighlighted;

    UIMainTopic uiMainTopic;
    Image image;

    bool isSelected = false;

    public override void Start()
    {
        base.Start();
        uiMainTopic = GetComponentInParent<UIMainTopic>();
        button.onClick.AddListener(OnClick);
    }

    void OnEnable()
    {
        image = GetComponent<Image>();
    }

    void OnClick()
    {
        uiMainTopic.SetSelectedTopicText(TopicData);

        //Selected
        if (uiMainTopic.currentTopicButton != this)
        {
            //Deselect Old Button
            if(uiMainTopic.currentTopicButton != null) uiMainTopic.currentTopicButton.DeselectAction();
            //Select this button as new
            SelectAction();
            uiMainTopic.currentTopicButton = this;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected == false) TextMeshProUGUI.color = Highlighted;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected == false)TextMeshProUGUI.color = Default;
    }

    void SelectAction()
    {
        TextMeshProUGUI.color = Highlighted;
        isSelected = true;
        image.sprite = SpriteHighlighted;
        image.fillAmount = 0;
        image.DOFillAmount(1, 0.7f);
    }

    void DeselectAction()
    {
        TextMeshProUGUI.color = Default;
        isSelected = false;
        image.sprite = SpriteDefault;
        image.fillAmount = 1;
    }
}
