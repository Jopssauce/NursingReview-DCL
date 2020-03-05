using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCard : UIButton
{
    public DataSubTopic SubTopicData;
    public UISubTopicMask UISubTopicMask;
    UIMainTopic uiMainTopic;
    public override void Start()
    {
        base.Start();
    }
    //OnEnable because object starts off as enabled
    private void OnEnable()
    {
        Initialize();
        uiMainTopic = GetComponentInParent<UIMainTopic>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        uiMainTopic.OpenHorizontalCardScroller(SubTopicData);
    }
}
