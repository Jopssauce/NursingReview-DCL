using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sub Topic", menuName = "Sub Topic Data")]
public class DataSubTopic : ScriptableObject
{
    public Sprite Background;
    public Sprite Mask;
    public string Name;
    public List<DataCard> Cards;
}
