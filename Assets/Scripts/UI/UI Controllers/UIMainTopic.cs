using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIMainTopic : UIController
{
    public TextMeshProUGUI SelectedTopicText;
    public GameObject CardFacePanel;
    public List<RectTransform> TopicButtons;
    public bool PlayAnimation = true;

    [Header("Video")]
    public string VideoUI;
    public VideoPanel VideoPanel = null;
    public RenderTexture VideoTexture = null;

    [Header("UI Elements")]
    public RectTransform Header;
    public RectTransform TopicContentPanel;
    public Image Background = null;
    public DataTopic DefaultTopic = null;
    public GameObject CardContent = null;
    public GameObject ButtonContent = null;

    private List<GameObject> Cards = new List<GameObject>();
    private Coroutine cardCoroutine;
    bool isVideoPlaying;

    public override void Initialize()
    {
        base.Initialize();
        SelectedTopicText.text = DefaultTopic.TopicName;
        InstaniateCards(DefaultTopic);
        if (PlayAnimation) StartCoroutine(TopicButtonsAnimation());
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

    //Clean up coroutines with sequences TODO
    IEnumerator TopicButtonsAnimation()
    {
        int index = 0;
        while (index < TopicButtons.Count)
        {
            Jump(TopicButtons[index], -20);
            yield return new WaitForSeconds(0.08f);
            index++;
        }
        //Fix Layout Group after animation
        ButtonContent.GetComponent<VerticalLayoutGroup>().enabled = true;
        LeftSequence();
        yield break;
    }

    public void LeftSequence()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Jump(Header, -50));
        sequence.Append(Jump(TopicContentPanel, -50));
    }

    IEnumerator CardsSequence()
    {
        int index = 0;
        while (index < Cards.Count)
        {
            Cards[index].GetComponent<Image>().DOFade(1, 0.4f);
            yield return new WaitForSeconds(0.08f);
            index++;
        }
        //Fix Layout Group after animation
        yield break;
    }

    public void PlayCardSequence()
    {
        if(cardCoroutine != null)StopCoroutine(cardCoroutine);
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        cardCoroutine = StartCoroutine(CardsSequence());
    }

    public Tween Jump(RectTransform rectTransform, float offset)
    {
        Vector2 position = rectTransform.anchoredPosition;
        rectTransform.gameObject.SetActive(true);
        return rectTransform.DOPunchAnchorPos(new Vector2(0, offset), 0.2f, 1, 1);
        //return rectTransform.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 0.3f).SetOptions(true);
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
            GameObject instance = Instantiate(topicData.Cards[i], CardContent.transform);
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
