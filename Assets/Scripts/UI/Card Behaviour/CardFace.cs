using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CardFace : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    public DataCard CardData;
    public Image FrontFace;
    public Image BackFace;
    public ScrollRect scrollRect;

    GameObject currentFace;
    GameObject lastFace;

    public bool isBack {  get; private set; }
    bool isDrag;
    bool isZoomed;

    Vector3 initialScale;

    //properties
    public float referenceAngle = 12;
    public float MaxTimeToClick = 0.60f;
    public float MinTimeToClick = 0.05f;
    public bool IsDebug = false;
    public bool isDoubleClicked;

    //private variables to keep track
    private float lastClickTime;
    public float doubleClickThreshold = 0.3f; // Time in seconds
    Tween switchTween;


    void OnEnable()
    {
        FrontFace.sprite = CardData.FrontFace;
        BackFace.sprite = CardData.BackFace;
        if(CardData.BackFace == null)
        {
            BackFace.GetComponent<Image>().enabled = false;
        }
        else
        {
            BackFace.GetComponent<Image>().enabled = true;
        }    
        FrontFace.transform.localScale = Vector3.one;
        currentFace = FrontFace.gameObject;
        initialScale = currentFace.transform.localScale;
        //FrontFace.transform.position += new Vector3(0, 200, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (isDrag == false)
        {         
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (timeSinceLastClick <= doubleClickThreshold)
                {
                    if (isZoomed)
                    {
                        isZoomed = false;
                        ZoomFace(1f);
                    }
                    else
                    {
                        isZoomed = true;
                        ZoomFace(2f);
                    }
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                SwitchFaces();
            }
        }
    
        lastClickTime = Time.time;
    }

    private void SwitchFaces()
    {
        if (switchTween != null && switchTween.IsActive()) return;

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
        currentFace = faceA.gameObject;
        lastFace = faceB.gameObject;
        faceA.gameObject.SetActive(true);

        float duration = 1.3f;
        switchTween = faceA.transform.DORotate(new Vector3(0, 180 - referenceAngle, 0), duration);
        faceB.transform.DORotate(new Vector3(0,  -referenceAngle, 0), duration);
        faceA.transform.localScale = faceB.transform.localScale;
        //scrollRect.content = faceA.GetComponent<RectTransform>();

        //if (tween.position % 2 == 0)
        //   faceB.gameObject.SetActive(false);

        isBack = !isBack;
    }

    public void ZoomFace(float zoom, float dur = 0.3f)
    {
        FrontFace.transform.DOScale(zoom, dur);
        BackFace.transform.DOScale(zoom, dur);
    }

    public void ResetCard()
    {
        FrontFace.transform.localEulerAngles = new Vector3(0, -referenceAngle, 0);
        BackFace.transform.localEulerAngles = new Vector3(0, 180 - referenceAngle, 0);
        FrontFace.transform.localScale = new Vector3(1, 1, 1);
        BackFace.transform.localScale = new Vector3(1, 1, 1);
        isBack = false;
        isZoomed = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }
}
