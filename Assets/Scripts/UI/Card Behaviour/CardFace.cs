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

    bool isBack = true;
    bool isDrag;
    bool isZoomed;

    Vector3 initialScale;

    //properties
    public float MaxTimeToClick = 0.60f;
    public float MinTimeToClick = 0.05f;
    public bool IsDebug = false;
    public bool isDoubleClicked;

    //private variables to keep track
    private float _minCurrentTime;
    private float _maxCurrentTime;

    Coroutine clickRoutine = null;

    public bool DoubleClick()
    {
        if (Time.time >= _minCurrentTime && Time.time <= _maxCurrentTime)
        {
            if (IsDebug)
                Debug.Log("Double Click");
            _minCurrentTime = 0;
            _maxCurrentTime = 0;
            isDoubleClicked = true;
            return true;
        }
        _minCurrentTime = Time.time + MinTimeToClick; _maxCurrentTime = Time.time + MaxTimeToClick;
        isDoubleClicked = false;
        return false;
    }

    IEnumerator DetectClick()
    {
        yield return new WaitForSeconds(0.2f);
        if (isDoubleClicked == false)
        {
            SwitchFaces();
        }
        else
        {
            if (isZoomed)
            {
                UnZoomFace(currentFace);
            }
            else
            {
                ZoomFace(currentFace);
            }
            StopCoroutine(clickRoutine);
            yield break;
        }
    }

    void OnEnable()
    {
        FrontFace.sprite = CardData.FrontFace;
        BackFace.sprite = CardData.BackFace;
        FrontFace.transform.localScale = Vector3.one;
        currentFace = FrontFace.gameObject;
        initialScale = currentFace.transform.localScale;
        //FrontFace.transform.position += new Vector3(0, 200, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //DoubleClick();
        //if (clickRoutine != null)
        //{
        //    StopCoroutine(clickRoutine);
        //    clickRoutine = null;
        //}
        if(isDrag == false)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (DoubleClick() == true)
                {
                    if (isZoomed)
                    {
                        UnZoomFace(currentFace);
                    }
                    else
                    {
                        ZoomFace(currentFace);
                    }
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                SwitchFaces();
            }
        }
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
        currentFace = faceA.gameObject;
        faceA.gameObject.SetActive(true);

        Tween tween = faceA.transform.DORotate(new Vector3(0, 0, 0), 0.3f);
        faceB.transform.DORotate(new Vector3(0, 180, 0), 0.3f);
        faceA.transform.localScale = faceB.transform.localScale;

        scrollRect.content = faceA.GetComponent<RectTransform>();

        if (tween.position % 2 == 0)
            faceB.gameObject.SetActive(false);

        isBack = !isBack;
    }

    public void ZoomFace(GameObject current)
    {
        isZoomed = true;
        current.transform.DOScale(2, 0.3f);
    }
    
    public void UnZoomFace(GameObject current)
    {
        isZoomed = false;
        current.transform.DOScale(1, 0.3f);
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
