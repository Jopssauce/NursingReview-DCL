using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRes : MonoBehaviour
{
    public int x = 1920;
    public int y = 1080;
    
    public void Set()
    {
        Screen.SetResolution(x, y, FullScreenMode.Windowed);
    }    
}
