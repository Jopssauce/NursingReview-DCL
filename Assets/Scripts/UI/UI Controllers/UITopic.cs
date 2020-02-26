using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITopic : UIController
{
    [Header("Trasnforms")]
    public RectTransform button1;
    public RectTransform button2;
    public RectTransform image;
    public RectTransform leftBar;

    [Header("Buttons")]
    public Button systems;
    public Button patients;

    public void TransitionScreen(string screen)
    {
        Sequence sequence = DOTween.Sequence();
        Jump(button1, 50);
        Jump(button2, 50);
        sequence.Append(Jump(image, 50));
        sequence.Append(leftBar.DOAnchorPos(new Vector2(-863.3f, -39f), 1));

        if(screen == "Patient Units UI") sequence.onComplete += LoadPatient;
        else
        {
            sequence.onComplete += LoadSystem;
        }

    }

    public Tween Jump(RectTransform rectTransform, float offset)
    {
        Vector2 position = rectTransform.anchoredPosition;
        rectTransform.DOPunchAnchorPos(new Vector2(0, offset), 0.2f, 1, 1);
        return rectTransform.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 0.3f).SetOptions(true);
    }

    void LoadSystem()
    {
        LoadActiveScene("Systems UI");
    }

    void LoadPatient()
    {
        LoadActiveScene("Patient Units UI");
    }

}
