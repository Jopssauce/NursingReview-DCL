using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public Canvas Canvas;
    public Vector2 ReferenceResolution = new Vector2(1920, 1080);

    protected PersistentSceneManager PersistentSceneManager;

    private void Awake()
    {
        DOTween.Init(DOTween.defaultAutoKill, DOTween.useSafeMode = false, DOTween.logBehaviour);
        CanvasScaler canvasScaler = Canvas.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = ReferenceResolution;
    }

    private void Start()
    {
        Initialize();
    }
    
    public virtual void Initialize()
    {
        Debug.Assert(Canvas != null, "No Canvas Detected!");
        PersistentSceneManager = PersistentSceneManager.instance;
    }

    public virtual void LoadScene(string sceneToLoad)
    {
        PersistentSceneManager.LoadSceneAdditive(sceneToLoad);
    }

    public virtual void LoadActiveScene(string sceneToLoad)
    {
        PersistentSceneManager.ReplaceActiveScene(sceneToLoad);
    }

    public virtual void QuitApp()
    {
        Application.Quit();
    }
}
