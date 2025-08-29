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
    public Vector3 OrigAnchorPos { get; private set; }
    private Vector3 faceAOriginalRotation;
    private Vector3 faceBOriginalRotation;
    private Vector3 originalScale;
    private Vector2 originalSizeDelta;
    private Vector2 dragOffset;
    private Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

    GameObject currentFace;
    GameObject lastFace;

    public bool isBack { get; private set; }
    public bool isTweening { get; private set; }
    private bool isDrag;
    private bool isZoomed;
    private bool isHorizontal;
    private bool isCenterMode;

    //properties
    public float MaxTimeToClick = 0.60f;
    public float MinTimeToClick = 0.05f;
    public bool IsDebug = false;
    public bool isDoubleClicked;
    public bool isInteractable = false;
    // Float Offset for the size when we send card to the center
    public float centerSizeDeltaOffset = 10f;
    // Anchored Position for horizontal Card
    public Vector2 horizontalCardPosition;
    public float horizontalCardHeightOffset = 100f;

    //Events
    public Action OnBeginCenter;

    //private variables to keep track
    public float doubleClickThreshold = 0.3f; // Time in seconds

    void Awake()
    {
        frontRectTrans = FrontFace.GetComponent<RectTransform>();
        backRectTrans = BackFace.GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();
        OrigLocalPos = frontRectTrans.localPosition;
        OrigAnchorPos = frontRectTrans.anchoredPosition;
        faceAOriginalRotation = frontRectTrans.localEulerAngles;
        faceBOriginalRotation = backRectTrans.localEulerAngles;
        originalScale = frontRectTrans.localScale;
        originalSizeDelta = frontRectTrans.sizeDelta;
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
    }

    public void InitCard()
    {
        FrontFace.sprite = CardData.FrontFace;
        BackFace.sprite = CardData.BackFace;
        if (CardData.BackFace == null)
        {
            BackFace.GetComponent<Image>().enabled = false;
        }
        else
        {
            BackFace.GetComponent<Image>().enabled = true;
        }
        FrontFace.transform.localScale = Vector3.one;
        currentFace = FrontFace.gameObject;
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

        if (Input.GetKeyDown(KeyCode.S) && !isTweening)
        {
            SwitchFaces();
        }

        if (Input.GetKeyDown(KeyCode.C) && !isTweening)
        {
            MoveToCenter();
        }

        if(Input.GetKey(KeyCode.H) && !isTweening)
        {
            MoveToHorizontalCardPosition();
        }    
    }

    private void SwitchFaces()
    {
        if (BackFace.sprite == null) return;

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
        RectTransform faceARect = faceA.GetComponent<RectTransform>();
        RectTransform faceBRect = faceB.GetComponent<RectTransform>();

        faceA.transform.DORotate(new Vector3(0, faceARect.localEulerAngles.y - 180, faceARect.localEulerAngles.z), duration).OnComplete(Callback);
        faceB.transform.DORotate(new Vector3(0, faceBRect.localEulerAngles.y - 180, faceBRect.localEulerAngles.z), duration);

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

        // Rotation
        FrontFace.transform.localEulerAngles = faceAOriginalRotation;
        BackFace.transform.localEulerAngles = faceBOriginalRotation;
        //Scale
        FrontFace.transform.localScale = new Vector3(1, 1, 1);
        BackFace.transform.localScale = new Vector3(1, 1, 1);
        // Aspect Ratio
        FrontFace.preserveAspect = false;
        BackFace.preserveAspect = false;
        // Position
        frontRectTrans.sizeDelta = originalSizeDelta;
        backRectTrans.sizeDelta = originalSizeDelta;
        ResetPosition();

        isBack = false;
        isZoomed = false;
        isHorizontal = false;
        isCenterMode = false;
    }

    public void MoveToCenter()
    {
        OnBeginCenter.Invoke();
        isTweening = true;
        isCenterMode = true;

        frontRectTrans.DOAnchorPos(new Vector3(screenCenter.x, 0, OrigLocalPos.z), 0.3f).OnComplete(Callback);
        backRectTrans.DOAnchorPos(new Vector3(screenCenter.x, 0, OrigLocalPos.z), 0.3f);

        Vector3 frontEuler = frontRectTrans.localEulerAngles;
        Vector3 backEuler = backRectTrans.localEulerAngles;

        // Flatten Card when centered
        if (isBack)
        {           
            frontRectTrans.DOLocalRotate(new Vector3(frontEuler.x, 180, frontEuler.z), 0.3f);
            backRectTrans.DOLocalRotate(new Vector3(backEuler.x, 0, backEuler.z), 0.3f);
        }
        else
        {
            frontRectTrans.DOLocalRotate(new Vector3(frontEuler.x, 0, frontEuler.z), 0.3f);
            backRectTrans.DOLocalRotate(new Vector3(backEuler.x, 180, backEuler.z), 0.3f);
        }

        float aspectRatio = FrontFace.sprite.textureRect.width / FrontFace.sprite.textureRect.height;
        float newWidth = (originalSizeDelta.x - (originalSizeDelta.x * aspectRatio)) + originalSizeDelta.x;
        // Horizontal Cards look better without preserve aspect on
        if (!isHorizontal)
        {
            FrontFace.preserveAspect = true;
            BackFace.preserveAspect = true;
        }
        FrontFace.rectTransform.sizeDelta = new Vector2(newWidth + centerSizeDeltaOffset, FrontFace.sprite.textureRect.height);
        BackFace.rectTransform.sizeDelta = new Vector2(newWidth + centerSizeDeltaOffset, FrontFace.sprite.textureRect.height);
    }

    public void MoveToHorizontalCardPosition()
    {
        if (isCenterMode) return;
        if (!isHorizontal)
        {
            MoveToHorizontal();
        }
        else
        {
            MoveToVertical();
        }
    }

    private void MoveToHorizontal()
    {
        isTweening = true;
        isHorizontal = true;
        frontRectTrans.DOLocalRotate(new Vector3(0, frontRectTrans.eulerAngles.y, 90), 0.3f).OnComplete(Callback);
        backRectTrans.DOLocalRotate(new Vector3(0, backRectTrans.eulerAngles.y, 90), 0.3f);
        
        frontRectTrans.DOAnchorPos(horizontalCardPosition, 0.3f);
        backRectTrans.DOAnchorPos(horizontalCardPosition, 0.3f);

        FrontFace.rectTransform.DOSizeDelta(new Vector2(frontRectTrans.sizeDelta.x, originalSizeDelta.y - horizontalCardHeightOffset), 0.3f);
        BackFace.rectTransform.DOSizeDelta(new Vector2(backRectTrans.sizeDelta.x, originalSizeDelta.y - horizontalCardHeightOffset), 0.3f);
    }

    private void MoveToVertical()
    {
        isTweening = true;
        isHorizontal = false;
        frontRectTrans.DOLocalRotate(new Vector3(0, frontRectTrans.eulerAngles.y, 0), 0.3f).OnComplete(Callback);
        backRectTrans.DOLocalRotate(new Vector3(0, backRectTrans.eulerAngles.y, 0), 0.3f);

        frontRectTrans.DOAnchorPos(OrigAnchorPos, 0.3f);
        backRectTrans.DOAnchorPos(OrigAnchorPos, 0.3f);

        FrontFace.rectTransform.DOSizeDelta(new Vector2(frontRectTrans.sizeDelta.x, originalSizeDelta.y),0.3f);
        BackFace.rectTransform.DOSizeDelta(new Vector2(backRectTrans.sizeDelta.x, originalSizeDelta.y), 0.3f);
    }

    /* This is the code in case they want to rotate horizontally during center
    private void MoveToHorizontal()
    {
        isTweening = true;
        isHorizontal = true;
        frontRectTrans.DOLocalRotate(new Vector3(0, frontRectTrans.eulerAngles.y, 90), 0.3f).OnComplete(Callback);
        backRectTrans.DOLocalRotate(new Vector3(0, backRectTrans.eulerAngles.y, 90), 0.3f);

        if (!isCenterMode)
        {
            frontRectTrans.DOAnchorPos(horizontalCardPosition, 0.3f);
            backRectTrans.DOAnchorPos(horizontalCardPosition, 0.3f);
            FrontFace.rectTransform.DOSizeDelta(new Vector2(frontRectTrans.sizeDelta.x, originalSizeDelta.y - horizontalCardHeightOffset), 0.3f);
            BackFace.rectTransform.DOSizeDelta(new Vector2(backRectTrans.sizeDelta.x, originalSizeDelta.y - horizontalCardHeightOffset), 0.3f);
        }
        else
        {
            FrontFace.preserveAspect = false;
            BackFace.preserveAspect = false;
        }


    }

    private void MoveToVertical()
    {
        isTweening = true;
        isHorizontal = false;
        frontRectTrans.DOLocalRotate(new Vector3(0, frontRectTrans.eulerAngles.y, 0), 0.3f).OnComplete(Callback);
        backRectTrans.DOLocalRotate(new Vector3(0, backRectTrans.eulerAngles.y, 0), 0.3f);

        if (!isCenterMode)
        {
            frontRectTrans.DOAnchorPos(OrigAnchorPos, 0.3f);
            backRectTrans.DOAnchorPos(OrigAnchorPos, 0.3f);
            FrontFace.rectTransform.DOSizeDelta(new Vector2(frontRectTrans.sizeDelta.x, originalSizeDelta.y), 0.3f);
            BackFace.rectTransform.DOSizeDelta(new Vector2(backRectTrans.sizeDelta.x, originalSizeDelta.y), 0.3f);
        }
        else
        {
            FrontFace.preserveAspect = true;
            BackFace.preserveAspect = true;
        }
    }*/

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
