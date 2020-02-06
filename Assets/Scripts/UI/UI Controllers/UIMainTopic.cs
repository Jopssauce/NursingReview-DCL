using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainTopic : UIController
{
    public TextMeshProUGUI SelectedTopicText;

    public void SetSelectedTopicText(string text)
    {
        SelectedTopicText.text = text;
    }
}
