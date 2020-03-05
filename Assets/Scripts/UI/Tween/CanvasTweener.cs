﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasTweener : MonoBehaviour
{
    public Canvas Canvas;
    UIMainTopic uiMainTopic;
    Sequence cardSequence;

    private void Awake()
    {
        uiMainTopic = Canvas.GetComponent<UIMainTopic>();
        uiMainTopic.onInstancedSubTopics += PlayCardSequence;
        if (uiMainTopic.PlayStartAnimation) uiMainTopic.onInitialize += RightSequence;

        uiMainTopic.onLoadVideoPlayer += delegate() { HideUI(); };
        uiMainTopic.onUnloadVideoPlayer += delegate () { UnHideUI(); };

    }

    public void RightSequence()
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < uiMainTopic.TopicButtons.Count; i++)
        {
            sequence.Append(Jump(uiMainTopic.TopicButtons[i], -20, 0.08f));
        }
        sequence.onComplete += LeftSequence;
        sequence.onComplete += delegate ()
        {
            uiMainTopic.UITopicButtonScrollView.ScrollRect.content.GetComponent<VerticalLayoutGroup>().enabled = true;
        };
    }

    public void LeftSequence()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Jump(uiMainTopic.UIContentGroup.Header.GetComponent<RectTransform>(), -50));
        sequence.Append(Jump(uiMainTopic.UIContentGroup.GetComponent<RectTransform>(), -50));
        sequence.onComplete += delegate ()
        {
            uiMainTopic.RaycastBlocker.SetActive(false);
        };
    }

    public void CardsSequence()
    {
        if (cardSequence != null) cardSequence.Kill();
        cardSequence = DOTween.Sequence();

        for (int i = 0; i < uiMainTopic.GridCards.Count; i++)
        {
            cardSequence.Append(uiMainTopic.GridCards[i].GetComponent<Image>().DOFade(1, 0.15f));
        }
    }

    public void PlayCardSequence()
    {
        for (int i = 0; i < uiMainTopic.GridCards.Count; i++)
        {
            uiMainTopic.GridCards[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        CardsSequence();
    }

    public Tween Jump(RectTransform rectTransform, float offset, float time = 0.1f)
    {
        Vector2 position = rectTransform.anchoredPosition;

        Tween tween = rectTransform.DOPunchAnchorPos(new Vector2(0, offset), time, 1, 1);
        tween.onPlay += delegate ()
        {
            rectTransform.gameObject.SetActive(true);
        };
        return tween;
    }

    public void HideUI(float time = 0.3f)
    {
        uiMainTopic.UITopicButtonScrollView.ParentCanvasGroup.DOFade(0, time);
        uiMainTopic.UIContentGroup.ParentCanvasGroup.DOFade(0, time);
        uiMainTopic.UINavigationGroup.ParentCanvasGroup.DOFade(0, time);
        uiMainTopic.UICardsViewerGroup.ParentCanvasGroup.DOFade(0, time);
    }

    public void UnHideUI(float time = 0.3f)
    {
        uiMainTopic.UITopicButtonScrollView.ParentCanvasGroup.DOFade(1, time);
        uiMainTopic.UIContentGroup.ParentCanvasGroup.DOFade(1, time);
        uiMainTopic.UINavigationGroup.ParentCanvasGroup.DOFade(1, time);
        uiMainTopic.UICardsViewerGroup.ParentCanvasGroup.DOFade(1, time);
    }
}
