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
        Initialize();
    }

    public virtual void Initialize()
    {
        PersistentSceneManager = PersistentSceneManager.instance;
        button = GetComponent<Button>();
        Canvas = GetComponentInParent<Canvas>();
    }

}
