using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class CardFace : MonoBehaviour, /*IPointerClickHandler,*/ IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DataCard CardData;
    public Image FrontFace;
    public Image BackFace;
    public ScrollRect scrollRect;

    private RectTransform frontRectTrans;
    private RectTransform backRectTrans;
    private Canvas canvas;
    public Vector3 OrigLocalPos { get; private set; }
    private Vector3 faceAOriginalRotation;
    private Vector3 faceBOriginalRotation;
    private Vector3 originalScale;
    private Vector2 dragOffset;
    private Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

    GameObject currentFace;
    GameObject lastFace;

    public bool isBack { get; private set; }
    public bool isTweening { get; private set; }
    private bool isDrag;
    private bool isZoomed;

    Vector3 initialScale;

    //properties
    public float referenceAngle = 12;
    public float MaxTimeToClick = 0.60f;
    public float MinTimeToClick = 0.05f;
    public bool IsDebug = false;
    public bool isDoubleClicked;
    public bool isInteractable = false;

    //Events
    public Action OnBeginCenter;

    //private variables to keep track
    private float lastClickTime;
    public float doubleClickThreshold = 0.3f; // Time in seconds
    public Tween switchTween;

    void Awake()
    {
        frontRectTrans = FrontFace.GetComponent<RectTransform>();
        backRectTrans = BackFace.GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();
        OrigLocalPos = frontRectTrans.localPosition;
        faceAOriginalRotation = frontRectTrans.localEulerAngles;
        faceBOriginalRotation = backRectTrans.localEulerAngles;
        originalScale = frontRectTrans.localScale;
    }

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

    public void Update()
    {
        if (!isInteractable) return;
        if(Input.GetKeyDown(KeyCode.Z))
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

        if (Input.GetKeyDown(KeyCode.S))
        {
            SwitchFaces();
        }


        if (Input.GetKeyDown(KeyCode.C) && !isTweening)
        {
            MoveToCenter();
        }
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    float timeSinceLastClick = Time.time - lastClickTime;

    //    if (isDrag == false)
    //    {         
    //        if (eventData.button == PointerEventData.InputButton.Left)
    //        {
    //            if (timeSinceLastClick <= doubleClickThreshold)
    //            {
    //                if (isZoomed)
    //                {
    //                    isZoomed = false;
    //                    ZoomFace(1f);
    //                }
    //                else
    //                {
    //                    isZoomed = true;
    //                    ZoomFace(2f);
    //                }
    //            }
    //        }
    //        else if (eventData.button == PointerEventData.InputButton.Right)
    //        {
    //            SwitchFaces();
    //        }
    //    }
    
    //    lastClickTime = Time.time;
    //}

    private void SwitchFaces()
    {
        if (switchTween != null && switchTween.IsActive() || BackFace.sprite == null) return;

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
        isTweening = true;
        currentFace = faceA.gameObject;
        lastFace = faceB.gameObject;
        faceA.SetActive(true);

        float duration = 1.3f;
        referenceAngle = 360f - faceA.GetComponent<RectTransform>().localEulerAngles.y;
        switchTween = faceA.transform.DORotate(new Vector3(0, 180 - referenceAngle, 0), duration).OnComplete(Callback);
        faceB.transform.DORotate(new Vector3(0,  -referenceAngle, 0), duration);

        //scrollRect.content = faceA.GetComponent<RectTransform>();

        //if (tween.position % 2 == 0)
        //   faceB.gameObject.SetActive(false);

        isBack = !isBack;
    }

    public void ZoomFace(float zoom, float dur = 0.3f)
    {
        isTweening = true;
        FrontFace.transform.DOScale(zoom, dur).OnComplete(Callback);
        BackFace.transform.DOScale(zoom, dur);
    }

    public void Callback()
    {
        isTweening = false;
    }

    public void ResetCard()
    {
        if (OrigLocalPos == frontRectTrans.localPosition && 
            faceAOriginalRotation == frontRectTrans.localEulerAngles &&
            originalScale == frontRectTrans.localScale) return;

        FrontFace.transform.localEulerAngles = faceAOriginalRotation;
        BackFace.transform.localEulerAngles = faceBOriginalRotation;
        FrontFace.transform.localScale = new Vector3(1, 1, 1);
        BackFace.transform.localScale = new Vector3(1, 1, 1);
        ResetPosition();

        isBack = false;
        isZoomed = false;
    }

    public void MoveToCenter()
    {
        OnBeginCenter.Invoke();
        isTweening = true;
        frontRectTrans.DOAnchorPos(new Vector3(screenCenter.x, 0, OrigLocalPos.z), 0.3f).OnComplete(Callback);
        backRectTrans.DOAnchorPos(new Vector3(screenCenter.x, 0, OrigLocalPos.z), 0.3f);

        // Flatten Card when centered
        if(isBack)
        {
            frontRectTrans.DOLocalRotate(Vector3.up * 180, 0.3f);
            backRectTrans.DOLocalRotate(Vector3.up * 0, 0.3f);
        }
        else
        {
            frontRectTrans.DOLocalRotate(Vector3.up * 0, 0.3f);
            backRectTrans.DOLocalRotate(Vector3.up * 180, 0.3f);
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (canvas == null) return;

        // Calculate the offset between the mouse position and the UI element's position
        // Both converted to the same coordinate space (canvas space)
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            data.position,
            data.pressEventCamera,
            out canvasPos);

        // Get the UI element's position in canvas space
        Vector2 elementCanvasPos = canvas.transform.InverseTransformPoint(frontRectTrans.position);

        // Calculate the offset
        dragOffset = elementCanvasPos - canvasPos;
    }

    public void OnDrag(PointerEventData data)
    {
        if (canvas == null) return;

        Vector2 canvasPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            data.position,
            data.pressEventCamera,
            out canvasPosition))
        {
            // Apply the offset to maintain the relative position from where we clicked
            Vector2 newPosition = canvasPosition + dragOffset;

            // Convert back to world position and apply to the RectTransform
            frontRectTrans.position = canvas.transform.TransformPoint(newPosition);
            backRectTrans.position = canvas.transform.TransformPoint(newPosition);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {

    }

    public void ResetPosition()
    {
        if (frontRectTrans != null)
        {
            isTweening = true;
            frontRectTrans.DOLocalMove(OrigLocalPos, 0.2f).OnComplete(Callback);
            backRectTrans.DOLocalMove(OrigLocalPos, 0.2f).OnComplete(Callback);
        }
    }

}
