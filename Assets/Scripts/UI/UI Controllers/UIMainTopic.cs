using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainTopic : UIController
{
    public TextMeshProUGUI SelectedTopicText;
    public GameObject CardFacePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CardFacePanel.SetActive(false);
        }
    }

    public void SetSelectedTopicText(string text)
    {
        SelectedTopicText.text = text;
    }
}
