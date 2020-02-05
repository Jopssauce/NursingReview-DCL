using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonMainTopic : UIButton, IPointerEnterHandler
{
    public Sprite ReferenceImage;

    UIMainTopic uiMainTopic;

    public override void Start()
    {
        base.Start();
        uiMainTopic = GetComponentInParent<UIMainTopic>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiMainTopic.HoverImage.sprite = ReferenceImage;
    }

    public void ReplaceActiveScene(string scene)
    {
        PersistentSceneManager.ReplaceActiveScene(scene);
    }
}
