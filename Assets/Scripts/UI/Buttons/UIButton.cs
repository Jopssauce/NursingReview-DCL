using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIButton : MonoBehaviour
{
    protected PersistentSceneManager PersistentSceneManager;
    protected Button button;
    protected Canvas Canvas;

    public virtual void Start()
    {
        PersistentSceneManager = PersistentSceneManager.instance;
        button = GetComponent<Button>();
        Canvas = GetComponentInParent<Canvas>();
    }

}
