using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Topic", menuName = "Topic Data")]
public class DataTopic : ScriptableObject
{
    public string TopicName;
    public string VideoUrl;
    public Sprite Background;
    public List<DataSubTopic> SubTopics;
}
