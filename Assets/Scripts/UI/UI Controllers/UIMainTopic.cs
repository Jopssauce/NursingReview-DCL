using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public delegate void OnInitialize();
public delegate void OnInstancedSubTopics();


public class UIMainTopic : UIController
{
    
    public TextMeshProUGUI SelectedTopicText;

    public List<RectTransform> TopicButtons;
    public ButtonMainTopic currentTopicButton;
    public bool PlayAnimation = true;

    [Header("Video")]
    public string VideoUI;
    public VideoPanel VideoPanel = null;
    public RenderTexture VideoTexture = null;

    [Header("Card UI Elements")]
    public GameObject CardPrefab;
    public ScrollRect CardScrollGrid = null;
    public ScrollRect CardHorizontalScrollRect;
    public GameObject PrefabCardFace;

    [Header("UI Elements")]
    public RectTransform Header;
    public RectTransform TopicContentPanel;
    public Image Background = null;
    public DataTopic DefaultTopic = null;
    public GameObject ButtonContent = null;

    public event OnInitialize onInitialize;
    public event OnInstancedSubTopics onInstancedSubTopics;

    public List<GameObject> Cards = new List<GameObject>();
    Tween BackgroundFadeTween;
    bool isVideoPlaying;

    public override void Initialize()
    {
        base.Initialize();
        SelectedTopicText.text = DefaultTopic.TopicName;
        InstantiateSubTopics(DefaultTopic);
        Canvas.worldCamera = Camera.main;
        onInitialize();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isVideoPlaying == true)
        {
            UnLoadVideoPlayer();
            VideoPanel.UnZoom();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isVideoPlaying == false)
        {
            CardHorizontalScrollRect.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Backspace) && isVideoPlaying == false)
        {
            PersistentSceneManager.ReplaceActiveScene("Topic UI");
        }
    }

    public void SetSelectedTopicText(DataTopic topicData)
    {
        SelectedTopicText.text = topicData.TopicName;
        Background.sprite = topicData.Background;

        if (BackgroundFadeTween != null) BackgroundFadeTween.Kill();
        Background.color = new Color(1,1,1,0);
        BackgroundFadeTween = Background.DOFade(1, 0.5f);

        InstantiateSubTopics(topicData);
    }

    public void OpenHorizontalCardScroller(DataSubTopic subTopicData)
    {
        InstantiateCards(subTopicData);
        CardHorizontalScrollRect.gameObject.SetActive(true);
    }

    private void InstantiateSubTopics(DataTopic topicData)
    {
        ClearCards();
        for (int i = 0; i < topicData.SubTopics.Count; i++)
        {
            DataSubTopic dataSubTopic = topicData.SubTopics[i];
            GameObject instance = Instantiate(CardPrefab, CardScrollGrid.content.transform);
            instance.GetComponent<ButtonCard>().SubTopicData = dataSubTopic;
            instance.GetComponent<Image>().sprite = dataSubTopic.UISprite;
            Cards.Add(instance);
        }
        onInstancedSubTopics();
    }

    private void InstantiateCards(DataSubTopic subTopicData)
    {
        for (int i = 0; i < subTopicData.Cards.Count; i++)
        {
            GameObject instance = Instantiate(PrefabCardFace, CardHorizontalScrollRect.content.transform);
            CardFace cardFace = PrefabCardFace.GetComponent<CardFace>();
            cardFace.CardData = subTopicData.Cards[i];
        }
    }

    private void ClearCards()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Destroy(Cards[i]);
        }
        Cards.Clear();
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
