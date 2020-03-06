using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CardFace : MonoBehaviour, IPointerClickHandler
{
    public DataCard CardData;
    public Image FrontFace;
    public Image BackFace;
    public ScrollRect scrollRect;

    bool isBack = true;

    void OnEnable()
    {
        FrontFace.sprite = CardData.FrontFace;
        BackFace.sprite = CardData.BackFace;
        FrontFace.transform.localScale = Vector3.one;
        //FrontFace.transform.position += new Vector3(0, 200, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SwitchFaces();
    }

    private void SwitchFaces()
    {
        if (isBack)
        {
            SwitchFace(BackFace.gameObject, FrontFace.gameObject);
        }
        else
        {
            SwitchFace(FrontFace.gameObject, BackFace.gameObject);
        }
    }
    
    //Switches faceA with faceB
    private void SwitchFace(GameObject faceA, GameObject faceB)
    {
        faceA.gameObject.SetActive(true);

        Tween tween = faceA.transform.DORotate(new Vector3(0, 0, 0), 0.3f);
        faceB.transform.DORotate(new Vector3(0, 180, 0), 0.3f);
        faceA.transform.localScale = faceB.transform.localScale;

        scrollRect.content = faceA.GetComponent<RectTransform>();

        if (tween.position % 2 == 0)
            faceB.gameObject.SetActive(false);

        isBack = !isBack;
    }
}
