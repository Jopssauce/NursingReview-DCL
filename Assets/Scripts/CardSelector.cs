using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    // Animation Cards
    public Image[] Cards;
    // Interactable Cards
    public CardFace[] CardFaces;
    public Animator animator;
    public DataSubTopic SubTopics;
    public Image DarkBG;
    public Image BG;
    int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(SubTopics.Background != null)
        {
            BG.sprite = SubTopics.Background;
        }

        for (int i = 0; i < CardFaces.Length; i++) 
        {
            CardFaces[i].CardData = SubTopics.Cards[i];
            CardFaces[i].InitCard();
            Cards[i].sprite = SubTopics.Cards[i].FrontFace;
        }

        ToggleInteractables(true);
        ToggleAnimCards(false);

        CardFaces[0].OnBeginCenter += () => { DarkBG.gameObject.SetActive(true); };
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && !CardFaces[0].isTweening)
        {
            CardFaces[0].ResetCard();
            DarkBG.gameObject.SetActive(false);
            if (CardFaces[0].isTweening) return;
            // Ensure sprite is backface to ensure smooth animation after resetting. This is for when next is played when a card is on it's bac
            if (CardFaces[0].isBack)
            {
                Cards[0].sprite = SubTopics.Cards[currentIndex].BackFace;
            }
            ToggleInteractables(false);
            ToggleAnimCards(true);       
            animator.Play("Next");
            

            if (currentIndex + 2 < SubTopics.Cards.Count)
                Cards[2].sprite = SubTopics.Cards[currentIndex + 2].FrontFace;
            else
                Cards[2].color = Color.clear;
        
        }

    }

    public void OnNextFinished()
    {
        currentIndex++;
        // Last Card
        if (currentIndex >= SubTopics.Cards.Count - 1)
        {
            currentIndex = SubTopics.Cards.Count - 1;
            Cards[0].sprite = SubTopics.Cards[currentIndex].FrontFace;
            CardFaces[0].CardData = SubTopics.Cards[currentIndex];
            ToggleInteractables(true);
            ToggleAnimCards(false);
            return;
        }
        

        Cards[0].sprite = SubTopics.Cards[currentIndex].FrontFace;
        Cards[1].sprite = SubTopics.Cards[currentIndex+1].FrontFace;

        CardFaces[0].CardData = SubTopics.Cards[currentIndex];
        CardFaces[1].CardData = SubTopics.Cards[currentIndex + 1];

        ToggleInteractables(true);
        ToggleAnimCards(false);
    }

    void ToggleInteractables(bool val)
    {
        for (int i = 0; i < CardFaces.Length; i++)
        {
            CardFaces[i].gameObject.SetActive(val);
        }
    }

    void ToggleAnimCards(bool val)
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i].gameObject.SetActive(val);
        }
    }
}
