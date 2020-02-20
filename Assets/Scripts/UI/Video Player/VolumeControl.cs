using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image), typeof(RectTransform))]
public class VolumeControl : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public VideoPlayer videoPlayer;

    private Image playbackProgress;
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        playbackProgress = GetComponent<Image>();

        if (playbackProgress.sprite == null)
        {
            var texture = Texture2D.whiteTexture;
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 100);
            playbackProgress.sprite = sprite;
            playbackProgress.fillAmount = 1;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Seek(Input.mousePosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Seek(Input.mousePosition);
    }

    private void Seek(Vector2 cursorPosition)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, cursorPosition, null, out var localPoint))
            return;

        var rect = rectTransform.rect;
        var progress = (localPoint.x - rect.x) / rect.width;

        playbackProgress.fillAmount = progress;
        videoPlayer.SetDirectAudioVolume(0, playbackProgress.fillAmount);
        
    }

}
