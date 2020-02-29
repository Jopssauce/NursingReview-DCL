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

    public List<GameObject> GridCards = new List<GameObject>();
    public List<GameObject> ScrollCards = new List<GameObject>();
    Tween BackgroundFadeTween;
    bool isVideoPlaying;

    public override void Initialize()
    {
        base.Initialize();
        SelectedTopicText.text = DefaultTopic.TopicName;
        InstantiateGridCards(DefaultTopic);
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
            CloseHorizontalCardScroller();
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

        InstantiateGridCards(topicData);
    }

    public void OpenHorizontalCardScroller(DataSubTopic subTopicData)
    {
        InstantiateScrollCards(subTopicData);
        CardHorizontalScrollRect.gameObject.SetActive(true);
        CardHorizontalScrollRect.horizontalNormalizedPosition = 1f;
        CardHorizontalScrollRect.DOHorizontalNormalizedPos(0, 0.5f);
    }

    public void CloseHorizontalCardScroller()
    {
        ClearGameobjects(ScrollCards);
        CardHorizontalScrollRect.gameObject.SetActive(false);
    }

    private void InstantiateGridCards(DataTopic topicData)
    {
        ClearGameobjects(GridCards);
        for (int i = 0; i < topicData.SubTopics.Count; i++)
        {
            DataSubTopic dataSubTopic = topicData.SubTopics[i];
            GameObject instance = Instantiate(CardPrefab, CardScrollGrid.content.transform);
            instance.GetComponent<ButtonCard>().SubTopicData = dataSubTopic;
            instance.GetComponent<Image>().sprite = dataSubTopic.UISprite;
            GridCards.Add(instance);
        }
        onInstancedSubTopics();
    }

    private void InstantiateScrollCards(DataSubTopic subTopicData)
    {
        ClearGameobjects(ScrollCards);
        for (int i = 0; i < subTopicData.Cards.Count; i++)
        {
            GameObject instance = Instantiate(PrefabCardFace, CardHorizontalScrollRect.content.transform);
            CardFace cardFace = PrefabCardFace.GetComponent<CardFace>();
            cardFace.CardData = subTopicData.Cards[i];
            ScrollCards.Add(instance);
        }
    }

    private void ClearGameobjects(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
        }
        list.Clear();
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
