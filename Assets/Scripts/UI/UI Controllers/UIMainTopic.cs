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
    public RenderTexture VideoTexture = null;

    [Header("Card UI Elements")]
    public GameObject CardPrefab;
    public GameObject PrefabScrollCard;

    [Header("UI Elements")]
    public GameObject RaycastBlocker;
    public DataTopic DefaultTopic = null;

    [Header("Groups")]
    public UITopicButtonScrollView UITopicButtonScrollView;
    public UIContentGroup UIContentGroup;
    public UIBackgroundGroup UIBackgroundGroup;
    public UINavigationGroup UINavigationGroup;
    public UICardsViewerGroup UICardsViewerGroup;

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
            UIContentGroup.VideoPanel.GetComponent<VideoPanel>().UnZoom();
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
        UIBackgroundGroup.Background.sprite = topicData.Background;

        if (BackgroundFadeTween != null) BackgroundFadeTween.Kill();
        UIBackgroundGroup.Background.color = new Color(1,1,1,0);
        BackgroundFadeTween = UIBackgroundGroup.Background.DOFade(1, 0.5f);

        InstantiateGridCards(topicData);
    }

    public void OpenHorizontalCardScroller(DataSubTopic subTopicData)
    {
        InstantiateScrollCards(subTopicData);
        UICardsViewerGroup.ScrollRect.gameObject.SetActive(true);
        UICardsViewerGroup.ScrollRect.horizontalNormalizedPosition = 1f;
        UICardsViewerGroup.ScrollRect.DOHorizontalNormalizedPos(0, 0.5f);
    }

    public void CloseHorizontalCardScroller()
    {
        ClearGameobjects(ScrollCards);
        UICardsViewerGroup.ScrollRect.gameObject.SetActive(false);
    }

    public void ActivateCardFace(DataCard CardData)
    {
        UICardsViewerGroup.CardFace.CardData = CardData;
        UICardsViewerGroup.CardFace.gameObject.SetActive(true);
    }

    private void InstantiateGridCards(DataTopic topicData)
    {
        ClearGameobjects(GridCards);
        for (int i = 0; i < topicData.SubTopics.Count; i++)
        {
            DataSubTopic dataSubTopic = topicData.SubTopics[i];
            GameObject instance = Instantiate(CardPrefab, UIContentGroup.CardGridView.content.transform);
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
            GameObject instance = Instantiate(PrefabScrollCard, UICardsViewerGroup.ScrollRect.content.transform);
            UIScrollCard scrollCard = instance.GetComponent<UIScrollCard>();
            scrollCard.CardData = subTopicData.Cards[i];
            scrollCard.uiMainTopic = this;
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
