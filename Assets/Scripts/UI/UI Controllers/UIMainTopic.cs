using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIMainTopic : UIController
{
    
    public TextMeshProUGUI SelectedTopicText;

    public List<RectTransform> TopicButtons;
    public bool PlayAnimation = true;

    [Header("Video")]
    public string VideoUI;
    public VideoPanel VideoPanel = null;
    public RenderTexture VideoTexture = null;

    [Header("Card UI Elements")]
    public GameObject CardPrefab;
    public GameObject CardFacePanel;
    public CardFace CardFace;
    
    [Header("UI Elements")]
    public RectTransform Header;
    public RectTransform TopicContentPanel;
    public Image Background = null;
    public DataTopic DefaultTopic = null;
    public GameObject CardContent = null;
    public GameObject ButtonContent = null;

    private List<GameObject> Cards = new List<GameObject>();
    Sequence cardSequence;
    bool isVideoPlaying;

    public override void Initialize()
    {
        base.Initialize();
        SelectedTopicText.text = DefaultTopic.TopicName;
        InstaniateCards(DefaultTopic);
        if (PlayAnimation) RightSequence();
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
            CardFacePanel.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Backspace) && isVideoPlaying == false)
        {
            PersistentSceneManager.ReplaceActiveScene("Topic UI");
        }
    }

    public void RightSequence()
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < TopicButtons.Count; i++)
        {
            sequence.Append(Jump(TopicButtons[i], -20));
            sequence.PrependInterval(0.07f);
        }
        sequence.onComplete += LeftSequence;
    }

    public void LeftSequence()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Jump(Header, -50));
        sequence.Append(Jump(TopicContentPanel, -50));
    }

    public void CardsSequence()
    {
        if (cardSequence != null) cardSequence.Kill();
        cardSequence = DOTween.Sequence();

        for (int i = 0; i < Cards.Count; i++)
        {
            cardSequence.Append(Cards[i].GetComponent<Image>().DOFade(1, 0.15f));
        }
    }

    public void PlayCardSequence()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
       CardsSequence();
    }

    public Tween Jump(RectTransform rectTransform, float offset, float time = 0.1f)
    {
        Vector2 position = rectTransform.anchoredPosition;
        
        Tween tween = rectTransform.DOPunchAnchorPos(new Vector2(0, offset), time, 1, 1);
        tween.onPlay += delegate()
        {
            rectTransform.gameObject.SetActive(true);
        };
        return tween;
    }

    public void SetSelectedTopicText(DataTopic topicData)
    {
        SelectedTopicText.text = topicData.TopicName;
        Background.sprite = topicData.Background;
        InstaniateCards(topicData);
    }

    private void InstaniateCards(DataTopic topicData)
    {
        ClearCards();
        for (int i = 0; i < topicData.Cards.Count; i++)
        {
            GameObject instance = Instantiate(CardPrefab, CardContent.transform);
            instance.GetComponent<ButtonCard>().CardData = topicData.Cards[i];
            instance.GetComponent<Image>().sprite = topicData.Cards[i].UISprite;
            Cards.Add(instance);
        }
        PlayCardSequence();
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
