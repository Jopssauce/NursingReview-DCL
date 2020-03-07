using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIButton : MonoBehaviour
{
    protected PersistentSceneManager PersistentSceneManager;
    public Button button { get; private set; }
    protected Canvas Canvas;

    public virtual void Awake()
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
