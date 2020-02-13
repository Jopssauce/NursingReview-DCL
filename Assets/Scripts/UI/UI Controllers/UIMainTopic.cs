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
    private RenderTexture VideoTexture = null;
    [SerializeField]
    private DataTopic DefaultTopic = null;
    [SerializeField]
    private GameObject CardContent = null;

    private List<GameObject> Cards = new List<GameObject>();
    bool isVideoPlaying;

    private void Awake()
    {
        SelectedTopicText.text = DefaultTopic.TopicName;
        InstaniateCards(DefaultTopic);
    }

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

    public void SetSelectedTopicText(DataTopic topicData)
    {
        SelectedTopicText.text = topicData.TopicName;
        InstaniateCards(topicData);
    }

    private void InstaniateCards(DataTopic topicData)
    {
        ClearCards();
        for (int i = 0; i < topicData.Cards.Count; i++)
        {
            GameObject instance = Instantiate(topicData.Cards[i], CardContent.transform);
            Cards.Add(instance);
        }
    }

    private void ClearCards()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Destroy(Cards[i]);
        }
        Cards.Clear();
        Debug.Log(Cards.Count);
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
