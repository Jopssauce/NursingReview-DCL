using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIContentGroup : UIGroup
{
    [Header("Header")]
    public GameObject Header;
    public TextMeshProUGUI HeaderTextMeshProUGUI;

    [Header("Tab Buttons")]
    public GameObject CardButton;
    public GameObject VideoButton;

    [Header("Video UI Elements")]
    public GameObject VideoPanel;
    public GameObject VideoFrame;

    [Header("Card UI Elements")]
    public ScrollRect CardGridView;
}
