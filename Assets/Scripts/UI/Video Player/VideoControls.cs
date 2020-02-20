using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoControls : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Sprite Pause;
    public Sprite Play;

    Image button;

    private void Awake()
    {
        button = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayPause();
        }
    }

    public void PlayPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            button.sprite = Play;
        }
        else
        {
            videoPlayer.Play();
            button.sprite = Pause;
        }
    }
}
