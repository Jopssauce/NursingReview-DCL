using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainTopic : UIController
{
    public TextMeshProUGUI SelectedTopicText;
    public GameObject CardFacePanel;

    public string VideoUI;
    [SerializeField]
    private RenderTexture VideoTexture;

    bool isVideoPlaying;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isVideoPlaying == true)
        {
            UnLoadVideoPlayer();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isVideoPlaying == false)
        {
            CardFacePanel.SetActive(false);
        }
    }

    public void SetSelectedTopicText(string text)
    {
        SelectedTopicText.text = text;
    }

    public void LoadVideoPlayer()
    {
        VideoTexture.Release();
        isVideoPlaying = true;
        PersistentSceneManager.LoadActiveAdditive(VideoUI);
    }

    public void UnLoadVideoPlayer()
    {
        isVideoPlaying = false;
        PersistentSceneManager.UnloadScene(VideoUI);
    }
}
