using System.Collections;
using System.Collections.Generic;
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
    int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < CardFaces.Length; i++) 
        {
            CardFaces[i].CardData = SubTopics.Cards[i];
        }

        ToggleInteractables(true);
        ToggleAnimCards(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentIndex + 2 < SubTopics.Cards.Count)
            {
                ToggleInteractables(false);
                ToggleAnimCards(true);
                animator.Play("Next");
                Cards[2].sprite = SubTopics.Cards[currentIndex + 2].FrontFace;
            }
            else
            {
                // Play LastNext animation
            }
        }
    }

    public void OnNextFinished()
    {
        if(currentIndex > SubTopics.Cards.Count)
        {
            return;
        }
        currentIndex++;

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
