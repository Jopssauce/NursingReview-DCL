using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Card", menuName = "Card Data")]
public class DataCard : ScriptableObject
{
    public string Name;
    public Sprite UISprite;
    public Sprite FrontFace;
    public Sprite BackFace;
    public bool IsFrontHorizontal;
    public bool IsBackHorizontal;
}
