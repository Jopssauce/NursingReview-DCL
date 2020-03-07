using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading.Tasks;
using TMPro;
using DG.Tweening;

public delegate void OnInitialize();
public delegate void OnInstancedSubTopics();
public delegate void OnInstancedTopics();
public delegate void OnLoadVideoPlayer();
public delegate void OnUnloadVideoPlayer();


public class UIMainTopic : UIController
{
    public UIButtonSelectable currentTopicButton;
    public UIButtonSelectable currentTab;
    public DataTopic DefaultTopic; 

    [Header("Animation Settings")]
    public bool PlayStartAnimation = true;

    [Header("Video")]
    public string VideoUI;
    public RenderTexture VideoTexture;

    [Header("Prefabs")]
    public GameObject MainTopicButton;
    public GameObject PrefabCard;
    public GameObject PrefabScrollCard;

    [Header("General Elements")]
    public GameObject RaycastBlocker;

    [Header("Groups")]
    public UITopicButtonScrollView UITopicButtonScrollView;
    public UIContentGroup UIContentGroup;
    public UIBackgroundGroup UIBackgroundGroup;
    public UINavigationGroup UINavigationGroup;
    public UICardsViewerGroup UICardsViewerGroup;

    public event OnInitialize onInitialize;
    public event OnInstancedSubTopics onInstancedSubTopics;
    public event OnInstancedTopics onInstancedTopics;
    public event OnLoadVideoPlayer onLoadVideoPlayer;
    public event OnUnloadVideoPlayer onUnloadVideoPlayer;

    [Header("Lists")]
    public List<DataTopic> Topics;
    public List<RectTransform> TopicButtons;
    public List<GameObject> GridCards = new List<GameObject>();
    public List<GameObject> ScrollCards = new List<GameObject>();

    Tween BackgroundFadeTween;
    bool isVideoPlaying;

    public override void Initialize()
    {
        base.Initialize();
        UIContentGroup.HeaderTextMeshProUGUI.text = DefaultTopic.TopicName;
        InstantiateGridCards(DefaultTopic);
        Canvas.worldCamera = Camera.main;
        InstantiateTopics();
        InitializeTabButtons();

        onInitialize?.Invoke();
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

    void InitializeTabButtons()
    {
        SelectButton(UIContentGroup.VideoButton.GetComponent<UIButtonSelectable>(), ref currentTab);
        UIContentGroup.CardButton.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            SelectButton(UIContentGroup.CardButton.GetComponent<UIButtonSelectable>(), ref currentTab);
        });

        UIContentGroup.VideoButton.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            SelectButton(UIContentGroup.VideoButton.GetComponent<UIButtonSelectable>(), ref currentTab);
        });
    }

    public void SelectButton(UIButtonSelectable selectedButton, ref UIButtonSelectable currentSelectedButton)
    {
        if (selectedButton != currentSelectedButton)
        {
            //Deselect Old Button
            if (currentSelectedButton != null) currentSelectedButton.DeselectAction();
            //Select this button as new
            selectedButton.SelectAction();
            currentSelectedButton = selectedButton;
        }
    }

    public void SetSelectedTopicText(DataTopic topicData)
    {
        UIContentGroup.HeaderTextMeshProUGUI.text = topicData.TopicName;
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

    private void InstantiateTopics()
    {
        for (int i = 0; i < Topics.Count; i++)
        {
            UIButtonSelectable button;
            GameObject instance = Instantiate(MainTopicButton, UITopicButtonScrollView.ScrollRect.content.transform);
            TopicButtons.Add(instance.GetComponent<RectTransform>());
            button = instance.GetComponent<UIButtonSelectable>();
            button.TopicData = Topics[i];
            button.TextMeshProUGUI.text = Topics[i].TopicName;

            //TODO REFACTOR THIS DIRTY DELEGATE
            button.button.onClick.AddListener(delegate ()
            {
                SelectButton(button, ref currentTopicButton);
                SetSelectedTopicText(button.TopicData);
            });

            instance.GetComponent<CanvasGroup>().alpha = 0;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(UITopicButtonScrollView.ScrollRect.content);
        UITopicButtonScrollView.ScrollRect
            .content.GetComponent<VerticalLayoutGroup>().enabled = false;

        onInstancedTopics?.Invoke();
    }

    private void InstantiateGridCards(DataTopic topicData)
    {
        ClearGameobjects(GridCards);
        for (int i = 0; i < topicData.SubTopics.Count; i++)
        {
            DataSubTopic dataSubTopic = topicData.SubTopics[i];
            GameObject instance = Instantiate(PrefabCard, UIContentGroup.CardGridView.content.transform);
            ButtonCard buttonCard = instance.GetComponent<ButtonCard>();
            buttonCard.SubTopicData = dataSubTopic;
            buttonCard.UISubTopicMask.TextMeshProUGUI.text = dataSubTopic.Name;
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
            scrollCard.image.sprite = subTopicData.Cards[i].FrontFace;
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
        onLoadVideoPlayer();
    }

    public void UnLoadVideoPlayer()
    {
        isVideoPlaying = false;
        PersistentSceneManager.UnloadScene(VideoUI);
        onUnloadVideoPlayer();
    }
}
