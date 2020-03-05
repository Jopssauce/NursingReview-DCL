using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UITitleScreen : UIController
{
    public CanvasGroup canvasGroup;
    public override void LoadActiveScene(string sceneToLoad)
    {
        Tween tween = canvasGroup.DOFade(0, 0.3f);
        tween.onComplete += delegate ()
        {
            base.LoadActiveScene(sceneToLoad);
        };
    }
}
